using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ViNovE.Framework.Error;

namespace ViNovE.Framework.InGame.Option
{
    public class GraphicOptionManager : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////////
        /// Singleton

        private static GraphicOptionManager instance;
        public static GraphicOptionManager GetInstance()
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(GraphicOptionManager)) as GraphicOptionManager;

                if (!instance)
                {
                    ErrorMessages.Instance.MissingSingletoneObject("GraphicOptionManager");
                }
            }

            return instance;
        }

        ////////////////////////////////////////////////////////////////////////
        /// Variables

        public Dropdown _screenResolutionDropdown;
        public Dropdown _fullscreenModeDropdown;
        public Dropdown _qualityDropdown;

        public FullScreenMode _fullscreenMode;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        // Start is called before the first frame update
        void Start()
        {
            InitGraphicOptionPanelItem();
        }

        /// <summary>
        /// <para>Eng. Initialize panel item of Graphic option. </para>
        /// <para>Kor. 그래픽 옵션 패널의 아이템 값을 초기화해줍니다. </para>
        /// </summary>
        public void InitGraphicOptionPanelItem()
        {

            _screenResolutionDropdown.value = InitResolutionOptions();
            _fullscreenModeDropdown.value = OptionManager.GetInstance()._optionData.FullscreenModeIndex;
            _qualityDropdown.value = OptionManager.GetInstance()._optionData.QualityIndex;
        }

        /// <summary>
        /// <para>Eng. Pushed "Apply" Button in Graphic Option State. </para>
        /// <para>Kor. 그래픽 옵션 상태에서, "적용" 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedGraphicAppliedButton()
        {
            //OptionManager.GetInstance()._optionData
            ApplyResolutionOption();
            OptionManager.GetInstance()._optionData.FullscreenModeIndex = _fullscreenModeDropdown.value;
            OptionManager.GetInstance()._optionData.QualityIndex = _qualityDropdown.value;

            OptionManager.GetInstance().SaveOptionData();
        }

        /// <summary>
        /// <para>Eng. Initialize Graphic Resolution Dropdown options. </para>
        /// <para>Kor. 그래픽 해상도 드롭다운 옵션을 초기화합니다. </para>
        /// </summary>
        public int InitResolutionOptions()
        {
            int _resolutionIndex = 0;
            int _tmpIndex = 0;
            foreach (Resolution _resolution in Screen.resolutions)
            {
                if (OptionManager.GetInstance()._optionData.ScreenResolution.width == _resolution.width && OptionManager.GetInstance()._optionData.ScreenResolution.height == _resolution.height)
                {
                    _resolutionIndex = _tmpIndex;
                }

                _screenResolutionDropdown.options.Add(new Dropdown.OptionData(_resolution.width + "*" + _resolution.height));
                _tmpIndex++;
            }

            return _resolutionIndex;
        }

        /// <summary>
        /// <para>Eng. Apply Resolution Option. </para>
        /// <para>Kor. 해상도 옵션을 적용합니다. </para>
        /// </summary>
        public void ApplyResolutionOption()
        {
            string _resolutionText = _screenResolutionDropdown.options[_screenResolutionDropdown.value].text;
            string[] _resolution = _resolutionText.Split('*');
            OptionManager.GetInstance().SetOptiondataResolution(int.Parse(_resolution[0]), int.Parse(_resolution[1]));
        }
    }
}