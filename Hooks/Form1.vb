Imports VB = Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility
Imports System.Runtime.InteropServices
Imports System.Text

Public Class Form1

    Public Delegate Function EnumWindowProcess(ByVal Handle As IntPtr, ByRef Parameter As IntPtr) As Boolean

    <DllImport("user32.dll", CharSet:=CharSet.Auto)> _
    Private Shared Function WindowFromPoint(ByVal Point As Point) As IntPtr
        ' Leave function empty 
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto)> _
    Private Shared Sub GetClassName(ByVal hWnd As Int32, ByVal lpClassName As System.Text.StringBuilder, ByVal nMaxCount As Int32)
        ' Leave function empty     
    End Sub

    <DllImport("user32.dll", CharSet:=CharSet.Auto)> _
    Public Shared Function GetParent(ByVal hwnd As Int32) As Int32
        ' Leave function empty  
    End Function

    <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
    Private Shared Function EnumChildWindows(ByVal WindowHandle As IntPtr, ByVal Callback As EnumWindowProcess, ByRef lParam As IntPtr) As Boolean
        ' Leave function empty  
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function GetWindowTextLength(ByVal hwnd As IntPtr) As Integer
        ' Leave function empty 
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function GetWindowText(ByVal hwnd As IntPtr, ByVal lpString As StringBuilder, ByVal cch As Integer) As Integer
        ' Leave function empty
    End Function

    Private m_Mouse As LowLevelMouseHook

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        m_Mouse.Dispose()
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_Mouse = New LowLevelMouseHook

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles Timer1.Tick
        Dim Counter As Int32 = 0
        Dim txt As String = ""

        txt &= "Position: " & MousePosition().ToString() & vbCrLf

        Dim window_handle As Integer = WindowFromPoint(New Point(MousePosition.X, MousePosition.Y)).ToString()
        txt &= "Window handle: " & window_handle & vbCrLf

        Dim root_handle As Integer = FindRoot(window_handle)
        txt &= "Root handle: " & root_handle & vbCrLf

        txt &= "Root text: " & vbCrLf & WindowText(root_handle) & vbCrLf
        Dim Children As IntPtr() = GetChildWindows(GetParent(root_handle))

        Debug.Print(txt)

        For Each Child As IntPtr In Children
            Dim ClassName As New StringBuilder("", 2565)

            Counter += 1
            GetClassName(Child, ClassName, 256)
            'Dim Text As String = "&H" & VB.Right("00000000" & Hex(Child), 8) & " - " & Chr(34) & ClassName.ToString & Chr(34)
            Dim Text As String = ClassName.ToString
            Debug.Print(Counter.ToString & ": " & Hex(Child.ToInt32) & " " & Text)
        Next
        'End If
    End Sub

    Public Shared Function GetChildWindows(ByVal ParentHandle As IntPtr) As IntPtr()
        Dim ChildrenList As New List(Of IntPtr)
        Dim ListHandle As GCHandle = GCHandle.Alloc(ChildrenList)

        Try
            EnumChildWindows(ParentHandle, AddressOf EnumWindow, GCHandle.ToIntPtr(ListHandle))
        Finally
            If ListHandle.IsAllocated Then ListHandle.Free()
        End Try

        Return ChildrenList.ToArray
    End Function

    Private Shared Function EnumWindow(ByVal Handle As IntPtr, ByRef Parameter As IntPtr) As Boolean
        Dim ChildrenList As List(Of IntPtr) = GCHandle.FromIntPtr(Parameter).Target

        If ChildrenList Is Nothing Then Throw New Exception("GCHandle Target could not be cast as List(Of IntPtr)")
        ChildrenList.Add(Handle)

        Return True
    End Function

    Private Function FindRoot(ByVal hWnd As Int32) As Int32
        Do
            Dim parent_hwnd As Int32 = GetParent(hWnd)
            If parent_hwnd = 0 Then Return hWnd
            hWnd = parent_hwnd
        Loop
    End Function

    Private Function WindowText(ByVal hWnd As Int32) As String
        If hWnd = 0 Then Return ""

        Dim text_len As Integer = GetWindowTextLength(hWnd)
        If text_len = 0 Then Return ""

        Dim sb As New System.Text.StringBuilder(text_len + 1)
        Dim ret = GetWindowText(hWnd, sb, sb.Capacity)
        If ret = 0 Then Return ""

        Return sb.ToString
    End Function

End Class
