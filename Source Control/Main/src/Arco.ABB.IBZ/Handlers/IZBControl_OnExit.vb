Public Class IZBControl_OnExit
    Inherits IZBEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        Dim lsError As String = OverzichtBesluiten.CheckData(WFCurrentCase)
        If lsError <> "" Then
            WFCurrentCase.RejectComment = lsError
            WFCurrentCase.RejectUser = "Routing"
        End If
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "IBZControl_OnExit"
        End Get
    End Property
End Class
