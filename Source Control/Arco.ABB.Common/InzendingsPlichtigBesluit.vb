Public Class InzendingsPlichtigBesluit
    Inherits Arco.Business.BusinessBase

#Region " Properties "
    Private mDatumbeSluit As String = ""
    Private mSoortBesluit As String = ""
    Private mPostDatum As String = ""
    Private mDatumIn As String = ""
    Private mInitieleTermijn As String = ""
    Private mKorteOmschrijving As String = ""
    Private mBoekJaar As String = ""
    Private mHoeveelste As String = ""
    Private mType As String = ""
    Private mBestuur As String = ""
    Private mGemeente As String = ""
    Private mOpmerking As String = ""
    Private mIDBestuur As String = ""
    Private mCaseID As Integer

    Public Property FTCid As Int32

    Public Property Case_ID As Integer
        Get
            Return mCaseID
        End Get
        Set(ByVal value As Integer)
            If mCaseID <> value Then
                mCaseID = value
                MarkDirty()
            End If
        End Set
    End Property
    Public Property IDBestuur As String
        Get
            Return mIDBestuur
        End Get
        Set(ByVal value As String)
            If mIDBestuur <> value Then
                mIDBestuur = value
                MarkDirty()
            End If
        End Set
    End Property
    Public Property Opmerking As String
        Get
            Return mOpmerking
        End Get
        Set(ByVal value As String)
            If mOpmerking <> value Then
                mOpmerking = Arco.Server.DataProvider.DBSubstring(value, 100)
                MarkDirty()
            End If
        End Set
    End Property
    Public Property Bestuur As String
        Get
            Return mBestuur
        End Get
        Set(ByVal value As String)
            If mBestuur <> value Then
                mBestuur = value
                MarkDirty()
            End If
        End Set
    End Property
    Public Property Gemeente As String
        Get
            Return mGemeente
        End Get
        Set(ByVal value As String)
            If mGemeente <> value Then
                mGemeente = value
                MarkDirty()
            End If
        End Set
    End Property
    Public Property Type As String
        Get
            Return mType
        End Get
        Set(ByVal value As String)
            If mType <> value Then
                mType = value
                MarkDirty()
            End If
        End Set
    End Property
    Public Property DatumBesluit As String
        Get
            Return mDatumbeSluit
        End Get
        Set(ByVal value As String)
            If mDatumbeSluit <> value Then
                mDatumbeSluit = value
                MarkDirty()
            End If
        End Set
    End Property
    Public Property SoortBesluit As String
        Get
            Return mSoortBesluit
        End Get
        Set(ByVal value As String)
            If mSoortBesluit <> value Then
                mSoortBesluit = value
                MarkDirty()
            End If
        End Set
    End Property
    Public Property PostDatum As String
        Get
            Return mPostDatum
        End Get
        Set(ByVal value As String)
            If mPostDatum <> value Then
                mPostDatum = value
                MarkDirty()
            End If
        End Set
    End Property
    Public Property DatumIn As String
        Get
            Return mDatumIn
        End Get
        Set(ByVal value As String)
            If mDatumIn <> value Then
                mDatumIn = value
                MarkDirty()
            End If
        End Set
    End Property
    Public Property InitieleTermijn As String
        Get
            Return mInitieleTermijn
        End Get
        Set(ByVal value As String)
            If mInitieleTermijn <> value Then
                mInitieleTermijn = value
                MarkDirty()
            End If
        End Set
    End Property
    Public Property KorteOmschrijving As String
        Get
            Return mKorteOmschrijving
        End Get
        Set(ByVal value As String)
            If mKorteOmschrijving <> value Then
                mKorteOmschrijving = Arco.Server.DataProvider.DBSubstring(value, 2000)
                MarkDirty()
            End If
        End Set
    End Property
    Public Property BoekJaar As String
        Get
            Return mBoekJaar
        End Get
        Set(ByVal value As String)
            If mBoekJaar <> value Then
                mBoekJaar = value
                MarkDirty()
            End If
        End Set
    End Property
    Public Property Hoeveelste As String
        Get
            Return mHoeveelste
        End Get
        Set(ByVal value As String)
            If mHoeveelste <> value Then
                mHoeveelste = value
                MarkDirty()
            End If
        End Set
    End Property
#End Region

    <Serializable()> _
    Public Class Criteria
        Public FTCID As Int32
        Friend Sub New()
        End Sub
        Friend Sub New(ByVal vlFTCID As Int32)
            FTCID = vlFTCID
        End Sub
    End Class
    Private Sub New()
    End Sub

#Region " Dataportal Handlers "

    <Attributes.RunLocal()> _
    Protected Overrides Sub DataPortal_Create(ByVal Criteria As Object)
    End Sub

    Protected Overrides Sub DataPortal_Delete(ByVal Criteria As Object)
        'todo
    End Sub

    Protected Overrides Sub DataPortal_Update()

        If Me.IsDeleted Then
            If Not Me.IsNew Then
            Else
                MarkNew()
            End If
        Else
            Using loQuery As Server.DataQuery = New Server.DataQuery
                If Me.IsNew Then
                    ' Insert.
                    ' COND_ID,PROP_ID,COND_OPERATOR,COND_VALUE,COND_NOT,TECH_ID,COND_POSX,COND_POSY,GUID,FFD

                    loQuery.StartInsert("BB_INZEND_BESL", "TYPE ,BESTUUR ,GEMEENTE, DATUM_BESLUIT , POST_DATUM, DATUM_IN, INITIELE_TERMIJN,SOORT_BESLUIT,OPMERKINGSVELD,ID_BESTUUR,KORTEOMSCHRIJVING,BOEKJAAR,HOEVEELSTE")
                    loQuery.AddStringInsert(mType)
                    loQuery.AddStringInsert(mBestuur)
                    loQuery.AddStringInsert(mGemeente)
                    loQuery.AddDateTimeInsert(mDatumbeSluit)
                    loQuery.AddDateTimeInsert(mPostDatum)
                    loQuery.AddDateTimeInsert(mDatumIn)
                    loQuery.AddDateTimeInsert(mInitieleTermijn)
                    loQuery.AddStringInsert(mSoortBesluit)
                    loQuery.AddStringInsert(mOpmerking)
                    loQuery.AddStringInsert(mIDBestuur)
                    loQuery.AddStringInsert(mKorteOmschrijving)
                    loQuery.AddStringInsert(mBoekJaar)
                    loQuery.AddStringInsert(mHoeveelste)
                    loQuery.CloseInsert()
                    loQuery.Connect()
                    loQuery.ExecuteNonQuery()

                    loQuery.Query = "Select max(ft_cid) as max from bb_inzend_besl"
                    FTCid = loQuery.ExecuteScalar()
                Else
                    ' Update.
                    'todo if needed
                    loQuery.StartUpdate("BB_INZEND_BESL")
                    loQuery.AddNumUpdate("CASE_ID", mCaseID)
                    loQuery.AddNumWhere("ft_Cid", FTCid)
                    loQuery.Connect()
                    loQuery.ExecuteNonQuery()
                End If
            End Using
            MarkOld()
        End If
    End Sub

    Protected Overrides Sub DataPortal_Fetch(ByVal Criteria As Object)

        ' Retrieve the data from the database.

        Dim loCrit As Criteria = CType(Criteria, Criteria)
        Dim lsSQL As String = "select datum_besluit, soort_besluit,post_datum ,datum_in, initiele_termijn, korteomschrijving,boekjaar,hoeveelste  from BB_inzend_besl where ft_Cid=" & loCrit.FTCID

        Using loQuery As Arco.Server.DataQuery = New Arco.Server.DataQuery
            loQuery.Query = lsSQL
            Try
                loQuery.Connect()
                Using loReader As Server.SafeDataReader = loQuery.ExecuteReader
                    While loReader.Read
                        mDatumbeSluit = loReader.GetString(0)
                        mSoortBesluit = loReader.GetString(1)
                        mPostDatum = loReader.GetString(2)
                        mDatumIn = loReader.GetString(3)
                        mInitieleTermijn = loReader.GetString(4)
                        mKorteOmschrijving = loReader.GetString(5)
                        mBoekJaar = loReader.GetString(6)
                        mHoeveelste = loReader.GetString(7)
                        FTCid = loCrit.FTCID
                    End While
                End Using
            Catch ex As Exception
                Arco.Utils.Logging.Log("error InzendingsPlichtigBesluit:" & ex.Message)
            End Try
        End Using
    End Sub

#End Region
    Public Shared Function NewInzendingsPlichtigBesluit() As InzendingsPlichtigBesluit
        Return CType(Client.DataPortal.Create(New Criteria), InzendingsPlichtigBesluit)
    End Function
    Public Shared Function GetInzendingsPlichtigBesluit(ByVal ft_cid As Int32) As InzendingsPlichtigBesluit
        Return CType(Client.DataPortal.Fetch(New Criteria(ft_cid)), InzendingsPlichtigBesluit)
    End Function
    Public Overloads Function Save() As InzendingsPlichtigBesluit
        Return CType(MyBase.Save, InzendingsPlichtigBesluit)
    End Function
End Class

