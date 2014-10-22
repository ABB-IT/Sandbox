Imports Arco.Doma.Library
Imports Arco.Doma.Library.Routing
Imports Arco.ABB.Common

<Serializable()> _
Public MustInherit Class RTFBriefTemplate
    Implements IBriefTemplate

    Public MustOverride Function ReplaceTags(ByVal vsContent As String, ByVal voCase As cCase) As String
    Public MustOverride ReadOnly Property TemplateFile As String

    Public Function CreateFromTemplate(ByVal voCase As cCase, ByVal voSettings As Arco.Doma.Library.Helpers.ArcoInfo) As String Implements IBriefTemplate.CreateFromTemplate
        Dim lsUniqueFileName As String = Arco.Utils.GUID.CreateGUIDWithCheckSum() & ".rtf"

        'save to upload dir
        Dim lsTempDir As String = Arco.Doma.FileManager.Directory.AddSlash(voSettings.GetValue("Locations", "DefaultUploadPath", ""))
        Dim lsTempPath As String = System.IO.Path.Combine(lsTempDir, lsUniqueFileName)

        ' Template openen, lezen en sluiten
        Dim lsTemplatePath As String = System.IO.Path.Combine(ABB.Common.Constants.TemplatePath, Me.TemplateFile)
        Dim lsContent As String = Arco.Doma.FileManager.File.ReadTextFileToString(lsTemplatePath)

        lsContent = ReplaceTags(lsContent, voCase)

        'Arco.Doma.FileManager.File.WriteFile(lsContent, lsTempPath, System.Text.Encoding.UTF8)

        Using w As IO.StreamWriter = New IO.StreamWriter(lsTempPath, True, System.Text.Encoding.Default)
            w.Write(lsContent)
        End Using

        Return lsTempPath
    End Function
End Class
