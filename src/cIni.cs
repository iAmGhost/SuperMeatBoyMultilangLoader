/*==============================================================================================================
  
  [ cINI - INI API Legacy Class (Ascii) ]
  ---------------------------------------
  Copyright (c)2004-2007 aejw.com
  http://www.aejw.com/
  
Build:         0022 - Feb 2007

EULA:          Creative Commons - Attribution-ShareAlike 2.5
               http://creativecommons.org/licenses/by-sa/2.5/
               
Disclaimer:    THIS FILES / SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED 
               TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT 
               OWNER BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
               TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY 
               THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT 
               OF THE USE OF THIS FILES / SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. USE AT YOUR OWN RISK.

==============================================================================================================*/
using System;
using System.Runtime.InteropServices;

namespace aejw
{

    public class cIni
    {

        /// <summary>
        /// cINI Constructor
        /// </summary>
        /// <param name="IniFilename">File location of ini / inf file</param>
        public cIni(string iniFilename)
        {
            this._iniFilename = iniFilename;
        }


        /// <summary>
        /// INI filename (If no path is specifyed the function will look with-in the windows directory for the file)
        /// </summary>
        public string IniFile
        {
            get { return this._iniFilename; }
            set { this._iniFilename = value; }
        }


        /// <summary>
        /// Max return length when reading data (Max: 32767)
        /// </summary>
        public int BufferLen
        {
            get { return this._bufferLen; }
            set
            {
                if (value > 32767) { this._bufferLen = 32767; }
                else if (value < 1) { this._bufferLen = 1; }
                else { this._bufferLen = value; }
            }
        }


        /// <summary>
        /// Read value from INI File
        /// </summary>
        /// <param name="Section">Section to read value from</param>
        /// <param name="Key">Key name to read value from</param>
        /// <param name="Default">Default value returned if key is not found</param>
        public string ReadValue(string section, string key, string defaultValue)
        {

            return getString(section, key, defaultValue);

        }


        /// <summary>
        /// Read value from INI File, default = ""
        /// </summary>
        /// <param name="Section">Section to read value from</param>
        /// <param name="Key">Key name to read value from</param>
        public string ReadValue(string section, string key)
        {

            return getString(section, key, "");

        }


        /// <summary>
        /// Write value to INI File
        /// </summary>
        /// <param name="Section">Section to write value to</param>
        /// <param name="Key">Key name to write value to</param>
        /// <param name="Value">Value to write to key / setting</param>
        public void WriteValue(string section, string key, string value)
        {

            WritePrivateProfileString(section, key, value, this._iniFilename);

        }


        /// <summary>
        /// Remove value from INI File
        /// </summary>
        /// <param name="Section">Section to remove key from</param>
        /// <param name="Key">Key name to remove</param>
        public void RemoveValue(string section, string key)
        {

            WritePrivateProfileString(section, key, null, this._iniFilename);

        }


        /// <summary>
        /// Read values in a section from Ini/Inf File
        /// </summary>
        /// <param name="Section">Section to read value names from</param>		
        /// <returns>Array of keys names within Ini/Inf</returns>
        public string[] ReadKeys(string section)
        {

            return getString(section, null, null).Split((char)0);

        }


        /// <summary>
        /// Read sections from Ini File
        /// </summary>		
        /// <returns>Array of section names within Ini/Inf</returns>
        public string[] ReadSections()
        {

            return getString(null, null, null).Split((char)0);

        }


        /// <summary>
        /// Remove section from INI File
        /// </summary>
        /// <param name="Section">Section to remove</param>
        public void RemoveSection(string section)
        {

            WritePrivateProfileString(section, null, null, this._iniFilename);

        }


        #region vars, private functions and api calls

        private string _iniFilename = null;
        private int _bufferLen = 512;

        [DllImport("kernel32", SetLastError = true)]
        private static extern int WritePrivateProfileString(string pSection, string pKey, string pValue, string pFile);
        [DllImport("kernel32", SetLastError = true)]
        private static extern int WritePrivateProfileStruct(string pSection, string pKey, string pValue, int pValueLen, string pFile);
        [DllImport("kernel32", SetLastError = true)]
        private static extern int GetPrivateProfileString(string pSection, string pKey, string pDefault, byte[] prReturn, int pBufferLen, string pFile);
        [DllImport("kernel32", SetLastError = true)]
        private static extern int GetPrivateProfileStruct(string pSection, string pKey, byte[] prReturn, int pBufferLen, string pFile);

        private string getString(string section, string key, string defaultValue)
        {

            string sRet = defaultValue;
            byte[] bRet = new byte[_bufferLen];
            int i = GetPrivateProfileString(section, key, defaultValue, bRet, _bufferLen, _iniFilename);
            sRet = System.Text.Encoding.GetEncoding(1252).GetString(bRet, 0, i).TrimEnd((char)0);
            return sRet;

        }


        #endregion

    }


}
