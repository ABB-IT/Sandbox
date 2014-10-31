
Option Explicit On
Option Strict On

Imports Arco.Doma.Library
Imports Arco.Doma.Library.Routing
Imports Arco.ABB.Common

Public Class StepXX_ScreenHandler
    Public Class StepXX_ScreenHandler
        Inherits Website.CaseScreenHandler

        Private Const SCRIPT_NAME As String = "Step XX-ScreenHandler"

        Public Overloads Overrides Sub onBeforeRender(ByRef roScreenItems As Arco.Doma.Library.Website.ScreenItemList, ByVal voScreenMode As Arco.Doma.Library.Website.Screen.DetailScreenDisplayMode, ByVal WFCurrentCase As Arco.Doma.Library.Routing.cCase)

            Dim id As Integer
            'todo : move to onKeep!!
            Try
                id = roScreenItems.GetFieldIndexByIdentifier("StepDueDate")
                If WFCurrentCase.Step_DueDate = "" Then
                    roScreenItems(id).Mode = Doma.Library.Website.ScreenItem.ItemMode.Hidden
                    WFCurrentCase.ShowProperty("HTML geen deadline")
                Else
                    roScreenItems(id).Mode = Doma.Library.Website.ScreenItem.ItemMode.ReadOnly
                    WFCurrentCase.HideProperty("HTML geen deadline")
                End If

            Catch ex As Exception
            Finally
            End Try


            Try
                'toon Kies Dossierbehandelaar 
                Select Case WFCurrentCase.CurrentStep.Step_Name
                    Case "Onderzoek en voorstel - afdeling 1", "Onderzoek en voorstel - afdeling 2", "Opvraging", "Wachten op ontvangst antwoord"
                        Dim liKnop As Integer = roScreenItems.GetFieldIndexByIdentifier("Kies Dossierbehandelaar")
                        If liKnop > 0 Then
                            If WFCurrentCase.GetProperty(Of String)("doorsturen dossier") <> "Ja" Then
                                roScreenItems.Item(liKnop).Mode = Website.ScreenItem.ItemMode.Hidden
                                Logging.AddToLog(WFCurrentCase, "item set to hidden")
                            End If
                        End If
                    Case Else
                End Select
            Catch ex As Exception
            Finally
            End Try

            Try
                ' Toon Kies Goedkeurder
                Select Case WFCurrentCase.CurrentStep.Step_Name
                    Case "Goedkeuring - afdeling 1", "Na rechtvaardiging: goedkeuring - Afdeling 1"
                        Dim liknop As Integer = roScreenItems.GetFieldIndexByIdentifier("Kies Goedkeurder")
                        If liknop > 0 Then
                            Logging.AddToLog(WFCurrentCase, "Laatste goedkeurder? = " & WFCurrentCase.GetProperty(Of String)("Laatste goedkeurder?"))
                            If WFCurrentCase.GetProperty(Of String)("Laatste goedkeurder?") <> "ja (kies goedkeurder)" Then
                                roScreenItems.Item(liknop).Mode = Website.ScreenItem.ItemMode.Hidden
                                Logging.AddToLog(WFCurrentCase, "item set to hidden")
                            End If
                        End If
                    Case "Goedkeuring - afdeling 2", "Na Kennisgeving: goedkeuring", "Na rechtvaardiging: goedkeuring - afdeling 2"
                        Dim liknop As Integer = roScreenItems.GetFieldIndexByIdentifier("Kies Goedkeurder")
                        If liknop > 0 Then
                            Logging.AddToLog(WFCurrentCase, "Laatste goedkeurder2? = " & WFCurrentCase.GetProperty(Of String)("Laatste goedkeurder2?"))
                            If WFCurrentCase.GetProperty(Of String)("Laatste goedkeurder2?") <> "ja" Then
                                roScreenItems.Item(liknop).Mode = Website.ScreenItem.ItemMode.Hidden
                                Logging.AddToLog(WFCurrentCase, "item set to hidden")
                            End If
                        End If
                    Case Else
                End Select
            Catch ex As Exception
            Finally
            End Try
        End Sub
    End Class

End Class
