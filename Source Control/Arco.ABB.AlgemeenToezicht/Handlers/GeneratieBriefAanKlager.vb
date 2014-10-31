Imports Arco.ABB.Common
Imports Arco.Doma.Library

Public Class GeneratieBriefAanKlager
    Inherits AlgemeenToezichtEventHandler

    Private Sub AddTemplate(ByVal WFCurrentCase As Doma.Library.Routing.cCase, ByVal voTemplate As Arco.ABB.Common.IBriefTemplate, ByVal vsTitel As String, ByVal vsPackage As String, ByVal vbClearFirst As Boolean)
        If Not voTemplate Is Nothing Then
            If vbClearFirst Then
                WFCurrentCase.RemoveAllFromPackage(vsPackage)
            End If
            Dim loSettings As Arco.Doma.Library.Helpers.ArcoInfo = Arco.Doma.Library.Helpers.ArcoInfo.GetParameters
            Dim lsCreatedFile As String = voTemplate.CreateFromTemplate(WFCurrentCase, loSettings)

            Dim loRoutingFile As Arco.Doma.Library.Routing.RoutingFile = Arco.Doma.Library.Routing.RoutingFile.NewFile(WFCurrentCase.Case_ID, WFCurrentCase.GetPackageInfo(vsPackage).ID)
            loRoutingFile.Title = vsTitel
            loRoutingFile.MoveFileToTargetPath = True
            loRoutingFile.TargetBasePath = Arco.Doma.FileManager.Directory.AddSlash(loSettings.GetValue("Locations", "DocPath", ""))
            loRoutingFile.Path = lsCreatedFile

            If Not String.IsNullOrEmpty(WFCurrentCase.StepExecutor) Then
                loRoutingFile.Author = ACL.User.GetUser(WFCurrentCase.StepExecutor).USER_DISPLAY_NAME
            End If

            loRoutingFile = loRoutingFile.Save
        End If
    End Sub
    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)

        Dim lsHiddenAdd As String = WFCurrentCase.GetProperty(Of String)("hiddenAdd")

        If lsHiddenAdd = "Add" Then
            WFCurrentCase.SetProperty("hiddenAdd", "") 'todo : convert to virtual property

            Dim lsKeuzetemplate As String = WFCurrentCase.GetProperty(Of String)("Keuze template")
            Dim lcsRoutingPack As String = "bijlagen"

            Dim loTemplate As IBriefTemplate = Nothing
            Dim dTemp As DateTime = Now
            ' Vraag van ABB: formaat van naamgeving wijzigen met JJJJMMDD en alle spaties vervangen door underscores.
            Select Case lsKeuzetemplate
                Case "Brief aan klager"
                    'AddTemplate(WFCurrentCase, New BriefAanklager, lsKeuzetemplate, lcsRoutingPack, False)
                    AddTemplate(WFCurrentCase, New BriefAanklager, lsKeuzetemplate.Replace(" ", "_") & "_" & dTemp.ToString("dd/MM/yyyy_HH:mm:ss"), lcsRoutingPack, False)
                Case "Brief aan bestuur"
                    'AddTemplate(WFCurrentCase, New BriefAanBestuur, lsKeuzetemplate, lcsRoutingPack, False)
                    AddTemplate(WFCurrentCase, New BriefAanBestuur, lsKeuzetemplate.Replace(" ", "_") & "_" & dTemp.ToString("dd/MM/yyyy_HH:mm:ss"), lcsRoutingPack, False)
                Case "Lege brief"
                    'AddTemplate(WFCurrentCase, New LegeBrief, lsKeuzetemplate, lcsRoutingPack, False)
                    AddTemplate(WFCurrentCase, New LegeBrief, lsKeuzetemplate.Replace(" ", "_") & "_" & dTemp.ToString("dd/MM/yyyy_HH:mm:ss"), lcsRoutingPack, False)
                Case "Nota aan minister"
                    'AddTemplate(WFCurrentCase, New NotaAanMinister, lsKeuzetemplate, lcsRoutingPack, False)
                    AddTemplate(WFCurrentCase, New NotaAanMinister, lsKeuzetemplate.Replace(" ", "_") & "_" & dTemp.ToString("dd/MM/yyyy_HH:mm:ss"), lcsRoutingPack, False)
                Case "Nota aan gouverneur"
                    'AddTemplate(WFCurrentCase, New NotaAanGouverneur, lsKeuzetemplate, lcsRoutingPack, False)
                    AddTemplate(WFCurrentCase, New NotaAanGouverneur, lsKeuzetemplate.Replace(" ", "_") & "_" & dTemp.ToString("dd/MM/yyyy_HH:mm:ss"), lcsRoutingPack, False)
                    'Case "MGTSamenv"
                    '   AddTemplate(WFCurrentCase, New ManagementSamenvatting, "Management Samenvatting", lcsRoutingPack, True)
            End Select
        End If

        ' Creëer Managementsamenvatting.
        Dim lsHiddenAdd10 As String = WFCurrentCase.GetProperty(Of String)("hiddenAdd10")
        If lsHiddenAdd10 = "Add" Then
            WFCurrentCase.SetProperty("hiddenAdd10", "") 'todo : convert to virtual property
            Dim dTemp As DateTime = Now
            AddTemplate(WFCurrentCase, New ManagementSamenvatting, "Management_Samenvatting_" & dTemp.ToString("dd/MM/yyyy_HH:mm:ss"), "Management Samenvatting", False)
        End If

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "GeneratieBriefAanKlager"
        End Get
    End Property
End Class
