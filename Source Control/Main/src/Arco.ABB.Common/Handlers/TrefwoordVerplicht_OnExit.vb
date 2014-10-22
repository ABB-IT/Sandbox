' **************************************************************************
' Project naming convention : Controle Trefwoord verplicht Adviesvragen
' **************************************************************************
' Author : Geoffrey
' Created by         on   
' Modified by        on 
' Description :
' **************************************************************************
Imports Arco.Doma.Library.baseObjects
Imports Arco.Doma.Library.Routing
Imports Arco.Utils.Logging
Imports Arco.Doma.Library


<Serializable()> _
Public Class TrefwoordVerplicht_OnExit
    Inherits ABBEventHandler


    Public Overrides Sub ExecuteCode(WFCurrentCase As Doma.Library.Routing.cCase)
        ' Trefwoord verplicht
        Dim lobjTable As Arco.Doma.Library.baseObjects.DM_OBJECT.Table = DirectCast(WFCurrentCase.GetProperty("trefwoordenlijst"), Arco.Doma.Library.baseObjects.DM_OBJECT.Table)
        If lobjTable.Rows.Count <= 1 Then
            WFCurrentCase.RejectComment = "Vul tenminste 1 trefwoord in!!!  "
            WFCurrentCase.RejectUser = "Routing"
        End If
        'SDP : never call save in a handler!
        '  WFCurrentCase.Save()
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "TrefwoordVerplicht_OnExit"
        End Get
    End Property
End Class

