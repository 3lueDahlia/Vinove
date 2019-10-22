using System;
using System.IO;
using UnityEngine;

using ViNovE.Framework.Enum;
using ViNovE.Framework.Error;

namespace ViNovE.Framework.InGame.Option
{
    public class VinoveOptionData
    {
        ///////////////////////////
        /// Graphic

        public Resolution _resolution;
        public int _fullscreenModeIndex;
        public int _qualityIndex;

        ///////////////////////////
        /// Sound
        public float _bgmVolume;
        public float _effectVolume;

        ///////////////////////////
        /// Game
        public int _dialogueSpeedIndex;

        //////////////////////////////////////////////////////
        /// Properties

        public Resolution ScreenResolution
        {
            get { return _resolution; }
            set { _resolution = value; }
        }

        public int FullscreenModeIndex
        {
            get { return _fullscreenModeIndex; }
            set
            {
                _fullscreenModeIndex = value;
                Screen.fullScreenMode = OptionManager.GetInstance().ConvertFullscreenIndexToMode(_fullscreenModeIndex);
            }
        }
        public int QualityIndex
        {
            get { return _qualityIndex; }
            set
            {
                _qualityIndex = value;
                QualitySettings.SetQualityLevel(_qualityIndex);
            }
        }

        public float BGMVolume
        {
            get { return _bgmVolume; }
            set
            {
                _bgmVolume = value;
                InGameManager.GetInstance()._sceneBGM.volume = _bgmVolume;
            }
        }

        public float EffectVolume
        {
            get { return _effectVolume; }
            set
            {
                _effectVolume = value;
                InGameManager.GetInstance()._scriptSoundEffect.volume = _effectVolume;
            }
        }

        public int DialogueSpeedIndex
        {
            get { return _dialogueSpeedIndex; }
            set
            {
                _dialogueSpeedIndex = value;
                InGameManager.GetInstance().DialogueSpeed = _dialogueSpeedIndex;
            }
        }
    }

    public partial class OptionManager : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////////
        /// Singleton

        private static OptionManager instance;
        public static OptionManager GetInstance()
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(OptionManager)) as OptionManager;

                if (!instance)
                {
                    ErrorMessages.Instance.MissingSingletoneObject("OptionManager");
                }
            }

            return instance;
        }


        ////////////////////////////////////////////////////////////////////////
        /// Variables

        /////////////////////////////////
        // Option Panel UI Items
        public GameObject _optionPanel;
        public GameObject _optionGraphicPanel;
        public GameObject _optionSoundPanel;
        public GameObject _optionGamePanel;

        /////////////////////////////////
        // Option Panel State
        OptionTabState _optionState;

        /////////////////////////////////
        // Local Option Data
        public VinoveOptionData _optionData = new VinoveOptionData();

        ////////////////////////////////////////////////////////////////////////
        /// Func

        public void Start()
        {
            _optionState = OptionTabState.Graphic;
            ReadOptionData();
        }

        /// <summary>
        /// <para>Eng. Init option data with option ini file. </para>
        /// <para>Kor. 옵션 정보 ini을 가져와 초기화합니다. </para>
        /// </summary>
        public void ReadOptionData()
        {
            INIParser _iniParser = new INIParser();
            if (File.Exists(Application.dataPath + "/option.ini"))
            {   // if, file has exists
                _iniParser.Open(Application.dataPath + "/option.ini");

                // -------------- Graphic --------------

                SetOptiondataResolution(_iniParser.ReadValue("Graphic", "ResolutionWidth", Screen.currentResolution.width),
                                        _iniParser.ReadValue("Graphic", "ResolutionHeight", Screen.currentResolution.height));
                _optionData.FullscreenModeIndex = _iniParser.ReadValue("Graphic", "FullscreenMode", 0);
                _optionData.QualityIndex = _iniParser.ReadValue("Graphic", "Quality", QualitySettings.GetQualityLevel());

                // -------------- Sound --------------

                _optionData.BGMVolume = _iniParser.ReadValue("Sound", "BGM", 1f);
                _optionData.EffectVolume = _iniParser.ReadValue("Sound", "Effect", 1f);

                // -------------- Game --------------

                _optionData.DialogueSpeedIndex = _iniParser.ReadValue("Game", "DialogueSpeed", 0);

                // -------------- End --------------

                _iniParser.Close();
            }
            else
            {
                _iniParser.Open(Application.dataPath + "/option.ini");

                // -------------- Graphic --------------
                _optionData._resolution = Screen.currentResolution;
                _optionData._fullscreenModeIndex = ConvertFullscreenModeToIndex(Screen.fullScreenMode);
                _optionData._qualityIndex = QualitySettings.GetQualityLevel();

                // -------------- Sound --------------

                _optionData._bgmVolume = InGameManager.GetInstance()._sceneBGM.volume;
                _optionData._effectVolume = InGameManager.GetInstance()._scriptSoundEffect.volume;

                // -------------- Game --------------

                _optionData._dialogueSpeedIndex = InGameManager.GetInstance().DialogueSpeed;

                // -------------- End --------------

                _iniParser.Close();

                SaveOptionData();
            }
        }

        /// <summary>
        /// <para>Eng. Init option data with option ini file. </para>
        /// <para>Kor. 옵션 정보를 저장합니다. </para>
        /// </summary>
        public void SaveOptionData()
        {
            INIParser _iniParser = new INIParser();

            _iniParser.Open(Application.dataPath + "/option.ini");

            // -------------- Graphic --------------

            _iniParser.WriteValue("Graphic", "ResolutionWidth", _optionData._resolution.width);
            _iniParser.WriteValue("Graphic", "ResolutionHeight", _optionData._resolution.height);
            _iniParser.WriteValue("Graphic", "FullscreenMode", _optionData._fullscreenModeIndex);
            _iniParser.WriteValue("Graphic", "Quality", _optionData._qualityIndex);

            // -------------- Sound --------------

            _iniParser.WriteValue("Sound", "BGM", _optionData._bgmVolume);
            _iniParser.WriteValue("Sound", "Effect", _optionData._effectVolume);

            // -------------- Game --------------

            _iniParser.WriteValue("Game", "DialogueSpeed", _optionData._dialogueSpeedIndex);

            // -------------- End --------------

            _iniParser.Close();
        }

        /// <summary>
        /// <para>Eng. Requested Open Option Menu. </para>
        /// <para>Kor. 옵션 메뉴를 호출하였습니다. </para>
        /// </summary>
        public void RequestOpenOptionInterface()
        {
            _optionPanel.SetActive(true);
        }

        /// <summary>
        /// <para>Eng. Requested Close Option Menu. </para>
        /// <para>Kor. 옵션 메뉴 종료를 호출하였습니다. </para>
        /// </summary>
        public void RequestCloseOptionInterface()
        {
            _optionPanel.SetActive(false);
        }

        /// <summary>
        /// <para>Eng. Pushed "Graphic" Button in Option State. </para>
        /// <para>Kor. 메인 메뉴 상태에서, "그래픽" 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedOptionGraphicButton()
        {
            _optionState = OptionTabState.Graphic;

            _optionGraphicPanel.SetActive(true);
            _optionSoundPanel.SetActive(false);
            _optionGamePanel.SetActive(false);

            GraphicOptionManager.GetInstance().InitGraphicOptionPanelItem();
        }

        /// <summary>
        /// <para>Eng. Pushed "Sound" Button in Option State. </para>
        /// <para>Kor. 옵션 상태에서, "사운드" 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedOptionSoundButton()
        {
            _optionState = OptionTabState.Sound;

            _optionGraphicPanel.SetActive(false);
            _optionSoundPanel.SetActive(true);
            _optionGamePanel.SetActive(false);

            SoundOptionManager.GetInstance().InitSoundOptionPanelItem();
        }

        /// <summary>
        /// <para>Eng. Pushed "Game" Button in Option State. </para>
        /// <para>Kor. 옵션 상태에서, "게임" 버튼을 눌렀습니다. </para>
        /// </summary>
        public void PushedOptionGameButton()
        {
            _optionState = OptionTabState.Game;

            _optionGraphicPanel.SetActive(false);
            _optionSoundPanel.SetActive(false);
            _optionGamePanel.SetActive(true);

            GameOptionManager.GetInstance().InitGameOptionPanelItem();
        }

        public int ConvertFullscreenModeToIndex(FullScreenMode _fullscreenMode)
        {
            switch (_fullscreenMode)
            {
                case FullScreenMode.FullScreenWindow:
                    return 0;
                case FullScreenMode.MaximizedWindow:
                    return 1;
                case FullScreenMode.Windowed:
                    return 2;
                default:
                    return default;
            }
        }

        public FullScreenMode ConvertFullscreenIndexToMode(int _fullscreenModeIndex)
        {
            switch (_fullscreenModeIndex)
            {
                case 0:
                    return FullScreenMode.FullScreenWindow;
                case 1:
                    return FullScreenMode.MaximizedWindow;
                case 2:
                    return FullScreenMode.Windowed;
                default:
                    return default;
            }
        }

        public void SetOptiondataResolution(int _width, int _height)
        {
            _optionData._resolution.width = _width;
            _optionData._resolution.height = _height;
            Screen.SetResolution(_width, _height, Screen.fullScreen);
        }
    }
}