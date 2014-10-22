Imports Arco.Doma.Library
Imports Arco.Doma.Library.Routing

Public Class AanmakenDossiers
    Inherits IZBEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)

        'IZB - AANMAKEN DOSSIERS
        '=> rijen toevoegen aan databank + creatie dossier + gegevens toevoegen aan dossier
        'controle rijen
        'rijen toevoegen
        'dossiers aanmaken

        Dim lsError As String = OverzichtBesluiten.CreateDossiers(WFCurrentCase)
        'Debug.PrintToFile(" 2")     
        If lsError <> "" Then
            Dim lsStepid As Integer = ABB.Common.Stappen.GetStapID("Registratie dossier", WFCurrentCase.Proc_ID)
            WFCurrentCase.Dispatch(lsStepid)
        End If
    End Sub


    Public Overrides ReadOnly Property Name As String
        Get
            Return "AanmakenDossiers"
        End Get
    End Property
End Class
