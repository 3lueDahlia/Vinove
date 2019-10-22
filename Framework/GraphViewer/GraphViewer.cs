#if UNITY_EDITOR
using ViNovE.Framework;
using ViNovE.Framework.Data;
using ViNovE.Framework.Error;

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace ViNovE.Framework.GraphViewer
{
    public partial class GraphViewer : EditorWindow
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables
        /// 

        ////////////////////////////////////
        /// Viewer Properties

        public static GraphViewer currentGraphViewer;   // current opened GraphViewer
        public static Vinove currentVinove;             // current opened Vinove in opened GraphViewer
        public static string currentVinovePath;         // relative path cuttrent openmed vinove
        public static VinoveScene currentScene;         // current opened VinoveScene in opened GraphViewer

        ////////////////////////////////////
        /// Viewer Element Items


        ////////////////////////////////////
        /// Viewer Temporary

        static string prevLinkUID;
        static Vector2 prevLinkPos;

        static VinoveScene _copiedScene;
        static VinoveScript _copiedScript;

        ////////////////////////////////////
        /// Viewer Status

        static bool scriptViewer = false;
        static bool linkerControllActivated = false;
        static bool graphTranslating = false;
        static bool needRepaint = false;
        static int prevBranchIndex = 0;

        ////////////////////////////////////
        /// Const Size & Count & etc...
        const float TOP_MARGIN = 20;        //canvas top margin
        const float BOTTOM_MARGIN = 15;     //canvas bottom margin
        const int GRID_SIZE = 15;           //canvas grid size

        ////////////////////////////////////
        /// Rect Sizes
        private static Rect canvasRect;     //canvasRect Size in GraphViewer window
        private static Rect toolbarRect;    //toolbarRect Size in GraphViewer window

        ////////////////////////////////////
        /// Event
        private static Event e;
        private static Vector2 prevMousePos;

        ////////////////////////////////////////////////////////////////////////
        /// Conversion

        static Vector2 ClientToViewport(Vector2 pos)
        {
            return (pos - Translation) / ZoomFactor;
        }   //GUI Client pos To Canvas Pos

        static Vector2 ViewportToClient(Vector2 canvasPos)
        {
            return (canvasPos * ZoomFactor) + Translation;
        }   //Canvas pos to GUI pos

        static Vector2 ClientToViewportScript(VinoveScene scene, Vector2 pos)
        {
            return (pos - scene.Translation) / scene.ZoomFactor;
        }   //GUI Client pos To Canvas Pos

        static Vector2 ViewportToClientScript(VinoveScene scene, Vector2 canvasPos)
        {
            return (canvasPos * scene.ZoomFactor) + scene.Translation;
        }   //Canvas pos to GUI pos

        static Rect MinimapRect
        {
            get
            {
                Vector2 _canvasRectQuater = new Vector2((canvasRect.width / 4), ((canvasRect.height + toolbarRect.height) / 4));
                return new Rect(_canvasRectQuater.x * 3, _canvasRectQuater.y * 3, _canvasRectQuater.x, _canvasRectQuater.y);
            }
        }
        static Rect MinimapDetectRect
        {
            get { return new Rect(-canvasRect.width + 150, -canvasRect.height + 150, canvasRect.width * 3 - 300, canvasRect.height * 3 - 300); }
        }

        ////////////////////////////////////////////////////////////////////////
        /// Properties

        private static Vector2 mousePosInClient
        {
            get {return ViewportToClient(Event.current.mousePosition); }
        }   //The mouse position in the canvas

        private static Vector2 mousePosInViewport
        {
            get { return ClientToViewport(Event.current.mousePosition); }
        }   //The mouse position in the canvas

        private static Vector2 mousePosInClientScript
        {
            get { return ViewportToClientScript(currentScene, Event.current.mousePosition); }
        }   //The mouse position in the script viewer canvas

        private static Vector2 mousePosInViewportScript
        {
            get { return ClientToViewportScript(currentScene, Event.current.mousePosition); }
        }   //The mouse position in the script viewer canvas

        private static float ZoomFactor
        {
            get { return currentVinove != null ? Mathf.Clamp(currentVinove.ZoomFactor, 0.25f, 1f) : 1f; }
            set { if (currentVinove != null) currentVinove.ZoomFactor = Mathf.Clamp(value, 0.25f, 1f); }
        }   //Zoom distance in GraphViewer

        private static Vector2 Translation
        {
            get { return currentVinove != null ? currentVinove.Translation : default; }
            set
            {
                var t = value;

                t.x = Mathf.Round(t.x);     //round for pos correction
                t.y = Mathf.Round(t.y);     //round for pos correction

                currentVinove.Translation = t;  //rewrite currentVinove data
            }
        }   //translation(GraphViewer) in Vinove class

        ////////////////////////////////////
        /// Initializer

        ////////////////////////////////////////////////////////////////////////
        /// Func
        /// 

        [MenuItem("Tools/ViNovE/New ViNovE")]
        /// <summary>
        /// <para> Eng. create and open file ViNovE. </para>
        /// <para> Kor. 새 ViNovE 파일을 생성하고 오픈합니다. </para> 
        /// </summary>
        public static void NewVinove()
        {
            string path = EditorUtility.SaveFilePanelInProject("Create New ViNovE", "NewVinove.asset", "asset", "");
            //string path = StandaloneFileBrowser.SaveFilePanel("New Vinove", Application.dataPath, "", "vnv");

            if (ErrorDetector.Instance.PathChecker(path) && path.StartsWith("Assets"))
            {
                currentVinovePath = path;
                currentVinove = ScriptableObject.CreateInstance<Vinove>();
                string _fileName = path.Substring(7, path.Length - 7);
                currentVinove.FileDir = Application.dataPath + "/" + _fileName;
                currentVinove.FileName = _fileName.Substring(0, _fileName.Length - 6);
                currentScene = null;

                Debug.Log(path + " ------------ " + currentVinove.FileDir + " ------------ " + currentVinove.FileName);

                AssetDatabase.CreateAsset(currentVinove, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(currentVinove);

                EditorUtility.FocusProjectWindow();
                Selection.activeObject = currentVinove;

                ShowWindow();
            }
        }

        [MenuItem("Tools/ViNovE/Open ViNovE")]
        /// <summary>
        /// <para> Eng. open already created ViNovE file. </para>
        /// <para> Kor. 기존에 생성된 ViNovE 파일을 오픈합니다. </para> 
        /// </summary>
        public static void OpenVinove()
        {
            string path = EditorUtility.OpenFilePanel("Open ViNovE", Application.dataPath, "asset");

            if (ErrorDetector.Instance.PathChecker(path) && path.StartsWith("Assets"))
            {
                currentVinovePath = GlobalFunc.Instance.PathAbsoluteToAssetRelative(path);
                AssetDatabase.ImportAsset(currentVinovePath);
                var currentLoaded = AssetDatabase.LoadAssetAtPath<Vinove>(currentVinovePath);
                EditorUtility.SetDirty(currentLoaded);
                currentVinove = currentLoaded as Vinove;
                string _fileName = path.Substring(7, path.Length - 7);
                currentVinove.FileDir = Application.dataPath + "/" + _fileName;
                currentVinove.FileName = _fileName.Substring(0, _fileName.Length - 6);

                Debug.Log(path + " ------------ " + currentVinove.FileDir + " ------------ " + currentVinove.FileName);

                ShowWindow();
            }
        }

        /// <summary>
        /// <para> Eng. open already created ViNovE .Asset file. </para>
        /// <para> Kor. 기존에 생성된 ViNovE .Asset 파일을 오픈합니다. </para> 
        /// </summary>
        /// <param name="openVinove"> <para> Eng. Detected open Vinove Asset </para>
        ///                           <para> Kor. 오픈 감지된 Vinove Asset </para> </param>
        public static void OpenVinove(Vinove openVinove)
        {
            currentVinove = openVinove;

            Debug.Log(currentVinove.FileDir + " ------------ " + currentVinove.FileName);

            ShowWindow();
        }

        /// <summary>
        /// <para> Eng. Open func double-clicked .asset file in unity editor </para>
        /// <para> Kor. 유니티 에디터 상에서 더블 클릭된 Asset을 열어주는  함수입니다 </para> 
        /// </summary>
        [OnOpenAssetAttribute(1)]
        public static bool OpenVinoveAsset(int instanceID, int line)
        {
            Vinove openedVinove = EditorUtility.InstanceIDToObject(instanceID) as Vinove;
            if(openedVinove != null)
            {
                currentVinovePath = AssetDatabase.GetAssetPath(instanceID);
                EditorUtility.SetDirty(openedVinove);
                OpenVinove(openedVinove);
                return true;
            }
            return false;
        }

        /// <summary>
        /// <para> Eng. Show window when you clicked menu item called "Scene Designer" </para>
        /// <para> Kor. "Window/Scene Designer" 메뉴 아이템을 클릭할 때, 윈도우를 보이게 합니다. </para> 
        /// </summary>
        public static void ShowWindow()
        {
            Init();

            currentGraphViewer = GetWindow<GraphViewer>("GraphViewer");
            currentGraphViewer.minSize = new Vector2(600f, 300f);
        }

        /// <summary>
        /// <para> Eng. Called when first editor window opened</para>
        /// <para> Kor. 에디터 윈도우가 처음 활성화 될 때 발생합니다. </para>
        /// </summary>
        private static void OnEnable()
        {
            
        }

        /// <summary>
        /// <para> Eng. Initialize func for need init data</para>
        /// <para> Kor. 별도로 초기화가 필요한 데이터들에 대한 초기화 함수입니다. </para>
        /// </summary>
        private static void Init()
        {
            scriptViewer = false;
            InitUndoStack();

            string _sheetPath = Application.dataPath + Constants.SHEET_RELATIVE_PATH;

            if (!System.IO.File.Exists(_sheetPath))
            {
                ViewerStyleSheetCreator test = new ViewerStyleSheetCreator();
                test.Load("Assets" + Constants.SHEET_RELATIVE_PATH);
                ViewerStyleSheet.Load();
            }
        }

        /// <summary>
        /// <para> Eng. Called when editor window closed </para>
        /// <para> Kor. 에디터 윈도우가 닫힐 때 발생합니다. </para>
        /// </summary>
        private void OnDestroy()
        {
            AssetDatabase.Refresh();
        }

        //------------------------- Draw -------------------------

        /// <summary>
        /// <para> Eng. Similar Update func. Not called 1 per frame, called 1 per interaction </para>
        /// <para> Kor. Update() 함수와 비슷하지만, 1 프레임 단위가 아닌 1번의 상호작용 단위로 호출됩니다 </para>
        /// </summary>
        private void OnGUI()
        {
            if (currentVinove == null)
            {
                this.ShowNotification(new GUIContent("Open Vinove File"));
            }
            else
            {
                wantsMouseMove = true;

                GUI.color = Color.white;
                GUI.backgroundColor = Color.white;

                this.RemoveNotification();

                toolbarRect = new Rect(0, 0, position.width, TOP_MARGIN);
                canvasRect = Rect.MinMaxRect(0, TOP_MARGIN, position.width, position.height);

                if (scriptViewer)
                {   // if, wanna see script viewer
                    prevMousePos = mousePosInViewportScript;
                    e = Event.current;

                    ScriptGraphEvents(currentVinove, currentScene, mousePosInViewportScript);

                    //drawing on canvas area
                    GUI.Box(canvasRect, string.Empty, ViewerStyleSheet.CanvasBackground);  //canvas background
                    DrawGrid(canvasRect, currentScene.Translation, currentScene.ZoomFactor);

                    DrawGraphLink();

                    DrawScriptsGraph();

                    DrawScriptMinimap();
                }
                else
                {
                    prevMousePos = mousePosInViewport;
                    e = Event.current;

                    GraphEvents(currentVinove, mousePosInViewport);

                    //drawing on canvas area
                    GUI.Box(canvasRect, string.Empty, ViewerStyleSheet.CanvasBackground);  //canvas background
                    DrawGrid(canvasRect, Translation, ZoomFactor);

                    DrawGraphLink();

                    DrawScenesGraph();
                    DrawBranchGraph();
                    DrawMergeGraph();

                    DrawSceneMinimap();
                }

                GUI.Box(canvasRect, string.Empty, ViewerStyleSheet.CanvasBorder);  //canvas background

                GUI.Box(toolbarRect, string.Empty);  //toolbar background
                DrawToolbar(canvasRect);

                if (needRepaint || Event.current.type == EventType.MouseMove)
                {
                    Repaint();
                }
            }
        }

        /// <summary>
        /// <para>Eng. Draw grid on the window(Graph Viewer). </para>
        /// <para>Kor. Graph Viewer 윈도우의 캔버스 영역에 격자를 그려줍니다. </para>
        /// </summary>
        /// <param name="canvasRect"> <para> Eng. Rect size of draw area on window </para>
        ///                             <para> Kor. 윈도우의 캔버스 영역입니다. </para> </param>
        /// <param name="offset"> <para> Eng. Distance current point to origin center point </para> 
        ///                             <para> Kor. 현재 위치에서 기존 중앙 위치 까지의 거리입니다. </para></param>
        /// <param name="zoomDistance"> <para> Eng. Zoom distance (0.25 ~ 1.0)  </para>
        ///                             <para> Kor. 확대 배율입니다.(0.25 ~ 1.0) </para></param>
        static void DrawGrid(Rect canvasRect, Vector2 offset, float zoomDistance)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            //Vector2 AbsTranslation = new Vector2(Mathf.Abs(offset.x), Mathf.Abs(offset.y));

            Handles.color = new Color(0.3f, 0.3f, 0.3f, 1f);

            float drawGridSize = zoomDistance > 0.5f ? GRID_SIZE : GRID_SIZE * 5;
            float gridStep = drawGridSize * zoomDistance;

            float xDiff = offset.x % gridStep;
            float xStart = canvasRect.xMin + xDiff;
            float xEnd = canvasRect.xMax;

            float yDiff = offset.y % gridStep;
            float yStart = canvasRect.yMin + yDiff;
            float yEnd = canvasRect.yMax;

            if(xStart < canvasRect.xMin)
            {
                xStart += gridStep;
            }
            if (yStart < canvasRect.yMin)
            {
                yStart += gridStep;
            }

            for (float i = xStart; i < xEnd; i += gridStep)
            {
                Handles.DrawLine(new Vector3(i, canvasRect.yMin, 0), new Vector3(i, canvasRect.yMax, 0));
            }
            for (float i = yStart; i < yEnd; i += gridStep)
            {
                Handles.DrawLine(new Vector3(canvasRect.xMin, i, 0), new Vector3(canvasRect.xMax, i, 0));
            }

            Handles.color = Color.white;
        }

        /// <summary>
        /// <para>Eng. Draw graph structure of vinove scenes. </para>
        /// <para>Kor. 비노비 씬의 그래프 구조를 그려줍니다. </para>
        /// </summary>
        static void DrawScenesGraph()
        {
            foreach(VinoveScene scene in currentVinove.Scene)
            {   // repeat each vinove scene
                Rect sceneGraphSize = new Rect(scene.CalculateGraphPosition(Translation, ZoomFactor));

                if (ErrorDetector.Instance.IsCanvasContainedRect(canvasRect, sceneGraphSize))
                {   // if, canvas contains graph
                    GUI.Box(sceneGraphSize, scene.Name, ViewerStyleSheet.GraphNodes);

                    Rect prevLinkerSize = scene.LinkerPosition("Prev", Translation, ZoomFactor);
                    Rect nextLinkerSize = scene.LinkerPosition("Next", Translation, ZoomFactor);

                    if (GUI.Button(prevLinkerSize, "", ViewerStyleSheet.GraphNodeLinker))
                    {
                        if(linkerControllActivated == true)
                        {
                            if(ErrorDetector.Instance.IsStringHasData(scene.Prev))
                            {
                                if (scene.Prev.StartsWith("BRA"))
                                {
                                    VinoveBranch _beforePrevBranch = currentVinove.FindBranchWithUID(scene.Prev);
                                    int _index = _beforePrevBranch.FindUIDLinkIndex(scene.UID);
                                    _beforePrevBranch.BranchsUIDs[_index] = null;
                                }
                                else if (scene.Prev.StartsWith("SCE"))
                                {
                                    VinoveScene _beforePrevScene = currentVinove.FindSceneWithUID(scene.Prev);
                                    _beforePrevScene.Next = null;
                                }
                                else if (scene.Prev.StartsWith("MRG"))
                                {
                                    VinoveMerge _beforePrevMerge = currentVinove.FindMergeWithUID(scene.Prev);
                                    _beforePrevMerge.Next = null;
                                }
                            }

                            if (prevLinkUID.StartsWith("BRA"))
                            {
                                VinoveBranch _prevBranch = currentVinove.FindBranchWithUID(prevLinkUID);

                                _prevBranch.BranchsUIDs[prevBranchIndex] = scene.UID;
                                scene.Prev = _prevBranch.UID;
                            }
                            else if (prevLinkUID.StartsWith("SCE"))
                            {
                                VinoveScene _prevScene = currentVinove.FindSceneWithUID(prevLinkUID);
                                _prevScene.Next = scene.UID;
                                scene.Prev = _prevScene.UID;
                            }
                            else if (prevLinkUID.StartsWith("MRG"))
                            {
                                VinoveMerge _prevMerge = currentVinove.FindMergeWithUID(prevLinkUID);
                                _prevMerge.Next = scene.UID;
                                scene.Prev = _prevMerge.UID;
                            }

                            prevLinkUID = null;
                            linkerControllActivated = false;
                        }
                    }

                    if (GUI.Button(nextLinkerSize, "", ViewerStyleSheet.GraphNodeLinker))
                    {
                        if (ErrorDetector.Instance.IsStringHasData(scene.Next))
                        {
                            if (scene.Next.StartsWith("BRA"))
                            {
                                VinoveBranch _prevBranch = currentVinove.FindBranchWithUID(scene.Next);
                                _prevBranch.Prev = null;
                            }
                            else if (scene.Next.StartsWith("SCE"))
                            {
                                VinoveScene _prevScene = currentVinove.FindSceneWithUID(scene.Next);
                                _prevScene.Prev = null;
                            }
                            else if (scene.Next.StartsWith("MRG"))
                            {
                                VinoveMerge _prevMerge = currentVinove.FindMergeWithUID(scene.Next);
                                _prevMerge.DeletePrevUID(scene.UID);
                            }

                            scene.Next = null;
                        }

                        linkerControllActivated = true;
                        prevLinkUID = scene.UID;
                        prevLinkPos = ConvertLinkCenter(nextLinkerSize);
                    }

                    if (currentVinove.StarterUID == scene.UID)
                    {
                        VinoveScene _starterScene = currentVinove.FindSceneWithUID(currentVinove.StarterUID);
                        Texture2D _starterTex = ViewerStyleSheet.StarterMarker;
                        Rect _markerPos = _starterScene.CalculateGraphPosition(Translation, ZoomFactor);

                        float _markerScaleFactor = 5 * (ZoomFactor * 2);

                        _markerPos.width = _markerScaleFactor;
                        _markerPos.height = _markerScaleFactor;

                        _markerPos.x += _markerPos.width;
                        _markerPos.y += _markerPos.height;

                        GUI.DrawTexture(_markerPos, ViewerStyleSheet.StarterMarker);
                    }
                }
            }
        }

        /// <summary>
        /// <para>Eng. Draw graph structure of vinove branch. </para>
        /// <para>Kor. 비노비 분기 그래프 구조를 그려줍니다. </para>
        /// </summary>
        static void DrawBranchGraph()
        {
            foreach (VinoveBranch branch in currentVinove.Branchs)
            {   // repeat each vinove scene
                Rect sceneGraphSize = new Rect(branch.CalculateBranchPosition(Translation, ZoomFactor));

                if (ErrorDetector.Instance.IsCanvasContainedRect(canvasRect, sceneGraphSize))
                {   // if, canvas contains graph
                    GUI.Box(sceneGraphSize, branch.Name, ViewerStyleSheet.GraphNodes);

                    Rect prevLinkerSize = branch.LinkerPosition("Prev", Translation, ZoomFactor);

                    if (GUI.Button(prevLinkerSize, "", ViewerStyleSheet.GraphNodeLinker))
                    {
                        if (linkerControllActivated == true)
                        {
                            if (ErrorDetector.Instance.IsStringHasData(branch.Prev))
                            {
                                if (branch.Prev.StartsWith("BRA"))
                                {
                                    VinoveBranch _beforePrevBranch = currentVinove.FindBranchWithUID(branch.Prev);
                                    int _index = _beforePrevBranch.FindUIDLinkIndex(branch.UID);
                                    _beforePrevBranch.BranchsUIDs[_index] = null;
                                }
                                else if (branch.Prev.StartsWith("SCE"))
                                {
                                    VinoveScene _beforePrevScene = currentVinove.FindSceneWithUID(branch.Prev);
                                    _beforePrevScene.Next = null;
                                }
                                else if (branch.Prev.StartsWith("MRG"))
                                {
                                    VinoveMerge _beforePrevMerge = currentVinove.FindMergeWithUID(branch.Prev);
                                    _beforePrevMerge.Next = null;
                                }
                            }

                            if (prevLinkUID.StartsWith("BRA"))
                            {
                                VinoveBranch _prevBranch = currentVinove.FindBranchWithUID(prevLinkUID);
                                _prevBranch.AddBranchNext(branch.UID, prevBranchIndex);
                                branch.Prev = _prevBranch.UID;
                            }
                            else if (prevLinkUID.StartsWith("SCE"))
                            {
                                VinoveScene _prevScene = currentVinove.FindSceneWithUID(prevLinkUID);
                                _prevScene.Next = branch.UID;
                                branch.Prev = _prevScene.UID;
                            }
                            else if (prevLinkUID.StartsWith("MRG"))
                            {
                                VinoveMerge _prevMerge = currentVinove.FindMergeWithUID(prevLinkUID);
                                _prevMerge.Next = branch.UID;
                                branch.Prev = _prevMerge.UID;
                            }

                            prevLinkUID = null;
                            linkerControllActivated = false;
                        }
                    }

                    for(int i=0; i<branch.BranchCount; i++)
                    {
                        Rect nextLinkerSize = branch.LinkerPosition("Next", Translation, ZoomFactor, i);
                        if (GUI.Button(nextLinkerSize, "", ViewerStyleSheet.GraphNodeLinker))
                        {
                            if (ErrorDetector.Instance.IsStringHasData(branch.BranchsUIDs[i]))
                            {
                                if (branch.BranchsUIDs[i].StartsWith("BRA"))
                                {
                                    VinoveBranch _prevBranch = currentVinove.FindBranchWithUID(branch.BranchsUIDs[i]);
                                    _prevBranch.Prev = null;
                                }
                                else if (branch.BranchsUIDs[i].StartsWith("SCE"))
                                {
                                    VinoveScene _prevScene = currentVinove.FindSceneWithUID(branch.BranchsUIDs[i]);
                                    _prevScene.Prev = null;
                                }
                                else if (branch.BranchsUIDs[i].StartsWith("MRG"))
                                {
                                    VinoveMerge _prevMerge = currentVinove.FindMergeWithUID(branch.BranchsUIDs[i]);
                                    _prevMerge.DeletePrevUID(branch.UID);
                                }

                                branch.BranchsUIDs[i] = null;
                            }
                            linkerControllActivated = true;
                            prevLinkUID = branch.UID;
                            prevLinkPos = ConvertLinkCenter(nextLinkerSize);
                            prevBranchIndex = i;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// <para>Eng. Draw graph structure of vinove merge. </para>
        /// <para>Kor. 비노비 병합 그래프 구조를 그려줍니다. </para>
        /// </summary>
        static void DrawMergeGraph()
        {
            foreach (VinoveMerge _merge in currentVinove.Merges)
            {   // repeat each vinove scene
                Rect sceneGraphSize = new Rect(_merge.CalculateMergePosition(Translation, ZoomFactor));

                if (ErrorDetector.Instance.IsCanvasContainedRect(canvasRect, sceneGraphSize))
                {   // if, canvas contains graph
                    GUI.Box(sceneGraphSize, _merge.Name, ViewerStyleSheet.GraphNodes);

                    for (int i = 0; i < _merge.MergeCount; i++)
                    {
                        Rect pevLinkerSize = _merge.LinkerPosition("Prev", Translation, ZoomFactor, i);
                        if (GUI.Button(pevLinkerSize, "", ViewerStyleSheet.GraphNodeLinker))
                        {
                            if (ErrorDetector.Instance.IsStringHasData(_merge.MergeUIDs[i]))
                            {
                                if (_merge.MergeUIDs[i].StartsWith("BRA"))
                                {
                                    VinoveBranch _beforePrevBranch = currentVinove.FindBranchWithUID(_merge.MergeUIDs[i]);
                                    int _index = _beforePrevBranch.FindUIDLinkIndex(_merge.UID);
                                    _beforePrevBranch.BranchsUIDs[_index] = null;
                                }
                                else if (_merge.MergeUIDs[i].StartsWith("SCE"))
                                {
                                    VinoveScene _beforePrevScene = currentVinove.FindSceneWithUID(_merge.MergeUIDs[i]);
                                    _beforePrevScene.Next = null;
                                }
                                else if (_merge.MergeUIDs[i].StartsWith("MRG"))
                                {
                                    VinoveMerge _beforePrevMerge = currentVinove.FindMergeWithUID(_merge.MergeUIDs[i]);
                                    int _index = _beforePrevMerge.FindUIDLinkIndex(_merge.UID);
                                    _beforePrevMerge.Next = null;
                                }
                            }

                            if (prevLinkUID.StartsWith("BRA"))
                            {
                                VinoveBranch _prevBranch = currentVinove.FindBranchWithUID(prevLinkUID);
                                if(_prevBranch.FindUIDLinkIndex(_merge.UID) != -1)
                                {   //if, return is not -1. then branch had already link with this merge.
                                    _prevBranch.DeleteNextUID(_merge.UID);
                                    _merge.DeletePrevUID(_prevBranch.UID);
                                }

                                _prevBranch.AddBranchNext(_merge.UID, prevBranchIndex);
                                _merge.MergeUIDs[i] = _prevBranch.UID;
                            }
                            else if (prevLinkUID.StartsWith("SCE"))
                            {
                                VinoveScene _prevScene = currentVinove.FindSceneWithUID(prevLinkUID);
                                _prevScene.Next = _merge.UID;
                                _merge.MergeUIDs[i] = _prevScene.UID;
                            }
                            else if (prevLinkUID.StartsWith("MRG"))
                            {
                                VinoveMerge _prevMerge = currentVinove.FindMergeWithUID(prevLinkUID);
                                _prevMerge.Next = _merge.UID;
                                _merge.MergeUIDs[i] = _prevMerge.UID;
                            }

                            prevLinkUID = null;
                            linkerControllActivated = false;
                        }
                    }

                    Rect nextLinkerSize = _merge.LinkerPosition("Next", Translation, ZoomFactor);
                    if (GUI.Button(nextLinkerSize, "", ViewerStyleSheet.GraphNodeLinker))
                    {
                        if (ErrorDetector.Instance.IsStringHasData(_merge.Next))
                        {
                            if (_merge.Next.StartsWith("BRA"))
                            {
                                VinoveBranch _prevBranch = currentVinove.FindBranchWithUID(_merge.Next);
                                _prevBranch.Prev = null;
                            }
                            else if (_merge.Next.StartsWith("SCE"))
                            {
                                VinoveScene _prevScene = currentVinove.FindSceneWithUID(_merge.Next);
                                _prevScene.Prev = null;
                            }
                            else if (_merge.Next.StartsWith("MRG"))
                            {
                                VinoveMerge _prevMerge = currentVinove.FindMergeWithUID(_merge.Next);
                                _prevMerge.DeletePrevUID(_merge.UID);
                            }

                            _merge.Next = null;
                        }

                        linkerControllActivated = true;
                        prevLinkUID = _merge.UID;
                        prevLinkPos = ConvertLinkCenter(nextLinkerSize);
                    }
                }
            }
        }

        /// <summary>
        /// <para>Eng. Draw graph structure of vinove scripts. </para>
        /// <para>Kor. 비노비 스크립트의 그래프 구조를 그려줍니다. </para>
        /// </summary>
        static void DrawScriptsGraph()
        {
            foreach (VinoveScript _script in currentScene.Scripts)
            {   // repeat each vinove scene
                Rect scriptGraphSize = new Rect(_script.CalculatePosition(currentScene.Translation, currentScene.ZoomFactor));

                if (ErrorDetector.Instance.IsCanvasContainedRect(canvasRect, scriptGraphSize))
                {   // if, canvas contains graph
                    GUI.Box(scriptGraphSize, _script.Name, ViewerStyleSheet.GraphNodes);

                    VinoveScene _parentScene = currentVinove.FindSceneWithUID(_script.ParentSceneUID);

                    Rect prevLinkerSize = _script.LinkerPosition("Prev", _parentScene.Translation, _parentScene.ZoomFactor);
                    Rect nextLinkerSize = _script.LinkerPosition("Next", _parentScene.Translation, _parentScene.ZoomFactor);

                    if (GUI.Button(prevLinkerSize, "", ViewerStyleSheet.GraphNodeLinker))
                    {
                        if (linkerControllActivated == true)
                        {
                            if (ErrorDetector.Instance.IsStringHasData(_script.Prev))
                            {   // before the change link, if graph already had prev link. need to delete [Next] of prev link
                                VinoveScript _beforePrevScript = _parentScene.FindScriptWithUID(_script.Prev);
                                _beforePrevScript.Next = null;
                            }

                            VinoveScript _prevScene = _parentScene.FindScriptWithUID(prevLinkUID);
                            _prevScene.Next = _script.UID;
                            _script.Prev = _prevScene.UID;

                            prevLinkUID = null;
                            linkerControllActivated = false;
                        }
                    }

                    if (GUI.Button(nextLinkerSize, "", ViewerStyleSheet.GraphNodeLinker))
                    {
                        if (ErrorDetector.Instance.IsStringHasData(_script.Next))
                        {
                            VinoveScript _beforeNextScript = _parentScene.FindScriptWithUID(_script.Next);
                            _beforeNextScript.Prev = null;

                            _script.Next = null;
                        }

                        linkerControllActivated = true;
                        prevLinkUID = _script.UID;
                        prevLinkPos = ConvertLinkCenter(nextLinkerSize);
                    }

                    if (ErrorDetector.Instance.IsStringHasData(currentScene.StarterUID))
                    {
                        VinoveScript _starterScript = currentScene.FindScriptWithUID(currentScene.StarterUID);

                        Texture2D _starterTex = ViewerStyleSheet.StarterMarker;
                        Rect _markerPos = _starterScript.CalculatePosition(currentScene.Translation, currentScene.ZoomFactor);

                        _markerPos.width = 10 * currentScene.ZoomFactor;
                        _markerPos.height = 10 * currentScene.ZoomFactor;

                        _markerPos.x += _markerPos.width;
                        _markerPos.y += _markerPos.height;

                        GUI.DrawTexture(_markerPos, ViewerStyleSheet.StarterMarker);
                    }
                }
            }
        }

        /// <summary>
        /// <para>Eng. Draw graph link. </para>
        /// <para>Kor. 그래프의 링크를 그려줍니다. </para>
        /// </summary>
        static void DrawGraphLink()
        {
            Handles.color = Color.grey;
            
            if (scriptViewer == false)
            {   // in Scene view
                foreach (VinoveScene _scene in currentVinove.Scene)
                {
                    if(ErrorDetector.Instance.IsStringHasData(_scene.Next))
                    {
                        Rect _sourceRect = _scene.LinkerPosition("Next", Translation, ZoomFactor);
                        Rect _targetRect = default;

                        if (_scene.Next.StartsWith("SCE"))
                        {
                            _targetRect = currentVinove.FindSceneWithUID(_scene.Next).LinkerPosition("Prev", Translation, ZoomFactor);
                        }
                        else if(_scene.Next.StartsWith("BRA"))
                        {
                            _targetRect = currentVinove.FindBranchWithUID(_scene.Next).LinkerPosition("Prev", Translation, ZoomFactor);
                        }
                        else if (_scene.Next.StartsWith("MRG"))
                        {
                            VinoveMerge _mergeTmp = currentVinove.FindMergeWithUID(_scene.Next);
                            _targetRect = _mergeTmp.LinkerPosition("Prev", Translation, ZoomFactor, _mergeTmp.FindUIDLinkIndex(_scene.UID));
                        }

                        DrawLinkLine(_sourceRect, _targetRect, ZoomFactor);
                    }
                }

                foreach (VinoveBranch _branch in currentVinove.Branchs)
                {
                    for (int i = 0; i < _branch.BranchCount; i++)
                    {
                        if (ErrorDetector.Instance.IsStringHasData(_branch.BranchsUIDs[i]))
                        {
                            Rect _sourceRect = _branch.LinkerPosition("Next", Translation, ZoomFactor, i);
                            Rect _targetRect = default;

                            if (_branch.BranchsUIDs[i].StartsWith("SCE"))
                            {
                                _targetRect = currentVinove.FindSceneWithUID(_branch.BranchsUIDs[i]).LinkerPosition("Prev", Translation, ZoomFactor);
                            }
                            else if (_branch.BranchsUIDs[i].StartsWith("BRA"))
                            {
                                _targetRect = currentVinove.FindBranchWithUID(_branch.BranchsUIDs[i]).LinkerPosition("Prev", Translation, ZoomFactor);
                            }
                            else if (_branch.BranchsUIDs[i].StartsWith("MRG"))
                            {
                                VinoveMerge _mergeTmp = currentVinove.FindMergeWithUID(_branch.BranchsUIDs[i]);
                                _targetRect = _mergeTmp.LinkerPosition("Prev", Translation, ZoomFactor, _mergeTmp.FindUIDLinkIndex(_branch.UID));
                            }

                            DrawLinkLine(_sourceRect, _targetRect, ZoomFactor);
                        }
                    }
                }

                foreach(VinoveMerge _merge in currentVinove.Merges)
                {
                    if(ErrorDetector.Instance.IsStringHasData(_merge.Next))
                    {
                        Rect _sourceRect = _merge.LinkerPosition("Next", Translation, ZoomFactor);
                        Rect _targetRect = default;

                        if (_merge.Next.StartsWith("SCE"))
                        {
                            _targetRect = currentVinove.FindSceneWithUID(_merge.Next).LinkerPosition("Prev", Translation, ZoomFactor);
                        }
                        else if (_merge.Next.StartsWith("BRA"))
                        {
                            _targetRect = currentVinove.FindBranchWithUID(_merge.Next).LinkerPosition("Prev", Translation, ZoomFactor);
                        }
                        else if (_merge.Next.StartsWith("MRG"))
                        {
                            VinoveMerge _mergeTmp = currentVinove.FindMergeWithUID(_merge.Next);
                            _targetRect = _mergeTmp.LinkerPosition("Prev", Translation, ZoomFactor, _mergeTmp.FindUIDLinkIndex(_merge.UID));
                        }

                        DrawLinkLine(_sourceRect, _targetRect, ZoomFactor);
                    }
                }
            }
            else
            {   // in Script view
                foreach(VinoveScript _script in currentScene.Scripts)
                {
                    if (ErrorDetector.Instance.IsStringHasData(_script.Next))
                    {
                        VinoveScene _parentScene = currentVinove.FindSceneWithUID(_script.ParentSceneUID);

                        Rect _sourceRect = _script.LinkerPosition("Next", _parentScene.Translation, _parentScene.ZoomFactor);
                        Rect _targetRect;

                        _targetRect = _parentScene.FindScriptWithUID(_script.Next).LinkerPosition("Prev", _parentScene.Translation, _parentScene.ZoomFactor);

                        DrawLinkLine(_sourceRect, _targetRect, _parentScene.ZoomFactor);
                    }
                }
            }

            Handles.color = new Color(0.6f, 0.6f, 0.6f);
            if (linkerControllActivated == true)
            {
                Handles.DrawAAPolyLine((ZoomFactor * 5), prevLinkPos, e.mousePosition);
            }
            Handles.color = Color.white;
        }

        /// <summary>
        /// <para>Eng. Draw Linked link line. </para>
        /// <para>Kor. 연결된 링크끼리의 선을 그려줍니다. </para>
        /// </summary>
        /// <param name="_sourceRect"> <para> Eng. Start Linker Rect </para>
        ///                             <para> Kor. 연결 시작부의 링커 위치입니다. </para> </param>
        /// <param name="_targetRect"> <para> Eng. End Linker Rect </para> 
        ///                             <para> Kor. 연결 도달부의 링커 위치입니다. </para></param>
        static void DrawLinkLine(Rect _sourceRect, Rect _targetRect, float _viewerZoomFactor)
        {
            if (canvasRect.Contains(new Vector2(_sourceRect.x, _sourceRect.y)) ||
                            canvasRect.Contains(new Vector2(_targetRect.x, _targetRect.y)))
            {   // if, canvas contains link
                Vector2 sourceLink = ConvertLinkCenter(_sourceRect);
                Vector2 targetLink = ConvertLinkCenter(_targetRect);

                Vector2 sourceLInkFront;
                Vector2 targetLinkFront;

                if (sourceLink.x > targetLink.x)
                {
                    sourceLInkFront = new Vector2(sourceLink.x + (sourceLink.x - targetLink.x) / 2, sourceLink.y);
                    targetLinkFront = new Vector2(targetLink.x - (sourceLink.x - targetLink.x) / 2, targetLink.y);

                    sourceLInkFront.x = Mathf.Clamp(sourceLInkFront.x, sourceLink.x, sourceLink.x+30);  
                    targetLinkFront.x = Mathf.Clamp(targetLinkFront.x, targetLink.x-30, sourceLink.x);
                }
                else
                {
                    sourceLInkFront = new Vector2(sourceLink.x + (targetLink.x - sourceLink.x) / 2, sourceLink.y);
                    targetLinkFront = new Vector2(targetLink.x - (targetLink.x - sourceLink.x) / 2, targetLink.y);
                }

                Handles.color = new Color(0.6f, 0.6f, 0.6f);
                Handles.DrawAAPolyLine((_viewerZoomFactor * 5), sourceLink, sourceLInkFront, targetLinkFront, targetLink);
                Handles.color = Color.white;
            }
        }


        /// <summary>
        /// <para>Eng. Zoom with focus. </para>
        /// <para>Kor. 포커스를 기준으로 확대/축소 합니다. </para>
        /// </summary>
        /// <param name="focus"> <para> Eng. it's mouse pos on canvas, focus for zoom in/out </para>
        ///                             <para> Kor. 캔버스 위의 마우스 위치로, 확대/축소할 포커스입니다 </para> </param>
        /// <param name="delta"> <para> Eng. Zoom factor </para> 
        ///                             <para> Kor. 확대/축소할 배율입니다. </para></param>
        static void ZoomWithFocus(Vector2 focus, float delta)
        {
            if (scriptViewer == true)
            {
                if (!(currentScene.ZoomFactor == 1 && delta > 0))
                {
                    Vector2 focusPos = ClientToViewportScript(currentScene, focus);

                    float tempZoomFactor = currentScene.ZoomFactor;
                    tempZoomFactor += delta;
                    tempZoomFactor = Mathf.Clamp(tempZoomFactor, 0.25f, 1f);
                    currentScene.ZoomFactor = tempZoomFactor;

                    Vector2 prevFocus = ViewportToClientScript(currentScene, focusPos);
                    Vector2 gap = focus - prevFocus;
                    currentScene.Translation += gap;
                }
            }
            else
            {
                if (!(ZoomFactor == 1 && delta > 0))
                {
                    Vector2 focusPos = ClientToViewport(focus);

                    float tempZoomFactor = ZoomFactor;
                    tempZoomFactor += delta;
                    tempZoomFactor = Mathf.Clamp(tempZoomFactor, 0.25f, 1f);
                    ZoomFactor = tempZoomFactor;

                    Vector2 prevFocus = ViewportToClient(focusPos);
                    Vector2 gap = focus - prevFocus;
                    Translation += gap;
                }
            }
        }

        //------------------------- Link -------------------------

        /// <summary>
        /// <para>Eng. Convert link pos Rect(x, y, width, height) to link center pos Vector2(x, y) </para>
        /// <para>Kor. 링크의 위치 정보 Rect(x, y, width, height)를 링크 중간 위치 정보(Vector2)로 변환합니다. </para>
        /// </summary>
        /// <param name="_linkRect"> <para> Eng. Convert target Link Rect </para>
        ///                           <para> Kor. 변환 될 링크 Rect 입니다. </para> </param>
        static Vector2 ConvertLinkCenter(Rect _linkRect)
        {
            return new Vector2(_linkRect.x + (_linkRect.width / 2), _linkRect.y + (_linkRect.height / 2));
        }

        /// <summary>
        /// <para>Eng. Tasks when graph deleted </para>
        /// <para>Kor. 그래프 삭제 시에 일어날 작업들입니다. </para>
        /// </summary>
        /// <param name="_deletedScene"> <para> Eng. Deleted Scene </para>
        ///                           <para> Kor. 삭제된 Scene 입니다 </para> </param>
        static void GraphDeleteSequence(string _deletedGraphUID, bool _stackFlag)
        {
            if (_deletedGraphUID.StartsWith("SCE"))
            {
                // ------------ Lower Script Delete ------------
                VinoveScene _deletedScene = currentVinove.FindSceneWithUID(_deletedGraphUID);

                if(_stackFlag == true)
                {
                    UndoStackDelete(_deletedGraphUID, _deletedScene);
                }

                foreach (VinoveScript _script in _deletedScene.Scripts)
                {
                    currentVinove.UIDs.Remove(_script.UID);
                }
                //_deletedScene.Scripts.Clear();

                // ------------ Links Delete ------------

                if (ErrorDetector.Instance.IsStringHasData(_deletedScene.Prev))
                {
                    if (_deletedScene.Prev.StartsWith("SCE"))
                    {
                        VinoveScene _prevScene = currentVinove.FindSceneWithUID(_deletedScene.Prev);
                        _prevScene.Next = null;
                    }
                    else if (_deletedScene.Prev.StartsWith("BRA"))
                    {
                        VinoveBranch _prevBranch = currentVinove.FindBranchWithUID(_deletedScene.Prev);
                        _prevBranch.DeleteNextUID(_deletedScene.UID);
                    }
                    else if (_deletedScene.Prev.StartsWith("MRG"))
                    {
                        VinoveMerge _prevMerge = currentVinove.FindMergeWithUID(_deletedScene.Prev);
                        _prevMerge.Next = null;
                    }
                }
                if (ErrorDetector.Instance.IsStringHasData(_deletedScene.Next))
                {
                    if (_deletedScene.Next.StartsWith("SCE"))
                    {
                        VinoveScene _nextScene = currentVinove.FindSceneWithUID(_deletedScene.Next);
                        _nextScene.Prev = null;
                    }
                    else if (_deletedScene.Next.StartsWith("BRA"))
                    {
                        VinoveBranch _nextBranch = currentVinove.FindBranchWithUID(_deletedScene.Next);
                        _nextBranch.Prev = null;
                    }
                    else if (_deletedScene.Next.StartsWith("MRG"))
                    {
                        VinoveMerge _nextMerge = currentVinove.FindMergeWithUID(_deletedScene.Next);
                        _nextMerge.DeletePrevUID(_deletedScene.UID);
                    }
                }

                // ------------ Scene Delete ------------
                string _deletedUID = _deletedScene.UID;

                currentVinove.UIDs.Remove(_deletedScene.UID);
                currentVinove.Scene.Remove(_deletedScene);

                if (currentScene == _deletedScene)
                {
                    currentScene = null;
                }

                if (currentVinove.StarterUID == _deletedUID)
                {
                    if (currentVinove.Scene.Count == 0)
                    {   // if, only left deleted script in scene scripts
                        currentVinove.StarterUID = null;
                    }
                    else
                    {   // else,
                        currentVinove.StarterUID = currentVinove.Scene[0].UID;
                    }
                }
            }
            else if (_deletedGraphUID.StartsWith("BRA"))
            {
                // ------------ Lower Script Delete ------------
                VinoveBranch _deletedBranch = currentVinove.FindBranchWithUID(_deletedGraphUID);

                if (_stackFlag == true)
                {
                    UndoStackDelete(_deletedGraphUID, _deletedBranch);
                }

                // ------------ Links Delete ------------
                if (ErrorDetector.Instance.IsStringHasData(_deletedBranch.Prev))
                {
                    if (_deletedBranch.Prev.StartsWith("SCE"))
                    {
                        VinoveScene _prevScene = currentVinove.FindSceneWithUID(_deletedBranch.Prev);
                        _prevScene.Next = null;
                    }
                    else if (_deletedBranch.Prev.StartsWith("BRA"))
                    {
                        VinoveBranch _prevBranch = currentVinove.FindBranchWithUID(_deletedBranch.Prev);
                        _prevBranch.DeleteNextUID(_deletedBranch.UID);
                    }
                    else if (_deletedBranch.Prev.StartsWith("MRG"))
                    {
                        VinoveMerge _prevMerge = currentVinove.FindMergeWithUID(_deletedBranch.Prev);
                        _prevMerge.Next = null;
                    }
                }

                for (int i = 0; i < _deletedBranch.BranchCount; i++)
                {
                    if (ErrorDetector.Instance.IsStringHasData(_deletedBranch.BranchsUIDs[i]))
                    {
                        if (_deletedBranch.BranchsUIDs[i].StartsWith("SCE"))
                        {
                            VinoveScene _nextScene = currentVinove.FindSceneWithUID(_deletedBranch.BranchsUIDs[i]);
                            _nextScene.Prev = null;
                        }
                        else if (_deletedBranch.BranchsUIDs[i].StartsWith("BRA"))
                        {
                            VinoveBranch _nextBranch = currentVinove.FindBranchWithUID(_deletedBranch.BranchsUIDs[i]);
                            _nextBranch.Prev = null;
                        }
                        else if (_deletedBranch.BranchsUIDs[i].StartsWith("MRG"))
                        {
                            VinoveMerge _nextMerge = currentVinove.FindMergeWithUID(_deletedBranch.BranchsUIDs[i]);
                            _nextMerge.DeletePrevUID(_deletedBranch.UID);
                        }
                    }
                }

                currentVinove.UIDs.Remove(_deletedBranch.UID);
                currentVinove.Branchs.Remove(_deletedBranch);
            }
            else if (_deletedGraphUID.StartsWith("MRG"))
            {
                // ------------ Lower Script Delete ------------
                VinoveMerge _deletedMerge = currentVinove.FindMergeWithUID(_deletedGraphUID);

                if (_stackFlag == true)
                {
                    UndoStackDelete(_deletedGraphUID, _deletedMerge);
                }

                // ------------ Links Delete ------------
                for (int i = 0; i < _deletedMerge.MergeCount; i++)
                {
                    if (ErrorDetector.Instance.IsStringHasData(_deletedMerge.MergeUIDs[i]))
                    {
                        if (_deletedMerge.MergeUIDs[i].StartsWith("SCE"))
                        {
                            VinoveScene _nextScene = currentVinove.FindSceneWithUID(_deletedMerge.MergeUIDs[i]);
                            _nextScene.Next = null;
                        }
                        else if (_deletedMerge.MergeUIDs[i].StartsWith("BRA"))
                        {
                            VinoveBranch _nextBranch = currentVinove.FindBranchWithUID(_deletedMerge.MergeUIDs[i]);
                            _nextBranch.DeleteNextUID(_deletedMerge.UID);
                        }
                        else if (_deletedMerge.MergeUIDs[i].StartsWith("MRG"))
                        {
                            VinoveMerge _nextMerge = currentVinove.FindMergeWithUID(_deletedMerge.MergeUIDs[i]);
                            _nextMerge.Next = null;
                        }
                    }
                }


                if (ErrorDetector.Instance.IsStringHasData(_deletedMerge.Next))
                {
                    if (_deletedMerge.Next.StartsWith("SCE"))
                    {
                        VinoveScene _prevScene = currentVinove.FindSceneWithUID(_deletedMerge.Next);
                        _prevScene.Prev = null;
                    }
                    else if (_deletedMerge.Next.StartsWith("BRA"))
                    {
                        VinoveBranch _prevBranch = currentVinove.FindBranchWithUID(_deletedMerge.Next);
                        _prevBranch.Prev = null;
                    }
                    else if (_deletedMerge.Next.StartsWith("MRG"))
                    {
                        VinoveMerge _prevMerge = currentVinove.FindMergeWithUID(_deletedMerge.Next);
                        _prevMerge.DeletePrevUID(_deletedMerge.UID);
                    }
                }

                currentVinove.UIDs.Remove(_deletedMerge.UID);
                currentVinove.Merges.Remove(_deletedMerge);
            }
            else if (_deletedGraphUID.StartsWith("SCR"))
            {
                VinoveScript _deletedScript = currentScene.FindScriptWithUID(_deletedGraphUID);
                VinoveScene _deletedScriptParent = currentVinove.FindSceneWithUID(_deletedScript.ParentSceneUID);

                if (_stackFlag == true)
                {
                    UndoStackDelete(_deletedGraphUID, _deletedScript);
                }

                // ------------ Links Delete ------------

                if (ErrorDetector.Instance.IsStringHasData(_deletedScript.Prev))
                {
                    VinoveScript _prevScript = _deletedScriptParent.FindScriptWithUID(_deletedScript.Prev);
                    _prevScript.Next = null;
                }
                if (ErrorDetector.Instance.IsStringHasData(_deletedScript.Next))
                {
                    VinoveScript _nextScript = _deletedScriptParent.FindScriptWithUID(_deletedScript.Next);
                    _nextScript.Prev = null;
                }

                // ------------ Script Delete ------------

                string _deletedUID = _deletedScript.UID;

                currentVinove.UIDs.Remove(_deletedScript.UID);
                _deletedScriptParent.Scripts.Remove(_deletedScript);

                if (currentScene.StarterUID == _deletedUID)
                {
                    if (currentScene.Scripts.Count == 0)
                    {   // if, only left deleted script in scene scripts
                        currentScene.StarterUID = null;
                    }
                    else
                    {   // else,
                        currentScene.StarterUID = currentScene.Scripts[0].UID;
                    }
                }
            }
        }
    }
}
#endif