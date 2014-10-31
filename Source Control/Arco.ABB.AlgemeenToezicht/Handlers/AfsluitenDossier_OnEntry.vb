Public Class AfsluitenDossier_OnEntry
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)       
        Dim lslookupdossierbehandelaar As String = WFCurrentCase.GetProperty(Of String)("lookup_dossierbehandelaar")
        If Not String.IsNullOrEmpty(lslookupdossierbehandelaar) Then
            WFCurrentCase.SetProperty("dossierbehandelaar", lslookupdossierbehandelaar) 'todo : check also if empty?
        End If

        If String.IsNullOrEmpty(lslookupdossierbehandelaar) Then
            WFCurrentCase.SetProperty("S_dossierbehandelaar?", False)
            WFCurrentCase.SetProperty("dossierbehandelaar", WFCurrentCase.GetProperty("afdeling"))
        Else
            WFCurrentCase.SetProperty("S_dossierbehandelaar?", True)
        End If

        WFCurrentCase.Step_DueDate = "geen deadline"
        WFCurrentCase.SetProperty("huidige termijn", "")
        WFCurrentCase.SetProperty("termijn_RO_2012", "")
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "AfsluitenDossier_OnEntry"
        End Get
    End Property
End Class
