Imports Arco.Doma.Library
Imports Arco.Doma.Library.Routing
Public Class OverzichtBesluiten
    Public Shared Function CheckData(ByVal WFCurrentCase As cCase) As String
        Dim lsError As String = ""
        Dim i As Integer = 1

        Dim loTable As baseObjects.DM_OBJECT.Table = WFCurrentCase.GetProperty(Of baseObjects.DM_OBJECT.Table)("OverzichtBesluiten")
        If loTable.Rows.Count <= 1 Then
            lsError &= "Gelieve tenminste 1 rij in te geven.<br>"
        End If

        Dim lsBestuurID As String = WFCurrentCase.GetProperty(Of String)("bestuur_id")
        If String.IsNullOrEmpty(lsBestuurID) OrElse lsBestuurID = "0" Then
            lsError = lsError & "Gelieve een bestuur te kiezen.<br>"
        End If

        'verify table
        For Each loRow As baseObjects.DM_OBJECT.Table.TableRow In loTable.Rows
            If loRow.Row_ID > 0 Then
                If WFCurrentCase.GetPropertyInfo("type/soort besluit", loTable.Prop_ID, loRow.Row_ID, True).isEmpty Then
                    lsError &= "Rij " & i & ": gelieve een type/soort besluit in te vullen.<br>"
                    Exit For
                End If
                If WFCurrentCase.GetPropertyInfo("postdatum", loTable.Prop_ID, loRow.Row_ID, True).isEmpty Then
                    lsError &= "Rij " & i & ": gelieve een postdatum in te vullen.<br>"
                    Exit For
                End If
                If WFCurrentCase.GetPropertyInfo("datum binnengekomen op afdeling", loTable.Prop_ID, loRow.Row_ID, True).isEmpty Then
                    lsError &= "Rij " & i & ": gelieve een ontvangstdatum in te vullen.<br>"
                    Exit For
                End If
                If WFCurrentCase.GetPropertyInfo("datum besluit", loTable.Prop_ID, loRow.Row_ID, True).isEmpty Then
                    lsError &= "Rij " & i & ": gelieve een datumbesluit in te vullen.<br>"
                    Exit For
                End If


                Select Case WFCurrentCase.GetProperty(Of String)("type/soort besluit", loRow.Row_ID, loTable.Prop_ID)
                    Case "budget", "budgetwijziging"
                        'controle boekjaar

                        If Not WFCurrentCase.GetPropertyInfo("boekjaar", loTable.Prop_ID, loRow.Row_ID, True).isEmpty Then
                            Dim lsBoekjaar As Int32 = CInt(WFCurrentCase.GetProperty(Of String)("boekjaar", loRow.Row_ID, loTable.Prop_ID))
                            'Dim lsBoekJaar As Int32 = WFCurrentCase.GetProperty(Of Int32)("boekjaar")
                            If lsBoekjaar < 1900 OrElse lsBoekjaar > 2100 Then
                                lsError &= "Rij " & i & ": gelieve een correct boekjaar in te vullen.<br>"
                                Exit For
                            End If
                        End If
                    Case Else
                End Select              

                i = i + 1
            End If
        Next
        Return lsError
    End Function
    Public Shared Function CreateDossiers(ByVal WFCurrentCase As cCase) As String

        Dim lsError As String = CheckData(WFCurrentCase)

        If String.IsNullOrEmpty(lsError) Then
            Dim liProc_id As Integer = Routing.Procedure.GetProcedure("Algemeen toezicht").PROC_ID


            Dim lstype As String = WFCurrentCase.GetProperty(Of String)("type bestuur")
            Dim lsbestuur As String = WFCurrentCase.GetProperty(Of String)("bestuur_naam")
            Dim liID_bestuur As String = WFCurrentCase.GetProperty(Of String)("bestuur_id")
            Dim lsgemeente As String = WFCurrentCase.GetProperty(Of String)("bestuur_gemeente")

            Dim loTable As baseObjects.DM_OBJECT.Table = WFCurrentCase.GetProperty(Of baseObjects.DM_OBJECT.Table)("OverzichtBesluiten")
            For Each loRow As baseObjects.DM_OBJECT.Table.TableRow In loTable.Rows
                If loRow.Row_ID > 0 Then
                    Dim lssoort As String = WFCurrentCase.GetProperty(Of String)("type/soort besluit", loRow.Row_ID, loTable.Prop_ID)
                    Dim lspostdatum As String = WFCurrentCase.GetProperty(Of String)("postdatum", loRow.Row_ID, loTable.Prop_ID)
                    Dim lsontvangstdatum As String = WFCurrentCase.GetProperty(Of String)("datum binnengekomen op afdeling", loRow.Row_ID, loTable.Prop_ID)
                    Dim lsDatumbesluit As String = WFCurrentCase.GetProperty(Of String)("datum besluit", loRow.Row_ID, loTable.Prop_ID)
                    Dim lsopmerkingsveld As String = WFCurrentCase.GetProperty(Of String)("Opmerkingen", loRow.Row_ID, loTable.Prop_ID)
                    Dim ldInitTermijn As String = WFCurrentCase.GetProperty(Of String)("initiële vervaltermijn", loRow.Row_ID, loTable.Prop_ID)
                    Dim lskorteomschrijving As String = WFCurrentCase.GetProperty(Of String)("Korte omschrijving besluit", loRow.Row_ID, loTable.Prop_ID)
                    Dim lsdossierbehandelaar As String = WFCurrentCase.GetProperty(Of String)("lookup_dossierbehandelaar", loRow.Row_ID, loTable.Prop_ID)
                    Dim lsafdeling As String = WFCurrentCase.GetProperty(Of String)("afdeling", loRow.Row_ID, loTable.Prop_ID)
                    Dim lsdienstteamcel As String = WFCurrentCase.GetProperty(Of String)("Dienst/Team/Cel", loRow.Row_ID, loTable.Prop_ID)
                    Dim lsboekjaar As String = WFCurrentCase.GetProperty(Of String)("boekjaar", loRow.Row_ID, loTable.Prop_ID)
                    Dim lshoeveelste As String = WFCurrentCase.GetProperty(Of String)("hoeveelste", loRow.Row_ID, loTable.Prop_ID)
                    'Debug.PrintToFile(" 3b")
                    Dim lbVerdeling As Boolean = False
                    If InStr(lsdossierbehandelaar, "Role") > 0 Then
                        lbVerdeling = True
                    End If

                    Dim loNewBesluit As ABB.Common.InzendingsPlichtigBesluit = CreateInzendingsPlichtigBesluit(lstype, lspostdatum, lsontvangstdatum, lsDatumbesluit, lsbestuur, lsgemeente, ldInitTermijn, lssoort, lsopmerkingsveld, liID_bestuur, lskorteomschrijving, lsboekjaar, lshoeveelste)
                    If loNewBesluit.FTCid <> 0 Then
                        If GetAutoOpstart(lssoort, lstype) = 1 Then
                            'If GetAutoOpstart(Datarow(7)) = 1 Then mail van Heidi van 01/02/2010 16:35
                            'Debug.PrintToFile(Now() & " Dossier opstarten voor ID:" & ft_Cid)
                            Dim liCaseID As Integer = CreateDossier(WFCurrentCase.CaseData.Created_By, liProc_id, liID_bestuur, lssoort, lstype, loNewBesluit.FTCid, lspostdatum, lsontvangstdatum, lsDatumbesluit, ldInitTermijn, lsbestuur, lskorteomschrijving, lsdossierbehandelaar, lsdienstteamcel, lsafdeling, lbVerdeling, lsboekjaar, lshoeveelste)
                            ' Debug.PrintToFile(Now() & " CASE ID:" & liCaseID & " voor ID " & ft_Cid)
                            If liCaseID <> 0 Then
                                loNewBesluit.Case_ID = liCaseID
                                loNewBesluit = loNewBesluit.Save
                            End If
                        End If
                    End If

                    'bewaren data
                    'Datarow.add(lstype) 0
                    'Datarow.add(lspostdatum) 1
                    'Datarow.add(lsontvangstdatum) 2
                    'Datarow.add(lsDatumbesluit) 3
                    'Datarow.add(lsbestuur) 4
                    'Datarow.add(lsgemeente) 5
                    'Datarow.add(ldInitTermijn) 6
                    'Datarow.add(lssoort) 7
                    'Datarow.add(lsopmerkingsveld) 8
                    'Datarow.add(liID_bestuur) 9
                    'Datarow.add(lskorteomschrijving) 10
                    'Datarow.add(lsdossierbehandelaar) 11
                    'Datarow.add(lsdienstteamcel) 12
                    ' Datarow.add(lsafdeling) 13
                    'Datarow.add(lbVerdeling) 14
                    'Datarow.add(lsboekjaar) 15
                    'Datarow.add(lshoeveelste) 16
                    'Debug.PrintToFile(" 3d")
                End If
            Next

        End If

        Return lsError
    End Function

    ''Functions
    ''--------------
    Private Shared Function CreateInzendingsPlichtigBesluit(ByVal lstype As String, ByVal lspostdatum As String, ByVal lsontvangstdatum As String, ByVal lsDatumbesluit As String, ByVal lsbestuur As String, ByVal lsgemeente As String, ByVal lsinitTermijn As String, ByVal lssoort_besluit As String, ByVal lsopmerkingsveld As String, ByVal liID_bestuur As String, ByVal lskorteomschrijving As String, ByVal lsboekjaar As String, ByVal lshoeveelste As String) As ABB.Common.InzendingsPlichtigBesluit

        Dim loNewBesluit As ABB.Common.InzendingsPlichtigBesluit = ABB.Common.InzendingsPlichtigBesluit.NewInzendingsPlichtigBesluit
        loNewBesluit.Type = lstype
        loNewBesluit.Bestuur = lsbestuur
        loNewBesluit.Gemeente = lsgemeente
        loNewBesluit.DatumBesluit = lsDatumbesluit
        loNewBesluit.PostDatum = lspostdatum
        loNewBesluit.DatumIn = lsontvangstdatum
        loNewBesluit.InitieleTermijn = lsinitTermijn
        loNewBesluit.SoortBesluit = lssoort_besluit
        loNewBesluit.Opmerking = lsopmerkingsveld
        loNewBesluit.IDBestuur = liID_bestuur
        loNewBesluit.KorteOmschrijving = lskorteomschrijving
        loNewBesluit.BoekJaar = lsboekjaar
        loNewBesluit.Hoeveelste = lshoeveelste
        loNewBesluit = loNewBesluit.Save

        Return loNewBesluit
    End Function


    Private Shared Function GetAutoOpstart(ByVal lsSoort As String, ByVal lsTypeBestuur As String) As Integer
        ' mail van Heidi van 01/02/2011 16:34
        If (lsTypeBestuur = "Bestuur van de eredienst" And lsSoort = "meerjarenplan") Or (lsTypeBestuur = "Autonoom GemeenteBedrijf" And lsSoort = "rekening") Then
            Return 0
        Else
            Return ABB.Common.SoortInzendingsplichtigBesluit.GetSoortInzendingsplichtigBesluit(lsSoort).AutoOpstart
        End If
    End Function

    Private Shared Function CreateDossier(ByVal lsUserStarter As String, ByVal liProc_ID As Integer, ByVal liBestuurid As String, ByVal lsSoortbesluit As String, ByVal lsTypebestuur As String, ByVal liInzend_BESL_nr As Int32, ByVal lspostdatum As String, ByVal lsontvangstdatum As String, ByVal lsDatumbesluit As String, ByVal ldInitTermijn As String, ByVal lsbestuur As String, ByVal lskortomschrijving As String, ByVal lsDossierbehandelaar As String, ByVal lsdienstteamcel As String, ByVal lsafdeling As String, ByVal lbVerdeling As Boolean, ByVal lsboekjaar As String, ByVal lshoeveelste As String) As Integer
        Dim liCase_id As Integer = 0
        Dim loCase As cCase = cCase.NewCase()
        loCase.Proc_ID = liProc_ID
        loCase.CaseData.Created_By = lsUserStarter
        loCase = loCase.Create()

        If loCase.Tech_ID > 0 Then
            liCase_id = loCase.Case_ID
            loCase.SetProperty("inzend_BESL_nr", liInzend_BESL_nr)
            loCase.SetProperty("lijstbesluit_postdatum", lspostdatum)
            loCase.SetProperty("lijst_ontvangstdatum", lsontvangstdatum)
            loCase.SetProperty("datum besluit", lsDatumbesluit)
            loCase.SetProperty("initiële vervaltermijn", ldInitTermijn)
            loCase.SetProperty("type/soort besluit", lsSoortbesluit)
            loCase.SetProperty("type bestuur", lsTypebestuur)
            loCase.SetProperty("type bestuur2", lsTypebestuur)
            'If lsTypebestuur <> WFGetProperty("type bestuur") Then '?????
            '    loCase.SetProperty("type bestuur", lsTypebestuur)
            'End If
            loCase.SetProperty("bestuur", liBestuurid)
            loCase.SetProperty("bestuur_naam", lsbestuur)
            loCase.SetProperty("voorwerp", "inzendingsplichtig besluit")
            loCase.SetProperty("aard dossier", "nazicht inzendingsplichtig besluit")
            If Not String.IsNullOrEmpty(lskortomschrijving) Then
                loCase.SetProperty("lijst_kortomschrijving", lskortomschrijving)
            End If

            If lbVerdeling = True Then
                Dim lsgebruikers As String = ABB.Common.Rollen.GetUsersFromRole(lsdienstteamcel, "V")
                If Not String.IsNullOrEmpty(lsgebruikers) Then
                    loCase.SetProperty("dossierbehandelaar", lsgebruikers)
                Else
                    loCase.SetProperty("dossierbehandelaar", lsDossierbehandelaar)
                End If
            Else
                loCase.SetProperty("dossierbehandelaar", lsDossierbehandelaar)
            End If

            loCase.SetProperty("afdeling", lsafdeling)
            loCase.SetProperty("lookup_dossierbehandelaar", lsDossierbehandelaar)
            loCase.SetProperty("S_dossierbehandelaar?", True)
            loCase.SetProperty("Dienst/TEAM/Cel", lsdienstteamcel)

            Dim loDossierNummerAanpassen As ABB.Common.DossierNummerAanpassen = New ABB.Common.DossierNummerAanpassen
            loDossierNummerAanpassen.ExecuteCode(loCase)

            If IsNumeric(lsboekjaar) Then
                loCase.SetProperty("boekjaar", lsboekjaar)
            End If
            loCase.SetProperty("hoeveelste", lshoeveelste)

            'update stepid
            'set dossiernaam for algemeen toezicht
            Dim loSetDossierNaam As Arco.ABB.AlgemeenToezicht.SetDossierNaam = New Arco.ABB.AlgemeenToezicht.SetDossierNaam
            loSetDossierNaam.ExecuteCode(loCase)

            'todo verify logic
            Dim listepid As Integer = ABB.Common.Stappen.GetStapID("Keuze dossierbehandelaar", liProc_ID)
            loCase = loCase.Dispatch(listepid)

            If lbVerdeling = False Then
                loCase = loCase.Dispatch()
            End If
            liCase_id = loCase.Case_ID
        End If
        Return liCase_id
    End Function
End Class
