Imports Arco.Doma.Library
Imports Arco.Doma.Library.Routing
Imports Arco.ABB.Common

Public Class AlgemeenScript_Onkeep
    Inherits IZBEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)

        Dim lsbestuur As String = WFCurrentCase.GetProperty(Of String)("bestuur")

        If lsbestuur <> "" Then         
            Dim loBestuur As Bestuur = Bestuur.GetBestuur(lsbestuur)
            WFCurrentCase.SetProperty("bestuur_naam", loBestuur.Naam)
            WFCurrentCase.SetProperty("bestuur_straatnr", loBestuur.StraatNr)
            WFCurrentCase.SetProperty("bestuur_postnummer", loBestuur.PostCode)
            WFCurrentCase.SetProperty("bestuur_gemeente", loBestuur.Gemeente)
            WFCurrentCase.SetProperty("bestuur_id", lsbestuur)
            ' terug leegmaken anders terug opgehaald
            WFCurrentCase.SetProperty("bestuur", "")
        End If

        Dim lsHidden As String = WFCurrentCase.GetProperty(Of String)("hiddenAdd")
        Dim lsSoortbesluit As String = WFCurrentCase.GetProperty(Of String)("type/soort besluit")
        Dim litableID As Integer = WFCurrentCase.GetPropertyInfo("OverzichtBesluiten").PROP_ID

        ''voeg lijn toe
        If lsHidden = "Add" Then          
            Dim ldDatumBesluit As String = WFCurrentCase.GetProperty(Of String)("datum besluit")
            Dim ldPostDatum As String = WFCurrentCase.GetProperty(Of String)("postdatum")
            Dim ldDatumbinnenkomst As String = WFCurrentCase.GetProperty(Of String)("datum binnengekomen op afdeling")

            Dim lsKorteomschrijving As String = WFCurrentCase.GetProperty(Of String)("Korte omschrijving besluit")
            Dim lsAfdeling As String = WFCurrentCase.GetProperty(Of String)("afdeling")
            Dim lsDienstTeamCel As String = WFCurrentCase.GetProperty(Of String)("Dienst/Team/Cel")
            Dim lsDossierbehandelaar As String
            Dim lstemp As String = WFCurrentCase.GetProperty(Of String)("lookup_dossierbehandelaar")
            If lstemp = "" Then
                lstemp = lsDienstTeamCel
                If lstemp = "" Then
                    lstemp = lsAfdeling
                    lsDossierbehandelaar = lstemp
                Else
                    lsDossierbehandelaar = "(Role) " & lstemp
                End If
            Else
                lsDossierbehandelaar = lstemp
            End If

            Dim lsOpmerkingen As String = WFCurrentCase.GetProperty(Of String)("Opmerkingen")
            Dim lsHoeveelste As String = WFCurrentCase.GetProperty(Of String)("hoeveelste")
            Dim lsboekjaar As String = WFCurrentCase.GetProperty(Of String)("boekjaar")

            'voeglijntoe

            Dim llRow As Integer = 0
            WFCurrentCase.CreateRowInTable(litableID, llRow, 0)
            WFCurrentCase.SetProperty("type/soort besluit", lsSoortbesluit, llRow, litableID)
            WFCurrentCase.SetProperty("datum besluit", ldDatumBesluit, llRow, litableID)
            WFCurrentCase.SetProperty("postdatum", ldPostDatum, llRow, litableID)
            WFCurrentCase.SetProperty("datum binnengekomen op afdeling", ldDatumbinnenkomst, llRow, litableID)
            WFCurrentCase.SetProperty("Korte omschrijving besluit", lsKorteomschrijving, llRow, litableID)
            WFCurrentCase.SetProperty("Opmerkingen", lsOpmerkingen, llRow, litableID)
            WFCurrentCase.SetProperty("lookup_dossierbehandelaar", lsDossierbehandelaar, llRow, litableID)
            WFCurrentCase.SetProperty("Dienst/Team/Cel", lsDienstTeamCel, llRow, litableID)
            WFCurrentCase.SetProperty("afdeling", lsAfdeling, llRow, litableID)
            WFCurrentCase.SetProperty("boekjaar", lsboekjaar, llRow, litableID)
            WFCurrentCase.SetProperty("hoeveelste", lsHoeveelste, llRow, litableID)
            'end voeglijntoe

            WFCurrentCase.SetProperty("hiddenAdd", "")
            WFCurrentCase.SetProperty("Korte omschrijving besluit", "")
            WFCurrentCase.SetProperty("Opmerkingen", "")

            ' **** boekjaar en hoeveelste moet leeggemaakt worden nadat de lijn is toegevoegd
            WFCurrentCase.SetProperty("hoeveelste", "")
            ' einde aanpassing
        End If

        Logging.AddToLog(WFCurrentCase, "boekjaar = " & WFCurrentCase.GetProperty(Of String)("boekjaar"))

        If lsSoortbesluit = "budget" OrElse lsSoortbesluit = "budgetwijziging" Then
            WFCurrentCase.SetPropertyVisible("boekjaar", True)
        Else
            WFCurrentCase.SetProperty("boekjaar", "")
            WFCurrentCase.SetPropertyVisible("boekjaar", False)
            Logging.AddToLog(WFCurrentCase, "boekjaar = " & WFCurrentCase.GetProperty(Of String)("boekjaar"))
        End If
        If lsSoortbesluit = "budgetwijziging" Then
            WFCurrentCase.SetPropertyVisible("hoeveelste", True)
        Else
            WFCurrentCase.SetPropertyVisible("hoeveelste", False)
        End If

        'berekenen init termijn
        Dim lsType As String = WFCurrentCase.GetProperty(Of String)("type bestuur")

        Dim loTable As baseObjects.DM_OBJECT.Table = WFCurrentCase.GetProperty(Of baseObjects.DM_OBJECT.Table)("OverzichtBesluiten")
        For Each loRow As baseObjects.DM_OBJECT.Table.TableRow In loTable.Rows
            If loRow.Row_ID > 0 Then               
                Dim lspostdatum As String = WFCurrentCase.GetProperty(Of String)("postdatum", loRow.Row_ID, loTable.Prop_ID)
                Dim lsontvangstdatum As String = WFCurrentCase.GetProperty(Of String)("datum binnengekomen op afdeling", loRow.Row_ID, loTable.Prop_ID)
                Dim lssoort As String = WFCurrentCase.GetProperty(Of String)("type/soort besluit", loRow.Row_ID, loTable.Prop_ID)
                'ls_TSbOpm=WFGetProperty("T_Opmerking_SB",True, loRow.ROW_ID,loTable.PROP_ID)
                Dim lsDatumbesluit As String = WFCurrentCase.GetProperty(Of String)("datum besluit", loRow.Row_ID, loTable.Prop_ID)
                Call Logging.AddToLog(WFCurrentCase, "lstype :" & lsType & " ,lspostdatum:" & lspostdatum & " ,lsontvangstdatum:" & lsontvangstdatum & "  lssoort :" & lssoort & " lsDatumbesluit:" & lsDatumbesluit)
                If lsType <> "" AndAlso lspostdatum <> "" AndAlso lsontvangstdatum <> "" AndAlso lssoort <> "" AndAlso lsDatumbesluit <> "" Then
                    Dim ldInitTermijn As String = SetInitieelVervalTermijn(lsType, lspostdatum, lsontvangstdatum, lssoort, lsDatumbesluit)
                    WFCurrentCase.SetProperty("initiële vervaltermijn", ldInitTermijn, loRow.Row_ID, loTable.Prop_ID)
                    Logging.AddToLog(WFCurrentCase, "initiële termijn = " & ldInitTermijn)
                Else
                    WFCurrentCase.SetProperty("initiële vervaltermijn", "", loRow.Row_ID, loTable.Prop_ID)
                End If
            End If
        Next
    End Sub

    Private Function SetInitieelVervalTermijn(ByVal lstype As String, ByVal lspostdatum As String, ByVal lsontvangstdatum As String, ByVal lssoortBesluit As String, ByVal lsDatumbesluit As String) As String
        'initieel termijn lijstbesluit
        Dim initdate As String = Now.Year & "-" & Now.Month & "-" & Now.Day

        If Not IsDate(lsDatumbesluit) Then
            Return ""
        End If
        If Not IsDate(lspostdatum) Then
            Return ""
        End If

        ' aantal dagen?
        Dim ldaantaldagen As Integer = 0
        If lstype <> "" AndAlso lspostdatum <> "" AndAlso lsDatumbesluit <> "" Then
            If DateDiff("n", lsDatumbesluit, "7/1/2009") < 1 Then

                'later dan 1 juli
                ' hier komt de logica
                '--------------------
                ''30062009 aanpassing
                If lstype = "Bestuur van de eredienst" Then
                    If lssoortBesluit = "rekening" Then
                        ' hier gewoon 200 dagen bijtellen. Geen rekening houden met weekends.
                        ldaantaldagen = 200
                        initdate = CStr(DateAdd("d", ldaantaldagen, CDate(lsontvangstdatum)))
                    Else
                        ldaantaldagen = 200
                        initdate = ABB.AlgemeenToezicht.TermijnBerekening.AddDays(lsontvangstdatum, ldaantaldagen)
                    End If

                Else
                    If lstype = "Intergemeentelijk SamenwerkingsVerband" Then
                        ldaantaldagen = 300
                        initdate = ABB.AlgemeenToezicht.TermijnBerekening.AddDays(lsontvangstdatum, ldaantaldagen)
                    Else
                        'rest
                        If lssoortBesluit = "rekening" Or lssoortBesluit = "eindrekening" Then
                            If DateDiff("n", lsDatumbesluit, "1/1/2013") < 1 Then
                                ldaantaldagen = 152
                                'initdate = CStr(DateAdd("d", ldaantaldagen, CDate(lsontvangstdatum)))
                                'If Weekday(CDate(initdate)) = 1 Or Weekday(CDate(initdate)) = 7 Then
                                'initdate = ABB.AlgemeenToezicht.TermijnBerekening.AddDays(lsDatumbesluit, ldaantaldagen)
                                initdate = ABB.AlgemeenToezicht.TermijnBerekening.AddDays(lsontvangstdatum, ldaantaldagen)
                                'End If
                            Else
                                ldaantaldagen = 300
                                'initdate = ABB.AlgemeenToezicht.TermijnBerekening.CalcTermijnDate(lsontvangstdatum, ldaantaldagen)
                                'initdate = DateAdd("d",ldaantaldagen,CDate(lsontvangstdatum))
                                ' GEEN rekening houden met alle weekends. Kalenderdagen bijtellen en het resultaat op eerstvolgende werkdag zetten
                                'initdate = CStr(DateAdd("d", ldaantaldagen, CDate(lsontvangstdatum)))
                                'If Weekday(CDate(initdate)) = 1 Or Weekday(CDate(initdate)) = 7 Then
                                initdate = ABB.AlgemeenToezicht.TermijnBerekening.AddDays(lsontvangstdatum, ldaantaldagen)
                                'End If
                            End If
                        Else
                            '' '' aanpassen op vraag van Veronique
                            If lssoortBesluit = "N-goedk meerjarenplan(wijziging) eredienstbestuur" Then
                                ldaantaldagen = 30
                                initdate = ABB.AlgemeenToezicht.TermijnBerekening.AddDays(lsontvangstdatum, ldaantaldagen)
                            Else
                                ldaantaldagen = 52
                                'initdate = ABB.AlgemeenToezicht.TermijnBerekening.CalcTermijnDate(lspostdatum, ldaantaldagen)
                                ' GEEN rekening houden met alle weekends. Kalenderdagen bijtellen en het resultaat op eerstvolgende werkdag zetten
                                'initdate = CStr(DateAdd("d", ldaantaldagen, CDate(lsontvangstdatum)))
                                'If Weekday(CDate(initdate)) = 1 Or Weekday(CDate(initdate)) = 7 Then
                                initdate = ABB.AlgemeenToezicht.TermijnBerekening.AddDays(lsontvangstdatum, ldaantaldagen)
                                'End If
                            End If
                            ' einde aanpassing
                        End If
                    End If
                End If
            Else
                ' vroeger dan 1 juli
                ' hier komt de logica
                '--------------------
                If lstype = "OCMW" Or lstype = "OCMW Vereniging" Then
                    If lssoortBesluit = "rekening" Then
                        ldaantaldagen = 300
                        initdate = ABB.AlgemeenToezicht.TermijnBerekening.AddDays(lsontvangstdatum, ldaantaldagen)
                    Else
                        ldaantaldagen = 50
                        initdate = ABB.AlgemeenToezicht.TermijnBerekening.AddDays(lsontvangstdatum, ldaantaldagen)
                    End If
                Else
                    If lstype = "Bestuur van de eredienst" Then
                        If lssoortBesluit = "rekening" Then
                            ' hier gewoon 200 dagen bijtellen. Geen rekening houden met weekends.
                            ldaantaldagen = 200
                            initdate = CStr(DateAdd("d", ldaantaldagen, CDate(lsontvangstdatum)))
                        Else
                            ldaantaldagen = 200
                            initdate = ABB.AlgemeenToezicht.TermijnBerekening.AddDays(lsontvangstdatum, ldaantaldagen)
                        End If
                    Else
                        If lstype = "Intergemeentelijk SamenwerkingsVerband" Then
                            ldaantaldagen = 300
                            initdate = ABB.AlgemeenToezicht.TermijnBerekening.AddDays(lsontvangstdatum, ldaantaldagen)
                        Else
                            'rest
                            If lssoortBesluit = "rekening" Or lssoortBesluit = "eindrekening" Then
                                ldaantaldagen = 300
                                initdate = ABB.AlgemeenToezicht.TermijnBerekening.AddDays(lsontvangstdatum, ldaantaldagen)
                            Else
                                ldaantaldagen = 52
                                'initdate = ABB.AlgemeenToezicht.TermijnBerekening.CalcTermijnDate(lspostdatum, ldaantaldagen)
                                ' GEEN rekening houden met alle weekends. Kalenderdagen bijtellen en het resultaat op eerstvolgende werkdag zetten
                                'initdate = CStr(DateAdd("d", ldaantaldagen, CDate(lsontvangstdatum)))
                                'If Weekday(CDate(initdate)) = 1 Or Weekday(CDate(initdate)) = 7 Then
                                initdate = ABB.AlgemeenToezicht.TermijnBerekening.AddDays(lsontvangstdatum, ldaantaldagen)
                                'End If
                                'end if
                            End If
                        End If
                    End If
                End If
                'einde logica
                '-----------------
            End If
        End If
        'op vraag van VV 15/12/2010
        If (lssoortBesluit = "budget" And lstype = "Bestuur van de eredienst") Then
            initdate = ""
        Else
        End If
        'einde aanpassing

        Return initdate
    End Function


    Public Overrides ReadOnly Property Name As String
        Get
            Return "AlgemeenScript_Onkeep"
        End Get
    End Property
End Class
