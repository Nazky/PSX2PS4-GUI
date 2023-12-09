
Imports System.IO
Imports System.Net
Imports System.Text
Imports HtmlAgilityPack
Imports DiscUtils.Iso9660
Imports PSXRFD

Public Class ExtractPSX

    Public Shared Sub systemcmd(ByVal cmd As String, arg As String)
        Dim pHelp As New ProcessStartInfo
        pHelp.FileName = cmd
        pHelp.Arguments = arg
        pHelp.UseShellExecute = True
        pHelp.RedirectStandardError = True
        pHelp.RedirectStandardOutput = True
        pHelp.WindowStyle = ProcessWindowStyle.Hidden

        Dim proc As Process = Process.Start(pHelp)
        proc.WaitForExit()
    End Sub
    Shared Sub info(binpath As String, frm As Main)
        Try
            frm.Invoke(Sub() frm.UseWaitCursor = True)
            'frm.Invoke(Sub() frm.Button5.Enabled = False)

            If binpath.Contains(".bin") Or binpath.Contains(".BIN") Then
                'MsgBox(binpath)
                Try
                    Dim Res As String = PSXRFD.PSXRFD.IDFinder(binpath)

                    If Res = "" Then
                        MsgBox("No game id found.", MsgBoxStyle.Critical)
                    Else
                        frm.Invoke(Sub() frm.RichTextBox1.Text = frm.RichTextBox1.Text & vbCrLf & "Game id found: " & Res)
                    End If
                    If frm.HopeSwitch2.Checked = True Then
                        Try
                            getcov(Res.Replace("_", "-").Replace(".", ""), frm)
                        Catch ex As Exception

                        End Try

                    Else
                        frm.Invoke(Sub() frm.NotifyIcon1.BalloonTipIcon = ToolTipIcon.Error)
                        frm.Invoke(Sub() frm.NotifyIcon1.BalloonTipTitle = "Cover downloader")
                        frm.Invoke(Sub() frm.NotifyIcon1.BalloonTipText = "Auto cover download disable")
                    End If

                    frm.Invoke(Sub() frm.Label8.Text = Res.Replace("_", "").Replace(".", ""))
                    getn(Res.Replace("_", "-").Replace(".", ""), frm)
                    My.Settings.GID = Res.Replace("_", "-").Replace(".", "")
                    frm.Invoke(Sub() frm.PictureBox1.Visible = True)
                    frm.Invoke(Sub() frm.PictureBox2.Visible = True)
                    frm.Invoke(Sub() frm.PictureBox3.Visible = True)
                    frm.Invoke(Sub() frm.Label2.Visible = True)
                    frm.Invoke(Sub() frm.Label8.Visible = True)
                    frm.Invoke(Sub() frm.RichTextBox1.Visible = True)
                    frm.Invoke(Sub() frm.RoyalButton1.Visible = True)
                    frm.Invoke(Sub() frm.PoisonProgressBar1.Visible = True)
                    frm.Invoke(Sub() frm.BigLabel1.Visible = False)
                    frm.Invoke(Sub() frm.PictureBox3.AllowDrop = True)

                    frm.Invoke(Sub() frm.UseWaitCursor = False)


                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.Critical)

                End Try
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

    Shared Sub getcov(str As String, frm As Main)
        Try
            If File.Exists("bin/covers/" & str & ".jpg") Then
                frm.Invoke(Sub() frm.PictureBox1.Image = Image.FromFile("bin/covers/" & str & ".jpg"))
                frm.Invoke(Sub() frm.TextBox3.Text = Application.StartupPath & "\bin\covers\" & str & ".jpg")
            Else
                Dim wSource As String = "https://raw.githubusercontent.com/xlenore/psx-covers/main/covers/default/" & str & ".jpg"
                Dim cd As New WebClient
                cd.DownloadFile(wSource, "bin/covers/" & str & ".jpg")
                frm.Invoke(Sub() frm.PictureBox1.Image = Image.FromFile("bin/covers/" & str & ".jpg"))
                frm.Invoke(Sub() frm.TextBox3.Text = Application.StartupPath & "\bin\covers\" & str & ".jpg")

            End If

        Catch ex As Exception
            frm.Invoke(Sub() frm.NotifyIcon1.BalloonTipIcon = ToolTipIcon.Error)
            frm.Invoke(Sub() frm.NotifyIcon1.BalloonTipTitle = "Cover downloader")
            frm.Invoke(Sub() frm.NotifyIcon1.BalloonTipText = ex.Message)
            frm.Invoke(Sub() frm.NotifyIcon1.ShowBalloonTip(1000))

            'MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub


    Shared Sub getid(str As String, frm As Main)
        Try
            Dim sSource As String = str 'String that is being searched
            Dim sDelimStart As String = "cdrom:" 'First delimiting word
            Dim sDelimEnd As String = ";" 'Second delimiting word
            Dim nIndexStart As Integer = sSource.IndexOf(sDelimStart) 'Find the first occurrence of f1
            Dim nIndexEnd As Integer = sSource.IndexOf(sDelimEnd) 'Find the first occurrence of f2

            If nIndexStart > -1 AndAlso nIndexEnd > -1 Then '-1 means the word was not found.
                Dim res As String = Strings.Mid(sSource, nIndexStart + sDelimStart.Length + 1, nIndexEnd - nIndexStart - sDelimStart.Length).Replace("/", "").Replace("\", "") 'Crop the text between
                'MessageBox.Show(res.Replace("_", "-").Replace(".", "")) 'Display
                If frm.HopeSwitch2.Checked = True Then
                    getcov(res.Replace("_", "-").Replace(".", ""), frm)
                Else
                    frm.Invoke(Sub() frm.NotifyIcon1.BalloonTipIcon = ToolTipIcon.Error)
                    frm.Invoke(Sub() frm.NotifyIcon1.BalloonTipTitle = "Cover downloader")
                    frm.Invoke(Sub() frm.NotifyIcon1.BalloonTipText = "Auto cover download disable")
                End If

                frm.Invoke(Sub() frm.Label8.Text = res.Replace("_", "").Replace(".", ""))
                getn(res.Replace("_", "-").Replace(".", ""), frm)
                My.Settings.GID = res.Replace("_", "-").Replace(".", "")
            Else
                MessageBox.Show("One or both of the delimiting words were not found!")
            End If
        Catch ex As Exception
            'MsgBox(ex.Message, MsgBoxStyle.Critical, "PS22PS4-GUI")
        End Try

    End Sub

    Shared Sub getn(id As String, frm As Main)
        Try
            Dim wc As New WebClient()
            Dim html = wc.DownloadString("http://redump.org/discs/quicksearch/" & id)
            wc.Dispose()
            Dim htmlDoc As New HtmlDocument()
            htmlDoc.LoadHtml(html)
            If htmlDoc.DocumentNode.InnerHtml.Contains("h1") Then
                For Each h1Node In htmlDoc.DocumentNode.SelectNodes("//h1")
                    ' Do Something...
                    frm.Invoke(Sub() frm.Label2.Text = h1Node.InnerHtml.Replace(":", " -").Replace("/", "").Replace("\\", " ").Replace("?", "").Replace("*", "").Replace("|", "").Replace(">", "").Replace("<", ""))
                    My.Settings.GN = h1Node.InnerHtml.Replace(":", " -").Replace("/", "").Replace("\\", " ").Replace("?", "").Replace("*", "").Replace("|", "").Replace(">", "").Replace("<", "")
                Next
            Else
                For Each h1Node In htmlDoc.DocumentNode.SelectNodes("//td/a")
                    ' Do Something...
                    frm.Invoke(Sub() frm.Label2.Text = h1Node.InnerHtml.Replace(":", " -").Replace("/", "").Replace("\\", " ").Replace("?", "").Replace("*", "").Replace("|", "").Replace(">", "").Replace("<", ""))
                    My.Settings.GN = h1Node.InnerHtml.Replace(":", " -").Replace("/", "").Replace("\\", " ").Replace("?", "").Replace("*", "").Replace("|", "").Replace(">", "").Replace("<", "")
                Next
            End If

        Catch ex As Exception
            'MsgBox(ex.Message, MsgBoxStyle.Critical, "PS22PS4-GUI")
        End Try


    End Sub
End Class
