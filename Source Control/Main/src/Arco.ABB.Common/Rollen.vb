Imports Arco.Doma.Library

<Serializable()> _
Public Class Rollen
    Public Shared Function GetUsersFromRole(ByVal lsRoleName As String, ByVal vsFilterCode As String) As String
        Dim loRole As ACL.Role = ACL.Role.GetRoleByName(lsRoleName)
        Dim loList As List(Of String) = New List(Of String)
        If loRole.ROLE_ID > 0 Then
            For Each loMember As ACL.RoleMemberList.RoleMemberInfo In loRole.Members
                If Not String.IsNullOrEmpty(loMember.MEMBER) AndAlso loMember.MEMBERTYPE = "User" Then
                    Dim lbAdd As Boolean = False
                    If Not String.IsNullOrEmpty(vsFilterCode) Then
                        lbAdd = ACL.User.GetUser(loMember.MEMBER).USER_DESC.Contains("V")
                    Else
                        lbAdd = True
                    End If
                    If lbAdd Then loList.Add(loMember.MEMBER)
                End If
            Next
        Else
            'todo : error role not found
        End If
        If loList.Count > 0 Then
            Return String.Join(",", loList.ToArray)
        Else
            Return ""
        End If
    End Function
    Public Shared Function GetDienstTeam(ByVal vsBehandelaar As String) As ACL.RoleMemberList.RoleMemberInfo
        'this returns the first role the behandelelaar is in
        Dim loCrit As ACL.RoleMemberList.Criteria = New ACL.RoleMemberList.Criteria
        loCrit.MEMBER = vsBehandelaar
        loCrit.TYPE = "User"
        loCrit.SHOW_USERS = True
        loCrit.OrderBy = "ROLE_DESCRIPTION"

        Dim lcolMembers As ACL.RoleMemberList = ACL.RoleMemberList.GetRoleMemberList(loCrit)
        If lcolMembers.Count > 0 Then
            Return lcolMembers.Item(0)
        Else
            'todo : the user is in no roles
            Return Nothing
        End If
    End Function

    Public Shared Function GetAfdelingFromRol(ByVal roleid As Int32) As String
        Dim lsAfdeling As String = ""
        Dim lsSQL As String = ""


        'Correction: statement generates errors because of: ORA-01789: query block has incorrect number of result columns. 
        'lsSQL = " SELECT  ROLE_NAME ,ROLE_DESCRIPTION,ROLE_STRUCTURED FROM RTROLE WHERE ROLE_STRUCTURED=1  AND  "
        lsSQL = " SELECT  ROLE_NAME  FROM RTROLE WHERE ROLE_STRUCTURED=1  AND  "
        lsSQL &= "ROLE_ID IN (SELECT PARENT_ROLE FROM RTROLE_LINKS WHERE RTROLE_LINKS.CHILD_ROLE='" & roleid.ToString() & "') union "
        lsSQL &= "SELECT  ROLE_NAME  FROM RTROLE WHERE ROLE_STRUCTURED=1  AND "
        lsSQL &= " ROLE_ID IN (SELECT PARENT_ROLE FROM RTROLE_LINKS WHERE RTROLE_LINKS.CHILD_ROLE IN (SELECT PARENT_ROLE FROM RTROLE_LINKS WHERE RTROLE_LINKS.CHILD_ROLE='" & roleid.ToString() & "')) union "
        lsSQL &= " SELECT ROLE_NAME  FROM RTROLE WHERE ROLE_STRUCTURED=1  AND "
        lsSQL &= "ROLE_ID IN (SELECT PARENT_ROLE FROM RTROLE_LINKS WHERE RTROLE_LINKS.CHILD_ROLE IN "
        lsSQL &= " (SELECT PARENT_ROLE FROM RTROLE_LINKS WHERE RTROLE_LINKS.CHILD_ROLE IN (SELECT PARENT_ROLE FROM RTROLE_LINKS WHERE RTROLE_LINKS.CHILD_ROLE='" & roleid.ToString() & "'))) "
        lsSQL &= "ORDER BY 1 "


        Using loQuery As Arco.Server.DataQuery = New Arco.Server.DataQuery
            loQuery.Query = lsSQL
            Try
                loQuery.Connect()
                Using loReader As Server.SafeDataReader = loQuery.ExecuteReader
                    If loReader.Read Then
                        lsAfdeling = loReader.GetString(0)
                    End If
                End Using
            Catch ex As Exception
                Arco.Utils.Logging.LogError("error GetAfdelingFromRol:", ex)
            End Try
        End Using

        Return lsAfdeling

    End Function
End Class
