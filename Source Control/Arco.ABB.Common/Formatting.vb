Imports Arco.Doma.Library

<Serializable()> _
Public Class Formatting
    Public Shared Function FormatAssignee(ByVal value As String) As String
        If Not String.IsNullOrEmpty(value) Then
            If value.Substring(0, 6).ToUpper = "(ROLE)" Then
                Return value
            Else
                Return ACL.User.GetUser(value).USER_DISPLAY_NAME
            End If
        Else
            Return ""
        End If
    End Function
End Class

