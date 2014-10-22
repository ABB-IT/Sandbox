
<Serializable()> _
Public Class Afdeling
    Public Property Naam As String
    Public Property StraatNr As String
    Public Property PostCode As String
    Public Property Gemeente As String
    Public Property Email As String
    Public Property Telefoon As String
    Public Property Fax As String
    Public Property NaamAfdelingshoofd As String
    Public Property AanspreekTitel As String
    Public Property NaamGouverneur As String
    Public Property CentraleAfdeling As Boolean

    Private Sub SetExtraData()
        Select Case Me.Naam
            Case "Afdeling Antwerpen"
                NaamAfdelingshoofd = "Guy Peeters"
                AanspreekTitel = "Mijnheer"
                NaamGouverneur = "Cathy Berx"
                CentraleAfdeling = False
            Case "Afdeling Financiën en Personeel"
                NaamAfdelingshoofd = "Johan Ide"
                AanspreekTitel = "Mijnheer"
                NaamGouverneur = "Geert Bourgeois"
                CentraleAfdeling = True
            Case "Afdeling Beleid Binnenland Steden en Inburgering"
                NaamAfdelingshoofd = "Sami Souguir"
                AanspreekTitel = "Algemeen directeur"
                NaamGouverneur = "Geert Bourgeois"
                CentraleAfdeling = True
            Case "Afdeling Limburg"
                NaamAfdelingshoofd = "Sandra Beckers"
                AanspreekTitel = "Mevrouw"
                NaamGouverneur = "Herman Reynders"
                CentraleAfdeling = False
            Case "Afdeling Oost-Vlaanderen"
                'NaamAfdelingshoofd = "Riet Zegers"
                NaamAfdelingshoofd = "Vicky Van den Berge"
                AanspreekTitel = "Mevrouw"
                'NaamGouverneur = "André Denys"
                NaamGouverneur = "Jan Briers"
                CentraleAfdeling = False
            Case "Afdeling Organisatie en Beheer"
                NaamAfdelingshoofd = "Piet Van Der Plas"
                AanspreekTitel = "Mijnheer"
                NaamGouverneur = "Geert Bourgeois"
                CentraleAfdeling = True
            Case "Afdeling Regelgeving en Werking"
                NaamAfdelingshoofd = "Rudy Janssens" 'todo : bugfixed, here was naamgouverneur
                AanspreekTitel = "Mijnheer"
                NaamGouverneur = "Geert Bourgeois"
                CentraleAfdeling = True
            Case "Afdeling Vlaams-Brabant"
                NaamAfdelingshoofd = "Nicole Pijpops"
                AanspreekTitel = "Mevrouw"
                NaamGouverneur = "Lodewijk De Witte"
                CentraleAfdeling = False
            Case "Afdeling West-Vlaanderen"
                NaamAfdelingshoofd = "Bruno Vanmarcke"
                AanspreekTitel = "Mijnheer"
                NaamGouverneur = "Paul Breyne"
                CentraleAfdeling = False
        End Select
    End Sub

    Private Sub New()

    End Sub
    Public Shared Function GetAfdeling(ByVal vsNaam As String) As Afdeling
        Dim loAfdeling As Afdeling = New Afdeling
        If Not String.IsNullOrEmpty(vsNaam) Then
            vsNaam = vsNaam.Replace("(Role)", "").Trim
            'why like?
            Dim lsSQL As String = "SELECT naam,straatnr,postcode, gemeente, emailadres, telefoonnr, fax FROM bb_afdeling where upper(naam) like upper('%" & vsNaam & "%')"
            'Dim lsSQL As String = "SELECT naam,straatnr,postcode, gemeente, emailadres, telefoonnr, fax FROM bb_afdeling where naam ='" & vsNaam & "'"
            Using loQuery As Arco.Server.DataQuery = New Arco.Server.DataQuery
                loQuery.Query = lsSQL
                Try
                    loQuery.Connect()
                    Using loReader As Server.SafeDataReader = loQuery.ExecuteReader
                        If loReader.Read Then
                            loAfdeling.Naam = loReader.GetString(0)
                            loAfdeling.StraatNr = loReader.GetString(1)
                            loAfdeling.PostCode = loReader.GetString(2)
                            loAfdeling.Gemeente = loReader.GetString(3)
                            loAfdeling.Email = loReader.GetString(4)
                            loAfdeling.Telefoon = loReader.GetString(5)
                            loAfdeling.Fax = loReader.GetString(6)
                            loAfdeling.SetExtraData()
                        End If
                    End Using
                Catch ex As Exception
                    Arco.Utils.Logging.LogError("error GetAfdeling:", ex)
                End Try
            End Using
        End If

        Return loAfdeling
    End Function
End Class

