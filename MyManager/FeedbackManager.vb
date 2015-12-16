Public Class FeedbackManager
    Inherits Manager

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub


    Public Function insert(ByVal testo As String, ByVal userId As Long, ByVal externalId As Long) As Long
        mStrSQL = "INSERT INTO feedback (DATE_ADDED, FK_USER_ID , FK_EXTERNAL_ID, TESTO )" & _
            " VALUES ( NOW() , @FK_USER_ID , @FK_EXTERNAL_ID , @TESTO )"

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = mStrSQL

        Me.mAddParameter(command, "@FK_USER_ID", userId)
        Me.mAddParameter(command, "@FK_EXTERNAL_ID", externalId)
        Me.mAddParameter(command, "@TESTO", testo)

        mExecuteNoQuery(command)
        Return _getIdentity()
    End Function





End Class
