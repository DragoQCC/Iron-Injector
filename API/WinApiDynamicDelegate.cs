using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Iron_Injector;

namespace Iron_Injector.API
{
    public class WinApiDynamicDelegate
    {
        // nt create section
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public delegate uint NtCreateSection(ref IntPtr SectionHandle, uint DesiredAccess, IntPtr ObjectAttributes, ref ulong MaximumSize, uint SectionPageProtection, uint AllocationAttributes, IntPtr FileHandle);
        // nt map view of section
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public delegate uint NtMapViewOfSection(IntPtr SectionHandle, IntPtr ProcessHandle, out IntPtr BaseAddress, IntPtr ZeroBits, IntPtr CommitSize, IntPtr SectionOffset, out ulong ViewSize, uint InheritDisposition, uint AllocationType, uint Win32Protect);
        // create process w
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public delegate bool CreateProcessW(string lpApplicationName, string lpCommandLine, ref WinAPIs.SECURITY_ATTRIBUTES lpProcessAttributes, ref WinAPIs.SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref WinAPIs.STARTUPINFO lpStartupInfo, out WinAPIs.PROCESS_INFORMATION lpProcessInformation);
        //write process mem
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesWritten);
        //VirtualProtect
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool VirtualProtect(IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);
        //Queue User APC
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate uint QueueUserAPC(IntPtr pfnAPC, IntPtr hThread, uint dwData);
        //resume thread
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate uint ResumeThread(IntPtr hThread);
        //create thread
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr param, uint dwCreationFlags, ref uint lpThreadId);
        //virtual alloc
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr VirtualAlloc(IntPtr lpStartAddr, uint size, uint flAllocationType, uint flProtect);
        //virtual alloc Ex
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        //wait for single object
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);
        //can we do Assembly Load(byte[])
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate byte[] Load(byte[] rawAssembly);
        //Load Library
        [UnmanagedFunctionPointer(CallingConvention.StdCall,CharSet =CharSet.Unicode)]
        public delegate IntPtr LoadLibraryW(string library);
        //get proc address
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GetProcAddress(IntPtr libPtr, string function);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool freeLibrary(IntPtr library);
    }
}
