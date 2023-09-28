using HarmonyLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using UnityEngine;
using UnityModManagerNet;
using Application = UnityEngine.Application;

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

                CreateBackupFile("시작");
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
            if (GUILayout.Button("백업 폴더 열기", GUILayout.Width(200)))
            {
                string userPath = Path.Combine("User");
                Process.Start(userPath);
            }
            GUILayout.Label(" ");
            GUILayout.Label("사용자 설정 및 공식맵 데이터");
            if (GUILayout.Button("Adofai를 시작할 때 백업된 파일로 복원하기", GUILayout.Width(300)))
            {
                RestoreData("시작");
            }

            if (GUILayout.Button("Adofai를 종료할 때 백업된 파일로 복원하기", GUILayout.Width(300)))
            {
                RestoreData("종료");
            }

            GUILayout.Label(" ");
            GUILayout.Label("커스텀 레벨");

            if (GUILayout.Button("Adofai를 시작할 때 백업된 파일로 복원하기", GUILayout.Width(300)))
            {
                RestoreCustomData("시작");
            }
            if (GUILayout.Button("Adofai를 종료할 때 백업된 파일로 복원하기", GUILayout.Width(300)))
            {
                RestoreCustomData("종료");
            }
            GUILayout.Label(" ");
            GUILayout.Label("(백업 파일을 불러오는 순간 ADOFAI가 재시작됩니다!)");
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            CreateBackupFile("종료");
        }

        private static void CreateBackupFile(String name)
        {
            string userPath = Path.Combine("User");
            string start = userPath + @"\"+name;
            DirectoryInfo startdi = new DirectoryInfo(start);
            if (!startdi.Exists)
            {
                startdi.Create();
            }
            File.Copy(userPath + @"\data.sav", userPath + @"\"+name+@"\" + DateTime.Now.ToString("yyyy-MM-dd tt hh.mm.ss") + ".sav");

            string userPath_Custom = Path.Combine("User");
            string start_Custom = userPath_Custom + @"\"+name+"_Custom";
            DirectoryInfo startdi_Custom = new DirectoryInfo(start_Custom);
            if (!startdi_Custom.Exists)
            {
                startdi_Custom.Create();
            }
            File.Copy(userPath_Custom + @"\custom_data.sav", userPath_Custom + @"\"+name+@"_Custom\" + DateTime.Now.ToString("yyyy-MM-dd tt hh.mm.ss") + ".sav");
        }

        private static void RestoreData(String name)
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

        private static void RestoreCustomData(String name)
        {
            string userPath = Path.Combine("User", name+"_Custom");
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
