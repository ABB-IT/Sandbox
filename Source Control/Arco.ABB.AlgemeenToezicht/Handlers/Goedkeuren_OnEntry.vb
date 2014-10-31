Public Class Goedkeuren_OnEntry
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        Dim liStapNr As Integer = StapNummers.GetStapNummer(WFCurrentCase)
        
        WFCurrentCase.SetProperty("hiddentoewijzing3", "")
        WFCurrentCase.SetProperty("hiddentoewijzing4", "")



        WFCurrentCase.SetProperty("keuze van de goedkeurder", "")
        WFCurrentCase.SetProperty("Allerlaatstegoedkeurder", WFCurrentCase.GetProperty("goedkeurder"))

        'Call WFSetProperty("goedkeurder","" )
        WFCurrentCase.SetProperty("laatste goedkeurder?", "")
        WFCurrentCase.SetProperty("laatste goedkeurder2?", "")

        WFCurrentCase.SetProperty("Ik keur het voorstel goed", "")

        If liStapNr <> 5 Then

            Call WFCurrentCase.SetProperty("goedkeuring_Dienst/TEAM/Cel", WFCurrentCase.GetProperty("Dienst/TEAM/Cel2"))
            Call WFCurrentCase.SetProperty("Goedkeuring_afdeling", WFCurrentCase.GetProperty("afdeling2"))
        Else
        End If

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Goedkeuren_OnEntry"
        End Get
    End Property
End Class
