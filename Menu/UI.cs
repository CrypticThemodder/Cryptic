using BepInEx;
using Cryptic;
using Cryptic.Mods;
using HarmonyLib;
using Photon.Pun;
using System;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static Modio.API.ModioAPI;

namespace UI2
{
    [BepInPlugin("com.cryptic.ui", "cryptic.ui", "0.0.1")]
    public class Main : BaseUnityPlugin
    {
        public static Rect rectwindow = new Rect(1f, 1f, 600f, 600f);
        public static CurrentPage currentPage = CurrentPage.None;

        private bool showUI = true;

        private GUIStyle windowStyle;
        private GUIStyle buttonStyle;
        private GUIStyle activeButtonStyle;

        public enum CurrentPage
        {
            None,
            PlayerInfo,
            Movement,
        }

        void OnGUI()
        {
            if (!showUI) return;

            if (Keyboard.current != null && Keyboard.current.qKey.isPressed) return;

            GUI.backgroundColor = new Color32(69, 69, 69, 255);
            GUI.Box(rectwindow, GUIContent.none);
            
            if (windowStyle == null)
            {
                Texture2D bg = LoadEmbeddedTexture("UI2.window_bg.png"); // replace with your exact resource name
                if (bg != null)
                {
                    windowStyle = new GUIStyle(GUI.skin.window);
                    windowStyle.normal.background = bg;
                    windowStyle.stretchWidth = true;
                    windowStyle.stretchHeight = true;
                }
                else
                {
                    windowStyle = new GUIStyle(GUI.skin.window); // fallback
                }
            }

            if (buttonStyle == null)
            {
                // Normal button: semi-transparent light gray
                Texture2D btnBG = MakeTex(2, 2, new Color32(200, 200, 200, 100)); // alpha 100/255 = ~40% opaque
                Texture2D hoverBG = MakeTex(2, 2, new Color32(220, 220, 220, 150)); // hover slightly brighter
                Texture2D activeBG = MakeTex(2, 2, new Color32(180, 180, 180, 150)); // active slightly darker

                buttonStyle = new GUIStyle(GUI.skin.button);
                buttonStyle.normal.background = btnBG;
                buttonStyle.hover.background = hoverBG;
                buttonStyle.onNormal.background = btnBG;
                buttonStyle.onHover.background = hoverBG;
                buttonStyle.fixedHeight = 40;
                buttonStyle.fontSize = 16;

                activeButtonStyle = new GUIStyle(buttonStyle);
                activeButtonStyle.normal.background = activeBG;
                activeButtonStyle.hover.background = hoverBG;
            }

            GUIStyle InfoStyle = currentPage == CurrentPage.PlayerInfo ? activeButtonStyle : buttonStyle;
            GUIStyle movementStyle = currentPage == CurrentPage.Movement ? activeButtonStyle : buttonStyle;

            // Draw the window with background
            rectwindow = GUILayout.Window(0, rectwindow,
                id => DrawWindow(id, buttonStyle, activeButtonStyle, InfoStyle, movementStyle),
                "Ghost.Panel", windowStyle);
        }
        public static float fpsValue = 60f;
        void DrawWindow(int windowID, GUIStyle buttonStyle, GUIStyle activeButtonStyle, GUIStyle disconnectButtonStyle, GUIStyle JoinRoomButton)
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical(GUILayout.Width(180));

            GUILayout.Label("=======================");
            if (GUILayout.Button("Disconnect", disconnectButtonStyle))
                PhotonNetwork.Disconnect();

            GUILayout.Label("Press 'E' To Toggle UI on/off");
            GUILayout.Label("FPS: " + Mathf.Ceil(1f / Time.unscaledDeltaTime).ToString());
            GUILayout.Label("Welcome " + PhotonNetwork.LocalPlayer.NickName + "!");
            GUILayout.Label("join a room");
            GUILayout.Label("FPS: " + fpsValue);

            fpsValue = GUILayout.HorizontalSlider(fpsValue, 12f, 240f);

            if (GUILayout.Button("Player Info", disconnectButtonStyle))
                currentPage = CurrentPage.PlayerInfo;

            GUILayout.Label("=======================");
            GUILayout.Space(10);

            GUIStyle movementStyle = currentPage == CurrentPage.Movement ? activeButtonStyle : buttonStyle;

            if (GUILayout.Button("Movement", movementStyle))
                currentPage = CurrentPage.Movement;

            GUILayout.EndVertical();

            GUILayout.BeginVertical();

            switch (currentPage)
            {
                case CurrentPage.Movement:
                    GUILayout.Space(10);
                    if (GUILayout.Button("Back", buttonStyle))
                        currentPage = CurrentPage.None;
                    break;

                case CurrentPage.PlayerInfo:
                    GUILayout.Label("FPS: " + Mathf.Ceil(1f / Time.unscaledDeltaTime).ToString());
                    GUILayout.Label("UserName: " + PhotonNetwork.LocalPlayer.NickName + "!");
                    GUILayout.Label("UID: " + PhotonNetwork.LocalPlayer.UserId + ".");
                    GUILayout.Label("Master Client: " + PhotonNetwork.LocalPlayer.IsMasterClient + ".");
                    GUILayout.Label("Leaderboard Spot: " + PhotonNetwork.LocalPlayer.ActorNumber + ".");
                    break;
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            // Clamp window position to screen
            rectwindow.x = Mathf.Clamp(rectwindow.x, 0, Screen.width - rectwindow.width);
            rectwindow.y = Mathf.Clamp(rectwindow.y, 0, Screen.height - rectwindow.height);

            GUI.DragWindow();
        }

        private Texture2D LoadEmbeddedTexture(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    Debug.LogError("Resource not found: " + resourceName);
                    return null;
                }

                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);

                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);

                // Make texture fully opaque
                Color[] pixels = tex.GetPixels();
                for (int i = 0; i < pixels.Length; i++)
                    pixels[i].a = 1f; // set alpha to 100%
                tex.SetPixels(pixels);
                tex.Apply();

                return tex;
            }
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        void Awake()
        {
            Harmony harmony = new Harmony("com.cryptic.ui");
            harmony.PatchAll();
        }

        void Update()
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                showUI = !showUI;
            }
        }
    }
}