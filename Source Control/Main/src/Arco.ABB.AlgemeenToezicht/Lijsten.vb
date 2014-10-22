Imports Arco.ABB.Common

Public Class Lijsten
    Public Shared Sub BerekenLijsten(ByVal WFCurrentCase As Arco.Doma.Library.Routing.cCase)
        'lijstbesluit
        'script voor de lijsten

        Dim lsvoorwerp As String = WFCurrentCase.GetProperty(Of String)("voorwerp")
        If String.IsNullOrEmpty(lsvoorwerp) OrElse lsvoorwerp = "handeling" Then
            WFCurrentCase.SetPropertyVisible("lijstbesluit_gemeente", False)
            WFCurrentCase.SetPropertyVisible("lijst_ontvangstdatum", False)
            WFCurrentCase.SetPropertyVisible("lijstbesluit_postdatum", False)
            WFCurrentCase.SetPropertyVisible("initiële vervaltermijn", False)
            WFCurrentCase.SetPropertyVisible("lijstbesluit_zitting", False)
            WFCurrentCase.SetPropertyVisible("datum besluit", False)
            WFCurrentCase.SetPropertyVisible("type/soort besluit", False)
            WFCurrentCase.SetPropertyVisible("Beslissingsorgaan", False)
            WFCurrentCase.SetPropertyVisible("titel besluit", False)
            WFCurrentCase.SetPropertyVisible("lijst_kortomschrijving", False)
            WFCurrentCase.SetPropertyVisible("boekjaar", False)
            WFCurrentCase.SetPropertyVisible("hoeveelste", False)
            WFCurrentCase.SetProperty("lijstbesluit_gemeente", "")
            WFCurrentCase.SetProperty("lijst_ontvangstdatum", "")
            WFCurrentCase.SetProperty("lijstbesluit_postdatum", "")
            WFCurrentCase.SetProperty("initiële vervaltermijn", "")
            WFCurrentCase.SetProperty("lijstbesluit_zitting", "")
            WFCurrentCase.SetProperty("datum besluit", "")
            WFCurrentCase.SetProperty("type/soort besluit", "")
            WFCurrentCase.SetProperty("Beslissingsorgaan", "")
            WFCurrentCase.SetProperty("titel besluit", "")
            WFCurrentCase.SetPropertyVisible("inzend_BESL_nr", False)
            WFCurrentCase.SetPropertyVisible("lijstbesluit_nr", False)
            WFCurrentCase.SetPropertyVisible("ander_BESL_nr", False)
            WFCurrentCase.SetProperty("inzend_BESL_nr", "")
            WFCurrentCase.SetProperty("ander_BESL_nr", "")
            WFCurrentCase.SetProperty("lijstbesluit_nr", "")
            WFCurrentCase.SetProperty("besluit_id", 0)
        End If

        If lsvoorwerp = "lijstbesluit" Then

            WFCurrentCase.SetPropertyVisible("inzend_BESL_nr", False)
            WFCurrentCase.SetPropertyVisible("lijstbesluit_nr", True)
            WFCurrentCase.SetPropertyVisible("ander_BESL_nr", False)
            WFCurrentCase.SetPropertyVisible("lijstbesluit_gemeente", True)
            WFCurrentCase.SetPropertyVisible("lijst_ontvangstdatum", True)
            WFCurrentCase.SetPropertyVisible("lijstbesluit_postdatum", True)
            WFCurrentCase.SetPropertyVisible("initiële vervaltermijn", True)
            WFCurrentCase.SetPropertyVisible("lijstbesluit_zitting", True)
            WFCurrentCase.SetPropertyVisible("datum besluit", False)
            WFCurrentCase.SetPropertyVisible("type/soort besluit", False)
            WFCurrentCase.SetPropertyVisible("Beslissingsorgaan", False)
            WFCurrentCase.SetPropertyVisible("titel besluit", False)
            WFCurrentCase.SetPropertyVisible("lijst_kortomschrijving", False)
            WFCurrentCase.SetPropertyVisible("boekjaar", False)
            WFCurrentCase.SetPropertyVisible("hoeveelste", False)


            Dim lsMeldingslijstNr = WFCurrentCase.GetProperty(Of String)("lijstbesluit_nr")
            If Not String.IsNullOrEmpty(lsMeldingslijstNr) Then
                Dim loLijst As MeldingsLijst = MeldingsLijst.GetMeldingsLijst(lsMeldingslijstNr)

                'Call Haalmeldingslijst(vsMeldingslijstNr , vsgemeente , vsdatumzitting , vspostdatum,vsontvangstdatum)
                WFCurrentCase.SetProperty("lijstbesluit_gemeente", loLijst.Gemeente)
                WFCurrentCase.SetProperty("lijstbesluit_zitting", loLijst.DatumZitting)
                WFCurrentCase.SetProperty("lijstbesluit_postdatum", loLijst.PostDatum)
                WFCurrentCase.SetProperty("initiële vervaltermijn", loLijst.InitieleTermijn)
                If String.IsNullOrEmpty(loLijst.OntvangstDatum) Then
                    WFCurrentCase.SetProperty("lijst_ontvangstdatum", loLijst.OntvangstDatum)
                End If
                WFCurrentCase.SetProperty("inzend_BESL_nr", "")
                WFCurrentCase.SetProperty("ander_BESL_nr", "")
            Else
                WFCurrentCase.SetProperty("lijstbesluit_gemeente", "")
                WFCurrentCase.SetProperty("lijst_ontvangstdatum", "")
                WFCurrentCase.SetProperty("lijstbesluit_postdatum", "")
                WFCurrentCase.SetProperty("initiële vervaltermijn", "")
                WFCurrentCase.SetProperty("lijstbesluit_zitting", "")
                WFCurrentCase.SetProperty("datum besluit", "")
                WFCurrentCase.SetProperty("type/soort besluit", "")
                WFCurrentCase.SetProperty("Beslissingsorgaan", "")
                WFCurrentCase.SetProperty("titel besluit", "")
            End If

        End If

        If lsvoorwerp = "inzendingsplichtig besluit" Then

            WFCurrentCase.SetPropertyVisible("inzend_BESL_nr", True)
            WFCurrentCase.SetPropertyVisible("lijstbesluit_nr", False)
            WFCurrentCase.SetPropertyVisible("ander_BESL_nr", False)
            WFCurrentCase.SetPropertyVisible("lijstbesluit_gemeente", False)
            WFCurrentCase.SetPropertyVisible("lijst_ontvangstdatum", True)
            WFCurrentCase.SetPropertyVisible("lijstbesluit_postdatum", True)
            WFCurrentCase.SetPropertyVisible("initiële vervaltermijn", True)
            WFCurrentCase.SetPropertyVisible("lijstbesluit_zitting", False)
            WFCurrentCase.SetPropertyVisible("datum besluit", True)
            WFCurrentCase.SetPropertyVisible("type/soort besluit", True)
            WFCurrentCase.SetPropertyVisible("Beslissingsorgaan", False)
            WFCurrentCase.SetPropertyVisible("titel besluit", False)
            WFCurrentCase.SetPropertyVisible("lijst_kortomschrijving", True)
            WFCurrentCase.SetPropertyVisible("boekjaar", True)
            WFCurrentCase.SetPropertyVisible("hoeveelste", True)

            Dim lsMeldingslijstNr As String = WFCurrentCase.GetProperty(Of String)("inzend_BESL_nr")
            'msgbox vsMeldingslijstNr
            Arco.Utils.Logging.Log("MeldingslijstNr = " & lsMeldingslijstNr)
            If Not String.IsNullOrEmpty(lsMeldingslijstNr) Then
                Dim loBesluit As InzendingsPlichtigBesluit = InzendingsPlichtigBesluit.GetInzendingsPlichtigBesluit(Convert.ToInt32(lsMeldingslijstNr))

                'Call Haalmeldingslijst(vsMeldingslijstNr , vsgemeente , vsdatumzitting , vspostdatum) 
                'Sub Haalinzendingsplichtigelijst(ByVal lsft_cid ,ByRef  vsdatum_besluit, ByRef vssoort_besluit, ByRef vspost_datum, ByRef vsdatum_in, ByRef vsinitiele_termijn)

                WFCurrentCase.SetProperty("datum besluit", loBesluit.DatumBesluit)
                WFCurrentCase.SetProperty("type/soort besluit", loBesluit.SoortBesluit)
                WFCurrentCase.SetProperty("lijstbesluit_postdatum", loBesluit.PostDatum)
                Try
                    If loBesluit.DatumIn <> String.Empty Then
                        loBesluit.DatumIn = loBesluit.DatumIn.Replace(" 00:00:00", "")
                        loBesluit.DatumIn = loBesluit.DatumIn.Replace(" 0:00:00", "")
                    End If
                Catch
                End Try
                WFCurrentCase.SetProperty("lijst_ontvangstdatum", loBesluit.DatumIn)
                WFCurrentCase.SetProperty("initiële vervaltermijn", loBesluit.InitieleTermijn)
                If String.IsNullOrEmpty(loBesluit.BoekJaar) OrElse IsNumeric(loBesluit.BoekJaar) = False Then
                    WFCurrentCase.SetProperty("boekjaar", 0)
                Else
                    WFCurrentCase.SetProperty("boekjaar", CInt(loBesluit.BoekJaar))
                End If
                WFCurrentCase.SetProperty("hoeveelste", loBesluit.Hoeveelste)
                WFCurrentCase.SetProperty("lijst_kortomschrijving", loBesluit.KorteOmschrijving)
                WFCurrentCase.SetProperty("ander_BESL_nr", "")
                WFCurrentCase.SetProperty("lijstbesluit_nr", "")
            Else
                WFCurrentCase.SetProperty("lijstbesluit_gemeente", "")
                WFCurrentCase.SetProperty("lijst_ontvangstdatum", "")
                WFCurrentCase.SetProperty("lijstbesluit_postdatum", "")
                WFCurrentCase.SetProperty("initiële vervaltermijn", "")
                WFCurrentCase.SetProperty("lijstbesluit_zitting", "")
                WFCurrentCase.SetProperty("datum besluit", "")
                WFCurrentCase.SetProperty("type/soort besluit", "")
                WFCurrentCase.SetProperty("Beslissingsorgaan", "")
                WFCurrentCase.SetProperty("titel besluit", "")
                WFCurrentCase.SetProperty("boekjaar", 0)
                WFCurrentCase.SetProperty("hoeveelste", "")
                WFCurrentCase.SetProperty("lijst_kortomschrijving", "")
            End If
        End If

        If lsvoorwerp = "ander besluit" Then
            WFCurrentCase.SetPropertyVisible("gegevens besluit/lijst", True)
            WFCurrentCase.SetPropertyVisible("inzend_BESL_nr", False)
            WFCurrentCase.SetPropertyVisible("lijstbesluit_nr", False)
            WFCurrentCase.SetPropertyVisible("ander_BESL_nr", True)

            WFCurrentCase.SetPropertyVisible("lijstbesluit_gemeente", False)
            WFCurrentCase.SetPropertyVisible("lijst_ontvangstdatum", False)
            WFCurrentCase.SetPropertyVisible("lijstbesluit_postdatum", False)
            WFCurrentCase.SetPropertyVisible("initiële vervaltermijn", False)
            WFCurrentCase.SetPropertyVisible("lijstbesluit_zitting", False)
            WFCurrentCase.SetPropertyVisible("lijst_kortomschrijving", False)
            WFCurrentCase.SetPropertyVisible("boekjaar", False)
            WFCurrentCase.SetPropertyVisible("hoeveelste", False)

            WFCurrentCase.SetPropertyVisible("datum besluit", True)
            WFCurrentCase.SetPropertyVisible("type/soort besluit", False)
            WFCurrentCase.SetPropertyVisible("Beslissingsorgaan", True)
            WFCurrentCase.SetPropertyVisible("titel besluit", True)
            Dim lsMeldingslijstNr As String = WFCurrentCase.GetProperty(Of String)("ander_BESL_nr")
            If Not String.IsNullOrEmpty(lsMeldingslijstNr) Then
                Dim loLijst As AndereLijst = AndereLijst.GetAndereLijst(lsMeldingslijstNr)

                WFCurrentCase.SetProperty("Beslissingsorgaan", loLijst.BeslissingsOrgaan)
                WFCurrentCase.SetProperty("datum besluit", loLijst.DatumBesluit)
                WFCurrentCase.SetProperty("titel besluit", loLijst.TitelBesluit)
                WFCurrentCase.SetProperty("inzend_BESL_nr", "")
                WFCurrentCase.SetProperty("lijstbesluit_nr", "")

            Else
                WFCurrentCase.SetProperty("lijstbesluit_gemeente", "")
                WFCurrentCase.SetProperty("lijst_ontvangstdatum", "")
                WFCurrentCase.SetProperty("lijstbesluit_postdatum", "")
                WFCurrentCase.SetProperty("initiële vervaltermijn", "")
                WFCurrentCase.SetProperty("lijstbesluit_zitting", "")
                WFCurrentCase.SetProperty("datum besluit", "")
                WFCurrentCase.SetProperty("type/soort besluit", "")
                WFCurrentCase.SetProperty("Beslissingsorgaan", "")
                WFCurrentCase.SetProperty("titel besluit", "")
            End If
        End If
    End Sub
End Class
