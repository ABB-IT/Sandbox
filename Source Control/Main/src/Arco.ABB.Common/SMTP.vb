Imports System.Net.Mail
Imports Arco.Doma.Library
Imports Arco.Doma.Library.Helpers
Public Class SMTP
    Public Shared Function GetClient() As System.Net.Mail.SmtpClient
        Dim loSettings As Arco.Doma.Library.Helpers.ArcoInfo = Helpers.ArcoInfo.GetParameters()
        Return GetClient(loSettings)
    End Function
    Public Shared Function GetClient(ByVal voSettings As ArcoInfo) As System.Net.Mail.SmtpClient
        'todo : 
        'check settings in arocinfo match
        '  conSendUsing = 2
        ' conSendSMTPServer = "10.1.21.20"
        'conSendSMTPServerPort = 25
        Return Arco.Doma.Library.Helpers.SMTP.GetClient(voSettings)
    End Function
End Class
