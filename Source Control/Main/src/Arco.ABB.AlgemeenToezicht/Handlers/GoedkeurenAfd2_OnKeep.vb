Public Class GoedkeurenAfd2_OnKeep
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        Dim liStapNr As Integer = StapNummers.GetStapNummer(WFCurrentCase)

        WFCurrentCase.SetProperty("hiddentoewijzing3", WFCurrentCase.GetProperty("goedkeuring_Dienst/TEAM/Cel"))
        WFCurrentCase.SetProperty("hiddentoewijzing4", WFCurrentCase.GetProperty("keuze van de goedkeurder"))
        If Not WFCurrentCase.GetPropertyInfo("keuze van de goedkeurder").isEmpty Then
            WFCurrentCase.SetProperty("goedkeurder", WFCurrentCase.GetProperty("keuze van de goedkeurder"))
        End If

        'WFCurrentCase.SetPropertyVisible("Keuze goedkeurder", True)
        WFCurrentCase.SetPropertyVisible("Goedkeuring_afdeling", True)
        WFCurrentCase.SetPropertyVisible("goedkeuring_Dienst/TEAM/Cel", True)
        WFCurrentCase.SetPropertyVisible("goedkeurder", True)

        If liStapNr <> 10 Then
            If (WFCurrentCase.GetProperty(Of String)("laatste goedkeurder?") = "neen (kies verdere afhandeling)" AndAlso WFCurrentCase.GetProperty(Of String)("Naar Gouverneur / Minister") = "Nee" AndAlso WFCurrentCase.GetProperty(Of String)("Ik keur het voorstel goed") = "ja") Then
                'WFCurrentCase.SetPropertyVisible("Afdeling2_bis",True)
                'WFCurrentCase.SetPropertyVisible("AfdelingAfhandeling",True)
                If WFCurrentCase.GetProperty(Of String)("AfdelingAfhandeling") <> "Eigen Afdeling" Then
                    WFCurrentCase.SetProperty("Afdeling2_bis", "(Role) " & WFCurrentCase.GetProperty(Of String)("AfdelingAfhandeling"))
                End If
            Else
                'rest
                'WFCurrentCase.SetPropertyVisible("Keuze goedkeurder", True)
                WFCurrentCase.SetPropertyVisible("Goedkeuring_afdeling", True)
                WFCurrentCase.SetPropertyVisible("goedkeuring_Dienst/TEAM/Cel", True)
                WFCurrentCase.SetPropertyVisible("goedkeurder", True)
                'einde test'
            End If
        End If

        ' show/hide datum verzenden van het stuk naar Minister.
        Select Case WFCurrentCase.CurrentStep.Step_Name
            Case "Goedkeuring - afdeling 2", "Onderzoek en voorstel - afdeling 2"
                If WFCurrentCase.GetProperty(Of String)("Moet dossier naar Minister?") = "ja" Then
                    WFCurrentCase.SetPropertyVisible("lbDatum_minister2", True)
                Else
                    WFCurrentCase.SetPropertyVisible("lbDatum_minister2", False)
                End If
            Case Else

        End Select

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "GoedkeurenAfd2_OnKeep"
        End Get
    End Property
End Class
