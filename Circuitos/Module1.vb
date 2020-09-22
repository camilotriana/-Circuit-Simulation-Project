Module Module1
    Public Ruta_Octave As String = ""
    Public Sub sendOctave(cadena As String)
        AppActivate(Ruta_Octave)
        SendKeys.SendWait(cadena & Chr(13))
        System.Threading.Thread.Sleep(10)
    End Sub
    Public Sub Cerrar_Octave()
        Try
            sendOctave("exit")
        Catch ex As Exception

        End Try
    End Sub
End Module
