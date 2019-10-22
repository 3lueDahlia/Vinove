#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ViNovE.Framework.Data;
using ViNovE.Framework.Error;

namespace ViNovE.Framework
{
    public class SpeakerDataInspector : PopupWindowContent
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        private const float _windowSizeWidth = 300;
        private const float _windowSizeHeight = 155;

        private static Vinove _currentVinove;

        string _inputtedSpeakerName = "";

        int _popupSpeakerSelectionIndex = 0;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. Inspector for add Speaker data</para>
        /// <para>Kor. 화자 정보를 추가할 수 있는 인스펙터입니다. </para>
        /// </summary>
        /// <param name="_vinove"> <para> Eng. Vinove class for Add speaker data</para>
        ///                         <para> Kor. 추가할 화자 정보를 지닐 Vinove 클래스입니다. </para> </param>
        public SpeakerDataInspector(Vinove _vinove)
        {
            _currentVinove = _vinove;
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

            GUI.Label(new Rect(10, 10, 270, 20), "Add Speaker Inspector");

            //---------------------------------------------------------------------

            string[] speakerPopupItems = _currentVinove.SpeakerNames.ToArray();

            if (_currentVinove.SpeakerNames.Count == 0)
            {   // if, speaker not resign in vinove
                GUI.Label(new Rect(20, 40, 260, 20), "Empty Speaker Data");
            }
            else
            {   // or
                _popupSpeakerSelectionIndex = EditorGUI.Popup(new Rect(20, 40, 260, 20), _popupSpeakerSelectionIndex, speakerPopupItems);

                if (GUI.Button(new Rect(20, 60, 260, 20), "Delete"))
                {   // Delete Speaker Illustration
                    _currentVinove.SpeakerNames.Remove(_currentVinove.SpeakerNames[_popupSpeakerSelectionIndex]);
                    _popupSpeakerSelectionIndex = 0;
                }
            }

            _inputtedSpeakerName = GUI.TextField(new Rect(20, 100, 260, 20), _inputtedSpeakerName);

            if (GUI.Button(new Rect(20, 125, 260, 20), "Add"))
            {
                if (_inputtedSpeakerName.Length == 0 || _currentVinove.SpeakerNames.Contains(_inputtedSpeakerName))
                {
                    ErrorMessages.Instance.WrongSpeakerBackgroundNameData();
                }
                else
                {
                    _currentVinove.SpeakerNames.Add(_inputtedSpeakerName);

                    _popupSpeakerSelectionIndex = 0;
                    _inputtedSpeakerName = "";
                }
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