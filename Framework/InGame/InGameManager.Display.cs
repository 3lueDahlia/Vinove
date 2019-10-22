using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ViNovE.Framework.Animation;
using ViNovE.Framework.Data;
using ViNovE.Framework.InGame.Utility;

namespace ViNovE.Framework.InGame
{
    public partial class InGameManager
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        float _dialogueSpeed = 4f;

        ////////////////////////////////////////////////////////////////////////
        /// Properties

        public int DialogueSpeed
        {
            get
            {
                switch (_dialogueSpeed)
                {
                    case 4f:
                        return 0;
                    case 1f:
                        return 1;
                    case 0.25f:
                        return 2;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (value)
                {
                    case 0:
                        _dialogueSpeed = 4f;
                        break;
                    case 1:
                        _dialogueSpeed = 1f;
                        break;
                    case 2:
                        _dialogueSpeed = 0.25f;
                        break;
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /////////////////////////////////
        /// Branch

        /// <summary>
        /// <para>Eng. Launch Branch. </para>
        /// <para>Kor. 분기 실행. </para>
        /// </summary>
        public void DisplayBranch()
        {
            _uiState = Enum.InGameInterfaceDisplayState.Branch;
            _branchPanel.SetActive(true);
            _branchButtons = new GameObject[_currentBranch.BranchCount];

            Vector2 _buttonSize = _branchButtonPrefab.GetComponent<RectTransform>().sizeDelta;
            float _buttonGap = _buttonSize.y / 2;
            float _branchPosY = -300 + ((_buttonSize.y + _buttonGap) * (_currentBranch.BranchCount/2));

            for (int i = 0; i < _currentBranch.BranchCount; i++)
            {
                _branchButtons[i] = Instantiate<GameObject>(_branchButtonPrefab
                    , _branchPanel.transform);

                _branchButtons[i].GetComponent<BranchTextChanger>().ChangeText(_currentBranch.BranchsAlternatives[i]);

                RectTransform _transf = _branchButtons[i].GetComponent<RectTransform>();
                _transf.anchoredPosition = new Vector2(0, _branchPosY);

                _branchPosY -= _buttonSize.y + _buttonGap;

                Button _button = _branchButtons[i].GetComponent<Button>();
                _button.onClick.AddListener(delegate ()
                {
                    int _buttonIndex = FindButtonIndex(_button);

                    RemoveAllBranchButtons();
                    _branchPanel.SetActive(false);
                    _uiState = Enum.InGameInterfaceDisplayState.Dialogue;

                    if(_currentBranch.IsEmptyAllNextLink() == false)
                    {
                        FindNextScene(_currentBranch.BranchsUIDs[_buttonIndex]);
                    }
                    else
                    {
                        _currentBranch = null;
                    }
                });
            }
        }

        /// <summary>
        /// <para>Eng. Delete all branch button on screen. </para>
        /// <para>Kor. 화면에 존재하는 모든 분기 버튼을 삭제합니다. </para>
        /// </summary>
        public void RemoveAllBranchButtons()
        {
            for (int i = 0; i < _branchButtons.Length; i++)
            {
                GameObject.DestroyImmediate(_branchButtons[i]);
            }
        }

        /// <summary>
        /// <para>Eng. Launch next scene. </para>
        /// <para>Kor. 다음 씬 실행. </para>
        /// </summary>
        public int FindButtonIndex(Button _targetButton)
        {
            for (int j = 0; j < _branchButtons.Length; j++)
            {
                if (_branchButtons[j].GetComponent<Button>() == _targetButton)
                {
                    return j;
                }
            }

            return default;
        }

        /////////////////////////////////
        /// Scene

        /// <summary>
        /// <para>Eng. Draw current scene data on ui. </para>
        /// <para>Kor. 현재 씬에 맞는 정보를 UI로 그려줍니다. </para>
        /// </summary>
        public void SyncScene()
        {
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
        }

        /////////////////////////////////
        /// Script

        /// <summary>
        /// <para>Eng. Draw current script data on ui. </para>
        /// <para>Kor. 현재 스크립트에 맞는 정보를 UI로 그려줍니다. </para>
        /// </summary>
        public void SyncScript()
        {
            if (_currentScript != null)
            {
                Texture2D _speakerIllust;

                Image _targetImage;

                GetSpeakerIllustPosToImage(_currentScript, out _targetImage);

                _speakerLeftIllustration.GetComponent<SpeakerIllustrationPosition>().ColorUncurrentSpeaker();
                _speakerCenterIllustration.GetComponent<SpeakerIllustrationPosition>().ColorUncurrentSpeaker();
                _speakerRightIllustration.GetComponent<SpeakerIllustrationPosition>().ColorUncurrentSpeaker();

                // ----------------------- Script - Speaker Illust -----------------------
                if (_currentVinove.SpeakerIllustrations.GetTextureData(_currentScript.ScriptConversation.SpeakerIllustKey, out _speakerIllust))
                {
                    SaveSpeakerIllustKeyAndAlpha(_currentScript, _targetImage);

                    SetSpeakerIllustImageFromTexture(_targetImage, _speakerIllust);

                    _targetImage.GetComponent<SpeakerIllustrationPosition>().ColorCurrentSpeaker();
                    _targetImage.gameObject.transform.SetSiblingIndex(2);

                    SpeakerAnimationManager.Instance.PlayAnimation(_targetImage.gameObject, _currentScript.ScriptConversation.SpeakerAnim);
                }

                // ----------------------- Script - Speaker Name -----------------------
                _speakerNameText.text = _currentScript.ScriptConversation.Speaker;


                // ----------------------- Script - Dialogue -----------------------
                DialogueAnimationManager.Instance.PlayAnimation(_dialogueText, _currentScript.ScriptConversation.Dialog);

                // ----------------------- Script - Sound Effect -----------------------
                if (_currentScript.ScriptConversation.SoundEffect != null)
                {
                    _scriptSoundEffect.clip = _currentScript.ScriptConversation.SoundEffect;

                    _scriptSoundEffect.Play();
                }
                else
                {
                    _scriptSoundEffect.clip = null;

                    _scriptSoundEffect.Stop();
                }
            }
        }

        /// <summary>
        /// <para>Eng. Set Speaker illustration in target object position using Texture2D data. </para>
        /// <para>Kor. Texture2D 데이터를 이용하여 대상 위치의 오브젝트에 화자 일러스트를 배치합니다. </para>
        /// </summary>
        public void SetSpeakerIllustImageFromTexture(Image _targetImage, Texture2D _speakerIllust)
        {
            Rect _speakerTexRect = new Rect(0, 0, _speakerIllust.width, _speakerIllust.height);
            _targetImage.rectTransform.sizeDelta = new Vector2(_speakerTexRect.width, _speakerTexRect.height);
            _targetImage.sprite = Sprite.Create(_speakerIllust, _speakerTexRect, new Vector2(0.5f, 0.5f));
        }

        public void SaveSpeakerIllustKeyAndAlpha(VinoveScript _currentScript, Image _speakerIluustImage)
        {
            switch (_currentScript.ScriptConversation.SpeakerPos)
            {
                case Enum.SpeakerPos.Left:
                    _leftIllustKey = _currentScript.ScriptConversation.SpeakerIllustKey;
                    break;
                case Enum.SpeakerPos.Center:
                    _centerIllustKey = _currentScript.ScriptConversation.SpeakerIllustKey;
                    break;
                case Enum.SpeakerPos.Right:
                    _rightIllustKey = _currentScript.ScriptConversation.SpeakerIllustKey;
                    break;
            }
        }

        /// <summary>
        /// <para>Eng. Returns an object image of the location where the speaker image of the script appears. </para>
        /// <para>Kor. 해당 스크립트의 화자 이미지가 나타날 위치의 오브젝트 이미지를 반환합니다. </para>
        /// </summary>
        /// <returns>
        /// <para>Eng. The image component of the object where the speaker's illustration will be placed. </para>
        /// <para>Kor. 해당 화자의 일러스트가 들어갈 오브젝트의 Image 컴포넌트 입니다. </para>
        /// </returns>
        public void GetSpeakerIllustPosToImage(VinoveScript _currentScript, out Image _image)
        {
            switch (_currentScript.ScriptConversation.SpeakerPos)
            {
                case Enum.SpeakerPos.Left:
                    _image = _speakerLeftIllustration;
                    break;
                case Enum.SpeakerPos.Center:
                    _image = _speakerCenterIllustration;
                    break;
                case Enum.SpeakerPos.Right:
                    _image = _speakerRightIllustration;
                    break;
                default:
                    _image = null;
                    break;
            }
        }

        /// <summary>
        /// <para>Eng. Returns an object image of the location where the speaker image of the script appears. </para>
        /// <para>Kor. 해당 스크립트의 화자 이미지가 나타날 위치의 오브젝트 이미지를 반환합니다. </para>
        /// </summary>
        /// <returns>
        /// <para>Eng. The image component of the object where the speaker's illustration will be placed. </para>
        /// <para>Kor. 해당 화자의 일러스트가 들어갈 오브젝트의 키 저장용 문자열 입니다. </para>
        /// </returns>
        public string GetSpeakerIllustPosToString(VinoveScript _currentScript)
        {
            switch (_currentScript.ScriptConversation.SpeakerPos)
            {
                case Enum.SpeakerPos.Left:
                    return _leftIllustKey;
                case Enum.SpeakerPos.Center:
                    return _centerIllustKey;
                case Enum.SpeakerPos.Right:
                    return _rightIllustKey;
                default:
                    return null;
            }
        }
    }
}