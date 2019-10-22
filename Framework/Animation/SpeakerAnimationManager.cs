using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ViNovE.Framework.Error;
using ViNovE.Framework.Enum;
using ViNovE.Framework.InGame;

namespace ViNovE.Framework.Animation
{
    public class SpeakerAnimationManager : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        ////////////////////////////////
        /// Singleton

        private static SpeakerAnimationManager _instance = null;    // Singleton Instance

        ////////////////////////////////
        /// State

        bool _isAnimRunnin = false;

        GameObject _currentObject;
        Vector2 _originPos = Vector2.zero;
        Color _targetColor = Color.clear;
        Coroutine _playingCoroutine;

        ////////////////////////////////////////////////////////////////////////
        /// Properties
        /// 
        public static SpeakerAnimationManager Instance
        {   //Singleton Property
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType(typeof(SpeakerAnimationManager)) as SpeakerAnimationManager;

                    if (_instance == null)
                    {
                        ErrorMessages.Instance.MissingSingletoneObject("SpeakerAnimationManager");
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

        public void PlayAnimation(GameObject _targetObject, Enum.AnimationState _state) 
        {
            _isAnimRunnin = true;

            _currentObject = _targetObject;

            Color _originColor = _targetObject.GetComponent<Image>().color;
            _originColor.a = 1.0f;
            _targetObject.GetComponent<Image>().color = _originColor;

            RectTransform _targetObjectTransform = _targetObject.GetComponent<RectTransform>();
            _originPos = _targetObjectTransform.anchoredPosition;

            switch (_state)
            {
                case Enum.AnimationState.Idle:
                    _targetObject.GetComponent<SpeakerIllustrationPosition>().IsActivated = true;
                    _isAnimRunnin = false;
                    break;

                //---------------------- Slide - In ----------------------
                case Enum.AnimationState.SlideInLtoR:
                    AnimSlideInLeftToRight(_targetObject);
                    break;
                case Enum.AnimationState.SlideInRtoL:
                    AnimSlideInRightToLeft(_targetObject);
                    break;
                case Enum.AnimationState.SlideInBtoT:
                    AnimSlideInBottomToTop(_targetObject);
                    break;
                case Enum.AnimationState.SlideInTtoB:
                    AnimSlideInTopToBottom(_targetObject);
                    break;

                //---------------------- Slide - Out ----------------------
                case Enum.AnimationState.SlideOutLtoR:
                    AnimSlideOutLeftToRight(_targetObject);
                    break;
                case Enum.AnimationState.SlideOutRtoL:
                    AnimSlideOutRightToLeft(_targetObject);
                    break;
                case Enum.AnimationState.SlideOutBtoT:
                    AnimSlideOutBottomToTop(_targetObject);
                    break;
                case Enum.AnimationState.SlideOutTtoB:
                    AnimSlideOutTopToBottom(_targetObject);
                    break;

                //---------------------- Fade ----------------------
                case Enum.AnimationState.FadeIn:
                    AnimFadeIn(_targetObject);
                    break;
                case Enum.AnimationState.FadeOut:
                    AnimFadeOut(_targetObject);
                    break;

                //---------------------- Emotion ----------------------
                case Enum.AnimationState.Vibrate:
                    AnimVibrate(_targetObject);
                    break;
                case Enum.AnimationState.Surprised:
                    AnimSurprised(_targetObject);
                    break;
            }
        }

        //---------------------- Slide - In ----------------------

        /// <summary>
        ///     <para>Eng. Animation that slide in from left to right. </para>
        ///     <para>Kor. 왼쪽에서 오른쪽으로 슬라이드하며 들어오는 애니메이션입니다. </para>
        /// </summary>
        /// <param name="_targetObject">
        ///     <para>Eng. Illustration object for animation. </para>
        ///     <para>Kor. 애니메이션 대상 일러스트 오브젝트입니다. </para></param>
        private void AnimSlideInLeftToRight(GameObject _targetObject)
        {
            _playingCoroutine = StartCoroutine(AnimCoroutineSlideInLtoR(_targetObject));
        }
        IEnumerator AnimCoroutineSlideInLtoR(GameObject _targetObject)
        {
            const int _posGap = 50;

            _targetObject.GetComponent<SpeakerIllustrationPosition>().IsActivated = true;

            RectTransform _targetObjectTransform = _targetObject.GetComponent<RectTransform>();
            _targetObjectTransform.anchoredPosition = new Vector2(_targetObjectTransform.anchoredPosition.x - _posGap, _targetObjectTransform.anchoredPosition.y);

            Image _objectImage = GetImageWithInitAlpha(_targetObject, true, out _targetColor);

            for (int i = 0; i < _posGap; i++)
            {
                _targetObjectTransform.anchoredPosition = new Vector2(_targetObjectTransform.anchoredPosition.x + 1, _targetObjectTransform.anchoredPosition.y);

                ImageAlphaFade(_objectImage, _posGap, true);

                yield return new WaitForSeconds(0.01f);
            }

            _targetObjectTransform.anchoredPosition = _originPos;

            InitVariables();
        }

        /// <summary>
        ///     <para>Eng. Animation that slide in from right to left. </para>
        ///     <para>Kor. 오른쪽에서 왼쪽으로 슬라이드하며 들어오는 애니메이션입니다. </para>
        /// </summary>
        /// <param name="_targetObject">
        ///     <para>Eng. Illustration object for animation. </para>
        ///     <para>Kor. 애니메이션 대상 일러스트 오브젝트입니다. </para></param>
        private void AnimSlideInRightToLeft(GameObject _targetObject)
        {
            _playingCoroutine = StartCoroutine(AnimCoroutineSlideInRtoL(_targetObject));
        }
        IEnumerator AnimCoroutineSlideInRtoL(GameObject _targetObject)
        {
            const int _posGap = 50;

            _targetObject.GetComponent<SpeakerIllustrationPosition>().IsActivated = true;

            RectTransform _targetObjectTransform = _targetObject.GetComponent<RectTransform>();
            _targetObjectTransform.anchoredPosition = new Vector2(_targetObjectTransform.anchoredPosition.x + _posGap, _targetObjectTransform.anchoredPosition.y);

            Image _objectImage = GetImageWithInitAlpha(_targetObject, true, out _targetColor);

            for (int i = 0; i < _posGap; i++)
            {
                _targetObjectTransform.anchoredPosition = new Vector2(_targetObjectTransform.anchoredPosition.x - 1, _targetObjectTransform.anchoredPosition.y);

                ImageAlphaFade(_objectImage, _posGap, true);

                yield return new WaitForSeconds(0.01f);
            }

            _targetObjectTransform.anchoredPosition = _originPos;

            InitVariables();
        }

        /// <summary>
        ///     <para>Eng. Animation that slide in from bottom to top. </para>
        ///     <para>Kor. 아래쪽에서 위쪽으로 슬라이드하며 들어오는 애니메이션입니다. </para>
        /// </summary>
        /// <param name="_targetObject">
        ///     <para>Eng. Illustration object for animation. </para>
        ///     <para>Kor. 애니메이션 대상 일러스트 오브젝트입니다. </para></param>
        private void AnimSlideInBottomToTop(GameObject _targetObject)
        {
            _playingCoroutine = StartCoroutine(AnimCoroutineSlideInBtoT(_targetObject));
        }
        IEnumerator AnimCoroutineSlideInBtoT(GameObject _targetObject)
        {
            const int _posGap = 50;

            _targetObject.GetComponent<SpeakerIllustrationPosition>().IsActivated = true;

            RectTransform _targetObjectTransform = _targetObject.GetComponent<RectTransform>();
            _targetObjectTransform.anchoredPosition = new Vector2(_targetObjectTransform.anchoredPosition.x, _targetObjectTransform.anchoredPosition.y - _posGap);

            Image _objectImage = GetImageWithInitAlpha(_targetObject, true, out _targetColor);

            for (int i = 0; i < _posGap; i++)
            {
                _targetObjectTransform.anchoredPosition = new Vector2(_targetObjectTransform.anchoredPosition.x, _targetObjectTransform.anchoredPosition.y + 1);

                ImageAlphaFade(_objectImage, _posGap, true);

                yield return new WaitForSeconds(0.01f);
            }

            _targetObjectTransform.anchoredPosition = _originPos;

            InitVariables();
        }

        /// <summary>
        ///     <para>Eng. Animation that slide in from top to bottom. </para>
        ///     <para>Kor. 위쪽에서 아래쪽으로 슬라이드하며 들어오는 애니메이션입니다. </para>
        /// </summary>
        /// <param name="_targetObject">
        ///     <para>Eng. Illustration object for animation. </para>
        ///     <para>Kor. 애니메이션 대상 일러스트 오브젝트입니다. </para></param>
        private void AnimSlideInTopToBottom(GameObject _targetObject)
        {
            _playingCoroutine = StartCoroutine(AnimCoroutineSlideInTtoB(_targetObject));
        }
        IEnumerator AnimCoroutineSlideInTtoB(GameObject _targetObject)
        {
            const int _posGap = 50;

            _targetObject.GetComponent<SpeakerIllustrationPosition>().IsActivated = true;

            RectTransform _targetObjectTransform = _targetObject.GetComponent<RectTransform>();
            _targetObjectTransform.anchoredPosition = new Vector2(_targetObjectTransform.anchoredPosition.x, _targetObjectTransform.anchoredPosition.y + _posGap);

            Image _objectImage = GetImageWithInitAlpha(_targetObject, true, out _targetColor);

            for (int i = 0; i < _posGap; i++)
            {
                _targetObjectTransform.anchoredPosition = new Vector2(_targetObjectTransform.anchoredPosition.x, _targetObjectTransform.anchoredPosition.y - 1);

                ImageAlphaFade(_objectImage, _posGap, true);

                yield return new WaitForSeconds(0.01f);
            }

            _targetObjectTransform.anchoredPosition = _originPos;

            InitVariables();
        }

        //---------------------- Slide - Out ----------------------

        /// <summary>
        ///     <para>Eng. Animation that slide out from left to right. </para>
        ///     <para>Kor. 왼쪽에서 오른쪽으로 슬라이드하며 나가는 애니메이션입니다. </para>
        /// </summary>
        /// <param name="_targetObject">
        ///     <para>Eng. Illustration object for animation. </para>
        ///     <para>Kor. 애니메이션 대상 일러스트 오브젝트입니다. </para></param>
        private void AnimSlideOutLeftToRight(GameObject _targetObject)
        {
            _playingCoroutine = StartCoroutine(AnimCoroutineSlideOutLtoR(_targetObject));
        }
        IEnumerator AnimCoroutineSlideOutLtoR(GameObject _targetObject)
        {
            const int _posGap = 50;

            RectTransform _targetObjectTransform = _targetObject.GetComponent<RectTransform>();
            _targetObjectTransform.anchoredPosition = new Vector2(_targetObjectTransform.anchoredPosition.x, _targetObjectTransform.anchoredPosition.y);

            Image _objectImage = GetImageWithInitAlpha(_targetObject, false, out _targetColor);

            for (int i = 0; i < _posGap; i++)
            {
                _targetObjectTransform.anchoredPosition = new Vector2(_targetObjectTransform.anchoredPosition.x + 1, _targetObjectTransform.anchoredPosition.y);

                ImageAlphaFade(_objectImage, _posGap, false);

                yield return new WaitForSeconds(0.01f);
            }

            _targetObjectTransform.anchoredPosition = _originPos;

            _targetObject.GetComponent<SpeakerIllustrationPosition>().IsActivated = false;

            InitVariables();
        }

        /// <summary>
        ///     <para>Eng. Animation that slide out from right to left. </para>
        ///     <para>Kor. 오른쪽에서 왼쪽으로 슬라이드하며 나가는 애니메이션입니다. </para>
        /// </summary>
        /// <param name="_targetObject">
        ///     <para>Eng. Illustration object for animation. </para>
        ///     <para>Kor. 애니메이션 대상 일러스트 오브젝트입니다. </para></param>
        private void AnimSlideOutRightToLeft(GameObject _targetObject)
        {
            _playingCoroutine = StartCoroutine(AnimCoroutineSlideOutRtoL(_targetObject));
        }
        IEnumerator AnimCoroutineSlideOutRtoL(GameObject _targetObject)
        {
            const int _posGap = 50;

            RectTransform _targetObjectTransform = _targetObject.GetComponent<RectTransform>();
            _targetObjectTransform.anchoredPosition = new Vector2(_targetObjectTransform.anchoredPosition.x, _targetObjectTransform.anchoredPosition.y);

            Image _objectImage = GetImageWithInitAlpha(_targetObject, false, out _targetColor);

            for (int i = 0; i < _posGap; i++)
            {
                _targetObjectTransform.anchoredPosition = new Vector2(_targetObjectTransform.anchoredPosition.x - 1, _targetObjectTransform.anchoredPosition.y);

                ImageAlphaFade(_objectImage, _posGap, false);

                yield return new WaitForSeconds(0.01f);
            }

            _targetObjectTransform.anchoredPosition = _originPos;

            _targetObject.GetComponent<SpeakerIllustrationPosition>().IsActivated = false;

            InitVariables();
        }

        /// <summary>
        ///     <para>Eng. Animation that slide out from bottom to top. </para>
        ///     <para>Kor. 아래쪽에서 위쪽으로 슬라이드하며 나가는 애니메이션입니다. </para>
        /// </summary>
        /// <param name="_targetObject">
        ///     <para>Eng. Illustration object for animation. </para>
        ///     <para>Kor. 애니메이션 대상 일러스트 오브젝트입니다. </para></param>
        private void AnimSlideOutBottomToTop(GameObject _targetObject)
        {
            _playingCoroutine = StartCoroutine(AnimCoroutineSlideOutBtoT(_targetObject));
        }
        IEnumerator AnimCoroutineSlideOutBtoT(GameObject _targetObject)
        {
            const int _posGap = 50;

            RectTransform _targetObjectTransform = _targetObject.GetComponent<RectTransform>();
            _targetObjectTransform.anchoredPosition = new Vector2(_targetObjectTransform.anchoredPosition.x, _targetObjectTransform.anchoredPosition.y);

            Image _objectImage = GetImageWithInitAlpha(_targetObject, false, out _targetColor);

            for (int i = 0; i < _posGap; i++)
            {
                _targetObjectTransform.anchoredPosition = new Vector2(_targetObjectTransform.anchoredPosition.x, _targetObjectTransform.anchoredPosition.y + 1);

                ImageAlphaFade(_objectImage, _posGap, false);

                yield return new WaitForSeconds(0.01f);
            }

            _targetObjectTransform.anchoredPosition = _originPos;

            _targetObject.GetComponent<SpeakerIllustrationPosition>().IsActivated = false;

            InitVariables();
        }

        /// <summary>
        ///     <para>Eng. Animation that slide out from top to bottom. </para>
        ///     <para>Kor. 위쪽에서 아래쪽으로 슬라이드하며 나가는 애니메이션입니다. </para>
        /// </summary>
        /// <param name="_targetObject">
        ///     <para>Eng. Illustration object for animation. </para>
        ///     <para>Kor. 애니메이션 대상 일러스트 오브젝트입니다. </para></param>
        private void AnimSlideOutTopToBottom(GameObject _targetObject)
        {
            _playingCoroutine = StartCoroutine(AnimCoroutineSlideOutTtoB(_targetObject));
        }
        IEnumerator AnimCoroutineSlideOutTtoB(GameObject _targetObject)
        {
            const int _posGap = 50;

            RectTransform _targetObjectTransform = _targetObject.GetComponent<RectTransform>();
            _targetObjectTransform.anchoredPosition = new Vector2(_targetObjectTransform.anchoredPosition.x, _targetObjectTransform.anchoredPosition.y);

            Image _objectImage = GetImageWithInitAlpha(_targetObject, false, out _targetColor);

            for (int i = 0; i < _posGap; i++)
            {
                _targetObjectTransform.anchoredPosition = new Vector2(_targetObjectTransform.anchoredPosition.x, _targetObjectTransform.anchoredPosition.y - 1);

                ImageAlphaFade(_objectImage, _posGap, false);

                yield return new WaitForSeconds(0.01f);
            }

            _targetObjectTransform.anchoredPosition = _originPos;

            _targetObject.GetComponent<SpeakerIllustrationPosition>().IsActivated = false;

            InitVariables();
        }

        //---------------------- Fade ----------------------

        /// <summary>
        ///     <para>Eng. Animation that color fade in. </para>
        ///     <para>Kor. 색이 서서히 짙어지게 만듭니다. </para>
        /// </summary>
        /// <param name="_targetObject">
        ///     <para>Eng. Illustration object for animation. </para>
        ///     <para>Kor. 애니메이션 대상 일러스트 오브젝트입니다. </para></param>
        private void AnimFadeIn(GameObject _targetObject)
        {
            _playingCoroutine = StartCoroutine(AnimCoroutineFadeIn(_targetObject));
        }
        IEnumerator AnimCoroutineFadeIn(GameObject _targetObject)
        {
            const int _repeatCount = 50;

            _targetObject.GetComponent<SpeakerIllustrationPosition>().IsActivated = true;

            Image _objectImage = GetImageWithInitAlpha(_targetObject, true, out _targetColor);

            for (int i = 0; i < _repeatCount; i++)
            {
                ImageAlphaFade(_objectImage, _repeatCount, true);

                yield return new WaitForSeconds(0.01f);
            }

            InitVariables();
        }

        /// <summary>
        ///     <para>Eng. Animation that color fade out. </para>
        ///     <para>Kor. 색이 서서히 옅어지게 만듭니다. </para>
        /// </summary>
        /// <param name="_targetObject">
        ///     <para>Eng. Illustration object for animation. </para>
        ///     <para>Kor. 애니메이션 대상 일러스트 오브젝트입니다. </para></param>
        private void AnimFadeOut(GameObject _targetObject)
        {
            _playingCoroutine = StartCoroutine(AnimCoroutineFadeOut(_targetObject));
        }
        IEnumerator AnimCoroutineFadeOut(GameObject _targetObject)
        {
            const int _repeatCount = 50;

            Image _objectImage = GetImageWithInitAlpha(_targetObject, false, out _targetColor);

            for (int i = 0; i < _repeatCount; i++)
            {
                ImageAlphaFade(_objectImage, _repeatCount, false);

                yield return new WaitForSeconds(0.01f);
            }

            _targetObject.GetComponent<SpeakerIllustrationPosition>().IsActivated = false;

            InitVariables();
        }

        //---------------------- Emotion ----------------------

        /// <summary>
        ///     <para>Eng. Animation that Illust Vibrate horizontal </para>
        ///     <para>Kor. 일러스트를 좌우로 떨리게 만듭니다. </para>
        /// </summary>
        /// <param name="_targetObject">
        ///     <para>Eng. Illustration object for animation. </para>
        ///     <para>Kor. 애니메이션 대상 일러스트 오브젝트입니다. </para></param>
        private void AnimVibrate(GameObject _targetObject)
        {
            _playingCoroutine = StartCoroutine(AnimCoroutineVibrate(_targetObject));
        }
        IEnumerator AnimCoroutineVibrate(GameObject _targetObject)
        {
            const int _repeatCount = 15;

            _targetObject.GetComponent<SpeakerIllustrationPosition>().IsActivated = true;

            RectTransform _targetObjectTransform = _targetObject.GetComponent<RectTransform>();

            int _vibrateVelocity = 4;

            Vector2 _movedPos = new Vector2(_targetObjectTransform.anchoredPosition.x - (_vibrateVelocity / 2), _targetObjectTransform.anchoredPosition.y);
            _targetObjectTransform.anchoredPosition = _movedPos;

            for (int i = 0; i < _repeatCount; i++)
            {
                _movedPos.x += _vibrateVelocity;
                _targetObjectTransform.anchoredPosition = _movedPos;
                _vibrateVelocity = (-_vibrateVelocity);

                yield return new WaitForSeconds(0.02f);
            }

            _targetObjectTransform.anchoredPosition = _originPos;

            InitVariables();
        }

        /// <summary>
        ///     <para>Eng. Illustration being lifted up and return original position. For looks like surprised</para>
        ///     <para>Kor. 이미지를 위로 띄웠다가 원래대로 복귀하여, 놀란 감정표현을 묘사합니다. </para>
        /// </summary>
        /// <param name="_targetObject">
        ///     <para>Eng. Illustration object for animation. </para>
        ///     <para>Kor. 애니메이션 대상 일러스트 오브젝트입니다. </para></param>
        private void AnimSurprised(GameObject _targetObject)
        {
            _playingCoroutine = StartCoroutine(AnimCoroutineSurprised(_targetObject));
        }
        IEnumerator AnimCoroutineSurprised (GameObject _targetObject)
        {
            const int _repeatCount = 20;

            _targetObject.GetComponent<SpeakerIllustrationPosition>().IsActivated = true;

            RectTransform _targetObjectTransform = _targetObject.GetComponent<RectTransform>();

            int _surpriseVelocity = 2;

            Vector2 _movedPos = new Vector2(_targetObjectTransform.anchoredPosition.x, _targetObjectTransform.anchoredPosition.y);

            for (int i = 0; i < _repeatCount/2; i++)
            {
                _movedPos.y += _surpriseVelocity;
                _targetObjectTransform.anchoredPosition = _movedPos;

                yield return new WaitForSeconds(0.01f);
            }
            for (int i = 0; i < _repeatCount / 2; i++)
            {
                _movedPos.y -= _surpriseVelocity;
                _targetObjectTransform.anchoredPosition = _movedPos;

                yield return new WaitForSeconds(0.01f);
            }

            _targetObjectTransform.anchoredPosition = _originPos;

            InitVariables();
        }

        /// <summary>
        ///     <para>Eng. return image after illust object image alpha with init func.</para>
        ///     <para>Kor. 일러스트 오브젝트의 이미지 알파값을 초기화하며, 이미지를 반환해주는 초기화 함수입니다. </para>
        /// </summary>
        /// <param name="_targetObject">
        ///     <para>Eng. Illustration object. </para>
        ///     <para>Kor. 대상 일러스트 오브젝트입니다. </para></param>
        /// <param name="_IsTransparent">
        ///     <para>Eng. true : Transperent image / false : non-Transparent. </para>
        ///     <para>Kor. true : 투명한 이미지 / false : 불투명한 이미지. </para></param>
        /// <returns>
        ///     <para>Eng. Image Component of Illustration Object. </para>
        ///     <para>Kor. 오브젝트의 Image 컴포넌트. </para>
        /// </returns>
        private Image GetImageWithInitAlpha(GameObject _targetObject, bool _IsTransparent, out Color _targetColor)
        {
            switch (_IsTransparent)
            {
                case true:
                 { 
                    Image _objectImage = _targetObject.GetComponent<Image>();
                    Color _alphaColor = _objectImage.color;
                    _alphaColor.a = 0f;
                    _objectImage.color = _alphaColor;

                    _targetColor = _alphaColor;
                    _targetColor.a = 1f;

                    return _objectImage;
                }
                case false:
                {
                    Image _objectImage = _targetObject.GetComponent<Image>();
                    Color _alphaColor = _objectImage.color;
                    _alphaColor.a = 1f;
                    _objectImage.color = _alphaColor;

                    _targetColor = _alphaColor;
                    _targetColor.a = 0f;

                    return _objectImage;
                }
                default:
                    _targetColor = default;
                    return null;
            }
        }

        /// <summary>
        ///     <para>Eng. Fade in or Fade out using image's alpha </para>
        ///     <para>Kor. 이미지의 알파값을 조정하여 점점 투명하게 혹은 점점 불투명하게 만듭니다. </para>
        /// </summary>
        /// <param name="_targetImage">
        ///     <para>Eng. Image component of target illustration object. </para>
        ///     <para>Kor. 대상 일러스트 오브젝트의 이미지 컴포넌트. </para></param>
        /// <param name="_repeatCount">
        ///     <para>Eng. Repeat count meaning animation time length. </para>
        ///     <para>Kor. 애니메이션의 길이를 의미하는 반복 회수입니다. </para></param>
        /// <param name="_IsFadeIn">
        ///     <para>Eng. true : Fade In / false : Fade Out. </para>
        ///     <para>Kor. true : 점점 불투명하게 / false : 점점 투명하게. </para></param>
        private void ImageAlphaFade(Image _targetImage, int _repeatCount, bool _IsFadeIn)
        {
            switch(_IsFadeIn)
            {
                case true:
                {
                    Color _alphaColor = _targetImage.color;

                    _alphaColor.a += 1.0f / _repeatCount;
                    _targetImage.color = _alphaColor;
                    break;
                }
                case false:
                {
                    Color _alphaColor = _targetImage.color;

                    _alphaColor.a -= 1.0f / _repeatCount;
                    _targetImage.color = _alphaColor;
                    break;
                }
            }
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

                _currentObject.GetComponent<RectTransform>().anchoredPosition = _originPos;

                if (_targetColor != Color.clear)
                {
                    _currentObject.GetComponent<Image>().color = _targetColor;
                }
            }

            InitVariables();
        }

        /// <summary>
        ///     <para>Eng. Basic value init variables. </para>
        ///     <para>Kor. 변수들의 기본 값 초기화입니다. </para>
        /// </summary>
        public void InitVariables()
        {
            _isAnimRunnin = false;

            _currentObject = null;
            _originPos = Vector2.zero;
            _targetColor = Color.clear;
            _playingCoroutine = null;
        }
    }
}