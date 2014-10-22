Imports Arco.ABB.Common

Public Class AlgemeenScript_OnEntry
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        If WFCurrentCase.GetProperty(Of String)("doorsturen dossier") = "Ja" Then
            WFCurrentCase.SetProperty("doorsturen dossier", "Nee")
        End If
        ' functie geeft de waarde bij het binnekomen weer van de 3 extra info-velden 20110113
        SetWeergaveInfoVeld(WFCurrentCase)    
    End Sub
    Sub SetWeergaveInfoVeld(ByRef WFCurrentCase As Doma.Library.Routing.cCase)
        WFCurrentCase.SetProperty("HTMLweergave_infoveld", Formatting.FormatAssignee(WFCurrentCase.GetProperty(Of String)("dossierbehandelaar")))
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "AlgemeenScript_OnEntry"
        End Get
    End Property
End Class
