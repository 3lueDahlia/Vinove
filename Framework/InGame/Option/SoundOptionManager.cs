using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ViNovE.Framework.Error;

namespace ViNovE.Framework.InGame.Option
{
    public class SoundOptionManager : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////////
        /// Singleton

        private static SoundOptionManager instance;
        public static SoundOptionManager GetInstance()
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(SoundOptionManager)) as SoundOptionManager;

                if (!instance)
                {
                    ErrorMessages.Instance.MissingSingletoneObject("SoundOptionManager");
                }
            }

            return instance;
        }


        ////////////////////////////////////////////////////////////////////////
        /// Variables

        public GameObject _bgmVolumeSlider;
        public GameObject _effectVolumeSlider;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        // Start is called before the first frame update
        void Start()
        {
            InitSoundOptionPanelItem();
        }


        /// <summary>
        /// <para>Eng. Initialize panel item of Sound option. </para>
        /// <para>Kor. 사운드 옵션 패널의 아이템 값을 초기화해줍니다. </para>
        /// </summary>
        public void InitSoundOptionPanelItem()
        {
            _bgmVolumeSlider.GetComponent<Slider>().value = OptionManager.GetInstance()._optionData.BGMVolume;
            _effectVolumeSlider.GetComponent<Slider>().value = OptionManager.GetInstance()._optionData.EffectVolume;
        }

        /// <summary>
        /// <para>Eng. Pushed "Apply" Button in Sound Option State. </para>
        /// <para>Kor. 사운드 옵션 상태에서, "적용" 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedSoundAppliedButton()
        {
            OptionManager.GetInstance()._optionData.BGMVolume = _bgmVolumeSlider.GetComponent<Slider>().value;
            OptionManager.GetInstance()._optionData.EffectVolume = _effectVolumeSlider.GetComponent<Slider>().value;

            OptionManager.GetInstance().SaveOptionData();
        }
    }
}