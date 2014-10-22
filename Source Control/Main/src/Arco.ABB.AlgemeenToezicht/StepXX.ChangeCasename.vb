
Imports Arco.Server
Imports Arco.Doma.Library
Imports Arco.Doma.Library.Routing
Imports System.Text

Public MustInherit Class StepXX
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(ByVal WFCurrentCase As Doma.Library.Routing.cCase)
        Dim loChangeName As SetDossierNaam = New SetDossierNaam
        loChangeName.ExecuteCode(WFCurrentCase)
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "ChangeCaseName"
        End Get
    End Property
End Class

