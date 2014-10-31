Public Class GoedkeurenAfd1_OnKeep
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)

        WFCurrentCase.SetProperty("hiddentoewijzing3", WFCurrentCase.GetProperty("goedkeuring_Dienst/TEAM/Cel"))
        WFCurrentCase.SetProperty("hiddentoewijzing4", WFCurrentCase.GetProperty("keuze van de goedkeurder"))
        If Not WFCurrentCase.GetPropertyInfo("keuze van de goedkeurder").isEmpty Then
            WFCurrentCase.SetProperty("goedkeurder", WFCurrentCase.GetProperty("keuze van de goedkeurder"))
        Else

        End If
     
        If WFCurrentCase.GetProperty(Of String)("AfdelingAfhandeling") <> "Eigen Afdeling" Then
            WFCurrentCase.SetProperty("Afdeling2_bis", "(Role) " & WFCurrentCase.GetProperty(Of String)("AfdelingAfhandeling"))
        End If
        'WFCurrentCase.SetPropertyVisible("Kies goedkeurder", True)
        WFCurrentCase.SetPropertyVisible("Goedkeuring_afdeling", True)
        WFCurrentCase.SetPropertyVisible("goedkeuring_Dienst/TEAM/Cel", True)
        WFCurrentCase.SetPropertyVisible("goedkeurder", True)

        Select Case WFCurrentCase.CurrentStep.Step_Name
            Case "Goedkeuring - afdeling 1"
                If WFCurrentCase.GetProperty(Of String)("Naar Gouverneur / Minister") = "Ja" Then
                    WFCurrentCase.SetPropertyVisible("Datum dossier naar G/M", True)
                Else
                    WFCurrentCase.SetPropertyVisible("Datum dossier naar G/M", False)
                End If
            Case Else
        End Select

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "AlgemeenToezichtEventHandler"
        End Get
    End Property
End Class
