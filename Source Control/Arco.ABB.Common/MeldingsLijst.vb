Public Class MeldingsLijst
    Public Property Gemeente As String
    Public Property DatumZitting As String
    Public Property PostDatum As String
    Public Property OntvangstDatum As String
    Public Property InitieleTermijn As String

    Private Sub New()

    End Sub
    Public Shared Function GetMeldingsLijst(ByVal ft_cid As String) As MeldingsLijst

        Dim lsSQL As String = "select GEMEENTE, DATUM_ZITTING,POST_DATUM , ONTVANGSTDATUM,INITIELE_TERMIJN  from bb_meldingslijst where FT_CID=" & ft_cid
        Dim loLijst As MeldingsLijst = New MeldingsLijst
        Using loQuery As Arco.Server.DataQuery = New Arco.Server.DataQuery
            loQuery.Query = lsSQL
            Try
                loQuery.Connect()
                Using loReader As Server.SafeDataReader = loQuery.ExecuteReader
                    While loReader.Read
                        loLijst.Gemeente = loReader.GetString(0)
                        loLijst.DatumZitting = loReader.GetString(1)
                        loLijst.PostDatum = loReader.GetString(2)
                        loLijst.OntvangstDatum = loReader.GetString(3)
                        loLijst.InitieleTermijn = loReader.GetString(4)
                        If String.IsNullOrEmpty(loLijst.Gemeente) AndAlso String.IsNullOrEmpty(loLijst.DatumZitting) AndAlso String.IsNullOrEmpty(loLijst.PostDatum) Then
                            loLijst.OntvangstDatum = ""
                            loLijst.InitieleTermijn = ""
                        End If
                    End While
                End Using
            Catch ex As Exception
                Arco.Utils.Logging.Log("error GetMeldingsLijst:" & ex.Message)
            End Try
        End Using
        Return loLijst

       

    End Function
End Class
