Public Class DatumNaarGouverneur
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        If WFCurrentCase.GetPropertyInfo("Datum dossier naar G/M").isEmpty AndAlso WFCurrentCase.GetProperty(Of String)("Naar Gouverneur / Minister") = "Ja" Then
            WFCurrentCase.SetProperty("Datum dossier naar G/M", System.DateTime.Now)
        End If

        If Not WFCurrentCase.CurrentStep.Step_Name = "Goedkeuring - afdeling 1" Then
            Dim loIncluded As VulAfdelingNaRechtvaardigingGoedkeuring_OnExit = New VulAfdelingNaRechtvaardigingGoedkeuring_OnExit
            loIncluded.Execute(WFCurrentCase)
        End If

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "DatumNaarGouverneur"
        End Get
    End Property
End Class
