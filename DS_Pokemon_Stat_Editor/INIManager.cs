using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Sinjoh_Editor
{
    public static class INIManager
    {
        public static bool IsDarkMode = false;
        public static Languages Language;
        private const string INI_FILE_NAME = "pokemon_sinjoh_editor.ini";
        private const string DARK_MODE_KEY_NAME = "DARKMODE";
        private const string LANGUAGE_KEY_NAME = "Language";

        public static void LoadINI()
        {


            if (File.Exists(INI_FILE_NAME))
            {
                try
                {
                    string fileLine;
                    using StreamReader iniReader = new StreamReader(new FileStream(INI_FILE_NAME, FileMode.Open));
                    fileLine = iniReader.ReadLine();

                    if (fileLine.Contains(LANGUAGE_KEY_NAME))
                    {
                        int languageValueStart = fileLine.IndexOf('=') + 1;
                        Language = fileLine.Substring(languageValueStart).ToLower() switch
                        {
                            "english" => Languages.ENGLISH,
                            "español" => Languages.SPANISH,
                            "français" => Languages.FRENCH,
                            "deutsch" => Languages.GERMAN,
                            "italiano" => Languages.ITALIAN,
                            "日本語" => Languages.JAPANESE,
                            "한국어" => Languages.KOREAN,
                            _ => Languages.ENGLISH
                        };
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

                iniWriter.WriteLine(LANGUAGE_KEY_NAME + "=english");
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

                string languageName;

                languageName = Language switch
                {
                    Languages.ENGLISH => "english",
                    Languages.SPANISH => "español",
                    Languages.FRENCH => "français",
                    Languages.GERMAN => "deutsch",
                    Languages.ITALIAN => "italiano",
                    Languages.JAPANESE => "日本語",
                    Languages.KOREAN => "한국어",
                    _ => "english"
                };


                iniWriter.WriteLine(LANGUAGE_KEY_NAME + '=' + languageName);
            }
            catch (Exception e)
            {
                throw new Exception("An exception occured while attempting to save to INI file.\n" + e.Message);
            }
        }
    }
}
