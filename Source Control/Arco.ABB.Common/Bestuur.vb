
<Serializable()> _
Public Class Bestuur
    Public Property Type As String
    Public Property Naam As String
    Public Property NIS As String
    Public Property StraatNr As String
    Public Property PostCode As String
    Public Property Gemeente As String

    Private Sub New()

    End Sub
    Public Shared Function GetBestuur(ByVal ft_cid As Int32) As Bestuur
        Return GetBestuur(ft_cid.ToString)
    End Function
    Public Shared Function GetBestuur(ByVal ft_cid As String) As Bestuur
        Dim lsSQL As String

        Dim loBestuur As Bestuur = New Bestuur
        lsSQL = "select TYPE, NAAM,NIS,STRAATNR, POSTCODE, GEMEENTE from BB_ADRESBESTUREN  where FT_CID=" & ft_cid
        Using loQuery As Arco.Server.DataQuery = New Arco.Server.DataQuery
            loQuery.Query = lsSQL
            Try
                loQuery.Connect()
                Using loReader As Server.SafeDataReader = loQuery.ExecuteReader
                    If loReader.Read Then
                        loBestuur.Type = loReader.GetString("type")
                        loBestuur.Naam = loReader.GetString("naam")
                        loBestuur.NIS = loReader.GetString("NIS")
                        loBestuur.StraatNr = loReader.GetString("straatnr")
                        loBestuur.PostCode = loReader.GetString("postcode")
                        loBestuur.Gemeente = loReader.GetString("gemeente")
                    End If
                End Using
            Catch ex As Exception
                Arco.Utils.Logging.Logerror("error HaalBestuurOp2:", ex)
            End Try
        End Using

        Return loBestuur
    End Function
End Class

