Imports Arco.Doma.Library
Public Class Stappen
    Public Shared Function GetStapID(ByVal vsnaam As String, ByVal vlProcID As Integer) As Integer
        Dim lcolSteps As Routing.StepList = Routing.StepList.GetStepList(vlProcID)
        For Each loStep As Routing.StepList.StepInfo In lcolSteps
            If loStep.Step_Name.Equals(vsnaam, StringComparison.CurrentCultureIgnoreCase) Then
                Return loStep.Step_ID
            End If
        Next
        Return 0
    End Function
End Class
