Imports Arco.ABB.Common
Imports Arco.Doma.Library
Public Class BriefAanklager
    Inherits AlgemeenToezichtBrief

    Public Sub New()
        Me.VervangBestuurTags = False
        Me.VulPostDatumKlachtIn = True
    End Sub
 
 
    Public Overrides Function ReplaceTags(lsContent As String, voCase As Doma.Library.Routing.cCase) As String
        lsContent = MyBase.ReplaceTags(lsContent, voCase) 'tags that are always replaced
     
      
             
        lsContent = lsContent.Replace("#klager_voornaam#", voCase.GetProperty(Of String)("klager_voornaam"))
        lsContent = lsContent.Replace("#klager_naam#", voCase.GetProperty(Of String)("klager_naam"))
        lsContent = lsContent.Replace("#klager_straatnr#", voCase.GetProperty(Of String)("klager_straatnr"))
        lsContent = lsContent.Replace("#klager_postnummer#", voCase.GetProperty(Of String)("klager_postnummer"))
        lsContent = lsContent.Replace("#klager_woonplaats#", voCase.GetProperty(Of String)("klager_gemeente"))

        lsContent = lsContent.Replace("#systeemdatum#", "")


        Return lsContent

    End Function

    Public Overrides ReadOnly Property TemplateFile As String
        Get
            Return "Briefaanklager.rtf"
        End Get
    End Property
End Class
