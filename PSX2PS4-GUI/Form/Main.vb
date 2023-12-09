Imports System.IO
Imports System.Reflection.Emit
Imports System.Threading

Public Class Main
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        HopeSwitch2.Checked = My.Settings.AC
        HopeSwitch3.Checked = My.Settings.AP
        HopeSwitch8.Checked = My.Settings.P5C
        PictureBox1.AllowDrop = True
        PictureBox2.AllowDrop = True
        checknet()
        Directory.CreateDirectory("bin\lang")
        'Directory.CreateDirectory("bin\configs")
        'Directory.CreateDirectory("bin\LUA")
        Directory.CreateDirectory("bin\covers")
        Directory.CreateDirectory("bin\emulators")
        'Directory.CreateDirectory("bin\info")

    End Sub

    Sub checknet()
        Try
            IO.File.WriteAllText("s.bat", "dotnet --list-runtimes > r.txt")
            Shell("s.bat", AppWinStyle.Hide, True)
            Dim r As String = IO.File.ReadAllText("r.txt")
            If r.Contains("Microsoft.NETCore.App 6") Then
                IO.File.Delete("r.txt")
                IO.File.Delete("s.bat")
            Else
                MsgBox(".net 6.0 Runtime not found please install it first !", MsgBoxStyle.Critical)
                Me.Invoke(Sub() Me.WindowState = FormWindowState.Minimized)
                Shell("winget install Microsoft.DotNet.DesktopRuntime.6 -h", AppWinStyle.NormalFocus, True)
                Me.Invoke(Sub() Me.WindowState = FormWindowState.Normal)

                IO.File.Delete("r.txt")
                IO.File.Delete("s.bat")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            MsgBox("Can't install .NET 6 using winget" & vbCr & "Please intall the Desktop Runtime manually.", MsgBoxStyle.Critical)
            Process.Start("https://download.visualstudio.microsoft.com/download/pr/4c5e26cf-2512-4518-9480-aac8679b0d08/523f1967fd98b0cf4f9501855d1aa063/windowsdesktop-runtime-6.0.13-win-x64.exe")
            IO.File.Delete("r.txt")
            IO.File.Delete("s.bat")
            'Me.Close()
        End Try


    End Sub

    Private Sub TextBox1_DragEnter(sender As Object, e As DragEventArgs) Handles TextBox1.DragEnter

    End Sub

    Private Sub TextBox1_DragDrop(sender As Object, e As DragEventArgs) Handles TextBox1.DragDrop


    End Sub

    Private Sub ToolStripComboBox1_LostFocus(sender As Object, e As EventArgs) Handles ToolStripComboBox1.LostFocus
        My.Settings.KI = ToolStripComboBox1.Text
        My.Settings.Save()
    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs)
        RichTextBox1.SelectionStart = RichTextBox1.Text.Length
        RichTextBox1.ScrollToCaret()
    End Sub

    Private Sub BigLabel1_DragDrop(sender As Object, e As DragEventArgs) Handles BigLabel1.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        Dim i = 0
        TextBox1.Text = ""
        'Label15.Text = "Disc number: 0"
        For Each path In files
            If i = 5 Then
                MsgBox("5 ISO/BIN LIMIT !!!", MsgBoxStyle.Critical, "PSX2PS4-GUI")
            Else
                TextBox1.Text = TextBox1.Text & files(i) & vbCrLf
                i += 1
            End If

        Next
        Dim fl = Split(TextBox1.Text, vbCrLf)
        'Label15.Text = Label15.Text.Replace("Disc number: 0", "Disc number: " & fl.Length - 1)
        Dim b As Thread = New Thread(Sub() ExtractPSX.info(fl(0), Me))
        b.IsBackground = True
        b.SetApartmentState(ApartmentState.STA)
        b.Start()
    End Sub

    Private Sub BigLabel1_DragEnter(sender As Object, e As DragEventArgs) Handles BigLabel1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub TabPage5_DragDrop(sender As Object, e As DragEventArgs) Handles TabPage5.DragDrop

    End Sub

    Private Sub TabPage5_DragEnter(sender As Object, e As DragEventArgs) Handles TabPage5.DragEnter

    End Sub

    Private Sub RichTextBox1_TextChanged_1(sender As Object, e As EventArgs) Handles RichTextBox1.TextChanged
        RichTextBox1.SelectionStart = RichTextBox1.Text.Length
        RichTextBox1.ScrollToCaret()
    End Sub

    Private Sub PictureBox3_DragDrop(sender As Object, e As DragEventArgs) Handles PictureBox3.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        Dim i = 0
        TextBox1.Text = ""
        'Label15.Text = "Disc number: 0"
        For Each path In files
            If i = 5 Then
                MsgBox("5 ISO/BIN LIMIT !!!", MsgBoxStyle.Critical, "PSX2PS4-GUI")
            Else
                TextBox1.Text = TextBox1.Text & files(i) & vbCrLf
                i += 1
            End If

        Next
        Dim fl = Split(TextBox1.Text, vbCrLf)
        'Label15.Text = Label15.Text.Replace("Disc number: 0", "Disc number: " & fl.Length - 1)
        Dim b As Thread = New Thread(Sub() ExtractPSX.info(fl(0), Me))
        b.IsBackground = True
        b.SetApartmentState(ApartmentState.STA)
        b.Start()
    End Sub

    Private Sub PictureBox3_DragEnter(sender As Object, e As DragEventArgs) Handles PictureBox3.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click

    End Sub

    Private Sub BigLabel1_DoubleClick(sender As Object, e As EventArgs) Handles BigLabel1.DoubleClick

    End Sub

    Private Sub LinkLabel9_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel9.LinkClicked
        Process.Start("https://github.com/xlenore")
    End Sub

    Private Sub LinkLabel8_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Process.Start("https://github.com/extramaster")
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Process.Start("https://github.com/hessu")
    End Sub

    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Process.Start("https://github.com/a1346054")
    End Sub

    Private Sub LinkLabel5_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Process.Start("https://github.com/yanalunaterra")
    End Sub

    Private Sub LinkLabel6_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Process.Start("https://github.com/h3xx")
    End Sub

    Private Sub LinkLabel7_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Process.Start("https://github.com/amroamroamro")
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Process.Start("https://twitter.com/Markus00095")
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://twitter.com/NazkyYT")
        Process.Start("https://ko-fi.com/nazkyyt")
    End Sub


    Private Sub HopeSwitch5_CheckedChanged(sender As Object, e As EventArgs) Handles HopeSwitch5.CheckedChanged
        If HopeSwitch5.Checked = True Then
            MsgBox("In the next update ;)", MsgBoxStyle.Information)
            HopeSwitch5.Checked = False
        End If
    End Sub

    Private Sub HopeSwitch4_CheckedChanged(sender As Object, e As EventArgs) Handles HopeSwitch4.CheckedChanged
        If HopeSwitch4.Checked = True Then
            MsgBox("In the next update ;)", MsgBoxStyle.Information)
            HopeSwitch4.Checked = False
        End If
    End Sub

    Private Sub HopeSwitch1_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub HopeSwitch2_CheckedChanged(sender As Object, e As EventArgs) Handles HopeSwitch2.CheckedChanged

    End Sub

    Private Sub HopeSwitch2_Click(sender As Object, e As EventArgs) Handles HopeSwitch2.Click
        If HopeSwitch2.Checked = False Then
            My.Settings.AC = False
        Else
            My.Settings.AC = True
        End If
        My.Settings.Save()
    End Sub

    Private Sub PictureBox1_DragDrop(sender As Object, e As DragEventArgs) Handles PictureBox1.DragDrop
        Try
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                e.Effect = DragDropEffects.Copy
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub PictureBox1_DragEnter(sender As Object, e As DragEventArgs) Handles PictureBox1.DragEnter
        Try
            Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
            For Each path In files

                PictureBox1.Image = Image.FromFile(path)
            Next
        Catch ex As Exception

        End Try

    End Sub

    Private Sub PictureBox2_DragDrop(sender As Object, e As DragEventArgs) Handles PictureBox2.DragDrop
        Try
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                e.Effect = DragDropEffects.Copy
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub PictureBox2_DragEnter(sender As Object, e As DragEventArgs) Handles PictureBox2.DragEnter
        Try
            Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
            For Each path In files

                PictureBox2.Image = Image.FromFile(path)
            Next
        Catch ex As Exception

        End Try

    End Sub

    Private Sub HopeSwitch3_CheckedChanged(sender As Object, e As EventArgs) Handles HopeSwitch3.CheckedChanged

    End Sub

    Private Sub BigLabel1_Click(sender As Object, e As EventArgs) Handles BigLabel1.Click

    End Sub

    Private Sub RoyalButton1_Click(sender As Object, e As EventArgs) Handles RoyalButton1.Click
        Try
            Dim b As Thread = New Thread(Sub() PSX2PS4.CreatePKG(TextBox1.Text, My.Settings.GID, My.Settings.GN, RichTextBox1, Me))
            b.IsBackground = True
            b.SetApartmentState(ApartmentState.STA)
            b.Start()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub HopeSwitch8_CheckedChanged(sender As Object, e As EventArgs) Handles HopeSwitch8.CheckedChanged

    End Sub

    Private Sub HopeSwitch3_Click(sender As Object, e As EventArgs) Handles HopeSwitch3.Click
        If HopeSwitch3.Checked = True Then
            My.Settings.KI = True
        Else
            My.Settings.KI = False
        End If
        My.Settings.Save()
    End Sub

    Private Sub HopeSwitch8_Click(sender As Object, e As EventArgs) Handles HopeSwitch8.Click
        If HopeSwitch8.Checked = True Then
            My.Settings.P5C = True
            MsgBox("This is a experimental settings using a old ps1 emulator, compatibility are maybe not the best ! but at least it's working on PS5", MsgBoxStyle.Exclamation)
        Else
            My.Settings.P5C = False
        End If
        My.Settings.Save()
    End Sub

    Private Sub PictureBox1_DoubleClick(sender As Object, e As EventArgs) Handles PictureBox1.DoubleClick
        Try
            Dim ic As New OpenFileDialog
            ic.Filter = "Game cover |*.png|*.jpg"
            ic.Title = "Select a cover for the game"
            If ic.ShowDialog = DialogResult.OK Then
                PictureBox1.Image = Image.FromFile(ic.FileName)
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
