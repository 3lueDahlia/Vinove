using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ViNovE.Framework.Error;
using ViNovE.Framework.InGame.SaveLoad;
using ViNovE.Framework.InGame.Option;

namespace ViNovE.Framework.InGame
{
    public partial class MainMenuManager : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////////
        /// Singleton

        private static MainMenuManager instance;
        public static MainMenuManager GetInstance()
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(MainMenuManager)) as MainMenuManager;

                if (!instance)
                {
                    ErrorMessages.Instance.MissingSingletoneObject("MainMenuManager");
                }
            }

            return instance;
        }

        ////////////////////////////////////////////////////////////////////////
        /// Variables

        public GameObject _mainMenuPanel;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. Pushed "New Game" Button in Main Menu State. </para>
        /// <para>Kor. 메인 메뉴 상태에서, "새 게임" 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedMainMenuNewGameButton()
        {
            InGameManager.GetInstance().PushedNewGame();
        }

        /// <summary>
        /// <para>Eng. Pushed "Load" Button in Main Menu State. </para>
        /// <para>Kor. 메인 메뉴 상태에서, "불러오기" 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedMainMenuLoadButton()
        {
            InGameManager.GetInstance().GameState = Enum.GameState.SaveLoad;
            SaveLoadSlotManager.GetInstance().RequestLoadSlot();
        }

        /// <summary>
        /// <para>Eng. Pushed "Option" Button in Main Menu State. </para>
        /// <para>Kor. 메인 메뉴 상태에서, "설정" 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedMainMenuOptionButton()
        {
            InGameManager.GetInstance().GameState = Enum.GameState.Option;
            OptionManager.GetInstance().RequestOpenOptionInterface();
        }

        /// <summary>
        /// <para>Eng. Pushed "Quit" Button in Main Menu State. </para>
        /// <para>Kor. 메인 메뉴 상태에서, "종료" 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedMainMenuQuitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// <para>Eng. Activate Main menu screen. </para>
        /// <para>Kor. 메인 메뉴 화면을 활성화 합니다 </para>
        /// </summary>
        public void EnableMainMenu()
        {
            _mainMenuPanel.SetActive(true);
        }

        /// <summary>
        /// <para>Eng. Deactivate Main menu screen. </para>
        /// <para>Kor. 메인 메뉴 화면을 비활성화 합니다 </para>
        /// </summary>
        public void DisableMainMenu()
        {
            _mainMenuPanel.SetActive(false);
        }
    }
}