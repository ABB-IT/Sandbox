Imports Arco.Doma.Library.Routing
Public Class StapNummers
    Public Shared Function GetStapNummer(ByVal voCase As cCase) As Integer
        Dim lsStapName As String = voCase.CurrentStep.Step_Name        
        Select Case lsStapName
            Case "Registratie dossier", "Keuze dossierbehandelaar"
                Return 1         
            Case "Opvraging"
                Return 2
            Case "Wachten op ontvangst antwoord"
                Return 3
            Case "Onderzoek en voorstel - afdeling 1", "Keuze dossierbehandelaar - afdeling 2", "Onderzoek en voorstel - Afdeling 2"
                Return 4
            Case "Goedkeuring - afdeling 1", "Goedkeuring - afdeling 2"
                Return 5                 
            Case "Resultaat onderzoek - afdeling 1", "Resultaat onderzoek - afdeling 2"
                Return 6         
            Case "Wachten op goedkeuring Gouverneur/Minister", "Wachten op goedkeuring Minister - Afdeling2"
                Return 7           
            Case "Eindbeslissing Minister: keuze afdeling/dossierbehandelaar"
                Return 8
            Case "Na schorsing: keuze medeopvolger"
                Return 9
            Case "Na schorsing: wachten op reactie", "Na rechtvaardiging: keuze medeopvolger", "Na rechtvaardiging: onderzoek en voorstel - afdeling 2", "Na rechtvaardiging: goedkeuring - afdeling 2"
                Return 10
            Case "Na rechtvaardiging: onderzoek en voorstel"
                Return 11
            Case "Na rechtvaardiging: goedkeuring - Afdeling 1"
                Return 12
            Case "Na rechtvaardiging: resultaat onderzoek"
                Return 13
            Case "Na rechtvaardiging: wachten op goedkeuring Minister"
                Return 14
            Case "Kennisgeving"
                Return 15
            Case "Na Kennisgeving: goedkeuring", "Na schorsing: onderzoek en voorstel", "Na schorsing: goedkeuring"
                Return 16
            Case "Afsluiten dossier"
                Return 17
            Case Else
                Return 0
        End Select
    End Function
End Class
