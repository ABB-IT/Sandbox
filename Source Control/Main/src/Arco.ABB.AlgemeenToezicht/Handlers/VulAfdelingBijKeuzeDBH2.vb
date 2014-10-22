Public Class VulAfdelingBijKeuzeDBH2
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        If (WFCurrentCase.GetProperty(Of String)("laatste goedkeurder?") = "neen (kies verdere afhandeling)" AndAlso WFCurrentCase.GetProperty(Of String)("Ik keur het voorstel goed") = "ja" AndAlso WFCurrentCase.GetPropertyInfo("Afdeling2_bis").isEmpty) Then
            WFCurrentCase.RejectComment = "Vul afdeling verdere afhandeling in!!"
        Else
            WFCurrentCase.SetProperty("afdeling2", WFCurrentCase.GetProperty("Afdeling2_bis"))
        End If
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "VulAfdelingBijKeuzeDBH2"
        End Get
    End Property
End Class
