#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ViNovE.Framework;
using ViNovE.Framework.Data;

namespace ViNovE.Framework
{
    public class SceneBackgroundInspector : PopupWindowContent
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        private const float _windowSizeWidth = 340;
        private const float _windowSizeHeight = 90;

        private int _selectedBackgroundImgIndex = 0;
        //private int _selectedBackgroundMusicIndex = 0;

        Vinove _currentVinove;
        VinoveScene _currentScene;

        string[] _backgroundIllustItems;
        string[] _backgroundMusicItems;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. Inspector for edit Scene Background Image, BGM</para>
        /// <para>Kor. 씬의 배경 이미지, 배경음을 설정하는 인스펙터입니다. </para>
        /// </summary>
        /// <param name="_vinove"> <para> Eng. Parent Vinove class of Scene Class</para>
        ///                         <para> Kor. 씬 클래스를 지닌 Vinove 클래스입니다. </para> </param>
        /// <param name="_scene"> <para> Eng. IMG, BGM change target Scene class.</para>
        ///                         <para> Kor. 이미지, 배경음을 변경할 씬 클래스입니다. </para> </param>
        public SceneBackgroundInspector(Vinove _vinove, VinoveScene _scene)
        {
            _currentVinove = _vinove;
            _currentScene = _scene;

            _backgroundIllustItems = new string[_currentVinove.SpeakerIllustrations.Count];
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

            GUI.Label(new Rect(10, 10, 410, 20), "Scene Background Inspector");

            //---------------------------------------------------------------------

            EditorGUI.LabelField(new Rect(20, 40, 130, 20), "Background Image");
            if (_currentVinove.BackgroundIllustrations.Count != 0)
            {
                _backgroundIllustItems = _currentVinove.BackgroundIllustrations.GetKeys();
                _selectedBackgroundImgIndex = GlobalFunc.Instance.FindStringIndexInArray(_backgroundIllustItems, _currentScene.BackgroundIllustKey);

                _selectedBackgroundImgIndex = EditorGUI.Popup(new Rect(140, 40, 180, 20), _selectedBackgroundImgIndex, _backgroundIllustItems);
                _currentScene.BackgroundIllustKey = _backgroundIllustItems[_selectedBackgroundImgIndex];
            }
            else
            {
                EditorGUI.LabelField(new Rect(140, 40, 180, 20), "No Background Image");
            }

            EditorGUI.LabelField(new Rect(20, 60, 130, 20), "Background Music");
            _currentScene.BackgroundMusic = EditorGUI.ObjectField(new Rect(140, 60, 180, 20), _currentScene.BackgroundMusic, typeof(AudioClip), false) as AudioClip;
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