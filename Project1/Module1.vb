Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports 屏幕反色
Module Module1
    Private App As AppDomain = AppDomain.CurrentDomain
    Function 加载屏幕反色模块(sender As Object, e As ResolveEventArgs) As Assembly
        If New AssemblyName(e.Name).Name = "屏幕反色" Then
            Return Assembly.Load(My.Resources.屏幕反色)
        End If
    End Function
    Sub New()
        AddHandler App.AssemblyResolve, AddressOf Module1.加载屏幕反色模块
    End Sub
    Sub Main()
        Do
            Dim www As Graphics = Graphics.FromHdc(ApiClass.GetWindowDC(ApiClass.GetDesktopWindow))
            Dim scr As Windows.Forms.Screen = Screen.PrimaryScreen
            Dim temp1 As New Bitmap(scr.WorkingArea.Width, scr.WorkingArea.Height)
            Dim ww As Graphics = Graphics.FromImage(temp1)
            ww.CopyFromScreen(New Point, New Point, scr.WorkingArea.Size)
            Dim handle As IntPtr = Class1.PContray(temp1).GetHicon
            www.DrawIcon(Icon.FromHandle(handle), scr.WorkingArea)
            Threading.Thread.Sleep(1000)
        Loop
    End Sub
End Module
