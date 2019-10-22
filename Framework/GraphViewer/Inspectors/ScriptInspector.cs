#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ViNovE.Framework;
using ViNovE.Framework.Enum;
using ViNovE.Framework.Animation;
using ViNovE.Framework.Data;

namespace ViNovE.Framework
{
    public class ScriptInspector : PopupWindowContent
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        private const float _windowSizeWidth = 300;
        private const float _windowSizeHeight = 330;

        private Vinove _currentVinove;
        private VinoveScript _currentScript;

        private int _popupSpeakerNameSelectedIndex = 0;
        private int _popupSpeakerIllustSelectedIndex = 0;
        private int _popupSpeakerPositionSelectedIndex = 0;
        private int _popupSpeakerAnimSelectedIndex = 0;

        string[] speakerIllustItems;
        string[] speakerPositionItems;
        string[] speakerAnimItems;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. Inspector for edit script </para>
        /// <para>Kor. 스크립트 편집을 위한 인스펙터입니다. </para>
        /// </summary>
        /// <param name="_vinove"> <para> Eng. Parent Vinove class of script Class</para>
        ///                         <para> Kor. 스크립트 클래스를 지니는 Vinove 클래스입니다. </para> </param>
        /// <param name="_script"> <para> Eng. target script class to edit</para>
        ///                         <para> Kor. 변경 대상 스크립트 클래스입니다. </para> </param>
        public ScriptInspector(Vinove _vinove, VinoveScript _script)
        {
            _currentVinove = _vinove;
            _currentScript = _script;

            speakerIllustItems = new string[_currentVinove.SpeakerIllustrations.Count];
            speakerPositionItems = Enum.EnumManager.Instance.GetSpeakerPositions();
            speakerAnimItems = Enum.EnumManager.Instance.GetAnimStates();
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
            GUI.Label(new Rect(10, 10, 410, 20), "Script Inspector");

            // ------------ Speaker - Name ------------

            EditorGUI.LabelField(new Rect(20, 40, 95, 20), "Speaker");
            if(_currentVinove.SpeakerNames.Count != 0)
            {
                string[] speakerNames = _currentVinove.SpeakerNames.ToArray();

                _popupSpeakerNameSelectedIndex = GlobalFunc.Instance.FindStringIndexInArray(speakerNames, _currentScript.ScriptConversation.Speaker);
                _popupSpeakerNameSelectedIndex = EditorGUI.Popup(new Rect(100, 40, 180, 20), _popupSpeakerNameSelectedIndex, speakerNames);

                _currentScript.ScriptConversation.Speaker = speakerNames[_popupSpeakerNameSelectedIndex];
            }
            else
            {
                EditorGUI.LabelField(new Rect(100, 40, 180, 20), "No Speaker Data");
                _currentScript.ScriptConversation.Speaker = "";
            }

            // ------------ Speaker - Illust ------------

            EditorGUI.LabelField(new Rect(20, 60, 95, 20), "Illust");
            if (_currentVinove.SpeakerIllustrations.Count != 0)
            {
                speakerIllustItems = _currentVinove.SpeakerIllustrations.GetKeys();
                _popupSpeakerIllustSelectedIndex = GlobalFunc.Instance.FindStringIndexInArray(speakerIllustItems, _currentScript.ScriptConversation.SpeakerIllustKey);

                _popupSpeakerIllustSelectedIndex = EditorGUI.Popup(new Rect(100, 60, 180, 20), _popupSpeakerIllustSelectedIndex, speakerIllustItems);
                _currentScript.ScriptConversation.SpeakerIllustKey = speakerIllustItems[_popupSpeakerIllustSelectedIndex];
            }
            else
            {
                EditorGUI.LabelField(new Rect(100, 60, 180, 20), "No Speaker Image");
            }

            // ------------ Speaker - Position ------------
            EditorGUI.LabelField(new Rect(20, 80, 95, 20), "Position");
            _popupSpeakerPositionSelectedIndex = GlobalFunc.Instance.FindStringIndexInArray(speakerPositionItems, _currentScript.ScriptConversation.SpeakerPos.ToString());

            _popupSpeakerPositionSelectedIndex = EditorGUI.Popup(new Rect(100, 80, 180, 20), _popupSpeakerPositionSelectedIndex, speakerPositionItems);
            _currentScript.ScriptConversation.SpeakerPos = EnumManager.Instance.ToEnum<Enum.SpeakerPos>(speakerPositionItems[_popupSpeakerPositionSelectedIndex]);

            // ------------ Anim ------------
            EditorGUI.LabelField(new Rect(20, 100, 95, 20), "Animation");
            _popupSpeakerAnimSelectedIndex = GlobalFunc.Instance.FindStringIndexInArray(speakerAnimItems, _currentScript.ScriptConversation.SpeakerAnim.ToString());

            _popupSpeakerAnimSelectedIndex = EditorGUI.Popup(new Rect(100, 100, 180, 20), _popupSpeakerAnimSelectedIndex, speakerAnimItems);
            _currentScript.ScriptConversation.SpeakerAnim = EnumManager.Instance.ToEnum<Enum.AnimationState>(speakerAnimItems[_popupSpeakerAnimSelectedIndex]);

            // ------------ Dialog ------------

            EditorGUI.LabelField(new Rect(20, 120, 95, 20), "Dialog");
            _currentScript.ScriptConversation.Dialog = EditorGUI.TextArea(new Rect(100, 120, 180, 170), _currentScript.ScriptConversation.Dialog);

            // ------------ Sound Effect ------------
            EditorGUI.LabelField(new Rect(20, 300, 95, 20), "Sound Effect");
            _currentScript.ScriptConversation.SoundEffect = EditorGUI.ObjectField(new Rect(100, 300, 180, 20), _currentScript.ScriptConversation.SoundEffect, typeof(AudioClip), false) as AudioClip;
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