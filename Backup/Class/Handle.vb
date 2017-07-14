Imports System.Data.SqlClient

Public Class DataHandler
    Public Shared Function GetDataReader(ByVal SQLStatement As String, ByVal ConnectionString As String) As SqlDataReader
        Dim dr As SqlDataReader
        Dim cmd As New SqlCommand
        With cmd
            ' Create a Connection object
            .Connection = New SqlConnection(ConnectionString)
            .Connection.Open()
            .CommandText = SQLStatement
            dr = .ExecuteReader(CommandBehavior.CloseConnection)
        End With
        Return dr
    End Function

    Public Shared Function QuoteString(ByVal Value As String) As String
        ' Replace the single quote(') to dpuble quote('')
        Return String.Format("'{0}'", Value.Replace("'", "''"))
    End Function

    Public Shared Sub SelectItemComboBox(ByVal cbo As ComboBox, ByVal strValue As String)
        Dim intI As Integer
        Dim blnFound As Boolean
        Dim li As MyListItem
        li = New MyListItem
        For intI = 0 To cbo.Items.Count - 1
            li = CType(cbo.Items(intI), MyListItem)
            If li.Value = strValue Then
                cbo.SelectedIndex = intI
                blnFound = True
                Exit For
            End If
        Next
        If Not blnFound Then
            cbo.SelectedIndex = -1
        End If
    End Sub

    Public Shared Sub RunSQLtoDGD(ByVal cSQL As String, ByVal DataGrid As DataGridView, ByVal ParamArray prmOleDb As IDataParameter())
        Dim ds As New DataSet
        Dim connOledb As New SqlConnection(Conn)
        Try
            Dim cmd As New SqlCommand(cSQL, connOledb)
            If prmOleDb IsNot Nothing Then
                For Each prmOle As SqlParameter In prmOleDb
                    If prmOle IsNot Nothing Then cmd.Parameters.Add(prmOle)
                Next
            End If
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(ds)
            DataGrid.DataSource = ds.Tables(0)
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            If connOledb.State = ConnectionState.Open Then connOledb.Close()
        End Try

    End Sub

    Public Shared Sub RunQuery(ByVal cSQL)

        Dim objConn As SqlConnection
        Dim cmd As SqlCommand

        objConn = New SqlConnection(Conn)
        cmd = New SqlCommand
        cmd.Connection = objConn
        objConn.Open()
        cmd.CommandText = cSQL
        cmd.ExecuteNonQuery()
        objConn.Close()

    End Sub

    Public Shared Sub AutoCompleteCombo_Leave(ByVal cbo As ComboBox)
        Dim iFoundIndex As Integer
        iFoundIndex = cbo.FindStringExact(cbo.Text)
        cbo.SelectedIndex = iFoundIndex
    End Sub

    Public Shared Function GetSelectedDGDRow(ByVal dgd As DataGridView)
        Return dgd.SelectedRows.Item(0).Index
    End Function

    Public Shared Sub SelectDGDRow(ByVal dgd As DataGridView, ByVal RowIndex As Integer)
        dgd.Item(0, RowIndex).Selected = True
    End Sub
End Class
