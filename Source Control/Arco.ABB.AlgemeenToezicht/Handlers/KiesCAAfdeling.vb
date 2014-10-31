Public Class KiesCAAfdeling
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        'Centrale Afdeling werd al gekozen in de vorige stap

        WFCurrentCase.SetProperty("Dienst/TEAM/Cel2", "")
        WFCurrentCase.SetProperty("dossierbehandelaar2", "")

        WFCurrentCase.SetProperty("Keuze Dossierbehandelaar2", "&nbsp;&nbsp;&nbsp;<input type='button' value=' Afdelingshoofd  CA kiest  Team en Behandelaar CA' onclick='javascript:keuzeDossierbehandelaar2();'")

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "KiesCAAfdeling"
        End Get
    End Property
End Class
