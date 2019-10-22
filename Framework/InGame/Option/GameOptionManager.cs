using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ViNovE.Framework.Error;

namespace ViNovE.Framework.InGame.Option
{
    public class GameOptionManager : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////////
        /// Singleton

        private static GameOptionManager instance;
        public static GameOptionManager GetInstance()
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(GameOptionManager)) as GameOptionManager;

                if (!instance)
                {
                    ErrorMessages.Instance.MissingSingletoneObject("GameOptionManager");
                }
            }

            return instance;
        }


        ////////////////////////////////////////////////////////////////////////
        /// Variables

        public Dropdown _dialogueSpeedDropdown;

        ////////////////////////////////////////////////////////////////////////
        /// Func
        
        // Start is called before the first frame update
        void Start()
        {

        }


        /// <summary>
        /// <para>Eng. Initialize panel item of Game option. </para>
        /// <para>Kor. 게임 옵션 패널의 아이템 값을 초기화해줍니다. </para>
        /// </summary>
        public void InitGameOptionPanelItem()
        {
            _dialogueSpeedDropdown.value = OptionManager.GetInstance()._optionData.DialogueSpeedIndex;
        }

        /// <summary>
        /// <para>Eng. Pushed "Apply" Button in Game Option State. </para>
        /// <para>Kor. 게임  옵션 상태에서, "적용" 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedGameAppliedButton()
        {
            OptionManager.GetInstance()._optionData.DialogueSpeedIndex = _dialogueSpeedDropdown.value;

            OptionManager.GetInstance().SaveOptionData();
        }
    }
}