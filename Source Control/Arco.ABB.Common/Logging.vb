Imports Arco.Doma.Library.Routing

<Serializable()> _
Public Class Logging
    Private Shared _writer As Arco.Utils.LogWriter
    Private Shared ReadOnly Property Writer As Arco.Utils.LogWriter
        Get
            If _writer Is Nothing Then
                _writer = New Arco.Utils.LogWriter("d:\Arco\Logging\ABBHandlers.log", False)
                'sNow = Year(dNow) & Right("100" & Month(dNow), 2) & Right("100" & Day(dNow), 2)
                'sFile = "d:\Arco\Logging\algemeentoezicht_" & sNow & ".log"
            End If
            Return _writer
        End Get
    End Property

    Public Shared Sub AddToLog(ByVal voCase As Doma.Library.Routing.cCase, ByVal line As String)
        Arco.Utils.Logging.Log(voCase.Tech_ID & " " & line, Writer)
    End Sub
End Class

