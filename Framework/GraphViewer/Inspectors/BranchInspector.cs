#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ViNovE.Framework.Data;

namespace ViNovE.Framework
{
    public class BranchInspector : PopupWindowContent
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        private const float _windowSizeWidth = 250;
        private const float _popupPaddingX = 20;
        private const float _popupPaddingY = 20;
        private const float _windowContentSizeY = 30;
        private float _windowSizeHeight = 0;

        private Vinove _currentVinove;
        private VinoveBranch _currentBranch;

        string[] _alterTmp;

        int _branchCount;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. Inspector for Branch data edit</para>
        /// <para>Kor. 분기의 정보를 편집하는 인스펙터입니다. </para>
        /// </summary>
        /// <param name="_vinove"> <para> Eng. Parent Vinove class of Branch</para>
        ///                         <para> Kor. 분기를 소지한 Vinove 클래스입니다. </para> </param>
        /// <param name="_branch"> <para> Eng. target Branch of Edit</para>
        ///                         <para> Kor. 편집 대상 분기입니다. </para> </param>
        public BranchInspector(Vinove _vinove, VinoveBranch _branch)
        {
            _currentVinove = _vinove;
            _currentBranch = _branch;

            _branchCount = _currentBranch.BranchCount;
            _windowSizeHeight = _popupPaddingY + _popupPaddingY + (_windowContentSizeY * _branchCount);
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

            GUI.Label(new Rect(10, 10, 190, 20), "Branch Alternative Inspector");

            //---------------------------------------------------------------------

            float contentSizeX = 90;
            float contentSizeY = 20;

            float contentPosY = _popupPaddingY + contentSizeY;

            for (int i=0; i<_branchCount; i++)
            {
                EditorGUI.LabelField(new Rect(_popupPaddingX, contentPosY, contentSizeX, contentSizeY), "Alternative " + (i + 1).ToString());
                _currentBranch.BranchsAlternatives[i] = EditorGUI.TextField(new Rect(_popupPaddingX + contentSizeX, contentPosY, contentSizeX, contentSizeY), _currentBranch.BranchsAlternatives[i]);

                contentPosY += _windowContentSizeY;
            }
        }

        public override void OnOpen()
        {

        }

        public override void OnClose()
        {

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