using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViNovE.Framework.Error
{
    public class ErrorDetector
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables
        /// 

        private static ErrorDetector _instance = null;

        ////////////////////////////////////////////////////////////////////////
        /// Properties
        /// 

        public static ErrorDetector Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ErrorDetector();

                    if (_instance == null)
                    {
                        ErrorMessages.Instance.MissingSingletoneObject("ErrorDetector");
                    }
                }

                return _instance;
            }
        }

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. Check file/folder dir path is available</para>
        /// <para>Kor. 파일 및 폴더의 주소가 유효한지 검사합니다. </para>
        /// </summary>
        /// <param name="_data"> <para> Eng. Link node</para>
        ///                           <para> Kor. 링크 될 노드</para> </param>
        public bool PathChecker(string _Path)
        {
            if (_Path.Length == 0)
            {
                Debug.LogWarning("File Dialog has Canceled");
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// <para>Eng. Detect 'is string didn't empty?'</para>
        /// <para>Kor. 문자열에 데이터가 있는지 검사합니다. </para>
        /// </summary>
        /// <param name="_string"> <para> Eng. Target string</para>
        ///                           <para> Kor. 검사 할 문자열입니다.</para> </param>
        public bool IsStringHasData(string _string)
        {
            if(_string == null || _string == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// <para>Eng. Detect 'is string empty?'</para>
        /// <para>Kor. 문자열이 비어있는지 검사합니다. </para>
        /// </summary>
        /// <param name="_string"> <para> Eng. Target string</para>
        ///                           <para> Kor. 검사 할 문자열입니다.</para> </param>
        public bool IsStringEmpty(string _string)
        {
            if (_string == null || _string == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// <para>Eng. Detect 'Is target rect layered canvas rect?'</para>
        /// <para>Kor. 검사 대상 사각형이, 캔버스 사각형과 겹치는지 판별합니다. </para>
        /// </summary>
        /// <param name="_canvasRect"> <para> Eng. Canvas Rect</para>
        ///                           <para> Kor. 캔버스 사각형입니다.</para> </param>
        /// <param name="_targetRect"> <para> Eng. Detect target Rect</para>
        ///                           <para> Kor. 검사 대상 사각형입니다.</para> </param>
        public bool IsCanvasContainedRect(Rect _canvasRect, Rect _targetRect)
        {
            if(_canvasRect.Contains(new Vector2(_targetRect.x, _targetRect.y)))
            {
                return true;
            }
            else if (_canvasRect.Contains(new Vector2(_targetRect.x, _targetRect.y + _targetRect.height)))
            {
                return true;
            }
            else if (_canvasRect.Contains(new Vector2(_targetRect.x + _targetRect.width, _targetRect.y)))
            {
                return true;
            }
            else if (_canvasRect.Contains(new Vector2(_targetRect.x + _targetRect.width, _targetRect.y + _targetRect.height)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}