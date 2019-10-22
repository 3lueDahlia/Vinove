using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViNovE.Framework.Error
{
    public class ErrorMessages
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables
        ///
        private static ErrorMessages _instance = null;

        ////////////////////////////////////////////////////////////////////////
        /// Properties
        /// 

        public static ErrorMessages Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ErrorMessages();

                    if (_instance == null)
                    {
                        Debug.LogError("no active ErrorMessages object");
                    }
                }

                return _instance;
            }
        }

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. Error : Can not found Singletone Object</para>
        /// <para>Kor. 에러 : 싱글턴 오브젝트를 찾을 수 없습니다. </para>
        /// </summary>
        /// <param name="_className"> <para> Eng. claass name</para>
        ///                         <para> Kor. 클래스 명</para> </param>
        /// <param name="_detail"> <para> Eng. detail information</para>
        ///                         <para> Kor. 상세 정보</para> </param>
        public void MissingSingletoneObject(string _className, string _detail = "")
        {
            Debug.LogError("no active " + _className + " object.\n" + _detail);
        }

        /// <summary>
        /// <para>Eng. Error : Can not found Function in Class</para>
        /// <para>Kor. 에러 : 해당 클래스 내에서 찾을 수 없는 함수입니다.</para>
        /// </summary>
        /// <param name="_className"> <para> Eng. Claass name</para>
        ///                         <para> Kor. 클래스 명</para> </param>
        /// <param name="_funcName"> <para> Eng. Function name</para>
        ///                         <para> Kor. 함수 명</para> </param>
        /// <param name="_detail"> <para> Eng. Detail information</para>
        ///                         <para> Kor. 상세 정보</para> </param>
        public void ErrorFuncInClass(string _className, string _funcName, string _detail = "")
        {
            Debug.LogError("func " + _funcName + " has error in " + _className + ".\n" + _detail);
        }

        /// <summary>
        /// <para>Eng. Error : Can not load Vinove data</para>
        /// <para>Kor. 에러 : 비노비 데이터를 불러올 수 없습니다.. </para>
        /// </summary>
        public void CantLoadVinoveData()
        {
            Debug.LogError("Can't load Vinove, SerializationData is Null.");
        }

        /// <summary>
        /// <para>Eng. Error : Put in wrong Speaker/Background Name. </para>
        /// <para>Kor. 에러 : 잘못된 화자/배경 이름을 입력하였습니다. </para>
        /// </summary>
        public void WrongSpeakerBackgroundNameData()
        {
            Debug.LogWarning("Wrong Speaker/Background name. Speaker/Background name length is need to more then 0 and Speaker/Background name can't be overlapped.");
        }

        /// <summary>
        /// <para>Eng. Error : Don't have selection image. </para>
        /// <para>Kor. 에러 : 선택된 이미지가 없습니다. </para>
        /// </summary>
        public void NoSelectionImage()
        {
            Debug.LogWarning("Wrong selection image. Check the image object field.");
        }

        /// <summary>
        /// <para>Eng. Error : Inputted wrong ranged digit number. </para>
        /// <para>Kor. 에러 : 잘못된 범위의 값을 입력하였습니다. </para>
        /// </summary>
        public void WrongInputRange(int _min, int _max)
        {
            Debug.LogWarning("Inpuuted wrong ranged digit number. You can input number in " + _min + "~" + _max);
        }

        /// <summary>
        /// <para>Eng. Error : Is not readable Texture(need readable set). </para>
        /// <para>Kor. 에러 : 읽을 수 없는 텍스쳐 입니다(Readable 설정 필요). </para>
        /// </summary>
        public void IsNotReadableTexture(string _name)
        {
            Debug.LogWarning("Texture " + _name + " is not readable");
        }

        /// <summary>
        /// <para>Eng. Error : Texture bigger then screen size. </para>
        /// <para>Kor. 에러 : 스크린 사이즈보다 큰 사이즈 입니다. </para>
        /// </summary>
        public void BiggerTextureSize()
        {
            Debug.LogWarning("Texture Y Size has to big than Screen");
        }
    }
}