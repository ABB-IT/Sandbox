Imports Arco.Server
Imports Arco.Doma.Library
Imports Arco.Doma.Library.Routing
Imports System.Text
Imports Arco.ABB.Common

Public Class SetDossierNaam
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        Dim lsAardDossier As String
        Dim TypeBestuur As String
        Dim lsBestuurNaam As String

        Dim sbNaam As StringBuilder = New StringBuilder
        sbNaam.Append(WFCurrentCase.CaseData.Creation_Date.Substring(0, 4))

        sbNaam.Append(" - ")
        sbNaam.Append(WFCurrentCase.GetProperty(Of String)("S_Dossiernummer"))

        lsAardDossier = WFCurrentCase.GetProperty(Of String)("aard dossier")
        If Not String.IsNullOrEmpty(lsAardDossier) Then
            sbNaam.Append(" - ")
            If lsAardDossier <> "nazicht inzendingsplichtig besluit" Then
                sbNaam.Append(lsAardDossier)
            Else
                Dim lsSoort As String = WFCurrentCase.GetProperty(Of String)("type/soort besluit")
                sbNaam.Append("Nazicht ")
                sbNaam.Append(SoortInzendingsplichtigBesluit.GetSoortInzendingsplichtigBesluit(lsSoort).Opmerking)
                sbNaam.Append(":")
                'sbNaam.Append(lsSoort)
            End If
        End If

        TypeBestuur = WFCurrentCase.GetProperty(Of String)("type bestuur")
        If Not String.IsNullOrEmpty(TypeBestuur) Then
            sbNaam.Append(" ")
            sbNaam.Append(TypeBestuur)
        End If

        lsBestuurNaam = WFCurrentCase.GetProperty(Of String)("bestuur_naam")
        If Not String.IsNullOrEmpty(lsBestuurNaam) Then
            sbNaam.Append(" ")
            sbNaam.Append(lsBestuurNaam)
        End If
        If sbNaam.Length > 98 Then
            WFCurrentCase.Case_Name = sbNaam.ToString.Substring(0, 98)
        Else
            WFCurrentCase.Case_Name = sbNaam.ToString
        End If
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "ChangeCaseName"
        End Get
    End Property
End Class
