Public Class SoortInzendingsplichtigBesluit
    Public Property Opmerking As String
    Public Property AutoOpstart As Integer

    Private Sub New()

    End Sub
    Public Shared Function GetSoortInzendingsplichtigBesluit(ByVal vsSoortbesluit As String) As SoortInzendingsplichtigBesluit

        Dim lsSQL As String = "SELECT opmerking_SB,AUTO_OPSTART FROM bb_srt_inzendspl_besluit where soort_besluit ='" & vsSoortbesluit & "'"

        Dim loLijst As SoortInzendingsplichtigBesluit = New SoortInzendingsplichtigBesluit
        Using loQuery As Arco.Server.DataQuery = New Arco.Server.DataQuery
            loQuery.Query = lsSQL
            Try
                loQuery.Connect()
                Using loReader As Server.SafeDataReader = loQuery.ExecuteReader
                    While loReader.Read
                        loLijst.Opmerking = loReader.GetString(0)
                        loLijst.AutoOpstart = loReader.GetInt32(1)
                    End While
                End Using
            Catch ex As Exception
                Arco.Utils.Logging.Log("error GetSoortInzendingsplichtigBesluit:" & ex.Message)
            End Try
        End Using
        Return loLijst



    End Function
End Class
