Public Class SkypeManager

    Private oSkype As SKYPE4COMLib.Skype = Nothing
    Private _status As ServiceStatus


    Public Enum ServiceStatus
        NotAvailabe
        Idle
        Busy
    End Enum


    Public Sub New()
        Try
            oSkype = New SKYPE4COMLib.Skype
            'oSkype.SilentMode = True
            _status = ServiceStatus.Idle
        Catch ex As Exception
            oSkype = Nothing
        End Try
    End Sub


    Public Function getStatus() As ServiceStatus
        If oSkype Is Nothing Then
            Return ServiceStatus.NotAvailabe
        End If
        Return _status
    End Function




    Public Function checkTelephone(ByVal telephoneNumber As String) As String


        'Try
        '    oSkype = New SKYPE4COMLib.Skype
        '    'oSkype.SilentMode = True
        '    _status = ServiceStatus.Idle
        'Catch ex As Exception
        '    oSkype = Nothing
        'End Try



        If oSkype Is Nothing Then
            Return ServiceStatus.NotAvailabe.ToString
        End If

        Dim result As String = ""

        If Not oSkype.Client.IsRunning Then
            oSkype.Client.Start(True, True)
        End If


        If oSkype.AttachmentStatus <> SKYPE4COMLib.TAttachmentStatus.apiAttachAvailable Then
            oSkype.Attach(8, False)
        End If


        While oSkype.AttachmentStatus <> SKYPE4COMLib.TAttachmentStatus.apiAttachSuccess
            System.Threading.Thread.Sleep(1000)
        End While



        result = oSkype.AttachmentStatus

        'oSkype.Attach(7, True)

        'oSkype.Client.Minimize()


        If oSkype.AttachmentStatus = SKYPE4COMLib.TAttachmentStatus.apiAttachNotAvailable Then
            Return "Check non available"
        End If

        _status = ServiceStatus.Busy

        Try
            If oSkype.CurrentUserStatus = SKYPE4COMLib.TUserStatus.cusOffline Then
                oSkype.ChangeUserStatus(SKYPE4COMLib.TUserStatus.cusOnline)
            End If


            Dim oUser As SKYPE4COMLib.User
            oUser = oSkype.User(telephoneNumber)

            Dim oCall As SKYPE4COMLib.Call
            oCall = oSkype.PlaceCall(oUser.Handle)

            Dim sentinel As Boolean = True
            Dim oStatus As SKYPE4COMLib.TCallStatus



            While (sentinel)

                oStatus = oCall.Status

                If oStatus = SKYPE4COMLib.TCallStatus.clsFailed Then
                    sentinel = False
                ElseIf oStatus = SKYPE4COMLib.TCallStatus.clsRouting _
                    OrElse oStatus = SKYPE4COMLib.TCallStatus.clsUnplaced Then
                    'per lo stato 0 e 1 lo lascio fare
                Else
                    'Per qualsiasi altro stato, segreteria, non raggiungibile , ecc 
                    'lo ritengo valido
                    oCall.Finish()
                    sentinel = False
                End If
                'SKYPE4COMLib.TCallStatus.clsEarlyMedia

                '()
                ' If oStatus = SKYPE4COMLib.TCallStatus.clsRinging Then _
                '    OrElse oStatus = SKYPE4COMLib.TCallStatus.clsEarlyMedia OrElse
                '    oStatus = SKYPE4COMLib.TCallStatus.clsBusy Then
                '    'System.Threading.Thread.Sleep(5000)
                '    oCall.Finish()
                '    'result = SKYPE4COMLib.TCallStatus.clsFinished

                '    oStatus = SKYPE4COMLib.TCallStatus.clsFinished
                '    sentinel = False
                'ElseIf oStatus = SKYPE4COMLib.TCallStatus.clsFinished _
                '    OrElse oStatus = SKYPE4COMLib.TCallStatus.clsRefused Then
                '    sentinel = False
                'Else

                'End If

            End While

            If oStatus = SKYPE4COMLib.TCallStatus.clsFailed Then
                result = "Number not valid"
            Else
                result = ""
            End If

        Catch ex As Exception
            Dim debugg As String
            debugg = ex.Message

        Finally
            _status = ServiceStatus.Idle
        End Try

        Return result
    End Function


End Class
