﻿Imports System.Security.Cryptography
Imports System.Security
Imports MySql.Data.MySqlClient
Imports Telerik.WinControls
Imports System.IO

Public Class Actions
    Shared entropy As Byte() = System.Text.Encoding.Unicode.GetBytes("woprjepi0-32i-w0dowop3k2c90m4cr429j5mv430kr320-rm-32rm32-9ricm329m0329mc39mejfm209jmr09jmrxcij320cj")

    Public Shared Function EncryptString(input As System.Security.SecureString) As String
        Dim encryptedData As Byte() = Cryptography.ProtectedData.Protect(System.Text.Encoding.Unicode.GetBytes(ToInsecureString(input)), entropy, System.Security.Cryptography.DataProtectionScope.CurrentUser)
        Return Convert.ToBase64String(encryptedData)
    End Function

    Public Shared Function DecryptString(encryptedData As String) As SecureString
        Try
            Dim decryptedData As Byte() = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), entropy, System.Security.Cryptography.DataProtectionScope.CurrentUser)
            Return ToSecureString(System.Text.Encoding.Unicode.GetString(decryptedData))
        Catch
            Return New SecureString()
        End Try
    End Function

    Public Shared Function ToSecureString(input As String) As SecureString
        Dim secure As New SecureString()
        For Each c As Char In input
            secure.AppendChar(c)
        Next
        secure.MakeReadOnly()
        Return secure
    End Function

    Public Shared Function ToInsecureString(input As SecureString) As String
        Dim returnValue As String = String.Empty
        Dim ptr As IntPtr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input)
        Try
            returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr)
        Finally
            System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr)
        End Try
        Return returnValue
    End Function

    Public Shared Sub SaveDB()

        Dim savedb_dialog As New SaveFileDialog()
        savedb_dialog.Filter = "mySQL Database|*.sql"
        savedb_dialog.Title = "Choose a Location to Save"
        Dim mysql_SAVE As New MySqlBackup(comm)
        mysql_SAVE.ExportInfo.AddCreateDatabase = True
        mysql_SAVE.ExportInfo.EnableEncryption = True
        mysql_SAVE.ExportInfo.EncryptionPassword = "9Wy3Z3xTApDKUtPVN+TegRLTGR2mj8_M3*3ZJwSts83g9+pL?ZLEn?3xnuMR!2g"
        If savedb_dialog.ShowDialog() = DialogResult.OK Then
            Try
                MySQLConn.ConnectionString = connstring
                Dim mysql_LOAD As New MySqlBackup(comm)
                mysql_LOAD.Command.Connection = MySQLConn
                MySQLConn.Open()
                mysql_SAVE.ExportToFile(savedb_dialog.FileName.ToString)
                MySQLConn.Close()
                'RadMessageBox.Show("Database Successfully Exported.", "TLTD Scheduling System", MessageBoxButtons.OK, RadMessageIcon.Info)
                Wu_RadMessageBox(2,"Database Successfully Exported.")
                
                'RadMessageBox.Show("Error in Importing Database:" & Environment.NewLine & ex.Message, "TLTD Scheduling System", MessageBoxButtons.OK, RadMessageIcon.Error)
             Catch ex As MySqlException
             If (ex.Number = 0 And (ex.Message.Contains("Unable to connect to any of the specified MySQL hosts") or ex.Message.Contains("Reading from the stream has failed"))) Or (ex.Number = 1042 And (ex.Message.Contains("Unable to connect to any of the specified MySQL hosts") or ex.Message.Contains("Reading from the stream has failed")))
                Wu_RadMessageBox(4," The database probably went offline.")
                Login.log_lbl_dbstatus.Text = "Offline"
                Login.log_lbl_dbstatus.ForeColor = Color.Red
                Return
            Else
            Wu_RadMessageBox(4,ex.Message)
            End If
        Catch ex As Exception
            Wu_RadMessageBox(4,ex.Message)
            End Try
        End If
    End Sub

    Public Shared Sub Wu_RadMessageBox(Icontype As Integer, messageContent As String)

        Dim form As RadMessageBoxForm = New RadMessageBoxForm()

        form.DialogResult = DialogResult.OK
        'form.RightToLeft = RightToLeft.No
        form.Text="CEU TLTD Reservation System"
        form.MessageText = messageContent
        form.StartPosition = FormStartPosition.CenterScreen

        Select Case Icontype
            Case 1
                My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)
                form.MessageIcon = GetRadMessageIcon(RadMessageIcon.Question)
            Case 2
                My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Beep)
                form.MessageIcon = GetRadMessageIcon(RadMessageIcon.Info)
            Case 3
                 My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
                form.MessageIcon = GetRadMessageIcon(RadMessageIcon.Exclamation)
            Case 4
                 My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Hand)
                form.MessageIcon = GetRadMessageIcon(RadMessageIcon.Error)
        End Select
        form.ButtonsConfiguration = MessageBoxButtons.OK
        form.DefaultButton = MessageBoxDefaultButton.Button1

        form.TopMost = True

        form.ShowDialog()
    End Sub


    Private Shared Function GetRadMessageIcon(icon As RadMessageIcon) As Bitmap
        Dim stream As Stream
        Dim image As Bitmap

        Select Case icon

            Case RadMessageIcon.Info
                stream = (System.Reflection.Assembly.GetAssembly(GetType(RadMessageBox)).GetManifestResourceStream("Telerik.WinControls.UI.Resources.RadMessageBox.MessageInfo.png"))
                image = TryCast(Bitmap.FromStream(stream), Bitmap)
                stream.Close()
                Return image
            Case RadMessageIcon.Question
                stream = (System.Reflection.Assembly.GetAssembly(GetType(RadMessageBox)).GetManifestResourceStream("Telerik.WinControls.UI.Resources.RadMessageBox.MessageQuestion.png"))
                image = TryCast(Bitmap.FromStream(stream), Bitmap)
                stream.Close()
                Return image
            Case RadMessageIcon.Exclamation
                stream = (System.Reflection.Assembly.GetAssembly(GetType(RadMessageBox)).GetManifestResourceStream("Telerik.WinControls.UI.Resources.RadMessageBox.MessageExclamation.png"))
                image = TryCast(Bitmap.FromStream(stream), Bitmap)
                stream.Close()
                Return image
            Case RadMessageIcon.Error
                stream = (System.Reflection.Assembly.GetAssembly(GetType(RadMessageBox)).GetManifestResourceStream("Telerik.WinControls.UI.Resources.RadMessageBox.MessageError.png"))
                image = TryCast(Bitmap.FromStream(stream), Bitmap)
                stream.Close()
                Return image
        End Select
        Return Nothing
    End Function
End Class