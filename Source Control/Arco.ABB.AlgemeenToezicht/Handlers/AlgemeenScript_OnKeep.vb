Imports Arco.ABB.Common

Public Class AlgemeenScript_OnKeep
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)

        Dim loTermijnScript As SetTermijnen = New SetTermijnen
        loTermijnScript.Execute(WFCurrentCase)

        Dim loChangeName As SetDossierNaam = New SetDossierNaam
        loChangeName.ExecuteCode(WFCurrentCase)

        Toewijzigingen.CascadeToewijzing(WFCurrentCase)
        OphalenGegevens(WFCurrentCase)
        Lijsten.BerekenLijsten(WFCurrentCase)
        SetWeergaveinfoveld(WFCurrentCase)

        WFCurrentCase.SetPropertyVisible("lbOntvGemeenteRek_OCMW", False)
        WFCurrentCase.SetPropertyVisible("lbOpmGemeenteVerzend_Dat", False)
        WFCurrentCase.SetPropertyVisible("lbOpmerkingenGemeentebestuur", False)

        '- Om de 3 nieuwe velden zeker niet te laten zien bij alle dossiers die niet voldoen aan de nieuwe OCMW regeleing
        Dim gddatumBesluit As Date
        Try
            gddatumBesluit = CDate(WFCurrentCase.GetProperty(Of String)("datum besluit"))
        Catch ex As Exception
            gddatumBesluit = CDate("01/01/2013")
        End Try
        Dim lsoud As Long = DateDiff("d", gddatumBesluit, "01/01/2013")
        If lsoud <= 0 Then
            If (WFCurrentCase.GetProperty(Of String)("voorwerp") = "inzendingsplichtig besluit" AndAlso WFCurrentCase.GetProperty(Of String)("type/soort besluit") = "rekening" AndAlso WFCurrentCase.GetProperty(Of String)("type bestuur") = "OCMW") Then
                WFCurrentCase.SetPropertyVisible("lbOntvGemeenteRek_OCMW", True)
                WFCurrentCase.SetPropertyVisible("lbOpmerkingenGemeentebestuur", True)
                WFCurrentCase.SetPropertyVisible("lbOpmGemeenteVerzend_Dat", True)
            End If
        End If


        'Tonen Schorsingsgronden on keep
        Dim lsResultaatOnderzoek As String = WFCurrentCase.GetProperty(Of String)("resultaat onderzoek")

        If lsResultaatOnderzoek = "schorsing" Then
            WFCurrentCase.SetPropertyVisible("schorsingsgronden", True)
        Else
            WFCurrentCase.SetPropertyVisible("schorsingsgronden", False)
        End If

        If ((lsResultaatOnderzoek = "vernietiging") OrElse (WFCurrentCase.GetProperty(Of String)("ResultaatNH") = "vernietiging")) Then
            WFCurrentCase.SetPropertyVisible("vernietigingsgronden", True)
        Else
            WFCurrentCase.SetPropertyVisible("vernietigingsgronden", False)
        End If

        If (lsResultaatOnderzoek = "goedkeuring met ambtshalve wijzigingen") Then
            WFCurrentCase.SetPropertyVisible("redenen goedkeuring met ambtshalve wijzigingen", True)
        Else
            WFCurrentCase.SetPropertyVisible("redenen goedkeuring met ambtshalve wijzigingen", False)
        End If
        If (lsResultaatOnderzoek = "goedkeuring met wijzigingen na advies gemeenteraad") Then
            WFCurrentCase.SetPropertyVisible("redenen goedkeuring met wijzigingen na advies GR", True)
        Else
            WFCurrentCase.SetPropertyVisible("redenen goedkeuring met wijzigingen na advies GR", False)
        End If
        If (lsResultaatOnderzoek = "niet-goedkeuring (ambtshalve)") Then
            WFCurrentCase.SetPropertyVisible("redenen niet-goedkeuring (ambtshalve)", True)
        Else
            WFCurrentCase.SetPropertyVisible("redenen niet-goedkeuring (ambtshalve)", False)
        End If


        If (lsResultaatOnderzoek = "niet-goedkeuring (na advies gemeenteraad)") Then
            WFCurrentCase.SetPropertyVisible("redenen niet-goedkeuring (na advies gemeenteraad)", True)
        Else
            WFCurrentCase.SetPropertyVisible("redenen niet-goedkeuring (na advies gemeenteraad)", False)
        End If

        If WFCurrentCase.GetProperty(Of String)("aard dossier") = "klacht" Then
            WFCurrentCase.SetPropertyVisible("postdatum klacht", True)
            WFCurrentCase.SetPropertyVisible("datum binnengekomen op afdeling", True)
            WFCurrentCase.SetPropertyVisible("medium", True)
            If WFCurrentCase.GetProperty(Of String)("medium") = "mail" Then
                WFCurrentCase.SetPropertyVisible("via_welke_mailbox", True)
            Else
                WFCurrentCase.SetPropertyVisible("via_welke_mailbox", False)
            End If
            WFCurrentCase.SetPropertyVisible("TITEL_klager", True)
            WFCurrentCase.SetPropertyVisible("klager ID", True)
            WFCurrentCase.SetPropertyVisible("klager_naam", True)
            WFCurrentCase.SetPropertyVisible("klager_voornaam", True)
            WFCurrentCase.SetPropertyVisible("klager_straatnr", True)
            WFCurrentCase.SetPropertyVisible("klager_gemeente", True)
            WFCurrentCase.SetPropertyVisible("klager_email", True)
            WFCurrentCase.SetPropertyVisible("hoedanigheid", True)
            WFCurrentCase.SetPropertyVisible("klager_postnummer", True)
            WFCurrentCase.SetPropertyVisible("lijn0", True)
        Else
            WFCurrentCase.SetPropertyVisible("via_welke_mailbox", False)
            WFCurrentCase.SetPropertyVisible("postdatum klacht", False)
            WFCurrentCase.SetPropertyVisible("datum binnengekomen op afdeling", False)
            WFCurrentCase.SetPropertyVisible("medium", False)
            WFCurrentCase.SetPropertyVisible("TITEL_klager", False)
            WFCurrentCase.SetPropertyVisible("klager ID", False)
            WFCurrentCase.SetPropertyVisible("klager_naam", False)
            WFCurrentCase.SetPropertyVisible("klager_voornaam", False)
            WFCurrentCase.SetPropertyVisible("klager_straatnr", False)
            WFCurrentCase.SetPropertyVisible("klager_gemeente", False)
            WFCurrentCase.SetPropertyVisible("klager_email", False)
            WFCurrentCase.SetPropertyVisible("hoedanigheid", False)
            WFCurrentCase.SetPropertyVisible("klager_postnummer", False)
            WFCurrentCase.SetPropertyVisible("lijn0", False)
        End If
    End Sub


    Private Sub OphalenGegevens(ByVal WFCurrentCase As Arco.Doma.Library.Routing.cCase)
        'on error resume Next
       
        If WFCurrentCase.GetProperty(Of String)("Contactpersoon toevoegen") = "Ja" Then
            WFCurrentCase.SetPropertyVisible("contactpersoon_gemeente", True)
            WFCurrentCase.SetPropertyVisible("contactpersoon_naam", True)
            WFCurrentCase.SetPropertyVisible("contactpersoon_postnummer", True)
            WFCurrentCase.SetPropertyVisible("contactpersoon_straatnr", True)
            WFCurrentCase.SetPropertyVisible("contactpersoon_voornaam", True)
            WFCurrentCase.SetPropertyVisible("contactpersoon_email", True)
            WFCurrentCase.SetPropertyVisible("Contact persoon ID", True)

            'ophalen contactpersoon
            Dim lsklager2 As String = WFCurrentCase.GetProperty(Of String)("Contact persoon ID")
            Dim loContact As ContactPersoon = ContactPersoon.GetContactPersoon(lsklager2)
            If Not String.IsNullOrEmpty(loContact.Naam) Then
                WFCurrentCase.SetProperty("contactpersoon_gemeente", loContact.Gemeente)
                WFCurrentCase.SetProperty("contactpersoon_naam", loContact.Naam)
                WFCurrentCase.SetProperty("contactpersoon_postnummer", loContact.PostCode)
                WFCurrentCase.SetProperty("contactpersoon_straatnr", loContact.StraatNr)
                WFCurrentCase.SetProperty("contactpersoon_voornaam", loContact.VoorNaam)
                WFCurrentCase.SetProperty("contactpersoon_email", loContact.Email)
                ' terug leegmaken anders terug opgehaald
                WFCurrentCase.SetProperty("Contact persoon ID", "")
            End If
        Else
            WFCurrentCase.SetPropertyVisible("contactpersoon_gemeente", False)
            WFCurrentCase.SetPropertyVisible("contactpersoon_naam", False)
            WFCurrentCase.SetPropertyVisible("contactpersoon_postnummer", False)
            WFCurrentCase.SetPropertyVisible("contactpersoon_straatnr", False)
            WFCurrentCase.SetPropertyVisible("contactpersoon_voornaam", False)
            WFCurrentCase.SetPropertyVisible("contactpersoon_email", False)
            WFCurrentCase.SetPropertyVisible("Contact persoon ID", False)        
        End If

        Dim lsklager As String = WFCurrentCase.GetProperty(Of String)("klager ID")
        If Not String.IsNullOrEmpty(lsklager) Then
            Dim loContact As ContactPersoon = ContactPersoon.GetContactPersoon(lsklager)
            If Not String.IsNullOrEmpty(loContact.Naam) Then
                WFCurrentCase.SetProperty("klager_gemeente", loContact.Gemeente)
                WFCurrentCase.SetProperty("klager_naam", loContact.Naam)
                WFCurrentCase.SetProperty("klager_postnummer", loContact.PostCode)
                WFCurrentCase.SetProperty("klager_straatnr", loContact.StraatNr)
                WFCurrentCase.SetProperty("klager_voornaam", loContact.VoorNaam)
                WFCurrentCase.SetProperty("klager_email", loContact.Email)
                ' terug leegmaken anders terug opgehaald
                WFCurrentCase.SetProperty("klager ID", "")
            End If
        End If


        'ophalen bestuur
        Dim lsbestuur As String = WFCurrentCase.GetProperty(Of String)("bestuur")
        If Not String.IsNullOrEmpty(lsbestuur) Then
            Dim loBestuur As Bestuur = Bestuur.GetBestuur(lsbestuur)
            If Not String.IsNullOrEmpty(loBestuur.Naam) Then
                WFCurrentCase.SetProperty("bestuur_gemeente", loBestuur.Gemeente)
                WFCurrentCase.SetProperty("bestuur_naam", loBestuur.Naam)
                WFCurrentCase.SetProperty("bestuur_postnummer", loBestuur.PostCode)
                WFCurrentCase.SetProperty("bestuur_straatnr", loBestuur.StraatNr)
                WFCurrentCase.SetProperty("bestuur_ID", lsbestuur)
                ' terug leegmaken anders terug opgehaald
                WFCurrentCase.SetProperty("bestuur", "")
            End If
        End If
    End Sub

    Sub SetWeergaveinfoveld(ByVal WFCurrentCase As Arco.Doma.Library.Routing.cCase)
        Dim lsWeergave As String = Formatting.FormatAssignee(WFCurrentCase.GetProperty(Of String)("dossierbehandelaar"))
        'lsWeergave &= "<br>"
        lsWeergave &= Formatting.FormatAssignee(WFCurrentCase.GetProperty(Of String)("dossierbehandelaar2"))
        WFCurrentCase.SetProperty("HTMLweergave_infoveld", lsWeergave)
    End Sub


    Public Overrides ReadOnly Property Name As String
        Get
            Return "AlgemeenScript_OnKeep"
        End Get
    End Property
End Class
