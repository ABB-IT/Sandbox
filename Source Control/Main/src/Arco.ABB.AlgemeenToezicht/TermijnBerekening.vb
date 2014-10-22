Imports Arco.Doma.Library.Routing
Imports Arco.ABB.Common

Public Class TermijnBerekening

    Protected Property HuidigeTermijn As String
    Protected Property InitTermijn As String
    Protected Property StapNummer As Integer
    Protected Property PostDatumKlacht As String
    Protected Property DatumBesluit As String
    Protected Property LijstBesluitZitting As String
    Protected Property DatumRechtvaardigingBeslissing As String
    Protected Property DatumOntvangstStukken As String
    Protected Property TypeBestuur As String
    Protected Property SoortBesluit As String
    Protected Property OpvraagbriefVerstuurdOp As String
    Protected Property BriefVerzondenOp As String
    Protected Property Voorwerp As String

    Protected Property DatumbinnAntwSchorsing As String
    Protected Property DatumInontvangstSchorsing As String
    Protected Property PoststempelBestSchorsing As String
    Protected Property Medium As String
    Protected Property LijstbesluitPostdatum As String
    Protected Property Aangetekend As String
    Protected Property ResultaatOnderzoek As String
    Protected Property AntwoordNaSchorsing As String
    Protected Property StukkenOpvragen As Boolean
    Protected Property PostDatumStukken As String
    Protected Property DatumBinnenkomstAfdeling As String
    Protected Property DatumRechtvaardigingsbeslissing As String
    Protected Property ExtraTermijnMinister As String
    Protected Property TermijnStuiten As Boolean
    Protected Property AardDossier As String
    Protected Property LijstOntvangstDatum As String

    Protected Property Step_Due As String

    Private Sub ReadProperties(ByVal WFCurrentCase As cCase)
        HuidigeTermijn = WFCurrentCase.GetProperty(Of String)("huidige termijn")
        InitTermijn = WFCurrentCase.GetProperty(Of String)("initiële vervaltermijn")
        If InitTermijn.Contains("23:59:59") = False Then
            InitTermijn = String.Concat(InitTermijn, " 23:59:59")
        End If
        PostDatumKlacht = WFCurrentCase.GetProperty(Of String)("postdatum klacht")
        PostDatumStukken = WFCurrentCase.GetProperty(Of String)("postdatum stukken")
        LijstBesluitZitting = WFCurrentCase.GetProperty(Of String)("lijstbesluit_zitting")
        DatumBesluit = WFCurrentCase.GetProperty(Of String)("datum besluit")
        DatumRechtvaardigingBeslissing = WFCurrentCase.GetProperty(Of String)("DatumRechtvaardigingsbeslissing")
        DatumOntvangstStukken = WFCurrentCase.GetProperty(Of String)("datum ontvangst stukken")
        Voorwerp = WFCurrentCase.GetProperty(Of String)("voorwerp")
        TypeBestuur = WFCurrentCase.GetProperty(Of String)("type bestuur")
        SoortBesluit = WFCurrentCase.GetProperty(Of String)("type/soort besluit")
        OpvraagbriefVerstuurdOp = WFCurrentCase.GetProperty(Of String)("opvraagbrief verstuurd op")
        BriefVerzondenOp = WFCurrentCase.GetProperty(Of String)("brief verzonden op")
        DatumbinnAntwSchorsing = WFCurrentCase.GetProperty(Of String)("Datum binnenkomst antwoord bestuur na schorsing")
        DatumInontvangstSchorsing = WFCurrentCase.GetProperty(Of String)("Datum inontvangstname schorsing")
        PoststempelBestSchorsing = WFCurrentCase.GetProperty(Of String)("Poststempel van antwoord bestuur na schorsing")
        Medium = WFCurrentCase.GetProperty(Of String)("medium")
        LijstbesluitPostdatum = WFCurrentCase.GetProperty(Of String)("lijstbesluit_postdatum")
        Aangetekend = WFCurrentCase.GetProperty(Of String)("medium") 'todo : correct?
        ResultaatOnderzoek = WFCurrentCase.GetProperty(Of String)("resultaat onderzoek")
        AntwoordNaSchorsing = WFCurrentCase.GetProperty(Of String)("Antwoord van bestuur na schorsing")
        StukkenOpvragen = WFCurrentCase.GetProperty(Of Boolean)("Stukken opvragen?")

        DatumBinnenkomstAfdeling = WFCurrentCase.GetProperty(Of String)("datum binnengekomen op afdeling")
        DatumRechtvaardigingsbeslissing = WFCurrentCase.GetProperty(Of String)("DatumRechtvaardigingsbeslissing")
        ExtraTermijnMinister = WFCurrentCase.GetProperty(Of String)("Extra termijn minister")
        TermijnStuiten = WFCurrentCase.GetProperty(Of Boolean)("TermijnStuiten")
        AardDossier = WFCurrentCase.GetProperty(Of String)("aard dossier")
        LijstOntvangstDatum = WFCurrentCase.GetProperty(Of String)("lijst_ontvangstdatum")
    End Sub

    Public Sub ZetTermijnen(ByVal WFCurrentCase As cCase)

        Call ReadProperties(WFCurrentCase)
        'If String.IsNullOrEmpty(PostDatumKlacht) Then
        '    WFCurrentCase.RejectComment = "Datum verzending klacht moet ingevuld zijn"
        'End If
        'If String.IsNullOrEmpty(DatumBinnenkomstAfdeling) Then
        '    WFCurrentCase.RejectComment = "Datum binnenkomst klacht op afdeling moet ingevuld zijn"
        'End If

        AddToLog(WFCurrentCase, "Start Termijnberekening ...")
        StapNummer = StapNummers.GetStapNummer(WFCurrentCase)
        AddToLog(WFCurrentCase, "ZetTermijnen: Stapnummer = " & StapNummer)

        If Not IsNieuwSysteem(WFCurrentCase) Then
            SetTermijn2(WFCurrentCase)
        Else
            SetTermijn(WFCurrentCase)
        End If
        Call SetExtraTermijnMinister(WFCurrentCase)

        'aanpassing door GVO 2011/06/17 om de tijdigheid van dossier te kunnen achterhalen
        'msgbox " stap  nr " & getstapnr & "huidige termijn " & HuidigeTermijn 
        Dim liStapNr As Int32 = StapNummers.GetStapNummer(WFCurrentCase)
        If liStapNr = 10 Then
            WFCurrentCase.SetProperty("Termijn_NS", HuidigeTermijn)
        ElseIf liStapNr = 6 Then
            WFCurrentCase.SetProperty("Termijn_RO", HuidigeTermijn)
        End If
    End Sub

    Private Sub SetTermijn(ByVal WFCurrentCase As cCase)
        'termijn berekening
        AddToLog(WFCurrentCase, "ZetTermijnen: Entering SetTermijn.")
        If IsControleTeLaat(WFCurrentCase) Then
            'TE LAAT geen termijn
            GeenDeadline(WFCurrentCase)
        Else
            If Not String.IsNullOrEmpty(Voorwerp) AndAlso Not String.IsNullOrEmpty(TypeBestuur) Then
                Select Case UCase(Voorwerp)
                    Case "HANDELING"
                        SetTermijn_handelingen(WFCurrentCase)
                    Case "LIJSTBESLUIT"
                        SetTermijn_lijstbesluit(WFCurrentCase)
                    Case "INZENDINGSPLICHTIG BESLUIT"
                        SetTermijn_Inzendingsplichtig_besluit(WFCurrentCase)
                    Case "ANDER BESLUIT"
                        SetTermijn_Andere_besluiten(WFCurrentCase)
                    Case Else
                        'geen
                End Select
            End If
            SetDeadline(WFCurrentCase, HuidigeTermijn, HuidigeTermijn)
        End If
        AddToLog(WFCurrentCase, "ZetTermijnen: Exiting SetTermijn.")
    End Sub

    Private Sub SetTermijn2(ByVal WFCurrentCase As cCase)
        AddToLog(WFCurrentCase, "ZetTermijnen: Entering SetTermijn2.")
        If IsControleTeLaat(WFCurrentCase) Then
            'TE LAAT geen termijn
            GeenDeadline(WFCurrentCase)
        Else
            'termijn berekening
            If Not String.IsNullOrEmpty(Voorwerp) AndAlso Not String.IsNullOrEmpty(TypeBestuur) Then
                Select Case UCase(Voorwerp)
                    Case "HANDELING"
                        SetTermijn_handelingen(WFCurrentCase)
                    Case "LIJSTBESLUIT"
                        SetTermijn_lijstbesluit2(WFCurrentCase)
                    Case "INZENDINGSPLICHTIG BESLUIT"
                        SetTermijn_Inzendingsplichtig_besluit2(WFCurrentCase)
                    Case "ANDER BESLUIT"
                        SetTermijn_Andere_besluiten2(WFCurrentCase)
                    Case Else
                        'geen
                End Select
            End If
            SetDeadline(WFCurrentCase, HuidigeTermijn, HuidigeTermijn)
        End If
        AddToLog(WFCurrentCase, "ZetTermijnen: Exiting SetTermijn2.")
    End Sub

    Private Sub SetTermijn_handelingen(ByVal WFCurrentCase As cCase)
        'geen termijn voor handelingen
        GeenDeadline(WFCurrentCase)
    End Sub

    '**********************************************
    '***Sub SetTermijn_lijstbesluit() ***
    '**********************************************
    Sub SetTermijn_lijstbesluit(ByVal WFCurrentCase As cCase)

        AddToLog(WFCurrentCase, "ZetTermijnen: Entering SetTermijn_lijstbesluit.")

        If TypeBestuur = "OCMW" OrElse TypeBestuur = "OCMW Vereniging" OrElse TypeBestuur = "Bestuur van de eredienst" OrElse TypeBestuur = "Intergemeentelijk SamenwerkingsVerband" Then
            If StapNummer = 1 OrElse StapNummer = 2 Then
                If String.IsNullOrEmpty(OpvraagbriefVerstuurdOp) Then
                    'berekening termijn stap1
                    If TypeBestuur <> "" AndAlso InitTermijn <> "" Then
                        'lijstbesluit ingegeven
                        If TypeBestuur = "Intergemeentelijk SamenwerkingsVerband" Then
                            HuidigeTermijn = InitTermijn
                        Else
                            If Aangetekend = "aangetekende brief" Then
                                HuidigeTermijn = AddDays(PostDatumKlacht, 30)
                            Else
                                HuidigeTermijn = InitTermijn
                            End If
                        End If
                    Else
                        HuidigeTermijn = ""
                    End If
                Else
                    HuidigeTermijn = ""
                End If
            End If
            If StapNummer >= 3 AndAlso StapNummer <= 10 Then
                HuidigeTermijn = AddDays(DatumOntvangstStukken, 30)
                If StapNummer = 6 Or StapNummer = 9 Or StapNummer = 10 Then
                    If Not String.IsNullOrEmpty(BriefVerzondenOp) AndAlso ResultaatOnderzoek = "schorsing" Then
                        If TypeBestuur = "Intergemeentelijk SamenwerkingsVerband" Then
                            HuidigeTermijn = AddDays(DatumInontvangstSchorsing, 30)
                        Else
                            HuidigeTermijn = AddDays(BriefVerzondenOp, 100)
                        End If
                    End If
                    If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                        If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                            HuidigeTermijn = ""
                        End If
                    End If
                End If
            End If

            If StapNummer >= 10 AndAlso StapNummer <= 14 Then
                If Not String.IsNullOrEmpty(DatumbinnAntwSchorsing) Then
                    If (AntwoordNaSchorsing = "hervaststelling") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing met aanpassing") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing zonder aanpassing") Then
                        If (TypeBestuur = "Intergemeentelijk SamenwerkingsVerband") OrElse (TypeBestuur = "Bestuur van de eredienst") Then
                            HuidigeTermijn = AddDays(DatumbinnAntwSchorsing, 30)
                        Else
                            HuidigeTermijn = AddDays(DatumbinnAntwSchorsing, 50)
                        End If
                    Else
                        HuidigeTermijn = ""
                    End If
                End If
                If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                    If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                        HuidigeTermijn = ""
                    End If
                End If
                'Aanpassing 05/01/2009
                If StapNummer = 13 AndAlso Not String.IsNullOrEmpty(ResultaatOnderzoek) AndAlso Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                    HuidigeTermijn = ""
                End If

            End If
            If StapNummer >= 15 AndAlso StapNummer <= 17 Then
                'geen deadline meer
                GeenDeadline(WFCurrentCase)
            End If
        Else

            If StapNummer = 1 Or StapNummer = 2 Then
                If String.IsNullOrEmpty(OpvraagbriefVerstuurdOp) Then
                    If Aangetekend = "aangetekende brief" Then
                        HuidigeTermijn = AddDays(PostDatumKlacht, 30)
                    Else
                        HuidigeTermijn = InitTermijn
                    End If
                Else
                    HuidigeTermijn = ""
                End If
            End If

            If StapNummer >= 3 AndAlso StapNummer <= 10 Then
                If Not String.IsNullOrEmpty(PostDatumStukken) Then
                    HuidigeTermijn = AddDays(PostDatumStukken, 32)
                End If
                If StapNummer = 6 Or StapNummer = 9 Or StapNummer = 10 Then
                    If ResultaatOnderzoek = "schorsing" AndAlso Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                        HuidigeTermijn = AddDays(BriefVerzondenOp, 100)
                    End If
                    If (ResultaatOnderzoek <> "schorsing" AndAlso ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                        If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                            HuidigeTermijn = ""
                        End If
                    End If
                End If
            End If
            If StapNummer >= 10 AndAlso StapNummer <= 14 Then

                If Not String.IsNullOrEmpty(DatumbinnAntwSchorsing) AndAlso Not String.IsNullOrEmpty(PoststempelBestSchorsing) Then
                    If (AntwoordNaSchorsing = "hervaststelling") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing met aanpassing") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing zonder aanpassing") Then

                        'aangepast 30092008
                        'HuidigeTermijn=""
                        HuidigeTermijn = AddDays(PoststempelBestSchorsing, 52)
                    Else
                        HuidigeTermijn = ""
                    End If
                    'aangepast 05012009
                    If StapNummer = 13 AndAlso Not String.IsNullOrEmpty(ResultaatOnderzoek) AndAlso Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                        HuidigeTermijn = ""
                    End If
                End If

            End If
            If StapNummer >= 15 AndAlso StapNummer <= 17 Then
                'geen deadline meer
                GeenDeadline(WFCurrentCase)
            End If
        End If
        AddToLog(WFCurrentCase, "ZetTermijnen: Exiting SetTermijn_lijstbesluit.")
    End Sub
    ''aanpassing 23062009
    '************************
    'SetTermijn_lijstbesluit2()
    '************************
    Sub SetTermijn_lijstbesluit2(ByVal WFCurrentCase As cCase)

        AddToLog(WFCurrentCase, "ZetTermijnen: Entering SetTermijn_lijstbesluit2.")

        If TypeBestuur = "Bestuur van de eredienst" OrElse TypeBestuur = "Intergemeentelijk SamenwerkingsVerband" Then
            If StapNummer = 1 OrElse StapNummer = 2 Then
                If String.IsNullOrEmpty(OpvraagbriefVerstuurdOp) Then
                    'berekening termijn stap1
                    If Not String.IsNullOrEmpty(TypeBestuur) AndAlso Not String.IsNullOrEmpty(InitTermijn) Then
                        'lijstbesluit ingegeven
                        If TypeBestuur = "Intergemeentelijk SamenwerkingsVerband" Then
                            HuidigeTermijn = InitTermijn
                        Else
                            If Aangetekend = "aangetekende brief" Then
                                HuidigeTermijn = AddDays(PostDatumKlacht, 30)
                            Else
                                HuidigeTermijn = InitTermijn
                            End If
                        End If
                    Else
                        HuidigeTermijn = ""
                    End If
                Else
                    HuidigeTermijn = ""
                End If
            End If
            If StapNummer >= 3 AndAlso StapNummer <= 10 Then
                If Not String.IsNullOrEmpty(DatumOntvangstStukken) Then
                    HuidigeTermijn = AddDays(DatumOntvangstStukken, 30)
                Else
                    HuidigeTermijn = ""
                End If
                If StapNummer = 6 OrElse StapNummer = 9 OrElse StapNummer = 10 Then
                    If Not String.IsNullOrEmpty(BriefVerzondenOp) AndAlso Not String.IsNullOrEmpty(ResultaatOnderzoek) Then
                        If TypeBestuur = "Intergemeentelijk SamenwerkingsVerband" Then
                            HuidigeTermijn = AddDays(DatumInontvangstSchorsing, 30)
                        Else
                            HuidigeTermijn = AddDays(BriefVerzondenOp, 100)
                        End If
                    End If
                    If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                        If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                            HuidigeTermijn = ""
                        End If
                    End If
                End If
            End If
            If StapNummer >= 10 AndAlso StapNummer <= 14 Then
                If Not String.IsNullOrEmpty(DatumbinnAntwSchorsing) Then
                    If (AntwoordNaSchorsing = "hervaststelling") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing met aanpassing") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing zonder aanpassing") Then
                        If (TypeBestuur = "Intergemeentelijk SamenwerkingsVerband") OrElse (TypeBestuur = "Bestuur van de eredienst") Then
                            HuidigeTermijn = AddDays(DatumbinnAntwSchorsing, 30)
                        Else
                            HuidigeTermijn = AddDays(DatumbinnAntwSchorsing, 50)
                        End If
                    Else
                        HuidigeTermijn = ""
                    End If
                End If
                If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                    If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                        HuidigeTermijn = ""
                    End If
                End If
                'aangepast 05/01/2009
                If StapNummer = 13 AndAlso Not String.IsNullOrEmpty(ResultaatOnderzoek) AndAlso Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                    HuidigeTermijn = ""
                End If
            End If
            If StapNummer >= 15 AndAlso StapNummer <= 17 Then
                'geen deadline meer
                GeenDeadline(WFCurrentCase)
            End If
        Else

            ' andere besturen ( gemeenten, ocmw's , e.a)
            If StapNummer = 1 OrElse StapNummer = 2 Then

                If String.IsNullOrEmpty(OpvraagbriefVerstuurdOp) Then
                    If Aangetekend = "aangetekende brief" Then
                        ''aanpassing 23/06/2009
                        HuidigeTermijn = AddDays(DatumBinnenkomstAfdeling, 29)
                        ''eind aanpassing 23/06/2009
                    Else
                        HuidigeTermijn = InitTermijn
                    End If
                Else
                    HuidigeTermijn = ""
                End If
            End If
            If StapNummer >= 3 AndAlso StapNummer <= 10 Then
                If Not String.IsNullOrEmpty(PostDatumStukken) Then
                    HuidigeTermijn = AddDays(PostDatumStukken, 32)
                Else
                    ''begin 15/11/2013
                    HuidigeTermijn = ""
                End If
                If StapNummer = 6 OrElse StapNummer = 9 OrElse StapNummer = 10 Then
                    If ResultaatOnderzoek = "schorsing" AndAlso Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                        ''aanpassing 23/06/2009
                        HuidigeTermijn = AddDays(BriefVerzondenOp, 62)
                        '' eind aanpassing 23/06/2009
                    End If
                    If (ResultaatOnderzoek <> "schorsing" AndAlso ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                        If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                            HuidigeTermijn = ""
                        End If
                    End If
                End If
            End If

            If StapNummer >= 10 AndAlso StapNummer <= 14 Then
                If Not String.IsNullOrEmpty(DatumbinnAntwSchorsing) AndAlso Not String.IsNullOrEmpty(PoststempelBestSchorsing) Then
                    If (AntwoordNaSchorsing = "hervaststelling") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing met aanpassing") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing zonder aanpassing") Then
                        ''aanpassing 23/06/2009
                        HuidigeTermijn = AddDays(PoststempelBestSchorsing, 32)
                        ''eind aanpassing 23/06/2009
                    Else
                        HuidigeTermijn = ""
                    End If
                    'aangepast 05/01/2009
                    If StapNummer = 13 AndAlso Not String.IsNullOrEmpty(ResultaatOnderzoek) AndAlso Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                        HuidigeTermijn = ""
                    End If
                End If
            End If

            If StapNummer >= 15 AndAlso StapNummer <= 17 Then
                'geen deadline meer
                GeenDeadline(WFCurrentCase)
            End If
        End If
        AddToLog(WFCurrentCase, "ZetTermijnen: Exiting SetTermijn_lijstbesluit2.")
    End Sub

    '**********************************************
    '***Sub SetTermijn_Inzendingsplichtig_besluit()***
    '**********************************************
    Sub SetTermijn_Inzendingsplichtig_besluit(ByVal WFCurrentCase As cCase)

        AddToLog(WFCurrentCase, "ZetTermijnen: Entering SetTermijn_Inzendingsplichtig_besluit.")
        If SoortBesluit = "rekening" Then
            If TypeBestuur = "Intergemeentelijk SamenwerkingsVerband" Then
                If StapNummer = 1 OrElse StapNummer = 2 Then
                    If String.IsNullOrEmpty(OpvraagbriefVerstuurdOp) Then
                        'bij IGS kunnen er bijkomende inlichtingen gevraagd worden die termijn stuiten
                        If Not String.IsNullOrEmpty(TypeBestuur) AndAlso Not String.IsNullOrEmpty(LijstbesluitPostdatum) Then
                            'lijstbesluit ingegeven
                            HuidigeTermijn = InitTermijn
                        Else
                            'nog geen deadline
                            HuidigeTermijn = ""
                        End If
                    Else
                        ''aanpassing 30062009
                        If TermijnStuiten = True Then
                            HuidigeTermijn = ""
                        Else
                            HuidigeTermijn = InitTermijn
                        End If
                    End If

                End If
            Else
                If StapNummer >= 3 AndAlso StapNummer <= 9 Then
                    ''30/06/2009
                    If TermijnStuiten = True Then
                        If Not String.IsNullOrEmpty(DatumOntvangstStukken) Then
                            If TypeBestuur = "Bestuur van de eredienst" Then
                                'HuidigeTermijn = AddDays(DatumOntvangstStukken, 200)
                                ' hier gewoon 200 dagen bijtellen. Geen rekening houden met weekends.
                                HuidigeTermijn = CStr(DateAdd("d", 200, CDate(DatumOntvangstStukken)))
                            Else
                                HuidigeTermijn = AddDays(DatumOntvangstStukken, 300)
                            End If
                        Else
                            HuidigeTermijn = ""
                        End If
                    Else
                        HuidigeTermijn = InitTermijn
                    End If

                    If StapNummer = 6 OrElse StapNummer = 9 Then
                        If ResultaatOnderzoek = "schorsing" Then
                            If (Not String.IsNullOrEmpty(BriefVerzondenOp) AndAlso Not String.IsNullOrEmpty(DatumInontvangstSchorsing)) Then
                                HuidigeTermijn = AddDays(DatumInontvangstSchorsing, 30)
                            End If
                        End If

                        If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                            If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                                HuidigeTermijn = ""
                            End If
                        End If
                    End If
                End If

                If StapNummer >= 10 AndAlso StapNummer <= 14 Then
                    If Not String.IsNullOrEmpty(DatumbinnAntwSchorsing) Then
                        If (AntwoordNaSchorsing = "hervaststelling") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing met aanpassing") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing zonder aanpassing") Then
                            HuidigeTermijn = AddDays(DatumbinnAntwSchorsing, 30)
                        Else
                            HuidigeTermijn = ""
                        End If
                    Else
                        If (Not String.IsNullOrEmpty(BriefVerzondenOp) AndAlso Not String.IsNullOrEmpty(DatumInontvangstSchorsing)) Then
                            HuidigeTermijn = AddDays(DatumInontvangstSchorsing, 30)
                        End If
                    End If
                    If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                        If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                            HuidigeTermijn = ""
                        End If
                    End If
                End If

                If StapNummer >= 15 AndAlso StapNummer <= 17 Then
                    'geen deadline meer
                    HuidigeTermijn = ""
                    GeenDeadline(WFCurrentCase) ' DBE 2014/05/27
                End If
                '	Else
                '				''add on 26012009
                '				If StapNummer >=1 And  StapNummer <=14  Then
                '					If gdInitTermijn <> "" Then
                '						HuidigeTermijn=gdInitTermijn
                '					End If
                '					If StapNummer =13 Then
                '						If BriefVerzondenOp <> "" Then
                '							HuidigeTermijn=""
                '						End If
                '					End If
                '					
                '				End If
                '
                '				If StapNummer >=15 And StapNummer <= 17  Then
                '				'geen deadline meer
                '					HuidigeTermijn=""
                '				End If
            End If
        Else
            'alle andere inzendingsplichtige besluiten (= niet rekening)
            If TypeBestuur = "OCMW" OrElse TypeBestuur = "OCMW Vereniging" Then
                If StapNummer >= 1 AndAlso StapNummer <= 10 Then
                    If Not String.IsNullOrEmpty(TypeBestuur) AndAlso Not String.IsNullOrEmpty(LijstbesluitPostdatum) Then
                        'besluit ingegeven
                        HuidigeTermijn = InitTermijn
                    Else
                        'nog geen deadline
                        HuidigeTermijn = ""
                    End If
                    If StapNummer = 6 OrElse StapNummer = 9 OrElse StapNummer = 10 Then
                        If ResultaatOnderzoek = "schorsing" Then
                            If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                                HuidigeTermijn = AddDays(BriefVerzondenOp, 100)
                            End If
                        End If

                        If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                            If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                                HuidigeTermijn = ""
                            End If
                        End If
                    End If
                End If

                If StapNummer >= 10 AndAlso StapNummer <= 14 Then
                    If Not String.IsNullOrEmpty(DatumbinnAntwSchorsing) Then
                        If (AntwoordNaSchorsing = "hervaststelling") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing met aanpassing") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing zonder aanpassing") Then
                            HuidigeTermijn = AddDays(DatumbinnAntwSchorsing, 50)
                        Else
                            HuidigeTermijn = ""
                        End If
                    End If
                    If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                        If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                            HuidigeTermijn = ""
                        End If
                    End If
                    'aangepast 05012009
                    If StapNummer = 13 AndAlso Not String.IsNullOrEmpty(ResultaatOnderzoek) And Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                        HuidigeTermijn = ""
                    End If
                End If

                If StapNummer >= 15 AndAlso StapNummer <= 17 Then
                    'geen deadline meer
                    HuidigeTermijn = ""
                    GeenDeadline(WFCurrentCase) ' DBE 2014/05/27
                End If
            Else
                'gemeentebesturen e.a.
                If StapNummer = 1 OrElse StapNummer = 2 Then
                    If String.IsNullOrEmpty(OpvraagbriefVerstuurdOp) Then
                        If Aangetekend = "aangetekende brief" Then
                            HuidigeTermijn = AddDays(PostDatumKlacht, 50)
                        Else
                            HuidigeTermijn = InitTermijn
                        End If
                    Else
                        'Datum verzenden opvraagbrief is ingevuld
                        ''aanpassing 30062009
                        If TermijnStuiten = True Then
                            HuidigeTermijn = ""
                        Else
                            If Aangetekend = "aangetekende brief" Then
                                HuidigeTermijn = AddDays(PostDatumKlacht, 50)
                            Else
                                HuidigeTermijn = InitTermijn
                            End If
                        End If
                    End If
                End If

                If StapNummer >= 3 AndAlso StapNummer <= 10 Then
                    '' 30062009
                    If TermijnStuiten = True Then
                        If Not String.IsNullOrEmpty(PostDatumStukken) Then
                            HuidigeTermijn = AddDays(PostDatumStukken, 52)
                        Else
                            'Stukken zijn nog niet binnen.
                            HuidigeTermijn = ""
                        End If
                    Else
                        'Als er geen stukken worden opgevraagd, blijven de termijnen dezelfde als in vorige stappen.
                        If Aangetekend = "aangetekende brief" Then
                            HuidigeTermijn = AddDays(PostDatumKlacht, 50)
                        Else
                            HuidigeTermijn = InitTermijn
                        End If
                    End If

                    If StapNummer = 6 OrElse StapNummer = 9 OrElse StapNummer = 10 Then
                        If ResultaatOnderzoek = "schorsing" Then
                            HuidigeTermijn = AddDays(BriefVerzondenOp, 62)
                            'aangepast op 12/07/2010 n.a.v. dossier 2010-7029
                        End If

                        If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                            If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                                HuidigeTermijn = ""
                            End If
                        End If
                    End If
                End If
                If StapNummer >= 10 AndAlso StapNummer <= 14 Then
                    If Not String.IsNullOrEmpty(PoststempelBestSchorsing) Then
                        If (AntwoordNaSchorsing = "hervaststelling") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing met aanpassing") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing zonder aanpassing") Then
                            HuidigeTermijn = AddDays(PoststempelBestSchorsing, 52)
                        Else
                            HuidigeTermijn = ""
                        End If
                    End If

                    If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                        If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                            HuidigeTermijn = ""
                        End If
                    End If

                End If
                If StapNummer >= 15 AndAlso StapNummer <= 17 Then
                    'geen deadline meer
                    HuidigeTermijn = ""
                    GeenDeadline(WFCurrentCase) ' DBE 2014/05/27
                End If
            End If
            'End If
        End If
        AddToLog(WFCurrentCase, "ZetTermijnen: Exiting SetTermijn_Inzendingsplichtig_besluit.")

    End Sub
    Sub SetTermijn_Inzendingsplichtig_besluit2(ByVal WFCurrentCase As cCase)

        AddToLog(WFCurrentCase, "ZetTermijnen: Entering SetTermijn_Inzendingsplichtig_besluit2.")
        AddToLog(WFCurrentCase, "SoortBesluit = " & SoortBesluit)
        AddToLog(WFCurrentCase, "Typebestuur  = " & TypeBestuur)
        Dim lsOntvGemeenteRek_OCMW As String
        Dim lsOpmGemeenteVerzend_Dat As String
        'msgbox "2"
        If SoortBesluit = "rekening" Then
            'msgbox "3"
            If TypeBestuur = "Intergemeentelijk SamenwerkingsVerband" Then
                If StapNummer = 1 OrElse StapNummer = 2 Then
                    If String.IsNullOrEmpty(OpvraagbriefVerstuurdOp) Then

                        If Not String.IsNullOrEmpty(TypeBestuur) AndAlso Not String.IsNullOrEmpty(LijstbesluitPostdatum) Then
                            'lijstbesluit ingegeven
                            HuidigeTermijn = InitTermijn
                        Else
                            'nog geen deadline
                            'bij IGS kunnen er bijkomende inlichtingen gevraagd worden die termijn schorsen
                            HuidigeTermijn = ""
                        End If
                    Else
                        ''aanpassing 30062009
                        If TermijnStuiten = True Then
                            HuidigeTermijn = ""
                        Else
                            HuidigeTermijn = InitTermijn
                        End If

                    End If
                End If
            Else
                ' wijziging naar aanleiding van mail van Heidi 12/01/2011 18:17 punt 1

                '------------------------------------------------Aanpassingen voor OCMW
                'AANPASSINGEN OCMW***************************************************
                If (TypeBestuur = "OCMW" AndAlso (DateDiff("n", DatumBesluit, "1/1/2013") < 0)) Then
                    'If TypeBestuur = "OCMW"   Then
                    'msgbox "OCMW en rekening   " & TypeBestuur & "stapnr" & StapNummer
                    Dim lsTermijn As Long
                    lsOntvGemeenteRek_OCMW = WFCurrentCase.GetProperty(Of String)("lbOntvGemeenteRek_OCMW")
                    If WFCurrentCase.GetProperty(Of Boolean)("lbOpmerkingenGemeentebestuur") = False Then
                        If lsOntvGemeenteRek_OCMW <> "" Then
                            'HuidigeTermijn = DateAdd("d", 203, lsOntvGemeenteRek_OCMW)
                            HuidigeTermijn = AddDays(lsOntvGemeenteRek_OCMW, 203)
                            'msgbox " Termijn OCMW en rekening en ontvangstdatum bij gemeentebestuur ingevuld " & HuidigeTermijn

                            ''''''''''''''''''''''Uitwerking nodig voor stuiten en opvragen
                            If StapNummer >= 3 And StapNummer <= 9 Then
                                If TermijnStuiten = True Then
                                    If Not String.IsNullOrEmpty(DatumOntvangstStukken) Then
                                        lsTermijn = DateDiff("d", lsOntvGemeenteRek_OCMW, DatumOntvangstStukken)
                                        'msgbox "Termijn   " & lsTermijn 
                                        If lsTermijn > 50 Then
                                            HuidigeTermijn = AddDays(DatumOntvangstStukken, 153)
                                        Else
                                            'msgbox "ontvangstdatum" 
                                            HuidigeTermijn = AddDays(lsOntvGemeenteRek_OCMW, 203)
                                        End If
                                        'msgbox "lstermijn    = " & lsTermijn & "Datum Ontvangststukken " & DatumOntvangstStukken & " datum ontvangst gemeente" & lsOntvGemeenteRek_OCMW & "Huidige Termijn " & HuidigeTermijn 
                                    Else
                                        HuidigeTermijn = ""
                                    End If
                                Else
                                End If
                            Else
                            End If
                            '		msgbox "vervolg voor andere stappen " 
                            If StapNummer = 6 OrElse StapNummer = 9 Then
                                If ResultaatOnderzoek = "schorsing" Then
                                    If (Not String.IsNullOrEmpty(BriefVerzondenOp) AndAlso Not String.IsNullOrEmpty(DatumInontvangstSchorsing)) Then
                                        HuidigeTermijn = AddDays(DatumInontvangstSchorsing, 30)
                                    End If
                                End If
                                If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                                    If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                                        HuidigeTermijn = ""
                                    End If
                                End If
                            End If
                            '		msgbox " laatste stappen "
                            If StapNummer >= 10 And StapNummer <= 14 Then
                                If Not String.IsNullOrEmpty(DatumbinnAntwSchorsing) Then
                                    If (AntwoordNaSchorsing = "hervaststelling") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing met aanpassing") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing zonder aanpassing") Then
                                        HuidigeTermijn = AddDays(DatumbinnAntwSchorsing, 30)
                                    Else
                                        HuidigeTermijn = ""
                                    End If
                                Else
                                    If (Not String.IsNullOrEmpty(BriefVerzondenOp) AndAlso Not String.IsNullOrEmpty(DatumInontvangstSchorsing)) Then
                                        HuidigeTermijn = AddDays(DatumInontvangstSchorsing, 30)
                                    End If
                                End If
                                If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                                    If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                                        HuidigeTermijn = ""
                                    End If
                                End If
                            End If
                            If StapNummer >= 15 And StapNummer <= 17 Then
                                HuidigeTermijn = ""
                                GeenDeadline(WFCurrentCase) ' DBE 2014/05/27
                            End If
                            '		msgbox "definitieve aanpassing voor rekening OCMW en "
                            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''Einde uitwerking
                        Else                                   'ontvangstdatum rekening bij bestuur niet ingevuld
                            HuidigeTermijn = ""
                            ' Antwoord van ABB op vraag ter verduidelijking van de foutboodschap dd.13/06/2014: boodschap vervangen omdat ze niet duidelijk genoeg is.
                            'WFCurrentCase.RejectComment = "Geef de ontvangsdatum in waarop het gemeentebestuur de rekening ontving "
                            WFCurrentCase.RejectComment = "Geef de Overzendingsdatum Rekening OCMW aan Gemeentebestuur in"
                        End If

                        '       msgbox "uit" 
                    Else           'Opmerkingen gemaakt door gemeentebestuur
                        'msgbox " wel opmerkingen bestuur" 
                        lsOpmGemeenteVerzend_Dat = WFCurrentCase.GetProperty(Of String)("lbOpmGemeenteVerzend_Dat")
                        '		msgbox " datum is " & lsOpmGemeenteVerzend_Dat
                        If Not String.IsNullOrEmpty(lsOpmGemeenteVerzend_Dat) Then
                            Dim lsDatumOntvangst_50 As String
                            'msgbox " Ontvangsdatum = " &  lsOntvGemeenteRek_OCMW & "Verzenddatum " & lsOpmGemeenteVerzend_Dat
                            lsDatumOntvangst_50 = AddDays(lsOntvGemeenteRek_OCMW, 50)
                            'msgbox "ontvangstdatum + 50 = normaal 14/6/2013 + 50 =03/08/2013	" & lsDatumOntvangst_50 
                            '  lstest = DateDiff("d", lsDatumOntvangst_50, lsOpmGemeenteVerzend_Dat)
                            'msgbox " geef het verschil weer tussen ontvangdataum + 50 en verzenddatum  verschil = " & lstest & "ontvangstdatum = " & lsDatumOntvangst_50 & "verddatum = " & lsOpmGemeenteVerzend_Dat 
                            If DateDiff("d", lsDatumOntvangst_50, lsOpmGemeenteVerzend_Dat) < 1 Then
                                'msgbox " binnen de 50dagen na opmerlingen"                              
                                '	HuidigeTermijn = DateAdd("d", 153, lsOpmGemeenteVerzend_Dat )
                                HuidigeTermijn = AddDays(lsOpmGemeenteVerzend_Dat, 153)
                                ' uitwerken verder nodig
                                'msgbox " datum " & lsOpmGemeenteVerzend_Dat
                                If StapNummer >= 3 And StapNummer <= 9 Then
                                    If TermijnStuiten = True Then
                                        If Not String.IsNullOrEmpty(DatumOntvangstStukken) Then
                                            lsTermijn = DateDiff("d", lsOpmGemeenteVerzend_Dat, DatumOntvangstStukken)
                                            'msgbox " termijn " & lsTermijn
                                            If lsTermijn > 0 Then
                                                '	HuidigeTermijn = DateAdd("d", 153,DatumOntvangstStukken)
                                                HuidigeTermijn = AddDays(DatumOntvangstStukken, 153)
                                                'HuidigeTermijn = DateAdd("d", lsTermijn, HuidigeTermijn)
                                                'msgbox " huidige termijn " & HuidigeTermijn
                                            Else
                                                'HuidigeTermijn = DateAdd("d", 153,lsOpmGemeenteVerzend_Dat )
                                                HuidigeTermijn = AddDays(lsOpmGemeenteVerzend_Dat, 153)
                                                ' HuidigeTermijn = DateAdd("d", 153,lsOpmGemeenteVerzend_Dat )
                                            End If
                                            'msgbox "lstermijn bij opmerlingne   = " & lsTermijn & "Datum Ontvangststukken " & DatumOntvangstStukken & " datum ontvangst gemeente" & lsOntvGemeenteRek_OCMW & "Huidige Termijn " & HuidigeTermijn 
                                        Else
                                            HuidigeTermijn = ""
                                        End If
                                    Else
                                    End If
                                Else
                                End If
                                If StapNummer = 6 OrElse StapNummer = 9 Then
                                    If ResultaatOnderzoek = "schorsing" Then
                                        If (Not String.IsNullOrEmpty(BriefVerzondenOp) AndAlso Not String.IsNullOrEmpty(DatumInontvangstSchorsing)) Then
                                            HuidigeTermijn = AddDays(DatumInontvangstSchorsing, 30)
                                        End If
                                    End If
                                    If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                                        If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                                            HuidigeTermijn = ""
                                        End If
                                    End If
                                End If
                                'msgbox " laatste stappen tijdig "
                                If StapNummer >= 10 And StapNummer <= 14 Then
                                    If Not String.IsNullOrEmpty(DatumbinnAntwSchorsing) Then
                                        If (AntwoordNaSchorsing = "hervaststelling") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing met aanpassing") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing zonder aanpassing") Then
                                            HuidigeTermijn = AddDays(DatumbinnAntwSchorsing, 30)
                                        Else
                                            HuidigeTermijn = ""
                                        End If
                                    Else
                                        If (Not String.IsNullOrEmpty(BriefVerzondenOp) AndAlso Not String.IsNullOrEmpty(DatumInontvangstSchorsing)) Then
                                            HuidigeTermijn = AddDays(DatumInontvangstSchorsing, 30)
                                        End If
                                    End If
                                    If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                                        If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                                            HuidigeTermijn = ""
                                        End If
                                    End If
                                End If
                                If StapNummer >= 15 And StapNummer <= 17 Then
                                    HuidigeTermijn = ""
                                    GeenDeadline(WFCurrentCase) ' DBE 2014/05/27
                                End If

                                'Einde uitwerking
                            Else
                                'uitwerking verder nodig
                                'msgbox "opmerkingen maar te laat"
                                '				HuidigeTermijn = DateAdd("d", 203, lsOntvGemeenteRek_OCMW)
                                HuidigeTermijn = AddDays(lsOntvGemeenteRek_OCMW, 203)

                                If StapNummer >= 3 And StapNummer <= 9 Then
                                    If TermijnStuiten = True Then
                                        If Not String.IsNullOrEmpty(DatumOntvangstStukken) Then
                                            lsTermijn = DateDiff("d", lsOntvGemeenteRek_OCMW, DatumOntvangstStukken)

                                            If lsTermijn > 0 Then
                                                HuidigeTermijn = AddDays(lsOntvGemeenteRek_OCMW, 203)
                                                HuidigeTermijn = AddDays(HuidigeTermijn, Convert.ToInt32(lsTermijn))
                                            Else
                                                HuidigeTermijn = AddDays(lsOntvGemeenteRek_OCMW, 203)
                                            End If
                                            'msgbox "lstermijn bij opmerlingne   = " & lsTermijn & "Datum Ontvangststukken " & DatumOntvangstStukken & " datum ontvangst gemeente" & lsOntvGemeenteRek_OCMW & "Huidige Termijn " & HuidigeTermijn 
                                        Else
                                            HuidigeTermijn = ""
                                        End If
                                    Else
                                    End If
                                Else
                                End If
                                'msgbox "vervolg voor andere stappen " 
                                If StapNummer = 6 OrElse StapNummer = 9 Then
                                    If ResultaatOnderzoek = "schorsing" Then
                                        If (Not String.IsNullOrEmpty(BriefVerzondenOp) AndAlso Not String.IsNullOrEmpty(DatumInontvangstSchorsing)) Then
                                            HuidigeTermijn = AddDays(DatumInontvangstSchorsing, 30)
                                        End If
                                    End If
                                    If (ResultaatOnderzoek <> "schorsing") And (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                                        If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                                            HuidigeTermijn = ""
                                        End If
                                    End If
                                End If

                                'msgbox " laatste stappen "
                                If StapNummer >= 10 AndAlso StapNummer <= 14 Then
                                    If Not String.IsNullOrEmpty(DatumbinnAntwSchorsing) Then
                                        If (AntwoordNaSchorsing = "hervaststelling") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing met aanpassing") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing zonder aanpassing") Then
                                            HuidigeTermijn = AddDays(DatumbinnAntwSchorsing, 30)
                                        Else
                                            HuidigeTermijn = ""
                                        End If
                                    Else
                                        If (Not String.IsNullOrEmpty(BriefVerzondenOp) AndAlso Not String.IsNullOrEmpty(DatumInontvangstSchorsing)) Then
                                            HuidigeTermijn = AddDays(DatumInontvangstSchorsing, 30)
                                        End If
                                    End If
                                    If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                                        If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                                            HuidigeTermijn = ""
                                        End If
                                    End If
                                End If
                                If StapNummer >= 15 And StapNummer <= 17 Then
                                    HuidigeTermijn = ""
                                    GeenDeadline(WFCurrentCase) ' DBE 2014/05/27
                                End If
                                'Einde uitwerking
                            End If  'verzendatum kleiner dan 50 dagen
                        Else   ' wanneer verzenddatum niet is ingevuld

                            '			msgbox " Geef een boodschap en duid de verzenddatum aan"
                            WFCurrentCase.RejectComment = "Geef de verzenddatum in waarop het gemeentebestuur de opmerkingen over de rekening van het OCMW verstuurde naar ABB "
                            '			msgbox "3"
                        End If       'verzenddatum niet ingevuld

                    End If          'voor opmerkingen door bestuur
                    'msgbox "huidige termijn " & HuidigeTermijn	

                    'msgbox "2"
                Else                   'voor gevallen geen OCMW maar wel rekening'

                    'msgbox " rekening eventueel OCMW maar voor 01/01/2013"
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
                    '----------------------------Einde Aanpassingen OCMW's

                    HuidigeTermijn = InitTermijn

                    'msgbox "kom ik hier nog"
                    AddToLog(WFCurrentCase, "TermijnStuiten = " & TermijnStuiten)
                    AddToLog(WFCurrentCase, "DatumOntvangstStukken = " & DatumOntvangstStukken)

                    If StapNummer >= 3 And StapNummer <= 9 Then
                        ''aanpassing 30062009
                        If TermijnStuiten = True Then
                            If Not String.IsNullOrEmpty(DatumOntvangstStukken) Then
                                If TypeBestuur = "Bestuur van de eredienst" Then
                                    ' hier gewoon 200 dagen bijtellen. Geen rekening houden met weekends.
                                    HuidigeTermijn = CStr(DateAdd("d", 200, CDate(DatumOntvangstStukken)))
                                    'HuidigeTermijn = AddDays(DatumOntvangstStukken, 200)
                                Else
                                    If DateDiff("n", DatumBesluit, "1/1/2013") < 1 Then
                                        If TypeBestuur = "Gemeentebestuur" Then
                                            'msgbox "2"
                                            ' msgbox gdPostdatumStukken Indien posdatumstukken niet is ingevuld gaat dit fouten geven moet dit nog voorzien worden.
                                            HuidigeTermijn = AddDays(PostDatumStukken, 152)
                                        Else
                                            HuidigeTermijn = AddDays(DatumOntvangstStukken, 152)
                                        End If
                                        'later
                                    Else
                                        HuidigeTermijn = AddDays(DatumOntvangstStukken, 300)
                                    End If
                                End If
                            Else
                                HuidigeTermijn = ""
                            End If
                        Else
                            'Geval 1 in de excellijst
                            'msgbox "niet stuiten" zie mail 16/09/2012 11:05 van Stefanie Kerkhofs
                            ' test in commentaar gezet op vraag van GVO op 28/05/2014.
                            'If (DateDiff("n", DatumBesluit, "1/1/2013") < 1 AndAlso TypeBestuur = "Gemeentebestuur") Then
                            'HuidigeTermijn = AddDays(LijstbesluitPostdatum, 152)
                            'msgbox "binnen en postdatum stukken " 	& gdLijstbesluitPostdatum
                            'Else
                            HuidigeTermijn = InitTermijn
                            'End If
                            'msgbox  "stuiten"
                        End If
                        AddToLog(WFCurrentCase, "HuidigeTermijn = " & HuidigeTermijn)
                        AddToLog(WFCurrentCase, "InitTermijn = " & InitTermijn)

                        If StapNummer = 6 OrElse StapNummer = 9 Then
                            If ResultaatOnderzoek = "schorsing" Then
                                If (Not String.IsNullOrEmpty(BriefVerzondenOp) AndAlso Not String.IsNullOrEmpty(DatumInontvangstSchorsing)) Then
                                    HuidigeTermijn = AddDays(DatumInontvangstSchorsing, 30)
                                End If
                            End If
                            If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                                If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                                    HuidigeTermijn = ""
                                End If
                            End If
                        End If
                    End If

                    If StapNummer >= 10 AndAlso StapNummer <= 14 Then
                        If Not String.IsNullOrEmpty(DatumbinnAntwSchorsing) Then
                            If (AntwoordNaSchorsing = "hervaststelling") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing met aanpassing") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing zonder aanpassing") Then
                                HuidigeTermijn = AddDays(DatumbinnAntwSchorsing, 30)
                            Else
                                HuidigeTermijn = ""
                            End If
                        Else
                            If (Not String.IsNullOrEmpty(BriefVerzondenOp) AndAlso Not String.IsNullOrEmpty(DatumInontvangstSchorsing)) Then
                                HuidigeTermijn = AddDays(DatumInontvangstSchorsing, 30)
                            End If
                        End If
                        If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                            If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                                HuidigeTermijn = ""
                            End If
                        End If
                    End If
                    If StapNummer >= 15 AndAlso StapNummer <= 17 Then
                        'geen deadline meer
                        HuidigeTermijn = ""
                        GeenDeadline(WFCurrentCase) ' DBE 2014/05/27
                    End If
                    '	Else
                    '				''add on 26012009
                    '				If StapNummer >=1 And  StapNummer <=14  Then
                    '					If InitTermijn <> "" Then
                    '						HuidigeTermijn=InitTermijn
                    '					End If
                    '					If StapNummer =13 Then
                    '						If BriefVerzondenOp <> "" Then
                    '							HuidigeTermijn=""
                    '						End If
                    '					End If
                    '				End If
                    '				If StapNummer >=15 And StapNummer <= 17  Then
                    '				'geen deadline meer
                    '					HuidigeTermijn=""
                    '				End If
                    '	msgbox HuidigeTermijn & " " & InitTermijn
                End If
                'msgbox  "End IF van   geen OCMW"
            End If            ''End IF van   geen OCMW"


            '----------------------
            '-----------------------
        Else
            'alle andere inzendingsplichtige besluiten (= niet rekening)
            'gemeentebesturen e.a.
            ''aanpassing 30/06/2009
            If StapNummer = 1 OrElse StapNummer = 2 Then
                If String.IsNullOrEmpty(OpvraagbriefVerstuurdOp) Then
                    If Aangetekend = "aangetekende brief" Then
                        ''aanpassing 23/06/2009
                        HuidigeTermijn = AddDays(DatumBinnenkomstAfdeling, 49)
                        ''eind aanpassing 23/06/2009
                    Else
                        HuidigeTermijn = InitTermijn
                    End If
                Else
                    ''aanpassing 30/06/2009
                    If TermijnStuiten = True Then
                        HuidigeTermijn = ""
                    Else
                        If Aangetekend = "aangetekende brief" Then
                            HuidigeTermijn = AddDays(DatumBinnenkomstAfdeling, 49)
                        Else
                            HuidigeTermijn = InitTermijn
                        End If
                    End If
                End If
            End If
            If StapNummer >= 3 AndAlso StapNummer <= 10 Then
                '' aanpassing 30/06/2009
                ' DBE 2014/05/27 zie geval 2 in excel. Moet checken op datum verzending stukken.
                If TermijnStuiten = True Then
                    If Not String.IsNullOrEmpty(PostDatumStukken) Then
                        AddToLog(WFCurrentCase, "PostDatumStukken = " & PostDatumStukken)
                        HuidigeTermijn = AddDays(PostDatumStukken, 52)
                        AddToLog(WFCurrentCase, "HuidigeTermijn = " & HuidigeTermijn)
                    Else
                        'Stukken zijn nog niet binnen.
                        HuidigeTermijn = ""
                    End If
                Else
                    'Als er geen stukken worden opgevraagd, blijven de termijnen dezelfde als in vorige stappen.
                    If Aangetekend = "aangetekende brief" Then
                        ''aanpassing 23/06/2009
                        HuidigeTermijn = AddDays(DatumBinnenkomstAfdeling, 49)
                        ''eind aanpassing 23/06/2009
                    Else
                        HuidigeTermijn = InitTermijn
                    End If
                End If
                If StapNummer = 6 OrElse StapNummer = 9 OrElse StapNummer = 10 Then
                    If ResultaatOnderzoek = "schorsing" Then
                        ''aanpassing 23/06/2009
                        HuidigeTermijn = AddDays(BriefVerzondenOp, 62)
                        ''eind aanpassing 23/06/2009
                    End If
                    If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                        If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                            HuidigeTermijn = ""
                        End If
                    End If
                End If
            End If

            If StapNummer >= 10 AndAlso StapNummer <= 14 Then

                If Not String.IsNullOrEmpty(PoststempelBestSchorsing) Then
                    If (AntwoordNaSchorsing = "hervaststelling" OrElse AntwoordNaSchorsing = "rechtvaardigingsbeslissing zonder aanpassing" OrElse AntwoordNaSchorsing = "rechtvaardigingsbeslissing met aanpassing") Then
                        ''aanpassing 23/06/2009
                        HuidigeTermijn = AddDays(PoststempelBestSchorsing, 32)
                        ''eind aanpassing 23/06/2009
                    Else
                        HuidigeTermijn = ""
                    End If
                End If
                If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                    If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                        HuidigeTermijn = ""
                    End If
                End If
            End If
            If StapNummer >= 15 AndAlso StapNummer <= 17 Then
                'geen deadline meer
                HuidigeTermijn = ""
                GeenDeadline(WFCurrentCase) ' DBE 2014/05/27
            End If
        End If
        'msgbox "1"
        AddToLog(WFCurrentCase, "ZetTermijnen: Exiting SetTermijn_Inzendingsplichtig_besluit2.")
    End Sub


    '**********************************************
    '***Sub SetTermijn_Andere_besluiten()***
    '**********************************************
    Sub SetTermijn_Andere_besluiten(ByVal WFCurrentCase As cCase)

        AddToLog(WFCurrentCase, "ZetTermijnen: Entering SetTermijn_Andere_besluiten.")
        If TypeBestuur = "Bestuur van de eredienst" OrElse TypeBestuur = "Intergemeentelijk SamenwerkingsVerband" Then
            If StapNummer = 1 OrElse StapNummer = 2 Then
                GeenDeadline(WFCurrentCase)
            End If
            If StapNummer >= 3 AndAlso StapNummer <= 10 Then
                If Not String.IsNullOrEmpty(DatumOntvangstStukken) Then
                    HuidigeTermijn = AddDays(DatumOntvangstStukken, 30)
                    Step_Due = HuidigeTermijn
                Else
                    'Stukken zijn nog niet binnen.
                    HuidigeTermijn = ""
                End If
                If StapNummer = 6 OrElse StapNummer = 9 OrElse StapNummer = 10 Then
                    If ResultaatOnderzoek = "schorsing" Then
                        HuidigeTermijn = AddDays(BriefVerzondenOp, 100)
                    End If
                    If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                        If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                            HuidigeTermijn = ""
                        End If
                    End If
                End If
            End If

            If StapNummer >= 10 AndAlso StapNummer <= 14 Then
                If Not String.IsNullOrEmpty(DatumbinnAntwSchorsing) Then
                    If (AntwoordNaSchorsing = "hervaststelling") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing met aanpassing") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing zonder aanpassing") Then
                        HuidigeTermijn = AddDays(DatumbinnAntwSchorsing, 50)
                        Step_Due = HuidigeTermijn
                    Else
                        HuidigeTermijn = ""
                    End If
                End If
                If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                    If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                        HuidigeTermijn = ""
                    End If
                End If
                'aangepast 05/01/2009
                If StapNummer = 13 AndAlso Not String.IsNullOrEmpty(ResultaatOnderzoek) And Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                    HuidigeTermijn = ""
                End If
            End If
            If StapNummer >= 11 AndAlso StapNummer <= 14 Then
                HuidigeTermijn = AddDays(DatumbinnAntwSchorsing, 50)
                Step_Due = HuidigeTermijn
                'aangepast 05/01/2009
                If StapNummer = 13 AndAlso Not String.IsNullOrEmpty(ResultaatOnderzoek) And Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                    HuidigeTermijn = ""
                End If
            End If
            If StapNummer >= 15 AndAlso StapNummer <= 17 Then
                'geen deadline meer
                GeenDeadline(WFCurrentCase)
            End If
        Else
            If StapNummer = 1 OrElse StapNummer = 2 Then
                GeenDeadline(WFCurrentCase)
            End If
            If StapNummer >= 3 AndAlso StapNummer <= 10 Then
                If StukkenOpvragen = True Then
                    If Not String.IsNullOrEmpty(PostDatumStukken) Then
                        HuidigeTermijn = AddDays(PostDatumStukken, 32)
                    Else
                        'Stukken zijn nog niet binnen.
                        HuidigeTermijn = ""
                    End If
                Else
                    HuidigeTermijn = ""
                End If

                If StapNummer = 6 OrElse StapNummer = 9 OrElse StapNummer = 8 OrElse StapNummer = 10 Then
                    If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                        If ResultaatOnderzoek = "schorsing" Then
                            ''aanpassing 23062009
                            HuidigeTermijn = AddDays(BriefVerzondenOp, 62)
                            '' eind aanpassing 23062009
                        End If
                        If ResultaatOnderzoek = "onderzoekresultaat aan minister voor eindbeslissing" Then
                            HuidigeTermijn = AddDays(PostDatumStukken, 32)
                        End If
                        If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                            HuidigeTermijn = ""
                        End If
                    End If
                End If
            End If
            If StapNummer >= 10 AndAlso StapNummer <= 14 Then
                If Not String.IsNullOrEmpty(PoststempelBestSchorsing) Then
                    If (AntwoordNaSchorsing = "hervaststelling") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing met aanpassing") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing zonder aanpassing") Then
                        ''aanpassing 23/06/2009 bijkomende aanpassing voor hervaststelling, rechtvaardigingsbeslissing met en zonder aanpassing
                        HuidigeTermijn = AddDays(PoststempelBestSchorsing, 32)
                        ''eind aanpassing 23/06/2009
                    Else
                        HuidigeTermijn = ""
                    End If
                End If
                If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                    If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                        HuidigeTermijn = ""
                    End If
                End If
                'aangepast 05/01/2009
                If StapNummer = 13 AndAlso Not String.IsNullOrEmpty(ResultaatOnderzoek) AndAlso Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                    HuidigeTermijn = ""
                End If
            End If
            If StapNummer >= 15 AndAlso StapNummer <= 17 Then
                'geen deadline meer
                GeenDeadline(WFCurrentCase)
            End If
        End If
        AddToLog(WFCurrentCase, "ZetTermijnen: Exiting SetTermijn_Andere_besluiten.")

    End Sub
    ''aanpassing 23062009
    '************************
    'SetTermijn_Andere_besluiten2()
    '************************
    Sub SetTermijn_Andere_besluiten2(ByVal WFCurrentCase As cCase)
        AddToLog(WFCurrentCase, "ZetTermijnen: Entering SetTermijn_Andere_besluiten2.")
        If TypeBestuur = "Bestuur van de eredienst" OrElse TypeBestuur = "Intergemeentelijk SamenwerkingsVerband" Then
            If StapNummer = 1 OrElse StapNummer = 2 Then
                GeenDeadline(WFCurrentCase)
            End If
            If StapNummer >= 3 AndAlso StapNummer <= 10 Then
                If Not String.IsNullOrEmpty(DatumOntvangstStukken) Then
                    HuidigeTermijn = AddDays(DatumOntvangstStukken, 30)
                    Step_Due = HuidigeTermijn
                Else
                    'Stukken zijn nog niet binnen.
                    HuidigeTermijn = ""
                End If
                If StapNummer = 6 OrElse StapNummer = 9 OrElse StapNummer = 10 Then
                    If ResultaatOnderzoek = "schorsing" Then
                        HuidigeTermijn = AddDays(BriefVerzondenOp, 100)
                    End If
                    If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                        If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                            HuidigeTermijn = ""
                        End If
                    End If
                End If
            End If

            If StapNummer >= 10 AndAlso StapNummer <= 14 Then
                If Not String.IsNullOrEmpty(DatumbinnAntwSchorsing) Then
                    If (AntwoordNaSchorsing = "hervaststelling") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing met aanpassing") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing zonder aanpassing") Then
                        HuidigeTermijn = AddDays(DatumbinnAntwSchorsing, 50)
                        Step_Due = HuidigeTermijn
                    Else
                        HuidigeTermijn = ""
                    End If
                End If
                If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                    If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                        HuidigeTermijn = ""
                    End If
                End If
                'aangepast 05/01/2009
                If StapNummer = 13 AndAlso Not String.IsNullOrEmpty(ResultaatOnderzoek) AndAlso Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                    HuidigeTermijn = ""
                End If
            End If
            If StapNummer >= 11 AndAlso StapNummer <= 14 Then
                HuidigeTermijn = AddDays(DatumbinnAntwSchorsing, 50)
                Step_Due = HuidigeTermijn
                'aangepast 05/01/2009
                If StapNummer = 13 AndAlso Not String.IsNullOrEmpty(ResultaatOnderzoek) And Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                    HuidigeTermijn = ""
                End If
            End If
            If StapNummer >= 15 AndAlso StapNummer <= 17 Then
                'geen deadline meer
                GeenDeadline(WFCurrentCase)
            End If
        Else
            If StapNummer = 1 OrElse StapNummer = 2 Then
                GeenDeadline(WFCurrentCase)
            End If
            If StapNummer >= 3 AndAlso StapNummer <= 10 Then
                If StukkenOpvragen = True Then
                    If Not String.IsNullOrEmpty(PostDatumStukken) Then
                        HuidigeTermijn = AddDays(PostDatumStukken, 32)
                    Else
                        'Stukken zijn nog niet binnen.
                        HuidigeTermijn = ""
                    End If
                Else
                    HuidigeTermijn = ""
                End If
                If StapNummer = 6 OrElse StapNummer = 9 OrElse StapNummer = 8 OrElse StapNummer = 10 Then
                    If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                        If ResultaatOnderzoek = "schorsing" Then
                            ''aanpassing 23/06/2009
                            HuidigeTermijn = AddDays(BriefVerzondenOp, 62)
                            '' eind aanpassing 23/06/2009
                        End If
                        If ResultaatOnderzoek = "onderzoekresultaat aan minister voor eindbeslissing" Then
                            HuidigeTermijn = AddDays(PostDatumStukken, 32)
                        End If
                        If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                            HuidigeTermijn = ""
                        End If
                    End If
                End If
            End If
            If StapNummer >= 10 AndAlso StapNummer <= 14 Then
                If Not String.IsNullOrEmpty(PoststempelBestSchorsing) Then
                    If (AntwoordNaSchorsing = "hervaststelling") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing met aanpassing") OrElse (AntwoordNaSchorsing = "rechtvaardigingsbeslissing zonder aanpassing") Then
                        ''aanpassing 23/06/2009 bijkomende aanpassing voor hervaststelling, rechtvaardigingsbeslissing met en zonder aanpassing
                        HuidigeTermijn = AddDays(PoststempelBestSchorsing, 32)
                        ''eind aanpassing 23/06/2009
                    Else
                        HuidigeTermijn = ""
                    End If
                End If
                If (ResultaatOnderzoek <> "schorsing") AndAlso (ResultaatOnderzoek <> "onderzoekresultaat aan minister voor eindbeslissing") Then
                    If Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                        HuidigeTermijn = ""
                    End If
                End If
                'aangepast 05/01/2009
                If StapNummer = 13 AndAlso Not String.IsNullOrEmpty(ResultaatOnderzoek) AndAlso Not String.IsNullOrEmpty(BriefVerzondenOp) Then
                    HuidigeTermijn = ""
                End If
            End If
            If StapNummer >= 15 AndAlso StapNummer <= 17 Then
                'geen deadline meer
                GeenDeadline(WFCurrentCase)
            End If
        End If
        AddToLog(WFCurrentCase, "ZetTermijnen: Exiting SetTermijn_Andere_besluiten2.")
    End Sub


    ''aanpassing 23062009 
    '**********************************************
    '*** Function  SetExtraTermijnMinister() ***
    '**********************************************
    Private Sub SetExtraTermijnMinister(ByVal WFCurrentcase As cCase)

        AddToLog(WFCurrentcase, "ZetTermijnen: Entering SetExtraTermijnMinister")
        If AardDossier = "klacht" Then
            If SoortBesluit <> "rekening" AndAlso TypeBestuur <> "Bestuur van de eredienst" And TypeBestuur <> "Intergemeentelijk SamenwerkingsVerband" Then
                If ((StapNummer >= 0) AndAlso (StapNummer <= 8)) Then
                    Dim ldTemp As String = ""
                    If Not String.IsNullOrEmpty(Voorwerp) AndAlso Not String.IsNullOrEmpty(TypeBestuur) Then
                        Select Case UCase(Voorwerp)
                            Case "HANDELING"

                            Case "LIJSTBESLUIT"
                                ' meldingslijsten
                                If StapNummer = 1 OrElse StapNummer = 2 Then
                                    If String.IsNullOrEmpty(OpvraagbriefVerstuurdOp) Then
                                        If Aangetekend = "aangetekende brief" Then
                                            HuidigeTermijn = AddDays(DatumBinnenkomstAfdeling, 49)
                                        Else
                                            HuidigeTermijn = SetInitieelVervalTermijnMeldingslijstenMinister()
                                        End If
                                    Else
                                        HuidigeTermijn = ""
                                    End If
                                End If
                                If StapNummer >= 3 AndAlso StapNummer <= 10 Then
                                    If Not String.IsNullOrEmpty(PostDatumStukken) Then
                                        HuidigeTermijn = AddDays(PostDatumStukken, 52)
                                    End If
                                End If
                            Case "INZENDINGSPLICHTIG BESLUIT"
                                If StapNummer = 1 OrElse StapNummer = 2 Then
                                    If String.IsNullOrEmpty(OpvraagbriefVerstuurdOp) Then
                                        If Aangetekend = "aangetekende brief" Then
                                            HuidigeTermijn = AddDays(DatumBinnenkomstAfdeling, 69)
                                        Else
                                            HuidigeTermijn = SetInitieelVervalTermijnInzendinsplichtigeMinister()
                                        End If
                                    Else
                                        'Datum verzenden opvraagbrief is ingevuld
                                        ''aanpassing 30/06/2009
                                        If TermijnStuiten = True Then
                                            HuidigeTermijn = ""
                                        Else
                                            If Aangetekend = "aangetekende brief" Then
                                                HuidigeTermijn = AddDays(PostDatumKlacht, 69)
                                            Else
                                                HuidigeTermijn = SetInitieelVervalTermijnInzendinsplichtigeMinister()
                                            End If
                                        End If
                                    End If
                                End If
                                If StapNummer >= 3 AndAlso StapNummer <= 10 Then
                                    If TermijnStuiten = True Then
                                        If Not String.IsNullOrEmpty(PostDatumStukken) Then
                                            HuidigeTermijn = AddDays(PostDatumStukken, 72)
                                        Else
                                            'Stukken zijn nog niet binnen.
                                            HuidigeTermijn = ""
                                        End If
                                    Else
                                        'Als er geen stukken worden opgevraagd, blijven de termijnen dezelfde als in vorige stappen.
                                        If Aangetekend = "aangetekende brief" Then
                                            ''aanpassing 23/06/2009
                                            HuidigeTermijn = AddDays(DatumBinnenkomstAfdeling, 69)
                                            ''eind aanpassing 23/06/2009
                                        Else
                                            HuidigeTermijn = SetInitieelVervalTermijnInzendinsplichtigeMinister()
                                        End If
                                    End If
                                End If
                            Case "ANDER BESLUIT"
                                ''andere besluiten
                                If StapNummer = 1 OrElse StapNummer = 2 Then
                                    GeenDeadline(WFCurrentcase)
                                End If
                                If StapNummer >= 3 AndAlso StapNummer <= 10 Then
                                    If StukkenOpvragen = True Then
                                        If Not String.IsNullOrEmpty(PostDatumStukken) Then
                                            HuidigeTermijn = AddDays(PostDatumStukken, 52)
                                        Else
                                            'Stukken zijn nog niet binnen.
                                            HuidigeTermijn = ""
                                        End If
                                    Else
                                        HuidigeTermijn = ""
                                    End If
                                End If
                            Case Else
                                'geen
                        End Select
                        If Not String.IsNullOrEmpty(WFCurrentcase.Step_DueDate) Then
                            ldTemp = AddDays(WFCurrentcase.Step_DueDate, 20)
                        End If
                        WFCurrentcase.SetProperty("Extra termijn minister", ldTemp)
                        WFCurrentcase.SetPropertyVisible("Extra termijn minister", True)
                    Else
                        WFCurrentcase.SetPropertyVisible("Extra termijn minister", False)

                    End If
                Else
                    WFCurrentcase.SetPropertyVisible("Extra termijn minister", False)
                End If
            Else
                WFCurrentcase.SetPropertyVisible("Extra termijn minister", False)
            End If
        End If
        AddToLog(WFCurrentcase, "ZetTermijnen: Exiting SetExtraTermijnMinister")
    End Sub

    ''extra functies 30062009
    ''meldingslijsten
    Private Function SetInitieelVervalTermijnMeldingslijstenMinister() As String
        Return SetInitieelVervalTermijn(52)
    End Function

    ''----------------------------------------
    ''andere besluiten
    Private Function SetInitieelVervalTermijnAndereMinister() As String
        Return SetInitieelVervalTermijn(MeldingsType.GetMeldingsType(TypeBestuur).Termijn)
    End Function

    ''---------------------------------
    ''inzendingsplichtige besluiten
    Private Function SetInitieelVervalTermijnInzendinsplichtigeMinister() As String
        Return SetInitieelVervalTermijn(72)
    End Function

    Private Function SetInitieelVervalTermijn(ByVal viAantalDagen As Integer) As String
        Dim initdate As String = Now.Year & "-" & Now.Month & "-" & Now.Day
        If Not String.IsNullOrEmpty(TypeBestuur) AndAlso Not String.IsNullOrEmpty(PostDatumStukken) Then
            If viAantalDagen <> 0 Then
                'initdate = CalcTermijnDate(PostDatumStukken, viAantalDagen)
                initdate = AddDays(PostDatumStukken, viAantalDagen)
            End If
        End If
        Return initdate
    End Function

    'Public Shared Function AddDays(ByVal vsString As String, ByVal viHowMany As Integer) As String
    '    If Not String.IsNullOrEmpty(vsString) Then
    '        Return DateAdd(DateInterval.Day, viHowMany, DateTime.Parse(vsString)).ToString("yyyy-MM-dd") & " 23:59:59"
    '    Else
    '        Return ""
    '    End If
    'End Function
    Public Shared Function AddDays(ByVal vsString As String, ByVal viHowMany As Integer) As String
        Arco.Utils.Logging.Log("ZetTermijnen: AddDays is adding " & viHowMany.ToString & " days.", "d:\arco\logging\Termijnberekeningen.log")
        If Not String.IsNullOrEmpty(vsString) Then
            Dim DateResult As String = DateAdd(DateInterval.Day, viHowMany, DateTime.Parse(vsString)).ToString("yyyy-MM-dd")
            Select Case Weekday(CDate(DateResult))
                Case 1
                    DateResult = DateAdd(DateInterval.Day, 1, DateTime.Parse(DateResult)).ToString("yyyy-MM-dd")
                Case 7
                    DateResult = DateAdd(DateInterval.Day, 2, DateTime.Parse(DateResult)).ToString("yyyy-MM-dd")
                Case Else
            End Select

            Dim lbFound As Boolean = True
            Dim colHolidays As Arco.Doma.Library.Routing.Holidays
            Try
                colHolidays = Arco.Doma.Library.Routing.Holidays.GetHolidays
                If Not colHolidays Is Nothing Then
                    While lbFound = True
                        lbFound = False
                        For Each DateItem As Arco.Doma.Library.Routing.Holidays.HolidayInfo In colHolidays
                            If DateItem.Recurrent Then
                                If Format(CDate(DateResult), "MM-dd") = Format(DateItem.Date, "MM-dd") Then
                                    DateResult = DateAdd(DateInterval.Day, 1, DateTime.Parse(DateResult)).ToString("yyyy-MM-dd")
                                    lbFound = True
                                End If
                            Else
                                If DateResult = Format(DateItem.Date, "yyyy-MM-dd") Then
                                    DateResult = DateAdd(DateInterval.Day, 1, DateTime.Parse(DateResult)).ToString("yyyy-MM-dd")
                                    lbFound = True
                                End If
                            End If
                        Next
                    End While
                End If
            Catch ex As Exception
                Return ""
            End Try
            Return DateResult & " 23:59:59"
        Else
            Return ""
        End If
    End Function

    Public Shared Function CalcTermijnDate(ByVal vsStartdate As String, ByVal vnDays As Integer) As String
        ' test op lege datum toegevoegd op 08/05/2014
        If Not IsDate(vsStartdate) Then
            Return ""
        End If
        'Return Holidays.CalculateWorkingDaysDeadline(DateTime.Parse(vsStartdate), vnDays).ToString("yyyy-MM-dd") & " 23:59:59"
        Return Holidays.CalculateWorkingDaysDeadline(DateTime.Parse(vsStartdate), vnDays).ToString("yyyy-MM-dd")
    End Function

    Private Sub GeenDeadline(ByVal WFCurrentCase As cCase)
        AddToLog(WFCurrentCase, "Geen deadline.")
        HuidigeTermijn = ""
        WFCurrentCase.Step_DueDate = ""
    End Sub

    Private Sub AddToLog(ByVal WFCurrentCase As cCase, ByVal vsText As String)
        Arco.Utils.Logging.Log(WFCurrentCase.Case_ID & ": " & WFCurrentCase.Case_Name & ": " & vsText, "d:\arco\logging\Termijnberekeningen.log")
    End Sub

    '************************
    'WFSetDeadline( ByVal Step_Due,  HuidigeTermijn )
    '************************
    Private Sub SetDeadline(ByVal WFCurrentCase As cCase, ByVal vsStepDue As String, ByVal vsHuidigeTermijn As String)
        AddToLog(WFCurrentCase, "SetDeadLine: vsStepDue      = " & vsStepDue)
        AddToLog(WFCurrentCase, "SetDeadLine: HuidigeTermijn = " & vsHuidigeTermijn)
        If String.IsNullOrEmpty(vsStepDue) Then
            GeenDeadline(WFCurrentCase)
        Else
            WFCurrentCase.Step_DueDate = vsStepDue
            WFCurrentCase.SetProperty("huidige termijn", vsHuidigeTermijn)
        End If

    End Sub

    Private Function IsControleTeLaat(ByVal WFCurrentCase As cCase) As Boolean
        If Not String.IsNullOrEmpty(PostDatumKlacht) AndAlso Not String.IsNullOrEmpty(InitTermijn) Then
            If DateTime.Parse(PostDatumKlacht) > DateTime.Parse(InitTermijn) Then
                Return True
            End If
        End If
        Return False
    End Function

#Region " isNieuwSysteem "
    Private Function isOldSysteemDate(ByVal vsDate As String) As Boolean
        If Not String.IsNullOrEmpty(vsDate) Then
            Dim dtParsed As DateTime
            If DateTime.TryParse(vsDate, dtParsed) Then
                If DateDiff("d", dtParsed, "2009/07/01") < 1 Then
                    'old system
                    Return True
                End If
            End If
        End If
        Return False
    End Function
    Private Function IsNieuwSysteem(ByVal WFCurrentCase As cCase) As Boolean

        If isOldSysteemDate(Me.LijstBesluitZitting) Then
            Return False
        End If
        If isOldSysteemDate(Me.DatumBesluit) Then
            Return False
        End If
        If isOldSysteemDate(Me.DatumRechtvaardigingBeslissing) Then
            Return False
        End If
        Return True

    End Function
#End Region

End Class
