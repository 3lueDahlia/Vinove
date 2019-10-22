#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ViNovE.Framework.Data;

namespace ViNovE.Framework
{
    public class BranchCountInspector : PopupWindowContent
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        private const float _windowSizeWidth = 200;
        private const float _windowSizeHeight = 70;

        private const int _countMin = 2;
        private const int _countMax = 5;

        string _sourceUID;

        Vinove _currentVinove;
        VinoveBranch _currentBranch;
        VinoveMerge _currentMerge;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. Inspector for edit Branch link count</para>
        /// <para>Kor. 분기의 링크 개수를 편집하는 인스펙터입니다. </para>
        /// </summary>
        /// <param name="_vinove"> <para> Eng. Parent Vinove class of Branch Class</para>
        ///                         <para> Kor. 분기 클래스를 지닌 Vinove 클래스입니다. </para> </param>
        /// <param name="_branch"> <para> Eng. Link count change target branch class.</para>
        ///                         <para> Kor. 링크 개수 변경 대상 분기 클래스입니다. </para> </param>
        public BranchCountInspector(Vinove _vinove, VinoveBranch _branch)
        {
            _currentVinove = _vinove;
            _currentBranch = _branch;
        }

        /// <summary>
        /// <para>Eng. Inspector for edit Merge link count</para>
        /// <para>Kor. 병합의 링크 개수를 편집하는 인스펙터입니다. </para>
        /// </summary>
        /// <param name="_vinove"> <para> Eng. Parent Vinove class of Merge Class</para>
        ///                         <para> Kor. 병합 클래스를 지닌 Vinove 클래스입니다. </para> </param>
        /// <param name="_merge"> <para> Eng. Link count change target merge class.</para>
        ///                         <para> Kor. 링크 개수 변경 대상 병합 클래스입니다. </para> </param>
        public BranchCountInspector(Vinove _vinove, VinoveMerge _merge)
        {
            _currentVinove = _vinove;
            _currentMerge = _merge;
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

            GUI.Label(new Rect(10, 10, 190, 20), "Branch Count Inspector");

            //---------------------------------------------------------------------

            EditorGUI.LabelField(new Rect(20, 40, 80, 20), "Count");

            if(_currentBranch != null)
            {
                int _changeCount = EditorGUI.IntField(new Rect(80, 40, 100, 20), _currentBranch.BranchCount);

                if (_changeCount != _currentBranch.BranchCount)
                {
                    _changeCount = Mathf.Clamp(_changeCount, _countMin, _countMax);

                    _currentBranch.ChangeBranchCount(_currentVinove, _changeCount, _countMin, _countMax);
                }
            }
            else if(_currentMerge != null)
            {
                int _countTmp = EditorGUI.IntField(new Rect(80, 40, 100, 20), _currentMerge.MergeCount);

                if (_countTmp != _currentMerge.MergeCount)
                {
                    _countTmp = Mathf.Clamp(_countTmp, _countMin, _countMax);
                    _currentMerge.ChangeMergeCount(_currentVinove, _countTmp, _countMin, _countMax);
                }
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