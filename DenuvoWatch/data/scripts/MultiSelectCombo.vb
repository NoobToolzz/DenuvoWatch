

' =============================================================================
' Class: MultiSelectCombo
' -----------------------------------------------------------------------------
' Wraps a standard WinForms ComboBox to provide multi-select behaviour via a
' CheckedListBox popup. When the user clicks the ComboBox dropdown arrow, a
' ToolStripDropDown containing a CheckedListBox appears below it. The
' ComboBox text area displays the checked items joined by ", ".
'
' Uses ToolStripDropDown instead of a separate Form so the popup does NOT
' steal focus from the owner form — it closes automatically when the user
' clicks elsewhere, and the owner form stays visible and active.
'
' Usage:
'   Dim msc As New MultiSelectCombo(cbDeveloper, Me)
'   Dim selected = msc.GetCheckedItems()   ' comma-separated for URL building
' =============================================================================
Public Class MultiSelectCombo
    Private ReadOnly combo As ComboBox
    Private ReadOnly ownerForm As Form
    Private ReadOnly placeholderText As String

    ' ToolStripDropDown hosts the CheckedListBox without stealing focus.
    Private ReadOnly dropdown As ToolStripDropDown
    Private ReadOnly host As ToolStripControlHost
    Private ReadOnly checkedList As CheckedListBox

    ' Raised whenever the user checks or unchecks an item.
    Public Event SelectionChanged As EventHandler

    ' ---------------------------------------------------------------------------
    ' Constructor
    '   Wires up the popup behaviour on the given ComboBox. Grabs the combo's
    '   current text as the placeholder so I can restore it when everything
    '   gets unchecked.
    ' ---------------------------------------------------------------------------
    Public Sub New(combo As ComboBox, ownerForm As Form)
        Me.combo = combo
        Me.ownerForm = ownerForm
        Me.placeholderText = combo.Text

        ' I shrink the built-in dropdown to basically nothing so it doesn't flash on screen
        combo.DropDownHeight = 1
        combo.IntegralHeight = False

        ' This is the list that actually shows up with checkboxes
        checkedList = New CheckedListBox() With {
            .CheckOnClick = True,
            .BorderStyle = BorderStyle.None
            }

        ' I host the CheckedListBox inside a ToolStripControlHost inside a
        ' ToolStripDropDown — this is the trick to get a popup that doesn't steal focus
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

        ' When the user clicks the dropdown arrow, show my popup instead
        AddHandler combo.DropDown, Sub(s, e) ShowPopup()

        ' If the form moves or resizes while the popup is open, close it so it doesn't float in the wrong spot
        AddHandler ownerForm.Move, Sub(s, e) dropdown.Close()
        AddHandler ownerForm.SizeChanged, Sub(s, e) dropdown.Close()
    End Sub

    ' ---------------------------------------------------------------------------
    ' ShowPopup
    '   Syncs the CheckedListBox items from the ComboBox, sizes and positions
    '   the dropdown below the ComboBox, and shows it.
    ' ---------------------------------------------------------------------------
    Private Sub ShowPopup()
        ' If it's already open, just close it — this acts as a toggle
        If dropdown.Visible Then
            dropdown.Close()
            Return
        End If

        ' Copy the items from the ComboBox into the CheckedListBox (only need to do this once)
        If checkedList.Items.Count <> combo.Items.Count Then
            checkedList.Items.Clear()
            For Each item In combo.Items
                checkedList.Items.Add(item)
            Next
        End If

        ' Figure out how tall the list should be — cap it at 300px so it scrolls if there are too many items
        Dim itemHeight As Integer = checkedList.GetItemHeight(0)
        Dim listHeight = Math.Min(checkedList.Items.Count*(itemHeight + 4) + 4, 300)
        Dim listWidth = combo.Width

        checkedList.Width = listWidth
        checkedList.Height = listHeight

        ' I have to set the size on all three — the list, the host, and the dropdown itself
        ' because AutoSize is off so nothing will figure it out for me
        host.Width = listWidth
        host.Height = listHeight
        dropdown.Width = listWidth
        dropdown.Height = listHeight

        ' Stick it right below the ComboBox
        Dim screenPoint = combo.PointToScreen(New Point(0, combo.Height))
        dropdown.Show(screenPoint)
    End Sub

    ' ---------------------------------------------------------------------------
    ' UpdateComboText
    '   Sets the ComboBox.Text to the display string of checked items.
    '   If nothing is checked, I put the placeholder back so the combo
    '   doesn't look empty and confusing.
    ' ---------------------------------------------------------------------------
    Private Sub UpdateComboText()
        Dim display = GetCheckedItemsDisplay()
        combo.Text = If(String.IsNullOrEmpty(display), placeholderText, display)
    End Sub

    ' ---------------------------------------------------------------------------
    ' GetCheckedItems
    '   Returns the checked item strings joined by "," (no spaces) — suitable
    '   for URL query parameter values.
    ' ---------------------------------------------------------------------------
    Public Function GetCheckedItems() As String
        Dim selected As New List(Of String)
        For Each item In checkedList.CheckedItems
            Dim s = item.ToString().Trim()
            If s <> "" Then selected.Add(s)
        Next
        Return String.Join(",", selected)
    End Function

    ' ---------------------------------------------------------------------------
    ' GetCheckedItemsDisplay
    '   Returns the checked item strings joined by ", " — for human-readable
    '   display in the ComboBox text area.
    ' ---------------------------------------------------------------------------
    Public Function GetCheckedItemsDisplay() As String
        Dim selected As New List(Of String)
        For Each item In checkedList.CheckedItems
            Dim s = item.ToString().Trim()
            If s <> "" Then selected.Add(s)
        Next
        Return String.Join(", ", selected)
    End Function

    ' ---------------------------------------------------------------------------
    ' Reset
    '   Unchecks all items and restores the ComboBox placeholder text.
    ' ---------------------------------------------------------------------------
    Public Sub Reset()
        For i = 0 To checkedList.Items.Count - 1
            checkedList.SetItemChecked(i, False)
        Next
        combo.Text = placeholderText
    End Sub

    ' ---------------------------------------------------------------------------
    ' HasSelection
    '   Returns True if at least one item is checked.
    ' ---------------------------------------------------------------------------
    Public Function HasSelection() As Boolean
        Return checkedList.CheckedItems.Count > 0
    End Function
End Class
