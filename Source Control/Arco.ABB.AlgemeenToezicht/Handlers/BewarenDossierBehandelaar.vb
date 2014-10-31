Public Class BewarenDossierBehandelaar
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        'this script does exactly the same
        Dim loOtherScript As AfsluitenDossier_OnEntry = New AfsluitenDossier_OnEntry()
        loOtherScript.ExecuteCode(WFCurrentCase)

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "BewarenDossierBehandelaar"
        End Get
    End Property
End Class
