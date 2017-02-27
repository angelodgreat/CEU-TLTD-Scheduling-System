﻿Imports System.Threading

'Lance Bendo

Public Class PendingSms

    Private WithEvents t As New System.Windows.Forms.Timer
    Private dt As DataTable
    Private endTime, rel_id, rel_mobile_no, rel_borrower, rel_eqtype As String
    Private isDone As Boolean = False
    Private smsIsDone As List(Of String)
    Private sms_queue As DataTable


    Sub New(ByVal smsRow As DataRow, dt As DataTable, ByVal t As System.Windows.Forms.Timer, ByVal smsIsDone As List(Of String), ByVal sms_queue As DataTable)
        Me.t = t
        Me.dt = dt
        initializeData(smsRow)
        Me.smsIsDone = smsIsDone
        Me.sms_queue = sms_queue
        endTime = DateTime.Now.AddSeconds(10).ToString   ''for testing purpose, 10 seconds yung default para magtrigger yung text. icomment lang ang line na to para sa real end_date
    End Sub

    Private Sub initializeData(ByVal row As DataRow)
        Me.rel_id = row(0).ToString
        'Me.endTime = formatDate(row(6).ToString, row(1).ToString)
        Me.rel_mobile_no = row(3).ToString
        Me.rel_borrower = getSurname(row(4).ToString)
        Me.rel_eqtype = row(5).ToString
    End Sub

    Private Function formatDate(ByVal dateRow As String, ByVal timeRow As String)
        Return DateTime.ParseExact(getDate(dateRow) & " " & timeRow, "M/dd/yyyy HH:mm:ss", Nothing).ToString
    End Function

    Private Function getSurname(ByVal fullname As String)
        Return fullname.Substring(0, fullname.IndexOf(","))

    End Function

    Private Function getDate(ByVal dateString As String)
        Return dateString.Substring(0, dateString.IndexOf(" "))
    End Function

    Public Function getReleaseId()
        Return Me.rel_id
    End Function

    Private Function getSmsContent(ByVal borrower As String, ByVal eqtype As String, ByVal penalty_amount As String)
        Dim content As String = "To: " & borrower _
        & Environment.NewLine _
        & Environment.NewLine _
        & "This is an automated message from the TLTD Department. We would like to inform you that your borrowed equipment '" _
        & eqtype _
        & "' has reached its allowable time. Faliure to return the equipment within one (1) hour will cost Php." _
        & penalty_amount _
        & Environment.NewLine _
        & Environment.NewLine _
        & "From: TLTD Department, Centro Escolar University"


        Return content
    End Function

    Private Sub t_Tick(ByVal sender As Object, e As EventArgs) Handles t.Tick
        Debug.WriteLine(endTime & " " & DateTime.Now.ToString)
        If endTime = DateTime.Now.ToString And isDone = False Then
            Dim str As String = "This sms is for " & Me.rel_id & " pls return the item"
            sms_queue.Rows.Add(rel_mobile_no, getSmsContent(Me.rel_borrower, Me.rel_eqtype, "100.00"))     '100.00 is the default amount of penalty
            smsIsDone.Add(getReleaseId())
            destroyThis()
        End If
    End Sub

    Public Sub destroyThis()
        RemoveHandler t.Tick, AddressOf t_Tick
        Me.isDone = Nothing
        Me.rel_id = Nothing
        MyBase.Finalize()
    End Sub


End Class
