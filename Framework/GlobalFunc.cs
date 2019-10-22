using System.Collections;
using System.Collections.Generic;

using ViNovE.Framework.Error;

using UnityEngine;

namespace ViNovE.Framework
{
    public class GlobalFunc
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables
        ///
        private static GlobalFunc _instance = null;

        ////////////////////////////////////////////////////////////////////////
        /// Properties
        /// 

        public static GlobalFunc Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GlobalFunc();

                    if (_instance == null)
                    {
                        ErrorMessages.Instance.MissingSingletoneObject("GlobalFunc");
                    }
                }

                return _instance;
            }
        }

        ////////////////////////////////////////////////////////////////////////
        /// Func
        
        public string GetFileNameFromPath(string _path)
        {
            string[] _paths = _path.Split('/');

            return _paths[_paths.Length - 1].Split('.')[0];

        }

        /// <summary>
        /// <para>Eng. Conversion absolute path to unity asset relative path </para>
        /// <para>Kor. 절대 주소를 유니티 애셋 상대 경로로 변환시킵니다. </para>
        /// </summary>
        /// <param name="_absolutePath"> <para> Eng. Absolute path of file</para>
        ///                         <para> Kor. 파일의 절대 경로</para> </param>
        public string PathAbsoluteToAssetRelative(string _absolutePath)
        {
            string _relativePath;
            string searchingText = "Assets/";

            string subStringText = _absolutePath.Substring(0, _absolutePath.IndexOf(searchingText));
            _relativePath = _absolutePath.Replace(subStringText, "");

            return _relativePath;
        }



        /// <summary>
        /// <para>Eng. Find target string index in string array </para>
        /// <para>Kor. 문자열 배열 속에서, 특정 문자열의 인덱스를 찾습니다. </para>
        /// </summary>
        /// <param name="_array"> <para> Eng. string array included target string</para>
        ///                         <para> Kor. 찾을 정보가 담긴 문자열 배열입니다. </para> </param>
        /// <param name="_target"> <para> Eng. Search target string </para>
        ///                         <para> Kor. 찾을 대상 문자열입니다. </para> </param>
        /// <returns>
        /// <para>Eng. Index of found string </para>
        /// <para>Kor. 찾은 문자열의 배열 속 인덱스입니다. </para>
        /// </returns>
        public int FindStringIndexInArray(string[] _array, string _target)
        {
            if (_target == null || _target.Length == 0)
            {
                return 0;
            }

            bool _findFailFlag = true;
            int arrayCount = 0;
            foreach (string _arrayData in _array)
            {
                if (_arrayData.Equals(_target))
                {
                    _findFailFlag = false;
                    break;
                }
                arrayCount++;
            }

            if (_findFailFlag == true)
            {
                arrayCount = 0;
            }

            return arrayCount;
        }
    }
}