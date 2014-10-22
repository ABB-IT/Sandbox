Public Class AlgemeenScript_OnExit
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        'versie : 1.1_081217
        'ALGEMEEN SCRIPT - ON EXIT

        If WFCurrentCase.GetProperty(Of String)("medium") <> "mail" Then

            WFCurrentCase.SetProperty("via_welke_mailbox", "")

            ' om de nutteloze staandaarwaarde in dit geval ongedaan te maken
        End If

        Dim lsbesluitdatum As String = WFCurrentCase.GetProperty(Of String)("datum besluit")
        If Not String.IsNullOrEmpty(lsbesluitdatum) Then
            If (DateDiff("n", lsbesluitdatum, "1/1/2013") < 0) Then
                If (WFCurrentCase.GetProperty(Of String)("voorwerp") = "inzendingsplichtig besluit" AndAlso WFCurrentCase.GetProperty(Of String)("type/soort besluit") = "rekening" AndAlso WFCurrentCase.GetProperty(Of String)("type bestuur") = "OCMW") Then
                    If WFCurrentCase.GetPropertyInfo("lbOntvGemeenteRek_OCMW").isEmpty Then
                        If StapNummers.GetStapNummer(WFCurrentCase) = 0 Then
                            WFCurrentCase.RejectComment = "Vul Datum ontvangst rekening aan gemeentebestuur in en /of Verzenddatum Opmerkingen aan ABB in !!!  "
                            WFCurrentCase.RejectUser = "Routing"
                        End If
                    End If
                End If
            End If
        End If


        If WFCurrentCase.GetProperty(Of Boolean)("stopzetten dossier") Then
            Dim lsReden As String = WFCurrentCase.GetProperty(Of String)("stopzetting reden uitleg")
            If Not String.IsNullOrEmpty(lsReden) Then
                WFCurrentCase.SetProperty("stopzetten dossier", True)
                WFCurrentCase.SetProperty("dossierbehandelaar", WFCurrentCase.StepExecutor)

                WFCurrentCase.DispatchStop(True, True)
            Else
                ' blokkeer routing
                WFCurrentCase.RejectComment = "Reden stopzetting moet ingevuld zijn om een dossier te stoppen "

            End If
        End If



    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "AlgemeenScript_OnExit"
        End Get
    End Property
End Class
