Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Drawing
Imports System.Threading

Public Class LowLevelMouseHook : Implements IDisposable

    Public Delegate Function LowLevelMouseProcDelegate(ByVal nCode As Int32, ByVal wParam As IntPtr, ByRef lParam As IntPtr) As Int32
    Public Delegate Function HookProc(ByVal nCode As Int32, ByVal wParam As IntPtr, ByRef lParam As IntPtr) As Int32

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure MSLLHOOKSTRUCT
        Public pt As Point
        Public mouseData As Integer
        Public flags As MSLLHOOKSTRUCTFlags
        Public time As UInt32
        Public dwExtraInfo As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
Public Class MouseHookStruct
        Public pt As Point
        Public hwnd As Integer
        Public wHitTestCode As Integer
        Public dwExtraInfo As Integer
    End Class

    <Flags()> _
    Public Enum MSLLHOOKSTRUCTFlags As Integer
        LLMHF_INJECTED = 1
    End Enum

    Private Const HC_ACTION As Integer = 0
    Private Const WH_MOUSE_LL As Integer = 14
    Private Const WM_LBUTTONDOWN As Integer = &H201
    Private Const WM_MOUSEWHEEL As Integer = &H20A

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function SetWindowsHookEx(ByVal idHook As Int32, ByVal lpfn As LowLevelMouseProcDelegate, ByVal hmod As IntPtr, ByVal dwThreadId As Int32) As Int32
        ' Leave function empty
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function CallNextHookEx(ByVal hHook As Int32, ByVal nCode As Int32, ByVal wParam As IntPtr, ByRef lParam As IntPtr) As Int32
        ' Leave function empty
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function UnhookWindowsHookEx(ByVal hHook As Int32) As Int32
        ' Leave function empty
    End Function

    Private hhkLowLevelMouse As Int32 = 0
    Private hHook As Int32 = 0
    Private disposedValue As Boolean = False        ' To detect redundant calls
    Private MouseHookProcedure As HookProc

    Public Sub New()
        DisableMouse()
    End Sub

    'set mouse hook
    Private Sub DisableMouse()
        'Set Mouse Hook
        hhkLowLevelMouse = SetWindowsHookEx(WH_MOUSE_LL, AddressOf LowLevelMouseProc, Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly.GetModules()(0)).ToInt32, 0)

        If hhkLowLevelMouse = 0 Then
            Debug.Print("mouse hook failed")
        Else
            Debug.Print("mouse hook success")
        End If
    End Sub

    Private Function LowLevelMouseProc(ByVal nCode As Int32, ByVal wParam As IntPtr, ByRef lParam As IntPtr) As Int32
        Dim MyMouseHookStruct As MouseHookStruct = DirectCast(Marshal.PtrToStructure(lParam, GetType(MouseHookStruct)), MouseHookStruct)

        If (nCode = HC_ACTION) Then
            If wParam = WM_LBUTTONDOWN Then
                ''do something on left mouse click
                Debug.Print("Left Mouse button")
            End If

            Return CallNextHookEx(hhkLowLevelMouse, nCode, wParam, lParam)
        End If
    End Function

    Private Sub DoHook()
        If hHook = 0 Then
            ' Create an instance of HookProc.
            MouseHookProcedure = New HookProc(AddressOf LowLevelMouseProc)

            hHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookProcedure, CType(0, IntPtr), Threading.Thread.CurrentThread.ManagedThreadId)
            'If the SetWindowsHookEx function fails.
            If hHook = 0 Then
                MessageBox.Show("SetWindowsHookEx Failed")
                Return
            End If
            button1.Text = "UnHook Windows Hook"
        Else
            Dim ret As Boolean = UnhookWindowsHookEx(hHook)
            'If the UnhookWindowsHookEx function fails.
            If ret = False Then
                MessageBox.Show("UnhookWindowsHookEx Failed")
                Return
            End If
            hHook = 0
            button1.Text = "Set Windows Hook"
            Me.Text = "Mouse Hook"
        End If

    End Sub

    'release hook
    Private Sub EnableMouse()
        'Release Mouse Hook
        UnhookWindowsHookEx(hhkLowLevelMouse)
    End Sub

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
                EnableMouse()
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
