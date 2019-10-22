#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ViNovE.Framework
{
    public class ViewerStyleSheetCreator
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        static ViewerStyleSheet styleSheet;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        public void Load(string _sheetPath)
        {
            EditorStyles.popup.normal.background = null;
            
            styleSheet = ScriptableObject.CreateInstance<ViewerStyleSheet>();

            styleSheet.styles = new ViewerStyleSheet.Styles();
            styleSheet.icons = new ViewerStyleSheet.Icons();

            canvasBackground();
            canvasBorder();
            graphNodes();
            graphNodeLinker();

            graphMinimap();

            ToolbarIconLoad();

            AssetDatabase.CreateAsset(styleSheet, _sheetPath);
            EditorUtility.SetDirty(styleSheet);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// <para> Eng. Canvas background style sheet </para>
        /// <para> Kor. 캔버스의 배경 스타일 시트 설정입니다. </para> 
        /// </summary>
        void canvasBackground()
        {
            styleSheet.styles.canvasBackground = new GUIStyle()
            {
                name = "canvasBG"
            };

            styleSheet.styles.canvasBackground.normal.background = Resources.Load<Texture2D>("StyleSheets/Viewer/Dark/CanvasBackground");

            styleSheet.styles.canvasBackground.border.left = 5;
            styleSheet.styles.canvasBackground.border.right = 5;
            styleSheet.styles.canvasBackground.border.top = 5;
            styleSheet.styles.canvasBackground.border.bottom = 5;

            styleSheet.styles.canvasBackground.padding.left = 0;
            styleSheet.styles.canvasBackground.padding.right = 0;
            styleSheet.styles.canvasBackground.padding.top = 3;
            styleSheet.styles.canvasBackground.padding.bottom = 2;
        }

        /// <summary>
        /// <para> Eng. Canvas background style sheet </para>
        /// <para> Kor. 캔버스의 배경 스타일 시트 설정입니다. </para> 
        /// </summary>
        void canvasBorder()
        {
            styleSheet.styles.canvasBorder = new GUIStyle()
            {
                name = "canvasBorder"
            };

            styleSheet.styles.canvasBorder.normal.background = Resources.Load<Texture2D>("StyleSheets/Viewer/Dark/CanvasBorder");

            styleSheet.styles.canvasBorder.border.left = 1;
            styleSheet.styles.canvasBorder.border.right = 1;
            styleSheet.styles.canvasBorder.border.top = 1;
            styleSheet.styles.canvasBorder.border.bottom = 1;
        }

        /// <summary>
        /// <para> Eng. Graph Nodes style sheet </para>
        /// <para> Kor. 그래프 노드의 스타일 시트입니다. </para> 
        /// </summary>
        void graphNodes()
        {
            styleSheet.styles.graphNodes = new GUIStyle()
            {
                name = "graphNodes"
            };

            styleSheet.styles.graphNodes.normal.background = Resources.Load<Texture2D>("StyleSheets/Viewer/Dark/GraphNode");
            styleSheet.styles.graphNodes.hover.background = Resources.Load<Texture2D>("StyleSheets/Viewer/Dark/GraphNodeHover");

            styleSheet.styles.graphNodes.border.left = 3;
            styleSheet.styles.graphNodes.border.right = 3;
            styleSheet.styles.graphNodes.border.top = 3;
            styleSheet.styles.graphNodes.border.bottom = 3;

            styleSheet.styles.graphNodes.fontSize = 0;
            styleSheet.styles.graphNodes.alignment = TextAnchor.MiddleCenter;
        }

        /// <summary>
        /// <para> Eng. Graph Node Link style sheet </para>
        /// <para> Kor. 그래프 노드 링크의 스타일 시트입니다 </para> 
        /// </summary>
        void graphNodeLinker()
        {
            styleSheet.styles.graphNodeLinker = new GUIStyle
            {
                name = "graphNodeLinker"
            };

            styleSheet.styles.graphNodeLinker.normal.background = Resources.Load<Texture2D>("StyleSheets/Viewer/Dark/GraphNodeLinker");
            styleSheet.styles.graphNodeLinker.hover.background = Resources.Load<Texture2D>("StyleSheets/Viewer/Dark/GraphNodeLinkerHover");

            styleSheet.styles.graphNodeLinker.border.left = 0;
            styleSheet.styles.graphNodeLinker.border.right = 0;
            styleSheet.styles.graphNodeLinker.border.top = 0;
            styleSheet.styles.graphNodeLinker.border.bottom = 0;

            styleSheet.styles.graphNodeLinker.fontSize = 0;
            styleSheet.styles.graphNodeLinker.alignment = TextAnchor.MiddleCenter;
        }

        /// <summary>
        /// <para> Eng. Graph Minimap style sheet </para>
        /// <para> Kor. 그래프 미니맵의 스타일 시트입니다 </para> 
        /// </summary>
        void graphMinimap()
        {
            styleSheet.styles.graphMinimap = new GUIStyle
            {
                name = "graphMinimap"
            };

            styleSheet.styles.graphMinimap.normal.background = Resources.Load<Texture2D>("StyleSheets/Viewer/Dark/MinimapBackground");

            styleSheet.styles.graphMinimap.border.left = 0;
            styleSheet.styles.graphMinimap.border.right = 0;
            styleSheet.styles.graphMinimap.border.top = 0;
            styleSheet.styles.graphMinimap.border.bottom = 0;

            styleSheet.styles.graphMinimap.fontSize = 0;
            styleSheet.styles.graphMinimap.alignment = TextAnchor.MiddleCenter;
        }

        /// <summary>
        /// <para> Eng. Toolbar icon png impoter </para>
        /// <para> Kor. 툴바 아이콘을 불러오기 위한 함수입니다 </para> 
        /// </summary>
        void ToolbarIconLoad()
        {
            styleSheet.icons.backward = Resources.Load<Texture2D>("StyleSheets/Viewer/ToolbarIcons/Icon_Backward");
            styleSheet.icons.backwardDisabled = Resources.Load<Texture2D>("StyleSheets/Viewer/ToolbarIcons/Icon_BackwardDisabled");
            styleSheet.icons.forward = Resources.Load<Texture2D>("StyleSheets/Viewer/ToolbarIcons/Icon_Forward");
            styleSheet.icons.forwardDisabled = Resources.Load<Texture2D>("StyleSheets/Viewer/ToolbarIcons/Icon_ForwardDisabled");
            styleSheet.icons.save = Resources.Load<Texture2D>("StyleSheets/Viewer/ToolbarIcons/Icon_Save");
            styleSheet.icons.undo = Resources.Load<Texture2D>("StyleSheets/Viewer/ToolbarIcons/Icon_Undo");
            styleSheet.icons.undoDisabled = Resources.Load<Texture2D>("StyleSheets/Viewer/ToolbarIcons/Icon_UndoDisabled");
            styleSheet.icons.redo = Resources.Load<Texture2D>("StyleSheets/Viewer/ToolbarIcons/Icon_Redo");
            styleSheet.icons.redoDisabled = Resources.Load<Texture2D>("StyleSheets/Viewer/ToolbarIcons/Icon_RedoDisabled");

            styleSheet.icons.setting = Resources.Load<Texture2D>("StyleSheets/Viewer/ToolbarIcons/Icon_Setting");
            styleSheet.icons.speakerIllust = Resources.Load<Texture2D>("StyleSheets/Viewer/ToolbarIcons/Icon_SpeakerInspector");
            styleSheet.icons.backgroundIllust = Resources.Load<Texture2D>("StyleSheets/Viewer/ToolbarIcons/Icon_BackgroundInspector");

            styleSheet.icons.starterMarker = Resources.Load<Texture2D>("StyleSheets/Viewer/Dark/StarterMarker");
        }
    }
}
#endif