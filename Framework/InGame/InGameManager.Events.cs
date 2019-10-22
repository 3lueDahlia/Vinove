using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ViNovE.Framework.Enum;
using ViNovE.Framework.InGame.SaveLoad;
using ViNovE.Framework.InGame.Option;

namespace ViNovE.Framework.InGame
{
    public partial class InGameManager
    {
        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. Pushed ESC menu Button in InGame State. </para>
        /// <para>Kor. 인 게임 상태에서, ESC 메뉴를 호출하는 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedEscapeMenuButton()
        {
            _uiState = InGameInterfaceDisplayState.EscapeMenu;
            _EscapeMenuPanel.SetActive(true);
        }


        /// <summary>
        /// <para>Eng. Pushed "Resume" Button in Escape Menu State. </para>
        /// <para>Kor. ESC 메뉴 상태에서, 게임 재개 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedEscapeMenuResumeButton()
        {
            _EscapeMenuPanel.SetActive(false);
            _gameState = GameState.InGame;
            _uiState = InGameInterfaceDisplayState.Dialogue;
        }

        /// <summary>
        /// <para>Eng. Pushed "Save" Button in Escape Menu State. </para>
        /// <para>Kor. ESC 메뉴 상태에서, 저장 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedEscapeMenuSaveButton()
        {
            _gameState = GameState.SaveLoad;

            string[] _keys = new string[3];
            _keys[0] = _leftIllustKey;
            _keys[1] = _centerIllustKey;
            _keys[2] = _rightIllustKey;

            float [] _colors = new float[3];
            _colors[0] = _speakerLeftIllustration.color.a;
            _colors[1] = _speakerCenterIllustration.color.a;
            _colors[2] = _speakerRightIllustration.color.a;

            bool[] _activated = new bool[3];
            _activated[0] = _speakerLeftIllustration.GetComponent<SpeakerIllustrationPosition>().IsActivated;
            _activated[1] = _speakerCenterIllustration.GetComponent<SpeakerIllustrationPosition>().IsActivated;
            _activated[2] = _speakerRightIllustration.GetComponent<SpeakerIllustrationPosition>().IsActivated;

            SaveLoadSlotManager.GetInstance().RequestSaveSlot(_currentScript, Enum.SaveLoadSlotManagerState.OptionSave, _keys, _colors, _activated);
        }

        /// <summary>
        /// <para>Eng. Requested "New Game" in Main Menu. </para>
        /// <para>Kor. 메인 메뉴 상태에서, 새 게임 저장을 요청받았습니다. </para>
        /// </summary>
        public void PushedNewGame()
        {
            string[] _keys = new string[3];
            _keys[0] = _leftIllustKey;
            _keys[1] = _centerIllustKey;
            _keys[2] = _rightIllustKey;

            float[] _colors = new float[3];
            _colors[0] = _speakerLeftIllustration.color.a;
            _colors[1] = _speakerCenterIllustration.color.a;
            _colors[2] = _speakerRightIllustration.color.a;

            bool[] _activated = new bool[3];
            _activated[0] = _speakerLeftIllustration.GetComponent<SpeakerIllustrationPosition>().IsActivated;
            _activated[1] = _speakerCenterIllustration.GetComponent<SpeakerIllustrationPosition>().IsActivated;
            _activated[2] = _speakerRightIllustration.GetComponent<SpeakerIllustrationPosition>().IsActivated;

            SaveLoadSlotManager.GetInstance().RequestSaveSlot(_currentScript, Enum.SaveLoadSlotManagerState.NewGameSave, _keys, _colors, _activated);
        }

        /// <summary>
        /// <para>Eng. Pushed "Load" Button in Escape Menu State. </para>
        /// <para>Kor. ESC 메뉴 상태에서, 불러오기 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedEscapeMenuLoadButton()
        {
            _gameState = GameState.SaveLoad;
            SaveLoadSlotManager.GetInstance().RequestLoadSlot();
        }

        /// <summary>
        /// <para>Eng. Pushed "Option" Button in Escape Menu State. </para>
        /// <para>Kor. ESC 메뉴 상태에서, 설정 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedEscapeMenuOptionButton()
        {
            _gameState = GameState.Option;
            OptionManager.GetInstance().RequestOpenOptionInterface();
        }

        /// <summary>
        /// <para>Eng. Pushed "Main Menu" Button in Escape Menu State. </para>
        /// <para>Kor. ESC 메뉴 상태에서, 메인 메뉴 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedEscapeMenuMainMenuButton()
        {
            _EscapeMenuPanel.SetActive(false);
            MainMenuManager.GetInstance().EnableMainMenu();
        }

        /// <summary>
        /// <para>Eng. Pushed "Quit" Button in Escape Menu State. </para>
        /// <para>Kor. ESC 메뉴 상태에서, 종료 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedEscapeMenuQuitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
