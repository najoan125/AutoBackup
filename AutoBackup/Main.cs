using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnityEngine;
using UnityModManagerNet;

namespace AutoBackup
{
    public static class Main
    {
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static Harmony harmony;
        public static bool IsEnabled = false;
        public static string startpath;

        public static void Setup(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnGUI = OnGUI;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            IsEnabled = value;

            if (value)
            {
                //켜질때
                harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());

                string userPath = Path.Combine("User");
                string start = userPath + @"\시작";
                DirectoryInfo startdi = new DirectoryInfo(start);
                if (!startdi.Exists)
                {
                    startdi.Create();
                }
                System.IO.File.Copy(userPath + @"\data.sav", userPath + @"\시작\" + System.DateTime.Now.ToString("yyyy-MM-dd tt hh.mm.ss") + ".sav");
            }
            else
            {
                //꺼질때
                harmony.UnpatchAll(modEntry.Info.Id);
            }
            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if (GUILayout.Button("백업 폴더 열기"))
            {
                string userPath = Path.Combine("User");
                System.Diagnostics.Process.Start(userPath);
            }
            GUILayout.Label(" ");
            if (GUILayout.Button("Adofai를 시작할 때 백업된 파일로 복원하기"))
            {
                string userPath = Path.Combine("User", "시작");
                string user = Path.Combine("User");
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.InitialDirectory = userPath;
                dialog.Filter = "sav files (*.sav)|*.sav";
                dialog.FilterIndex = 2;
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.Copy(dialog.FileName, user + @"\data.sav", true);
                }
            }
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            string userPath = Path.Combine("User");
            string start = userPath + @"\종료";
            DirectoryInfo startdi = new DirectoryInfo(start);
            if (!startdi.Exists)
            {
                startdi.Create();
            }
            System.IO.File.Copy(userPath + @"\data.sav", userPath + @"\종료\" + System.DateTime.Now.ToString("yyyy-MM-dd tt hh.mm.ss") + ".sav");
        }
    }
}
