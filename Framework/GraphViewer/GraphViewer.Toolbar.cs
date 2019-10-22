#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace ViNovE.Framework.GraphViewer
{
    public partial class GraphViewer
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. Draw toolbar on the window(Graph Viewer). </para>
        /// <para>Kor. Graph Viewer 윈도우의 캔버스 영역에 툴바를 그려줍니다. </para>
        /// </summary>
        /// <param name="canvasRect"> <para> Eng. Rect size of draw area on window </para>
        ///                           <para> Kor. 윈도우의 캔버스 영역입니다. </para> </param>
        static void DrawToolbar(Rect canvasRect)
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            ///----------------------------------------------------------------------------------------------
            if (scriptViewer == false)
            {   // if, current viewer isn't scriptViewer

                GUI.enabled = false;
                GUILayout.Button(new GUIContent(ViewerStyleSheet.BackwardDisabled, "BackwardDisabled"), EditorStyles.toolbarButton, GUILayout.MaxHeight(20), GUILayout.MaxWidth(26));

                if (currentScene != null)
                {   //if, had currentScene
                    GUI.enabled = true;
                    if(GUILayout.Button(new GUIContent(ViewerStyleSheet.Forward, "Forward"), EditorStyles.toolbarButton, GUILayout.MaxHeight(20), GUILayout.MaxWidth(26)))
                    {
                        scriptViewer = true;
                        prevLinkUID = null;
                        linkerControllActivated = false;

                        _copiedScene = null;
                        _copiedScript = null;

                        _undoList.Clear();
                    }
                }
                else
                {   // or dont had currentScene
                    GUI.enabled = false;
                    GUILayout.Button(new GUIContent(ViewerStyleSheet.ForwardDisabled, "ForwardDisabled"), EditorStyles.toolbarButton, GUILayout.MaxHeight(20), GUILayout.MaxWidth(26));
                }
            }
            else
            {   // or, current viewer is scriptViewer
                GUI.enabled = true;
                if (GUILayout.Button(new GUIContent(ViewerStyleSheet.Backward, "Backward"), EditorStyles.toolbarButton, GUILayout.MaxHeight(20), GUILayout.MaxWidth(26)))
                {
                    scriptViewer = false;
                    prevLinkUID = null;
                    linkerControllActivated = false;

                    _copiedScene = null;
                    _copiedScript = null;

                    _undoList.Clear();
                }

                GUI.enabled = false;
                GUILayout.Button(new GUIContent(ViewerStyleSheet.ForwardDisabled, "ForwardDisabled"), EditorStyles.toolbarButton, GUILayout.MaxHeight(20), GUILayout.MaxWidth(26)); // draw Disabled button Texture
            }

            GUILayout.Space(10);

            GUI.enabled = true;
            if (GUILayout.Button(new GUIContent(ViewerStyleSheet.Save, "Save"), EditorStyles.toolbarButton, GUILayout.MaxHeight(20), GUILayout.MaxWidth(26)))
            {
                ToolbarFuncSave();
            }

            GUILayout.Space(10);

            if(_undoList.Count != 0)
            {
                if (GUILayout.Button(new GUIContent(ViewerStyleSheet.Undo, "Undo"), EditorStyles.toolbarButton, GUILayout.MaxHeight(20), GUILayout.MaxWidth(26)))
                {
                    UndoSequence();
                    Debug.Log("Button pushed Undo");
                }
            }
            else
            {
                GUI.enabled = false;
                GUILayout.Button(new GUIContent(ViewerStyleSheet.UndoDisabled, "UndoDisabled"), EditorStyles.toolbarButton, GUILayout.MaxHeight(20), GUILayout.MaxWidth(26));

                GUI.enabled = true;
            }

            //if (_redoStack.Count != 0)
            //{
            //    if (GUILayout.Button(new GUIContent(ViewerStyleSheet.Redo, "Redo"), EditorStyles.toolbarButton, GUILayout.MaxHeight(20), GUILayout.MaxWidth(26)))
            //    {
            //        RedoSequence();
            //        Debug.Log("Button pushed Redo");
            //    }
            //}
            //else
            //{
            //    GUI.enabled = false;
            //    GUILayout.Button(new GUIContent(ViewerStyleSheet.RedoDisabled, "RedoDisabled"), EditorStyles.toolbarButton, GUILayout.MaxHeight(20), GUILayout.MaxWidth(26));
            //    GUI.enabled = true;
            //}
            ///----------------------------------------------------------------------------------------------

            GUILayout.Space(10);
            GUILayout.FlexibleSpace();

            //...

            GUI.backgroundColor = Color.clear;
            GUI.color = new Color(1, 1, 1, 0.3f);
            GUILayout.Label(string.Format("ViNovE Version : 0.1.0"), EditorStyles.toolbarButton);

            //...

            GUILayout.FlexibleSpace();
            GUILayout.Space(10);

            ///----------------------------------------------------------------------------------------------

            GUI.backgroundColor = Color.clear;
            GUI.color = new Color(1, 1, 1, 0.3f);
            GUILayout.Label(string.Format("visualnoveleditorvinove@gmail.com"), EditorStyles.toolbarTextField);

            GUI.color = Color.white;
            GUI.backgroundColor = Color.white;

            if (GUILayout.Button(new GUIContent(ViewerStyleSheet.SpeakerIllust, "SpeakerSetting"), EditorStyles.toolbarButton, GUILayout.MaxHeight(20), GUILayout.MaxWidth(26)))
            {
                GenericMenu _speakerMenu = new GenericMenu();

                _speakerMenu.AddItem(new GUIContent("Add Speaker Data"), false, () =>
                {
                    AddSpeakerInspector();
                });
                _speakerMenu.AddItem(new GUIContent("Add Speaker Illustration"), false, () =>
                {
                    AddIllustInspector("Speaker");
                });

                _speakerMenu.ShowAsContext();
            }
            if (GUILayout.Button(new GUIContent(ViewerStyleSheet.BackgroundIllust, "BackgroundIllust"), EditorStyles.toolbarButton, GUILayout.MaxHeight(20), GUILayout.MaxWidth(26)))
            {
                AddIllustInspector("Background");
            }

            GUILayout.Space(10);

            if (GUILayout.Button(new GUIContent(ViewerStyleSheet.Setting, "Setting"), EditorStyles.toolbarButton, GUILayout.MaxHeight(20), GUILayout.MaxWidth(26)))
            {
                GenericMenu _settingMenu = new GenericMenu();

                _settingMenu.AddItem(new GUIContent("Build/Copy Game Data"), false, () =>
                {
                    ToolbarCopyGameData();
                });
                _settingMenu.AddItem(new GUIContent("Build/Open Project Build"), false, () =>
                {
                    ToolbarProjectBuild();
                });
                //_settingMenu.AddItem(new GUIContent("Image/Texture Resize"), false, () =>
                //{
                //    ToolbarTextureResize();
                //});

                _settingMenu.ShowAsContext();
            }

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// <para>Eng. Toolbar Function : Save Vinove</para>
        /// <para>Kor. 툴바 기능 : Vinove 저장 </para>
        /// </summary>
        static public void ToolbarFuncSave()
        {
            EditorUtility.SetDirty(currentVinove);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            _undoList.Clear();
            Debug.Log("Asset Saved");
        }

        /// <summary>
        /// <para>Eng. Toolbar Function : Call Project Build Setting</para>
        /// <para>Kor. 툴바 기능 : 빌드 세팅 호출 </para>
        /// </summary>
        static public void ToolbarCopyGameData()
        {
            string _gamedatPath = Application.dataPath + "/Resources/" + "Gamedat.asset";

            if (File.Exists(_gamedatPath))
            {
                File.Delete(_gamedatPath);
            }
            Directory.CreateDirectory(Application.dataPath + "/Resources/");
            File.Copy(currentVinove.FileDir, _gamedatPath);
        }

        /// <summary>
        /// <para>Eng. Toolbar Function : Call Project Build Setting</para>
        /// <para>Kor. 툴바 기능 : 빌드 세팅 호출 </para>
        /// </summary>
        static public void ToolbarProjectBuild()
        {
            EditorWindow.GetWindow(Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
        }

        ///// <summary>
        ///// <para>Eng. Toolbar Function : Texture Resize </para>
        ///// <para>Kor. 툴바 기능 : Texture Resize </para>
        ///// </summary>
        //static public void ToolbarTextureResize()
        //{
        //    OpenTextureResizeInspector();
        //}
    }
}
#endif