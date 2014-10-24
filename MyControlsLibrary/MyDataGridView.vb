
Public Class MyDataGridView
    Inherits Windows.Forms.DataGridView

    Private _showButtonDelete As Boolean = False

    Public Event MyDataGridViewOnDeleteClick(ByVal rowNumbers As Integer())

    Public Sub New()
        Me.ShowCellErrors = True
    End Sub

    Public Sub _setColumnReadOnly(ByVal index As Integer)
        Me.Columns(index).DefaultCellStyle.BackColor = Color.LightGray
        Me.Columns(index).ReadOnly = True
    End Sub

    Public Sub _setColumnReadOnly(ByVal name As String)
        Me.Columns(name).DefaultCellStyle.BackColor = Color.LightGray
        Me.Columns(name).ReadOnly = True
    End Sub

    Private Sub MyDataGridView_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles Me.CellValidating
        Dim cell As DataGridViewCell = Me.Rows.Item(e.RowIndex).Cells.Item(e.ColumnIndex)

        If cell.Value Is Nothing OrElse cell.Value.GetType() Is cell.ValueType Then
            cell.ErrorText = String.Empty
        End If
    End Sub

    'Private Sub MyDataGridView_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Me.CellValidated
    ' Me.Rows(e.RowIndex).Cells(e.ColumnIndex).ErrorText = ""
    'End Sub


    Public Sub _addButtonDelete()
        Me.AllowUserToDeleteRows = False
        _showButtonDelete = True
    End Sub


    Private Sub MyDataGridView_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataSourceChanged
        If _showButtonDelete AndAlso Not Me.Columns.Contains("btnMyDelete") Then
            Dim imageColumn As New Windows.Forms.DataGridViewImageColumn
            imageColumn.HeaderText = ""
            imageColumn.Name = "btnMyDelete"
            imageColumn.Image = My.Resources.MyResource.elimina_16x16
            imageColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            Me.Columns.Add(imageColumn)

            AddHandler Me.CellContentClick, AddressOf Me.DataGridView1_CellContentClick
        End If
    End Sub
  

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Me.CellContentDoubleClick
        If Me.Columns(e.ColumnIndex).Name = "btnMyDelete" Then

            If System.Windows.Forms.MessageBox.Show("Confermare la cancellazione dell'elemento selezionato", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) <> System.Windows.Forms.DialogResult.OK Then
                Exit Sub
            End If

            Dim temp(0) As Integer
            temp(0) = e.RowIndex

            RaiseEvent MyDataGridViewOnDeleteClick(temp)
        End If

    End Sub



    Private Sub MyDataGridView_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Me.DataError
        If e.GetType.Name = "DataGridViewDataErrorEventArgs" Then

            Dim cell As DataGridViewCell = Me.Rows.Item(e.RowIndex).Cells.Item(e.ColumnIndex)


            If IsDBNull(cell.Value) Then
                e.Cancel = True
                Exit Sub
            End If


            Dim messaggio As String = ""

            If Not String.IsNullOrEmpty(e.Exception.Message) Then
                messaggio = e.Exception.Message
            End If

            If cell.GetType().Name = "DataGridViewComboBoxCell" Then
                e.Cancel = True
                If Not String.IsNullOrEmpty(cell.Value) Then
                    messaggio &= " Valore: " & cell.Value
                End If
            Else
                'nel caso di una Text Box annullo l'inserimento per perdere il focus!
                e.Cancel = False
                If Not String.IsNullOrEmpty(cell.EditedFormattedValue) Then
                    messaggio &= " Valore: " & cell.EditedFormattedValue
                End If
            End If


            cell.ErrorText = messaggio

            'If TypeOf cell Is System.Windows.Forms.DataGridViewCheckBoxColumn Then

            'End If



            '    If Not String.IsNullOrEmpty(cell.FormattedValue) Then
            'cell.ErrorText &= vbCrLf & "Valore: " & cell.FormattedValue
            'End If

            ' cell.ErrorText = String.Format("{0}{1}Valore immesso: '{2}'", e.Exception.Message, vbCrLf, cell.EditedFormattedValue)
            'Me.Rows(e.RowIndex).ErrorText = "Errore sul valore"
            'Me.Rows(e.RowIndex).Cells(e.ColumnIndex).ErrorText = e.Exception.Message
            ' System.Windows.Forms.MessageBox.Show(String.Format("{0}{1}Valore immesso: '{2}'", e.Exception.Message, vbCrLf, cell.EditedFormattedValue), My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop)
        Else
            Throw New Exception("Datagrid view", e.Exception)
        End If
    End Sub



    ' ''' <summary>
    ' ''' COLONNA CALENDAR CONTROL
    ' ''' </summary>
    ' ''' 
    Public Class CalendarColumn
        Inherits DataGridViewColumn

        Public Sub New()
            MyBase.New(New CalendarCell())
        End Sub

        Public Overrides Property CellTemplate() As DataGridViewCell
            Get
                Return MyBase.CellTemplate
            End Get

            Set(ByVal value As DataGridViewCell)
                ' Ensure that the cell used for the template is a CalendarCell.
                If (value IsNot Nothing) AndAlso Not value.GetType().IsAssignableFrom(GetType(CalendarCell)) Then
                    Throw New InvalidCastException("Must be a CalendarCell")
                End If
                MyBase.CellTemplate = value

            End Set
        End Property

        Public Class CalendarCell
            Inherits DataGridViewTextBoxCell

            Public Sub New()
                ' Use the short date format.
                Me.Style.Format = "d"
            End Sub

            Public Overrides Sub InitializeEditingControl(ByVal rowIndex As Integer, _
                ByVal initialFormattedValue As Object, _
                ByVal dataGridViewCellStyle As DataGridViewCellStyle)

                ' Set the value of the editing control to the current cell value.
                MyBase.InitializeEditingControl(rowIndex, initialFormattedValue, _
                    dataGridViewCellStyle)

                Dim ctl As CalendarEditingControl = _
                    CType(DataGridView.EditingControl, CalendarEditingControl)

                ' Use the default row value when Value property is null.
                If (Me.Value Is Nothing) OrElse IsDBNull(Me.Value) Then
                    ctl.Value = CType(Me.DefaultNewRowValue, DateTime)
                Else
                    ctl.Value = CType(Me.Value, DateTime)
                End If
            End Sub

            Public Overrides ReadOnly Property EditType() As Type
                Get
                    ' Return the type of the editing control that CalendarCell uses.
                    Return GetType(CalendarEditingControl)
                End Get
            End Property

            Public Overrides ReadOnly Property ValueType() As Type
                Get
                    ' Return the type of the value that CalendarCell contains.
                    Return GetType(DateTime)
                End Get
            End Property

            Public Overrides ReadOnly Property DefaultNewRowValue() As Object
                Get
                    ' Use the current date and time as the default value.
                    Return DateTime.Now
                End Get
            End Property

        End Class

        Class CalendarEditingControl
            Inherits DateTimePicker
            Implements IDataGridViewEditingControl

            Private dataGridViewControl As DataGridView
            Private valueIsChanged As Boolean = False
            Private rowIndexNum As Integer

            Public Sub New()
                Me.Format = DateTimePickerFormat.Short
            End Sub

            Public Property EditingControlFormattedValue() As Object _
                Implements IDataGridViewEditingControl.EditingControlFormattedValue

                Get
                    Return Me.Value.ToShortDateString()
                End Get

                Set(ByVal value As Object)
                    Try
                        ' This will throw an exception of the string is 
                        ' null, empty, or not in the format of a date.
                        Me.Value = DateTime.Parse(CStr(value))
                    Catch
                        ' In the case of an exception, just use the default
                        ' value so we're not left with a null value.
                        Me.Value = DateTime.Now
                    End Try
                End Set

            End Property

            Public Function GetEditingControlFormattedValue(ByVal context _
                As DataGridViewDataErrorContexts) As Object _
                Implements IDataGridViewEditingControl.GetEditingControlFormattedValue

                Return Me.Value.ToShortDateString()

            End Function

            Public Sub ApplyCellStyleToEditingControl(ByVal dataGridViewCellStyle As  _
                DataGridViewCellStyle) _
                Implements IDataGridViewEditingControl.ApplyCellStyleToEditingControl

                Me.Font = dataGridViewCellStyle.Font
                Me.CalendarForeColor = dataGridViewCellStyle.ForeColor
                Me.CalendarMonthBackground = dataGridViewCellStyle.BackColor

            End Sub

            Public Property EditingControlRowIndex() As Integer _
                Implements IDataGridViewEditingControl.EditingControlRowIndex

                Get
                    Return rowIndexNum
                End Get
                Set(ByVal value As Integer)
                    rowIndexNum = value
                End Set

            End Property

            Public Function EditingControlWantsInputKey(ByVal key As Keys, _
                ByVal dataGridViewWantsInputKey As Boolean) As Boolean _
                Implements IDataGridViewEditingControl.EditingControlWantsInputKey

                ' Let the DateTimePicker handle the keys listed.
                Select Case key And Keys.KeyCode
                    Case Keys.Left, Keys.Up, Keys.Down, Keys.Right, _
                        Keys.Home, Keys.End, Keys.PageDown, Keys.PageUp

                        Return True

                    Case Else
                        Return Not dataGridViewWantsInputKey
                End Select

            End Function

            Public Sub PrepareEditingControlForEdit(ByVal selectAll As Boolean) _
                Implements IDataGridViewEditingControl.PrepareEditingControlForEdit

                ' No preparation needs to be done.

            End Sub

            Public ReadOnly Property RepositionEditingControlOnValueChange() _
                As Boolean Implements _
                IDataGridViewEditingControl.RepositionEditingControlOnValueChange

                Get
                    Return False
                End Get

            End Property

            Public Property EditingControlDataGridView() As DataGridView _
                Implements IDataGridViewEditingControl.EditingControlDataGridView

                Get
                    Return dataGridViewControl
                End Get
                Set(ByVal value As DataGridView)
                    dataGridViewControl = value
                End Set

            End Property

            Public Property EditingControlValueChanged() As Boolean _
                Implements IDataGridViewEditingControl.EditingControlValueChanged

                Get
                    Return valueIsChanged
                End Get
                Set(ByVal value As Boolean)
                    valueIsChanged = value
                End Set

            End Property

            Public ReadOnly Property EditingControlCursor() As Cursor _
                Implements IDataGridViewEditingControl.EditingPanelCursor

                Get
                    Return MyBase.Cursor
                End Get

            End Property

            Protected Overrides Sub OnValueChanged(ByVal eventargs As EventArgs)

                ' Notify the DataGridView that the contents of the cell have changed.
                valueIsChanged = True
                Me.EditingControlDataGridView.NotifyCurrentCellDirty(True)
                MyBase.OnValueChanged(eventargs)

            End Sub

        End Class

    End Class



    Public Class NumericUpAndDown_Column
        Inherits DataGridViewColumn

        Public Sub New()
            MyBase.New(New NudCell())
        End Sub

        Public Overrides Property CellTemplate() As DataGridViewCell
            Get
                Return MyBase.CellTemplate
            End Get

            Set(ByVal value As DataGridViewCell)
                Try
                    ' Ensure that the cell used for the template is a CalendarCell.
                    If (value IsNot Nothing) AndAlso Not value.GetType().IsAssignableFrom(GetType(NudCell)) Then
                        Throw New InvalidCastException("Must be a nud cell")
                    End If
                    MyBase.CellTemplate = value
                Catch

                    ' In the case of an exception, just use the default
                    ' value so we're not left with a null value.
                    Throw New Exception("error ppg 020")
                End Try

            End Set
        End Property

        Public Class NudCell
            Inherits DataGridViewTextBoxCell

            Public Sub New()
                ' Use the Decimal number
                'Me.Style.Format = "D"
            End Sub

            Overloads Sub InitializeEditingControl(ByVal rowIndex As Integer, _
                                                            ByVal initialFormattedValue As Decimal, _
                                                            ByVal dataGridViewCellStyle As DataGridViewCellStyle)

                ' Set the value of the editing control to the current cell value.
                Try
                    MyBase.InitializeEditingControl(rowIndex, initialFormattedValue, _
                        dataGridViewCellStyle)

                    'Dim ctl As CalendarEditingControl = CType(DataGridView.EditingControl, CalendarEditingControl)
                    Dim ctl As NudEditingControl = CType(DataGridView.EditingControl, NudEditingControl)

                    ' Use the default row value when Value property is null.
                    If (Me.Value Is Nothing) OrElse IsDBNull(Me.Value) Then
                        ctl.Value = CType(Me.DefaultNewRowValue, Decimal)
                    Else
                        ctl.Value = CType(Me.Value, Decimal)
                    End If
                Catch

                    ' In the case of an exception, just use the default
                    ' value so we're not left with a null value.
                    Throw New Exception("error ppg 019")
                End Try

            End Sub

            Public Overrides ReadOnly Property EditType() As Type
                Get
                    ' Return the type of the editing control that CalendarCell uses.
                    Return GetType(NudEditingControl)
                End Get
            End Property

            Public Overrides ReadOnly Property ValueType() As Type
                Get
                    ' Return the type of the value that CalendarCell contains.
                    Return GetType(Double)
                End Get
            End Property

            Public Overrides ReadOnly Property DefaultNewRowValue() As Object
                Get
                    ' Use the current date and time as the default value.
                    Return Decimal.Zero
                End Get
            End Property

        End Class

        Class NudEditingControl
            Inherits NumericUpDown
            Implements IDataGridViewEditingControl

            Private dataGridViewControl As DataGridView
            Private valueIsChanged As Boolean = False
            Private rowIndexNum As Integer

            Public Sub New()
                '                Me.Format = DateTimePickerFormat.Short
            End Sub

            Public Property EditingControlFormattedValue() As Object _
                Implements IDataGridViewEditingControl.EditingControlFormattedValue

                Get
                    Return Me.Value
                End Get

                Set(ByVal value As Object)
                    Try
                        ' This will throw an exception of the string is 
                        ' null, empty, or not in the format of a date.
                        Me.Value = Decimal.Parse(CStr(value))
                    Catch
                        ' In the case of an exception, just use the default
                        ' value so we're not left with a null value.
                        Throw New Exception("error ppg 009")
                    End Try
                End Set

            End Property

            Public Function GetEditingControlFormattedValue(ByVal context As DataGridViewDataErrorContexts) As Object _
                Implements IDataGridViewEditingControl.GetEditingControlFormattedValue
                Try
                    Return Decimal.Parse(Me.Value)
                Catch
                    Throw New Exception("paolo errore 001")
                End Try
            End Function

            Public Sub ApplyCellStyleToEditingControl(ByVal dataGridViewCellStyle As  _
                DataGridViewCellStyle) _
                Implements IDataGridViewEditingControl.ApplyCellStyleToEditingControl

                'Me.Font = dataGridViewCellStyle.Font
                'Me.CalendarForeColor = dataGridViewCellStyle.ForeColor
                'Me.CalendarMonthBackground = dataGridViewCellStyle.BackColor

                Try

                Catch
                    Throw New Exception("paolo errore 002")
                End Try
            End Sub

            Public Property EditingControlRowIndex() As Integer _
                Implements IDataGridViewEditingControl.EditingControlRowIndex

                Get
                    Return rowIndexNum
                End Get
                Set(ByVal value As Integer)
                    rowIndexNum = value
                End Set

            End Property

            Public Function EditingControlWantsInputKey(ByVal key As Keys, _
                ByVal dataGridViewWantsInputKey As Boolean) As Boolean _
                Implements IDataGridViewEditingControl.EditingControlWantsInputKey

                ' Let the DateTimePicker handle the keys listed.
                Select Case key And Keys.KeyCode
                    Case Keys.Left, Keys.Up, Keys.Down, Keys.Right, _
                        Keys.Home, Keys.End, Keys.PageDown, Keys.PageUp

                        Return True

                    Case Else
                        Return Not dataGridViewWantsInputKey
                End Select

            End Function

            Public Sub PrepareEditingControlForEdit(ByVal selectAll As Boolean) _
                Implements IDataGridViewEditingControl.PrepareEditingControlForEdit

                Try

                Catch
                    Throw New Exception("paolo errore 003")
                End Try

            End Sub

            Public ReadOnly Property RepositionEditingControlOnValueChange() _
                As Boolean Implements _
                IDataGridViewEditingControl.RepositionEditingControlOnValueChange

                Get
                    Return False
                End Get

            End Property

            Public Property EditingControlDataGridView() As DataGridView _
                Implements IDataGridViewEditingControl.EditingControlDataGridView

                Get
                    Return dataGridViewControl
                End Get
                Set(ByVal value As DataGridView)
                    dataGridViewControl = value
                End Set

            End Property

            Public Property EditingControlValueChanged() As Boolean _
                Implements IDataGridViewEditingControl.EditingControlValueChanged

                Get
                    Return valueIsChanged
                End Get
                Set(ByVal value As Boolean)
                    valueIsChanged = value
                End Set

            End Property

            Public ReadOnly Property EditingControlCursor() As Cursor _
                Implements IDataGridViewEditingControl.EditingPanelCursor

                Get
                    Return MyBase.Cursor
                End Get

            End Property

            Protected Overrides Sub OnValueChanged(ByVal eventargs As EventArgs)

                ' Notify the DataGridView that the contents of the cell have changed.
                Try
                    valueIsChanged = True
                    Me.EditingControlDataGridView.NotifyCurrentCellDirty(True)
                    MyBase.OnValueChanged(eventargs)
                Catch
                    Throw New Exception("error ppg")
                End Try
            End Sub

        End Class

    End Class


  
 
End Class
