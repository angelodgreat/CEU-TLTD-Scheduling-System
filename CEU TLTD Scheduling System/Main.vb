﻿Imports MySql.Data.MySqlClient
Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.UI.Data

Public Class Main


    Dim MysqlConn As MySqlConnection

    Dim query As String


    Dim svYN As DialogResult
    Dim addYN As DialogResult
    Dim editYN As DialogResult
    Dim cancelYN As DialogResult
    Dim updateYN As DialogResult
    Dim deleteYN As DialogResult
    Dim doneYN As DialogResult
    Dim closingYN As DialogResult
    Dim returnYN As DialogResult
    Dim reserveYN As DialogResult
    Dim returnEquipYN As DialogResult
    Dim penaltiesDeleteYN As DialogResult
    Dim returned_eqDeleteYN As DialogResult



    Public identifier_reservationno As String
    Public random As System.Random = New System.Random


    Public equipment As String
    Public rowcounter As Integer = 0

    'WU_SETTINGS'
    Dim bdsrc_penaltylist As New BindingSource
    Dim bdsrc_returnedeq As New BindingSource
    Dim eq_keepSelectedRowIndexAfterUpdate As Integer 'WU_TRY1

    'WU_SETTINGS'

    'MENU Bar
     Private Sub MenuBar_MouseLeave(sender As Object, e As EventArgs) Handles menuItem_DBManage.MouseLeave, menuItem_About.MouseLeave, menuItem_Settings.MouseLeave
        If ThemeResolutionService.ApplicationThemeName = "VisualStudio2012Dark" Then
        Dim item As RadMenuItem = TryCast(sender, RadMenuItem)
	   item.FillPrimitive.BackColor = Color.Transparent
      End IF
    End Sub
    Private Sub MenuBar_MouseEnter(sender As Object, e As EventArgs) Handles menuItem_DBManage.MouseEnter, menuItem_About.MouseEnter, menuItem_Settings.MouseEnter
    If ThemeResolutionService.ApplicationThemeName = "VisualStudio2012Dark" Then
	Dim item As RadMenuItem = TryCast(sender, RadMenuItem)
	item.FillPrimitive.BackColor = Color.FromArgb(62, 62, 64)
	item.FillPrimitive.GradientStyle = Telerik.WinControls.GradientStyles.Solid
    End IF
End Sub
    Private Sub menuItem_LoadDB_Click(sender As Object, e As EventArgs) Handles menuItem_LoadDB.Click
        DBPasswordPrompt.Show()        
    End Sub

    Private Sub menuItem_SaveDB_Click(sender As Object, e As EventArgs) Handles menuItem_SaveDB.Click
      Actions.SaveDB()
    End Sub

    Private Sub menuItem_Settings_Click(sender As Object, e As EventArgs) Handles menuItem_Settings.Click
        MainSettingsWindow.ShowDialog()
    End Sub

    Private Sub menuItem_About_Click(sender As Object, e As EventArgs) Handles menuItem_About.Click
        MsgBox("ABOUT WINDOW HERE")
    End Sub


    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
       
        getFromDB_settings_penalty()
         
        reservation_rgv_recordeddata.Show()
        reservations_rgv_showavailableitems.Hide()

        main_load_academicsonly()
        main_load_schoolonly()

        load_rec_table()
        load_eq_table()
        load_main_acc()
        load_main_prof()
        startup_disabled_buttons()
        load_released_list()
        load_released_list2()
        load_penalty_list()
        load_returned_eq_list()
        rec_load_choices_eqtype()
        auto_generate_reservationno()
        reserved_load_table()
        startup_disabled_textbox()
        show_hide_txt_lbl()
        color_coding()
        rec_rrtc_actname.Enabled = False
        rec_lbl_actname.Enabled = False
        lu_date.Value = Date.Now
        rec_dtp_date.Value = Date.Now
        Main_Timer.Enabled = True
        load_cb_eq_type()
    End Sub


    Public Sub startup_disabled_buttons()
        eq_btn_update.Hide()
        eq_btn_delete.Hide()
        acc_staff_btn_update.Hide()
        acc_staff_btn_delete.Hide()
        acc_prof_btn_delete.Hide()
        acc_prof_btn_update.Hide()
    End Sub
    Public Sub startup_disabled_textbox()
        'rel_tb_status.Enabled = False
        'rel_tb_id.Enabled = False
        rel_tb_borrower.Enabled = False
        rel_tb_reservationnum.Enabled = False
        ret_tb_reservationnum.Enabled = False
        rel_tb_startdate.Enabled = False
        rel_tb_starttime.Enabled = False
        rel_tb_endtime.Enabled = False
        rel_tb_equipmentnum.Enabled = False
        rel_tb_equipment.Enabled = False
        ret_tb_reservationnum.Enabled = False
        ret_tb_borrower.Enabled = False
        ret_tb_sdate.Enabled = False
        ret_tb_stime.Enabled = False
        ret_tb_etime.Enabled = False
        ret_tb_equipmentnum.Enabled = False
        ret_tb_equipment.Enabled = False
    End Sub
    Public Sub color_coding()
        If (rel_tb_status.Text = "Reserved") Then
            rel_tb_status.BackColor = Color.Blue
        ElseIf (rel_tb_status.Text = "Released") Then
            rel_tb_status.BackColor = Color.Red
        ElseIf (rel_tb_status.Text = "Returned") Then
            rel_tb_status.BackColor = Color.Green
        ElseIf (rel_tb_status.Text = "") Then
            rel_tb_status.BackColor = Color.Gray
        End If

    End Sub
    Public Sub show_hide_txt_lbl()
        lbl_equipment.Hide()
        lbl_equipmentnum.Hide()
        lbl_ret_equipment.Hide()
        lbl_ret_equipmentnum.Hide()
        lbl_ret_release.Hide()
        rel_tb_equipmentnum.Hide()
        rel_tb_equipment.Hide()
        ret_tb_equipment.Hide()
        ret_tb_equipmentnum.Hide()
        ret_nameofstaff_release2.Hide()
    End Sub

    Private Sub btn_showavailequip_Click(sender As Object, e As EventArgs)
        reservation_rgv_recordeddata.Hide()
        reservations_rgv_showavailableitems.Show()
    End Sub

    Private Sub btn_showalldata_Click(sender As Object, e As EventArgs)
        reservation_rgv_recordeddata.Show()
        reservations_rgv_showavailableitems.Hide()
    End Sub

    'Formatting of GridViews

    Private Sub acc_staff_list_ViewCellFormatting(sender As Object, e As CellFormattingEventArgs) Handles acc_staff_list.ViewCellFormatting
        e.CellElement.TextAlignment = ContentAlignment.MiddleCenter
    End Sub
    Private Sub eq_rgv_showregequipment_ViewCellFormatting(sender As Object, e As CellFormattingEventArgs) Handles eq_rgv_showregequipment.ViewCellFormatting
        e.CellElement.TextAlignment = ContentAlignment.MiddleCenter
    End Sub

    Private Sub main_rgv_recordedacademicsonly_ViewCellFormatting(sender As Object, e As CellFormattingEventArgs) Handles main_rgv_recordedacademicsonly.ViewCellFormatting
        e.CellElement.TextAlignment = ContentAlignment.MiddleCenter
    End Sub

    Private Sub main_rgv_recordedschoolonly_ViewCellFormatting(sender As Object, e As CellFormattingEventArgs) Handles main_rgv_recordedschoolonly.ViewCellFormatting
        e.CellElement.TextAlignment = ContentAlignment.MiddleCenter
    End Sub

    Private Sub acc_prof_list_ViewCellFormatting(sender As Object, e As CellFormattingEventArgs) Handles acc_prof_list.ViewCellFormatting
        e.CellElement.TextAlignment = ContentAlignment.MiddleCenter
    End Sub
    Private Sub reservation_rgv_recordeddata_ViewCellFormatting(sender As Object, e As CellFormattingEventArgs) Handles reservation_rgv_recordeddata.ViewCellFormatting
        e.CellElement.TextAlignment = ContentAlignment.MiddleCenter
    End Sub

    Private Sub penalty_grid_list_ViewCellFormatting(sender As Object, e As CellFormattingEventArgs) Handles penalty_grid_list.ViewCellFormatting
        e.CellElement.TextAlignment = ContentAlignment.MiddleCenter
    End Sub
    Private Sub reserved_grid_list_ViewCellFormatting(sender As Object, e As CellFormattingEventArgs) Handles reserved_grid_list.ViewCellFormatting
        e.CellElement.TextAlignment = ContentAlignment.MiddleCenter
    End Sub

    Private Sub released_grid_list_ViewCellFormatting(sender As Object, e As CellFormattingEventArgs) Handles released_grid_list.ViewCellFormatting
        e.CellElement.TextAlignment = ContentAlignment.MiddleCenter
    End Sub
    'End Formatting of GridViews

    Public Sub getFromDB_settings_penalty()
        mysqlconn.Open()
        Dim looper As Integer = 0
        Dim q2 As String = "SELECT value FROM ceutltdscheduler.settings"
        comm = New MySqlCommand(q2, mysqlconn)
        reader = comm.ExecuteReader
            While reader.Read
            If looper=0
                penalty_price = reader.GetString("value")
            Else If looper=1
                penalty_graceperiod = reader.GetString("value") 
            Else If looper=2
                penalty_chargeinterval = reader.GetString("value")
            End If
                looper+=1
            End While
        mysqlconn.Close()
    End Sub
    Public Sub load_rec_table()

        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim sda As New MySqlDataAdapter
        Dim bsource As New BindingSource
        Dim dbdataset As New DataTable
        Dim ds As New DataSet
        dbdataset.Clear()

        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        Try
            MysqlConn.Open()

            query = "Select reservationno as 'Reservation Number' ,borrower as 'Borrower',id as 'ID', equipmentno as 'Equipment No', equipment as 'Equipment', DATE_FORMAT(date,'%M %d %Y') as 'Date',TIME_FORMAT(starttime, '%H:%i') as 'Start Time', TIME_FORMAT(endtime, '%H:%i') as 'End Time',activitytype as 'Activiity Type',actname as 'Activity' from reservation where date ='" & Format(CDate(rec_dtp_date.Value), "yyyy-MM-dd") & "' ORDER by date ASC"

            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            reservation_rgv_recordeddata.DataSource = bsource
            reservation_rgv_recordeddata.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()

        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()

        End Try
    End Sub

    Public Sub main_load_academicsonly()
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring

        Dim dbdataset As New DataTable
        Dim ds As New DataSet
        Dim sda As New MySqlDataAdapter
        Dim bsource As New BindingSource

        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        Try
            MysqlConn.Open()
            query = "Select reservationno as 'Reservation Number' ,borrower as 'Borrower',id as 'ID', equipmentno as 'Equipment No', equipment as 'Equipment', DATE_FORMAT(date,'%M %d %Y') as 'Date',TIME_FORMAT(starttime, '%H:%i') as 'Start Time', TIME_FORMAT(endtime, '%H:%i') as 'End Time',activitytype as 'Activiity Type' FROM reservation where date ='" & Format(CDate(lu_date.Value), "yyyy-MM-dd") & "' and activitytype='Academic'  ORDER by date ASC"
            ' PROBLEM: ID and Reservation is missing. THE NEXT COMMENT shows the old query
            'query = "SELECT TIME_FORMAT(starttime, '%H:%i') as 'Start Time', TIME_FORMAT(endtime, '%H:%i') as 'End Time',borrower as 'Borrower', equipment as 'Equipment', equipmentno as 'Equipment No' ,DATE_FORMAT(date,'%M %d %Y') as 'Date', activitytype as 'Activiity Type',actname as 'Activity' from reservation where date='" & Format(CDate(lu_date.Value), "yyyy-MM-dd") & "'ORDER BY starttime ASC"
            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            main_rgv_recordedacademicsonly.DataSource = bsource
            main_rgv_recordedacademicsonly.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()

        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()

        End Try
    End Sub

    Public Sub main_load_schoolonly()
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring

        Dim dbdataset As New DataTable
        Dim ds As New DataSet
        Dim sda As New MySqlDataAdapter
        Dim bsource As New BindingSource

        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        Try
            MysqlConn.Open()
            query = "Select reservationno as 'Reservation Number' ,borrower as 'Borrower',id as 'ID', equipmentno as 'Equipment No', equipment as 'Equipment', DATE_FORMAT(date,'%M %d %Y') as 'Date',TIME_FORMAT(starttime, '%H:%i') as 'Start Time', TIME_FORMAT(endtime, '%H:%i') as 'End Time',activitytype as 'Activiity Type',actname as 'Activity' from reservation where date ='" & Format(CDate(lu_date.Value), "yyyy-MM-dd") & "'and activitytype='School Activity' ORDER by date ASC"
            ' PROBLEM: ID and Reservation is missing. THE NEXT COMMENT shows the old query
            'query = "SELECT TIME_FORMAT(starttime, '%H:%i') as 'Start Time', TIME_FORMAT(endtime, '%H:%i') as 'End Time',borrower as 'Borrower', equipment as 'Equipment', equipmentno as 'Equipment No' ,DATE_FORMAT(date,'%M %d %Y') as 'Date', activitytype as 'Activiity Type',actname as 'Activity' from reservation where date='" & Format(CDate(lu_date.Value), "yyyy-MM-dd") & "'ORDER BY starttime ASC"
            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            main_rgv_recordedschoolonly.DataSource = bsource
            main_rgv_recordedschoolonly.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()

        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()

        End Try
    End Sub

    'Programmed by BRENZ STARTING POINT

    Public Sub load_main_acc()
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring

        Dim sda As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource

        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        Try
            MysqlConn.Open()
            Dim query As String
            query = "Select staff_id as 'Staff ID' , staff_fname as 'First Name' , staff_mname as 'Middle Name' , staff_surname as 'Surname' , staff_username as 'Username' , staff_type as 'User Type' from staff_reg"
            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            acc_staff_list.DataSource = bsource
            acc_staff_list.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()

        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()

        End Try
    End Sub

    'Programmed by BRENZ SECOND POINT
    Public Sub load_main_prof()
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring

        Dim sda As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource

        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        Try
            MysqlConn.Open()
            Dim query As String
            query = "Select bor_id as 'Professor ID' , bor_fname as 'First Name' , bor_mname as 'Middle Name' , bor_surname as 'Surname' , bor_college as 'College/School'  from borrowers_reg"
            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            acc_prof_list.DataSource = bsource
            acc_prof_list.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()

        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()

        End Try

    End Sub

    Public Sub load_eq_table_retain_filters_after_update()  'WU_TRY1
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim SDA As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource
        Try
            MysqlConn.Open()

            query = "SELECT equipmentno AS 'Equipment Number', equipment AS 'Equipment', equipmentsn AS 'Serial Number',equipmenttype AS 'Equipment Type', equipmentlocation AS 'Equipment Location',equipmentowner AS 'Owner',equipmentstatus AS 'Status' FROM equipments ORDER BY equipmentno DESC"

            comm = New MySqlCommand(query, MysqlConn)
            SDA.SelectCommand = comm
            SDA.Fill(dbdataset)
            bsource.DataSource = dbdataset
            eq_rgv_showregequipment.DataSource = bsource
            SDA.Update(dbdataset)

            MysqlConn.Close()

        Catch ex As MySqlException
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try

        Dim DV As New DataView(dbdataset)
        DV.RowFilter = String.Format("`Equipment Number` Like'%{0}%' and `Equipment Type` Like'%{1}%' and `Status` Like'%{2}%' ", eq_filter_eqno.Text, eq_filter_eqtype.Text, eq_filter_eqstatus.Text)
        eq_rgv_showregequipment.DataSource = DV
        eq_rgv_showregequipment.Rows(eq_keepSelectedRowIndexAfterUpdate).IsCurrent = True  'WUTRY_1
        eq_sn.Enabled = False 'WUTRY1
    End Sub


    'Programmed by BRENZ THIRD POINT SAVE BUTTON

    Private Sub acc_staff_btn_save_Click(sender As Object, e As EventArgs) Handles acc_staff_btn_save.Click
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim READER As MySqlDataReader
        If (acc_sf_id.Text = "") Or (acc_sf_fname.Text = "") Or (acc_sf_mname.Text = " ") Or (acc_sf_lname.Text = " ") Or (acc_sf_usertype.Text = " ") Or (acc_sf_username.Text = " ") Then
            RadMessageBox.Show(Me, "Please complete the fields to Save!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Else
            Try
                MysqlConn.Open()
                Dim Query As String
                Query = "insert into ceutltdscheduler.staff_reg (staff_id,staff_fname,staff_mname,staff_surname,staff_type,staff_username,staff_password) values (@staffid, @staffFname, @staffMname, @staffLname, @staffUsertype, @staffUsername, sha2(@staffPassword, 512))"
                comm = New MySqlCommand(Query, MysqlConn)
                comm.Parameters.AddWithValue("staffid", acc_sf_id.Text)
                comm.Parameters.AddWithValue("staffFname", acc_sf_fname.Text)
                comm.Parameters.AddWithValue("staffMname", acc_sf_mname.Text)
                comm.Parameters.AddWithValue("staffLname", acc_sf_lname.Text)
                comm.Parameters.AddWithValue("staffUsertype", acc_sf_usertype.Text)
                comm.Parameters.AddWithValue("staffUsername", acc_sf_username.Text)
                comm.Parameters.AddWithValue("staffPassword", acc_sf_password.Text)


                svYN = RadMessageBox.Show(Me, "Are you sure you want To save this information? ", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
                If svYN = MsgBoxResult.Yes Then
                    If (acc_sf_password.Text = acc_sf_retypepassword.Text) Then
                        READER = comm.ExecuteReader
                        RadMessageBox.Show("Register Complete!")
                        acc_sf_id.Text = " "
                        acc_sf_fname.Text = " "
                        acc_sf_mname.Text = " "
                        acc_sf_lname.Text = " "
                        acc_sf_usertype.Text = " "
                        acc_sf_username.Text = " "
                        acc_sf_password.Text = ""
                        acc_sf_retypepassword.Text = ""

                    Else
                        RadMessageBox.Show("Password did Not match!")


                    End If
                End If
                MysqlConn.Close()
            Catch ex As MySqlException
                MessageBox.Show(ex.Message)
            Finally
                MysqlConn.Dispose()
                load_main_acc()
            End Try
        End If
    End Sub


    'Programmed by BRENZ 4th point Cell Double Click

    Private Sub acc_staff_list_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles acc_staff_list.CellDoubleClick
        updateYN = RadMessageBox.Show(Me, "Do you want to select this information?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If updateYN = MsgBoxResult.Yes Then

            If e.RowIndex >= 0 Then
                Dim row As Telerik.WinControls.UI.GridViewRowInfo

                row = Me.acc_staff_list.Rows(e.RowIndex)

                acc_sf_id.Text = row.Cells("Staff ID").Value.ToString
                acc_sf_fname.Text = row.Cells("First Name").Value.ToString
                acc_sf_mname.Text = row.Cells("Middle Name").Value.ToString
                acc_sf_lname.Text = row.Cells("Surname").Value.ToString
                acc_sf_usertype.Text = row.Cells("User Type").Value.ToString
                acc_sf_username.Text = row.Cells("Username").Value.ToString

                acc_sf_id.Enabled = False
                acc_sf_password.Enabled = False
                acc_sf_retypepassword.Enabled = False
                acc_staff_btn_update.Show()
                acc_staff_btn_delete.Show()
                acc_staff_btn_save.Hide()
                load_main_acc()

            End If

        End If
    End Sub

    'Programmed by BRENZ 5th Point Update Button!
    Private Sub acc_staff_btn_update_Click(sender As Object, e As EventArgs) Handles acc_staff_btn_update.Click

        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        updateYN = RadMessageBox.Show(Me, "Do you want to update the selected Information?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If updateYN = MsgBoxResult.Yes Then
            If (acc_sf_id.Text = "") Or (acc_sf_fname.Text = "") Or (acc_sf_mname.Text = " ") Or (acc_sf_lname.Text = " ") Or (acc_sf_usertype.Text = " ") Or (acc_sf_username.Text = " ") Then
                RadMessageBox.Show(Me, "Please complete the fields to update!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Else
                Try
                    MysqlConn.Open()
                    query = "UPDATE staff_reg set staff_id = '" & acc_sf_id.Text & "' , staff_fname = '" & acc_sf_fname.Text & "' , staff_mname = '" & acc_sf_mname.Text & "' , staff_surname = '" & acc_sf_lname.Text & "' , staff_type = '" & acc_sf_usertype.Text & "' , staff_username = '" & acc_sf_username.Text & "' where staff_id = '" & acc_sf_id.Text & "' "
                    comm = New MySqlCommand(query, MysqlConn)
                    reader = comm.ExecuteReader

                    RadMessageBox.Show(Me, "Update Success!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info)
                    MysqlConn.Close()


                Catch ex As Exception
                    RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
                Finally
                    MysqlConn.Dispose()
                End Try
            End If
        End If
        load_main_acc()

    End Sub

    'Programmed by BRENZ 6th Point Delete Button!
    Private Sub acc_staff_btn_delete_Click(sender As Object, e As EventArgs) Handles acc_staff_btn_delete.Click
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        deleteYN = RadMessageBox.Show(Me, "Are you sure you want To Delete this information? ", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If deleteYN = MsgBoxResult.Yes Then
            Try
                MysqlConn.Open()
                Dim Query As String
                Query = "delete from staff_reg where staff_id = '" & acc_sf_id.Text & "'"
                comm = New MySqlCommand(Query, MysqlConn)
                reader = comm.ExecuteReader

                RadMessageBox.Show(Me, "Delete Complete!", "Delete")
                MysqlConn.Close()
            Catch ex As MySqlException
                MessageBox.Show(ex.Message)
            Finally
                MysqlConn.Dispose()
                load_main_acc()
                acc_sf_id.Text = " "
                acc_sf_fname.Text = " "
                acc_sf_mname.Text = " "
                acc_sf_lname.Text = " "
                acc_sf_usertype.Text = " "
                acc_sf_username.Text = " "

            End Try
        End If

    End Sub

    'Programmed by Brenz 7th point Clear Button!
    Private Sub acc_staff_btn_clear_Click(sender As Object, e As EventArgs) Handles acc_staff_btn_clear.Click
        doneYN = RadMessageBox.Show(Me, "Are you sure you want to clear the fields?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If doneYN = MsgBoxResult.Yes Then
            acc_sf_id.Text = " "
            acc_sf_fname.Text = " "
            acc_sf_mname.Text = " "
            acc_sf_lname.Text = " "
            acc_sf_usertype.Text = " "
            acc_sf_username.Text = " "
            acc_sf_password.Text = ""
            acc_sf_retypepassword.Text = ""
            acc_sf_password.Enabled = True
            acc_sf_retypepassword.Enabled = True
            acc_staff_btn_save.Show()
            acc_staff_btn_update.Hide()
            acc_staff_btn_delete.Hide()
        End If

    End Sub

    'Programmed by Brenz 8th point prof Save Button!
    Private Sub acc_prof_btn_save_Click(sender As Object, e As EventArgs) Handles acc_prof_btn_save.Click
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim READER As MySqlDataReader
        If (acc_pf_id.Text = "") Or (acc_pf_fname.Text = "") Or (acc_pf_mname.Text = " ") Or (acc_pf_lname.Text = " ") Or (acc_pf_college.Text = " ") Then
            RadMessageBox.Show(Me, "Please complete the fields to Save!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Else
            Try
                MysqlConn.Open()
                Dim Query As String
                Query = "insert into `borrowers_reg`  values (@BorID, @BorFname, @BorMname, @BorLname, @BorCollege)"
                comm = New MySqlCommand(Query, MysqlConn)
                comm.Parameters.AddWithValue("BorID", acc_pf_id.Text)
                comm.Parameters.AddWithValue("BorFname", acc_pf_fname.Text)
                comm.Parameters.AddWithValue("BorMname", acc_pf_mname.Text)
                comm.Parameters.AddWithValue("BorLname", acc_pf_lname.Text)
                comm.Parameters.AddWithValue("BorCollege", acc_pf_college.Text)


                svYN = RadMessageBox.Show(Me, "Are you sure you want To save this information? ", "TLTD Schuling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
                If svYN = MsgBoxResult.Yes Then

                    READER = comm.ExecuteReader
                    RadMessageBox.Show("Register Complete!")

                End If
                MysqlConn.Close()
            Catch ex As MySqlException
                MessageBox.Show(ex.Message)
            Finally
                MysqlConn.Dispose()
                load_main_prof()
                acc_pf_id.Text = " "
                acc_pf_fname.Text = " "
                acc_pf_mname.Text = " "
                acc_pf_lname.Text = " "
                acc_pf_college.Text = " "

            End Try
        End If
    End Sub



    'Programmed by Brenz 9th point Cell Double Click Prof List!
    Private Sub acc_prof_list_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles acc_prof_list.CellDoubleClick
        updateYN = RadMessageBox.Show(Me, "Do you want to select this information?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If updateYN = MsgBoxResult.Yes Then

            If e.RowIndex >= 0 Then
                Dim row As Telerik.WinControls.UI.GridViewRowInfo

                row = Me.acc_prof_list.Rows(e.RowIndex)

                acc_pf_id.Text = row.Cells("Professor ID").Value.ToString
                acc_pf_fname.Text = row.Cells("First Name").Value.ToString
                acc_pf_mname.Text = row.Cells("Middle Name").Value.ToString
                acc_pf_lname.Text = row.Cells("Surname").Value.ToString
                acc_pf_college.Text = row.Cells("College/School").Value.ToString

                acc_pf_id.Enabled = False
                acc_prof_btn_update.Show()
                acc_prof_btn_delete.Show()
                acc_prof_btn_save.Hide()
                load_main_prof()

            End If

        End If

    End Sub

    'Programmed by Brenz 10th Point Prof Update Button
    Private Sub acc_prof_btn_update_Click(sender As Object, e As EventArgs) Handles acc_prof_btn_update.Click
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        updateYN = RadMessageBox.Show(Me, "Do you want to update the selected Information?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If updateYN = MsgBoxResult.Yes Then
            If (acc_pf_id.Text = "") Or (acc_pf_fname.Text = "") Or (acc_pf_mname.Text = " ") Or (acc_pf_lname.Text = " ") Or (acc_pf_college.Text = " ") Then
                RadMessageBox.Show(Me, "Please complete the fields to update!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Else
                Try
                    MysqlConn.Open()
                    query = "UPDATE borrowers_reg set bor_id = '" & acc_pf_id.Text & "' , bor_fname = '" & acc_pf_fname.Text & "' , bor_mname = '" & acc_pf_mname.Text & "' , bor_surname = '" & acc_pf_lname.Text & "' , bor_college = '" & acc_pf_college.Text & "' where bor_id = '" & acc_pf_id.Text & "' "
                    comm = New MySqlCommand(query, MysqlConn)
                    reader = comm.ExecuteReader

                    RadMessageBox.Show(Me, "Update Success!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info)
                    MysqlConn.Close()


                Catch ex As Exception
                    RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
                Finally
                    MysqlConn.Dispose()
                End Try
            End If
        End If
        load_main_prof()
    End Sub

    'Programmed by Brenz 11th point prof Delete Button!
    Private Sub acc_prof_btn_delete_Click(sender As Object, e As EventArgs) Handles acc_prof_btn_delete.Click
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        deleteYN = RadMessageBox.Show(Me, "Are you sure you want To Delete this information? ", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If deleteYN = MsgBoxResult.Yes Then
            Try
                MysqlConn.Open()
                Dim Query As String
                Query = "delete from borrowers_reg where bor_id = '" & acc_pf_id.Text & "'"
                comm = New MySqlCommand(Query, MysqlConn)
                reader = comm.ExecuteReader

                RadMessageBox.Show(Me, "Delete Complete!", "Delete")
                MysqlConn.Close()
            Catch ex As MySqlException
                MessageBox.Show(ex.Message)
            Finally
                MysqlConn.Dispose()
                load_main_prof()
                acc_pf_id.Text = " "
                acc_pf_fname.Text = " "
                acc_pf_mname.Text = " "
                acc_pf_lname.Text = " "
                acc_pf_college.Text = " "

            End Try
        End If



    End Sub

    'Programmed by Brenz 12th Point Clear Button
    Private Sub acc_prof_btn_clear_Click(sender As Object, e As EventArgs) Handles acc_prof_btn_clear.Click
        doneYN = RadMessageBox.Show(Me, "Are you sure you want to clear the fields?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If doneYN = MsgBoxResult.Yes Then
            acc_pf_id.Text = " "
            acc_pf_fname.Text = " "
            acc_pf_mname.Text = " "
            acc_pf_lname.Text = " "
            acc_pf_college.Text = " "

            acc_prof_btn_delete.Hide()
            acc_prof_btn_update.Hide()
            acc_prof_btn_save.Show()
        End If
    End Sub

    'Programmed by BRENZ 13th POINT Load form grid RELEASED at Releasing Management!
    Public Sub load_released_list()
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring

        Dim sda As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource

        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        Try
            MysqlConn.Open()
            Dim query As String
            query = "Select rel_reservation_no as 'Reservation Number', rel_id_passnum as 'Pass Number ' , rel_borrower as ' Borrower ' , rel_equipment_no as 'Equipment Number', rel_equipment as 'Equipment',DATE_FORMAT(rel_assign_date,'%M %d %Y') as 'Date',TIME_FORMAT(rel_starttime, '%H:%i') as 'Start Time', TIME_FORMAT(rel_endtime, '%H:%i') as 'End Time'  , rel_status as ' Status ' , rel_releasedby as ' Released By'  from released_info"
            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            released_grid_list.DataSource = bsource
            released_grid_list.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()
        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try
    End Sub

    'Programmed by BRENZ 14th POINT Load form grid RELEASED at returning Management!
    Public Sub load_released_list2()
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring

        Dim sda As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource

        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        Try
            MysqlConn.Open()
            Dim query As String
            query = "Select rel_reservation_no as 'Reservation Number' , rel_id_passnum as 'Pass Number' , rel_borrower as 'Borrower' , rel_equipment_no as 'Equipment No' , rel_equipment as 'Equipment' , DATE_FORMAT(rel_assign_date,'%M %d %Y') as 'Date',TIME_FORMAT(rel_starttime, '%H:%i') as 'Start Time', TIME_FORMAT(rel_endtime, '%H:%i') as 'End Time' , rel_status as 'Status' , rel_releasedby as 'Released By'  from released_info"
            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            released_grid_list2.DataSource = bsource
            released_grid_list2.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()
        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try
    End Sub

    'Programmed by BRENZ 15th point RELEASE BTN at Releasing Management!
    Private Sub released_btn_release_Click(sender As Object, e As EventArgs) Handles released_btn_release.Click

        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim READER As MySqlDataReader
        If (rel_tb_reservationnum.Text = "") Or (rel_tb_borrower.Text = "") Or (rel_tb_equipmentnum.Text = "") Or (rel_tb_equipment.Text = "") Or (rel_tb_startdate.Text = " ") Or (rel_tb_starttime.Text = " ") Or (rel_tb_endtime.Text = " ") Then
            RadMessageBox.Show(Me, "Please complete the fields to Save!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Else
            Try
                MysqlConn.Open()
                Dim Query As String
                ' Dim Query2 As String
                Query = "insert into released_info (rel_id_passnum,rel_reservation_no,rel_borrower,rel_equipment_no,rel_equipment,rel_assign_date,rel_starttime,rel_endtime,rel_status,rel_releasedby) VALUES('" & rel_tb_id.Text & "','" & rel_tb_reservationnum.Text & "','" & rel_tb_borrower.Text & "','" & rel_tb_equipmentnum.Text & "','" & rel_tb_equipment.Text & "','" & Format(CDate(rel_tb_startdate.Value), "yyyy-MM-dd") & "','" & Format(CDate(rel_tb_starttime.Text), "HH:mm") & "','" & Format(CDate(rel_tb_endtime.Text), "HH:mm") & "','" & rel_tb_status.Text & "','" & rel_nameofstaff_release.Text & "')"  '; update reservation set  res_status = '" & rel_tb_status.Text & "' where reservationno = '" & rel_tb_reservationnum.Text & "'"
                'Query = "delete from reservation where  reservationno = '" & rel_tb_reservationnum.Text & "'"
                comm = New MySqlCommand(Query, MysqlConn)
                'comm.Parameters.AddWithValue("ID", rel_tb_id.Text)
                'comm.Parameters.AddWithValue("ID", rel_tb_id.Text)
                'comm.Parameters.AddWithValue("ID", rel_tb_id.Text)


                svYN = RadMessageBox.Show(Me, "Are you sure you want to Release this Equipment/s? ", "TLTD Schuling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
                If svYN = MsgBoxResult.Yes Then
                    READER = comm.ExecuteReader
                    RadMessageBox.Show("Released!")

                End If
                MysqlConn.Close()
            Catch ex As MySqlException
                MessageBox.Show(ex.Message)
            Finally
                MysqlConn.Dispose()
                rel_tb_reservationnum.Text = ""
                rel_tb_id.Text = ""
                rel_tb_borrower.Text = ""
                rel_tb_startdate.Text = "01/01/99"
                rel_tb_starttime.Text = ""
                rel_tb_endtime.Text = ""
                rel_tb_status.Text = ""
                rel_tb_equipment.Text = ""
                rel_tb_equipmentnum.Text = ""
                show_hide_txt_lbl()
                load_released_list()
                load_released_list2()
                reserved_load_table()
                load_rec_table()
                color_coding()
            End Try
        End If

    End Sub


    'Programmed by BRENZ 16th Point UPDATE BTN at Releasing Management
    Private Sub released_btn_update_Click(sender As Object, e As EventArgs)
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        updateYN = RadMessageBox.Show(Me, "Do you want to Update the Date/Time/Location of the Reserved Equipment?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If updateYN = MsgBoxResult.Yes Then
            If (rel_tb_startdate.Text = "") Or (rel_tb_starttime.Text = " ") Or (rel_tb_endtime.Text = " ") Then
                RadMessageBox.Show(Me, "Please complete the fields to update!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Else
                Try
                    MysqlConn.Open()
                    query = "UPDATE released_info set rel_assign_date = '" & rel_tb_starttime.Text & "'  , rel_starttime = '" & rel_tb_starttime.Text & "' , rel_endtime = '" & rel_tb_endtime.Text & "'  where rel_assign_date = '" & rel_tb_startdate.Text & "' "
                    comm = New MySqlCommand(query, MysqlConn)
                    reader = comm.ExecuteReader

                    RadMessageBox.Show(Me, "Update Success!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info)
                    MysqlConn.Close()


                Catch ex As Exception
                    RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
                Finally
                    MysqlConn.Dispose()
                    load_released_list()
                    load_released_list2()
                End Try
            End If
        End If



    End Sub

    'Programmed by BRENZ 17th Point Cancel BTN at Returning Management
    Private Sub return_btn_cancel_Click(sender As Object, e As EventArgs) Handles return_btn_cancel.Click
        cancelYN = RadMessageBox.Show(Me, "Do you want to cancel returning? ", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If cancelYN = MsgBoxResult.Yes Then
            ret_tb_reservationnum.Text = ""
            ret_tb_id.Text = ""
            ret_tb_borrower.Text = ""
            ret_tb_sdate.Text = "01/01/99"
            ret_tb_stime.Text = ""
            ret_tb_etime.Text = ""
            ret_tb_status.Text = ""
            'ret_tb_equipment.Text = ""
            ret_tb_equipmentnum.Text = ""
            show_hide_txt_lbl()
        End If
    End Sub


    'Programmed by BRENZ 18th Point Cancel BTN at Releasing Management
    Private Sub released_btn_cancel_Click(sender As Object, e As EventArgs) Handles released_btn_cancel.Click
        cancelYN = RadMessageBox.Show(Me, "Do you want to cancel returning? ", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If cancelYN = MsgBoxResult.Yes Then
            rel_tb_id.Text = ""
            rel_tb_borrower.Text = ""
            rel_tb_startdate.Text = "01/01/99"
            rel_tb_starttime.Text = ""
            rel_tb_status.Text = ""
            rel_tb_equipment.Text = ""
            rel_tb_equipmentnum.Text = ""
            lbl_equipment.Hide()
            lbl_equipmentnum.Hide()
            rel_tb_equipment.Hide()
            rel_tb_equipmentnum.Hide()
            color_coding()



        End If




    End Sub

    'Programmed by BRENZ 19th Point reserved records at releasing management

    Public Sub reserved_load_table()
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring

        Dim sda As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource

        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        Try
            MysqlConn.Open()
            Dim query As String
            query = "Select reservationno as 'Reservation Number' , borrower as 'Borrower', equipmentno as 'Equipment No', equipment as 'Equipment', DATE_FORMAT(date,'%M %d %Y') as 'Date',TIME_FORMAT(starttime, '%H:%i') as 'Start Time', TIME_FORMAT(endtime, '%H:%i') as 'End Time' , res_status as 'Status' from reservation  ORDER by date ASC"
            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            reserved_grid_list.DataSource = bsource
            reserved_grid_list.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()
        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try

    End Sub

    'Programmed by BRENZ 20th Point reserved_grid_list cell double click at releasing management
    Private Sub reserved_grid_list_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles reserved_grid_list.CellDoubleClick
        Dim released As String

        If e.RowIndex >= 0 Then
            Dim row As Telerik.WinControls.UI.GridViewRowInfo

            row = Me.reserved_grid_list.Rows(e.RowIndex)
            released = row.Cells("Status").Value.ToString

            If released = "Released" Then
                RadMessageBox.Show(Me, "The equipment is already released", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Exclamation)

            Else

                updateYN = RadMessageBox.Show(Me, "Do you want to select this information?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
                If updateYN = MsgBoxResult.Yes Then

                    If e.RowIndex >= 0 Then
                        Dim row2 As Telerik.WinControls.UI.GridViewRowInfo

                        row2 = Me.reserved_grid_list.Rows(e.RowIndex)

                        rel_tb_reservationnum.Text = row2.Cells("Reservation Number").Value.ToString
                        rel_tb_borrower.Text = row2.Cells("Borrower").Value.ToString
                        rel_tb_startdate.Text = row2.Cells("Date").Value.ToString
                        rel_tb_starttime.Text = row2.Cells("Start Time").Value.ToString
                        rel_tb_endtime.Text = row2.Cells("End Time").Value.ToString
                        rel_tb_status.Text = row2.Cells("Status").Value.ToString
                        rel_tb_equipmentnum.Text = row2.Cells("Equipment No").Value.ToString
                        rel_tb_equipment.Text = row2.Cells("Equipment").Value.ToString




                        lbl_equipment.Show()
                        lbl_equipmentnum.Show()
                        rel_tb_equipment.Show()
                        rel_tb_equipmentnum.Show()
                        reserved_load_table()
                        color_coding()
                    End If

                End If
            End If
        End If
    End Sub


    'Programmed by BRENZ/wu 21st Point Closing button
    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        closingYN = RadMessageBox.Show(Me, "Are you sure you want to Log-Out?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation)
        If closingYN = MsgBoxResult.Yes Then
            Me.Dispose()
            Login.Show()
        ElseIf closingYN = MsgBoxResult.No Then
            e.Cancel = True
        End If
    End Sub


    'Programmed by BRENZ 22nd cell double click release2 @ return management


    Private Sub released_grid_list2_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles released_grid_list2.CellDoubleClick
        returnYN = RadMessageBox.Show(Me, "Do you want to select this information?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If returnYN = MsgBoxResult.Yes Then

            If e.RowIndex >= 0 Then
                Dim row As Telerik.WinControls.UI.GridViewRowInfo

                row = Me.released_grid_list2.Rows(e.RowIndex)

                ret_tb_reservationnum.Text = row.Cells("Reservation Number").Value.ToString
                ret_tb_id.Text = row.Cells("Pass Number").Value.ToString
                ret_tb_borrower.Text = row.Cells("Borrower").Value.ToString
                ret_tb_sdate.Text = row.Cells("Date").Value.ToString
                ret_tb_stime.Text = row.Cells("Start Time").Value.ToString
                ret_tb_etime.Text = row.Cells("End Time").Value.ToString
                ret_tb_status.Text = row.Cells("Status").Value.ToString
                ret_tb_equipmentnum.Text = row.Cells("Equipment No").Value.ToString
                ret_tb_equipment.Text = row.Cells("Equipment").Value.ToString
                ret_nameofstaff_release2.Text = row.Cells("Released By").Value.ToString

                lbl_ret_equipmentnum.Show()
                lbl_ret_equipment.Show()
                ret_tb_equipmentnum.Show()
                ret_tb_equipment.Show()
                lbl_ret_release.Show()
                ret_nameofstaff_release2.Show()
                load_released_list2()
                'show_hide_txt_lbl()
                color_coding()
            End If

        End If
    End Sub







    'Main Window Search Functions Umali C1
    Private Sub lu_byequipment_TextChanged(sender As Object, e As EventArgs) Handles lu_byequipment.TextChanged
        lu_byequipment_filter_delay.Interval = 700
        lu_byequipment_filter_delay.Stop()
        lu_byequipment_filter_delay.Start()
    End Sub

    Private Sub lu_byequipment_filter_delay_Tick(sender As Object, e As EventArgs) Handles lu_byequipment_filter_delay.Tick
        lu_byequipment_filter_delay.Stop()
        ''''''''Pending Changes because of client request

        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim SDA As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource
        Try
            MysqlConn.Open()


            query = "Select reservationno as 'Reservation Number' ,borrower as 'Borrower',id as 'ID', equipmentno as 'Equipment No', equipment as 'Equipment',
            DATE_FORMAT(date,'%M %d %Y') as 'Date',TIME_FORMAT(starttime, '%H:%i') as 'Start Time', TIME_FORMAT(endtime, '%H:%i') as 'End Time',
            activitytype as 'Activiity Type',actname as 'Activity' from reservation where date ='" & Format(CDate(rec_dtp_date.Value), "yyyy-MM-dd") & "'  ORDER BY equipment desc"

            comm = New MySqlCommand(query, MysqlConn)
            SDA.SelectCommand = comm
            SDA.Fill(dbdataset)
            bsource.DataSource = dbdataset
            main_rgv_recordedacademicsonly.DataSource = bsource
            SDA.Update(dbdataset)

            MysqlConn.Close()

        Catch ex As MySqlException
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try

        Dim DV As New DataView(dbdataset)
        DV.RowFilter = String.Format("`Equipment` Like'%{0}%' and `Borrower` Like'%{1}%'", lu_byequipment.Text, lu_byname.Text)
        main_rgv_recordedacademicsonly.DataSource = DV
    End Sub

    'Search by Name in Main Tab
    Private Sub lu_byname_TextChanged(sender As Object, e As EventArgs) Handles lu_byname.TextChanged
        lu_byname_filter_delay.Interval = 700
        lu_byname_filter_delay.Stop()
        lu_byname_filter_delay.Start()
    End Sub

    Private Sub lu_byname_filter_delay_Tick(sender As Object, e As EventArgs) Handles lu_byname_filter_delay.Tick
        lu_byname_filter_delay.Stop()
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim SDA As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource
        Try
            MysqlConn.Open()

            query = "Select reservationno as 'Reservation Number' ,borrower as 'Borrower',id as 'ID', equipmentno as 'Equipment No', equipment as 'Equipment',
            DATE_FORMAT(date,'%M %d %Y') as 'Date',TIME_FORMAT(starttime, '%H:%i') as 'Start Time', TIME_FORMAT(endtime, '%H:%i') as 'End Time',
            activitytype as 'Activiity Type',actname as 'Activity' from reservation where date ='" & Format(CDate(rec_dtp_date.Value), "yyyy-MM-dd") & "'"

            comm = New MySqlCommand(query, MysqlConn)
            SDA.SelectCommand = comm
            SDA.Fill(dbdataset)
            bsource.DataSource = dbdataset
            main_rgv_recordedacademicsonly.DataSource = bsource
            SDA.Update(dbdataset)

            MysqlConn.Close()

        Catch ex As MySqlException
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try

        Dim DV As New DataView(dbdataset)
        DV.RowFilter = String.Format("`Borrower` Like'%{0}%' and `Equipment` Like'%{1}%'", lu_byname.Text, lu_byequipment.Text)
        main_rgv_recordedacademicsonly.DataSource = DV
    End Sub

    'Equipment Management Codes Umali E1 EQ_LOAD_EQ_TABLE

    Public Sub load_eq_table()
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring

        Dim sda As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource

        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        Try
            MysqlConn.Open()

            query = "SELECT equipmentno as 'Equipment Number', equipment as 'Equipment', equipmentsn as 'Serial Number',equipmenttype as 'Equipment Type', equipmentlocation as 'Equipment Location',equipmentowner as 'Owner',equipmentstatus as 'Status' from equipments ORDER BY equipment ASC"

            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            eq_rgv_showregequipment.DataSource = bsource
            eq_rgv_showregequipment.ReadOnly = True




            sda.Update(dbdataset)
            MysqlConn.Close()

        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()

        End Try

    End Sub

    'Equipment Management Codes Umali E2 EQ_BTN_SAVE

    Private Sub eq_btn_save_Click(sender As Object, e As EventArgs)
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim reader As MySqlDataReader


        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If
        If (eq_equipmentno.Text = "") Or (eq_sn.Text = "") Or (eq_equipment.Text = "") Or (eq_equipmentlocation.Text = "") Or (eq_owner.Text = "") Or (eq_status.Text = "") Then
            RadMessageBox.Show(Me, "Please complete the fields to update!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)

        Else
            Try
                MysqlConn.Open()
                query = "SELECT * from equipments where (equipmentsn=@eq_sn)"
                comm = New MySqlCommand(query, MysqlConn)
                comm.Parameters.AddWithValue("eq_sn", eq_sn.Text)
                reader = comm.ExecuteReader

                Dim count As Integer

                While reader.Read
                    count += 1
                End While

                If count = 1 Then
                    RadMessageBox.Show(Me, "Equipment Number" & eq_equipmentno.Text & " And the equipment " & eq_equipment.Text & " Is already registered")
                Else
                    MysqlConn.Close()
                    MysqlConn.Open()

                    addYN = RadMessageBox.Show(Me, "Are you sure you want To save this equipment?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
                    If addYN = MsgBoxResult.Yes Then
                        query = "INSERT INTO `equipments` VALUES (@eq_eqno, @eq_eqeq, @eq_eqsn, @eq_eqlocation, @eq_eqowner, @eq_eqstatus, @eq_eqtype)"
                        comm = New MySqlCommand(query, MysqlConn)

                        comm.Parameters.AddWithValue("eq_eqno", eq_equipmentno.Text)
                        comm.Parameters.AddWithValue("eq_eqeq", eq_equipment.Text)
                        comm.Parameters.AddWithValue("eq_eqsn", eq_sn.Text)
                        comm.Parameters.AddWithValue("eq_eqlocation", eq_equipmentlocation.Text)
                        comm.Parameters.AddWithValue("eq_eqowner", eq_owner.Text)
                        comm.Parameters.AddWithValue("eq_eqstatus", eq_status.Text)
                        comm.Parameters.AddWithValue("eq_eqtype", eq_type.Text)


                        reader = comm.ExecuteReader
                        RadMessageBox.Show(Me, "Equipment Registered Successfully!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info)
                        MysqlConn.Close()
                    End If
                End If
            Catch ex As Exception
                RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()

            End Try
        End If
        load_eq_table()
        counter_of_total_eq()
    End Sub


    'Equipment Management Codes Umali E3 EQ_BTN_UPDATE

    Private Sub eq_btn_update_Click(sender As Object, e As EventArgs)
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        updateYN = RadMessageBox.Show(Me, "Do you want To update the selected equipment?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If updateYN = MsgBoxResult.Yes Then
            If (eq_equipmentno.Text = "") Or (eq_sn.Text = "") Or (eq_equipment.Text = "") Or (eq_equipmentlocation.Text = "") Or (eq_owner.Text = "") Or (eq_status.Text = "") Then
                RadMessageBox.Show(Me, "Please complete the fields To update!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Else
                Try
                    MysqlConn.Open()
                    query = "UPDATE equipments SET equipmentno=@eq_eqno, equipmentsn=@eq_eqsn, equipment=@eq_eqeq, equipmentlocation=@eq_eqlocation, equipmentowner=@eq_eqowner, equipmentstatus=@eq_eqstatus, equipmenttype=@eq_eqtype WHERE (equipmentsn=@eq_eqsn) AND (equipmentno=@eq_eqno)"
                    comm = New MySqlCommand(query, MysqlConn)

                    comm.Parameters.AddWithValue("eq_eqno", eq_equipmentno.Text)
                    comm.Parameters.AddWithValue("eq_eqeq", eq_equipment.Text)
                    comm.Parameters.AddWithValue("eq_eqsn", eq_sn.Text)
                    comm.Parameters.AddWithValue("eq_eqlocation", eq_equipmentlocation.Text)
                    comm.Parameters.AddWithValue("eq_eqowner", eq_owner.Text)
                    comm.Parameters.AddWithValue("eq_eqstatus", eq_status.Text)
                    comm.Parameters.AddWithValue("eq_eqtype", eq_type.Text)


                    reader = comm.ExecuteReader

                    RadMessageBox.Show(Me, "Update Success!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info)
                    MysqlConn.Close()


                Catch ex As Exception
                    RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
                Finally
                    MysqlConn.Dispose()
                End Try
            End If
        End If

        load_eq_table_retain_filters_after_update()


        counter_of_total_eq()
    End Sub

    'Equipment Management Codes Umali E4 EQ_BTN_DELETE

    Private Sub eq_btn_delete_Click(sender As Object, e As EventArgs)
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        deleteYN = RadMessageBox.Show(Me, "Are you sure you want to delete?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If deleteYN = MsgBoxResult.Yes Then
            Try
                MysqlConn.Open()
                query = "DELETE FROM equipments WHERE equipmentno=@eq_eqno AND equipmentsn=@eq_eqsn "
                comm = New MySqlCommand(query, MysqlConn)
                comm.Parameters.AddWithValue("eq_eqno", eq_equipmentno.Text)
                comm.Parameters.AddWithValue("eq_eqsn", eq_sn.Text)

                reader = comm.ExecuteReader

                eq_equipment.Text = ""
                eq_equipmentno.Text = ""
                eq_equipmentlocation.Text = ""
                eq_sn.Text = ""
                eq_status.Text = ""
                eq_owner.Text = ""

                RadMessageBox.Show(Me, "Successfully Deleted!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info)
                MysqlConn.Close()
            Catch ex As Exception
                RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()
            End Try
        End If
        load_eq_table()
        counter_of_total_eq()
    End Sub

    Private Sub eq_clear_filter_Click(sender As Object, e As EventArgs) Handles eq_clear_filter.Click
        eq_filter_eqno.Text = ""
        eq_filter_eqtype.Text = ""
        eq_filter_eqstatus.Text = ""
    End Sub

    'Equipment Management Codes Umali E5 EQ_CELLDOUBLECLICK

    Private Sub eq_rgv_showregequipment_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles eq_rgv_showregequipment.CellDoubleClick
        If e.RowIndex >= 0 Then
            Dim row As Telerik.WinControls.UI.GridViewRowInfo
            eq_keepSelectedRowIndexAfterUpdate = e.RowIndex 'WUTRY_1
            row = Me.eq_rgv_showregequipment.Rows(e.RowIndex)

            eq_equipmentno.Text = row.Cells("Equipment Number").Value.ToString
            eq_equipment.Text = row.Cells("Equipment").Value.ToString
            eq_equipmentlocation.Text = row.Cells("Equipment Location").Value.ToString
            eq_sn.Text = row.Cells("Serial Number").Value.ToString
            eq_status.Text = row.Cells("Status").Value.ToString
            eq_owner.Text = row.Cells("Owner").Value.ToString
            eq_type.Text = row.Cells("Equipment Type").Value.ToString

            eq_sn.Enabled = False
            eq_btn_update.Show()
            eq_btn_delete.Show()
            eq_btn_save.Hide()
        End If
    End Sub

    'Equipment Management Codes Umali E6 = EQ_BTN_CLEAR

    Private Sub eq_btn_clear_Click(sender As Object, e As EventArgs)
        eq_equipmentno.Text = ""
        eq_equipment.Text = ""
        eq_equipmentlocation.Text = ""
        eq_sn.Text = ""
        eq_status.Text = ""
        eq_owner.Text = ""
        eq_type.Text = ""
        eq_sn.Enabled = True
        eq_btn_update.Hide()
        eq_btn_delete.Hide()
        eq_btn_save.Show()
    End Sub

    'Equipment Management Codes Umali E7 = SEARCH BY EQ_NO

    Private Sub eqno_filter_delay_Tick(sender As Object, e As EventArgs) Handles eqno_filter_delay.Tick
        eqno_filter_delay.Stop()
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim SDA As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource

        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        Try
            MysqlConn.Open()

            query = "SELECT equipmentno AS 'Equipment Number', equipment AS 'Equipment', equipmentsn AS 'Serial Number',equipmenttype AS 'Equipment Type', equipmentlocation AS 'Equipment Location',equipmentowner AS 'Owner',equipmentstatus AS 'Status' FROM equipments ORDER BY equipmentno DESC"

            comm = New MySqlCommand(query, MysqlConn)
            SDA.SelectCommand = comm
            SDA.Fill(dbdataset)
            bsource.DataSource = dbdataset
            eq_rgv_showregequipment.DataSource = bsource
            SDA.Update(dbdataset)

            MysqlConn.Close()

        Catch ex As MySqlException
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try

        Dim DV As New DataView(dbdataset)
        DV.RowFilter = String.Format("`Equipment Number` Like'%{0}%' and `Equipment Type` Like'%{1}%' and `Status` Like'%{2}%' ", eq_filter_eqno.Text, eq_filter_eqtype.Text, eq_filter_eqstatus.Text)
        eq_rgv_showregequipment.DataSource = DV
    End Sub


    Private Sub eq_filter_eqno_TextChanged(sender As Object, e As EventArgs) Handles eq_filter_eqno.TextChanged
        eqno_filter_delay.Interval = 700
        eqno_filter_delay.Stop()
        eqno_filter_delay.Start()
    End Sub

    'Equipment Management Codes Umali E8 = SEARCH BY EQ_TYPE
    Private Sub eq_filter_eqtype_TextChanged(sender As Object, e As EventArgs) Handles eq_filter_eqtype.TextChanged
        eqtype_filter_delay.Interval = 700
        eqtype_filter_delay.Stop()
        eqtype_filter_delay.Start()
    End Sub
    'THIS IS TO DELAY THE FILTER WHEN the user types so that it won't be laggy
    Private Sub eqtype_filter_delay_Tick(sender As Object, e As EventArgs) Handles eqtype_filter_delay.Tick
        eqtype_filter_delay.Stop()
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim SDA As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource
        Try
            MysqlConn.Open()

            query = "SELECT equipmentno AS 'Equipment Number', equipment AS 'Equipment', equipmentsn AS 'Serial Number',equipmenttype AS 'Equipment Type', equipmentlocation AS 'Equipment Location',equipmentowner AS 'Owner',equipmentstatus AS 'Status' FROM equipments ORDER BY equipmentno DESC"

            comm = New MySqlCommand(query, MysqlConn)
            SDA.SelectCommand = comm
            SDA.Fill(dbdataset)
            bsource.DataSource = dbdataset
            eq_rgv_showregequipment.DataSource = bsource
            SDA.Update(dbdataset)

            MysqlConn.Close()

        Catch ex As MySqlException
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try

        Dim DV As New DataView(dbdataset)
        DV.RowFilter = String.Format("`Equipment Number` Like'%{0}%' and `Equipment Type` Like'%{1}%' and `Status` Like'%{2}%' ", eq_filter_eqno.Text, eq_filter_eqtype.Text, eq_filter_eqstatus.Text)
        eq_rgv_showregequipment.DataSource = DV
    End Sub

    'Equipment Management Codes Umali E9 = SEARCH BY EQ_STATUS
    Private Sub eq_filter_eqstatus_TextChanged(sender As Object, e As EventArgs) Handles eq_filter_eqstatus.TextChanged
        eqstatus_filter_delay.Interval = 700
        eqstatus_filter_delay.Stop()
        eqstatus_filter_delay.Start()
    End Sub

    Private Sub eqstatus_filter_delay_Tick(sender As Object, e As EventArgs) Handles eqstatus_filter_delay.Tick
        eqstatus_filter_delay.Stop()
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim SDA As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource

        If eq_filter_eqstatus.Text = "-" Then
            load_eq_table()
        Else
            Try
                MysqlConn.Open()

                query = "SELECT equipmentno AS 'Equipment Number', equipment AS 'Equipment', equipmentsn AS 'Serial Number',equipmenttype AS 'Equipment Type', equipmentlocation AS 'Equipment Location',equipmentowner AS 'Owner',equipmentstatus AS 'Status' FROM equipments ORDER BY equipmentno DESC"

                comm = New MySqlCommand(query, MysqlConn)
                SDA.SelectCommand = comm
                SDA.Fill(dbdataset)
                bsource.DataSource = dbdataset
                eq_rgv_showregequipment.DataSource = bsource
                SDA.Update(dbdataset)

                MysqlConn.Close()

            Catch ex As MySqlException
                RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()
            End Try

            Dim DV As New DataView(dbdataset)
            DV.RowFilter = String.Format("`Equipment Number` Like'%{0}%' and `Equipment Type` Like'%{1}%' and `Status` Like'%{2}%' ", eq_filter_eqno.Text, eq_filter_eqtype.Text, eq_filter_eqstatus.Text)
            eq_rgv_showregequipment.DataSource = DV
        End If
    End Sub

    'Equipment Management Codes Umali E10 = COUNTER
    Private Sub eq_counter_type_SelectedIndexChanged_1(sender As Object, e As UI.Data.PositionChangedEventArgs) Handles eq_counter_type.SelectedIndexChanged
        counter_of_total_eq()
    End Sub

    'Equipment Management Codes Umali E11 = COUNTER CODE
    Public Sub counter_of_total_eq()
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If
        MysqlConn.ConnectionString = connstring
        Try
            MysqlConn.Open()

            Dim holder As String

            query = "SELECT COUNT(equipmenttype) AS 'total' FROM equipments WHERE equipmenttype=@eq_countertype"
            comm = New MySqlCommand(query, MysqlConn)
            comm.Parameters.AddWithValue("eq_countertype", eq_counter_type.Text)

            reader = comm.ExecuteReader
            While reader.Read
                holder = reader.GetString("total")
            End While
            eq_total_units.Text = holder

            MysqlConn.Close()

        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()

        End Try
    End Sub


    ' Reservation Management Code Umali R1 = LOADING DATA TO rec_eq_type_choose
    Public Sub rec_load_choices_eqtype()
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If
        MysqlConn.ConnectionString = connstring
        Try
            MysqlConn.Open()
            query = "SELECT DISTINCT(equipmenttype) FROM equipments ORDER BY equipmenttype ASC"
            comm = New MySqlCommand(query, MysqlConn)
            reader = comm.ExecuteReader


            rec_eq_type_choose.Items.Clear()
            eq_counter_type.Items.Clear()

            While reader.Read

                rec_eq_type_choose.Items.Add(reader.GetString("equipmenttype"))
                eq_counter_type.Items.Add(reader.GetString("equipmenttype"))
            End While
            MysqlConn.Close()
        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()

        End Try
    End Sub

    ' Reservation Management Code Umali R2 = LOADING DATA TO rec_eq_choosesn 
    Private Sub rec_eq_type_choose_SelectedIndexChanged(sender As Object, e As UI.Data.PositionChangedEventArgs) Handles rec_eq_type_choose.SelectedIndexChanged
        rec_eq_chooseno.Items.Clear()
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If
        MysqlConn.ConnectionString = connstring

        Try
            MysqlConn.Open()

            query = "SELECT equipmentno FROM equipments WHERE equipmenttype=@rec_eq_type_choose"
            comm = New MySqlCommand(query, MysqlConn)
            comm.Parameters.AddWithValue("rec_eq_type_choose", rec_eq_type_choose.Text)
            reader = comm.ExecuteReader

            rec_eq_chooseno.Items.Clear()
            While reader.Read
                rec_eq_chooseno.Items.Add(reader.GetString("equipmentno"))
            End While

            MysqlConn.Close()
        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try

    End Sub

    ' Reservation Management Code Umali R2.5 = LOADING DATA TO rec_eq_chooseeq 
    Private Sub rec_eq_chooseno_SelectedIndexChanged(sender As Object, e As UI.Data.PositionChangedEventArgs) Handles rec_eq_chooseno.SelectedIndexChanged
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If
        MysqlConn.ConnectionString = connstring
        Try
            MysqlConn.Open()
            query = "SELECT equipment from equipments where equipmenttype=@rec_eq_type_choose and equipmentno=@rec_eq_chooseno"
            comm = New MySqlCommand(query, MysqlConn)
            comm.Parameters.AddWithValue("rec_eq_type_choose", rec_eq_type_choose.Text)
            comm.Parameters.AddWithValue("rec_eq_chooseno", rec_eq_chooseno.Text)


            reader = comm.ExecuteReader

            rec_eq_chooseeq.Items.Clear()
            While reader.Read
                rec_eq_chooseeq.Items.Add(reader.GetString("equipment"))
            End While

            MysqlConn.Close()
        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try
    End Sub



    'Reservation Management Code Umali R3 = Adding Data from combobox to eq_rgv_addeq RadDataGrid
    Private Sub rec_btn_add_eq_Click(sender As Object, e As EventArgs) Handles rec_btn_add_eq.Click

        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If
        MysqlConn.ConnectionString = connstring

        If rec_eq_chooseno.Text = "" Then
            RadMessageBox.Show(Me, "Choose Serial Number!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        ElseIf rec_eq_type_choose.Text = "" Then
            RadMessageBox.Show(Me, "Choose the Type of Equipment!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        ElseIf rec_eq_chooseeq.Text = "" Then
            RadMessageBox.Show(Me, "Choose the Equipment!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Else
            Try
                MysqlConn.Open()
                query = "SELECT equipmentsn as 'Serial Number', equipmentno as '#' from equipments where equipmenttype=@rec_eq_type_choose and equipment=@rec_chooseeq and equipmentno=@rec_eq_chooseno"
                comm = New MySqlCommand(query, MysqlConn)
                comm.Parameters.AddWithValue("rec_eq_type_choose", rec_eq_type_choose.Text)
                comm.Parameters.AddWithValue("rec_eq_chooseno", rec_eq_chooseno.Text)
                comm.Parameters.AddWithValue("rec_chooseeq", rec_eq_chooseeq.Text)

                reader = comm.ExecuteReader

                Dim count As Integer
                count = 0
                Dim tempsn As String
                Dim tempno As String
                While reader.Read
                    tempsn = reader.GetString("Serial Number")
                    tempno = reader.GetString("#")
                End While

                eq_rgv_addeq.Rows.Add(tempno, rec_eq_chooseeq.Text, tempsn)

                rowcounter += 1

                MysqlConn.Close()
            Catch ex As Exception
                RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()
            End Try

        End If

    End Sub

    'Reservation Management Code Umali R4 = Removing data from eq_rgv_addeq RadDataGrid
    Private Sub rec_del_eq_Click(sender As Object, e As EventArgs) Handles rec_del_eq.Click

        If (eq_rgv_addeq.Rows.Count = 0) Then
            RadMessageBox.Show(Me, "The Table is already empty.", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Else
            eq_rgv_addeq.Rows.RemoveAt(eq_rgv_addeq.CurrentRow.Index)
        End If

    End Sub

    'Reservation Management Code Umali R5 = Checking the availabity of the equipment
    Private Sub rec_btn_check_availability_Click(sender As Object, e As EventArgs) Handles rec_btn_check_availability.Click
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If
        MysqlConn.ConnectionString = connstring
        rowcounter = 0

        Try
            MysqlConn.Open()
            query = "SELECT equipmentno,equipment,equipmentsn from reservation where equipmentno=@rows_eqno and equipment=@rows_eq and equipmentsn=@rows_eqsn"
            comm = New MySqlCommand(query, MysqlConn)
            comm.Parameters.AddWithValue("rows_eqno", eq_rgv_addeq.Rows(eq_rgv_addeq.SelectedRows(0).Index).Cells(0).Value.ToString())
            comm.Parameters.AddWithValue("rows_eq", eq_rgv_addeq.Rows(eq_rgv_addeq.SelectedRows(0).Index).Cells(1).Value.ToString())
            comm.Parameters.AddWithValue("rows_eqsn", eq_rgv_addeq.Rows(eq_rgv_addeq.SelectedRows(0).Index).Cells(2).Value.ToString())


            reader = comm.ExecuteReader

            Dim count As Integer
            count = 0


            While reader.Read
                count += 1
            End While

            If count = 1 Then
                RadMessageBox.Show(Me, "Item Not Available", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Else
                RadMessageBox.Show(Me, "Item Available", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info)
            End If
            MysqlConn.Close()
        Catch ex As ArgumentOutOfRangeException
            RadMessageBox.Show(Me, "No equipments to check.", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()

        End Try
    End Sub




    'Reservation Management Code Umali R6 = Inserting Data to database
    'Under observation because of bugs that can be found in the future ' as of 09.06.16 4:55pm'

    Private Sub rec_btn_save_Click(sender As Object, e As EventArgs) Handles rec_btn_save.Click
        reserveYN = RadMessageBox.Show(Me, "Are you sure you want to reserve?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If reserveYN = MsgBoxResult.Yes Then



            MysqlConn = New MySqlConnection
            MysqlConn.ConnectionString = connstring
            Dim READER As MySqlDataReader

            Dim conflictequipmentno As String = ""
            Dim conflictequipment As String = ""
            Dim conflictequipmentsn As String = ""

            If (rec_cb_reserveno.Text = "") Or (rec_cb_idnum.Text = "") Or (rec_cb_borrower.Text = "") Or (rec_dtp_date.Text = "") Or (rec_dtp_starttime.Text = "") Or (rec_dtp_endtime.Text = "") Or (rec_cb_college_school.Text = "") Or (rec_cb_location.Text = "") Or (rec_cb_status.Text = "") Or (rec_eq_chooseno.Text = "") Or (rec_eq_type_choose.Text = "") Or (eq_rgv_addeq.Rows.Count < 0) Or (rec_cb_acttype.Text = "") Then
                RadMessageBox.Show(Me, "Please complete the fields", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Else
                'ADD THE CHECKBOX TO PICK IF IT IS FROM ANOTHER DAY, ANOTHER DATE PICKER, BUT BY DEFAULT IT IS NOT CHECKED
                Dim elapsedTime As TimeSpan = DateTime.Parse(Format(CDate(rec_dtp_date.Value), "yyyy-MM-dd") & " " & rec_dtp_endtime.Text).Subtract(DateTime.Parse(DateTime.Parse(Format(CDate(rec_dtp_date.Value), "yyyy-MM-dd") & " " & rec_dtp_starttime.Text)))
                'ADD THE CHECKBOX TO PICK IF IT IS FROM ANOTHER DAY, ANOTHER DATE PICKER, BUT BY DEFAULT IT IS NOT CHECKED
                If elapsedTime.CompareTo(TimeSpan.Zero) <= 0 Then
                    RadMessageBox.Show(Me, "The Starting Time can't be the same or later on the Ending Time.", "Reservation", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1)
                Else
                    Dim counter As Integer
                    Dim rownumber As Integer = eq_rgv_addeq.Rows.Count
                    counter = 0
                    Dim errorcount As Boolean = False
                    If rownumber > 0 Then
                        While counter <> rownumber

                            Dim equipmentnorgv As String = eq_rgv_addeq.Rows(counter).Cells(0).Value
                            Dim equipmentrgv As String = eq_rgv_addeq.Rows(counter).Cells(1).Value
                            Dim equipmentsnrgv As String = eq_rgv_addeq.Rows(counter).Cells(2).Value

                            Try

                                MysqlConn.Close()
                                MysqlConn.Open()

                                'Modified with params and nonparams
                                query = "SELECT * FROM reservation WHERE equipment=@RE_equipment AND equipmentsn=@RE_equipmentsn AND equipmentno=@RE_equipmentno AND (('" & Format(CDate(rec_dtp_date.Value), "yyyy-MM-dd") & " " & Format(CDate(rec_dtp_starttime.Text), "HH:mm:01") & "' BETWEEN CONCAT(date,' ',starttime) AND CONCAT(date,' ',endtime)) OR ('" & Format(CDate(rec_dtp_date.Value), "yyyy-MM-dd") & " " & Format(CDate(rec_dtp_endtime.Text), "HH:mm:01") & "' BETWEEN CONCAT(date,' ',starttime) AND CONCAT (date,' ',endtime))) "


                                'Modified with Complete Params
                                'query = "SELECT * FROM reservation WHERE (equipment=@RE_equipment OR equipmentsn=@RE_equipmentsn OR equipmentno=@RE_equipmentno) AND (('@RE_date @RE_starttime' BETWEEN CONCAT(date,'',starttime) AND CONCAT(date,'',endtime)) OR ('@RE_date @RE_endtime' BETWEEN CONCAT(date,' ',starttime) AND CONCAT (date,' ',endtime))) "

                                'Original
                                'query = "Select * from reservation where reservationno=@RE_reservationno And equipment=@RE_equipment And equipmentsn=@RE_equipmentsn And equipmentno=@RE_equipmentno And Date=@RE_date And starttime=@RE_starttime And endtime=@RE_endtime"
                                comm = New MySqlCommand(query, MysqlConn)
                                comm.Parameters.AddWithValue("RE_reservationno", rec_cb_reserveno.Text)
                                comm.Parameters.AddWithValue("RE_equipment", equipmentrgv)
                                comm.Parameters.AddWithValue("RE_equipmentsn", equipmentsnrgv)
                                comm.Parameters.AddWithValue("RE_equipmentno", equipmentnorgv)
                                comm.Parameters.AddWithValue("RE_date", Format(CDate(rec_dtp_date.Value), "yyyy-MM-dd"))
                                comm.Parameters.AddWithValue("RE_starttime", Format(CDate(rec_dtp_starttime.Text), "HH:mm"))
                                comm.Parameters.AddWithValue("RE_endtime", Format(CDate(rec_dtp_endtime.Text), "HH:mm"))

                                READER = comm.ExecuteReader
                                Dim count As Integer
                                count = 0
                                While READER.Read
                                    count = count + 1
                                    equipmentnorgv = READER.GetString("equipmentno")
                                    equipmentrgv = READER.GetString("equipment")
                                    equipmentsnrgv = READER.GetString("equipmentsn")

                                End While

                                If count > 0 Then
                                    RadMessageBox.Show(Me, "The equipment " & equipmentrgv & " with serial number of " & equipmentsnrgv & " is already taken", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
                                    errorcount = True
                                    Exit While
                                Else

                                    MysqlConn.Close()
                                    MysqlConn.Open()
                                    query = "INSERT INTO `reservation` VALUES ('" & rec_cb_reserveno.Text & "','" & equipmentnorgv & "', '" & equipmentrgv & "', '" & equipmentsnrgv & "', '" & rec_cb_idnum.Text & "', '" & Format(CDate(rec_dtp_date.Value), "yyyy-MM-dd") & "','" & Format(CDate(rec_dtp_starttime.Text), "HH:mm") & "', '" & Format(CDate(rec_dtp_endtime.Text), "HH:mm") & "', '" & rec_cb_borrower.Text & "', '" & rec_cb_location.Text & "' , '" & lbl_nameofstaff_reserved.Text & "' ,'" & rec_cb_status.Text & "','" & rec_cb_acttype.Text & "','" & rec_rrtc_actname.Text & "')
                            ;INSERT INTO `reservation_equipments` VALUES ('" & rec_cb_reserveno.Text & "','" & equipmentnorgv & "', '" & equipmentrgv & "', '" & equipmentsnrgv & "')"

                                    comm = New MySqlCommand(query, MysqlConn)
                                    READER = comm.ExecuteReader
                                    MysqlConn.Close()

                                End If

                            Catch ex As Exception
                                RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
                            Finally
                                MysqlConn.Dispose()
                            End Try
                            counter = counter + 1


                        End While
                        eq_rgv_addeq.Rows.Clear()
                        rowcounter = 0
                    End If

                    If errorcount = False Then
                        RadMessageBox.Show(Me, "Succesfully Equipment Reserved!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info)

                    Else
                        RadMessageBox.Show(Me, "Not Successfully Reserved!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)

                    End If
                End If
            End If
            main_load_academicsonly()
            main_load_schoolonly()
            load_rec_table()
            reserved_load_table()
        End If
    End Sub

    'Showing All Data to Reservation Grid View
    Private Sub rec_btn_showalldata_Click(sender As Object, e As EventArgs) Handles rec_btn_showalldata.Click
        load_rec_table()
    End Sub

    'SHowing all available equipments to Reservation Grid View
    'Currently Pending because of importance
    Private Sub rec_btn_showavailequip_Click(sender As Object, e As EventArgs) Handles rec_btn_showavailequip.Click
        'MysqlConn = New MySqlConnection
        'MysqlConn.ConnectionString = connstring

        'Dim sda As New MySqlDataAdapter
        'Dim dbdataset As New DataTable
        'Dim bsource As New BindingSource

        'If MysqlConn.State = ConnectionState.Open Then
        '    MysqlConn.Close()
        'End If

        'Try
        '    MysqlConn.Open()
        '    'Relacing on how to show the taken equipments
        '    query = "SELECT equipmenttype AS 'Equipment Type',equipmentno AS 'Equipment No.',equipment AS 'Equipment', equipmentsn AS 'Equipment Serial' FROM equipments  ORDER BY equipmenttype ASC"

        '    comm = New MySqlCommand(query, MysqlConn)
        '    sda.SelectCommand = comm
        '    sda.Fill(dbdataset)
        '    bsource.DataSource = dbdataset
        '    reservation_rgv_recordeddata.DataSource = bsource
        '    reservation_rgv_recordeddata.ReadOnly = True
        '    sda.Update(dbdataset)
        '    MysqlConn.Close()

        'Catch ex As Exception
        '    RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        'Finally
        '    MysqlConn.Dispose()

        'End Try
    End Sub

    'Showing all Taken equipments to Reservation Grid View
    Private Sub rec_btn_showalltakeneq_Click(sender As Object, e As EventArgs) Handles rec_btn_showalltakeneq.Click
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring

        Dim sda As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource

        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        Try
            MysqlConn.Open()
            'Pending Changes to show all taken equipments
            query = "SELECT reservationno as 'Reservation Number',equipmentno AS 'Equipment No.',equipment AS 'Equipment', equipmentsn AS 'Equipment Serial' FROM reservation_equipments"

            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            reservation_rgv_recordeddata.DataSource = bsource
            reservation_rgv_recordeddata.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()

        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()

        End Try
    End Sub

    'Clearing of the Equipment Choice
    Private Sub rec_btn_eqclear_Click(sender As Object, e As EventArgs) Handles rec_btn_eqclear.Click
        rec_eq_type_choose.Text = ""
        rec_eq_chooseno.Text = ""
        rec_eq_chooseeq.Text = ""

    End Sub

    'AutoGenerating of Reservation#
    Public Sub auto_generate_reservationno()
        identifier_reservationno = Now.ToString("mmddyyy" + "-")
        identifier_reservationno = identifier_reservationno + random.Next(1, 100000000).ToString
        rec_cb_reserveno.Text = identifier_reservationno
    End Sub

    'Deletion of data in Reservation Page
    Private Sub rec_btn_delete_Click(sender As Object, e As EventArgs) Handles rec_btn_delete.Click
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        deleteYN = RadMessageBox.Show(Me, "Are you sure you want to delete?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If deleteYN = MsgBoxResult.Yes Then
            Try
                MysqlConn.Open()
                query = "DELETE FROM reservation WHERE (reservationno=@R_rec_cb_reserveno) AND (id=@R_rec_cb_idnum)"
                comm = New MySqlCommand(query, MysqlConn)
                comm.Parameters.AddWithValue("R_rec_cb_reserveno", rec_cb_reserveno.Text)
                comm.Parameters.AddWithValue("R_rec_cb_idnum", rec_cb_idnum.Text)

                reader = comm.ExecuteReader


                RadMessageBox.Show(Me, "Successfully Deleted!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info)
                MysqlConn.Close()
            Catch ex As Exception
                RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()
            End Try
        End If
        load_rec_table()
        main_load_academicsonly()
        main_load_schoolonly()
    End Sub

    'Combining (Fname,Lname) in Borrower Field in Reservation
    Private Sub rec_cb_idnum_TextChanged(sender As Object, e As EventArgs) Handles rec_cb_idnum.TextChanged
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If
        MysqlConn.ConnectionString = connstring
        Try
            MysqlConn.Open()
            query = "SELECT CONCAT (bor_surname,', ',bor_fname) as 'Name' FROM borrowers_reg WHERE bor_id=@bor_idno"
            comm = New MySqlCommand(query, MysqlConn)
            comm.Parameters.AddWithValue("bor_idno", rec_cb_idnum.Text)
            reader = comm.ExecuteReader

            rec_cb_borrower.Items.Clear()
            While reader.Read
                rec_cb_borrower.Items.Add(reader.GetString("name"))
            End While

            MysqlConn.Close()
        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try
    End Sub


    'Clearing of Fields in Reservation Page
    Private Sub rec_btn_clear_Click(sender As Object, e As EventArgs) Handles rec_btn_clear.Click
        auto_generate_reservationno()
        rec_cb_borrower.Text = ""
        rec_cb_idnum.Text = ""
        rec_cb_acttype.Text = ""
        rec_dtp_date.Value = Date.Now
        rec_cb_status.Text = ""
        rec_dtp_starttime.Text = ""
        rec_dtp_endtime.Text = ""
        rec_cb_college_school.Text = ""
        rec_cb_location.Text = ""
        rec_cb_status.Text = ""
        rec_eq_chooseeq.Text = ""
        rec_eq_chooseno.Text = ""
        rec_eq_type_choose.Text = ""
        eq_rgv_addeq.Rows.Clear()
        rec_rrtc_actname.Text = ""
    End Sub

    'Loading of data in Reservation Page Grid
    Private Sub rec_dtp_date_ValueChanged(sender As Object, e As EventArgs) Handles rec_dtp_date.ValueChanged
        load_rec_table()
    End Sub

    'Loading of data in Main Page Grid
    Private Sub lu_date_filter_delay_Tick(sender As Object, e As EventArgs) Handles lu_date_filter_delay.Tick
        lu_date_filter_delay.Stop()
        main_load_academicsonly()
        main_load_schoolonly()
    End Sub

    Private Sub lu_date_ValueChanged(sender As Object, e As EventArgs) Handles lu_date.ValueChanged
        lu_date_filter_delay.Interval = 500
        lu_date_filter_delay.Stop()
        lu_date_filter_delay.Start()
    End Sub

    'Double Click function in Reservation Tab Page
    Private Sub reservation_rgv_recordeddata_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles reservation_rgv_recordeddata.CellDoubleClick
        If e.RowIndex >= 0 Then
            Dim row As Telerik.WinControls.UI.GridViewRowInfo

            row = Me.reservation_rgv_recordeddata.Rows(e.RowIndex)

            rec_cb_reserveno.Text = row.Cells("Reservation Number").Value.ToString
            rec_cb_idnum.Text = row.Cells("ID").Value.ToString.ToString
        End If

        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        deleteYN = RadMessageBox.Show(Me, "Are you sure you want to delete?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If deleteYN = MsgBoxResult.Yes Then
            Try
                MysqlConn.Open()
                query = "DELETE FROM reservation WHERE (reservationno=@R_rec_cb_reserveno) AND (id=@R_rec_cb_idnum)"
                comm = New MySqlCommand(query, MysqlConn)
                comm.Parameters.AddWithValue("R_rec_cb_reserveno", rec_cb_reserveno.Text)
                comm.Parameters.AddWithValue("R_rec_cb_idnum", rec_cb_idnum.Text)

                reader = comm.ExecuteReader


                RadMessageBox.Show(Me, "Successfully Deleted!", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info)
                MysqlConn.Close()
            Catch ex As Exception
                RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()
            End Try
        End If
        load_rec_table()
        main_load_academicsonly()
        main_load_schoolonly()
    End Sub


    'Auto Generating of Reservation Number
    Private Sub btn_resetreservationno_Click(sender As Object, e As EventArgs) Handles btn_resetreservationno.Click
        auto_generate_reservationno()
    End Sub

    'Timer Code
    Private Sub Main_Timer_Tick(sender As Object, e As EventArgs) Handles Main_Timer.Tick
        Dim title As String = "CEU TLTD Reservation System"
        Me.Text = title + Date.Now.ToString("            MMMM dd, yyyy  hh:mm:ss tt")
    End Sub

    'DropDown Code for other
    Private Sub eq_btn_other_Click(sender As Object, e As EventArgs) Handles eq_btn_other.Click
        eq_type.DropDownStyle = RadDropDownStyle.DropDown
    End Sub

    'Load eqtype
    Public Sub load_cb_eq_type()
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If
        MysqlConn.ConnectionString = connstring
        Try
            MysqlConn.Open()
            query = "SELECT DISTINCT(equipmenttype) from equipments"
            comm = New MySqlCommand(query, MysqlConn)
            reader = comm.ExecuteReader

            eq_type.Items.Clear()
            While reader.Read
                eq_type.Items.Add(reader.GetString("equipmenttype"))
            End While

            MysqlConn.Close()
        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try
    End Sub

    Private Sub rpv1_Click(sender As Object, e As EventArgs) Handles rpv1.Click
        'load_main_table()
        'load_rec_table()
    End Sub


    Private Sub rec_cb_acttype_SelectedIndexChanged(sender As Object, e As UI.Data.PositionChangedEventArgs) Handles rec_cb_acttype.SelectedIndexChanged
        If rec_cb_acttype.Text = "School Activity" Then
            rec_rrtc_actname.Enabled = True
            rec_lbl_actname.Enabled = True
        Else
            rec_rrtc_actname.Enabled = False
            rec_lbl_actname.Enabled = False
            rec_rrtc_actname.Text = ""
        End If

    End Sub

     Private Sub return_btn_returned_Click(sender As Object, e As EventArgs) Handles return_btn_returned.Click
        returnEquipYN = RadMessageBox.Show(Me, "Are you sure you want to return this equipment?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        Dim serverdatetime As String
        If returnEquipYN = MsgBoxResult.Yes Then
            'GET SERVERTIME
            Try
                MysqlConn.Open()
                Dim q As String = "SELECT date_format(now(), '%Y-%m-%d %H:%i') As SERVERTIME"
                comm = New MySqlCommand(q, MysqlConn)
                reader = comm.ExecuteReader
                While reader.Read
                    serverdatetime = reader.GetString("SERVERTIME")
                End While
                MysqlConn.Close()
            Catch ex As MySqlException
                MessageBox.Show(ex.Message)
            Finally
                MysqlConn.Dispose()
            End Try
            'END GET SERVERTIME

            Dim releasing_table_date As String = Format(CDate(ret_tb_sdate.Value), "yyyy-MM-dd")
            Dim releasing_table_endtime As String = ret_tb_etime.Text

            Dim elapsedTime As TimeSpan = DateTime.Parse(serverdatetime).Subtract(DateTime.Parse(releasing_table_date & " " & releasing_table_endtime))

            If elapsedTime.CompareTo(TimeSpan.Zero) < 0 Then
                Try
                    If MysqlConn.State = ConnectionState.Open Then
                        MysqlConn.Close()
                    End If
                    MysqlConn.Open()
                    Dim Query As String = "INSERT INTO ceutltdscheduler.returned_info (ret_reservation_num,ret_id_passnum,ret_borrower,ret_equipment_no,ret_equipment,ret_assign_date,ret_starttime,ret_endtime,ret_status,ret_releasedby,ret_returnedto,ret_remarks) VALUES(@resno,@borrowerid,@borrowername,@eqno,@eqname,@resdate,@stime,@etime,'Returned',@staff_releaser,@staff_returner,@remarks); DELETE FROM ceutltdscheduler.released_info WHERE rel_id_passnum=@borrowerid and rel_reservation_no=@resno and rel_borrower=@borrowername and rel_equipment_no=@eqno and rel_equipment=@eqname and rel_assign_date=@resdate and rel_starttime=@stime and rel_endtime=@etime and rel_releasedby=@staff_releaser"
                    comm = New MySqlCommand(Query, MysqlConn)
                    comm.Parameters.AddWithValue("@resno", ret_tb_reservationnum.Text)
					comm.Parameters.AddWithValue("@borrowerid", ret_tb_id.Text)
                    comm.Parameters.AddWithValue("@borrowername", ret_tb_borrower.Text)
                    comm.Parameters.AddWithValue("@eqno", ret_tb_equipmentnum.Text)
                    comm.Parameters.AddWithValue("@eqname", ret_tb_equipment.Text)
                    comm.Parameters.AddWithValue("@resdate", Format(CDate(ret_tb_sdate.Value), "yyyy-MM-dd"))
                    comm.Parameters.AddWithValue("@stime", ret_tb_stime.Text)
                    comm.Parameters.AddWithValue("@etime", ret_tb_etime.Text)
					comm.Parameters.AddWithValue("@staff_releaser",ret_nameofstaff_release2.Text)
                    comm.Parameters.AddWithValue("@staff_returner", ret_nameofstaff_return.Text)
					comm.Parameters.AddWithValue("@remarks", ret_remarks.Text)
                    comm.ExecuteNonQuery()
                    MysqlConn.Close()

                Catch ex As MySqlException
                    MessageBox.Show(ex.Message)
                Finally
                    MysqlConn.Dispose()
                    load_released_list2()
                    load_returned_eq_list()
                End Try
                RadMessageBox.Show(Me, "Congratulations!, The Item is returned early.", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info, MessageBoxDefaultButton.Button1)
            Else
                Dim seconds As Integer = (elapsedTime.TotalSeconds)
                Dim counter As Integer = 1
                Dim charge As Integer = 0
                Dim graceperiod As Integer =  Convert.ToInt32(penalty_graceperiod)
                Dim chargeinterval As Integer = Convert.ToInt32(penalty_chargeinterval)
                While counter <= seconds
                    counter += 1
                    If counter Mod chargeinterval = 0 And counter>=graceperiod Then 'GRACE PERIOD Convert.toInt32(string_from_DB)
                        charge += 1
                    End If
                End While
                If charge = 0 Then
                    Try
                        If MysqlConn.State = ConnectionState.Open Then
                            MysqlConn.Close()
                        End If
                         MysqlConn.Open()
                    Dim Query As String = "INSERT INTO ceutltdscheduler.returned_info (ret_reservation_num,ret_id_passnum,ret_borrower,ret_equipment_no,ret_equipment,ret_assign_date,ret_starttime,ret_endtime,ret_status,ret_releasedby,ret_returnedto,ret_remarks) VALUES(@resno,@borrowerid,@borrowername,@eqno,@eqname,@resdate,@stime,@etime,'Returned',@staff_releaser,@staff_returner,@remarks); DELETE FROM ceutltdscheduler.released_info WHERE rel_id_passnum=@borrowerid and rel_reservation_no=@resno and rel_borrower=@borrowername and rel_equipment_no=@eqno and rel_equipment=@eqname and rel_assign_date=@resdate and rel_starttime=@stime and rel_endtime=@etime and rel_releasedby=@staff_releaser"
                    comm = New MySqlCommand(Query, MysqlConn)
                    comm.Parameters.AddWithValue("@resno", ret_tb_reservationnum.Text)
					comm.Parameters.AddWithValue("@borrowerid", ret_tb_id.Text)
                    comm.Parameters.AddWithValue("@borrowername", ret_tb_borrower.Text)
                    comm.Parameters.AddWithValue("@eqno", ret_tb_equipmentnum.Text)
                    comm.Parameters.AddWithValue("@eqname", ret_tb_equipment.Text)
                    comm.Parameters.AddWithValue("@resdate", Format(CDate(ret_tb_sdate.Value), "yyyy-MM-dd"))
                    comm.Parameters.AddWithValue("@stime", ret_tb_stime.Text)
                    comm.Parameters.AddWithValue("@etime", ret_tb_etime.Text)
					comm.Parameters.AddWithValue("@staff_releaser",ret_nameofstaff_release2.Text)
                    comm.Parameters.AddWithValue("@staff_returner", ret_nameofstaff_return.Text)
					comm.Parameters.AddWithValue("@remarks", ret_remarks.Text)
                    comm.ExecuteNonQuery()
                    MysqlConn.Close()

                    Catch ex As MySqlException
                        MessageBox.Show(ex.Message)
                    Finally
                        MysqlConn.Dispose()
                        load_released_list2()
                        load_returned_eq_list()
                    End Try
                    RadMessageBox.Show(Me, "Congratulations! The item is returned on time." & Environment.NewLine & String.Format("Exceeded Time: {0:%m} minutes(s)", elapsedTime), "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1)
                ElseIf charge > 0 Then
                    If seconds >= 86400 Then
                        Try
                            If MysqlConn.State = ConnectionState.Open Then
                                MysqlConn.Close()
                            End If
                            MysqlConn.Open()
                            Dim Query As String = "INSERT INTO ceutltdscheduler.penalties (bor_id,bor_name,eq_no,eq_name,res_date,st_time,ed_time,bor_price,ret_mark,ret_date) VALUES(@borrowerid,@borrowername,@eqno,@eqname,@resdate,@stime,@etime,@price,@staff_returner,DATE_FORMAT(now(), '%Y-%m-%d %H:%i'));INSERT INTO ceutltdscheduler.returned_info (ret_reservation_num,ret_id_passnum,ret_borrower,ret_equipment_no,ret_equipment,ret_assign_date,ret_starttime,ret_endtime,ret_status,ret_releasedby,ret_returnedto,ret_remarks) VALUES(@resno,@borrowerid,@borrowername,@eqno,@eqname,@resdate,@stime,@etime,'Returned',@staff_releaser,@staff_returner,@remarks)"'; DELETE FROM ceutltdscheduler.released_info WHERE rel_id_passnum=@borrowerid and rel_reservation_no=@resno and rel_borrower=@borrowername and rel_equipment_no=@eqno and rel_equipment=@eqname and rel_assign_date=@resdate and rel_starttime=@stime and rel_endtime=@etime and rel_releasedby=@staff_releaser"
                            comm = New MySqlCommand(Query, MysqlConn)
							comm.Parameters.AddWithValue("@resno", ret_tb_reservationnum.Text)
                            comm.Parameters.AddWithValue("@borrowerid", ret_tb_id.Text)
                            comm.Parameters.AddWithValue("@borrowername", ret_tb_borrower.Text)
                            comm.Parameters.AddWithValue("@eqno", ret_tb_equipmentnum.Text)
                            comm.Parameters.AddWithValue("@eqname", ret_tb_equipment.Text)
                            comm.Parameters.AddWithValue("@resdate", Format(CDate(ret_tb_sdate.Value), "yyyy-MM-dd"))
                            comm.Parameters.AddWithValue("@stime", ret_tb_stime.Text)
                            comm.Parameters.AddWithValue("@etime", ret_tb_etime.Text)
                            comm.Parameters.AddWithValue("@price", Convert.ToDouble(charge * penalty_price).ToString)
							comm.Parameters.AddWithValue("@staff_releaser",ret_nameofstaff_release2.Text)
                            comm.Parameters.AddWithValue("@staff_returner", ret_nameofstaff_return.Text)
							comm.Parameters.AddWithValue("@remarks", ret_remarks.Text)
                            comm.ExecuteNonQuery()
                            MysqlConn.Close()
                        Catch ex As MySqlException
                            MessageBox.Show(ex.Message)
                        Finally
                            MysqlConn.Dispose()
                            load_penalty_list()
                            load_released_list2()
                            load_returned_eq_list()
                        End Try
                        RadMessageBox.Show(Me, "Time Exceeded!!" & Environment.NewLine & Environment.NewLine & String.Format("Exceeding Time: {0:%d} day(s)", elapsedTime) & String.Format(" {0:%h} hours(s) ", elapsedTime) & String.Format("{0:%m} minutes(s)", elapsedTime) & Environment.NewLine & "Charge is: " & Convert.ToDouble(charge * penalty_price) & " pesos.", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1)
                    Else
                        Try
                            If MysqlConn.State = ConnectionState.Open Then
                                MysqlConn.Close()
                            End If
                            MysqlConn.Open()
                            Dim Query As String = "INSERT INTO ceutltdscheduler.penalties (bor_id,bor_name,eq_no,eq_name,res_date,st_time,ed_time,bor_price,ret_mark,ret_date) VALUES(@borrowerid,@borrowername,@eqno,@eqname,@resdate,@stime,@etime,@price,@staff_returner,DATE_FORMAT(now(), '%Y-%m-%d %H:%i'));INSERT INTO ceutltdscheduler.returned_info (ret_reservation_num,ret_id_passnum,ret_borrower,ret_equipment_no,ret_equipment,ret_assign_date,ret_starttime,ret_endtime,ret_status,ret_releasedby,ret_returnedto,ret_remarks) VALUES(@resno,@borrowerid,@borrowername,@eqno,@eqname,@resdate,@stime,@etime,'Returned',@staff_releaser,@staff_returner,@remarks)"'; DELETE FROM ceutltdscheduler.released_info WHERE rel_id_passnum=@borrowerid and rel_reservation_no=@resno and rel_borrower=@borrowername and rel_equipment_no=@eqno and rel_equipment=@eqname and rel_assign_date=@resdate and rel_starttime=@stime and rel_endtime=@etime and rel_releasedby=@staff_releaser"
                            comm = New MySqlCommand(Query, MysqlConn)
							comm.Parameters.AddWithValue("@resno", ret_tb_reservationnum.Text)
                            comm.Parameters.AddWithValue("@borrowerid", ret_tb_id.Text)
                            comm.Parameters.AddWithValue("@borrowername", ret_tb_borrower.Text)
                            comm.Parameters.AddWithValue("@eqno", ret_tb_equipmentnum.Text)
                            comm.Parameters.AddWithValue("@eqname", ret_tb_equipment.Text)
                            comm.Parameters.AddWithValue("@resdate", Format(CDate(ret_tb_sdate.Value), "yyyy-MM-dd"))
                            comm.Parameters.AddWithValue("@stime", ret_tb_stime.Text)
                            comm.Parameters.AddWithValue("@etime", ret_tb_etime.Text)
                            comm.Parameters.AddWithValue("@price", Convert.ToDouble(charge * penalty_price).ToString)
							comm.Parameters.AddWithValue("@staff_releaser",ret_nameofstaff_release2.Text)
                            comm.Parameters.AddWithValue("@staff_returner", ret_nameofstaff_return.Text)
							comm.Parameters.AddWithValue("@remarks", ret_remarks.Text)
                            comm.ExecuteNonQuery()
                            MysqlConn.Close()
                        Catch ex As MySqlException
                            MessageBox.Show(ex.Message)
                        Finally
                            MysqlConn.Dispose()
                            load_penalty_list()
							load_released_list2()
                            load_returned_eq_list()
							'load_return_info() 'NAWAWALA PAH
                        End Try
                        RadMessageBox.Show(Me, "Time Exceeded!!" & Environment.NewLine & Environment.NewLine & String.Format("Exceeding Time: {0:%h} hours(s) ", elapsedTime) & String.Format("{0:%m} minutes(s)", elapsedTime) & Environment.NewLine & "Charge is: " & Convert.ToDouble(charge * penalty_price) & " pesos.", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1)
                    End If
                End If
            End If
        Else
            released_grid_list2.Focus()
        End If
    End Sub

     'Penalty GRIDVIEW Controls
    Public Sub load_penalty_list()
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring

        Dim sda As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        

        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        Try
            MysqlConn.Open()
            Dim query As String
            'query = "Select rel_reservation_no as 'Reservation Number' , rel_id_passnum as 'Pass Number' , rel_borrower as 'Borrower' , rel_equipment_no as 'Equipment No' , rel_equipment as 'Equipment' , DATE_FORMAT(rel_assign_date,'%M %d %Y') as 'Date',TIME_FORMAT(rel_starttime, '%H:%i') as 'Start Time', TIME_FORMAT(rel_endtime, '%H:%i') as 'End Time' , rel_status as 'Status' , rel_releasedby as 'Released By'  from released_info"
            query = "SELECT pen_id as 'Penalty ID',bor_id as 'Pass #', bor_name as 'Borrower Name', eq_no as 'Equipment Number', eq_name as 'Equipment Name', DATE_FORMAT(res_date,'%M %d %Y') as 'Reservation Date', TIME_FORMAT(st_time, '%H:%i') as 'Start Time', TIME_FORMAT(ed_time, '%H:%i') as 'End Time', bor_price as 'Price', ret_mark as 'Marked Returned By', DATE_FORMAT(ret_date, '%M %d %Y %H:%i') as 'Return Date' FROM ceutltdscheduler.penalties"
            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bdsrc_penaltylist.DataSource = dbdataset
            penalty_grid_list.DataSource = bdsrc_penaltylist
            penalty_grid_list.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()
            'penalty_grid_list.Columns("Penalty ID").IsVisible = false 'SHOW LATER
        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try
    End Sub
    Private Sub penalty_grid_list_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles penalty_grid_list.CellDoubleClick
        penaltiesDeleteYN = RadMessageBox.Show(Me, "Are you sure you want to delete the selected data?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If penaltiesDeleteYN = MsgBoxResult.Yes Then
            Dim uniqueid As String
            Dim row As GridViewRowInfo = penalty_grid_list.Rows(e.RowIndex)
            uniqueid = row.Cells("Penalty ID").Value.ToString
            Try
                Dim query = "DELETE FROM ceutltdscheduler.penalties where pen_id=" & uniqueid
                MysqlConn.Open()
                comm = New MySqlCommand(query, MysqlConn)
                comm.ExecuteNonQuery()
                MysqlConn.Close()
                RadMessageBox.Show(Me, "Selected penalty sucessfully deleted.", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info)
            Catch ex As MySqlException
                RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()
                load_penalty_list()
            End Try
        End If
    End Sub
    Private Sub penalty_grid_list_sort(sender as object, e as GridViewCollectionChangedEventArgs)  Handles penalty_grid_list.SortChanged
        Dim sorts As RadSortExpressionCollection = penalty_grid_list.MasterTemplate.SortDescriptors
        If sorts.Count = 0
          bdsrc_penaltylist.Sort = ""
        Else
        Dim sort as string = sorts.ToString()
     
    if (sort <> Me.bdsrc_penaltylist.Sort)
        me.bdsrc_penaltylist.Sort = sort   
    End If 
 End If
End Sub
    Private Sub penalty_grid_list_ContextMenuOpening(sender As Object, e As ContextMenuOpeningEventArgs) Handles penalty_grid_list.ContextMenuOpening
        If TypeOf Me.penalty_grid_list.CurrentRow Is GridViewDataRowInfo Then
            Dim menu As New RadDropDownMenu()
            Dim DeleteMenu As New RadMenuItem("Delete Selected Data")
            AddHandler DeleteMenu.Click, AddressOf penalty_grid_list_DeleteRightClick
            menu.Items.Add(DeleteMenu)
            e.ContextMenu = menu
        End If
    End Sub
    Private Sub penalty_grid_list_DeleteRightClick(sender As Object, e As EventArgs)
        penaltiesDeleteYN = RadMessageBox.Show(Me, "Are you sure you want to delete the selected data?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If penaltiesDeleteYN = MsgBoxResult.Yes Then
            Dim ligaw As Integer = 0
            Dim uniqueid As String
            Dim morethanOneSelected = False
            Dim query = "DELETE FROM ceutltdscheduler.penalties where pen_id="
            For Each row As GridViewRowInfo In penalty_grid_list.SelectedRows
                row = penalty_grid_list.Rows(penalty_grid_list.SelectedRows(ligaw).Index)
                uniqueid = row.Cells("Penalty ID").Value.ToString
                If penalty_grid_list.SelectedRows.Count > 1 Then
                    query += uniqueid
                    query += " or "
                    query += "pen_id="
                    morethanOneSelected = True
                Else
                    query += uniqueid
                End If
                ligaw += 1
            Next
            Try
                If morethanOneSelected Then
                    query = (query.Remove(query.Length - 11, 11))
                Else
                End If
                MysqlConn.Open()
                comm = New MySqlCommand(query, MysqlConn)
                comm.ExecuteNonQuery()
                MysqlConn.Close()
                RadMessageBox.Show(Me, "Selected penalties sucessfully deleted.", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info)
            Catch ex As MySqlException
                RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()
                load_penalty_list()
            End Try
        End If
    End Sub
    Private Sub penalty_grid_list_KeyPress(sender As Object, e As KeyEventArgs) Handles penalty_grid_list.KeyDown
         If e.KeyCode = Keys.Delete Then
        penaltiesDeleteYN = RadMessageBox.Show(Me, "Are you sure you want to delete the selected data?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If penaltiesDeleteYN = MsgBoxResult.Yes Then
            Dim ligaw As Integer = 0
            Dim uniqueid As String
            Dim morethanOneSelected = False
            Dim query = "DELETE FROM ceutltdscheduler.penalties where pen_id="
            For Each row As GridViewRowInfo In penalty_grid_list.SelectedRows
                row = penalty_grid_list.Rows(penalty_grid_list.SelectedRows(ligaw).Index)
                uniqueid = row.Cells("Penalty ID").Value.ToString
                If penalty_grid_list.SelectedRows.Count > 1 Then
                    query += uniqueid
                    query += " or "
                    query += "pen_id="
                    morethanOneSelected = True
                Else
                    query += uniqueid
                End If
                ligaw += 1
            Next
            Try
                If morethanOneSelected Then
                    query = (query.Remove(query.Length - 11, 11))
                Else
                End If
                MysqlConn.Open()
                comm = New MySqlCommand(query, MysqlConn)
                comm.ExecuteNonQuery()
                MysqlConn.Close()
                RadMessageBox.Show(Me, "Selected penalties sucessfully deleted.", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info)
            Catch ex As MySqlException
                RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()
                load_penalty_list()
            End Try

        End If
            End If
    End Sub

 
    'Returned Equipment GRIDVIEW Controls
    Public Sub load_returned_eq_list()
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring

        Dim sda As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        

        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        Try
            MysqlConn.Open()
            Dim query As String
            'query = "Select rel_reservation_no as 'Reservation Number' , rel_id_passnum as 'Pass Number' , rel_borrower as 'Borrower' , rel_equipment_no as 'Equipment No' , rel_equipment as 'Equipment' , DATE_FORMAT(rel_assign_date,'%M %d %Y') as 'Date',TIME_FORMAT(rel_starttime, '%H:%i') as 'Start Time', TIME_FORMAT(rel_endtime, '%H:%i') as 'End Time' , rel_status as 'Status' , rel_releasedby as 'Released By'  from released_info"
            query = "SELECT ret_id as 'Return ID',ret_reservation_num as 'Reservation Number', ret_id_passnum as 'Pass #', ret_borrower as 'Borrower', ret_equipment_no as 'Equipment Number', ret_equipment as 'Equipment Name', DATE_FORMAT(ret_assign_date,'%M %d, %Y') as 'Assigned Date', TIME_FORMAT(ret_starttime, '%H:%i') as 'Start Time', TIME_FORMAT(ret_endtime, '%H:%i') as 'End Time', ret_releasedby as 'Released By', ret_returnedto as 'Returned To', ret_remarks as 'Remarks' FROM ceutltdscheduler.returned_info"
            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bdsrc_returnedeq.DataSource = dbdataset
            returned_eq_list.DataSource = bdsrc_returnedeq
            returned_eq_list.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()
            'returned_eq_list.Columns("Return ID").IsVisible = false 'SHOW LATER
        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try
    End Sub
    Private Sub returned_eq_list_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles returned_eq_list.CellDoubleClick
        returned_eqDeleteYN = RadMessageBox.Show(Me, "Are you sure you want to delete the selected log?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If returned_eqDeleteYN = MsgBoxResult.Yes Then
            Dim uniqueid As String
            Dim row As GridViewRowInfo = returned_eq_list.Rows(e.RowIndex)
            uniqueid = row.Cells("Return ID").Value.ToString
            Try
                Dim query = "DELETE FROM ceutltdscheduler.returned_info where ret_id=" & uniqueid
                MysqlConn.Open()
                comm = New MySqlCommand(query, MysqlConn)
                comm.ExecuteNonQuery()
                MysqlConn.Close()
                RadMessageBox.Show(Me, "Selected log sucessfully deleted.", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info)
            Catch ex As MySqlException
                RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()
                load_returned_eq_list()
            End Try
        End If
    End Sub
    Private Sub returned_eq_list_sort(sender as object, e as GridViewCollectionChangedEventArgs)  Handles returned_eq_list.SortChanged
        Dim sorts As RadSortExpressionCollection = returned_eq_list.MasterTemplate.SortDescriptors
        If sorts.Count = 0
          bdsrc_returnedeq.Sort = ""
        Else
        Dim sort as string = sorts.ToString()
     
    if (sort <> Me.bdsrc_returnedeq.Sort)
        me.bdsrc_returnedeq.Sort = sort   
    End If 
 End If
End Sub
    Private Sub returned_eq_list_ContextMenuOpening(sender As Object, e As ContextMenuOpeningEventArgs) Handles returned_eq_list.ContextMenuOpening
        If TypeOf Me.returned_eq_list.CurrentRow Is GridViewDataRowInfo Then
            Dim menu As New RadDropDownMenu()
            Dim DeleteMenu As New RadMenuItem("Delete Selected Data")
            AddHandler DeleteMenu.Click, AddressOf returned_eq_list_DeleteRightClick
            menu.Items.Add(DeleteMenu)
            e.ContextMenu = menu
        End If
    End Sub
    Private Sub returned_eq_list_DeleteRightClick(sender As Object, e As EventArgs)
        returned_eqDeleteYN = RadMessageBox.Show(Me, "Are you sure you want to delete the selected data?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If returned_eqDeleteYN = MsgBoxResult.Yes Then
            Dim ligaw As Integer = 0
            Dim uniqueid As String
            Dim morethanOneSelected = False
            Dim query = "DELETE FROM ceutltdscheduler.returned_info where ret_id="
            For Each row As GridViewRowInfo In returned_eq_list.SelectedRows
                row = returned_eq_list.Rows(returned_eq_list.SelectedRows(ligaw).Index)
                uniqueid = row.Cells("Return ID").Value.ToString
                If returned_eq_list.SelectedRows.Count > 1 Then
                    query += uniqueid
                    query += " or "
                    query += "ret_id="
                    morethanOneSelected = True
                Else
                    query += uniqueid
                End If
                ligaw += 1
            Next
            Try
                If morethanOneSelected Then
                    query = (query.Remove(query.Length - 11, 11))
                Else
                End If
                MysqlConn.Open()
                comm = New MySqlCommand(query, MysqlConn)
                comm.ExecuteNonQuery()
                MysqlConn.Close()
                RadMessageBox.Show(Me, "Selected logs sucessfully deleted.", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info)
            Catch ex As MySqlException
                RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()
                load_returned_eq_list()
            End Try
        End If
    End Sub
    Private Sub returned_eq_list_KeyPress(sender As Object, e As KeyEventArgs) Handles returned_eq_list.KeyDown
         If e.KeyCode = Keys.Delete Then
            returned_eqDeleteYN = RadMessageBox.Show(Me, "Are you sure you want to delete the selected data?", "CEU TLTD Reservation System", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If returned_eqDeleteYN = MsgBoxResult.Yes Then
            Dim ligaw As Integer = 0
            Dim uniqueid As String
            Dim morethanOneSelected = False
            Dim query = "DELETE FROM ceutltdscheduler.returned_info where ret_id="
            For Each row As GridViewRowInfo In returned_eq_list.SelectedRows
                row = returned_eq_list.Rows(returned_eq_list.SelectedRows(ligaw).Index)
                uniqueid = row.Cells("Return ID").Value.ToString
                If returned_eq_list.SelectedRows.Count > 1 Then
                    query += uniqueid
                    query += " or "
                    query += "ret_id="
                    morethanOneSelected = True
                Else
                    query += uniqueid
                End If
                ligaw += 1
            Next
            Try
                If morethanOneSelected Then
                    query = (query.Remove(query.Length - 11, 11))
                Else
                End If
                MysqlConn.Open()
                comm = New MySqlCommand(query, MysqlConn)
                comm.ExecuteNonQuery()
                MysqlConn.Close()
                RadMessageBox.Show(Me, "Selected logs sucessfully deleted.", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Info)
            Catch ex As MySqlException
                RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()
                load_returned_eq_list()
            End Try
        End If
      End If
    End Sub




    Private Sub btn_gotoinsmat_Click(sender As Object, e As EventArgs) Handles btn_gotoinsmat.Click
        InstructionalMaterials.Show()
        Me.Hide()

    End Sub





    'PENDING CHANGES
    'WILL I CHANGE BORROWER TO BORROWER ID#?





    '[FUTURE REFERENCE] 'Reservation Management Code Umali R5.5 = Checking the availabity of the equipment using CellClick

    'Private Sub eq_rgv_addeq_CellClick(sender As Object, e As GridViewCellEventArgs) Handles eq_rgv_addeq.CellClick
    '    If MysqlConn.State = ConnectionState.Open Then
    '        MysqlConn.Close()
    '    End If
    '    MysqlConn.ConnectionString = connstring


    '    Try
    '        MysqlConn.Open()
    '        query = "Select equipment,equipmentsn from reservation_equipment where equipment='" & eq_rgv_addeq.Rows(e.RowIndex).Cells(0).Value & "' and equipmentsn='" & eq_rgv_addeq.Rows(e.RowIndex).Cells(1).Value & "' "
    '        comm = New MySqlCommand(query, MysqlConn)
    '        reader = comm.ExecuteReader

    '        Dim count As Integer
    '        count = 0


    '        While reader.Read
    '            count += 1
    '        End While

    '        If count = 1 Then
    '            RadMessageBox.Show(Me, "Item Not Available", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
    '        Else
    '            RadMessageBox.Show(Me, "Item Available", "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
    '        End If

    '    Catch ex As Exception
    '        RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
    '    Finally
    '        MysqlConn.Dispose()

    '    End Try
    'End Sub




    ''TEST CODES

    'Private Sub eq_type_SelectedIndexChanged(sender As Object, e As UI.Data.PositionChangedEventArgs)
    '    If MysqlConn.State = ConnectionState.Open Then
    '        MysqlConn.Close()
    '    End If
    '    MysqlConn.ConnectionString = connstring
    '    Try
    '        MysqlConn.Open()
    '        query = "SELECT * from equipments where equipmenttype='" & eq_type.Text & "'"
    '        comm = New MySqlCommand(query, MysqlConn)
    '        reader = comm.ExecuteReader

    '        Dim count As Integer
    '        count = 0
    '        eq_equipment.Items.Clear()
    '        While reader.Read

    '            eq_equipment.Items.Add(reader.GetString("equipment"))

    '        End While
    '    Catch ex As Exception
    '        RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
    '    Finally
    '        MysqlConn.Dispose()

    '    End Try
    'End Sub

    'Private Sub eq_equipment_SelectedIndexChanged(sender As Object, e As UI.Data.PositionChangedEventArgs)
    '    If MysqlConn.State = ConnectionState.Open Then
    '        MysqlConn.Close()
    '    End If
    '    MysqlConn.ConnectionString = connstring
    '    Try
    '        MysqlConn.Open()
    '        query = "SELECT * from equipments where equipmenttype='" & eq_type.Text & "' and equipment='" & eq_equipment.Text & "' "
    '        comm = New MySqlCommand(query, MysqlConn)
    '        reader = comm.ExecuteReader

    '        Dim count As Integer
    '        count = 0
    '        eq_equipmentloction.Items.Clear()

    '        While reader.Read
    '            eq_equipmentloction.Items.Add(reader.GetString("equipmentlocation"))
    '        End While

    '    Catch ex As Exception
    '        RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
    '    Finally
    '        MysqlConn.Dispose()

    '    End Try
    'End Sub

    'Private Sub eq_equipmentlocation_SelectedIndexChanged(sender As Object, e As UI.Data.PositionChangedEventArgs)
    '    If MysqlConn.State = ConnectionState.Open Then
    '        MysqlConn.Close()
    '    End If
    '    MysqlConn.ConnectionString = connstring
    '    Try
    '        MysqlConn.Open()
    '        query = "SELECT * from equipments where equipmenttype='" & eq_type.Text & "' and equipment='" & eq_equipment.Text & "' and equipmentlocation='" & eq_equipmentloction.Text & "' "
    '        comm = New MySqlCommand(query, MysqlConn)
    '        reader = comm.ExecuteReader

    '        Dim count As Integer
    '        count = 0
    '        eq_owner.Items.Clear()

    '        While reader.Read
    '            eq_owner.Items.Add(reader.GetString("equipmentowner"))
    '        End While

    '    Catch ex As Exception
    '        RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
    '    Finally
    '        MysqlConn.Dispose()

    '    End Try
    'End Sub

    'Private Sub eq_owner_SelectedIndexChanged(sender As Object, e As UI.Data.PositionChangedEventArgs)
    '    If MysqlConn.State = ConnectionState.Open Then
    '        MysqlConn.Close()
    '    End If
    '    MysqlConn.ConnectionString = connstring
    '    Try
    '        MysqlConn.Open()
    '        query = "SELECT * from equipments where equipmenttype='" & eq_type.Text & "' and equipment='" & eq_equipment.Text & "' and equipmentlocation='" & eq_equipmentloction.Text & "' and equipmentowner='" & eq_owner.Text & "' "
    '        comm = New MySqlCommand(query, MysqlConn)
    '        reader = comm.ExecuteReader

    '        Dim count As Integer
    '        count = 0
    '        eq_status.Items.Clear()

    '        While reader.Read
    '            eq_status.Items.Add(reader.GetString("equipmentstatus"))
    '        End While

    '    Catch ex As Exception
    '        RadMessageBox.Show(Me, ex.Message, "CEU TLTD Reservation System", MessageBoxButtons.OK, RadMessageIcon.Error)
    '    Finally
    '        MysqlConn.Dispose()

    '    End Try
    'End Sub


End Class
