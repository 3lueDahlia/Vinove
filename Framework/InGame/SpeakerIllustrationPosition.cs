using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ViNovE.Framework.Enum;

namespace ViNovE.Framework.InGame
{
    public class SpeakerIllustrationPosition : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        public bool _isActivated = false;
        public SpeakerPos _speakerIlluPos;

        ////////////////////////////////////////////////////////////////////////
        /// Properties

        public bool IsActivated
        {
            get { return _isActivated; }
            set { _isActivated = value; }
        }

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. Return speaker illustration position with fit image size. </para>
        /// <para>Kor. 이미지 크기에 맞는 화자 일러스트의 위치를 반환합니다. </para>
        /// </summary>
        public Vector2 GetSpeakerPosition(Rect _imageSize)
        {
            switch(_speakerIlluPos)
            {
                case SpeakerPos.Left:
                    return new Vector2((_imageSize.width / 2) * -1, 0);

                case SpeakerPos.Center:
                    return new Vector2(0, 0);

                case SpeakerPos.Right:
                    return new Vector2((_imageSize.width / 2), 0);

                default:
                    return default;
            }
        }

        public void SetDeactivated()
        {
            _isActivated = false;
            GetComponent<Image>().color = Color.clear;
        }

        public void ColorCurrentSpeaker()
        {
            GetComponent<Image>().color = Color.white;
        }

        public void ColorUncurrentSpeaker()
        {
            if(_isActivated)
            {
                GetComponent<Image>().color = Color.grey;
            }
        }
    }
}