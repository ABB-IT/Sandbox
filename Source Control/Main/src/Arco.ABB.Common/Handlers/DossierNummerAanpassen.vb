Imports Arco.Doma.Library.Extensibility
Imports Arco.ABB.Common

<Serializable()> _
Public Class DossierNummerAanpassen
    Inherits ABBEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)


        Dim sNummer As String
        'Debug.PrintToFile  "(DOSIERNUMMER AANPASSEN) - 333"
        sNummer = WFCurrentCase.GetProperty(Of String)("S_Dossiernummer")

        If sNummer = "1" Then
            Dim lsSeq As String
            Dim lsyear As String = CustomArcoInfoDirect.GetParam("JAAR")
            If lsyear <> System.DateTime.Now.Year.ToString Then
                ' update year                
                CustomArcoInfoDirect.SetParam("JAAR", System.DateTime.Now.Year.ToString)
                'update seq set to 1
                lsSeq = "1"
            Else
                'add one to seq 
                lsSeq = CustomArcoInfoDirect.GetParam("SEQ_TOEZICHT")
                If String.IsNullOrEmpty(lsSeq) Then
                    lsSeq = "0"
                End If
                lsSeq = (Convert.ToInt32(lsSeq) + 1).ToString

            End If

            CustomArcoInfoDirect.SetParam("SEQ_TOEZICHT", lsSeq)
            Call WFCurrentCase.SetProperty("S_Dossiernummer", lsSeq)
        End If

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "DOSSIERNUMMER AANPASSEN"
        End Get
    End Property
End Class

