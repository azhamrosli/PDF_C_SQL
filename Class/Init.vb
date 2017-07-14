Imports System.IO
Imports Microsoft.Win32

Module Init
    Private strConn As String
    Private strConnCA As String
    Private strRefNum As String
    Private strYA As String
    Private strServer As String
    Private strUserName As String
    Private strPassword As String

    Public Class PublicFunc

        Public Shared Sub InitVar()

            Dim strFullPath As String
            Dim strContents As String
            Dim objReader As StreamReader
            Dim strServerName As String = ""
            Dim strDBName As String = ""
            Dim strUser As String = ""
            Dim strPwd As String = ""

            Try
                Dim dic As Dictionary(Of String, String) = Init.PublicFunc.GetServerInfo
                strFullPath = Application.StartupPath & "..\TaxcomC.ini" 'after make exe run this
                'strFullPath = "..\..\TaxcomC++.ini" 'before make exe can run test
                objReader = New StreamReader(strFullPath)
                strContents = objReader.ReadToEnd()
                objReader.Close()
                'strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strContents
                'NGOHCS CA2008 
                'strConnCA = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strContents.Substring(0, strContents.LastIndexOf("\") + 1) + "TAX_CA_C.mdb"

                If dic("UserName") <> "" And dic("Password") <> "" Then
                    strConn = "Server=" & dic("Server") & ";Database=" & strContents & ";User Id=" & dic("UserName") & ";Password=" & dic("Password") & ";MultipleActiveResultSets=True;"
                    strConnCA = "Server=" & dic("Server") & ";Database=TAX_CA_C;User Id=" & dic("UserName") & ";Password=" & dic("Password") & ";MultipleActiveResultSets=True;"
                Else
                    strConn = "Server=" & dic("Server") & ";Database=" & strContents & ";User Id=" & dic("UserName") & ";Password=" & dic("Password") & ";MultipleActiveResultSets=True;"
                    strConnCA = "Server=" & dic("Server") & ";Database=TAX_CA_C;Integrated Security=true;MultipleActiveResultSets=True;"
                End If

                'NGOHCS CA2008 END
            Catch exp As Exception
                MsgBox(exp.Message, MsgBoxStyle.Critical)
                End
            End Try
            LoadDefaultAccount()
        End Sub

        Public Shared Function GetServerInfo() As Dictionary(Of String, String)
            Dim dic As New Dictionary(Of String, String)
            dic.Add("Server", Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\TAXOFFICE\", "value1", ""))
            dic.Add("UserName", Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\TAXOFFICE\", "value2", ""))
            dic.Add("Password", Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\TAXOFFICE\", "value3", ""))
            Return dic
        End Function

        ' === csNgoh C2008.5 === '
        Public Shared Sub LoadDefaultAccount()

            Dim objReader As StreamReader
            Dim strFullPath As String = ""
            Dim strContents As String = ""

            Try
                strFullPath = Application.StartupPath & "..\DefaultAcc.ini"
                objReader = New StreamReader(strFullPath)
                strContents = objReader.ReadLine()
                objReader.Close()
                strRefNum = _Split(strContents, ",", "LEFT")
                strYA = _Split(strContents, ",", "RIGHT")

            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical)
            End Try

        End Sub

        Private Shared Function _Split(ByVal _String1 As String, ByVal _String2 As String, ByVal SplitMode As String) As String
            Dim strResult As String = ""

            Try
                If _String1 = "" Then
                    strResult = ""
                ElseIf _String2 = "" Then
                    strResult = _String1
                Else
                    If SplitMode = "LEFT" Then
                        strResult = Left(_String1, InStr(_String1, _String2) - 1)
                    ElseIf SplitMode = "RIGHT" Then
                        strResult = Right(_String1, Len(_String1) - (InStr(_String1, _String2)))
                    End If
                End If
                Return strResult

            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical)
                Return ""
            End Try
        End Function
        ' === end csNgoh C2008.5 === '
    End Class
    Public Property Conn() As String
        Get
            Return strConn
        End Get
        Set(ByVal Value As String)
            strConn = Value
        End Set
    End Property
    'NGOHCS CA2008
    Public Property ConnCA() As String
        Get
            Return strConnCA
        End Get
        Set(ByVal Value As String)
            strConnCA = Value
        End Set
    End Property
    'NGOHCS CA2008
    ' === csNgoh C2008.5 === '
    Public Property _strRefNum() As String
        Get
            Return strRefNum
        End Get
        Set(ByVal value As String)
            strRefNum = value
        End Set
    End Property

    Public Property _strYA() As String
        Get
            Return strYA
        End Get
        Set(ByVal value As String)
            strYA = value
        End Set
    End Property
    ' === end csNgoh C2008.5 === '
End Module
