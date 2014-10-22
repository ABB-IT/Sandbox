Public Class WachtenOpOntvangstAntwoord_OnExit
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)   
        Dim lsdoorsturenAfdeling As String = WFCurrentCase.GetProperty(Of String)("doorsturen dossier")
        If String.IsNullOrEmpty(lsdoorsturenAfdeling) OrElse lsdoorsturenAfdeling = "Nee" Then
            ' controle onskernmerk & brief verstuurd op
            If WFCurrentCase.GetProperty(Of Boolean)("Stukken binnen?") AndAlso Not WFCurrentCase.GetPropertyInfo("postdatum stukken").isEmpty Then
            Else
                WFCurrentCase.RejectComment = "Alle PostStukken moeten binnen zijn , Het veld Datum verzenden poststukken moet ingevuld zijn"              
            End If
        End If

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "WachtenOpOntvangstAntwoord_OnExit"
        End Get
    End Property
End Class
