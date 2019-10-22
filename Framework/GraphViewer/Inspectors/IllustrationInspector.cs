#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ViNovE.Framework.Data;
using ViNovE.Framework.Error;

namespace ViNovE.Framework
{
    public class IllustrationInspector : PopupWindowContent
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        private const float _windowSizeWidth = 235;
        private const float _windowSizeHeight = 600;

        private static Vinove _currentVinove;

        private static string _category;

        Texture2D _addedIllustration = null;
        string _addedIllustKey = "";

        int _popupSelectionIndex = 0;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. Inspector for add Speaker, BackgroundIMG illust</para>
        /// <para>Kor. 화자, 배경 일러스트를 추가할 수 있는 인스펙터입니다. </para>
        /// </summary>
        /// <param name="_vinove"> <para> Eng. Vinove class for Add illust</para>
        ///                         <para> Kor. 추가할 일러스트를 지닐 Vinove 클래스입니다. </para> </param>
        /// <param name="_IlluType"> <para> Eng. Types of Illustrations(Background or Speaker) to add</para>
        ///                         <para> Kor. 추가할 일러스트의 종류(배경 or 화자)입니다.</para> </param>
        public IllustrationInspector(Vinove _vinove, string _IlluType)
        {
            _currentVinove = _vinove;
            _category = _IlluType;
        }

        /// <summary>
        /// <para>Eng. Rendering and GUI func of Popup Window </para>
        /// <para>Kor. 팝업 윈도우의 렌더링 및 GUI 이벤트 처리 함수입니다. </para>
        /// </summary>
        /// <param name="_popupPos"> <para> Eng. Position of Popup Window</para>
        ///                         <para> Kor. 팝업의 위치입니다. </para> </param>
        public override void OnGUI(Rect _popupPos)
        {
            switch (_category)
            {
                case "Speaker":
                    DrawSpeakerInspector();
                    break;
                case "Background":
                    DrawBackgroundInspector();
                    break;
            }
            
        }

        /// <summary>
        /// <para>Eng. Inspector Func for add Speaker Illust </para>
        /// <para>Kor. 화자 일러스트 추가용 인스펙터 함수입니다. </para>
        /// </summary>
        private void DrawSpeakerInspector()
        {
            string[] speakerIllusts;

            //---------------------------------------------------------------------

            GUI.Label(new Rect(10, 10, 410, 20), _category + " Inspector");

            //---------------------------------------------------------------------

            if (_currentVinove.SpeakerIllustrations.Count != 0)
            {
                speakerIllusts = new string[_currentVinove.SpeakerIllustrations.Count];
                speakerIllusts = _currentVinove.SpeakerIllustrations.GetKeys();

                _popupSelectionIndex = EditorGUI.Popup(new Rect(20, 40, 195, 20), _popupSelectionIndex, speakerIllusts);

                Texture2D _selectedSpeaker = new Texture2D(0, 0);
                _currentVinove.SpeakerIllustrations.GetTextureData(speakerIllusts[_popupSelectionIndex], out _selectedSpeaker);

                GUI.DrawTexture(new Rect(20, 70, 195, 195), _selectedSpeaker); // Img Drawing

                if (GUI.Button(new Rect(20, 275, 195, 20), "Delete"))
                {   // Delete Speaker Illustration
                    _currentVinove.SpeakerIllustrations.Remove(speakerIllusts[_popupSelectionIndex]);
                    _popupSelectionIndex = 0;
                }
            }
            else
            {
                EditorGUI.LabelField(new Rect(20, 50, 195, 20), "No " + _category + " Image");

                EditorGUI.LabelField(new Rect(20, 195, 195, 20), "No Selected Image"); // Img Drawing
            }

            //---------------------------------------------------------------------

            _addedIllustration = EditorGUI.ObjectField(new Rect(20, 335, 195, 195), _addedIllustration, typeof(Texture2D), false) as Texture2D;

            _addedIllustKey = GUI.TextField(new Rect(20, 540, 195, 20), _addedIllustKey);

            if (GUI.Button(new Rect(20, 570, 195, 20), "Add"))
            {
                if (_addedIllustration == null)
                {
                    ErrorMessages.Instance.NoSelectionImage();
                } 
                else if(_addedIllustKey.Length == 0 || _currentVinove.SpeakerIllustrations.ContainsKey(_addedIllustKey) == true)
                {
                    ErrorMessages.Instance.WrongSpeakerBackgroundNameData();
                }
                else
                {
                    string _addedIllustPath = AssetDatabase.GetAssetPath(_addedIllustration);

                    // -------- Resize ---------
                    Rect _speakerTexRect = new Rect(0, 0, _addedIllustration.width, _addedIllustration.height);
                    float _resolutionY = Screen.currentResolution.height - ((Screen.currentResolution.height / 10) * 2);

                    if (_speakerTexRect.height > _resolutionY)
                    {
                        ErrorMessages.Instance.BiggerTextureSize();
                    }

                    _currentVinove.SpeakerIllustrations.Add(_addedIllustKey, _addedIllustPath, _addedIllustration);
                    _popupSelectionIndex = 0;
                    _addedIllustKey = "";
                    _addedIllustration = null;
                }
            }
        }

        /// <summary>
        /// <para>Eng. Inspector Func for add Background Illust </para>
        /// <para>Kor. 배경 일러스트 추가용 인스펙터 함수입니다. </para>
        /// </summary>
        private void DrawBackgroundInspector()
        {
            string[] backgroundIllusts;

            //---------------------------------------------------------------------

            GUI.Label(new Rect(10, 10, 410, 20), _category + " Inspector");

            //---------------------------------------------------------------------

            if (_currentVinove.BackgroundIllustrations.Count != 0)
            {
                backgroundIllusts = new string[_currentVinove.BackgroundIllustrations.Count];
                backgroundIllusts = _currentVinove.BackgroundIllustrations.GetKeys();
                _popupSelectionIndex = EditorGUI.Popup(new Rect(20, 40, 195, 20), _popupSelectionIndex, backgroundIllusts);

                Texture2D _selectedBackground = new Texture2D(0, 0);
                _currentVinove.BackgroundIllustrations.GetTextureData(backgroundIllusts[_popupSelectionIndex], out _selectedBackground);

                GUI.DrawTexture(new Rect(20, 70, 195, 195), _selectedBackground); // Img Drawing

                if (GUI.Button(new Rect(20, 275, 195, 20), "Delete"))
                {   // Delete Speaker Illustration
                    _currentVinove.BackgroundIllustrations.Remove(backgroundIllusts[_popupSelectionIndex]);
                    _popupSelectionIndex = 0;
                }
            }
            else
            {
                EditorGUI.LabelField(new Rect(20, 50, 195, 20), "No " + _category + " Image");

                EditorGUI.LabelField(new Rect(20, 195, 195, 20), "No Selected Image"); // Img Drawing
            }

            //---------------------------------------------------------------------

            _addedIllustration = EditorGUI.ObjectField(new Rect(20, 335, 195, 195), _addedIllustration, typeof(Texture2D), false) as Texture2D;

            _addedIllustKey = GUI.TextField(new Rect(20, 540, 195, 20), _addedIllustKey);

            if (GUI.Button(new Rect(20, 570, 195, 20), "Add"))
            {
                if (_addedIllustration == null)
                {
                    ErrorMessages.Instance.NoSelectionImage();
                }
                else if (_addedIllustKey.Length == 0 || _currentVinove.BackgroundIllustrations.ContainsKey(_addedIllustKey) == true)
                {
                    ErrorMessages.Instance.WrongSpeakerBackgroundNameData();
                }
                else
                {
                    string _addedIllustPath = AssetDatabase.GetAssetPath(_addedIllustration);
                    _currentVinove.BackgroundIllustrations.Add(_addedIllustKey, _addedIllustPath, _addedIllustration);
                    _addedIllustKey = "";
                    _addedIllustration = null;
                    _popupSelectionIndex = 0;
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