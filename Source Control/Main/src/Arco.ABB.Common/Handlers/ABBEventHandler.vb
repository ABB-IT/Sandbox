Imports Arco.Doma.Library
Public MustInherit Class ABBEventHandler
    Inherits Extensibility.RoutingEventHandler
    Public MustOverride Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
    Public MustOverride ReadOnly Property Name As String

    Public NotOverridable Overrides Sub Execute(WFCurrentCase As Doma.Library.Routing.cCase)
        Try
            Logging.AddToLog(WFCurrentCase, "BEGIN : " & Me.Name)

            ExecuteCode(WFCurrentCase)
        Catch ex As Exception
            Logging.AddToLog(WFCurrentCase, "ERROR : " & Me.Name & " : " & ex.Message)
            Throw
        Finally
            Logging.AddToLog(WFCurrentCase, "EINDE : " & Me.Name)
        End Try
    End Sub
End Class
