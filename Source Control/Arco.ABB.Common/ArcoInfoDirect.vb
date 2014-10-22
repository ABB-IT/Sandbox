
''' <summary>
''' class for uncached access to arcoinfo
''' </summary>
''' <remarks></remarks>
<Serializable()> _
Public Class CustomArcoInfoDirect
    Public Shared Sub SetParam(ByVal vsName As String, ByVal vsValue As String)
        Using loQuery As Arco.Server.DataQuery = New Arco.Server.DataQuery

            Try
                loQuery.Connect()
                loQuery.Query = "update arcoinfo set parmvalue='" & vsValue & "' where arcoinfo.category='Custom' and parmname='" & vsName & "'"
                loQuery.ExecuteNonQuery()
            Catch ex As Exception
                Arco.Utils.Logging.LogError("error HaalContactpersoon2:", ex)
            End Try
        End Using
    End Sub
    Public Shared Function GetParam(ByVal vsName As String) As String
        Using loQuery As Arco.Server.DataQuery = New Arco.Server.DataQuery
            loQuery.Query = "select parmvalue from arcoinfo where arcoinfo.category='Custom' and parmname='" & vsName & "'"
            Try
                loQuery.Connect()
                Using loReader As Server.SafeDataReader = loQuery.ExecuteReader
                    If loReader.Read Then
                        Return loReader.GetString(0)
                    Else
                        Return ""
                    End If
                End Using
            Catch ex As Exception
                Arco.Utils.Logging.LogError("error GetParam:", ex)
                Return ""
            End Try
        End Using
    End Function
End Class

