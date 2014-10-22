Imports Arco.ABB.Common
Imports Arco.Doma.Library
Public Class BriefAanBestuur
    Inherits AlgemeenToezichtBrief


    Public Overrides ReadOnly Property TemplateFile As String
        Get
            Return "Briefaanbestuur.rtf"
        End Get
    End Property
End Class
