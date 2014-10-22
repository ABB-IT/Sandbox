Imports Arco.Doma.Library

<Serializable()> _
Public Class Trefwoorden
    Public Shared Function GetTrefWoord(ByVal vsFTCid As String) As String
        Dim loIDList As List(Of String) = New List(Of String)
        loIDList.Add(vsFTCid)
        Return GetTrefwoordenLijst(loIDList)
    End Function
    Private Shared Function GetTrefwoordenLijst(ByVal voFTCIDList As List(Of String)) As String
        Dim loList As List(Of String) = New List(Of String)
        If voFTCIDList.Count > 0 Then
            Dim lsSQL As String
            lsSQL = "select trefwoord from bb_trefwoorden where ft_cid in (" & String.Join(",", voFTCIDList.ToArray) & ")"


            'lsSQL = "select trefwoord from bb_trefwoorden where ft_cid in ( "
            'lsSQL &= " select value_tostring from rtcase_property where case_id=" & WFCurrentcase.Case_ID.ToString & " and "
            'lsSQL &= " prop_id=( "
            'lsSQL &= " select prop_id from rtproperty where proc_id=( "
            'lsSQL &= " select proc_id from rtprocedure where proc_name= 'Adviesvragen') "
            'lsSQL &= " and prop_name = 'trefwoord' and parent_prop_id is not null)) "

            Using loQuery As Arco.Server.DataQuery = New Arco.Server.DataQuery
                loQuery.Query = lsSQL
                Try
                    loQuery.Connect()
                    Using loReader As Server.SafeDataReader = loQuery.ExecuteReader
                        While loReader.Read
                            loList.Add(loReader.GetString(0))
                        End While
                    End Using
                Catch ex As Exception
                    Arco.Utils.Logging.LogError("error GetTrefwoordenLijst:", ex)
                End Try
            End Using
        End If

        If loList.Count > 0 Then
            Return String.Join(" - ", loList.ToArray)
        Else
            Return ""
        End If
    End Function
    Public Shared Function GetTrefwoordenLijst(ByVal WFCurrentcase As Arco.Doma.Library.Routing.cCase) As String
        Dim loTable As baseObjects.DM_OBJECT.Table = CType(WFCurrentcase.GetProperty("trefwoordenlijst"), baseObjects.DM_OBJECT.Table)
        Dim loIDList As List(Of String) = New List(Of String)
        For Each loRow As baseObjects.DM_OBJECT.Table.TableRow In loTable.Rows
            If (loRow.Row_ID > 0) Then
                Dim lsID As String = WFCurrentcase.GetProperty(Of String)("trefwoord", loRow.Row_ID, loTable.Prop_ID)
                If Not String.IsNullOrEmpty(lsID) Then loIDList.Add(lsID)

            End If
        Next
        Return GetTrefwoordenLijst(loIDList)
    End Function
End Class

