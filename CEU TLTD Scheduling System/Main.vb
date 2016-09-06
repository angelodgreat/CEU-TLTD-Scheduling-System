﻿Imports MySql.Data.MySqlClient
Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.UI.Data

Public Class Main

    Dim ds As New DataSet
    Dim MysqlConn As MySqlConnection

    Dim dbDataSet As New DataTable
    Dim svYN As DialogResult
    Dim addYN As DialogResult
    Dim editYN As DialogResult
    Dim cancelYN As DialogResult
    Dim updateYN As DialogResult
    Dim deleteYN As DialogResult
    Dim doneYN As DialogResult

    Public identifier_reservationno As String
    Public random As System.Random = New System.Random

    Public equipment As String
    Public rowcounter As Integer = 0
    Dim query As String


    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        reservation_rgv_recordeddata.Show()
        reservations_rgv_showavailableitems.Hide()
        load_main_table()
        load_rec_table()
        load_eq_table()
        load_main_acc()
        load_main_prof()
        startup_disabled_buttons()
        load_released_list()
        load_released_list2()
        load_returned_list()
        rec_load_choices_eqtype()
        auto_generate_reservationno()
        reserved_load_table()
        startup_disabled_textbox()
        show_hide_txt_lbl()
        color_coding()
        lu_date.Value = Date.Now
        rec_dtp_date.Value = Date.Now
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
        rel_tb_equipmentnum.Hide()
        rel_tb_equipment.Hide()


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
    Private Sub eq_rgv_showregequipment_ViewCellFormatting(sender As Object, e As CellFormattingEventArgs) Handles eq_rgv_showregequipment.ViewCellFormatting
        e.CellElement.TextAlignment = ContentAlignment.MiddleCenter
    End Sub

    Private Sub main_rgv_recordeddatamain_ViewCellFormatting(sender As Object, e As CellFormattingEventArgs) Handles main_rgv_recordeddatamain.ViewCellFormatting
        e.CellElement.TextAlignment = ContentAlignment.MiddleCenter
    End Sub

    Private Sub reservation_rgv_recordeddata_ViewCellFormatting(sender As Object, e As CellFormattingEventArgs) Handles reservation_rgv_recordeddata.ViewCellFormatting
        e.CellElement.TextAlignment = ContentAlignment.MiddleCenter
    End Sub

    Public Sub load_rec_table()
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

            query = "Select reservationno as 'Reservation Number' ,borrower as 'Borrower',id as 'ID', equipmentno as 'Equipment No', equipment as 'Equipment', DATE_FORMAT(date,'%M %d %Y') as 'Date',TIME_FORMAT(starttime, '%H:%i') as 'Start Time', TIME_FORMAT(endtime, '%H:%i') as 'End Time' from reservation where date ='" & Format(CDate(rec_dtp_date.Value), "yyyy-MM-dd") & "' ORDER by date ASC"

            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            reservation_rgv_recordeddata.DataSource = bsource
            reservation_rgv_recordeddata.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()

        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()

        End Try
    End Sub

    Public Sub load_main_table()
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

            query = "Select borrower as 'Borrower',id as 'ID', equipmentno as 'Equipment No', equipment as 'Equipment', DATE_FORMAT(date,'%M %d %Y') as 'Date',TIME_FORMAT(starttime, '%H:%i') as 'Start Time', TIME_FORMAT(endtime, '%H:%i') as 'End Time' from reservation where date='" & Format(CDate(lu_date.Value), "yyyy-MM-dd") & "' ORDER BY starttime ASC"

            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            main_rgv_recordeddatamain.DataSource = bsource
            main_rgv_recordeddatamain.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()

        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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
            query = "Select prof_id as 'Professor ID' , prof_fname as 'First Name' , prof_mname as 'Middle Name' , prof_surname as 'Surname' , prof_college as 'College/School' , prof_type as 'User Type'  from prof_reg"
            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            acc_prof_list.DataSource = bsource
            acc_prof_list.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()

        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()

        End Try

    End Sub

    'Programmed by BRENZ THIRD POINT SAVE BUTTON

    Private Sub acc_staff_btn_save_Click(sender As Object, e As EventArgs) Handles acc_staff_btn_save.Click
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim READER As MySqlDataReader
        If (acc_sf_id.Text = "") Or (acc_sf_fname.Text = "") Or (acc_sf_mname.Text = " ") Or (acc_sf_lname.Text = " ") Or (acc_sf_usertype.Text = " ") Or (acc_sf_username.Text = " ") Then
            RadMessageBox.Show(Me, "Please complete the fields to Save!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        Else
            Try
                MysqlConn.Open()
                Dim Query As String
                Query = "insert into ceutltdscheduler.staff_reg (staff_id,staff_fname,staff_mname,staff_surname,staff_type,staff_username,staff_password) values ('" & acc_sf_id.Text & "' , '" & acc_sf_fname.Text & "', '" & acc_sf_mname.Text & "', '" & acc_sf_lname.Text & "' ,  '" & acc_sf_usertype.Text & "' , '" & acc_sf_username.Text & "' , sha2('" & acc_sf_password.Text & "', 512))"
                comm = New MySqlCommand(Query, MysqlConn)

                svYN = RadMessageBox.Show(Me, "Are you sure you want To save this information? ", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
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
        updateYN = RadMessageBox.Show(Me, "Do you want to select this information?", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
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

        updateYN = RadMessageBox.Show(Me, "Do you want to update the selected Information?", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If updateYN = MsgBoxResult.Yes Then
            If (acc_sf_id.Text = "") Or (acc_sf_fname.Text = "") Or (acc_sf_mname.Text = " ") Or (acc_sf_lname.Text = " ") Or (acc_sf_usertype.Text = " ") Or (acc_sf_username.Text = " ") Then
                RadMessageBox.Show(Me, "Please complete the fields to update!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
            Else
                Try
                    MysqlConn.Open()
                    query = "UPDATE staff_reg set staff_id = '" & acc_sf_id.Text & "' , staff_fname = '" & acc_sf_fname.Text & "' , staff_mname = '" & acc_sf_mname.Text & "' , staff_surname = '" & acc_sf_lname.Text & "' , staff_type = '" & acc_sf_usertype.Text & "' , staff_username = '" & acc_sf_username.Text & "' where staff_id = '" & acc_sf_id.Text & "' "
                    comm = New MySqlCommand(query, MysqlConn)
                    reader = comm.ExecuteReader

                    RadMessageBox.Show(Me, "Update Success!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Info)
                    MysqlConn.Close()


                Catch ex As Exception
                    RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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

        deleteYN = RadMessageBox.Show(Me, "Are you sure you want To Delete this information? ", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
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
        doneYN = RadMessageBox.Show(Me, "Are you sure you want to clear the fields?", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
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
        If (acc_pf_id.Text = "") Or (acc_pf_fname.Text = "") Or (acc_pf_mname.Text = " ") Or (acc_pf_lname.Text = " ") Or (acc_pf_college.Text = " ") Or (acc_pf_usertype.Text = " ") Then
            RadMessageBox.Show(Me, "Please complete the fields to Save!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        Else
            Try
                MysqlConn.Open()
                Dim Query As String
                Query = "insert into ceutltdscheduler.prof_reg (prof_id,prof_fname,prof_mname,prof_surname,prof_college,prof_type) values ('" & acc_pf_id.Text & "' , '" & acc_pf_fname.Text & "' , '" & acc_pf_mname.Text & "' , '" & acc_pf_lname.Text & "' , '" & acc_pf_college.Text & "' , '" & acc_pf_usertype.Text & "')"
                comm = New MySqlCommand(Query, MysqlConn)

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
                acc_pf_usertype.Text = " "

            End Try
        End If
    End Sub



    'Programmed by Brenz 9th point Cell Double Click Prof List!
    Private Sub acc_prof_list_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles acc_prof_list.CellDoubleClick
        updateYN = RadMessageBox.Show(Me, "Do you want to select this information?", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If updateYN = MsgBoxResult.Yes Then

            If e.RowIndex >= 0 Then
                Dim row As Telerik.WinControls.UI.GridViewRowInfo

                row = Me.acc_prof_list.Rows(e.RowIndex)

                acc_pf_id.Text = row.Cells("Professor ID").Value.ToString
                acc_pf_fname.Text = row.Cells("First Name").Value.ToString
                acc_pf_mname.Text = row.Cells("Middle Name").Value.ToString
                acc_pf_lname.Text = row.Cells("Surname").Value.ToString
                acc_pf_college.Text = row.Cells("College/School").Value.ToString
                acc_pf_usertype.Text = row.Cells("User Type").Value.ToString

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

        updateYN = RadMessageBox.Show(Me, "Do you want to update the selected Information?", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If updateYN = MsgBoxResult.Yes Then
            If (acc_pf_id.Text = "") Or (acc_pf_fname.Text = "") Or (acc_pf_mname.Text = " ") Or (acc_pf_lname.Text = " ") Or (acc_pf_college.Text = " ") Or (acc_pf_usertype.Text = " ") Then
                RadMessageBox.Show(Me, "Please complete the fields to update!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
            Else
                Try
                    MysqlConn.Open()
                    query = "UPDATE prof_reg set prof_id = '" & acc_pf_id.Text & "' , prof_fname = '" & acc_pf_fname.Text & "' , prof_mname = '" & acc_pf_mname.Text & "' , prof_surname = '" & acc_pf_lname.Text & "' , prof_college = '" & acc_pf_college.Text & "' , prof_type = '" & acc_pf_usertype.Text & "' where prof_id = '" & acc_pf_id.Text & "' "
                    comm = New MySqlCommand(query, MysqlConn)
                    reader = comm.ExecuteReader

                    RadMessageBox.Show(Me, "Update Success!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Info)
                    MysqlConn.Close()


                Catch ex As Exception
                    RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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

        deleteYN = RadMessageBox.Show(Me, "Are you sure you want To Delete this information? ", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If deleteYN = MsgBoxResult.Yes Then
            Try
                MysqlConn.Open()
                Dim Query As String
                Query = "delete from prof_reg where prof_id = '" & acc_pf_id.Text & "'"
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
                acc_pf_usertype.Text = " "

            End Try
        End If



    End Sub

    'Programmed by Brenz 12th Point Clear Button
    Private Sub acc_prof_btn_clear_Click(sender As Object, e As EventArgs) Handles acc_prof_btn_clear.Click
        doneYN = RadMessageBox.Show(Me, "Are you sure you want to clear the fields?", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If doneYN = MsgBoxResult.Yes Then
            acc_pf_id.Text = " "
            acc_pf_fname.Text = " "
            acc_pf_mname.Text = " "
            acc_pf_lname.Text = " "
            acc_pf_college.Text = " "
            acc_pf_usertype.Text = " "

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
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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
            query = "Select rel_id_passnum as 'Pass Number ' , rel_borrower as ' Borrower ' , rel_equipment_no as ' Equipment No ' , rel_equipment as ' Equipment ' , DATE_FORMAT(rel_assign_date,'%M %d %Y') as 'Date',TIME_FORMAT(rel_starttime, '%H:%i') as 'Start Time', TIME_FORMAT(rel_endtime, '%H:%i') as 'End Time' , rel_status as ' Status ' , rel_releasedby as ' Released By'  from released_info"
            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            released_grid_list2.DataSource = bsource
            released_grid_list2.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()
        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try
    End Sub

    'Programmed by BRENZ 15th POINT Load form grid RETURNED at returning Management!
    Public Sub load_returned_list()
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
            query = "Select  ret_id_passnum as 'Pass Number ' , ret_borrower as ' Borrower ' , ret_equipment_no as 'Equipment Number', ret_equipment as 'Equipment', DATE_FORMAT(ret_assign_date,'%M %d %Y') as 'Date',TIME_FORMAT(ret_starttime, '%H:%i') as 'Start Time', TIME_FORMAT(ret_endtime, '%H:%i') as 'End Time' , ret_status as ' Status ' , ret_releasedby as ' Released By' , ret_returnedto as ' Returned To '  from returned_info"
            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            returned_grid_list.DataSource = bsource
            returned_grid_list.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()
        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try
    End Sub

    'Programmed by BRENZ 16th point RELEASE BTN at Releasing Management!
    Private Sub released_btn_release_Click(sender As Object, e As EventArgs) Handles released_btn_release.Click

        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim READER As MySqlDataReader
        If (rel_tb_id.Text = "") Or (rel_tb_reservationnum.Text = "") Or (rel_tb_borrower.Text = "") Or (rel_tb_equipmentnum.Text = "") Or (rel_tb_equipment.Text = "") Or (rel_tb_startdate.Text = " ") Or (rel_tb_starttime.Text = " ") Or (rel_tb_endtime.Text = " ") Or (rel_tb_releasedby.Text = " ") Then
            RadMessageBox.Show(Me, "Please complete the fields to Save!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        Else
            Try
                MysqlConn.Open()
                Dim Query As String
                ' Dim Query2 As String
                Query = "insert into `released_info`  values ('" & rel_tb_id.Text & "' ,'" & rel_tb_reservationnum.Text & " ',  '" & rel_tb_borrower.Text & "' , '" & rel_tb_reservationnum.Text & "', '" & rel_tb_equipment.Text & "', '" & Format(CDate(rel_tb_startdate.Value), "yyyy-MM-dd") & "','" & Format(CDate(rel_tb_starttime.Text), "HH:mm") & "', '" & Format(CDate(rel_tb_endtime.Text), "HH:mm") & "', '" & rel_tb_status.Text & "' , '" & rel_tb_releasedby.Text & "'); delete from reservation where  reservationno = '" & rel_tb_reservationnum.Text & "'"
                'Query = "delete from reservation where  reservationno = '" & rel_tb_reservationnum.Text & "'"
                comm = New MySqlCommand(Query, MysqlConn)

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
                load_released_list()
                load_released_list2()
                reserved_load_table()
                load_rec_table()
                color_coding()
            End Try
        End If

    End Sub


    'Programmed by BRENZ 17th Point UPDATE BTN at Releasing Management
    Private Sub released_btn_update_Click(sender As Object, e As EventArgs)
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        updateYN = RadMessageBox.Show(Me, "Do you want to Update the Date/Time/Location of the Reserved Equipment?", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If updateYN = MsgBoxResult.Yes Then
            If (rel_tb_startdate.Text = "") Or (rel_tb_starttime.Text = " ") Or (rel_tb_endtime.Text = " ") Then
                RadMessageBox.Show(Me, "Please complete the fields to update!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
            Else
                Try
                    MysqlConn.Open()
                    query = "UPDATE released_info set rel_assign_date = '" & rel_tb_starttime.Text & "'  , rel_starttime = '" & rel_tb_starttime.Text & "' , rel_endtime = '" & rel_tb_endtime.Text & "'  where rel_assign_date = '" & rel_tb_startdate.Text & "' "
                    comm = New MySqlCommand(query, MysqlConn)
                    reader = comm.ExecuteReader

                    RadMessageBox.Show(Me, "Update Success!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Info)
                    MysqlConn.Close()


                Catch ex As Exception
                    RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
                Finally
                    MysqlConn.Dispose()
                    load_released_list()
                    load_released_list2()
                End Try
            End If
        End If



    End Sub

    'Programmed by BRENZ 18th Point Cancel BTN at Returning Management
    Private Sub return_btn_cancel_Click(sender As Object, e As EventArgs) Handles return_btn_cancel.Click
        cancelYN = RadMessageBox.Show(Me, "Do you want to cancel returning? ", "TLTD Scheduling management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If cancelYN = MsgBoxResult.Yes Then
            ret_tb_id.Text = ""
            ret_tb_borrower.Text = ""
            ret_tb_sdate.Text = "01/01/99"
            ret_tb_stime.Text = ""
            ret_tb_etime.Text = ""
            ret_tb_status.Text = ""
            ret_tb_released.Text = ""
            rel_tb_returned.Text = ""
        End If
    End Sub


    'Programmed by BRENZ 19th Point Cancel BTN at Releasing Management
    Private Sub released_btn_cancel_Click(sender As Object, e As EventArgs) Handles released_btn_cancel.Click
        cancelYN = RadMessageBox.Show(Me, "Do you want to cancel returning? ", "TLTD Scheduling management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If cancelYN = MsgBoxResult.Yes Then
            rel_tb_id.Text = ""
            rel_tb_borrower.Text = ""
            rel_tb_startdate.Text = "01/01/99"
            rel_tb_starttime.Text = ""
            rel_tb_status.Text = ""
            rel_tb_releasedby.Text = ""
            rel_tb_equipment.Text = ""
            rel_tb_equipmentnum.Text = ""
            lbl_equipment.Hide()
            lbl_equipmentnum.Hide()
            rel_tb_equipment.Hide()
            rel_tb_equipmentnum.Hide()
            color_coding()



        End If




    End Sub

    'Programmed by BRENZ 20th Point reserved records at releasing management

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
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try

    End Sub

    'Programmed by BRENZ 21st Point reserved_grid_list cell double click at releasing management
    Private Sub reserved_grid_list_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles reserved_grid_list.CellDoubleClick
        updateYN = RadMessageBox.Show(Me, "Do you want to select this information?", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If updateYN = MsgBoxResult.Yes Then

            If e.RowIndex >= 0 Then
                Dim row As Telerik.WinControls.UI.GridViewRowInfo

                row = Me.reserved_grid_list.Rows(e.RowIndex)

                rel_tb_reservationnum.Text = row.Cells("Reservation Number").Value.ToString
                rel_tb_borrower.Text = row.Cells("Borrower").Value.ToString
                rel_tb_startdate.Text = row.Cells("Date").Value.ToString
                rel_tb_starttime.Text = row.Cells("Start Time").Value.ToString
                rel_tb_endtime.Text = row.Cells("End Time").Value.ToString
                rel_tb_status.Text = row.Cells("Status").Value.ToString
                rel_tb_equipmentnum.Text = row.Cells("Equipment No").Value.ToString
                rel_tb_equipment.Text = row.Cells("Equipment").Value.ToString


                lbl_equipment.Show()
                lbl_equipmentnum.Show()
                rel_tb_equipment.Show()
                rel_tb_equipmentnum.Show()
                reserved_load_table()
                color_coding()
            End If

        End If
    End Sub

    'Main Window Search Functions Umali C1


    Private Sub btn_searchbydate_Click(sender As Object, e As EventArgs) Handles btn_searchbydate.Click
        load_main_table()

        ''''''''Pending Changes because of client request

        'MysqlConn = New MySqlConnection
        'MysqlConn.ConnectionString = connstring
        'Dim SDA As New MySqlDataAdapter
        'Dim dbdataset As New DataTable
        'Dim bsource As New BindingSource

        'If (lu_startdate.Text = "") Or (lu_enddate.Text = "") Or (lu_starttime.Text = "") Or (lu_endtime.Text) Then
        '    RadMessageBox.Show(Me, "Please fill all fields to search.", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        'Else

        '    Try
        '        MysqlConn.Open()

        '        query = "select DATE_FORMAT(startdate,'%M %d %Y') as 'Start Date', DATE_FORMAT(enddate,'%M %d, %Y') as 
        '    'End Date', TIME_FORMAT(starttime, '%H:%i') as 'Start Time', TIME_FORMAT(endtime, '%H:%i') as 'End Time', borrower as 
        '    'Borrower',location as 'Location' from reservation
        '    where (((startdate between @startdate and @enddate and 
        '    (starttime between @starttime and @endtime)) or
        '    ((enddate between @startdate and @enddate) and
        '    (endtime between @endtime and @endtime))))"

        '        comm = New MySqlCommand(query, MysqlConn)
        '        comm.Parameters.AddWithValue("startdate", Format(CDate(lu_startdate.Value), "yyyy-MM-dd"))
        '        comm.Parameters.AddWithValue("enddate", Format(CDate(lu_enddate.Value), "yyyy-MM-dd"))
        '        comm.Parameters.AddWithValue("starttime", Format(CDate(lu_starttime.Text), "HH:mm"))
        '        comm.Parameters.AddWithValue("endtime", Format(CDate(lu_starttime.Text), "HH:mm"))


        '        SDA.SelectCommand = comm
        '        SDA.Fill(dbdataset)
        '        bsource.DataSource = dbdataset
        '        main_rgv_recordeddatamain.DataSource = bsource
        '        SDA.Update(dbdataset)

        '        MysqlConn.Close()

        '    Catch ex As MySqlException
        '        RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        '    Finally
        '        MysqlConn.Dispose()
        '    End Try

        'End If
    End Sub

    'Main Window Search Functions Umali C2

    Private Sub lu_bylocation_TextChanged(sender As Object, e As EventArgs) Handles lu_bylocation.TextChanged
        ''''''''Pending Changes because of client request


        'MysqlConn = New MySqlConnection
        'MysqlConn.ConnectionString = connstring
        'Dim SDA As New MySqlDataAdapter
        'Dim dbdataset As New DataTable
        'Dim bsource As New BindingSource
        'Try
        '    MysqlConn.Open()

        '    query = "SELECT DATE_FORMAT(startdate,'%M %d %Y') AS 'Start Date', DATE_FORMAT(enddate,'%M %d, %Y') AS 
        '    'End Date', TIME_FORMAT(starttime, '%H:%i') AS 'Start Time', TIME_FORMAT(endtime, '%H:%i') AS 'End Time', borrower AS 
        '    'Borrower',location AS 'Location', equipment AS 'Equipment' FROM reservation ORDER BY location desc"
        '    comm = New MySqlCommand(query, MysqlConn)
        '    SDA.SelectCommand = comm
        '    SDA.Fill(dbdataset)
        '    bsource.DataSource = dbdataset
        '    main_rgv_recordeddatamain.DataSource = bsource
        '    SDA.Update(dbdataset)

        '    MysqlConn.Close()

        'Catch ex As MySqlException
        '    RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        'Finally
        '    MysqlConn.Dispose()
        'End Try

        'Dim DV As New DataView(dbdataset)
        'DV.RowFilter = String.Format("`Location` Like'%{0}%'", lu_bylocation.Text)
        'main_rgv_recordeddatamain.DataSource = DV
    End Sub

    'Main Window Search Functions Umali C3
    Private Sub lu_byequipment_TextChanged(sender As Object, e As EventArgs) Handles lu_byequipment.TextChanged
        ''''''''Pending Changes because of client request

        'MysqlConn = New MySqlConnection
        'MysqlConn.ConnectionString = connstring
        'Dim SDA As New MySqlDataAdapter
        'Dim dbdataset As New DataTable
        'Dim bsource As New BindingSource
        'Try
        '    MysqlConn.Open()

        '    query = "SELECT DATE_FORMAT(startdate,'%M %d %Y') AS 'Start Date', DATE_FORMAT(enddate,'%M %d, %Y') AS 
        '    'End Date', TIME_FORMAT(starttime, '%H:%i') AS 'Start Time', TIME_FORMAT(endtime, '%H:%i') AS 'End Time', borrower AS 
        '    'Borrower',location AS 'Location', equipment AS 'Equipment' FROM reservation ORDER BY equipment desc"
        '    comm = New MySqlCommand(query, MysqlConn)
        '    SDA.SelectCommand = comm
        '    SDA.Fill(dbdataset)
        '    bsource.DataSource = dbdataset
        '    main_rgv_recordeddatamain.DataSource = bsource
        '    SDA.Update(dbdataset)

        '    MysqlConn.Close()

        'Catch ex As MySqlException
        '    RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        'Finally
        '    MysqlConn.Dispose()
        'End Try

        'Dim DV As New DataView(dbdataset)
        'DV.RowFilter = String.Format("`Equipment` Like'%{0}%'", lu_byequipment.Text)
        'main_rgv_recordeddatamain.DataSource = DV
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
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()

        End Try

    End Sub

    'Equipment Management Codes Umali E2 EQ_BTN_SAVE

    Private Sub eq_btn_save_Click(sender As Object, e As EventArgs) Handles eq_btn_save.Click
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim reader As MySqlDataReader


        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If
        If (eq_equipmentno.Text = "") Or (eq_sn.Text = "") Or (eq_equipment.Text = "") Or (eq_equipmentlocation.Text = "") Or (eq_owner.Text = "") Or (eq_status.Text = "") Then
            RadMessageBox.Show(Me, "Please complete the fields to update!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)

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
                    RadMessageBox.Show(Me, "Equipment #" & eq_equipmentno.Text & " And the equipment " & eq_equipment.Text & " Is already registered")
                Else
                    MysqlConn.Close()
                    MysqlConn.Open()

                    addYN = RadMessageBox.Show(Me, "Are you sure you want To save this equipment?", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
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
                        RadMessageBox.Show(Me, "Equipment Registered!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Info)
                        MysqlConn.Close()
                    End If
                End If
            Catch ex As Exception
                RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()

            End Try
        End If
        load_eq_table()
        counter_of_total_eq()
    End Sub


    'Equipment Management Codes Umali E3 EQ_BTN_UPDATE

    Private Sub eq_btn_update_Click(sender As Object, e As EventArgs) Handles eq_btn_update.Click
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        updateYN = RadMessageBox.Show(Me, "Do you want To update the selected equipment?", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Question)
        If updateYN = MsgBoxResult.Yes Then
            If (eq_equipmentno.Text = "") Or (eq_sn.Text = "") Or (eq_equipment.Text = "") Or (eq_equipmentlocation.Text = "") Or (eq_owner.Text = "") Or (eq_status.Text = "") Then
                RadMessageBox.Show(Me, "Please complete the fields To update!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
            Else
                Try
                    MysqlConn.Open()
                    query = "UPDATE equipments SET equipmentno=@eq_eqno, equipmentsn=@eq_eqsn, equipment=@eq_eqeq, equipmentlocation=@eq_eqlocation, equipmentowner=@eq_eqowner, equipmentstatus=@eq_eqstatus, equipmentype=@eq_eqtype WHERE (equipmentsn=@eq_eqsn) AND (equipmentno=@eq_eqno)"
                    comm = New MySqlCommand(query, MysqlConn)

                    comm.Parameters.AddWithValue("eq_eqno", eq_equipmentno.Text)
                    comm.Parameters.AddWithValue("eq_eqeq", eq_equipment.Text)
                    comm.Parameters.AddWithValue("eq_eqsn", eq_sn.Text)
                    comm.Parameters.AddWithValue("eq_eqlocation", eq_equipmentlocation.Text)
                    comm.Parameters.AddWithValue("eq_eqowner", eq_owner.Text)
                    comm.Parameters.AddWithValue("eq_eqstatus", eq_status.Text)
                    comm.Parameters.AddWithValue("eq_eqtype", eq_type.Text)


                    reader = comm.ExecuteReader

                    RadMessageBox.Show(Me, "Update Success!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Info)
                    MysqlConn.Close()


                Catch ex As Exception
                    RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
                Finally
                    MysqlConn.Dispose()
                End Try
            End If
        End If
        load_eq_table()
        counter_of_total_eq()
    End Sub

    'Equipment Management Codes Umali E4 EQ_BTN_DELETE

    Private Sub eq_btn_delete_Click(sender As Object, e As EventArgs) Handles eq_btn_delete.Click
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        deleteYN = RadMessageBox.Show(Me, "Are you sure you want to delete?", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
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

                RadMessageBox.Show(Me, "Successfully Deleted!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Info)
                MysqlConn.Close()
            Catch ex As Exception
                RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()
            End Try
        End If
        load_eq_table()
        counter_of_total_eq()
    End Sub

    'Equipment Management Codes Umali E5 EQ_CELLDOUBLECLICK

    Private Sub eq_rgv_showregequipment_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles eq_rgv_showregequipment.CellDoubleClick
        If e.RowIndex >= 0 Then
            Dim row As Telerik.WinControls.UI.GridViewRowInfo

            row = Me.eq_rgv_showregequipment.Rows(e.RowIndex)

            eq_equipmentno.Text = row.Cells("Equipment Number").Value.ToString
            eq_equipment.Text = row.Cells("Equipment").Value.ToString
            eq_equipmentlocation.Text = row.Cells("Equipment Location").Value.ToString
            eq_sn.Text = row.Cells("Serial Number").Value.ToString
            eq_status.Text = row.Cells("Status").Value.ToString
            eq_owner.Text = row.Cells("Owner").Value.ToString
            eq_type.Text = row.Cells("Equipment Type").Value.ToString

            eq_btn_update.Show()
            eq_btn_delete.Show()
            eq_btn_save.Hide()
        End If
    End Sub

    'Equipment Management Codes Umali E6 = EQ_BTN_CLEAR

    Private Sub eq_btn_clear_Click(sender As Object, e As EventArgs) Handles eq_btn_clear.Click
        eq_equipmentno.Text = ""
        eq_equipment.Text = ""
        eq_equipmentlocation.Text = ""
        eq_sn.Text = ""
        eq_status.Text = ""
        eq_owner.Text = ""
        eq_type.Text = ""

        eq_btn_update.Hide()
        eq_btn_delete.Hide()
        eq_btn_save.Show()
    End Sub

    'Equipment Management Codes Umali E7 = SEARCH BY EQ_NO
    Private Sub eq_filter_eqno_TextChanged(sender As Object, e As EventArgs) Handles eq_filter_eqno.TextChanged

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
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try

        Dim DV As New DataView(dbdataset)
        DV.RowFilter = String.Format("`Equipment Number`= '{0}'", eq_filter_eqno.Text)
        eq_rgv_showregequipment.DataSource = DV
    End Sub

    'Equipment Management Codes Umali E8 = SEARCH BY EQ_TYPE
    Private Sub eq_filter_eqtype_TextChanged(sender As Object, e As EventArgs) Handles eq_filter_eqtype.TextChanged
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
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try

        Dim DV As New DataView(dbdataset)
        DV.RowFilter = String.Format("`Equipment Type` Like'%{0}%'", eq_filter_eqtype.Text)
        eq_rgv_showregequipment.DataSource = DV
    End Sub

    'Equipment Management Codes Umali E9 = SEARCH BY EQ_STATUS
    Private Sub eq_filter_eqstatus_TextChanged(sender As Object, e As EventArgs) Handles eq_filter_eqstatus.TextChanged
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
                RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()
            End Try

            Dim DV As New DataView(dbdataset)
            DV.RowFilter = String.Format("`Status` Like'%{0}%'", eq_filter_eqstatus.Text)
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
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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
            While reader.Read

                rec_eq_type_choose.Items.Add(reader.GetString("equipmenttype"))

            End While
            MysqlConn.Close()
        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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
            RadMessageBox.Show(Me, "Choose Serial Number!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        ElseIf rec_eq_type_choose.Text = "" Then
            RadMessageBox.Show(Me, "Choose the Type of Equipment!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        ElseIf rec_eq_chooseeq.Text = "" Then
            RadMessageBox.Show(Me, "Choose the Equipment!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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
                RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()
            End Try

        End If

    End Sub

    'Reservation Management Code Umali R4 = Removing data from eq_rgv_addeq RadDataGrid
    Private Sub rec_del_eq_Click(sender As Object, e As EventArgs) Handles rec_del_eq.Click

        If (eq_rgv_addeq.Rows.Count = 0) Then
            RadMessageBox.Show(Me, "The Table is already empty.", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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
                RadMessageBox.Show(Me, "Item Not Available", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
            Else
                RadMessageBox.Show(Me, "Item Available", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
            End If
            MysqlConn.Close()
        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()

        End Try
    End Sub




    'Reservation Management Code Umali R6 = Inserting Data to database
    'THIS NEED FIXING BECAUSE OF MANY CHANGES OCCURED IN ITS INTERFACE 
    'FIXED SINGLE SAVED IN EQUIPMENTS 8.15
    'GOING TO DO
    'FIXED SOME LOGIC LIKE RECORD WILL NOT SAVE IF EQUIPMENT IS EMPTY
    '
    '08.26.16 
    'NEED TO FIX THE ERROR IN DUPLICATION OF TIME ACCORDING TO BORROWER. NEED TO REFERENCE THE EQUIPMENT NOT THE USER
    '08.26.16 Update 2
    'Parameter can't record multi-rows like reserving many rows

    Private Sub rec_btn_save_Click(sender As Object, e As EventArgs) Handles rec_btn_save.Click
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim READER As MySqlDataReader

        Dim conflictequipmentno As String = ""
        Dim conflictequipment As String = ""
        Dim conflictequipmentsn As String = ""
        If (rec_cb_reserveno.Text = "") Or (rec_cb_idnum.Text = "") Or (rec_cb_borrower.Text = "") Or (rec_dtp_date.Text = "") Or (rec_dtp_starttime.Text = "") Or (rec_dtp_endtime.Text = "") Or (rec_cb_college_school.Text = "") Or (rec_cb_location.Text = "") Or (rec_cb_status.Text = "") Or (rec_cb_reserved.Text = "") Or (rec_eq_chooseno.Text = "") Or (rec_eq_type_choose.Text = "") Or (eq_rgv_addeq.Rows.Count < 0) Or (rec_cb_acttype.Text = "") Then

            RadMessageBox.Show(Me, "Please complete the fields", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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

                        query = "SELECT * from reservation where reservationno=@RE_reservationno and equipment=@RE_equipment and equipmentsn=@RE_equipmentsn and equipmentno=@RE_equipmentno"
                        comm = New MySqlCommand(query, MysqlConn)
                        comm.Parameters.AddWithValue("RE_reservationno", rec_cb_reserveno.Text)
                        comm.Parameters.AddWithValue("RE_equipment", equipmentrgv)
                        comm.Parameters.AddWithValue("RE_equipmentsn", equipmentsnrgv)
                        comm.Parameters.AddWithValue("RE_equipmentno", equipmentnorgv)


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
                            RadMessageBox.Show(Me, "The equipment " & equipmentrgv & " with serial number of " & equipmentsnrgv & " is already taken", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
                            errorcount = True
                            Exit While
                        Else

                            MysqlConn.Close()
                            MysqlConn.Open()
                            query = "INSERT INTO `reservation` VALUES ('" & rec_cb_reserveno.Text & "','" & equipmentnorgv & "', '" & equipmentrgv & "', '" & equipmentsnrgv & "', '" & rec_cb_idnum.Text & "', '" & Format(CDate(rec_dtp_date.Value), "yyyy-MM-dd") & "','" & Format(CDate(rec_dtp_starttime.Text), "HH:mm") & "', '" & Format(CDate(rec_dtp_endtime.Text), "HH:mm") & "', '" & rec_cb_borrower.Text & "', '" & rec_cb_location.Text & "' ,'" & rec_cb_reserved.Text & "', '" & rec_cb_status.Text & "','" & rec_cb_acttype.Text & "')
                            ;INSERT INTO `reservation_equipments` VALUES ('" & rec_cb_reserveno.Text & "','" & equipmentnorgv & "', '" & equipmentrgv & "', '" & equipmentsnrgv & "')"

                            comm = New MySqlCommand(query, MysqlConn)
                            READER = comm.ExecuteReader
                            MysqlConn.Close()

                        End If

                    Catch ex As Exception
                        RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
                    Finally
                        MysqlConn.Dispose()
                    End Try
                    counter = counter + 1


                End While
                eq_rgv_addeq.Rows.Clear()
                rowcounter = 0
            End If

            If errorcount = False Then
                RadMessageBox.Show(Me, "Succesfully Equipment Reserved!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Info)
                auto_generate_reservationno()
            Else
                RadMessageBox.Show(Me, "Some equipments are not succesfully reserved", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
                auto_generate_reservationno()
            End If
        End If
        load_main_table()
        load_rec_table()
        reserved_load_table()
    End Sub

    'Showing All Data to Reservation Grid View

    Private Sub rec_btn_showalldata_Click(sender As Object, e As EventArgs) Handles rec_btn_showalldata.Click
        load_rec_table()
    End Sub

    'SHowing all available equipments to Reservation Grid View

    Private Sub rec_btn_showavailequip_Click(sender As Object, e As EventArgs) Handles rec_btn_showavailequip.Click
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
            'Relacing on how to show the taken equipments
            query = "Select equipmenttype as 'Equipment Type',equipmentno as 'Equipment No.',equipment as 'Equipment', equipmentsn as 'Equipment SN.' from equipments  ORDER BY equipmenttype ASC"

            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            reservation_rgv_recordeddata.DataSource = bsource
            reservation_rgv_recordeddata.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()

        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()

        End Try
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
            query = "Select equipmenttype as 'Equipment Type',equipmentno as 'Equipment No.',equipment as 'Equipment', equipmentsn as 'Equipment SN.' from equipments ORDER BY equipmenttype ASC"

            comm = New MySqlCommand(query, MysqlConn)
            sda.SelectCommand = comm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            reservation_rgv_recordeddata.DataSource = bsource
            reservation_rgv_recordeddata.ReadOnly = True
            sda.Update(dbdataset)
            MysqlConn.Close()

        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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


    'DELETING OF RECORD IN RESERVATION RECORD
    'NEED TO FIX QUERY
    'MANY ERRORS FOUND
    Private Sub rec_btn_delete_Click(sender As Object, e As EventArgs) Handles rec_btn_delete.Click
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        deleteYN = RadMessageBox.Show(Me, "Are you sure you want to delete?", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If deleteYN = MsgBoxResult.Yes Then
            Try
                MysqlConn.Open()
                query = "DELETE FROM reservation WHERE (reservationno=@R_rec_cb_reserveno) AND (id=@R_rec_cb_idnum)"
                comm = New MySqlCommand(query, MysqlConn)
                comm.Parameters.AddWithValue("R_rec_cb_reserveno", rec_cb_reserveno.Text)
                comm.Parameters.AddWithValue("R_rec_cb_idnum", rec_cb_idnum.Text)

                reader = comm.ExecuteReader


                RadMessageBox.Show(Me, "Successfully Deleted!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Info)
                MysqlConn.Close()
            Catch ex As Exception
                RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()
            End Try
        End If
        load_rec_table()
        load_main_table()
    End Sub


    Private Sub rec_cb_idnum_TextChanged(sender As Object, e As EventArgs) Handles rec_cb_idnum.TextChanged
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If
        MysqlConn.ConnectionString = connstring
        Try
            MysqlConn.Open()
            query = "SELECT CONCAT (lname,', ',fname) as 'Name' FROM accounts WHERE id=@rec_idno"
            comm = New MySqlCommand(query, MysqlConn)
            comm.Parameters.AddWithValue("rec_idno", rec_cb_idnum.Text)
            reader = comm.ExecuteReader

            rec_cb_borrower.Items.Clear()
            While reader.Read
                rec_cb_borrower.Items.Add(reader.GetString("name"))
            End While

            MysqlConn.Close()
        Catch ex As Exception
            RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        Finally
            MysqlConn.Dispose()
        End Try
    End Sub



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
        rec_cb_reserved.Text = ""
        rec_eq_chooseeq.Text = ""
        rec_eq_chooseno.Text = ""
        rec_eq_type_choose.Text = ""
        eq_rgv_addeq.Rows.Clear()

    End Sub

    Private Sub rec_dtp_date_ValueChanged(sender As Object, e As EventArgs) Handles rec_dtp_date.ValueChanged
        load_rec_table()
    End Sub

    Private Sub lu_date_ValueChanged(sender As Object, e As EventArgs) Handles lu_date.ValueChanged
        load_main_table()
    End Sub

    Private Sub reservation_rgv_recordeddata_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles reservation_rgv_recordeddata.CellDoubleClick
        If e.RowIndex >= 0 Then
            Dim row As Telerik.WinControls.UI.GridViewRowInfo

            row = Me.reservation_rgv_recordeddata.Rows(e.RowIndex)

            rec_cb_reserveno.Text = row.Cells("Reservation Number").Value.ToString
            rec_cb_idnum.Text = row.Cells("ID").Value.ToString.ToString


        End If
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
    '            RadMessageBox.Show(Me, "Item Not Available", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
    '        Else
    '            RadMessageBox.Show(Me, "Item Available", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
    '        End If

    '    Catch ex As Exception
    '        RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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
    '        RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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
    '        RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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
    '        RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
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
    '        RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
    '    Finally
    '        MysqlConn.Dispose()

    '    End Try
    'End Sub


End Class
