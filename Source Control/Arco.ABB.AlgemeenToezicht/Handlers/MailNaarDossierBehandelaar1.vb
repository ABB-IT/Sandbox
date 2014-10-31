Imports Arco.ABB.Common
Imports Arco.Doma.Library

Public Class MailNaarDossierBehandelaar1
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)


        '(00) Mail naar Dossierbehandelaar1        
        Dim lsBeh1 As String = WFCurrentCase.GetProperty(Of String)("dossierbehandelaar")
        Dim lsBeh2 As String = WFCurrentCase.GetProperty(Of String)("dossierbehandelaar2")
        Dim loBehandelaar1 As ACL.User
        If Not String.IsNullOrEmpty(lsBeh1) Then
            loBehandelaar1 = ACL.User.GetUser(lsBeh1)
        Else
            loBehandelaar1 = ACL.User.NewUser("")
        End If
        Dim loBehandelaar2 As ACL.User
        If Not String.IsNullOrEmpty(lsBeh2) Then
            loBehandelaar2 = ACL.User.GetUser(lsBeh2)
        Else
            loBehandelaar2 = ACL.User.NewUser("")
        End If

        '#### MAIL Template bij afsluiten_dossier


        Dim lsTemplateFile As String = System.IO.Path.Combine(ABB.Common.Constants.TemplatePath, "Mailtemplate_Afdeling1.txt")
        'Call AddToLog(lpFilePathGlobal  & "\" & "Template gevonden")

 
        Dim conBodyMailAfdeling1 As String

        If Arco.Doma.FileManager.File.Exists(lsTemplateFile) Then            
            conBodyMailAfdeling1 = Arco.Doma.FileManager.File.ReadTextFileToString(lsTemplateFile)
        Else
            conBodyMailAfdeling1 = "Geen e-mailinhoud gevonden."
        End If
        '###### GENEREER BODY MAIL ######
        Dim LpLinkDossier As String
        Dim lpWerklijst As String

        ' DBe 20140704: Aanpassing niet meer hardcoded, maar via parameter uit ArcoInfo ophalen.
        'lpWerklijst = "http://wv158904/DocRoom/"
        'LpLinkDossier = "http://wv158904/DocRoom/DM_Detailview.aspx?RTCASE_TECH_ID="
        'LpLinkDossier = LpLinkDossier & WFCurrentCase.Tech_ID
        Dim loCol As Arco.Doma.Library.Helpers.ArcoInfo = Arco.Doma.Library.Helpers.ArcoInfo.GetParameters()
        lpWerklijst = loCol.GetValue("DOMA", "url", "")
        LpLinkDossier = lpWerklijst & "DM_Detailview.aspx?RTCASE_TECH_ID=" & WFCurrentCase.Tech_ID
        Logging.AddToLog(WFCurrentCase, "Sending mail with link : " & LpLinkDossier)
        ' Einde aanpassing DBe 20140704.

        Dim lsBody As String

        lsBody = conBodyMailAfdeling1
        lsBody = lsBody.Replace("#Dossier#", WFCurrentCase.Case_Name)
        'lpBody  = Replace(lpBody,"#TECH_ID#", WFCurrentCase.Tech_ID)
        lsBody = lsBody.Replace("#TECH_ID#", LpLinkDossier)
        lsBody = lsBody.Replace("#DBH1#", loBehandelaar1.USER_DISPLAY_NAME)
        lsBody = lsBody.Replace("#DBH2#", loBehandelaar2.USER_DISPLAY_NAME)
        lsBody = lsBody.Replace("#werklijst#", lpWerklijst)


        Dim loSmtp As System.Net.Mail.SmtpClient = ABB.Common.SMTP.GetClient()
        Using loMsg As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
            loMsg.From = New System.Net.Mail.MailAddress("dbs@bz.vlaanderen.be")
            loMsg.Subject = " Dossier: " & WFCurrentCase.Case_Name & "  werd doorgestuurd in DBS vanuit de CA naar  jou."

            Dim lsMailDossierbehandelaar1 As String = ""
            lsMailDossierbehandelaar1 = loBehandelaar1.USER_MAIL
            If String.IsNullOrEmpty(lsMailDossierbehandelaar1) Then
                lsMailDossierbehandelaar1 = "dbs@bz.vlaanderen.be"
            End If
            If lpWerklijst.Contains("158904") = True Then
                lsMailDossierbehandelaar1 = "dbs@bz.vlaanderen.be"      ' op preproductie: vast mailadres gebruiken.
            End If
            Logging.AddToLog(WFCurrentCase, "Mail wordt verstuurd naar : " & lsMailDossierbehandelaar1)
            If Not String.IsNullOrEmpty(lsMailDossierbehandelaar1) Then
                loMsg.To.Add(New System.Net.Mail.MailAddress(lsMailDossierbehandelaar1))
            End If

            loMsg.Body = lsBody
            loMsg.IsBodyHtml = False

            loSmtp.Send(loMsg)
        End Using

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "MailNaarDossierBehandelaar1"
        End Get
    End Property
End Class
