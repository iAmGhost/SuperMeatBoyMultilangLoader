using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace SuperMeatBoyMultilangLoader
{
    class SuperMeatBoyLanguagePatcher
    {
        private Dictionary<string, byte> languages = new Dictionary<string, byte>();
        private string exePath;
        private string language;
        string[] languageNames = { "debug", "english", "japanese", "german", "french", "spanish",
                                         "italian", "korean", "chinese", "portuguese", "chinese-fontonly",
                                         "russian", "poilish"};

        public SuperMeatBoyLanguagePatcher()
        {
            for (int i = 0; i < languageNames.Length; i++)
            {
                languages.Add(languageNames[i], (byte)i);
            }
        }

        public string[] LanguageNames
        {
            get
            {
                return languageNames;
            }
        }

        public string Language
        {
            get
            {
                return language;
            }
            set
            {
                language = value.ToLower();
            }
        }

        public string SuperMeatBoyPath
        {
            get
            {
                return exePath;
            }
            set
            {
                exePath = value;
            }
        }

        public void Launch()
        {
            Process process = new Process();
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

            process.StartInfo.FileName = exePath;
            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
            process.Start();

            IntPtr baseAddress = (from ProcessModule mod in process.Modules
                                        where mod.ModuleName == Path.GetFileName(exePath)
                                        select mod.BaseAddress).SingleOrDefault();

            //SuperMeatBoy.exe+4490 = Get language code function
            IntPtr getLanguageAddress = baseAddress + 0x4490;

            //0xB8 = MOV EAX
            //0xC3 = RETN
            Console.WriteLine("Patching 0x{0:X8}...", (int)getLanguageAddress);
            byte[] newData = new byte[] { 0xb8, LanguageCodeInByte, 0x00, 0x00, 0x00, 0xC3};
            int writtenBytes;

            Memory memory = new Memory();
            memory.ReadProcess = process;
            memory.Open();

            memory.Write(getLanguageAddress, newData, out writtenBytes);
        }

        public int LanguageCode
        {
            get
            {
                byte code = 0x00;
                languages.TryGetValue(language, out code);

                return (int)code;
            }
        }

        public byte LanguageCodeInByte
        {
            get
            {
                byte code = 0x00;
                languages.TryGetValue(language, out code);

                return code;
            }
        }
    }
}
