Public Class VoorwerpInvullenNrLijst
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        Dim lsVoorwerp As String = WFCurrentCase.GetProperty(Of String)("voorwerp")
        Select Case lsVoorwerp
            Case "lijstbesluit"
                If WFCurrentCase.GetPropertyInfo("lijstbesluit_nr").isEmpty Then
                    WFCurrentCase.RejectComment = "U bent verplicht een lijstbesluit te selecteren!! "
                End If
            Case "ander besluit"
                If WFCurrentCase.GetPropertyInfo("ander_BESL_nr").isEmpty Then
                    WFCurrentCase.RejectComment = "U bent verplicht een ander besluit te selecteren!! "
                End If
            Case "inzendingsplichtig besluit"            
                If WFCurrentCase.GetPropertyInfo("inzend_BESL_nr").isEmpty Then
                    WFCurrentCase.RejectComment = "U bent verplicht een inzendingsplichtig besluit te selecteren!! "

                End If
            Case Else               
        End Select
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "VoorwerpInvullenNrLijst"
        End Get
    End Property
End Class
