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

        public static void Setup(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            Application.quitting += OnApplicationQuit;
        }

        private static void OnApplicationQuit()
        {
            Utils.CreateLatestBackupFile();
            Utils.CreateBackupFile("종료");
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            IsEnabled = value;

            if (value)
            {
                //켜질때
                harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());

                Utils.CreateBackupFile("시작");
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
            if (GUILayout.Button("ADOFAI를 시작할 때 백업된 파일로 복원하기", GUILayout.Width(300)))
            {
                Utils.RestoreData("시작");
            }
            if (GUILayout.Button("ADOFAI를 종료할 때 백업된 파일로 복원하기", GUILayout.Width(300)))
            {
                Utils.RestoreData("종료");
            }
            if (GUILayout.Button("ADOFAI가 마지막으로 종료될 때 백업된 파일로 복원하기", GUILayout.Width(400)))
            {
                Utils.RestoreToLatestData();
            }



            GUILayout.Label(" ");
            GUILayout.Label("커스텀 레벨");

            if (GUILayout.Button("ADOFAI를 시작할 때 백업된 파일로 복원하기", GUILayout.Width(300)))
            {
                Utils.RestoreCustomData("시작");
            }
            if (GUILayout.Button("ADOFAI를 종료할 때 백업된 파일로 복원하기", GUILayout.Width(300)))
            {
                Utils.RestoreCustomData("종료");
            }
            if (GUILayout.Button("ADOFAI가 마지막으로 종료될 때 백업된 파일로 복원하기", GUILayout.Width(400)))
            {
                Utils.RestoreToLatestCustomData();
            }
            GUILayout.Label(" ");
            GUILayout.Label("(백업 파일을 불러오는 순간 ADOFAI가 재시작됩니다!)");
        }
    }
}
