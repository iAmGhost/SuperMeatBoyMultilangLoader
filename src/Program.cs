using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using aejw;

namespace SuperMeatBoyMultilangLoader
{
    class Program
    {
        static string currentDir = AppDomain.CurrentDomain.BaseDirectory;
        static string currentExeNameNoExt = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);
        static string currentExeName = AppDomain.CurrentDomain.FriendlyName;

        static void Main(string[] args)
        {
            string iniPath = currentDir + "/" + currentExeNameNoExt + ".ini";

            Console.WriteLine("SuperMeatBoy Multilanguage Loader 1.0");
            Console.WriteLine("Programmed by iAmGhost");
            Console.WriteLine("Project Page: https://github.com/iAmGhost/SuperMeatBoyMultilangLoader");
            Console.WriteLine("----------------");

            string language, smbpath;

            language = getArgData(args, "language");
            smbpath = getArgData(args, "smbpath");

            if (args.Length == 0 && File.Exists(iniPath))
            {
                cIni ini = new cIni(iniPath);

                language = ini.ReadValue("LoaderOptions", "Language", "");
                smbpath = ini.ReadValue("LoaderOptions", "SMBPath", "");

                if ((language == "" || smbpath == ""))
                {
                    Console.WriteLine("Ini file example:");
                    Console.WriteLine("[LoaderOptions]");
                    Console.WriteLine("Language=Korean");
                    Console.WriteLine("SMBPath=SuperMeatBoy.exe");
                    printLanguages();
                    Environment.Exit(1);
                }
            }
            else
            {
                if ((language == null || smbpath == null) && !File.Exists(iniPath))
                {
                    Console.WriteLine("Usage:");
                    Console.WriteLine(currentExeName + " --language=Korean --smbpath=\"SuperMeatBoy.exe\"");
                    Console.WriteLine("Or create " + currentExeNameNoExt + ".ini.");
                    printLanguages();
                    Environment.Exit(0);
                }
            }

            if (Path.Equals(Path.GetFullPath(currentDir + currentExeName), Path.GetFullPath(smbpath)))
            {
                Console.WriteLine("SMBPath is same as this loader.");
                Console.WriteLine("Halted for prevent infinite loop.");
                Environment.Exit(1);
            }

            if (!File.Exists(smbpath))
            {
                Console.WriteLine("Cannot find Super Meat Boy executable: {0}", smbpath);
                Environment.Exit(1);
            }

            Console.WriteLine("Starting Loader...");

            SuperMeatBoyLanguagePatcher patcher = new SuperMeatBoyLanguagePatcher();
            patcher.Language = language;
            patcher.SuperMeatBoyPath = smbpath;

            Console.WriteLine("Language: {0} (0x{1:X2})", language, patcher.LanguageCode);

            patcher.Launch();

            Environment.Exit(0);
        }

        static string getArgData(string[] args, string key)
        {
            key = "--" + key + "=";
            string result = null;

            foreach (string arg in args)
            {
                if (arg.IndexOf(key) == 0)
                {
                    result = arg.Substring(key.Length, arg.Length - key.Length);
                }
            }

            return result;
        }

        static void printLanguages()
        {
            string[] langs = new SuperMeatBoyLanguagePatcher().LanguageNames;
            Console.WriteLine("----------------");
            Console.Write("Supported Languages: ");

            foreach (string lang in langs)
            {
                Console.Write(lang + ", ");
            }

            Console.WriteLine("");
        }
    }
}
