Imports Arco.ABB.Common
Public Class Goedkeuren_Vernietigingronden_OnExit
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(ByVal WFCurrentCase As Doma.Library.Routing.cCase)

        If WFCurrentCase.GetProperty(Of String)("ResultaatNH").ToLower = "vernietiging" Then
            If WFCurrentCase.GetProperty(Of String)("vernietigingsgronden") = "" Then
                If String.IsNullOrEmpty(WFCurrentCase.RejectComment) Then
                    WFCurrentCase.RejectComment = "Vernietigingsgronden: moet ingevuld zijn in geval van een vernietiging."
                Else
                    WFCurrentCase.RejectComment &= vbCrLf & "Vernietigingsgronden: moet ingevuld zijn in geval van een vernietiging."
                End If
            End If
        End If
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Goedkeuren_Vernietigingsgronden_OnExit"
        End Get
    End Property
End Class
