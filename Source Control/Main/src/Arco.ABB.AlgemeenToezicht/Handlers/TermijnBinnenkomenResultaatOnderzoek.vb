Public Class TermijnBinnenkomenResultaatOnderzoek
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        WFCurrentCase.SetProperty("Termijn_RO_2012", WFCurrentCase.GetProperty("huidige termijn"))
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "TermijnBinnenkomenResultaatOnderzoek"
        End Get
    End Property
End Class
