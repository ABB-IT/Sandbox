Imports Arco.ABB.Common
Imports Arco.Doma.Library

Public Class MailNaarGeneriekeMailboxCA
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)

        Dim loBehandelaar1 As ACL.User = ACL.User.GetUser(WFCurrentCase.GetProperty(Of String)("dossierbehandelaar"))
       
        'msgbox " goedkeurder : " & lpLaatsteGoedkeurder 
        Dim lsAfdeling As String = WFCurrentCase.GetProperty(Of String)("Afdeling2_bis")
      
        ' Dim loLaatsteGoedkeurder As ACL.User = ACL.User.GetUser(WFCurrentCase.GetProperty(Of String)("goedkeurder"))
       
        'msgbox " Mail afzender " & lpMailLaatsteGoedkeurder 
        Dim lsMailAfdeling As String = ""
        If lsAfdeling = "(Role) Afdeling Regelgeving en Werking" Then
            ' DBe 20140722: wijziging van de adressen aangevraagd per mail van GVO dd. 20140714 10:10
            'lsMailAfdeling = "dbs@bz.vlaanderen.be"
            lsMailAfdeling = "binnenland-juridisch@vlaanderen.be@bz.vlaanderen.be"
        ElseIf lsAfdeling = "(Role) Afdeling Financiën en Personeel" Then
            ' DBe 20140722: wijziging van de adressen aangevraagd per mail van GVO dd. 20140714 10:10
            'lsMailAfdeling = "dbs@bz.vlaanderen.be"
            lsMailAfdeling = "johan.ide@bz.vlaanderen.be"
        End If

        Dim conBodyMailAfdeling2 As String
        Dim lsTemplateFile As String = System.IO.Path.Combine(ABB.Common.Constants.TemplatePath, "Mailtemplate_Afdeling2.txt")
        If Arco.Doma.FileManager.File.Exists(lsTemplateFile) Then
            conBodyMailAfdeling2 = Arco.Doma.FileManager.File.ReadTextFileToString(lsTemplateFile)
        Else
            conBodyMailAfdeling2 = "Geen e-mailinhoud gevonden."
        End If

        '###### GENEREER BODY MAIL ######
        Dim LpLinkDossier As String

        ' DBe 20140704: Aanpassing niet meer hardcoded, maar via parameter uit ArcoInfo ophalen.
        'LpLinkDossier = "http://wv158904/DocRoom/DM_Detailview.aspx?RTCASE_TECH_ID="
        'LpLinkDossier = LpLinkDossier & WFCurrentCase.Tech_ID
        Dim loCol As Arco.Doma.Library.Helpers.ArcoInfo = Arco.Doma.Library.Helpers.ArcoInfo.GetParameters()
        LpLinkDossier = loCol.GetValue("DOMA", "url", "") & "DM_Detailview.aspx?RTCASE_TECH_ID=" & WFCurrentCase.Tech_ID
        Logging.AddToLog(WFCurrentCase, "Sending mail with link : " & LpLinkDossier)
        ' Einde aanpassing DBe 20140704.

        Dim lsBody As String
        lsBody = conBodyMailAfdeling2
        lsBody = lsBody.Replace("#Dossier#", WFCurrentCase.Case_Name)
        'lpBody  = Replace(lpBody,"#TECH_ID#", WFCurrentCase.Tech_ID)
        lsBody = lsBody.Replace("#TECH_ID#", LpLinkDossier)

        Dim loSmtp As System.Net.Mail.SmtpClient = ABB.Common.SMTP.GetClient()
        Using loMsg As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
            loMsg.From = New System.Net.Mail.MailAddress("dbs@bz.vlaanderen.be")
            loMsg.Subject = " Dossier: " & WFCurrentCase.Case_Name & "  werd doorgestuurd in DBS vanuit de PA naar uw afdeling."

            If Not String.IsNullOrEmpty(lsMailAfdeling) Then
                loMsg.To.Add(New System.Net.Mail.MailAddress(lsMailAfdeling))
                Logging.AddToLog(WFCurrentCase, "Mail wordt verstuurd naar : " & lsMailAfdeling)
            End If
            Try
                If Not String.IsNullOrEmpty(loBehandelaar1.USER_MAIL) Then
                    loMsg.CC.Add(New System.Net.Mail.MailAddress(loBehandelaar1.USER_MAIL))
                    Logging.AddToLog(WFCurrentCase, "Mail (CC) wordt verstuurd naar : " & loBehandelaar1.USER_MAIL)
                End If
            Catch ex As Exception
            End Try

            loMsg.Bcc.Add(New System.Net.Mail.MailAddress("guy.vanoudenhove@bz.vlaanderen.be"))
            loMsg.Body = lsBody
            loMsg.IsBodyHtml = False

            loSmtp.Send(loMsg)
        End Using

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "MailNaarGeneriekeMailboxCA"
        End Get
    End Property
End Class
