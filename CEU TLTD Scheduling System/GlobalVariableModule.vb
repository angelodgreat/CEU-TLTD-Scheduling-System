﻿Imports System.IO
Imports MySql.Data.MySqlClient
Module GlobalVariableModule
    Public system_Name = "CEU TLTD Reservation System"
    Public MySQLConn As New MySqlConnection
    Public MySQLConn_Bendo As New MySqlConnection
    Public MySQLConnReservationBackgroundWorker As New MySqlConnection
    Public MySQLConnCheckDBONLY As New MySqlConnection
    Public connstring As String = "server=" & My.Settings.cons_server & ";port=" & My.Settings.cons_port & ";userid=" & Actions.ToInsecureString(Actions.DecryptString(My.Settings.cons_username)) & ";password=" & Actions.ToInsecureString(Actions.DecryptString(My.Settings.cons_password)) & ";database=ceutltdscheduler"
    Public CheckDBConnstring As String = "server=" & My.Settings.cons_server & ";port=" & My.Settings.cons_port & ";userid=" & Actions.ToInsecureString(Actions.DecryptString(My.Settings.cons_username)) & ";password=" & Actions.ToInsecureString(Actions.DecryptString(My.Settings.cons_password))
    Public comm As MySqlCommand
    Public reader As MySqlDataReader
    Public adapter As New MySqlDataAdapter
    Public activeuserfname As String
    Public activeuserlname As String
    Public username As String
    Public studno As String
    Public lc As Login
    Public borrower_firstname As String
    Public borrower_lastname As String
    Public borrower_mobileno As String
    Public GlobalException As Exception = Nothing
    Public GlobalMySQLException As MySqlException = Nothing
    Public MultipurposeWindowPanel As Char
    Public reservationDBexists = False
    Public Sub applyconstringImmediately()
        Logger("Apply Connection String")
        connstring = "server=" & My.Settings.cons_server & ";port=" & My.Settings.cons_port & ";userid=" & Actions.ToInsecureString(Actions.DecryptString(My.Settings.cons_username)) & ";password=" & Actions.ToInsecureString(Actions.DecryptString(My.Settings.cons_password)) & ";database=ceutltdscheduler"
        CheckDBConnstring = "server=" & My.Settings.cons_server & ";port=" & My.Settings.cons_port & ";userid=" & Actions.ToInsecureString(Actions.DecryptString(My.Settings.cons_username)) & ";password=" & Actions.ToInsecureString(Actions.DecryptString(My.Settings.cons_password))
    End Sub

    'PENALTY Settings
    Public penalty_price As String
    Public penalty_graceperiod As String
    Public penalty_chargeinterval As String
    'PENALTY Settings

    'Delay Settings
    Public refresh_delay As Integer = My.Settings.refreshDelay
    Public search_delay As Integer = My.Settings.searchDelay
    Public Sub applydelaysImmediately()
        refresh_delay = My.Settings.refreshDelay
        search_delay = My.Settings.searchDelay
        Main.refresh_released_grid_list.Interval = refresh_delay
        Main.refresh_main_rgv_recordedacademicsonly.Interval = refresh_delay
    End Sub
    'Delay Settings

    'SMS Settings
    Public gsm_switch As Boolean = My.Settings.gsmIsOn
    Public Sub Save_SMS_Settings()
        My.Settings.gsmIsOn = MainSettingsWindow.setting_gsm_toggleswitch.Value
    End Sub
    'SMS Settings

    'GSM Module Connection using SerialPort
    Public portname, baudrate, parity, databits, stopbits, handshake, rtsenabled, receivedbytestreshold, newline, readtimeout

    'Console Window Status
    Public IsDebugMode As Boolean = False
    Public SilentDebug As Boolean = False

    'Logger Variables
    Dim LogFile As StreamWriter
    Dim path As String = "Log" & Now.ToString("MMddyyyyHHmmss") & ".txt"

    'Logger Class
    Public Sub Logger(ByVal msg As String)
        If IsDebugMode Then
            LogFile = File.AppendText(path)
            LogFile.WriteLine("[" & Now.ToString("MM-dd-yyyy HH:mm:ss") & "] " & msg)
            Console.WriteLine("[" & Now.ToString("MM-dd-yyyy HH:mm:ss") & "] " & msg)
            LogFile.Close()
        ElseIf SilentDebug Then
            LogFile = File.AppendText(path)
            LogFile.WriteLine("[" & Now.ToString("MM-dd-yyyy HH:mm:ss") & "] " & msg)
            LogFile.Close()
        End If
    End Sub
End Module
