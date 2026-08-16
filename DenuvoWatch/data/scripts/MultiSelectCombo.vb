' =============================================================================
' Class: MultiSelectCombo
' Turns a ComboBox into a multi-select with checkboxes in a popup.
' =============================================================================
Public Class MultiSelectCombo
    Private ReadOnly combo As ComboBox
    Private ReadOnly ownerForm As Form
    Private ReadOnly placeholderText As String

    Private ReadOnly dropdown As ToolStripDropDown
    Private ReadOnly host As ToolStripControlHost
    Private ReadOnly checkedList As CheckedListBox

    Public Event SelectionChanged As EventHandler

    ' Grab the combo's text as a placeholder so I can put it back when everything's unchecked
    Public Sub New(combo As ComboBox, ownerForm As Form)
        Me.combo = combo
        Me.ownerForm = ownerForm
        Me.placeholderText = combo.Text

        ' Hide the built-in dropdown so it doesn't flash
        combo.DropDownHeight = 1
        combo.IntegralHeight = False

        checkedList = New CheckedListBox() With {
            .CheckOnClick = True,
            .BorderStyle = BorderStyle.None
        }

        ' The popup is a ToolStripDropDown so it doesn't steal focus
        host = New ToolStripControlHost(checkedList) With {
            .AutoSize = False,
            .Margin = New Padding(0, 0, 0, 0),
            .Padding = New Padding(0, 0, 0, 0)
        }

        dropdown = New ToolStripDropDown() With {
            .AutoSize = False,
            .DropShadowEnabled = True
        }
        dropdown.Items.Add(host)

        AddHandler checkedList.ItemCheck, Sub(s, e)
            checkedList.BeginInvoke(Sub()
                UpdateComboText()
                RaiseEvent SelectionChanged(Me, EventArgs.Empty)
            End Sub)
        End Sub

        AddHandler combo.DropDown, Sub(s, e) ShowPopup()
        AddHandler ownerForm.Move, Sub(s, e) dropdown.Close()
        AddHandler ownerForm.SizeChanged, Sub(s, e) dropdown.Close()
    End Sub

    ' Copy items over, size it, and show it under the combo
    Private Sub ShowPopup()
        If dropdown.Visible Then
            dropdown.Close()
            Return
        End If

        ' Only need to copy items once
        If checkedList.Items.Count <> combo.Items.Count Then
            checkedList.Items.Clear()
            For Each item In combo.Items
                checkedList.Items.Add(item)
            Next
        End If

        ' Apply theme to the popup list
        If IsDarkTheme Then
            checkedList.BackColor = DarkSurface
            checkedList.ForeColor = DarkText
        Else
            checkedList.BackColor = SystemColors.Window
            checkedList.ForeColor = SystemColors.ControlText
        End If

        ' Cap at 300px tall so it scrolls if the list is long
        Dim itemHeight As Integer = checkedList.GetItemHeight(0)
        Dim listHeight = Math.Min(checkedList.Items.Count*(itemHeight + 4) + 4, 300)
        Dim listWidth = combo.Width

        checkedList.Width = listWidth
        checkedList.Height = listHeight

        ' Gotta set the size on all three or nothing shows up right
        host.Width = listWidth
        host.Height = listHeight
        dropdown.Width = listWidth
        dropdown.Height = listHeight

        Dim screenPoint = combo.PointToScreen(New Point(0, combo.Height))
        dropdown.Show(screenPoint)
    End Sub

    ' Put the checked items in the combo text, or the placeholder if nothing's checked
    Private Sub UpdateComboText()
        Dim display = GetCheckedItemsDisplay()
        combo.Text = If(String.IsNullOrEmpty(display), placeholderText, display)
    End Sub

    ' What's checked, joined by commas - for the URL
    Public Function GetCheckedItems() As String
        Dim selected As New List(Of String)
        For Each item In checkedList.CheckedItems
            Dim s = item.ToString().Trim()
            If s <> "" Then selected.Add(s)
        Next
        Return String.Join(",", selected)
    End Function

    ' Same but with spaces - for showing the user
    Public Function GetCheckedItemsDisplay() As String
        Dim selected As New List(Of String)
        For Each item In checkedList.CheckedItems
            Dim s = item.ToString().Trim()
            If s <> "" Then selected.Add(s)
        Next
        Return String.Join(", ", selected)
    End Function

    ' Clear everything and put the placeholder back
    Public Sub Reset()
        For i = 0 To checkedList.Items.Count - 1
            checkedList.SetItemChecked(i, False)
        Next
        combo.Text = placeholderText
    End Sub

    Public Function HasSelection() As Boolean
        Return checkedList.CheckedItems.Count > 0
    End Function
End Class
