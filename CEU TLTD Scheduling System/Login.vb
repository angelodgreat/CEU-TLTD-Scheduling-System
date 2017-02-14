﻿Imports MySql.Data.MySqlClient
Imports Telerik.WinControls

Public Class Login
    Dim mysqlconn As MySqlConnection
    Dim Command As MySqlCommand
    Public db_is_deadCount As Integer
    Dim a As Boolean = false

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Timer
        log_timer.Enabled = True
        CheckDBStatus()
        ThemeResolutionService.ApplicationThemeName = My.Settings.WindowTheme
        log_username.Select()
    End Sub


    'Login Button Codes
    Private Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click
        Try
            If String.IsNullOrEmpty(log_username.Text) Or String.IsNullOrEmpty(log_password.Text) Then
                RadMessageBox.Show(Me, "Please enter username and password.", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1)

            Else
                If log_username.Text.Contains("@ceu.edu.ph") Then
                Else log_username.Text = log_username.Text + "@ceu.edu.ph"
                End If
                If a = False Then
                DB_DeadCounter()
                Else
                    Dim looper As Integer
                    If MysqlConn.State = ConnectionState.Open Then
                        MysqlConn.Close()
                    End If
                    mysqlconn.Open()
                    Dim q2 As String = "SELECT * FROM staff_reg WHERE staff_username=@proc_email_login and staff_password=sha2(@proc_password_login, 512)"
                    comm = New MySqlCommand(q2, mysqlconn)
                    comm.Parameters.AddWithValue("@proc_email_login", log_username.Text)
                    comm.Parameters.AddWithValue("@proc_password_login", log_password.Text)
                    Dim accountstate As Boolean
                    reader = comm.ExecuteReader
                    While reader.Read
                        looper += 1
                        username = reader.GetString("staff_username")
                        activeuserfname = reader.GetString("staff_fname")
                        activeuserlname = reader.GetString("staff_surname")
                        If reader.GetString("staff_isactive").Equals("0") Then
                            accountstate=False
                        ElseIf reader.GetString("staff_isactive").Equals("1") Then
                            accountstate=True
                        End If
                        Main.lbl_nameofstaff_reserved.Text = activeuserlname + ", " + activeuserfname
                        Main.rel_nameofstaff_release.Text = activeuserlname + ", " + activeuserfname
                        Main.ret_nameofstaff_return.Text = activeuserlname + ", " + activeuserfname
                    End While
                    mysqlconn.Close()
                    log_username.Text = String.Empty
                    log_password.Text = String.Empty
                    log_username.Select()

                    If looper = 1 And accountstate Then
                        Me.Hide()
                        Main.Show()
                    ElseIf looper = 1 And Not accountstate Then
                        RadMessageBox.Show(Me, "Your account is not active please inform the staff about the state of your account.", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1)
                    Else
                        RadMessageBox.Show(Me, "Incorrect Username or Password.", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1)
                    End If
                 End If
                End If
            Catch ex As MySqlException
                log_lbl_dbstatus.Text = "Offline"
                log_lbl_dbstatus.ForeColor = Color.Red
                a=False
                RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1)
            End Try    
    End Sub

    'Codes for connection status
    Public Sub CheckDBStatus()
        'Online/Offline Status
        Try
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString = connstring
            mysqlconn.Open()
            mysqlconn.Close()
            a = True
            If a = True Then
                log_lbl_dbstatus.Text = "Online"
                log_lbl_dbstatus.ForeColor = Color.Green
            Else
                log_lbl_dbstatus.Text = "Offline"
                log_lbl_dbstatus.ForeColor = Color.Red

            End If
        Catch ex As MySqlException
                log_lbl_dbstatus.Text = "Offline"
                log_lbl_dbstatus.ForeColor = Color.Red
                If db_is_deadCount = 0 Then
                    db_is_deadCount+=1
                Else
                    DB_DeadCounter()
                End If
        Finally
            mysqlconn.Dispose()
        End Try
    End Sub

    Private Sub DB_DeadCounter()
        If db_is_deadCount>=3 Then
            RadMessageBox.Show(Me, "The server is still Offline." & Environment.NewLine & "Please check the connection settings by clicking the gear icon on the top right and ask the database administrator to input the required details.", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1)
        Else
            RadMessageBox.Show(Me, "The server is offline.", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error,MessageBoxDefaultButton.Button1)
            db_is_deadCount +=1
        End If
    End Sub
    Private Sub log_lbl_dbstatus_MouseHover(sender As Object, e As EventArgs) Handles log_lbl_dbstatus.MouseHover
        If a=False
            Dim aa As DialogResult = RadMessageBox.Show(Me, "The server is offline. Would you like to check again?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
            If aa=DialogResult.Yes
                CheckDBStatus()
                If a=True
                Else
                    db_is_deadCount+=1
                End If
            End If
        End If
    End Sub

    'System Timer
    Private Sub log_timer_Tick(sender As Object, e As EventArgs) Handles log_timer.Tick
        log_lbl_date.Text = Date.Now.ToString("MMMM dd, yyyy")
        log_lbl_time.Text = Date.Now.ToString("hh:mm:ss tt")
    End Sub

    Private Sub settingButton_Click(sender As Object, e As EventArgs) Handles settingButton.Click
        ConnectionWindow.ShowDialog()
    End Sub

    Private Sub settingButton_MouseHover(sender As Object, e As EventArgs) Handles settingButton.MouseHover
        settingButton.Image = New Bitmap(My.Resources.settingIcon_Hover)
        Cursor = Cursors.Hand
    End Sub

    Private Sub settingButton_MouseLeave(sender As Object, e As EventArgs) Handles settingButton.MouseLeave
        settingButton.Image = New Bitmap(My.Resources.settingIcon)
        Cursor = Cursors.Arrow
    End Sub


    Private Sub Login_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        If (System.Windows.Forms.Application.MessageLoop) 
        System.Windows.Forms.Application.Exit()
        Else
        System.Environment.Exit(1)
        End If
    End Sub
End Class
