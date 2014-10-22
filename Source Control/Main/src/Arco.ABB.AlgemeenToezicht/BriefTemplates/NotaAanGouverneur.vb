Imports Arco.ABB.Common
Imports Arco.Doma.Library
Public Class NotaAanGouverneur
    Inherits AlgemeenToezichtsNota
   


    Public Overrides ReadOnly Property TemplateFile As String
        Get
            Return "Nota_aan_gouverneur.rtf"
        End Get
    End Property
End Class
