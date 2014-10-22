Public Class ToonWettelijkeToezichtstermijn
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)

        If Not WFCurrentCase.GetPropertyInfo("Termijn_NS").isEmpty Then
            WFCurrentCase.SetPropertyVisible("Termijn_NS", True)
            WFCurrentCase.SetPropertyVisible("Termijn_RO", False)

        Else
            If Not WFCurrentCase.GetPropertyInfo("Termijn_RO").isEmpty Then
                WFCurrentCase.SetPropertyVisible("Termijn_NS", False)
                WFCurrentCase.SetPropertyVisible("Termijn_RO", True)
            Else
                WFCurrentCase.SetPropertyVisible("Termijn_RO", False)
                WFCurrentCase.SetPropertyVisible("Termijn_NS", False)
            End If
        End If


    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "ToonWettelijkeToezichtstermijn"
        End Get
    End Property
End Class
