Public Class Goedkeuren_OnKeep
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        WFCurrentCase.SetProperty("hiddentoewijzing3", WFCurrentCase.GetProperty("goedkeuring_Dienst/TEAM/Cel"))
        WFCurrentCase.SetProperty("hiddentoewijzing4", WFCurrentCase.GetProperty("keuze van de goedkeurder"))
        'If Not WFCurrentCase.GetPropertyInfo("keuze van de goedkeurder").isEmpty Then
        '    WFCurrentCase.SetProperty("goedkeurder", WFCurrentCase.GetProperty("keuze van de goedkeurder"))
        'End If

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Goedkeuren_OnKeep"
        End Get
    End Property
End Class
