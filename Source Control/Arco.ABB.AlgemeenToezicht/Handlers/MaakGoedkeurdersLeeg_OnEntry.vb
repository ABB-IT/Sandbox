Public Class MaakGoedkeurdersLeeg_OnEntry
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        '(00) Maak GOEDKEURDER LEEG  - ON ENTRY      
        WFCurrentCase.SetProperty("Ik keur het voorstel goed", "")
        WFCurrentCase.SetProperty("laatste goedkeurder?", "")
        WFCurrentCase.SetProperty("laatste goedkeurder2?", "")
        WFCurrentCase.SetProperty("Allerlaatstegoedkeurder", WFCurrentCase.GetProperty("goedkeurder"))

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "MaakGoedkeurdersLeeg_OnEntry"
        End Get
    End Property
End Class
