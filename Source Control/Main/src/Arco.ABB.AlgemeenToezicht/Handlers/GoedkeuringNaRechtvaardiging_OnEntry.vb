Public Class GoedkeuringNaRechtvaardiging_OnEntry
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)

        WFCurrentCase.SetProperty("hiddentoewijzing3", "")
        WFCurrentCase.SetProperty("hiddentoewijzing4", "")


        WFCurrentCase.SetProperty("keuze van de goedkeurder", "")
        WFCurrentCase.SetProperty("goedkeurder", "")
        WFCurrentCase.SetProperty("laatste goedkeurder?", "")
        WFCurrentCase.SetProperty("Dienst/TEAM/Cel", WFCurrentCase.GetProperty("Dienst/TEAM/Cel"))
        WFCurrentCase.SetProperty("Goedkeuring_afdeling", WFCurrentCase.GetProperty("afdeling"))
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "GoedkeuringNaRechtvaardiging_OnEntry"
        End Get
    End Property
End Class
