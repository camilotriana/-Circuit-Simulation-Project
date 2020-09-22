Imports System.IO

Public Class FormPrincipal
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Cerrar_Octave()
        End
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        MsgBox("Seleccione ruta de instalación de Octave", MsgBoxStyle.OkOnly)
        Dim folder As New FolderBrowserDialog
        Dim ruta As String = ""
        If folder.ShowDialog() = DialogResult.OK Then
            ruta = folder.SelectedPath & "\mingw64\bin\octave-cli.exe"
            Try
                Dim escritor As StreamWriter
                My.Computer.FileSystem.DeleteFile(Application.StartupPath &
                                                  "\Ruta.txt")
                escritor = File.AppendText(Application.StartupPath &
                                                  "\Ruta.txt")
                escritor.Write(ruta)
                escritor.Flush()
                escritor.Close()
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form1.MdiParent = Me
        Form1.Show()
    End Sub

    Private Sub FormPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim ruta_archivo As StreamReader
        ruta_archivo = New StreamReader(Application.StartupPath &
                                                  "\Ruta.txt")
        Ruta_Octave = ruta_archivo.ReadLine
        ruta_archivo.Close()
    End Sub
End Class