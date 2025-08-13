using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Sinjoh_Editor
{
    internal static class INIManager
    {
        public static bool IsDarkMode = false;
        private const string INI_FILE_NAME = "pokemon_sinjoh_editor.ini";
        private const string DARK_MODE_KEY_NAME = "DARKMODE";

        public static void LoadINI()
        {


            if (File.Exists(INI_FILE_NAME))
            {
                try
                {
                    string fileLine;
                    using StreamReader iniReader = new StreamReader(new FileStream(INI_FILE_NAME, FileMode.Open));
                    fileLine = iniReader.ReadLine();

                    if (fileLine.Contains(DARK_MODE_KEY_NAME))
                    {
                        int darkModeValueStart = fileLine.IndexOf('=') + 1;
                        if (!bool.TryParse(fileLine.Substring(darkModeValueStart), out IsDarkMode))
                            IsDarkMode = false;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("An exception occured while attempting to read INI file.\n" + e.Message);
                }

            }
            else
                createNewINI();


        }

        private static void createNewINI()
        {
            try
            {
                using StreamWriter iniWriter = new StreamWriter(new FileStream(INI_FILE_NAME, FileMode.Create));

                iniWriter.WriteLine(DARK_MODE_KEY_NAME + "=false");
            }
            catch (Exception e)
            {
                throw new Exception("An exception occured while attempting to create a new INI file.\n" + e.Message);
            }
            
        }

        public static void SaveINI()
        {
            try
            {
                using StreamWriter iniWriter = new StreamWriter(new FileStream(INI_FILE_NAME, FileMode.OpenOrCreate));

                iniWriter.WriteLine(DARK_MODE_KEY_NAME + '=' + IsDarkMode);
            }
            catch (Exception e)
            {
                throw new Exception("An exception occured while attempting to save to INI file.\n" + e.Message);
            }
        }
    }
}
