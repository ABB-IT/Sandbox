Imports Arco.ABB.Common
Imports Arco.Doma.Library.Routing

Public MustInherit Class AlgemeenToezichtsNota
    Inherits AlgemeenToezichtBrief
    Public Overrides Function ReplaceTags(ByVal lsContent As String, ByVal voCase As Doma.Library.Routing.cCase) As String
        lsContent = MyBase.ReplaceTags(lsContent, voCase)

        Dim lstrefwoorden As String = Trefwoorden.GetTrefwoordenLijst(voCase)
        lsContent = lsContent.Replace("#trefwoorden#", lstrefwoorden)
        lsContent = VervangDatumKabinet(voCase, lsContent)
        lsContent = VervangSamenVattingEnKennisDeling(voCase, lsContent)
        lsContent = lsContent.Replace("#voorstelbeslissing#", voCase.GetProperty(Of String)("resultaat onderzoek"))
        lsContent = VervangTermijn(voCase, lsContent)
        lsContent = VervangGrondRedenVanSchorsing(voCase, lsContent)
        Return lsContent
    End Function

    Protected Function VervangDatumKabinet(ByVal voCase As cCase, ByVal lsContent As String) As String
        Dim lsdatumkabinet As String = voCase.GetProperty(Of String)("Datum dossier naar G/M")
        If Not String.IsNullOrEmpty(lsdatumkabinet) Then
            Dim dTemp As DateTime
            If DateTime.TryParse(lsdatumkabinet, dTemp) Then
                lsdatumkabinet = dTemp.ToString("dd/MM/yyyy")
                'lsdatumkabinet = LeadingZero(Day(CDate(lsdatumkabinet)), 2) & "/" & LeadingZero(Month(CDate(lsdatumkabinet)), 2) & "/" & Year(CDate(lsdatumkabinet))
            End If
        End If
        lsContent = lsContent.Replace("#datum kabinet#", lsdatumkabinet)

        Return lsContent
    End Function

    Protected Function VervangTermijn(ByVal voCase As cCase, ByVal lsContent As String) As String
        Dim lsTermijn As String = voCase.GetProperty(Of String)("huidige termijn")

        If Not String.IsNullOrEmpty(lsTermijn) Then
            Dim dTemp As DateTime
            If DateTime.TryParse(lsTermijn, dTemp) Then
                lsTermijn = dTemp.ToString("dd/MM/yyyy")
            End If
        End If
        lsContent = lsContent.Replace("#datum binnenkomst#", lsTermijn)

        Return lsContent
    End Function

    Protected Function VervangSamenVattingEnKennisDeling(ByVal voCase As cCase, ByVal lsContent As String) As String
        Dim lsSamenvatting, lsKennisdeling2, lsKennisdeling1, lsKennisdeling3, lsKennisdeling4 As String
        lsSamenvatting = voCase.GetProperty(Of String)("lbSamenvattingDossier")

        If (voCase.GetProperty(Of Boolean)("lbPrecedent") = False) Then
            lsKennisdeling2 = "Neen"
        Else
            lsKennisdeling2 = "    Ja"
        End If

        If (voCase.GetProperty(Of Boolean)("lbBeleidsstandpunt") = False) Then
            lsKennisdeling1 = "Neen"
        Else
            lsKennisdeling1 = "    Ja"
        End If

        If (voCase.GetProperty(Of Boolean)("lbFAQ") = False) Then
            lsKennisdeling3 = "Neen"
        Else
            lsKennisdeling3 = "    Ja"
        End If
        If (voCase.GetProperty(Of Boolean)("lbRegelgeving") = False) Then
            lsKennisdeling4 = "Neen"
        Else
            lsKennisdeling4 = "    Ja"
        End If

        lsContent = lsContent.Replace("#Samenvatting#", lsSamenvatting)
        lsContent = lsContent.Replace("#Kennisdeling2#", lsKennisdeling2)
        lsContent = lsContent.Replace("#Kennisdeling1#", lsKennisdeling1)
        lsContent = lsContent.Replace("#Kennisdeling3#", lsKennisdeling3)
        lsContent = lsContent.Replace("#Kennisdeling4#", lsKennisdeling4)
        Return lsContent

    End Function

    Protected Function VervangGrondRedenVanSchorsing(ByVal voCase As cCase, ByVal lsContent As String) As String

        ' aanpassing omtrent de redenen van schorsing, vernietiging, goedkeuring en niet goedkeuring in te geven
        Dim lsGrond As String = ""
        If voCase.GetProperty(Of String)("vernietigingsgronden") <> "" Then
            lsGrond = "Vernietigingsgronden: " & voCase.GetProperty(Of String)("vernietigingsgronden")
        Else
        End If
        If voCase.GetProperty(Of String)("schorsingsgronden") <> "" Then
            lsGrond = "Schorsingsgronden: " & voCase.GetProperty(Of String)("schorsingsgronden")
        Else
        End If
        If voCase.GetProperty(Of String)("redenen goedkeuring met ambtshalve wijzigingen") <> "" Then
            lsGrond = "Redenen: " & voCase.GetProperty(Of String)("redenen goedkeuring met ambtshalve wijzigingen")
        Else
        End If
        If voCase.GetProperty(Of String)("redenen goedkeuring met wijzigingen na advies GR") <> "" Then
            lsGrond = "Redenen: " & voCase.GetProperty(Of String)("redenen goedkeuring met wijzigingen na advies GR")
        Else
        End If
        If voCase.GetProperty(Of String)("redenen niet-goedkeuring (ambtshalve)") <> "" Then
            lsGrond = "Redenen: " & voCase.GetProperty(Of String)("redenen niet-goedkeuring (ambtshalve)")
        Else
        End If
        If voCase.GetProperty(Of String)("redenen niet-goedkeuring (na advies gemeenteraad)") <> "" Then
            lsGrond = "Redenen: " & voCase.GetProperty(Of String)("redenen niet-goedkeuring (na advies gemeenteraad)")
        Else
        End If
        lsContent = lsContent.Replace("#grond#", lsGrond)
        Dim lsMotivering As String
        If voCase.GetProperty(Of String)("lbKwalifMotiv") <> "" Then
            lsMotivering = "Motivering: " & voCase.GetProperty(Of String)("lbKwalifMotiv")
        Else
            lsMotivering = ""
        End If
        lsContent = lsContent.Replace("#motivering#", lsMotivering)
        'einde velden opvullen

        Return lsContent
    End Function

End Class
