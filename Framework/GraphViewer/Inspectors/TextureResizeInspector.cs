#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

using ViNovE.Framework.Error;

namespace ViNovE.Framework
{
    public class TextureResizeInspector : PopupWindowContent
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        private const float _windowSizeWidth = 235;
        private const float _windowSizeHeight = 600;

        Texture2D _addedIllustration = null;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. Rendering and GUI func of Popup Window </para>
        /// <para>Kor. 팝업 윈도우의 렌더링 및 GUI 이벤트 처리 함수입니다. </para>
        /// </summary>
        /// <param name="_popupPos"> <para> Eng. Position of Popup Window</para>
        ///                         <para> Kor. 팝업의 위치입니다. </para> </param>
        public override void OnGUI(Rect _popupPos)
        {
            //---------------------------------------------------------------------

            //GUI.Label(new Rect(10, 10, 410, 20), "Texture Resize Inspector");

            ////---------------------------------------------------------------------

            //_addedIllustration = EditorGUI.ObjectField(new Rect(20, 40, 195, 195), _addedIllustration, typeof(Texture2D), false) as Texture2D;

            //if (GUI.Button(new Rect(20, 245, 195, 20), "Resize"))
            //{
            //    if (_addedIllustration == null)
            //    {
            //        ErrorMessages.Instance.NoSelectionImage();
            //    }
            //    else
            //    {
            //        string _addedIllustPath = AssetDatabase.GetAssetPath(_addedIllustration);

            //        // -------- Resize ---------
            //        Rect _speakerTexRect = new Rect(0, 0, _addedIllustration.width, _addedIllustration.height);
            //        float _resolutionY = Screen.currentResolution.height - ((Screen.currentResolution.height / 10) * 2);
            //        if (_speakerTexRect.height > _resolutionY)
            //        {
            //            _speakerTexRect.width = _speakerTexRect.width - ((_speakerTexRect.height - _resolutionY) / (_speakerTexRect.height / 100)) * (_speakerTexRect.width / 100);
            //            _speakerTexRect.height = _resolutionY;

            //            Texture2D _resizedTexture = GlobalFunc.Instance.ResizeTexture(_addedIllustration, new Vector2(_speakerTexRect.width, _speakerTexRect.height));

            //            if(_resizedTexture != null)
            //            {
            //                byte[] _encodedPNG = _resizedTexture.EncodeToPNG();
            //                string _absolutePath = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + _addedIllustPath.Substring(0, _addedIllustPath.Length - 4) + "_resized.png";
            //                System.IO.File.WriteAllBytes(_absolutePath, _encodedPNG);

            //                _addedIllustration = AssetDatabase.LoadAssetAtPath<Texture2D>(_addedIllustPath);
            //            }
            //        }
            //        // ------------------------
            //    }
            //}
        }

        public override void OnOpen()
        {
        }

        public override void OnClose()
        {

        }

        /// <summary>
        /// <para>Eng. Return size of Popup Window </para>
        /// <para>Kor. 팝업 윈도우의 크기를 반환합니다. </para>
        /// </summary>
        /// <returns>
        /// <para>Eng. Size of Popup Window (Type : Vector2) </para>
        /// <para>Kor. 팝업 윈도우의 크기입니다. (자료형 : Vector2) </para>
        /// </returns>
        public override Vector2 GetWindowSize()
        {
            return new Vector2(_windowSizeWidth, _windowSizeHeight);
        }
    }
}

#endif