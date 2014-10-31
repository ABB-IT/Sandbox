Imports Arco.ABB.Common

Public Class GoedkeurenAfd1_OnExit
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        Dim loCheckTrefwoorden As TrefwoordVerplicht_OnExit = New TrefwoordVerplicht_OnExit
        loCheckTrefwoorden.Execute(WFCurrentCase)

        If String.IsNullOrEmpty(WFCurrentCase.RejectComment) Then
            If WFCurrentCase.GetPropertyInfo("goedkeurder").isEmpty Then
                Dim liStapNr As Integer = StapNummers.GetStapNummer(WFCurrentCase)
                If WFCurrentCase.GetProperty(Of String)("laatste goedkeurder?") = "ja (kies goedkeurder)" OrElse (liStapNr = 11 OrElse liStapNr = 4) Then

                    WFCurrentCase.RejectComment = "geen goedkeurder aangeduid! "
                End If
            End If
        End If


    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "GoedkeurenAfd1_OnExit"
        End Get
    End Property
End Class
