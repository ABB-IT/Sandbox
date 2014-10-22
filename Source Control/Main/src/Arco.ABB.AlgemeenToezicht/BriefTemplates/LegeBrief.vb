Imports Arco.ABB.Common
Imports Arco.Doma.Library
Public Class LegeBrief
    Inherits AlgemeenToezichtBrief

  
    Public Overrides ReadOnly Property TemplateFile As String
        Get
            Return "Legebrief.rtf"
        End Get
    End Property
End Class
