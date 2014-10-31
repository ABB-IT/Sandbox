<Serializable()> _
Public Class DatumNaarMinisterNaSchorsing
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        If WFCurrentCase.GetPropertyInfo("lbDatum_M_NS").isEmpty Then
            WFCurrentCase.SetProperty("lbDatum_M_NS", System.DateTime.Now)    
        End If
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "DatumNaarMinisterNaSchorsing"
        End Get
    End Property
End Class
