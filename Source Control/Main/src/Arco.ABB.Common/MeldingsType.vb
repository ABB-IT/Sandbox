
<Serializable()> _
Public Class MeldingsType
    Public Property Type As String
    Public Property Termijn As Int32
    Public Shared Function GetMeldingsType(ByVal vsTypeBestuur As String) As MeldingsType
        Dim lssql As String

        Dim loType As MeldingsType = New MeldingsType

        lssql = "SELECT type,termijn FROM bb_type where meldingstype='" & vsTypeBestuur & "'"
        Using loQuery As Arco.Server.DataQuery = New Arco.Server.DataQuery
            loQuery.Query = lssql
            Try
                loQuery.Connect()
                Using loReader As Server.SafeDataReader = loQuery.ExecuteReader
                    While loReader.Read
                        loType.Type = loReader.GetString(0)
                        loType.Termijn = loReader.GetInt32(1)
                    End While
                End Using
            Catch ex As Exception
                Arco.Utils.Logging.Log("error GetMeldingstype:" & ex.Message)
            End Try
        End Using

        Return loType
    End Function
End Class

