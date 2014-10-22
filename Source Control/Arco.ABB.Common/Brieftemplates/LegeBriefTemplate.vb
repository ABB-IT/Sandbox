
<Serializable()> _
Public Class LegeBriefTemplate
    Inherits RTFBriefTemplate

    Public Overrides Function ReplaceTags(vsContent As String, voCase As Doma.Library.Routing.cCase) As String
        'todo : not used and lsBehandelaar2 not set... fix or leave

        'lsbestuur = WFGetProperty("bestuur")

        'lsnaambestuur = WFGetProperty("bestuur_naam")


        'lsbestuurstraatnr = WFGetProperty("bestuur_straatnr")

        'lsbestuurpostnummer = (WFGetProperty("bestuur_postnummer") & " " & WFGetProperty("bestuur_naam"))

        'lscontactpersoon = WFGetProperty("contactpersoon_naam")

        'lsAfdelingnaam = WFGetProperty("afdeling")

        ''lsPostdatumklacht= (Date) Heidi de postdatum zelf kunnen invullen omdat de brieven in de praktijk later worden verstuurs

        'lsPostdatumklacht = Day(CDate(lsPostdatumklacht)) & "/" & Month(CDate(lsPostdatumklacht)) & "/ " & Year(CDate(lsPostdatumklacht))
        'lsPostdatumklacht = " "
        'If lsBehandelaar2 = lsStepexecutor Then
        '    lsAfdelingnaam = WFGetProperty("afdeling2")
        '    lsNaamDBH = fsUserData(WFGetProperty("dossierbehandelaar2"), "USER_DISPLAY_NAME")
        '    lsVNaamDBH = fsUserData(WFGetProperty("dossierbehandelaar2"), "USER_FIRSTNAME")
        '    lsFNaamDBH = fsUserData(WFGetProperty("dossierbehandelaar2"), "USER_LASTNAME")


        '    lsMailDBH = fsUserData(WFGetProperty("dossierbehandelaar2"), "USER_MAIL")

        '    lstelefoonummerDBH = fsUserData(WFGetProperty("dossierbehandelaar2"), "USER_PHONE")


        'Else
        '    lsAfdelingnaam = WFGetProperty("afdeling")

        '    lsNaamDBH = fsUserData(WFGetProperty("dossierbehandelaar"), "USER_DISPLAY_NAME")

        '    lsMailDBH = fsUserData(WFGetProperty("dossierbehandelaar"), "USER_MAIL")
        '    lstelefoonummerDBH = fsUserData(WFGetProperty("dossierbehandelaar"), "USER_PHONE")
        '    lsVNaamDBH = fsUserData(WFGetProperty("dossierbehandelaar"), "USER_FIRSTNAME")
        '    lsFNaamDBH = fsUserData(WFGetProperty("dossierbehandelaar"), "USER_LASTNAME")

        'End If
        'lsVNaamDBH = lsVNaamDBH & " " & lsFNaamDBH

        'If lsVNaamDBH <> " " Then
        '    lsNaamDBH = lsVNaamDBH
        'Else
        'End If

        'lsbetreft = WFGetProperty("betreft")
        ''lsjaar="2010"
        'lsjaar = lsYear

        'lsDossiernummer = WFGetProperty("S_Dossiernummer")

        'lsContent = Replace(lsContent, "#postdatum klacht#", lsPostdatumklacht)
        'lsContent = Replace(lsContent, "#Naam DBH#", lsNaamDBH)
        'lsContent = Replace(lsContent, "#telefoonnummer DBH#", lstelefoonummerDBH)
        'lsContent = Replace(lsContent, "#Mailadres DBH#", lsMailDBH)
        'lsContent = Replace(lsContent, "#Betreft#", lsBetreft)
        'lsContent = Replace(lsContent, "#jaar# ", lsjaar)
        'lsContent = Replace(lsContent, "#S_Dossiernummer#", lsDossiernummer)
        'lsContent = Replace(lsContent, "#bestuur#", lsbestuur)
        'lsContent = Replace(lsContent, "#Naam_bestuur#", lsnaambestuur)
        'lsContent = Replace(lsContent, "#bestuur_straatnr#", lsbestuurstraatnr)
        'lsContent = Replace(lsContent, "#bestuur_postnummer#", lsbestuurpostnummer)
        ''	lsContent = Replace(lsContent,"#klager_woonplaats#",lsklagerwoonplaats)
        'Dim lsAfdeling As String
        'If lsBehandelaar2 = lsStepexecutor Then
        '    lsAfdeling = voCase.GetProperty(Of String)("afdeling2")
        'Else
        '    lsAfdeling = voCase.GetProperty(Of String)("afdeling")
        'End If
        'Dim loAfdeling As Afdeling = Afdeling.GetAfdeling(lsAfdeling)

        'lsContent = Replace(lsContent, "#BB_AFDELING(NAAM)#", loAfdeling.Naam)
        'lsContent = Replace(lsContent, "#BB_AFDELING(STRAATNR)#", loAfdeling.StraatNr)
        'lsContent = Replace(lsContent, "#BB_AFDELING(GEMEENTE)#", loAfdeling.Gemeente)
        'lsContent = Replace(lsContent, "#BB_AFDELING(POSTCODE)#", loAfdeling.PostCode)
        'lsContent = Replace(lsContent, "#BB_AFDELING(TEL)#", loAfdeling.Telefoon)
        'lsContent = Replace(lsContent, "#BB_AFDELING(FAX)#", loAfdeling.Fax)
        'lsContent = Replace(lsContent, "#BB_AFDELING(EMAIL)#", loAfdeling.Email)

        'If loAfdeling.CentraleAfdeling Then
        '    lsAfdeling = "Centrale afdeling: ondertekening door de minister"
        '    lsaanspreektitel = "Viceminister-president van de Vlaamse Regering en Vlaams minister van           Bestuurszaken, Binnenlands Bestuur, Inburgering, Toerisme en Vlaamse Rand"

        'Else
        '    'lsAfdeling="tekstveld met ondertekening door de gouverneur"
        '    lsaanspreektitel = "Gouverneur"
        'End If


        'lsContent = Replace(lsContent, "#AFDELING#", lsAfdeling)
        'lsContent = Replace(lsContent, "#NAAM_GOUVERNEUR#", lsnaamgouveneur)
        'lsContent = Replace(lsContent, "#AANSPREEKTITEL#", lsaanspreektitel)


    End Function

    Public Overrides ReadOnly Property TemplateFile As String
        Get
            Return "Legebrief.rtf"
        End Get
    End Property
End Class
