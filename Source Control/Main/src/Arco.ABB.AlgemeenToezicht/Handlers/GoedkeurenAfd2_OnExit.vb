Imports Arco.ABB.Common

Public Class GoedkeurenAfd2_OnExit
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        Dim loCheckTrefwoorden As TrefwoordVerplicht_OnExit = New TrefwoordVerplicht_OnExit
        loCheckTrefwoorden.Execute(WFCurrentCase)

        If String.IsNullOrEmpty(WFCurrentCase.RejectComment) Then
            If WFCurrentCase.GetPropertyInfo("goedkeurder").isEmpty AndAlso (WFCurrentCase.GetProperty(Of String)("laatste goedkeurder2?") = "ja" OrElse WFCurrentCase.GetPropertyInfo("laatste goedkeurder2?").isEmpty) Then             
                WFCurrentCase.RejectComment = "geen goedkeurder aangeduid! "
            End If
        End If


    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "GoedkeurenAfd2_OnExit"
        End Get
    End Property
End Class