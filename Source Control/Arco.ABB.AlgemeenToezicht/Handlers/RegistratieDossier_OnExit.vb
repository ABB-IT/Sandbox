Public Class RegistratieDossier_OnExit
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)

        WFCurrentCase.RejectComment = String.Empty
        WFCurrentCase.RejectUser = String.Empty

        'dispatch naar keuze dossierbehandelaar indien er geen gekozen is
        If WFCurrentCase.GetPropertyInfo("lookup_dossierbehandelaar").isEmpty Then
            WFCurrentCase.SetProperty("S_dossierbehandelaar?", False)
        Else
            WFCurrentCase.SetProperty("S_dossierbehandelaar?", True)
        End If

        '  controle afdeling is ingevuld bij het vrijgeven van het dossier
        If Not WFCurrentCase.GetProperty(Of Boolean)("stopzetten dossier") Then
            If WFCurrentCase.CurrentStep.Step_Name = "Keuze dossierbehandelaar" Then
                If WFCurrentCase.GetPropertyInfo("afdeling").isEmpty Then
                    WFCurrentCase.RejectComment = "Afdeling moet verplicht ingevuld zijn! "
                End If
            End If

            Dim lsErrMess As String = String.Empty
            If Not WFCurrentCase.GetPropertyInfo("voorwerp").isEmpty Then
                Select Case WFCurrentCase.GetProperty(Of String)("voorwerp")
                    Case "lijstbesluit"
                        If (WFCurrentCase.GetProperty(Of String)("lijstbesluit_nr") = "0" Or WFCurrentCase.GetPropertyInfo("lijstbesluit_nr").isEmpty) Then
                            lsErrMess = "Als een lijstbesluit werd gekozen, moet een besluit uit de lijst worden geselecteerd."
                        End If
                    Case "inzendingsplichtig besluit"
                        If (WFCurrentCase.GetProperty(Of String)("inzend_BESL_nr") = "0" Or WFCurrentCase.GetPropertyInfo("inzend_BESL_nr").isEmpty) Then
                            lsErrMess = "Als een inzendingsplichtig besluit werd gekozen, moet een besluit uit de lijst worden geselecteerd."
                        End If
                    Case "ander besluit"
                        If (WFCurrentCase.GetProperty(Of String)("ander_BESL_nr") = "0" Or WFCurrentCase.GetPropertyInfo("ander_BESL_nr").isEmpty) Then
                            lsErrMess = "Als een ander besluit werd gekozen, moet een besluit uit de lijst worden geselecteerd."
                        End If
                    Case Else
                End Select

                'Dim lsHiddenAdd20 As String = WFCurrentCase.GetProperty(Of String)("hiddenAdd20")
                'If Not lsHiddenAdd20 = "Add" Then
                '    If String.IsNullOrEmpty(lsErrMess) Then
                '        lsErrMess = "Check eerst op dubbels vooraleer verder te gaan aub."
                '    Else
                '        lsErrMess &= vbCrLf & "Check eerst op dubbels vooraleer verder te gaan aub."
                '    End If
                'End If

                If String.IsNullOrEmpty(WFCurrentCase.RejectComment) Then
                    WFCurrentCase.RejectComment = lsErrMess
                Else
                    WFCurrentCase.RejectComment &= vbCrLf & lsErrMess
                End If

                ' Enkel indien alles goed is toegewezen: adhv voorwerp worden de niet-relevante velden leeggemaakt om fouten in latere opzoekingen te vermijden.
                If String.IsNullOrEmpty(WFCurrentCase.RejectComment) Then
                    Select Case WFCurrentCase.GetProperty(Of String)("voorwerp")
                        Case "lijstbesluit"
                            Call InitialiseerIZBVelden(WFCurrentCase)
                            Call InitialiseerAnderBesluitVelden(WFCurrentCase)
                            WFCurrentCase.SetProperty("datum besluit", "")
                        Case "inzendingsplichtig besluit"
                            Call InitialiseerLijstBesluitVelden(WFCurrentCase)
                            Call InitialiseerAnderBesluitVelden(WFCurrentCase)
                        Case "ander besluit"
                            Call InitialiseerLijstBesluitVelden(WFCurrentCase)
                            Call InitialiseerIZBVelden(WFCurrentCase)
                            WFCurrentCase.SetProperty("lijstbesluit_postdatum", "")
                            WFCurrentCase.SetProperty("lijst_ontvangstdatum", "")
                            WFCurrentCase.SetProperty("initiële vervaltermijn", "")
                        Case "handeling"
                            Call InitialiseerLijstBesluitVelden(WFCurrentCase)
                            Call InitialiseerIZBVelden(WFCurrentCase)
                            Call InitialiseerAnderBesluitVelden(WFCurrentCase)
                            WFCurrentCase.SetProperty("lijstbesluit_postdatum", "")
                            WFCurrentCase.SetProperty("lijst_ontvangstdatum", "")
                            WFCurrentCase.SetProperty("initiële vervaltermijn", "")
                            WFCurrentCase.SetProperty("datum besluit", "")
                    End Select
                End If
            End If
        End If
    End Sub

    Private Sub InitialiseerLijstBesluitVelden(ByVal WFCurrentCase As Doma.Library.Routing.cCase)
        WFCurrentCase.SetProperty("lijstbesluit_nr", "")
        WFCurrentCase.SetProperty("lijstbesluit_zitting", "")
        WFCurrentCase.SetProperty("lijstbesluit_gemeente", "")
    End Sub

    Private Sub InitialiseerIZBVelden(ByVal WFCurrentCase As Doma.Library.Routing.cCase)
        WFCurrentCase.SetProperty("inzend_besl_nr", "")
        WFCurrentCase.SetProperty("boekjaar", 0)
        WFCurrentCase.SetProperty("hoeveelste", 0)
        WFCurrentCase.SetProperty("lijst_kortomschrijving", "")
    End Sub

    Private Sub InitialiseerAnderBesluitVelden(ByVal WFCurrentCase As Doma.Library.Routing.cCase)
        WFCurrentCase.SetProperty("ander_besl_nr", "")
        WFCurrentCase.SetProperty("beslissingsorgaan", "")
        WFCurrentCase.SetProperty("titel besluit", "")
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "RegistratieDossier_OnExit"
        End Get
    End Property
End Class
