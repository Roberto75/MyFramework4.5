Public Interface IFormMaster
    Function MenuTopSubItemClicked(ByVal clickedItem As System.Windows.Forms.ToolStripItem) As Boolean
    Function BuildMenuLeft() As Boolean
    Function BuildMenuTop() As Boolean
End Interface

