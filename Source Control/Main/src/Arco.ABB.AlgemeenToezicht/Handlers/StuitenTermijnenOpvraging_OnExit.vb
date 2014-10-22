Public Class StuitenTermijnenOpvraging_OnExit
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        Dim loInclude As VoorwerpInvullenNrLijst = New VoorwerpInvullenNrLijst
        loInclude.Execute(WFCurrentCase)

        If WFCurrentCase.GetProperty(Of Boolean)("Stukken opvragen?") = False Then
            WFCurrentCase.SetProperty("TermijnStuiten", False)      
        End If

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "StuitenTermijnenOpvraging_OnExit"
        End Get
    End Property
End Class
