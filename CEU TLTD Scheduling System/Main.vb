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

    Public equipment As String
    Dim query As String

    Public Sub startup_disabled_buttons()
        acc_staff_btn_update.Hide()
        acc_staff_btn_delete.Hide()
        eq_btn_update.Hide()
        eq_btn_delete.Hide()
    End Sub


    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        reservation_rgv_recordeddata.Show()
        reservations_rgv_showavailableitems.Hide()
        load_main_table()
        load_rec_table()
        load_eq_table()
        load_main_acc()
        load_main_prof()
        startup_disabled_buttons()



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

            query = "Select DATE_FORMAT(startdate,'%M %d %Y') as 'Start Date', DATE_FORMAT(enddate,'%M %d, %Y') as 'End Date', TIME_FORMAT(starttime, '%H:%i') as 'Start Time', TIME_FORMAT(endtime, '%H:%i') as 'End Time', borrower as 'Borrower',location as 'Location', equipment as 'Equipment' from reservation"

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

            query = "Select DATE_FORMAT(startdate,'%M %d %Y') as 'Start Date', DATE_FORMAT(enddate,'%M %d, %Y') as 'End Date', TIME_FORMAT(starttime, '%H:%i') as 'Start Time', TIME_FORMAT(endtime, '%H:%i') as 'End Time', borrower as 'Borrower',location as 'Location', equipment as 'Equipment' from reservation"

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
            query = "Select staff_id as 'Staff ID' , staff_fname as 'First Name' , staff_mname as 'Middle Name' , staff_surname as 'Surname' , staff_college as 'College/School'  from staff_reg"
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
            query = "Select prof_id as 'Professor ID' , prof_fname as 'First Name' , prof_mname as 'Middle Name' , prof_surname as 'Surname' , prof_college as 'College/School'  from prof_reg"
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

        Try
            MysqlConn.Open()
            Dim Query As String
            Query = "insert into ceutltdscheduler.staff_reg (staff_id,staff_fname,staff_mname,staff_surname,staff_college,staff_type,staff_username,staff_password) values ('" & acc_sf_id.Text & "' , '" & acc_sf_fname.Text & "', '" & acc_sf_mname.Text & "', '" & acc_sf_lname.Text & "' , '" & acc_sf_department.Text & "' , '" & acc_sf_usertype.Text & "' , '" & acc_sf_username.Text & "' , sha2('" & acc_sf_password.Text & "', 512))"
            comm = New MySqlCommand(Query, MysqlConn)

            svYN = RadMessageBox.Show(Me, "Are you sure you want To save this information? ", "Exit", MessageBoxButtons.YesNo, RadMessageIcon.Question)
            If svYN = MsgBoxResult.Yes Then
                If (acc_sf_password.Text = acc_sf_retypepassword.Text) Then
                    READER = comm.ExecuteReader
                    RadMessageBox.Show("Register Complete!")

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

    End Sub





    Private Sub rec_btn_save_Click(sender As Object, e As EventArgs) Handles rec_btn_save.Click
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim READER As MySqlDataReader


        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If
        If (rec_dtp_startdate.Text = "") Or (rec_dtp_enddate.Text = "") Or (rec_dtp_starttime.Text = "") Or (rec_dtp_endtime.Text = "") Or (rec_cb_borrower.Text = "") Or (rec_cb_location.Text = "") Or (rec_cob_equipment.Text = "") Then
            RadMessageBox.Show(Me, "Please complete the form", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
        Else
            Try
                MysqlConn.Open()


                query = "SELECT * from reservation where (equipment='" & rec_cob_equipment.Text & "') and (('" & Format(CDate(rec_dtp_startdate.Value), "yyyy-MM-dd") & " " & Format(CDate(rec_dtp_starttime.Text), "hh:mm") & "' BETWEEN CONCAT(startdate,' ',starttime) AND CONCAT(enddate,' ',endtime)) OR
            ('" & Format(CDate(rec_dtp_enddate.Value), "yyyy-MM-dd") & " " & Format(CDate(rec_dtp_endtime.Text), "hh:mm") & "' BETWEEN CONCAT (enddate,' ',starttime) AND CONCAT(enddate,' ',endtime)))"
                comm = New MySqlCommand(query, MysqlConn)
                READER = comm.ExecuteReader

                Dim count As Integer
                count = 0

                While READER.Read
                    count += 1
                End While

                If count = 1 Then
                    RadMessageBox.Show(Me, "The time " & Format(CDate(rec_dtp_starttime.Text), "HH:mm") & " and " & "the equipment " & rec_cob_equipment.Text & " is already in used.", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)

                Else
                    MysqlConn.Close()
                    MysqlConn.Open()

                    addYN = RadMessageBox.Show(Me, "Are you sure you want to save this reservation?", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
                    If addYN = MsgBoxResult.Yes Then
                        query = "INSERT INTO reservation (startdate,enddate,starttime,endtime,borrower,location,equipment,reservedby,status) VALUES ('" & Format(CDate(rec_dtp_startdate.Value), "yyyy-MM-dd") & "','" & Format(CDate(rec_dtp_enddate.Value), "yyyy-MM-dd") & "','" & Format(CDate(rec_dtp_starttime.Text), "HH:mm") & "',
                        '" & Format(CDate(rec_dtp_endtime.Text), "HH:mm") & "', '" & rec_cb_borrower.Text & "','" & rec_cb_location.Text & "','" & rec_cob_equipment.Text & "','" & rec_cb_reserved.Text & "','" & rec_cb_status.Text & "')  "
                        comm = New MySqlCommand(query, MysqlConn)
                        READER = comm.ExecuteReader

                        RadMessageBox.Show(Me, "Details Saved!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Info)
                        MysqlConn.Close()

                    End If

                End If

            Catch ex As Exception
                RadMessageBox.Show(Me, ex.Message, "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
            Finally
                MysqlConn.Dispose()

            End Try

        End If
        load_main_table()
        load_rec_table()

    End Sub


    'Main Window Search Functions

    Private Sub btn_searchbydate_Click(sender As Object, e As EventArgs) Handles btn_searchbydate.Click
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim SDA As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource
        Try
            MysqlConn.Open()

            query = "select DATE_FORMAT(startdate,'%M %d %Y') as 'Start Date', DATE_FORMAT(enddate,'%M %d, %Y') as 
            'End Date', TIME_FORMAT(starttime, '%H:%i') as 'Start Time', TIME_FORMAT(endtime, '%H:%i') as 'End Time', borrower as 
            'Borrower',location as 'Location', equipment as 'Equipment' from reservation
            where (((startdate between '" & Format(CDate(lu_startdate.Value), "yyyy-MM-dd") & "' and '" & Format(CDate(lu_enddate.Value), "yyyy-MM-dd") & "') and 
            (starttime between '" & Format(CDate(lu_starttime.Text), "HH:mm") & "' and '" & Format(CDate(lu_starttime.Text), "HH:mm") & "')) or
            ((enddate between '" & Format(CDate(lu_startdate.Value), "yyyy-MM-dd") & "' and '" & Format(CDate(lu_enddate.Value), "yyyy-MM-dd") & "') and
            (endtime between '" & Format(CDate(lu_endtime.Text), "HH:mm") & "' and '" & Format(CDate(lu_endtime.Text), "HH:mm") & "')))"

            comm = New MySqlCommand(query, MysqlConn)
            SDA.SelectCommand = comm
            SDA.Fill(dbdataset)
            bsource.DataSource = dbdataset
            main_rgv_recordeddatamain.DataSource = bsource
            SDA.Update(dbdataset)

            MysqlConn.Close()

        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            MysqlConn.Dispose()
        End Try
    End Sub

    Private Sub lu_bylocation_TextChanged(sender As Object, e As EventArgs) Handles lu_bylocation.TextChanged
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim SDA As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource
        Try
            MysqlConn.Open()

            query = "SELECT DATE_FORMAT(startdate,'%M %d %Y') AS 'Start Date', DATE_FORMAT(enddate,'%M %d, %Y') AS 
            'End Date', TIME_FORMAT(starttime, '%H:%i') AS 'Start Time', TIME_FORMAT(endtime, '%H:%i') AS 'End Time', borrower AS 
            'Borrower',location AS 'Location', equipment AS 'Equipment' FROM reservation ORDER BY location desc"
            comm = New MySqlCommand(query, MysqlConn)
            SDA.SelectCommand = comm
            SDA.Fill(dbdataset)
            bsource.DataSource = dbdataset
            main_rgv_recordeddatamain.DataSource = bsource
            SDA.Update(dbdataset)

            MysqlConn.Close()

        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            MysqlConn.Dispose()
        End Try

        Dim DV As New DataView(dbdataset)
        DV.RowFilter = String.Format("`Location` Like'%{0}%'", lu_bylocation.Text)
        main_rgv_recordeddatamain.DataSource = DV
    End Sub
    Private Sub lu_byequipment_TextChanged(sender As Object, e As EventArgs) Handles lu_byequipment.TextChanged
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = connstring
        Dim SDA As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource
        Try
            MysqlConn.Open()

            query = "SELECT DATE_FORMAT(startdate,'%M %d %Y') AS 'Start Date', DATE_FORMAT(enddate,'%M %d, %Y') AS 
            'End Date', TIME_FORMAT(starttime, '%H:%i') AS 'Start Time', TIME_FORMAT(endtime, '%H:%i') AS 'End Time', borrower AS 
            'Borrower',location AS 'Location', equipment AS 'Equipment' FROM reservation ORDER BY equipment desc"
            comm = New MySqlCommand(query, MysqlConn)
            SDA.SelectCommand = comm
            SDA.Fill(dbdataset)
            bsource.DataSource = dbdataset
            main_rgv_recordeddatamain.DataSource = bsource
            SDA.Update(dbdataset)

            MysqlConn.Close()

        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            MysqlConn.Dispose()
        End Try

        Dim DV As New DataView(dbdataset)
        DV.RowFilter = String.Format("`Equipment` Like'%{0}%'", lu_byequipment.Text)
        main_rgv_recordeddatamain.DataSource = DV
    End Sub

    'Equipment Management Codes

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

            query = "SELECT equipmentno as 'Equipment Number', equipment as 'Equipment', equipmentsn as 'Serial Number', equipmentlocation as 'Equipment Location',equipmentowner as 'Owner',equipmentstatus as 'Status' from equipments"

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
                query = "SELECT * from equipments where (equipmentsn='" & eq_sn.Text & "')"
                comm = New MySqlCommand(query, MysqlConn)
                reader = comm.ExecuteReader

                Dim count As Integer

                While reader.Read
                    count += 1
                End While

                If count = 1 Then
                    RadMessageBox.Show(Me, "Equipment #" & eq_equipmentno.Text & " and the equipment " & eq_equipment.Text & " is already registered")
                Else
                    MysqlConn.Close()
                    MysqlConn.Open()

                    addYN = RadMessageBox.Show(Me, "Are you sure you want to save this equipment?", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
                    If addYN = MsgBoxResult.Yes Then
                        query = "INSERT INTO `equipments` VALUES ('" & eq_equipmentno.Text & "','" & eq_equipment.Text & "','" & eq_sn.Text & "','" & eq_equipmentlocation.Text & "','" & eq_owner.Text & "','" & eq_status.Text & "')"
                        comm = New MySqlCommand(query, MysqlConn)
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

    End Sub




    Private Sub eq_btn_update_Click(sender As Object, e As EventArgs) Handles eq_btn_update.Click
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        updateYN = RadMessageBox.Show(Me, "Do you want to update the selected equipment?", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Question)
        If updateYN = MsgBoxResult.Yes Then
            If (eq_equipmentno.Text = "") Or (eq_sn.Text = "") Or (eq_equipment.Text = "") Or (eq_equipmentlocation.Text = "") Or (eq_owner.Text = "") Or (eq_status.Text = "") Then
                RadMessageBox.Show(Me, "Please complete the fields to update!", "TLTD Scheduling Management", MessageBoxButtons.OK, RadMessageIcon.Error)
            Else
                Try
                    MysqlConn.Open()
                    query = "UPDATE equipments SET equipmentno='" & eq_equipmentno.Text & "',equipmentsn='" & eq_sn.Text & "',equipment='" & eq_equipment.Text & "', equipmentlocation='" & eq_equipmentlocation.Text & "',equipmentowner='" & eq_owner.Text & "',equipmentstatus='" & eq_status.Text & "' where (equipmentsn='" & eq_sn.Text & "') and (equipmentno='" & eq_equipmentno.Text & "')"
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
        load_eq_table()
    End Sub

    Private Sub eq_btn_delete_Click(sender As Object, e As EventArgs) Handles eq_btn_delete.Click
        If MysqlConn.State = ConnectionState.Open Then
            MysqlConn.Close()
        End If

        deleteYN = RadMessageBox.Show(Me, "Are you sure you want to delete?", "TLTD Scheduling Management", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If deleteYN = MsgBoxResult.Yes Then
            Try
                MysqlConn.Open()
                query = "DELETE FROM equipments where equipmentid='" & eq_equipmentno.Text & "' and equipmentsn='" & eq_sn.Text & "' "
                comm = New MySqlCommand(query, MysqlConn)
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
    End Sub

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

            eq_btn_update.Show()
            eq_btn_delete.Show()
            eq_btn_save.Hide()
        End If
    End Sub

    Private Sub eq_btn_clear_Click(sender As Object, e As EventArgs) Handles eq_btn_clear.Click
        eq_equipmentno.Text = ""
        eq_equipment.Text = ""
        eq_equipmentlocation.Text = ""
        eq_sn.Text = ""
        eq_status.Text = ""
        eq_owner.Text = ""


        eq_btn_update.Hide()
        eq_btn_delete.Hide()
        eq_btn_save.Show()
    End Sub
End Class
