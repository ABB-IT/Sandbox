Imports Arco.ABB.Common
Imports Arco.Doma.Library
Imports Arco.Doma.Library.Routing
''' <summary>
''' Class containing shared functions for Brief Templates
''' </summary>
''' <remarks></remarks>
Public MustInherit Class AlgemeenToezichtBrief
    Inherits RTFBriefTemplate

    Protected Class Behandelaar
        Public Property User As ACL.User
        Public Property Naam As String
        Public Property AfdelingNaam As String

        Private Sub New()

        End Sub

        Public Shared Function GetBehandelaar(ByVal voCase As cCase) As Behandelaar
            Dim loRet As Behandelaar = New Behandelaar

            Dim lsBehandelaar As String = voCase.GetProperty(Of String)("dossierbehandelaar")
            Dim lsBehandelaar2 As String = voCase.GetProperty(Of String)("dossierbehandelaar2")                       
            If lsBehandelaar2 = voCase.StepExecutor Then
                loRet.AfdelingNaam = voCase.GetProperty(Of String)("afdeling2")
                loRet.User = ACL.User.GetUser(lsBehandelaar2)
            ElseIf Not String.IsNullOrEmpty(lsBehandelaar) Then
                loRet.AfdelingNaam = voCase.GetProperty(Of String)("afdeling")
                loRet.User = ACL.User.GetUser(lsBehandelaar)
            Else
                loRet.AfdelingNaam = voCase.GetProperty(Of String)("afdeling")
                loRet.User = ACL.User.NewUser("")
            End If

            loRet.Naam = loRet.User.USER_DISPLAY_NAME
            Dim lsVoorEnAchterNaamBehandelaar As String = loRet.User.USER_FIRSTNAME & " " & loRet.User.USER_LASTNAME
            If Not String.IsNullOrWhiteSpace(lsVoorEnAchterNaamBehandelaar) Then
                loRet.Naam = lsVoorEnAchterNaamBehandelaar
            End If

            Return loRet
        End Function
    End Class

    Private Function VervangBestuur(ByVal voCase As cCase, ByVal lsContent As String) As String
        Dim lsTypeBestuur As String = voCase.GetProperty(Of String)("type bestuur")
        Dim lsBestuur As String = voCase.GetProperty(Of String)("bestuur")
        Dim lsNaamBestuur As String = voCase.GetProperty(Of String)("bestuur_naam")
        Dim lsbestuurstraatnr As String = voCase.GetProperty(Of String)("bestuur_straatnr")
        Dim lsbestuurpostnummer As String = (voCase.GetProperty(Of String)("bestuur_postnummer") & " " & voCase.GetProperty(Of String)("bestuur_gemeente")) '

        lsContent = lsContent.Replace("#bestuur#", lsTypeBestuur)
        lsContent = lsContent.Replace("#Type bestuur#", lsTypeBestuur)
        lsContent = lsContent.Replace("#Bestuur-naam#", lsNaamBestuur)
        lsContent = lsContent.Replace("#bestuur#", lsbestuur)
        lsContent = lsContent.Replace("#Naam_bestuur#", lsnaambestuur)
        lsContent = lsContent.Replace("#bestuur_straatnr#", lsbestuurstraatnr)
        lsContent = lsContent.Replace("#bestuur_postnummer#", lsbestuurpostnummer)
        Return lsContent

    End Function

    Public Overrides Function ReplaceTags(ByVal lsContent As String, ByVal voCase As Doma.Library.Routing.cCase) As String
        lsContent = lsContent.Replace("#Betreft#", voCase.GetProperty(Of String)("betreft"))
        lsContent = lsContent.Replace("#jaar# ", DateTime.Parse(voCase.CaseData.Creation_Date).Year.ToString)
        lsContent = lsContent.Replace("#S_Dossiernummer#", voCase.GetProperty(Of String)("S_Dossiernummer"))

        If VervangBestuurTags Then
            lsContent = VervangBestuur(voCase, lsContent)
        End If

        Dim loBehandelaar As Behandelaar = Behandelaar.GetBehandelaar(voCase)
        lsContent = lsContent.Replace("#Naam DBH#", loBehandelaar.Naam)
        lsContent = lsContent.Replace("#telefoonnummer DBH#", loBehandelaar.User.USER_PHONE)
        lsContent = lsContent.Replace("#Mailadres DBH#", loBehandelaar.User.USER_MAIL)

        If Me.VulPostDatumKlachtIn Then
            Dim lsPostdatumklacht As String = voCase.GetProperty(Of String)("postdatum klacht")
            If Not String.IsNullOrEmpty(lsPostdatumklacht) Then
                Dim dTemp As DateTime
                If DateTime.TryParse(lsPostdatumklacht, dTemp) Then
                    lsPostdatumklacht = dTemp.ToString("dd/MM/yyyy")
                    'lsdatumkabinet = LeadingZero(Day(CDate(lsdatumkabinet)), 2) & "/" & LeadingZero(Month(CDate(lsdatumkabinet)), 2) & "/" & Year(CDate(lsdatumkabinet))
                End If
            End If
            ''20102010
            lsContent = lsContent.Replace("#postdatum klacht#", lsPostdatumklacht)
        Else
            lsContent = lsContent.Replace("#postdatum klacht#", "")
        End If

        Arco.Utils.Logging.Log("loBehandelaar.AfdelingNaam = " & loBehandelaar.AfdelingNaam, "d:\arco\logging\algemeentoezicht.log")

        Dim loAfdeling As Afdeling = Afdeling.GetAfdeling(loBehandelaar.AfdelingNaam)
        lsContent = lsContent.Replace("#BB_AFDELING(NAAM)#", loAfdeling.Naam)
        lsContent = lsContent.Replace("#BB_AFDELING(STRAATNR)#", loAfdeling.StraatNr)
        lsContent = lsContent.Replace("#BB_AFDELING(GEMEENTE)#", loAfdeling.Gemeente)
        lsContent = lsContent.Replace("#BB_AFDELING(POSTCODE)#", loAfdeling.PostCode)
        lsContent = lsContent.Replace("#BB_AFDELING(TEL)#", loAfdeling.Telefoon)
        lsContent = lsContent.Replace("#BB_AFDELING(FAX)#", loAfdeling.Fax)
        lsContent = lsContent.Replace("#BB_AFDELING(EMAIL)#", loAfdeling.Email)
        Dim lsSlotformule As String = "Hoogachtend,"

        If loAfdeling.CentraleAfdeling = True Then
            loAfdeling.Naam = "Centrale afdeling: ondertekening door de minister"
            loAfdeling.AanspreekTitel = "Viceminister-president van de Vlaamse Regering en Vlaams minister van          Bestuurszaken, Binnenlands Bestuur, Inburgering, Toerisme en Vlaamse Rand"
            '  loAfdeling.Naam = " "
        Else
            'lsAfdeling="tekstveld met ondertekening door de gouverneur"
            loAfdeling.AanspreekTitel = "Gouverneur"

            'lsNaam =trim(Replace(lsNaam ,"Afdeling"," "))
            If Not String.IsNullOrEmpty(loAfdeling.Naam) Then loAfdeling.Naam = loAfdeling.Naam.Replace("Afdeling", "").Trim
        End If

        lsContent = lsContent.Replace("#Slotformule#", lsSlotformule)
        lsContent = lsContent.Replace("#AFDELING#", loAfdeling.Naam)
        lsContent = lsContent.Replace("#NAAM_GOUVERNEUR#", loAfdeling.NaamGouverneur)
        lsContent = lsContent.Replace("#AANSPREEKTITEL#", loAfdeling.AanspreekTitel)

        Return lsContent
    End Function

    Protected Property VulPostDatumKlachtIn As Boolean

    Private mBestuur As Boolean = True
    Protected Property VervangBestuurTags As Boolean
        Get
            Return mBestuur
        End Get
        Set(value As Boolean)
            mBestuur = value
        End Set
    End Property

    Public MustOverride Overrides ReadOnly Property TemplateFile As String

End Class
