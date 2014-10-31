Imports Arco.Doma.Library.Routing

Public Class SetTermijnen
    Inherits AlgemeenToezichtEventHandler

    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        'todo : check where included


        '************************
        ' VARIABELEN
        '************************
       
        '********************************************
        'DECLARATIE VARIABELEN
        '********************************************

        Dim loBereken As TermijnBerekening = New TermijnBerekening
        loBereken.ZetTermijnen(WFCurrentCase)


        '@Include = "(00) Info voor opvolging Norm - on entry"
        Dim infoscript As InfoOpvolgingNorm_OnEntry = New InfoOpvolgingNorm_OnEntry
        infoscript.ExecuteCode(WFCurrentCase)

    End Sub


    Public Overrides ReadOnly Property Name As String
        Get
            Return "SetTermijnen"
        End Get
    End Property
End Class
