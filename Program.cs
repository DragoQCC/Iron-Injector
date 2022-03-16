using System;


namespace Iron_Injector
{
    internal class Program
    {
        public static string prompt = "[Iron]>";
        public static string OrginalPrompt = "[Iron]>";
        static void Main(string[] args)
        {
            string input = "";
            UI.ui.Banner();
            Console.WriteLine("Use help to see valid commands.");

            while (true)
            {
                Console.Write("[Iron]> ");
                input = Console.ReadLine();
                if (input != "exit")
                {
                    UI.ui.callCommand(input);
                }
                else
                {
                    Console.WriteLine("bye");
                    break;
                }
            }
            Environment.Exit(0);
        }
    }
}
