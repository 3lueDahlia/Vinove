#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ViNovE.Framework.Data;

namespace ViNovE.Framework
{
    public class RenameInspector : PopupWindowContent
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables
        
        private const float _windowSizeWidth = 200;
        private const float _windowSizeHeight = 70;

        string _sourceUID;

        Vinove _currentVinove;

        VinoveScene _scene;
        VinoveBranch _branch;
        VinoveMerge _merge;
        VinoveScript _script;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. Inspector for Scene, Branch, Merge Graph rename</para>
        /// <para>Kor. 씬, 분기, 병합 그래프의 이름을 변경하는 인스펙터입니다. </para>
        /// </summary>
        /// <param name="_vinove"> <para> Eng. Parent vinove class of graph</para>
        ///                         <para> Kor. 해당 그래프를 지니고 있는 Vinove 클래스 입니다. </para> </param>
        /// <param name="_uid"> <para> Eng. UID of rename target Graph</para>
        ///                         <para> Kor. 이름을 변경할 대상 그래프의 UID 입니다 </para> </param>
        public RenameInspector(Vinove _vinove, string _uid)
        {
            _currentVinove = _vinove;
            _sourceUID = _uid;

            if(_sourceUID.StartsWith("BRA"))
            {
                _branch = _currentVinove.FindBranchWithUID(_sourceUID);
            }
            else if (_sourceUID.StartsWith("MRG"))
            {
                _merge = _currentVinove.FindMergeWithUID(_sourceUID);
            }
            else if (_sourceUID.StartsWith("SCE"))
            {
                _scene = _currentVinove.FindSceneWithUID(_sourceUID);
            }
        }

        /// <summary>
        /// <para>Eng. Inspector for Script Graph rename</para>
        /// <para>Kor. 스크립트 그래프의 이름을 변경하는 인스펙터입니다. </para>
        /// </summary>
        /// <param name="_vinove"> <para> Eng. Parent vinove class of graph</para>
        ///                         <para> Kor. 해당 그래프를 지니고 있는 Vinove 클래스 입니다. </para> </param>
        /// <param name="_scene"> <para> Eng. parent scene class of Scrip</para>
        ///                         <para> Kor. 이름 변경 대상 스크립트를 지닌 씬 클래스 입니다. </para> </param>
        /// <param name="_uid"> <para> Eng. UID of rename target Graph</para>
        ///                         <para> Kor. 이름을 변경할 대상 그래프의 UID 입니다 </para> </param>
        public RenameInspector(Vinove _vinove, VinoveScene _scene, string _uid)
        {
            _currentVinove = _vinove;
            _sourceUID = _uid;

            if (_sourceUID.StartsWith("SCR"))
            {
                _script = _scene.FindScriptWithUID(_uid);
            }
        }

        /// <summary>
        /// <para>Eng. Rendering and GUI func of Popup Window </para>
        /// <para>Kor. 팝업 윈도우의 렌더링 및 GUI 이벤트 처리 함수입니다. </para>
        /// </summary>
        /// <param name="_popupPos"> <para> Eng. Position of Popup Window</para>
        ///                         <para> Kor. 팝업의 위치입니다. </para> </param>
        public override void OnGUI(Rect _popupPos)
        {
            //---------------------------------------------------------------------

            GUI.Label(new Rect(10, 10, 170, 20), "Rename Inspector");

            //---------------------------------------------------------------------

            EditorGUI.LabelField(new Rect(20, 40, 80, 20), "Name");
            if (_branch != null)
            {
                _branch.Name = EditorGUI.TextField(new Rect(80, 40, 100, 20), _branch.Name);
            }
            else if (_merge != null)
            {
                _merge.Name = EditorGUI.TextField(new Rect(80, 40, 100, 20), _merge.Name);
            }
            else if(_scene != null)
            {
                _scene.Name = EditorGUI.TextField(new Rect(80, 40, 100, 20), _scene.Name);
            }
            else if(_script != null)
            {
                _script.Name = EditorGUI.TextField(new Rect(80, 40, 100, 20), _script.Name);
            }
        }

        /// <summary>
        /// <para>Eng. Return size of Popup Window </para>
        /// <para>Kor. 팝업 윈도우의 크기를 반환합니다. </para>
        /// </summary>
        /// <returns>
        /// <para>Eng. Size of Popup Window (Type : Vector2) </para>
        /// <para>Kor. 팝업 윈도우의 크기입니다. (자료형 : Vector2) </para>
        /// </returns>
        public override Vector2 GetWindowSize()
        {
            return new Vector2(_windowSizeWidth, _windowSizeHeight);
        }
    }
}
#endif