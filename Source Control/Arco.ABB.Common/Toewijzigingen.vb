
<Serializable()> _
Public Class Toewijzigingen
    Public Shared Sub CascadeToewijzing(ByRef WFCurrentCase As Doma.Library.Routing.cCase)

        Dim lsLookupBehandelaar As String = WFCurrentCase.GetProperty(Of String)("lookup_dossierbehandelaar")
        Dim lsDienstTeamCel As String = WFCurrentCase.GetProperty(Of String)("Dienst/TEAM/Cel")
        Dim lsAfdeling As String = WFCurrentCase.GetProperty(Of String)("afdeling")

        WFCurrentCase.SetProperty("hiddentoewijzing", lsDienstTeamCel)
        WFCurrentCase.SetProperty("hiddentoewijzing2", WFCurrentCase.GetProperty("lookup_dossierbehandelaar"))

        If Not String.IsNullOrEmpty(lsAfdeling) Then
            Logging.AddToLog(WFCurrentCase, "Cascadetoewijzing, afdeling = " & lsAfdeling)
            If Not String.IsNullOrEmpty(lsLookupBehandelaar) Then
                Logging.AddToLog(WFCurrentCase, "Cascadetoewijzing, LookupBehandelaar = " & lsLookupBehandelaar)
                WFCurrentCase.SetProperty("dossierbehandelaar", lsLookupBehandelaar)
            ElseIf Not String.IsNullOrEmpty(lsDienstTeamCel) Then
                ' rol met alleen goedkeurders dienst/TEAM/CEL
                Logging.AddToLog(WFCurrentCase, "Cascadetoewijzing, DienstTeamCel = " & lsDienstTeamCel)
                Dim lsgebruikers As String = Rollen.GetUsersFromRole(lsDienstTeamCel, "V").ToString
                If Not String.IsNullOrEmpty(lsgebruikers) Then
                    Logging.AddToLog(WFCurrentCase, "Cascadetoewijzing, DienstTeamCel, Found users :  " & lsgebruikers)
                    WFCurrentCase.SetProperty("dossierbehandelaar", lsgebruikers)
                Else
                    Logging.AddToLog(WFCurrentCase, "Cascadetoewijzing, setting DienstTeamCel")
                    WFCurrentCase.SetProperty("dossierbehandelaar", lsDienstTeamCel)
                End If
            Else
                Logging.AddToLog(WFCurrentCase, "Cascadetoewijzing, setting afdeling")
                WFCurrentCase.SetProperty("dossierbehandelaar", lsAfdeling)
            End If
        End If

        'todo : clear lookup ,agfeling, diesnt prop??
    End Sub
End Class

