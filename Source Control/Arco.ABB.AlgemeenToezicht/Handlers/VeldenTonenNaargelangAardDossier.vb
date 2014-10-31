Imports Arco.ABB.Common

Public Class VeldenTonenNaargelangAardDossier
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)



        Lijsten.BerekenLijsten(WFCurrentCase)

        Dim lsAardDossier As String = WFCurrentCase.GetProperty(Of String)("aard dossier")
        Dim lsvoorwerp As String = WFCurrentCase.GetProperty(Of String)("voorwerp")

        If lsAardDossier <> "klacht" Then
            WFCurrentCase.SetPropertyVisible("TITEL_klager", False)
            WFCurrentCase.SetPropertyVisible("klager ID", False)
            WFCurrentCase.SetPropertyVisible("klager_naam", False)
            WFCurrentCase.SetPropertyVisible("klager_voornaam", False)
            WFCurrentCase.SetPropertyVisible("klager_straatnr", False)
            WFCurrentCase.SetPropertyVisible("klager_postnummer", False)
            WFCurrentCase.SetPropertyVisible("klager_gemeente", False)
            WFCurrentCase.SetPropertyVisible("klager_email", False)
            WFCurrentCase.SetPropertyVisible("hoedanigheid", False)
            '	WFCurrentCase.SetPropertyVisible("Titel_beroep",False)
            WFCurrentCase.SetPropertyVisible("Lijn 12", False)
            WFCurrentCase.SetPropertyVisible("Titel_contactpersoon", True)
            WFCurrentCase.SetPropertyVisible("Contact persoon ID", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_naam", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_voornaam", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_straatnr", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_postnummer", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_gemeente", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_email", True)

        Else

            WFCurrentCase.SetPropertyVisible("TITEL_klager", True)
            WFCurrentCase.SetPropertyVisible("klager ID", True)
            WFCurrentCase.SetPropertyVisible("klager_naam", True)
            WFCurrentCase.SetPropertyVisible("klager_voornaam", True)
            WFCurrentCase.SetPropertyVisible("klager_straatnr", True)
            WFCurrentCase.SetPropertyVisible("klager_postnummer", True)
            WFCurrentCase.SetPropertyVisible("klager_gemeente", True)
            WFCurrentCase.SetPropertyVisible("klager_email", True)
            WFCurrentCase.SetPropertyVisible("hoedanigheid", True)
            '	WFCurrentCase.SetPropertyVisible("Titel_beroep",True)
            WFCurrentCase.SetPropertyVisible("Lijn 12", True)
            WFCurrentCase.SetPropertyVisible("Titel_contactpersoon", True)
            WFCurrentCase.SetPropertyVisible("Contact persoon ID", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_naam", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_voornaam", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_straatnr", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_postnummer", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_gemeente", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_email", True)

        End If



        If lsAardDossier <> "klacht" Then
            WFCurrentCase.SetPropertyVisible("postdatum klacht", False)
            WFCurrentCase.SetPropertyVisible("datum binnengekomen op afdeling", False)
            WFCurrentCase.SetPropertyVisible("medium", False)

            WFCurrentCase.SetProperty("postdatum klachT", "")
            WFCurrentCase.SetProperty("datum binnengekomen op afdeling", "")
            WFCurrentCase.SetProperty("medium", "")



        Else

            WFCurrentCase.SetPropertyVisible("postdatum klacht", True)
            WFCurrentCase.SetPropertyVisible("datum binnengekomen op afdeling", True)
            WFCurrentCase.SetPropertyVisible("medium", True)
        End If
        If lsvoorwerp = "inzendingsplichtig besluit" Then
            WFCurrentCase.SetPropertyVisible("Titel_contactpersoon", True)
            WFCurrentCase.SetPropertyVisible("Contact persoon ID", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_naam", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_voornaam", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_straatnr", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_postnummer", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_gemeente", True)
            WFCurrentCase.SetPropertyVisible("Contactpersoon_email", True)


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

            Dim lsMeldingslijstNr As String = WFCurrentCase.GetProperty(Of String)("inzend_BESL_nr")
            'msgbox vsMeldingslijstNr
            If Not String.IsNullOrEmpty(lsMeldingslijstNr) Then
                Dim loLijst As InzendingsPlichtigBesluit = InzendingsPlichtigBesluit.GetInzendingsPlichtigBesluit(Convert.ToInt32(lsMeldingslijstNr))

                'Call Haalmeldingslijst(vsMeldingslijstNr , vsgemeente , vsdatumzitting , vspostdatum) 
                'Sub Haalinzendingsplichtigelijst(ByVal lsft_cid ,ByRef  vsdatum_besluit, ByRef vssoort_besluit, ByRef vspost_datum, ByRef vsdatum_in, ByRef vsinitiele_termijn)
                WFCurrentCase.SetProperty("datum besluit", loLijst.DatumBesluit)
                WFCurrentCase.SetProperty("type/soort besluit", loLijst.SoortBesluit)
                WFCurrentCase.SetProperty("lijstbesluit_postdatum", loLijst.PostDatum)
                WFCurrentCase.SetProperty("lijst_ontvangstdatum", loLijst.DatumIn)
                WFCurrentCase.SetProperty("initiële vervaltermijn", loLijst.InitieleTermijn)
                WFCurrentCase.SetProperty("boekjaar", loLijst.BoekJaar)
                WFCurrentCase.SetProperty("hoeveelste", loLijst.Hoeveelste)
                WFCurrentCase.SetProperty("lijst_kortomschrijving", loLijst.KorteOmschrijving)
                WFCurrentCase.SetProperty("ander_BESL_nr", "")
                WFCurrentCase.SetProperty("lijstbesluit_nr", "")

                '03092010
                WFCurrentCase.SetProperty("besluit_ID", lsMeldingslijstNr)
                'todo : verify
                WFCurrentCase.SetProperty("bestuur_gemeente", loLijst.Hoeveelste)
                WFCurrentCase.SetProperty("type bestuur", loLijst.BoekJaar)

                'Sub Haalinzendingsplichtigelijst(ByVal lsft_cid ,ByRef  vsdatum_besluit, ByRef vssoort_besluit, ByRef vspost_datum, ByRef vsdatum_in, ByRef vsinitiele_termijn)
              

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
                '03092010
                WFCurrentCase.SetProperty("lijst_kortomschrijving", "")
                WFCurrentCase.SetProperty("besluit_ID", 0)
            End If
        End If


    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "VeldenTonenNaargelangAardDossier"
        End Get
    End Property
End Class
