Imports System.Drawing.Imaging
Imports System.IO
Imports DiscUtils.OpticalDisk
Imports System.Web

Public Class PSX2PS4

    <STAThread>
    Shared Sub CreatePKG(BIN As String, gid As String, gn As String, rcht As RichTextBox, frm As Main)
        Dim pkgf As String
        Dim Icon As String
        Dim background As String
        If frm.InvokeRequired Then
            'MsgBox("test")
            Try
                If frm.HopeSwitch3.Checked = True Then
                    Dim sp As New SaveFileDialog
                    sp.Filter = "PS4 PKG (*.pkg)|*.pkg"
                    sp.Title = "Select where to save the PKG"
                    sp.FileName = gn
                    sp.AddExtension = True
                    If sp.ShowDialog = DialogResult.OK Then
                        pkgf = sp.FileName
                    Else
                        pkgf = Environment.CurrentDirectory & "\PSX-FPKG"
                    End If
                Else
                    pkgf = Environment.CurrentDirectory & "\PSX-FPKG"
                End If
                frm.Invoke(Sub() frm.NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info)
                frm.Invoke(Sub() frm.NotifyIcon1.BalloonTipTitle = "Creating PS4 PKG")
                frm.Invoke(Sub() frm.NotifyIcon1.BalloonTipText = "Sit back and relax, i inform you when it's done ;)")
                frm.Invoke(Sub() frm.NotifyIcon1.ShowBalloonTip(1000))
                frm.Invoke(Sub() frm.PoisonProgressBar1.Value = 0)
                frm.Invoke(Sub() frm.UseWaitCursor = True)
                frm.Invoke(Sub() frm.MenuStrip1.Enabled = False)
                frm.PictureBox1.Image.Save("cover.jpg", ImageFormat.Jpeg)
                Icon = Environment.CurrentDirectory & "\cover.jpg"
                frm.PictureBox2.Image.Save("back.jpg", ImageFormat.Jpeg)
                background = Environment.CurrentDirectory & "\back.jpg"
                If Directory.Exists(pkgf & "\Temp") Then
                    Directory.Delete(pkgf & "\Temp", True)
                End If
                frm.Invoke(Sub() rcht.Text = "BIN list: " & vbCrLf & BIN & vbCrLf & "Game ID: " & gid & vbCrLf & "Game Name: " & gn & vbCrLf & vbCrLf & "Icon path: " & Icon & vbCrLf & "Background path: " & background & vbCrLf & vbCrLf)

                dpkg(rcht, pkgf, frm)
                cpsx(rcht, BIN, pkgf, frm)
                setid(rcht, gid, pkgf, frm)
                setgn(rcht, gn, pkgf, frm)
                setc(rcht, Icon, pkgf, frm)
                setb(rcht, background, pkgf, frm)
                compilpkg(rcht, pkgf, gn, frm)

                frm.Invoke(Sub() frm.PoisonProgressBar1.Value = 100)
                frm.Invoke(Sub() frm.UseWaitCursor = False)
                frm.Invoke(Sub() frm.MenuStrip1.Enabled = True)


            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical)
            End Try
        End If


    End Sub

    Shared Sub dpkg(rcht As RichTextBox, pkgf As String, frm As Main)
        Try
            If frm.HopeSwitch8.Checked = True Then
                frm.Invoke(Sub() rcht.Text = rcht.Text & "Decrypting emulator (PS5 compatibility enabled)..." & vbCrLf)
                System.IO.Directory.CreateDirectory(pkgf & "\Temp")
                systemcmd("bin\tools\orbis-pub-cmd.exe", "img_extract --passcode 00000000000000000000000000000000 .\bin\emulators\emups5.pkg " & Chr(34) & pkgf & "\Temp" & Chr(34))
                My.Computer.FileSystem.RenameDirectory(pkgf & "\Temp\Sc0", "sce_sys")
                System.IO.File.Move(pkgf & "\Temp\sce_sys\param.sfo", pkgf & "\Temp\image0\sce_sys\param.sfo")
                System.IO.Directory.Delete(pkgf & "\Temp\sce_sys", True)
                frm.Invoke(Sub() frm.PoisonProgressBar1.Value += 11)
            Else
                frm.Invoke(Sub() rcht.Text = rcht.Text & "Decrypting emulator..." & vbCrLf)
                System.IO.Directory.CreateDirectory(pkgf & "\Temp")
                systemcmd("bin\tools\orbis-pub-cmd.exe", "img_extract --passcode 00000000000000000000000000000000 .\bin\emulators\emups4.pkg " & Chr(34) & pkgf & "\Temp" & Chr(34))
                My.Computer.FileSystem.RenameDirectory(pkgf & "\Temp\Sc0", "sce_sys")
                System.IO.File.Move(pkgf & "\Temp\sce_sys\param.sfo", pkgf & "\Temp\image0\sce_sys\param.sfo")
                System.IO.Directory.Delete(pkgf & "\Temp\sce_sys", True)
                frm.Invoke(Sub() frm.PoisonProgressBar1.Value += 11)
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try


    End Sub

    Shared Sub cpsx(rcht As RichTextBox, BIN As String, pkgf As String, frm As Main)
        Try
            If BIN.Contains(".bin") Or BIN.Contains(".BIN") Then
                frm.Invoke(Sub() rcht.Text = rcht.Text & "Importing BIN and CUE..." & vbCrLf)
                Dim fl = Split(BIN, vbCrLf)
                Dim i = 0
                Dim D = 1
                For Each str As String In fl(i)
                    If fl(i) = "" Then
                    Else
                        If frm.HopeSwitch8.Checked = True Then
                            frm.Invoke(Sub() rcht.Text = rcht.Text & "Copying CD" & D & "..." & vbCrLf)
                            System.IO.File.Copy(fl(i), pkgf & "\Temp\image0\SIEA\data\CD" & D & ".bin")
                            frm.Invoke(Sub() rcht.Text = rcht.Text & "Copying CUE from CD" & D & "..." & vbCrLf)
                            System.IO.File.Copy(fl(i).Replace(".bin", ".cue"), pkgf & "\Temp\image0\SIEA\data\CD" & D & ".cue")
                        Else
                            frm.Invoke(Sub() rcht.Text = rcht.Text & "Copying CD" & D & "..." & vbCrLf)
                            System.IO.File.Copy(fl(i), pkgf & "\Temp\image0\data\CD" & D & ".bin")
                            frm.Invoke(Sub() rcht.Text = rcht.Text & "Copying CUE from CD" & D & "..." & vbCrLf)
                            System.IO.File.Copy(fl(i).Replace(".bin", ".cue"), pkgf & "\Temp\image0\data\CD" & D & ".cue")
                            Dim appbe As String = System.IO.File.ReadAllText(pkgf & "\Temp\image0\SIEA\app_boot.lua")
                            If appbe.Contains("Markus95") Then
                                File.WriteAllText(pkgf & "\Temp\image0\SIEA\app_boot.lua", appbe.Replace("Markus95", "CD" & D))
                            ElseIf appbe.Contains("--enable-change-disc-ui=true") Then
                            Else

                            End If
                        End If
                        i += 1
                        D += 1
                    End If
                Next
                frm.Invoke(Sub() rcht.Text = rcht.Text & "Enable change disc UI..." & vbCrLf)
                Dim confd As String = System.IO.File.ReadAllText(pkgf & "\Temp\image0\SIEA\config-region.txt")
                If confd.Contains("--enable-change-disc-ui=false") Then
                    File.WriteAllText(pkgf & "\Temp\image0\SIEA\config-region.txt", confd.Replace("--enable-change-disc-ui=false", "--enable-change-disc-ui=true"))
                ElseIf confd.Contains("--enable-change-disc-ui=true") Then
                Else
                    File.AppendAllText(pkgf & "\Temp\image0\SIEA\config-region.txt", vbCrLf & "--enable-change-disc-ui=true")
                End If
                frm.Invoke(Sub() rcht.Text = rcht.Text & "Edit app_boot..." & vbCrLf)

                frm.Invoke(Sub() frm.PoisonProgressBar1.Value += 11)
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

    Public Shared Function stringToByteArray(text As String) As Byte()
        Dim bytes As Byte() = New Byte(text.Length \ 2 - 1) {}

        For i As Integer = 0 To text.Length - 1 Step 2
            bytes(i \ 2) = Byte.Parse(text(i).ToString() & text(i + 1).ToString(), System.Globalization.NumberStyles.HexNumber)
        Next

        Return bytes
    End Function

    Shared Sub AppendByteToBIN(ByVal FilepathToAppendTo As String, ByRef Content() As Byte)
        Dim s As New System.IO.FileStream(FilepathToAppendTo, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite)
        s.Write(Content, 0, Content.Length)
        s.Close()
    End Sub

    Shared Function GetCRC32(ByVal sFileName As String) As String
        Try
            Dim FS As FileStream = New FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.Read, 8192)
            Dim CRC32Result As Integer = &HFFFFFFFF
            Dim Buffer(4096) As Byte
            Dim ReadSize As Integer = 4096
            Dim Count As Integer = FS.Read(Buffer, 0, ReadSize)
            Dim CRC32Table(256) As Integer
            Dim DWPolynomial As Integer = &HEDB88320
            Dim DWCRC As Integer
            Dim i As Integer, j As Integer, n As Integer
            'Create CRC32 Table
            For i = 0 To 255
                DWCRC = i
                For j = 8 To 1 Step -1
                    If (DWCRC And 1) Then
                        DWCRC = ((DWCRC And &HFFFFFFFE) \ 2&) And &H7FFFFFFF
                        DWCRC = DWCRC Xor DWPolynomial
                    Else
                        DWCRC = ((DWCRC And &HFFFFFFFE) \ 2&) And &H7FFFFFFF
                    End If
                Next j
                CRC32Table(i) = DWCRC
            Next i
            'Calcualting CRC32 Hash
            Do While (Count > 0)
                For i = 0 To Count - 1
                    n = (CRC32Result And &HFF) Xor Buffer(i)
                    CRC32Result = ((CRC32Result And &HFFFFFF00) \ &H100) And &HFFFFFF
                    CRC32Result = CRC32Result Xor CRC32Table(n)
                Next i
                Count = FS.Read(Buffer, 0, ReadSize)
            Loop
            FS.Close()
            Return Hex(CRC32Result)
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Shared Sub setid(rcht As RichTextBox, gid As String, pkgf As String, frm As Main)
        Try
            frm.Invoke(Sub() rcht.Text = rcht.Text & "Patching game id..." & vbCrLf)
            System.IO.Directory.CreateDirectory(pkgf & "\Temp")
            'MsgBox("""" & pkgf & "\Temp\image0\sce_sys\param.sfo"" " & "--address 0x31F --text " & """" & My.Settings.GID.Replace("-", "") & """")
            systemcmd("bin\tools\replhex.exe", """" & pkgf & "\Temp\image0\sce_sys\param.sfo"" " & "--address 0x35B --text " & """" & My.Settings.GID.Replace("-", "").Replace("SLES", "").Replace("SCES", "").Replace("SLUS", "").Replace("SCUS", "").Replace("SLPS", "").Replace("SCPS", "") & """")
            systemcmd("bin\tools\replhex.exe", """" & pkgf & "\Temp\image0\sce_sys\param.sfo"" " & "--address 0x730 --text " & """" & My.Settings.GID.Replace("-", "").Replace("SLES", "").Replace("SCES", "").Replace("SLUS", "").Replace("SCUS", "").Replace("SLPS", "").Replace("SCPS", "") & """")
            frm.Invoke(Sub() frm.PoisonProgressBar1.Value += 11)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Shared Sub setgn(rcht As RichTextBox, gn As String, pkgf As String, frm As Main)
        Try
            frm.Invoke(Sub() rcht.Text = rcht.Text & "Patching game name..." & vbCrLf)
            systemcmd("bin\tools\replhex.exe", """" & pkgf & "\Temp\image0\sce_sys\param.sfo"" " & "--address 0x620 --hex " & """00000000000000000000000000000000""")
            systemcmd("bin\tools\replhex.exe", """" & pkgf & "\Temp\image0\sce_sys\param.sfo"" " & "--address 0x630 --hex " & """00000000000000000000000000000000""")
            systemcmd("bin\tools\replhex.exe", """" & pkgf & "\Temp\image0\sce_sys\param.sfo"" " & "--address 0x62C --text " & """" & gn.Replace("Game name: ", "") & """")
            frm.Invoke(Sub() frm.PoisonProgressBar1.Value += 11)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Shared Sub setc(rcht As RichTextBox, icon As String, pkgf As String, frm As Main)
        Try
            frm.Invoke(Sub() rcht.Text = rcht.Text & "Patching game cover..." & vbCrLf)
            'System.IO.File.Copy(icon, pkgf & "\Temp\image0\sce_sys\icon0.png")
            systemcmd("bin\tools\magick.exe", "convert  " & Chr(34) & icon & Chr(34) & " " & Chr(34) & pkgf & "\Temp\image0\sce_sys\icon0.jpg" & Chr(34))
            systemcmd("bin\tools\magick.exe", "convert  " & Chr(34) & pkgf & "\Temp\image0\sce_sys\icon0.jpg" & Chr(34) & " " & Chr(34) & pkgf & "\Temp\image0\sce_sys\icon0.png" & Chr(34))
            systemcmd("bin\tools\magick.exe", "mogrify -resize 512x512!  " & Chr(34) & pkgf & "\Temp\image0\sce_sys\icon0.png" & Chr(34))
            System.IO.File.Copy(pkgf & "\Temp\image0\sce_sys\icon0.png", pkgf & "\Temp\image0\sce_sys\save_data.png")
            systemcmd("bin\tools\magick.exe", "mogrify -resize 228x128!  " & pkgf & "\Temp\image0\sce_sys\save_data.png")
            frm.Invoke(Sub() frm.PoisonProgressBar1.Value += 11)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Shared Sub setb(rcht As RichTextBox, back As String, pkgf As String, frm As Main)
        Try
            frm.Invoke(Sub() rcht.Text = rcht.Text & "Patching game background..." & vbCrLf)
            'System.IO.File.Copy(icon, pkgf & "\Temp\image0\sce_sys\icon0.png")
            systemcmd("bin\tools\magick.exe", "convert  " & Chr(34) & back & Chr(34) & " " & Chr(34) & pkgf & "\Temp\image0\sce_sys\pic1.jpg" & Chr(34))
            systemcmd("bin\tools\magick.exe", "mogrify -resize 1920x1080! " & Chr(34) & pkgf & "\Temp\image0\sce_sys\pic1.jpg" & Chr(34))
            systemcmd("bin\tools\magick.exe", "convert  " & Chr(34) & pkgf & "\Temp\image0\sce_sys\pic1.jpg" & Chr(34) & " " & Chr(34) & pkgf & "\Temp\image0\sce_sys\pic1.png" & Chr(34))
            frm.Invoke(Sub() frm.PoisonProgressBar1.Value += 11)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Shared Sub compilpkg(rcht As RichTextBox, pkgf As String, gn As String, frm As Main)
        Try
            frm.Invoke(Sub() rcht.Text = rcht.Text & "Compiling PKG..." & vbCrLf)
            systemcmd("bin\tools\gengp4.exe", Chr(34) & pkgf & "\Temp\image0" & Chr(34))
            'My.Computer.FileSystem.RenameFile(pkgf & "\Temp\image0.gp4", "image0.txt")
            Dim gp4 As String = System.IO.File.ReadAllText(pkgf & "\Temp\image0.gp4")
            System.IO.File.WriteAllText(pkgf & "\Temp\image0.gp4", gp4.Replace("<scenarios default_id=""1"">", "<scenarios default_id=""0"">"))
            systemcmd("bin\tools\orbis-pub-cmd.exe", "img_create " & Chr(34) & pkgf & "\Temp\image0.gp4" & Chr(34) & " " & Chr(34) & pkgf & "\" & My.Settings.GN & "_" & My.Settings.GID & ".pkg" & Chr(34))
            'My.Computer.FileSystem.RenameFile(pkgf & "\Temp\image0.gp4 " & pkgf & "\" & "1.pkg", My.Settings.GID & "_PS2.pkg")
            'System.IO.Directory.Delete(pkgf & "\Temp", True)
            frm.Invoke(Sub() rcht.Text = rcht.Text & "PKG compiled." & vbCrLf)
            frm.Invoke(Sub() frm.NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info)
            frm.Invoke(Sub() frm.NotifyIcon1.BalloonTipTitle = "PKG compiled")
            frm.Invoke(Sub() frm.NotifyIcon1.BalloonTipText = "Game : " & My.Settings.GN & vbCrLf & "PKG : " & pkgf & "\" & My.Settings.GN & "_" & My.Settings.GID & ".pkg")
            frm.Invoke(Sub() frm.NotifyIcon1.ShowBalloonTip(1000))
            System.IO.Directory.Delete(pkgf & "\Temp", True)
            System.IO.File.Delete("back.jpg")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

    Shared Sub systemcmd(ByVal cmd As String, arg As String)
        Dim pHelp As New ProcessStartInfo
        pHelp.FileName = cmd
        pHelp.Arguments = arg
        pHelp.UseShellExecute = True
        pHelp.WindowStyle = ProcessWindowStyle.Hidden

        Dim proc As Process = Process.Start(pHelp)
        proc.WaitForExit()
    End Sub
End Class
