﻿Imports MySql.Data.MySqlClient
Module GlobalVariableModule
    Public MySQLConn As New MySqlConnection
    Public connstring As String = "server=" & My.Settings.cons_server & ";port=" & My.Settings.cons_port & ";userid=" & Actions.ToInsecureString(Actions.DecryptString(My.Settings.cons_username)) & ";password=" & Actions.ToInsecureString(Actions.DecryptString(My.Settings.cons_password)) & ";database=" & My.Settings.cons_database
    Public comm As MySqlCommand
    Public reader As MySqlDataReader
    Public adapter As New MySqlDataAdapter
    Public activeuserfname As String
    Public activeuserlname As String
    Public username As String
    Public studno As String
    Public lc As Login

End Module
