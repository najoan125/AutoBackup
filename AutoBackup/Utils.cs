using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Application = UnityEngine.Application;

namespace AutoBackup
{
    public static class Utils
    {
        public static void CreateBackupFile(String name)
        {
            string userPath = Path.Combine("User");
            string start = userPath + @"\" + name;
            DirectoryInfo startdi = new DirectoryInfo(start);
            if (!startdi.Exists)
            {
                startdi.Create();
            }
            File.Copy(userPath + @"\data.sav", userPath + @"\" + name + @"\" + DateTime.Now.ToString("yyyy-MM-dd tt hh.mm.ss") + ".sav");

            string userPath_Custom = Path.Combine("User");
            string start_Custom = userPath_Custom + @"\" + name + "_Custom";
            DirectoryInfo startdi_Custom = new DirectoryInfo(start_Custom);
            if (!startdi_Custom.Exists)
            {
                startdi_Custom.Create();
            }
            File.Copy(userPath_Custom + @"\custom_data.sav", userPath_Custom + @"\" + name + @"_Custom\" + DateTime.Now.ToString("yyyy-MM-dd tt hh.mm.ss") + ".sav");
        }

        public static void CreateLatestBackupFile()
        {
            string userPath = Path.Combine("User");
            File.Copy(userPath + @"\data.sav", userPath + @"\latest_data.sav", true);
            File.Copy(userPath + @"\custom_data.sav", userPath + @"\latest_custom_data.sav", true);
        }

        public static void RestoreToLatestData()
        {
            string userPath = Path.Combine("User");
            if (!File.Exists(userPath + @"\latest_data.sav"))
            {
                return;
            }
            File.Copy(userPath + @"\latest_data.sav", userPath + @"\data.sav", true);
            Process.Start(Application.dataPath.Replace("_Data", ".exe"));
            Application.Quit();
        }

        public static void RestoreToLatestCustomData()
        {
            string userPath = Path.Combine("User");
            if (!File.Exists(userPath + @"\latest_custom_data.sav"))
            {
                return;
            }
            File.Copy(userPath + @"\latest_custom_data.sav", userPath + @"\custom_data.sav", true);
            Process.Start(Application.dataPath.Replace("_Data", ".exe"));
            Application.Quit();
        }

        public static void RestoreData(String name)
        {
            string userPath = Path.Combine("User", name);
            string user = Path.Combine("User");
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = userPath;
            dialog.Filter = "sav files (*.sav)|*.sav";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.Copy(dialog.FileName, user + @"\data.sav", true);
                Process.Start(Application.dataPath.Replace("_Data", ".exe"));
                Application.Quit();
            }
        }

        public static void RestoreCustomData(String name)
        {
            string userPath = Path.Combine("User", name + "_Custom");
            string user = Path.Combine("User");
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = userPath;
            dialog.Filter = "sav files (*.sav)|*.sav";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.Copy(dialog.FileName, user + @"\custom_data.sav", true);
                Process.Start(Application.dataPath.Replace("_Data", ".exe"));
                Application.Quit();
            }
        }
    }
}
