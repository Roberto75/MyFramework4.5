Public Interface IFormSearch
    Function DoSearch(ByVal pageNumber As Int16, ByVal pageSize As Int16) As Boolean
    'Function ApplyGrant() As Boolean
    'Function ApplyFilter() As Boolean
    Function InitColumns() As Boolean
    'Function ItemOnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) As Boolean
End Interface