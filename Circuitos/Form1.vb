Imports System.IO
Public Class Form1
    Dim vr, vl, vc, vz, vzl, tipoGraf, voltaje As String
    Dim pInfo As New ProcessStartInfo
    Dim p As Process
    Dim vectory1(10) As Double
    Dim vectory2(10) As Double
    Dim vectory3(10) As Double
    Dim vectort(10) As Double
    Dim cant_elementos, gan As Integer
    Dim aux As Integer = 0
    Dim valor_min, valor_min1, valor_max, valor_max1 As Double
    Dim ciclo1, ciclo2 As Integer
    Dim rango_sim As Double = 0
    Dim rango_sim2 As Double = 0
    Dim proceso_sim As Boolean = False
    Dim valor_intervalo As Double = 0
    Dim dif1, dif2, auxdif As Integer

    Private Sub MinZ_Scroll(sender As Object, e As EventArgs) Handles MinZ.Scroll
        Lminz.Text = MinZ.Value
    End Sub

    Private Sub MaxZ_Scroll(sender As Object, e As EventArgs) Handles MaxZ.Scroll
        Lmaxz.Text = MaxZ.Value
        MaxZ.Minimum = MinZ.Value
    End Sub

    Private Sub MinZL_Scroll(sender As Object, e As EventArgs) Handles MinZL.Scroll
        Lminzl.Text = MinZL.Value
    End Sub

    Private Sub MaxZL_Scroll(sender As Object, e As EventArgs) Handles MaxZL.Scroll
        Lmaxzl.Text = MaxZL.Value
        MaxZL.Minimum = MinZL.Value
    End Sub

    Private Sub TrackBarvi_Scroll(sender As Object, e As EventArgs) Handles TrackBarvi.Scroll
        LIV.Text = TrackBarvi.Value / 100
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CB1.SelectedIndexChanged
        Select Case CB1.SelectedIndex
            Case 0
                elemento1.Image = Image.FromFile(Application.StartupPath & "\Imagenes\r.png")
                MinZ.Minimum = 1
                MinZ.Maximum = 5
                MaxZ.Maximum = 6
                Lminz.Text = MinZ.Value
                Lmaxz.Text = ""
            Case 1
                elemento1.Image = Image.FromFile(Application.StartupPath & "\Imagenes\l.png")
                MinZ.Minimum = 0
                MinZ.Maximum = 1
                MaxZ.Maximum = 2
                Lminz.Text = MinZ.Value
                Lmaxz.Text = ""
            Case 2
                elemento1.Image = Image.FromFile(Application.StartupPath & "\Imagenes\c.png")
                MinZ.Minimum = 0
                MinZ.Maximum = 1
                MaxZ.Maximum = 2
                Lminz.Text = MinZ.Value
                Lmaxz.Text = ""
        End Select
    End Sub

    Private Sub CB2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CB2.SelectedIndexChanged
        Select Case CB2.SelectedIndex
            Case 0
                elemento2.Image = Image.FromFile(Application.StartupPath & "\Imagenes\r1.png")
                MinZL.Minimum = 1
                MinZL.Maximum = 5
                MaxZL.Maximum = 6
                Lminzl.Text = MinZL.Value
                Lmaxzl.Text = ""
            Case 1
                elemento2.Image = Image.FromFile(Application.StartupPath & "\Imagenes\l1.png")
                MinZL.Minimum = 0
                MinZL.Maximum = 1
                MaxZL.Maximum = 2
                Lminzl.Text = MinZL.Value
                Lmaxzl.Text = ""
            Case 2
                elemento2.Image = Image.FromFile(Application.StartupPath & "\Imagenes\c1.png")
                MinZL.Minimum = 0
                MinZL.Maximum = 1
                MaxZL.Maximum = 2
                Lminzl.Text = MinZL.Value
                Lmaxzl.Text = ""
        End Select
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        valor_intervalo = TrackBarvi.Value / 100
        valor_min = MinZ.Value
        valor_max = MaxZ.Value
        valor_min1 = MinZL.Value
        valor_max1 = MaxZL.Value

        If valor_min < valor_min1 Then
            dif1 = valor_min1
        Else
            dif1 = valor_min
        End If

        If valor_max < valor_max1 Then
            dif2 = valor_max1
        Else
            dif2 = valor_max
        End If

        Timer2.Enabled = True
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If proceso_sim = False And (rango_sim + dif1) <= dif2 Then
            If (rango_sim + valor_min) <= valor_max Then
                Z.Text = Math.Round(valor_min + rango_sim, 2)
            End If

            If (rango_sim + valor_min1) <= valor_max1 Then
                ZL.Text = Math.Round(valor_min1 + rango_sim, 2)
            End If

            rango_sim += valor_intervalo
            Button1.PerformClick()
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CB1.SelectedIndex = 0
        CB2.SelectedIndex = 0
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        proceso_sim = True
        aux = 0
        Chart1.Series(0).Points.Clear()
        Chart2.Series(0).Points.Clear()
        Chart3.Series(0).Points.Clear()
        Try
            pInfo.FileName = Ruta_Octave
            pInfo.WindowStyle = ProcessWindowStyle.Minimized
            p = Process.Start(pInfo)
        Catch ex As Exception
            MsgBox("No se encuentra el archivo Octave-CLI")
            Button1.Enabled = False
        End Try

        System.Threading.Thread.Sleep(1000)

        sendOctave("cd '" & Application.StartupPath & "'")

        Select Case CB1.SelectedIndex
            Case 0
                vz = Z.Text
            Case 1
                vz = Z.Text & "*s"
            Case 2
                vz = "1/(" & Z.Text & "*s)"
        End Select

        Select Case CB2.SelectedIndex
            Case 0
                vzl = ZL.Text
            Case 1
                vzl = ZL.Text & "*s"
            Case 2
                vzl = "1/(" & ZL.Text & "*s)"
        End Select

        vr = R.Text
        vc = C.Text
        vl = L.Text
        cant_elementos = Cantidad.Text
        gan = Ganancia.Text

        If CBG.Text = "PASO" Then
            tipoGraf = "step"
        Else
            tipoGraf = "impulse"
        End If

        voltaje = TBVoltaje.Text

        Crear_Mfile()

        sendOctave("POctave")
        System.Threading.Thread.Sleep(1500)
        sendOctave("exit")
        Cargar()
    End Sub

    Sub Cargar()
        ReDim vectory1(cant_elementos)
        ReDim vectory2(cant_elementos)
        ReDim vectory3(cant_elementos)
        ReDim vectort(cant_elementos)

        Dim datosy1, datosy2, datost As StreamReader

        datosy1 = New StreamReader(Application.StartupPath & "\v.txt")
        For j As Integer = 0 To cant_elementos - 1
            vectory1(j) = Val(datosy1.ReadLine) * gan
        Next
        datosy1.Close()

        datosy2 = New StreamReader(Application.StartupPath & "\i.txt")
        For j As Integer = 0 To cant_elementos - 1
            vectory2(j) = Val(datosy2.ReadLine) * gan
        Next
        datosy2.Close()

        For j As Integer = 0 To cant_elementos - 1
            vectory3(j) = vectory1(j) * vectory2(j)
        Next

        datost = New StreamReader(Application.StartupPath & "\t1.txt")
        For j As Integer = 0 To cant_elementos - 1
            vectort(j) = Val(datost.ReadLine)
        Next
        datost.Close()
        If CheckBox1.CheckState = True Then
            Timer1.Enabled = True
        Else
            For i As Integer = 0 To cant_elementos - 1
                Proceso()
            Next
            Chart1.Series(0).ToolTip = "#VAL"
            Chart2.Series(0).ToolTip = "#VAL"
            Chart3.Series(0).ToolTip = "#VAL"
            Registro_DG()
        End If

    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Proceso()
        If aux = cant_elementos Then
            Registro_DG()
            Timer1.Enabled = False
        End If
    End Sub
    Sub Proceso()
        Chart1.Series(0).Points.AddXY(Math.Round(vectort(aux), 2), vectory1(aux))
        Chart2.Series(0).Points.AddXY(Math.Round(vectort(aux), 2), vectory2(aux))
        Chart3.Series(0).Points.AddXY(Math.Round(vectort(aux), 2), vectory3(aux))
        aux += 1
    End Sub
    Sub Registro_DG()
        Form2.DataGridView1.Rows.Add()
        Form2.DataGridView1.Item(0, Form2.DataGridView1.RowCount - 1).Value = CB1.Text
        Form2.DataGridView1.Item(1, Form2.DataGridView1.RowCount - 1).Value = Z.Text
        Form2.DataGridView1.Item(2, Form2.DataGridView1.RowCount - 1).Value = CB2.Text
        Form2.DataGridView1.Item(3, Form2.DataGridView1.RowCount - 1).Value = ZL.Text

        Dim bmp As New Bitmap(Chart1.Width, Chart1.Height)
        Chart1.DrawToBitmap(bmp, Chart1.DisplayRectangle)
        Form2.DataGridView1.Item(4, Form2.DataGridView1.RowCount - 1).Value = bmp

        Dim bmp2 As New Bitmap(Chart2.Width, Chart2.Height)
        Chart2.DrawToBitmap(bmp2, Chart2.DisplayRectangle)
        Form2.DataGridView1.Item(5, Form2.DataGridView1.RowCount - 1).Value = bmp2

        Dim bmp3 As New Bitmap(Chart3.Width, Chart3.Height)
        Chart3.DrawToBitmap(bmp3, Chart3.DisplayRectangle)
        Form2.DataGridView1.Item(6, Form2.DataGridView1.RowCount - 1).Value = bmp3

        Form2.MdiParent = FormPrincipal
        Form2.Show()

        proceso_sim = False
    End Sub
    Private Sub Form1_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Try
            sendOctave("exit")
        Catch ex As Exception

        End Try
    End Sub
    Sub Crear_Mfile()
        Dim escritor As StreamWriter
        Try
            My.Computer.FileSystem.DeleteFile(Application.StartupPath &
                                          "\POctave.m")
        Catch ex As Exception

        End Try
        escritor = File.AppendText(Application.StartupPath &
                                          "\POctave.m")
        escritor.Write("clc" & vbCrLf &
        "clear" & vbCrLf &
        "pkg load control" & vbCrLf &
        "s=tf('s');" & vbCrLf &
        "R=" & vr & ";" & vbCrLf &
        "L=" & vl & ";" & vbCrLf &
        "C=" & vc & ";" & vbCrLf &
        "Z=" & vz & ";" & vbCrLf &
        "ZL=" & vzl & ";" & vbCrLf &
        "V=" & voltaje & ";" & vbCrLf &
        "Z1=L*s+ZL;" & vbCrLf &
        "Z2=Z1/(1+C*s*Z1);" & vbCrLf &
        "ZT=Z+Z2+R;" & vbCrLf &
        "I=V/ZT;" & vbCrLf & 'Como los componentes están en serie, la I es igual es cada uno de ellos.
        "VZ2=I*Z2;" & vbCrLf &
        "IZ1=VZ2/Z1;" & vbCrLf & 'Como los componentes están en paralelo, el V es igual es cada uno de ellos.
        "IZL=IZ1;" & vbCrLf &
        "VZL=IZ1*ZL;" & vbCrLf & 'Como los componentes están en serie, la I es igual es cada uno de ellos.
        "GVZL=VZL/V;" & vbCrLf &
        "GIZL=IZL/I;" & vbCrLf &
        "PZL=VZL*IZL;" & vbCrLf &
        "P=V*I;" & vbCrLf &
        "GPZL=PZL/P;" & vbCrLf &
        "[v,t1]=" & tipoGraf & "(GVZL);" & vbCrLf &
        "c=length(t1);" & vbCrLf &
        "tiempo=t1(c)*1.1;" & vbCrLf &
        "[v,t1]=" & tipoGraf & "(GVZL,tiempo,tiempo/" & cant_elementos & ");" & vbCrLf &
        "dlmwrite('" & Application.StartupPath &
                    "\t1.txt',t1,'\n');" & vbCrLf &
        "dlmwrite('" & Application.StartupPath &
                    "\v.txt',v,'\n');" & vbCrLf &
        "[i,t1]=" & tipoGraf & "(GIZL);" & vbCrLf &
        "c=length(t1);" & vbCrLf &
        "tiempo=t1(c)*1.1;" & vbCrLf &
        "[i,t1]=" & tipoGraf & "(GIZL,tiempo,tiempo/" & cant_elementos & ");" & vbCrLf &
        "dlmwrite('" & Application.StartupPath &
                    "\t1.txt',t1,'\n');" & vbCrLf &
        "dlmwrite('" & Application.StartupPath &
                    "\i.txt',i,'\n');" & vbCrLf)
        escritor.Flush()
        escritor.Close()
    End Sub
End Class
