using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ViNovE.Framework.InGame.SaveLoad
{
    public class SavedButtonManager : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        ///////////////////////////////
        /// UI Items

        public Text _savedDateText;
        public Text _speakerNameText;
        public Text _dialogueText;

        ///////////////////////////////
        /// UI Items
        
        public VinoveGameSaveData _savedData;

        ////////////////////////////////////////////////////////////////////////
        /// Func


        /// <summary>
        /// <para>Eng. Change Saved Data. </para>
        /// <para>Kor. 저장 데이터를 변경합니다. </para>
        /// </summary>
        public void ChangeSaveData(VinoveGameSaveData _data)
        {
            _savedData = _data;
        }


        /// <summary>
        /// <para>Eng. Change text displayed ingame UI. </para>
        /// <para>Kor. 실제 UI 상으로 보이는 텍스트를 변경합니다. </para>
        /// </summary>
        public void ChangeTexts(string _savedDate, string _speakerName, string _dialogue)
        {
            _speakerNameText.text = _speakerName;
            _dialogueText.text = _dialogue;
            _savedDateText.text = _savedDate;
        }

        public void PushedSavedButton()
        {
            SaveLoadSlotManager.GetInstance().SavedButtonPushed(gameObject);
        }
    }
}