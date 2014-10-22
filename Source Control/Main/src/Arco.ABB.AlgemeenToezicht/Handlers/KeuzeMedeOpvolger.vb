Imports Arco.ABB.Common

<Serializable()> _
Public Class KeuzeMedeOpvolger
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)

        '  WFCurrentCase.SetProperty("S_dossierbehandelaar?", False)

        Dim lsBehandelaar2 As String = WFCurrentCase.GetProperty(Of String)("lookup_dossierbehandelaar2")
        If String.IsNullOrEmpty(lsBehandelaar2) Then
            WFCurrentCase.SetProperty("S_dossierbehandelaar?", False)
        Else
            WFCurrentCase.SetProperty("S_dossierbehandelaar?", True)
        End If

        Toewijzigingen.CascadeToewijzing(WFCurrentCase)

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Keuze medeopvolger"
        End Get
    End Property
End Class

