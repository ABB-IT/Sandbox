Imports Arco.ABB.Common

Public Class AlgemeenScriptAfd2_OnEntry
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        If WFCurrentCase.GetProperty(Of String)("doorsturen dossier") = "Ja" Then
            WFCurrentCase.SetProperty("doorsturen dossier", "Nee")
        End If
        ' functie geeft de waarde bij het binnekomen weer van de 3 extra info-velden 20110113
        SetWeergaveInfoVeld(WFCurrentCase)
        WFCurrentCase.SetProperty("Goedkeuring_afdeling", WFCurrentCase.GetProperty("afdeling2"))
        WFCurrentCase.SetProperty("goedkeuring_Dienst/TEAM/Cel", WFCurrentCase.GetProperty("Dienst/TEAM/Cel2"))
        WFCurrentCase.SetProperty("goedkeurder", "")

    End Sub
    Sub SetWeergaveInfoVeld(ByRef WFCurrentCase As Doma.Library.Routing.cCase)
        WFCurrentCase.SetProperty("HTMLweergave_infoveld", Formatting.FormatAssignee(WFCurrentCase.GetProperty(Of String)("dossierbehandelaar2")))
    End Sub
    Public Overrides ReadOnly Property Name As String
        Get
            Return "AlgemeenScriptAfd2_OnEntry"
        End Get
    End Property
End Class
