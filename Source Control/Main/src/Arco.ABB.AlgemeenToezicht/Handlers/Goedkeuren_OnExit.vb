Imports Arco.ABB.Common
Public Class Goedkeuren_OnExit
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        Dim loCheckTrefwoorden As TrefwoordVerplicht_OnExit = New TrefwoordVerplicht_OnExit
        loCheckTrefwoorden.Execute(WFCurrentCase)

        If String.IsNullOrEmpty(WFCurrentCase.RejectComment) Then
            If WFCurrentCase.GetPropertyInfo("goedkeurder").isEmpty AndAlso WFCurrentCase.GetProperty(Of String)("doorsturen dossier") = "Nee" Then               
                    WFCurrentCase.RejectComment = "geen goedkeurder aangeduid! "               
            End If
        End If

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Goedkeuren_OnExit"
        End Get
    End Property
End Class
