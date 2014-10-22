Imports Arco.ABB.Common
Imports Arco.Doma.Library

Public Class NotaAanMinister
    Inherits AlgemeenToezichtsNota
  

    Public Overrides ReadOnly Property TemplateFile As String
        Get
            Return "Nota_aan_minister.rtf"
        End Get
    End Property
End Class
