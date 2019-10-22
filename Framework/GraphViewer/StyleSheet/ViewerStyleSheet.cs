#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ViNovE.Framework.Error;

namespace ViNovE.Framework
{
    
    public class ViewerStyleSheet : ScriptableObject
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables, Classes

        private static ViewerStyleSheet _styleSheet;
        private static ViewerStyleSheet StyleSheet
        {
            get { return _styleSheet = Resources.Load<ViewerStyleSheet>("StyleSheets/ViewerStyleSheetDark"); }
        }

        public static void Load()
        {
            _styleSheet = StyleSheet;
            if(_styleSheet == null)
            {
                ErrorMessages.Instance.MissingSingletoneObject("ViewerStyleSheet");
            }
        }
        
        [System.Serializable]
        public class Styles
        {
            public GUIStyle graphNodes;
            public GUIStyle graphNodeLinker;

            public GUIStyle canvasBackground;
            public GUIStyle canvasBorder;
            public GUIStyle scriptGraphInspector;

            public GUIStyle graphMinimap;
        }

        [System.Serializable]
        public class Icons
        {
            [Header("Toolbar")]
            public Texture2D backward;
            public Texture2D backwardDisabled;
            public Texture2D forward;
            public Texture2D forwardDisabled;
            public Texture2D save;
            public Texture2D undo;
            public Texture2D undoDisabled;
            public Texture2D redo;
            public Texture2D redoDisabled;

            public Texture2D setting;
            public Texture2D speakerIllust;
            public Texture2D backgroundIllust;

            public Texture2D starterMarker;
        }

        [ReadOnly]
        public Styles styles;
        [ReadOnly]
        public Icons icons;

        ///////////////////////////////////////////////
        /// Properties

        //////////////////////
        /// GUIStyles
        /// 
        public static GUIStyle GraphNodes
        {
            get { return StyleSheet.styles.graphNodes; }
        }
        public static GUIStyle GraphNodeLinker
        {
            get { return StyleSheet.styles.graphNodeLinker; }
        }

        public static GUIStyle CanvasBackground
        {
            get { return StyleSheet.styles.canvasBackground; }
        }
        public static GUIStyle CanvasBorder
        {
            get { return StyleSheet.styles.canvasBorder; }
        }
        public static GUIStyle ScriptGraphInspector
        {
            get { return StyleSheet.styles.scriptGraphInspector; }
        }


        public static GUIStyle GraphMinimap
        {
            get { return StyleSheet.styles.graphMinimap; }
        }



        //////////////////////
        /// Toolbars

        public static Texture2D Backward
        {
            get { return StyleSheet.icons.backward; }
        }
        public static Texture2D BackwardDisabled
        {
            get { return StyleSheet.icons.backwardDisabled; }
        }
        public static Texture2D Forward
        {
            get { return StyleSheet.icons.forward; }
        }
        public static Texture2D ForwardDisabled
        {
            get { return StyleSheet.icons.forwardDisabled; }
        }
        public static Texture2D Save
        {
            get { return StyleSheet.icons.save; }
        }
        public static Texture2D Undo
        {
            get { return StyleSheet.icons.undo; }
        }
        public static Texture2D UndoDisabled
        {
            get { return StyleSheet.icons.undoDisabled; }
        }
        public static Texture2D Redo
        {
            get { return StyleSheet.icons.redo; }
        }
        public static Texture2D RedoDisabled
        {
            get { return StyleSheet.icons.redoDisabled; }
        }

        public static Texture2D Setting
        {
            get { return StyleSheet.icons.setting; }
        }

        public static Texture2D SpeakerIllust
        {
            get { return StyleSheet.icons.speakerIllust; }
        }
        public static Texture2D BackgroundIllust
        {
            get { return StyleSheet.icons.backgroundIllust; }
        }

        public static Texture2D StarterMarker
        {
            get { return StyleSheet.icons.starterMarker; }
        }
    }
}
#endif