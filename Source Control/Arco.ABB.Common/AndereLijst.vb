Public Class AndereLijst
    Public Property BeslissingsOrgaan As String
    Public Property DatumBesluit As String
    Public Property TitelBesluit As String

    Private Sub New()

    End Sub
    Public Shared Function GetAndereLijst(ByVal ft_cid As String) As AndereLijst

        Dim lsSQL As String = "select beslissingsorgaan , datum_besluit,titel_besluit from BB_ander_besl where ft_Cid=" & ft_cid

        Dim loLijst As AndereLijst = New AndereLijst
        Using loQuery As Arco.Server.DataQuery = New Arco.Server.DataQuery
            loQuery.Query = lsSQL
            Try
                loQuery.Connect()
                Using loReader As Server.SafeDataReader = loQuery.ExecuteReader
                    While loReader.Read
                        loLijst.BeslissingsOrgaan = loReader.GetString(0)
                        loLijst.DatumBesluit = loReader.GetString(1)
                        loLijst.TitelBesluit = loReader.GetString(2)

                    End While
                End Using
            Catch ex As Exception
                Arco.Utils.Logging.Log("error GetAndereLijst:" & ex.Message)
            End Try
        End Using
        Return loLijst



    End Function
End Class
