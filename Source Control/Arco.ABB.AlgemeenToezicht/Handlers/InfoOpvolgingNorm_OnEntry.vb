Public Class InfoOpvolgingNorm_OnEntry
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)

        Dim lcVerschil As Long

        Dim lsHuidigeTermijn As String = WFCurrentCase.GetProperty(Of String)("huidige termijn")
        If (String.IsNullOrEmpty(lsHuidigeTermijn) OrElse lsHuidigeTermijn = "geen termijn") Then
            lcVerschil = 7
        Else
            Dim dtTemp As DateTime
            If DateTime.TryParse(lsHuidigeTermijn, dtTemp) Then
                lcVerschil = DateAndTime.DateDiff(DateInterval.Day, System.DateTime.Now, dtTemp)
            Else
                lcVerschil = 7
            End If	
        End If
        'msgbox " geef verschil " & lcVerschil 

        If lcVerschil < 7 Then
            WFCurrentCase.SetPropertyVisible("lbHTMLWaarschuwing7dagen", True)
            WFCurrentCase.SetPropertyVisible("lbDatumContactKab", True)
            WFCurrentCase.SetPropertyVisible("lbMediumContactKab", True)

        Else
            WFCurrentCase.SetPropertyVisible("lbHTMLWaarschuwing7dagen", False)
            WFCurrentCase.SetPropertyVisible("lbDatumContactKab", False)
            WFCurrentCase.SetPropertyVisible("lbMediumContactKab", False)

        End If

    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "InfoOpvolgingNorm_OnEntry"
        End Get
    End Property
End Class
