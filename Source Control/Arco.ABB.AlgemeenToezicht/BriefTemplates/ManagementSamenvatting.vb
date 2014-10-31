Imports Arco.ABB.Common
Imports Arco.Doma.Library
Public Class ManagementSamenvatting
    Inherits AlgemeenToezichtsNota
 
    Public Overrides Function ReplaceTags(lsContent As String, voCase As Doma.Library.Routing.cCase) As String
        lsContent = MyBase.ReplaceTags(lsContent, voCase)

        lsContent = lsContent.Replace("#systeemdatum#", System.DateTime.Now.ToString("dd/MM/yyyy"))

        Return lsContent

    End Function

    Public Overrides ReadOnly Property TemplateFile As String
        Get
            Return "MGTSamenv.rtf"
        End Get
    End Property
End Class
