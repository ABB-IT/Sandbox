Public Class InfoTijdigheidDossier
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        If WFCurrentCase.GetProperty(Of String)("aard dossier") <> "klacht" Then
            WFCurrentCase.SetProperty("hoedanigheid", "NVT")        
        End If

        'doesn't do anything, just gets!
        
        'If WFGetProperty("resultaat onderzoek") = "schorsing" Then

        '    lpTermijnRO = WFGetProperty("Termijn_NS")

        'Else
        '    lpTermijnRO = WFGetProperty("Termijn_RO")

        'End If

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "InfoTijdigheidDossier"
        End Get
    End Property
End Class
