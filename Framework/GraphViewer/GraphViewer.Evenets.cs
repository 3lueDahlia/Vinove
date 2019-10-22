#if UNITY_EDITOR
using System;

using ViNovE.Framework.InGame;
using ViNovE.Framework.Data;
using ViNovE.Framework.Data.IO;

using UnityEngine;
using UnityEditor;

using ViNovE.Framework.InGame.Utility;

namespace ViNovE.Framework.GraphViewer
{
    public partial class GraphViewer
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        static VinoveScene hoverScene;
        static VinoveBranch hoverBranch;
        static VinoveMerge hoverMerge;
        static VinoveScript hoverScript;

        ////////////////////////////////
        /// Status
        public bool _popupOpened = false;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. GraphViewer Event Proc Func </para>
        /// <para>Kor. GraphViewer의 이벤트를 담당하는 함수 </para>
        /// </summary>
        /// <param name="_vinove"> <para> Eng. Currently opened vinove file</para>
        ///                           <para> Kor. 현재 열린 비노비 파일 </para> </param>
        /// <param name="_canvasMousePos"> <para> Eng. Mouse pos on Canvas</para>
        ///                           <para> Kor. 캔버스 내에서의 마우스 위치</para> </param>
        static void GraphEvents(Vinove _vinove, Vector2 _canvasMousePos)
        {
            if(IsMouseOnMinimap())
            {

            }
            else
            {
                if (e.type == EventType.Repaint)
                {
                    needRepaint = false;
                }

                if (e.type == EventType.MouseUp)
                {
                    graphTranslating = false;

                    hoverScene = null;
                    hoverBranch = null;
                    hoverMerge = null;
                }

                if(e.type == EventType.MouseMove)
                {
                    hoverScene = currentVinove.IsContainWholeGraph(e.mousePosition);
                    if (hoverScene == null)
                    {
                        hoverBranch = currentVinove.IsContainWholeBranch(e.mousePosition);
                        if (hoverBranch == null)
                        {
                            hoverMerge = currentVinove.IsContainWholeMerge(e.mousePosition);
                        }
                    }

                    needRepaint = true;
                }

                if (e.type == EventType.ScrollWheel)
                {   // Zoom
                    if(canvasRect.Contains(e.mousePosition))
                    {
                        float zoomCtrl = e.control ? 0.1f : 0.2f;
                        float zoomValue = e.delta.y > 0 ? -zoomCtrl : zoomCtrl;

                        ZoomWithFocus(e.mousePosition, zoomValue);
                        e.Use();
                    }
                }

                if (((e.alt && e.button == 0 && e.type == EventType.MouseDrag && e.isMouse))
                    ||  (e.button == 2 && e.type == EventType.MouseDrag && canvasRect.Contains(e.mousePosition)))
                {   //translation canvas
                    Translation += e.delta;
                
                    e.Use();
                }

                if(e.type == EventType.MouseDown && e.button == 1 && canvasRect.Contains(e.mousePosition))
                {   // Open node popupMenu
                    GenericMenu popupMenu = new GenericMenu();

                    VinoveScene overScene = currentVinove.IsContainWholeGraph(e.mousePosition);
                    VinoveBranch overBranch = currentVinove.IsContainWholeBranch(e.mousePosition);
                    VinoveMerge overMerge = currentVinove.IsContainWholeMerge(e.mousePosition);

                    if (overScene == null && overBranch == null && overMerge == null)
                    {   // if, right click not on scene node
                        popupMenu.AddItem(new GUIContent("Add Scene"), false, () =>
                        {
                            currentVinove.CreateScene("New Scene", new Rect(_canvasMousePos.x, _canvasMousePos.y, 200, 100));
                            Debug.Log("popupMenu_AddScene Test");
                        });

                        popupMenu.AddItem(new GUIContent("Add Branch"), false, () =>
                        {
                            currentVinove.CreateBranch("New Branch", new Rect(_canvasMousePos.x, _canvasMousePos.y, 150, 200));
                            Debug.Log("popupMenu_AddBranch Test");
                        });

                        popupMenu.AddItem(new GUIContent("Add Merge"), false, () =>
                        {
                            currentVinove.CreateMerge("New Merge", new Rect(_canvasMousePos.x, _canvasMousePos.y, 150, 200));
                            Debug.Log("popupMenu_AddMerge Test");
                        });

                        popupMenu.AddSeparator("");

                        if (_copiedScene == null)
                        {
                            popupMenu.AddDisabledItem(new GUIContent("Paste Scene"));
                        }
                        else
                        {
                            popupMenu.AddItem(new GUIContent("Paste Scene"), false, () =>
                            {
                                currentVinove.PasteScene(_copiedScene, new Rect(_canvasMousePos.x, _canvasMousePos.y, 200, 100));
                                _copiedScene = null;
                            });
                        }

                        popupMenu.AddSeparator("");
                    }
                    else if (overScene != null)
                    {   // on graph
                        popupMenu.AddItem(new GUIContent("Open Scene"), false, () =>
                        {
                            currentScene = overScene;
                            scriptViewer = true;

                            _copiedScene = null;
                        });

                        popupMenu.AddItem(new GUIContent("Copy Scene"), false, () =>
                        {
                            _copiedScene = overScene;
                        });

                        popupMenu.AddItem(new GUIContent("Delete Scene"), false, () =>
                        {
                            GraphDeleteSequence(overScene.UID, true);
                        });

                        popupMenu.AddSeparator("");

                        popupMenu.AddItem(new GUIContent("Select Starter"), false, () =>
                        {

                            currentVinove.StarterUID = overScene.UID;
                        });

                        popupMenu.AddItem(new GUIContent("Set Background Image and Music"), false, () =>
                        {
                            OpenSceneBackgroundInspector(overScene);
                        });

                        popupMenu.AddSeparator("");

                        popupMenu.AddItem(new GUIContent("Rename"), false, () =>
                        {
                            OpenRenameInspector(overScene.UID);
                        });

                        popupMenu.AddItem(new GUIContent("Run"), false, () =>
                        {
                            AssetPathArchive _archive = new AssetPathArchive(currentVinovePath, overScene.UID);
                            string _archivePath = "Assets/TemporaryArchive.vnvach";

                            BinaryIOManager.GetInst().BinarySerialize<AssetPathArchive>(_archive, _archivePath);

                            ToolbarFuncSave();

                            EditorApplication.isPlaying = true;
                        });
                    }
                    else if(overBranch != null)
                    {
                        popupMenu.AddItem(new GUIContent("Open Branch"), false, () =>
                        {
                            OpenBranchInspector(overBranch);
                            Debug.Log("popupMenu_OpenBranch Test : " + overBranch.UID);
                        });

                        popupMenu.AddItem(new GUIContent("Delete Branch"), false, () =>
                        {
                            GraphDeleteSequence(overBranch.UID, true);
                            Debug.Log("popupMenu_DeleteBranch");
                        });

                        popupMenu.AddSeparator("");

                        popupMenu.AddItem(new GUIContent("Change Branch Count"), false, () =>
                        {
                            OpenCountInspector(overBranch);
                        });

                        popupMenu.AddSeparator("");

                        popupMenu.AddItem(new GUIContent("Rename"), false, () =>
                        {
                            OpenRenameInspector(overBranch.UID);
                        });
                    }
                    else if (overMerge != null)
                    {
                        popupMenu.AddItem(new GUIContent("Change Merge Count"), false, () =>
                        {
                            OpenCountInspector(overMerge);
                        });

                        popupMenu.AddSeparator("");

                        popupMenu.AddItem(new GUIContent("Delete Merge"), false, () =>
                        {
                            GraphDeleteSequence(overMerge.UID, true);
                            Debug.Log("popupMenu_DeleteBranch");
                        });

                        popupMenu.AddSeparator("");

                        popupMenu.AddItem(new GUIContent("Rename"), false, () =>
                        {
                            OpenRenameInspector(overMerge.UID);
                        });
                    }

                    popupMenu.ShowAsContext();
                }

                if (e.type == EventType.MouseDrag && e.button == 0 && canvasRect.Contains(e.mousePosition) && graphTranslating == true)
                {
                    if (hoverScene != null)
                    {
                        graphTranslating = true;

                        hoverScene.MoveSceneGraph(e.delta, ZoomFactor);
                        needRepaint = true;
                    }
                    else if(hoverBranch != null)
                    {
                        graphTranslating = true;

                        hoverBranch.MoveBranchGraph(e.delta, ZoomFactor);
                        needRepaint = true;
                    }
                    else if (hoverMerge != null)
                    {
                        graphTranslating = true;

                        hoverMerge.MoveMergeGraph(e.delta, ZoomFactor);
                        needRepaint = true;
                    }

                    e.Use();
                }


                if (e.isMouse && e.type == EventType.MouseDown && e.clickCount == 2)
                {   // Double clicked
                    graphTranslating = false;

                    VinoveScene overScene = currentVinove.IsContainWholeGraph(e.mousePosition);

                    if (overScene != null)
                    {
                        currentScene = overScene;
                        scriptViewer = true;
                        prevLinkUID = null;
                        linkerControllActivated = false;

                        _copiedScene = null;

                        _undoList.Clear();
                    }

                    VinoveBranch overBranch = currentVinove.IsContainWholeBranch(e.mousePosition);

                    if (overBranch != null)
                    {
                        OpenBranchInspector(overBranch);
                    }
                    e.Use();
                }

                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    if (hoverScene == null && hoverBranch == null && hoverMerge == null)
                    {
                        linkerControllActivated = false;
                    }
                    else
                    { 
                        graphTranslating = true;
                    }
                }
            }
        }

        /// <summary>
        /// <para>Eng. Script GraphViewer Event Proc Func </para>
        /// <para>Kor. Script GraphViewer의 이벤트를 담당하는 함수 </para>
        /// </summary>
        /// <param name="_vinove"> <para> Eng. Currently opened vinove file</para>
        ///                           <para> Kor. 현재 열린 비노비 파일 </para> </param>
        /// <param name="_openedScene"> <para> Eng. Currently opened scene</para>
        ///                           <para> Kor. 현재 뷰어로 열린 씬</para> </param>
        /// <param name="_canvasMousePos"> <para> Eng. Mouse pos in Canvas</para>
        ///                           <para> Kor. 캔버스 내에서의 마우스 위치</para> </param>
        static void ScriptGraphEvents(Vinove _vinove, VinoveScene _openedScene, Vector2 _canvasMousePos)
        {
            if (IsMouseOnMinimap())
            {

            }
            else
            {
                if (e.type == EventType.Repaint)
                {
                    needRepaint = false;
                }

                if (e.type == EventType.MouseUp)
                {
                    graphTranslating = false;
                    hoverScript = null;
                }

                if (e.type == EventType.ScrollWheel)
                {   // Zoom
                    if (canvasRect.Contains(e.mousePosition))
                    {
                        float zoomCtrl = e.control ? 0.1f : 0.2f;
                        float zoomValue = e.delta.y > 0 ? -zoomCtrl : zoomCtrl;

                        ZoomWithFocus(e.mousePosition, zoomValue);
                        e.Use();
                    }
                }

                if (((e.alt && e.button == 0 && e.type == EventType.MouseDrag && e.isMouse))
                    || (e.button == 2 && e.type == EventType.MouseDrag && canvasRect.Contains(e.mousePosition)))
                {   //translation canvas
                    _openedScene.Translation += e.delta;

                    e.Use();
                }

                if (e.type == EventType.MouseDown && e.button == 1 && canvasRect.Contains(e.mousePosition))
                {   // Open node popupMenu
                    GenericMenu popupMenu = new GenericMenu();

                    VinoveScript overScript = _openedScene.IsContainWholeGraph(e.mousePosition);
                    if (overScript == null)
                    {   // if, right click not on scene node
                        popupMenu.AddItem(new GUIContent("Add Script"), false, () =>
                        {
                            currentVinove.CreateScript(_openedScene, "New Script", new Rect(_canvasMousePos.x, _canvasMousePos.y, Constants.HORIZONTAL_GRAPH_SIZE_X, Constants.HORIZONTAL_GRAPH_SIZE_Y));
                            Debug.Log("popupMenu_AddScript Test");
                        });
                        popupMenu.AddSeparator("");

                        if (_copiedScript == null)
                        {
                            popupMenu.AddDisabledItem(new GUIContent("Paste Script"));
                        }
                        else
                        {
                            popupMenu.AddItem(new GUIContent("Paste Script"), false, () =>
                            {
                                currentVinove.PasteScript(currentScene, _copiedScript, new Rect(_canvasMousePos.x, _canvasMousePos.y, Constants.HORIZONTAL_GRAPH_SIZE_X, Constants.HORIZONTAL_GRAPH_SIZE_Y));
                                _copiedScript = null;
                            });
                        }
                        popupMenu.AddSeparator("");
                    }
                    else
                    {   // on graph
                        popupMenu.AddItem(new GUIContent("Open Script"), false, () =>
                        {
                            OpenScriptInspector(overScript);
                            Debug.Log("popupMenu_OpenScript Test : " + overScript.UID);
                        });

                        popupMenu.AddItem(new GUIContent("Copy Script"), false, () =>
                        {
                            _copiedScript = overScript;
                        });

                        popupMenu.AddItem(new GUIContent("Delete Script"), false, () =>
                        {
                            GraphDeleteSequence(overScript.UID, true);
                            Debug.Log("popupMenu_DeleteScript Test");
                        });

                        popupMenu.AddSeparator("");

                        popupMenu.AddItem(new GUIContent("Select Starter"), false, () =>
                        {
                            currentScene.StarterUID = overScript.UID;
                        });

                        popupMenu.AddSeparator("");

                        popupMenu.AddItem(new GUIContent("Rename"), false, () =>
                        {
                            OpenRenameInspector(overScript.ParentSceneUID, overScript.UID);
                        });

                        popupMenu.AddItem(new GUIContent("Run"), false, () =>
                        {
                            ToolbarFuncSave();

                            AssetPathArchive _archive = new AssetPathArchive(currentVinovePath, overScript.ParentSceneUID, overScript.UID);
                            string _archivePath = "Assets/TemporaryArchive.vnvach";

                            BinaryIOManager.GetInst().BinarySerialize<AssetPathArchive>(_archive, _archivePath);

                            EditorApplication.isPlaying = true;
                        });
                    }

                    popupMenu.ShowAsContext();
                }

                if (e.type == EventType.MouseDrag && e.button == 0 && canvasRect.Contains(e.mousePosition))
                {   // Graph Translating
                    VinoveScript overScript = currentScene.IsContainWholeGraph(e.mousePosition);

                    if (hoverScript != null)
                    {
                        graphTranslating = true;

                        hoverScript.MoveScriptGraph((e.delta / currentScene.ZoomFactor));
                        needRepaint = true;
                    }

                    e.Use();
                }

                if (e.isMouse && e.type == EventType.MouseDown && e.clickCount == 2)
                {   // Double clicked
                    graphTranslating = false;

                    VinoveScript overScript = currentScene.IsContainWholeGraph(e.mousePosition);

                    if (overScript != null)
                    {
                        OpenScriptInspector(overScript);
                    }
                    e.Use();
                }

                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    VinoveScript _scriptTmp = currentScene.IsContainWholeGraph(e.mousePosition);

                    if (_scriptTmp == null)
                    {
                        linkerControllActivated = false;
                    }
                    else if (graphTranslating == false)
                    {
                        hoverScript = _scriptTmp;
                    }
                }
            }
        }

        static bool IsMouseOnMinimap()
        {
            return MinimapRect.Contains(e.mousePosition) ? true : false;
        }
    }
}
#endif