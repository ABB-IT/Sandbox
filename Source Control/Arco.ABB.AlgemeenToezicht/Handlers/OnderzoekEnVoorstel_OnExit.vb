Public Class OnderzoekEnVoorstel_OnExit
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)       
        Dim lsdoorsturenAfdeling As String = WFCurrentCase.GetProperty(Of String)("doorsturen dossier")
        Dim lsGoedkeurder As String = WFCurrentCase.GetProperty(Of String)("keuze van de goedkeurder")
        If String.IsNullOrEmpty(lsdoorsturenAfdeling) OrElse lsdoorsturenAfdeling = "Nee" Then
            If String.IsNullOrEmpty(lsGoedkeurder) Then
                lsGoedkeurder = WFCurrentCase.GetProperty(Of String)("goedkeurder")
                If String.IsNullOrEmpty(lsGoedkeurder) Then
                    WFCurrentCase.RejectComment = "keuze goedkeurder moet ingevuld zijn."
                Else
                    WFCurrentCase.SetProperty("keuze van de goedkeurder", lsGoedkeurder)
                End If
            End If
        End If
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "OnderzoekEnVoorstel_OnExit"
        End Get
    End Property
End Class
