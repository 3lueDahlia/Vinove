using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ViNovE.Framework.Animation;
using ViNovE.Framework.Debugger;
using ViNovE.Framework.Enum;
using ViNovE.Framework.Error;

namespace ViNovE.Framework.InGame
{
    public partial class InGameManager
    {
        /// <summary>
        /// <para>Eng. Launch next script. </para>
        /// <para>Kor. 다음 스크립트 실행. </para>
        /// </summary>
        public void NextScript()
        {
            if (_currentScript != null)
            {
                if (ErrorDetector.Instance.IsStringHasData(_currentScript.Next))
                {
                    _currentScript = _currentScene.FindScriptWithUID(_currentScript.Next);

                    SyncScript();
                }
                else
                {
                    NextScene();
                }
            }
            else
            {
                NextScene();
            }
        }

        /// <summary>
        /// <para>Eng. Launch next scene. </para>
        /// <para>Kor. 다음 씬 실행. </para>
        /// </summary>
        public void NextScene()
        {
            if (_currentScene != null && ErrorDetector.Instance.IsStringHasData(_currentScene.Next))
            {
                FindNextScene(_currentScene.Next);
            }
            else if (_currentBranch != null)
            {
                DisplayBranch();
            }
            else if (_currentMerge != null && ErrorDetector.Instance.IsStringHasData(_currentMerge.Next))
            {
                FindNextScene(_currentMerge.Next);
            }
            else
            {
                _progressState = InGameProgressState.Idle;
                StopAllCoroutines();
            }
        }

        /// <summary>
        /// <para>Eng. Find Next Scene or Branch or Merge. </para>
        /// <para>Kor. 다음 씬, 분기, 병합을 찾습니다. </para>
        /// </summary>
        public void FindNextScene(string _UID)
        {
            if (ErrorDetector.Instance.IsStringHasData(_UID))
            {
                _currentScene = null;
                _currentBranch = null;
                _currentMerge = null;

                if (_UID.StartsWith("SCE"))
                {
                    _currentScene = _currentVinove.FindSceneWithUID(_UID);

                    if (ErrorDetector.Instance.IsStringHasData(_currentScene.StarterUID))
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
                        else
                        {
                            _sceneBGM.clip = null;

                            _sceneBGM.Stop();
                        }

                        // ----------------------- Scene - Starter Script Data -----------------------
                        _currentScript = _currentScene.FindScriptWithUID(_currentScene.StarterUID);

                        SyncScript();
                    }
                    else
                    {
                        NextScene();
                    }
                }
                else if (_UID.StartsWith("BRA"))
                {
                    _currentBranch = _currentVinove.FindBranchWithUID(_UID);
                    NextScene();
                }
                else if (_UID.StartsWith("MRG"))
                {
                    _currentMerge = _currentVinove.FindMergeWithUID(_UID);
                    NextScene();
                }
            }
        }

        /// <summary>
        /// <para>Eng. Call when Auto toggle button clicked. Event Proc </para>
        /// <para>Kor. 오토 토글 버튼 입력 시, 들어오는 이벤트 처리 함수입니다. </para>
        /// </summary>
        public void Next()
        {
            if (SpeakerAnimationManager.Instance.IsAnimRunnin == true || DialogueAnimationManager.Instance.IsAnimRunnin == true && _progressState == InGameProgressState.Idle)
            {
                SpeakerAnimationManager.Instance.SkipAnimation();
                DialogueAnimationManager.Instance.SkipAnimation();
            }
            else if (_currentScene != null && _progressState == InGameProgressState.Idle && SpeakerAnimationManager.Instance.IsAnimRunnin == false && DialogueAnimationManager.Instance.IsAnimRunnin == false)
            {
                NextScript();
            }
        }

        /// <summary>
        /// <para>Eng. Call when Auto toggle button clicked. Event Proc </para>
        /// <para>Kor. 오토 토글 버튼 입력 시, 들어오는 이벤트 처리 함수입니다. </para>
        /// </summary>
        public void Auto()
        {
            if (_progressState != Enum.InGameProgressState.Auto)
            {
                _progressState = Enum.InGameProgressState.Auto;

                StartCoroutine(ProgressCoroutineAuto());
            }
            else
            {
                _progressState = Enum.InGameProgressState.Idle;
            }
        }
        IEnumerator ProgressCoroutineAuto()
        {
            while (_progressState == Enum.InGameProgressState.Auto)
            {
                if (SpeakerAnimationManager.Instance.IsAnimRunnin == false && DialogueAnimationManager.Instance.IsAnimRunnin == false && _uiState != InGameInterfaceDisplayState.Branch)
                {
                    NextScript();
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                }
            }
        }


        /// <summary>
        /// <para>Eng. Call when Skip toggle button clicked. Event Proc </para>
        /// <para>Kor. 스킵 토글 버튼 입력 시, 들어오는 이벤트 처리 함수입니다. </para>
        /// </summary>
        public void Skip()
        {
            if (_progressState != Enum.InGameProgressState.Skip)
            {
                _progressState = Enum.InGameProgressState.Skip;

                StartCoroutine(ProgressCoroutineSkip());
            }
            else
            {
                _progressState = Enum.InGameProgressState.Idle;
            }
        }
        IEnumerator ProgressCoroutineSkip()
        {
            SpeakerAnimationManager.Instance.SkipAnimation();
            DialogueAnimationManager.Instance.SkipAnimation();

            while (_progressState == Enum.InGameProgressState.Skip)
            {
                if (_uiState != InGameInterfaceDisplayState.Branch)
                {
                    NextScript();

                    SpeakerAnimationManager.Instance.SkipAnimation();
                    DialogueAnimationManager.Instance.SkipAnimation();
                }

                yield return new WaitForSeconds(0.1f);
            }
        }


        /// <summary>
        /// <para>Eng. When mouse pointer over on button for block click event </para>
        /// <para>Kor. 버튼에 마우스 오버 시, 버튼 인식을 위해 클릭 이벤트 진행을 막습니다. </para>
        /// </summary>
        public void DisableMouseClick()
        {
            _needDisableClick = true;
        }

        /// <summary>
        /// <para>Eng. When mouse pointer over on button for block click event </para>
        /// <para>Kor. 버튼에 마우스 오버 상태에서, 나갈 경우, 클릭 이벤트 진행을 속개합니다. </para>
        /// </summary>
        public void EnableMouseClick()
        {
            _needDisableClick = false;
        }
    }
}