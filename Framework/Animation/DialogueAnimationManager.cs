using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ViNovE.Framework.InGame;
using ViNovE.Framework.Error;
using ViNovE.Framework.Enum;

namespace ViNovE.Framework.Animation
{
    public class DialogueAnimationManager : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        ////////////////////////////////
        /// Singleton

        private static DialogueAnimationManager _instance = null;    // Singleton Instance

        ////////////////////////////////
        /// State

        Text _currentText;
        string _currentDialogue;
        Coroutine _playingCoroutine;

        bool _isAnimRunnin = false;
        //DialogueAnimationSpeed _animSpeed = DialogueAnimationSpeed.Normal;

        ////////////////////////////////////////////////////////////////////////
        /// Properties
        /// 

        public static DialogueAnimationManager Instance
        {   //Singleton Property
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType(typeof(DialogueAnimationManager)) as DialogueAnimationManager;

                    if (_instance == null)
                    {
                        ErrorMessages.Instance.MissingSingletoneObject("DialogueAnimation");
                    }
                }

                return _instance;
            }
        }

        public bool IsAnimRunnin
        {
            get { return _isAnimRunnin; }
            set { _isAnimRunnin = value; }
        }

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        ///     <para>Eng. An animation that draws text from the dialogue window one by one. </para>
        ///     <para>Kor. 대사 창의 글자를 한 글자씩 그려주는 애니메이션입니다. </para>
        /// </summary>
        /// <param name="_targetObjectText">
        ///     <para>Eng. Text component of dialogue window object. </para>
        ///     <para>Kor. 대사 창 오브젝트의 Text 컴포넌트 입니다. </para></param>
        /// <param name="_targetDialogue">
        ///     <para>Eng. This is the dialogue should input to UI. </para>
        ///     <para>Kor. UI에 입력 해야할 대사입니다. </para></param>
        public void PlayAnimation(Text _targetObjectText, string _targetDialogue)
        {
            _isAnimRunnin = true;

            _currentText = _targetObjectText;
            _currentDialogue = _targetDialogue;


            if (ErrorDetector.Instance.IsStringHasData(_targetDialogue))
            {
                _playingCoroutine = StartCoroutine(AnimCoroutineDialogue(_targetObjectText, _targetDialogue));
            }
            else
            {
                _targetObjectText.text = "";

                InitVariables();
            }
        }
        IEnumerator AnimCoroutineDialogue(Text _targetObjectText, string _targetDialogue)
        {
            for (int i=0; i<=_targetDialogue.Length; i++)
            {
                _targetObjectText.text = _targetDialogue.Substring(0, i);

                yield return new WaitForSeconds(0.01f * InGameManager.GetInstance().DialogueSpeed);
            }

            InitVariables();
        }

        //----------------------------------------------------

        /// <summary>
        ///     <para>Eng. Skip runnin animation. </para>
        ///     <para>Kor. 진행중인 애니매이션을 건너뜁니다. </para>
        /// </summary>
        public void SkipAnimation()
        {
            if(_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
                _currentText.text = _currentDialogue;

                InitVariables();
            }
        }

        /// <summary>
        ///     <para>Eng. Basic value init variables. </para>
        ///     <para>Kor. 변수들의 기본 값 초기화입니다. </para>
        /// </summary>
        public void InitVariables()
        {
            _isAnimRunnin = false;

            _currentText = null;
            _currentDialogue = null;

            _playingCoroutine = null;
        }
    }
}