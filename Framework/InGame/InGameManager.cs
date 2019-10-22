using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using ViNovE.Framework.Animation;
using ViNovE.Framework.Data;
using ViNovE.Framework.Data.IO;
using ViNovE.Framework.Debugger;
using ViNovE.Framework.Enum;
using ViNovE.Framework.Error;
using ViNovE.Framework.InGame.SaveLoad;
using ViNovE.Framework.InGame.Utility;

namespace ViNovE.Framework.InGame
{
    public partial class InGameManager : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////////
        /// Singleton

        private static InGameManager instance;
        public static InGameManager GetInstance()
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(InGameManager)) as InGameManager;

                if (!instance)
                {
                    ErrorMessages.Instance.MissingSingletoneObject("InGameManager");
                }
            }

            return instance;
        }

        ////////////////////////////////////////////////////////////////////////
        /// Variables
        /// 

        ///////////////////////////////////
        /// UI Canvas

        [Header("UI Canvas")]

        public GameObject _uiCanvas;

        [Space(5)]

        ///////////////////////////////////
        /// UI Items - Main Menu

        [Header("Main Menu Items")]

        public GameObject _mainMenuInterfacePanel;
        
        [Space(5)]

        ///////////////////////////////////
        /// UI Items - InGame

        [Header("InGame Items")]

        public GameObject _inGameInterfacePanel;

        public Image _backgroundIllustration;

        public Image _speakerLeftIllustration;
        public Image _speakerCenterIllustration;
        public Image _speakerRightIllustration;

        public Image _speakerNameBackgroundImg;
        public Text _speakerNameText;

        public Image _dialogueBackgroundImg;
        public Text _dialogueText;

        public Image _dialogueAutoButtonImg;
        public Text _dialogueAutoButtonText;
        public Image _dialogueSkipButtonImg;
        public Text _dialogueSkipButtonText;

        public GameObject _branchPanel;
        public Image _branchBackgroundImg;
        public GameObject[] _branchButtons;
        public GameObject _branchButtonPrefab;

        public AudioSource _sceneBGM;
        public AudioSource _scriptSoundEffect;

        [Space(5)]

        ///////////////////////////////////
        /// UI Items - Escape Menu

        [Header("Escape Menu Items")]

        public GameObject _EscapeMenuPanel;

        [Space(5)]

        ///////////////////////////////////
        /// UI Items - Debugger

        [Header("Debugger")]

        public GameObject _debuggerPanel;

        [Space(5)]

        ///////////////////////////////////
        /// Vinove Data

        [Header("Vinove Datas")]

        public Vinove _currentVinove;
        public VinoveScene _currentScene;
        public VinoveBranch _currentBranch;
        public VinoveMerge _currentMerge;
        public VinoveScript _currentScript;

        ///////////////////////////////////
        /// State
        
        private GameState _gameState;
        private InGameInterfaceDisplayState _uiState;
        private InGameProgressState _progressState;
        private bool _needDisableClick;

        ///////////////////////////////////
        /// Temporary Data for save
        
        string _leftIllustKey;
        string _centerIllustKey;
        string _rightIllustKey;

        float _leftColorAlpha;
        float _centerColorAlpha;
        float _rightColorAlpha;


        ////////////////////////////////////////////////////////////////////////
        /// Func

        public GameState GameState
        {
            get { return _gameState; }
            set { _gameState = value; }
        }

        ////////////////////////////////////////////////////////////////////////
        /// Func

        public void Awake()
        {
            ManagerInitialize();
        }

        public void Start()
        {
        }

        public void Update()
        {
            if(_gameState == GameState.InGame && _uiState == InGameInterfaceDisplayState.Dialogue)
            {
                if ((Input.GetMouseButtonDown(0) && _needDisableClick == false) || Input.GetKeyDown(KeyCode.Space))
                {
                    Next();
                }
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(_debuggerPanel.activeSelf == true)
                {
                    _debuggerPanel.SetActive(false);
                }
                else
                {
                    _debuggerPanel.SetActive(true);
                    DebuggerManager.GetInstance().Init();
                }
            }
        }

        /// <summary>
        /// <para>Eng. Func Ingame Manager Variables init to basic value. </para>
        /// <para>Kor. 인게임 매니저의 기본 값들을 초기화하는 함수입니다. </para>
        /// </summary>
        public void ManagerInitialize()
        {
            _gameState = GameState.MainMenu;
            _uiState = InGameInterfaceDisplayState.Dialogue;
            _progressState = InGameProgressState.Idle;
            _needDisableClick = false;

            VinoveInitialize();
        }

        /// <summary>
        /// <para>Eng. Assign Vinove file on Canvas UI GameObject. </para>
        /// <para>Kor. 비노비 파일을 인게임 매니저 오브젝트에 등록합니다. </para>
        /// </summary>
        public void VinoveInitialize()
        {
#if UNITY_EDITOR
            string _archivePath = Application.dataPath + "/TemporaryArchive.vnvach";

            if (System.IO.File.Exists(_archivePath))
            {
                var loaded = Resources.Load<Vinove>("Gamedat");
                AssetPathArchive _archive = BinaryIOManager.GetInst().BinaryDeserialize<AssetPathArchive>(_archivePath);

                AssetDatabase.ImportAsset(_archive._path);
                var currentLoaded = AssetDatabase.LoadAssetAtPath<Vinove>(_archive._path);
                _currentVinove = currentLoaded as Vinove;
                // ----------------------- Vinove - Starter Scene -----------------------

                if (ErrorDetector.Instance.IsStringHasData(_archive._sceneUID))
                {
                    // ----------------------- Scene -----------------------
                    _currentScene = _currentVinove.FindSceneWithUID(_archive._sceneUID);

                    // ----------------------- Scene - Specific Script -----------------------

                    if (ErrorDetector.Instance.IsStringHasData(_archive._scriptUID))
                    {
                        _currentScript = _currentScene.FindScriptWithUID(_archive._scriptUID);
                    }
                    // ----------------------- Scene - Starter Script -----------------------
                    else if (ErrorDetector.Instance.IsStringHasData(_currentScene.StarterUID))
                    {
                        _currentScript = _currentScene.FindScriptWithUID(_currentScene.StarterUID);
                    }
                }
            }

#else
            if (_currentVinove == null)
            {
                _currentVinove = Resources.Load<Vinove>("Gamedat");

                // ----------------------- Vinove - Starter Scene -----------------------

                if (ErrorDetector.Instance.IsStringHasData(_currentVinove.StarterUID))
                {
                    // ----------------------- Scene -----------------------
                    _currentScene = _currentVinove.FindSceneWithUID(_currentVinove.StarterUID);

                    // ----------------------- Scene - Specific Script -----------------------

                    if (ErrorDetector.Instance.IsStringHasData(_currentScene.StarterUID))
                    {
                        _currentScript = _currentScene.FindScriptWithUID(_currentScene.StarterUID);
                    }
                }
            }
#endif
        }

        /// <summary>
        /// <para>Eng. Load game using script uid. </para>
        /// <para>Kor. 스크립트 UID 등의 정보로, 게임을 불러옵니다. </para>
        /// </summary>
        public void LoadGameByScript(VinoveGameSaveData _savedData)
        {
            _currentScene = _currentVinove.FindSceneWithUID(_savedData._parentSceneUID);
            _currentScript = _currentScene.FindScriptWithUID(_savedData._scriptUID);

            // ----------------------- Scene - Background Img -----------------------
            Texture2D _sceneBackgroundIllust;
            if (_currentVinove.BackgroundIllustrations.GetTextureData(_currentScene.BackgroundIllustKey, out _sceneBackgroundIllust))
            {
                Rect _backgroundTexRect = new Rect(0, 0, _sceneBackgroundIllust.width, _sceneBackgroundIllust.height);
                _backgroundIllustration.sprite = Sprite.Create(_sceneBackgroundIllust, _backgroundTexRect, new Vector2(0.5f, 0.5f));
            }

            // ----------------------- Scene - Background Music -----------------------
            if (_currentScene.BackgroundMusic != null)
            {
                _sceneBGM.clip = _currentScene.BackgroundMusic;

                _sceneBGM.Play();
            }

            // ----------------------- Script - Speaker Illustrations & Color -----------------------

            Texture2D _speakerIllust;
            if (_currentVinove.SpeakerIllustrations.GetTextureData(_savedData._speakerIllustKey[0], out _speakerIllust))
            {
                SetSpeakerIllustImageFromTexture(_speakerLeftIllustration, _speakerIllust);

                Color _illustColor = _speakerLeftIllustration.color;
                _speakerLeftIllustration.color = new Color(_illustColor.r, _illustColor.g, _illustColor.b, _savedData._speakerIllustColorAlpha[0]);
                _speakerLeftIllustration.GetComponent<SpeakerIllustrationPosition>().IsActivated = _savedData._speakerIllustActivated[0];
                _speakerLeftIllustration.GetComponent<SpeakerIllustrationPosition>().ColorUncurrentSpeaker();
            }

            if (_currentVinove.SpeakerIllustrations.GetTextureData(_savedData._speakerIllustKey[1], out _speakerIllust))
            {
                SetSpeakerIllustImageFromTexture(_speakerCenterIllustration, _speakerIllust);

                Color _illustColor = _speakerCenterIllustration.color;
                _speakerCenterIllustration.color = new Color(_illustColor.r, _illustColor.g, _illustColor.b, _savedData._speakerIllustColorAlpha[1]);
                _speakerCenterIllustration.GetComponent<SpeakerIllustrationPosition>().IsActivated = _savedData._speakerIllustActivated[1];
                _speakerCenterIllustration.GetComponent<SpeakerIllustrationPosition>().ColorUncurrentSpeaker();
            }

            if (_currentVinove.SpeakerIllustrations.GetTextureData(_savedData._speakerIllustKey[2], out _speakerIllust))
            {
                SetSpeakerIllustImageFromTexture(_speakerRightIllustration, _speakerIllust);

                Color _illustColor = _speakerRightIllustration.color;
                _speakerRightIllustration.color = new Color(_illustColor.r, _illustColor.g, _illustColor.b, _savedData._speakerIllustColorAlpha[2]);
                _speakerRightIllustration.GetComponent<SpeakerIllustrationPosition>().IsActivated = _savedData._speakerIllustActivated[2];
                _speakerRightIllustration.GetComponent<SpeakerIllustrationPosition>().ColorUncurrentSpeaker();
            }

            // ----------------------- Script -----------------------
            SyncScript();
        }
                
        public void NewGameInitialize()
        {
            _gameState = GameState.InGame;
            _uiState = InGameInterfaceDisplayState.Dialogue;

            // ----------------------- Vinove - Starter Scene -----------------------

            if (ErrorDetector.Instance.IsStringHasData(_currentVinove.StarterUID))
            {
                // ----------------------- Scene -----------------------
                _currentScene = _currentVinove.FindSceneWithUID(_currentVinove.StarterUID);

                // ----------------------- Scene - Starter Script -----------------------
                if (ErrorDetector.Instance.IsStringHasData(_currentScene.StarterUID))
                {
                    _currentScript = _currentScene.FindScriptWithUID(_currentScene.StarterUID);
                }
            }

            _speakerLeftIllustration.GetComponent<SpeakerIllustrationPosition>().SetDeactivated();
            _speakerCenterIllustration.GetComponent<SpeakerIllustrationPosition>().SetDeactivated();
            _speakerRightIllustration.GetComponent<SpeakerIllustrationPosition>().SetDeactivated();

            SyncInterface();
        }

        public void SyncInterface()
        {
            SyncScene();
            SyncScript();
        }
    }
}