
<Serializable()> _
Public Class ContactPersoon

    Public Property VoorNaam As String
    Public Property Naam As String
    Public Property Email As String
    Public Property StraatNr As String
    Public Property PostCode As String
    Public Property Gemeente As String
    Public Property Telefoon As String
    Public Property Fax As String

    Private Sub New()

    End Sub
    Public Shared Function GetContactPersoon(ByVal ft_cid As String) As ContactPersoon
        Dim lsSQL As String
        Dim loRet As ContactPersoon = New ContactPersoon
        lsSQL = "select naam, voornaam , straatnr, postcode , gemeente,email, telefoon,fax from BB_klagers where FT_CID=" & ft_cid
        Using loQuery As Arco.Server.DataQuery = New Arco.Server.DataQuery
            loQuery.Query = lsSQL
            Try
                loQuery.Connect()
                Using loReader As Server.SafeDataReader = loQuery.ExecuteReader
                    If loReader.Read Then

                        loRet.VoorNaam = loReader.GetString("voornaam")
                        loRet.Naam = loReader.GetString("naam")
                        loRet.Email = loReader.GetString("email")
                        loRet.StraatNr = loReader.GetString("straatnr")
                        loRet.PostCode = loReader.GetString("postcode")
                        loRet.Gemeente = loReader.GetString("gemeente")
                        loRet.Telefoon = loReader.GetString("telefoon")
                        loRet.Fax = loReader.GetString("fax")
                    End If
                End Using
            Catch ex As Exception
                Arco.Utils.Logging.LogError("error HaalContactpersoon2:", ex)
            End Try
        End Using

        Return loRet
    End Function
End Class
