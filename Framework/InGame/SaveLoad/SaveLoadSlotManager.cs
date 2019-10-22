using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using ViNovE.Framework.Data;
using ViNovE.Framework.Data.IO;
using ViNovE.Framework.Enum;
using ViNovE.Framework.Error;

namespace ViNovE.Framework.InGame.SaveLoad
{
    [Serializable]
    public class VinoveGameSaveData
    {
        public string _parentSceneUID;
        public string _scriptUID;

        public string _savedDate;

        public string [] _speakerIllustKey;
        public float [] _speakerIllustColorAlpha;
        public bool [] _speakerIllustActivated;
    }

    public class SaveLoadSlotManager : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////////
        /// Singleton

        private static SaveLoadSlotManager instance;
        public static SaveLoadSlotManager GetInstance()
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(SaveLoadSlotManager)) as SaveLoadSlotManager;

                if (!instance)
                {
                    ErrorMessages.Instance.MissingSingletoneObject("SaveLoadSlotManager");
                }
            }

            return instance;
        }

        ////////////////////////////////////////////////////////////////////////
        /// Variables
        
        ///////////////////////////////
        /// UI Items

        public GameObject _saveLoadSlotPanel;
        public GameObject _rewritePanel;

        public GameObject _contentParent;
        public GameObject _addSaveFileButton;

        public GameObject _savedFileButtonPrefab;

        public List<GameObject> _savedFileButtons;

        ///////////////////////////////
        /// Manager State

        SaveLoadSlotManagerState _state;

        ///////////////////////////////
        /// Save Data

        List<VinoveGameSaveData> _saveDatas;

        ///////////////////////////////
        /// Temporary Save Data

        public string _saveFolderDir;

        VinoveScript _saveTargetScript;
        string _parentSceneUID;
        string _saveTargetUID;

        string[] _speakerIllustKey;
        float[] _speakerIllustColorAlpha;
        bool[] _speakerIllustActivated;

        GameObject _pushedSavedButton;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        public void Awake()
        {
            _saveDatas = new List<VinoveGameSaveData>();
            _savedFileButtons = new List<GameObject>();
        }

        public void Start()
        {
            _saveFolderDir = Application.dataPath + "/saves/";
            Directory.CreateDirectory(_saveFolderDir);

            ReloadSaveFiles();
        }

        ///////////////////////////////
        /// Outward Called

        /// <summary>
        /// <para>Eng. Requested game load. </para>
        /// <para>Kor. 불러오기 요청을 받았습니다. </para>
        /// </summary>
        public void RequestLoadSlot()
        {
            _state = SaveLoadSlotManagerState.Load;
            EnableSlot();
        }

        /// <summary>
        /// <para>Eng. Requested game save. </para>
        /// <para>Kor. 저장 요청을 받았습니다. </para>
        /// </summary>
        public void RequestSaveSlot(VinoveScript _targetScript, SaveLoadSlotManagerState _calledBy, string[] _illustKey = null, float[] _illustColorAlpha = null, bool[] _illustActivated = null)
        {
            _state = _calledBy;

            switch (_state)
            {
                case SaveLoadSlotManagerState.OptionSave:
                    _saveTargetScript = _targetScript;
                    _saveTargetUID = _targetScript.UID;
                    _parentSceneUID = _targetScript.ParentSceneUID;

                    _speakerIllustKey = _illustKey;
                    _speakerIllustColorAlpha = _illustColorAlpha;
                    _speakerIllustActivated = _illustActivated;

                    _addSaveFileButton.SetActive(true);

                    EnableSlot();

                    break;
                case SaveLoadSlotManagerState.NewGameSave:
                    _saveTargetScript = _targetScript;
                    _saveTargetUID = _targetScript.UID;
                    _parentSceneUID = _targetScript.ParentSceneUID;

                    _speakerIllustKey = _illustKey;
                    _speakerIllustColorAlpha = _illustColorAlpha;
                    _speakerIllustActivated = _illustActivated;

                    _addSaveFileButton.SetActive(true);

                    EnableSlot();
                    break;
            }
        }

        /// <summary>
        /// <para>Eng. Requested Save Load slot close. </para>
        /// <para>Kor. 슬롯 종료 요청을 받았습니다. </para>
        /// </summary>
        public void RequestCloseSlot()
        {
            _saveTargetUID = null;

            DisableSlot();
        }

        /// <summary>
        /// <para>Eng. Pushed Saved Button. </para>
        /// <para>Kor. 저장된 게임 버튼을 눌렀습니다. </para>
        /// </summary>
        public void SavedButtonPushed(GameObject _pushedButton)
        {
            switch (_state)
            {
                case SaveLoadSlotManagerState.NewGameSave:
                    _pushedSavedButton = _pushedButton;
                    _rewritePanel.SetActive(true);
                    break;
                case SaveLoadSlotManagerState.OptionSave:
                    _pushedSavedButton = _pushedButton;
                    _rewritePanel.SetActive(true);
                    break;
                case SaveLoadSlotManagerState.Load:
                    _pushedSavedButton = _pushedButton;
                    VinoveGameSaveData _savedData = _pushedSavedButton.GetComponent<SavedButtonManager>()._savedData;
                    InGameManager.GetInstance().LoadGameByScript(_savedData);
                    DisableSlot();
                    MainMenuManager.GetInstance().DisableMainMenu();
                    InGameManager.GetInstance().VinoveInitialize();
                    InGameManager.GetInstance().GameState = Enum.GameState.InGame;
                    break;
            }
        }

        /// <summary>
        /// <para>Eng. Pushed Add Save Button. </para>
        /// <para>Kor. 새 저장 버튼을 눌렀습니다. </para>
        /// </summary>
        public void AddSaveButtonPushed()
        {
            SaveData();
        }

        /// <summary>
        /// <para>Eng. Pushed Rewrite Yes Button. </para>
        /// <para>Kor. 덮어쓰기 저장 버튼을 눌렀습니다. </para>
        /// </summary>
        public void RewriteYesButtonPushed()
        {
            switch (_state)
            {
                case SaveLoadSlotManagerState.NewGameSave:
                    SaveData(_pushedSavedButton);
                    _rewritePanel.SetActive(false);
                    break;
                case SaveLoadSlotManagerState.OptionSave:
                    SaveData(_pushedSavedButton);
                    _rewritePanel.SetActive(false);
                    break;
            }
        }

        /// <summary>
        /// <para>Eng. Pushed Rewrite No Button. </para>
        /// <para>Kor. 덮어쓰기 미저장 버튼을 눌렀습니다. </para>
        /// </summary>
        public void RewriteNoButtonPushed()
        {
            switch (_state)
            {
                case SaveLoadSlotManagerState.NewGameSave:
                    _rewritePanel.SetActive(false);
                    _pushedSavedButton = null;
                    break;
                case SaveLoadSlotManagerState.OptionSave:
                    _rewritePanel.SetActive(false);
                    _pushedSavedButton = null;
                    break;
            }
        }

        ///////////////////////////////
        /// Save & Load Sequence

        /// <summary>
        /// <para>Eng. Load all save file in local save folder </para>
        /// <para>Kor. 세이브 폴더내에 저장된 파일을 게임으로 모두 불러옵니다. </para>
        /// </summary>
        void ReloadSaveFiles()
        {
            DirectoryInfo info = new System.IO.DirectoryInfo(_saveFolderDir);
            FileInfo [] fileInfo = info.GetFiles();

            foreach (FileInfo file in fileInfo)
            {
                if(file.Name.EndsWith(".sav"))
                {
                    VinoveGameSaveData _saveData = BinaryIOManager.GetInst().BinaryDeserialize<VinoveGameSaveData>(file.FullName);
                    _saveDatas.Add(_saveData);

                    VinoveScript _script = InGameManager.GetInstance()._currentVinove.FindSceneWithUID(_saveData._parentSceneUID).FindScriptWithUID(_saveData._scriptUID);

                    GameObject _instantiatedButton = Instantiate(_savedFileButtonPrefab, _contentParent.transform);
                    _instantiatedButton.GetComponent<SavedButtonManager>().ChangeSaveData(_saveData);
                    _instantiatedButton.GetComponent<SavedButtonManager>().ChangeTexts(_saveData._savedDate, _script.ScriptConversation.Speaker, _script.ScriptConversation.Dialog);
                    _savedFileButtons.Add(_instantiatedButton);
                }
            }
        }

        /// <summary>
        /// <para>Eng. Load all save file in local save folder </para>
        /// <para>Kor. 세이브 폴더내에 저장된 파일을 게임으로 모두 불러옵니다. </para>
        /// </summary>
        void ReloadSaveFile(string _path)
        {
            VinoveGameSaveData _saveData = BinaryIOManager.GetInst().BinaryDeserialize<VinoveGameSaveData>(_path);
            _saveDatas.Add(_saveData);

            VinoveScript _script = InGameManager.GetInstance()._currentVinove.FindSceneWithUID(_saveData._parentSceneUID).FindScriptWithUID(_saveData._scriptUID);

            GameObject _instantiatedButton = Instantiate(_savedFileButtonPrefab, _contentParent.transform);
            _instantiatedButton.GetComponent<SavedButtonManager>().ChangeSaveData(_saveData);
            _instantiatedButton.GetComponent<SavedButtonManager>().ChangeTexts(_saveData._savedDate, _script.ScriptConversation.Speaker, _script.ScriptConversation.Dialog);
            _savedFileButtons.Add(_instantiatedButton);
        }

        /// <summary>
        /// <para>Eng. Load all save file in local save folder </para>
        /// <para>Kor. 세이브 폴더내에 저장된 파일을 게임으로 모두 불러옵니다. </para>
        /// </summary>
        void ReloadSaveFile(VinoveGameSaveData _saveData)
        {
            VinoveScript _script = InGameManager.GetInstance()._currentVinove.FindSceneWithUID(_saveData._parentSceneUID).FindScriptWithUID(_saveData._scriptUID);

            _pushedSavedButton.GetComponent<SavedButtonManager>().ChangeSaveData(_saveData);
            _pushedSavedButton.GetComponent<SavedButtonManager>().ChangeTexts(_saveData._savedDate, _script.ScriptConversation.Speaker, _script.ScriptConversation.Dialog);

            _pushedSavedButton = null;
        }

        /// <summary>
        /// <para>Eng. Create class for archive and save to file. </para>
        /// <para>Kor. 저장용 클래스를 생성하고, 파일화 하여 저장합니다. </para>
        /// </summary>
        void SaveData()
        {
            VinoveGameSaveData _saveData = new VinoveGameSaveData();
            _saveData._parentSceneUID = _parentSceneUID;
            _saveData._scriptUID = _saveTargetUID;
            _saveData._speakerIllustKey = _speakerIllustKey;
            _saveData._speakerIllustColorAlpha = _speakerIllustColorAlpha;
            _saveData._speakerIllustActivated = _speakerIllustActivated;

            _saveData._savedDate = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            BinaryIOManager.GetInst().BinarySerialize<VinoveGameSaveData>(_saveData, _saveFolderDir + _saveData._savedDate + ".sav");

            ReloadSaveFile(_saveFolderDir + _saveData._savedDate + ".sav");

            if (_state == SaveLoadSlotManagerState.NewGameSave)
            {
                DisableSlot();
                MainMenuManager.GetInstance().DisableMainMenu();
                InGameManager.GetInstance().NewGameInitialize();
            }
        }

        /// <summary>
        /// <para>Eng. Create class for archive and save to file. </para>
        /// <para>Kor. 저장용 클래스를 생성하고, 파일화 하여 저장합니다. </para>
        /// </summary>
        void SaveData(GameObject _button)
        {
            VinoveGameSaveData _saveData = _button.GetComponent<SavedButtonManager>()._savedData;
            string _tmpDate = _saveData._savedDate;

            _saveData._parentSceneUID = _parentSceneUID;
            _saveData._scriptUID = _saveTargetUID;
            _saveData._savedDate = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            _saveData._speakerIllustKey = _speakerIllustKey;
            _saveData._speakerIllustColorAlpha = _speakerIllustColorAlpha;
            _saveData._speakerIllustActivated = _speakerIllustActivated;

            System.IO.File.Delete(_saveFolderDir + _tmpDate + ".sav");
            BinaryIOManager.GetInst().BinarySerialize<VinoveGameSaveData>(_saveData, _saveFolderDir +_saveData._savedDate + ".sav");

            ReloadSaveFile(_saveData);

            if (_state == SaveLoadSlotManagerState.NewGameSave)
            {
                DisableSlot();
                MainMenuManager.GetInstance().DisableMainMenu();
                InGameManager.GetInstance().NewGameInitialize();
            }
        }

        ///////////////////////////////
        /// Object Manage

        /// <summary>
        /// <para>Eng. Manager child objects make activate. </para>
        /// <para>Kor. 매니저 휘하 오브젝트들을 활성화합니다. </para>
        /// </summary>
        public void EnableSlot()
        {
            _saveLoadSlotPanel.SetActive(true);
        }

        /// <summary>
        /// <para>Eng. Manager child objects make deactivate. </para>
        /// <para>Kor. 매니저 휘하 오브젝트들을 비활성화합니다. </para>
        /// </summary>
        public void DisableSlot()
        {
            _addSaveFileButton.SetActive(false);
            _saveLoadSlotPanel.SetActive(false);
        }
    }
}