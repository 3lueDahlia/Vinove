using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViNovE.Framework.Data
{
    [Serializable]
    public class VinoveScene
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables
        /// 

        /////////////////////////////
        /// For Graph Edit
        [SerializeField]
        private Vector2 _translation = new Vector2(0, 0);
        [SerializeField]
        private float _zoomFactor = 1f;

        /////////////////////////////
        /// For Vinove Data

        [SerializeField]
        string _starterUID;

        [SerializeField]
        private Rect _rectSize;  // position on GraphViewer
        [SerializeField]
        private string _UID;        // Unique Identification
        [SerializeField]
        private string _sceneName;       // Scene Name

        [SerializeField]
        private Rect _prevGraphRect;
        [SerializeField]
        private string _prevGraphUID;
        [SerializeField]
        private Rect _nextGraphRect;
        [SerializeField]
        private string _nextGraphUID;

        [SerializeField]
        string _backgroundIllustKey = "";
        [SerializeField]
        AudioClip _backgroundMusic = null;

        //for Vinove + Ingame Datas
        [SerializeField]
        List<VinoveScript> _scripts;

        ////////////////////////////////////////////////////////////////////////
        /// Properties
        /// 

        /////////////////////////////
        /// For Graph Edit
        
        public Vector2 Translation
        {
            get { return _translation; }
            set { _translation = value; }
        }

        public float ZoomFactor
        {
            get { return _zoomFactor; }
            set { _zoomFactor = value; }
        }

        /////////////////////////////
        /// For Vinove Data
        public Rect Position
        {
            get { return _rectSize; }
            set { _rectSize = value; }
        }

        public string UID
        {
            get { return _UID; }
            set { _UID = value; }
        }

        public string Name
        {
            get { return _sceneName; }
            set { _sceneName = value; }
        }

        public string StarterUID
        {
            get { return _starterUID; }
            set { _starterUID = value; }
        }

        public Rect PrevLinkRect
        {
            get { return _prevGraphRect; }
            set { _prevGraphRect = value; }
        }
        public string Next
        {
            get { return _nextGraphUID; }
            set { _nextGraphUID = value; }
        }

        public Rect NextLinkRect
        {
            get { return _nextGraphRect; }
            set { _nextGraphRect = value; }
        }
        public string Prev
        {
            get { return _prevGraphUID; }
            set { _prevGraphUID = value; }
        }

        public string BackgroundIllustKey
        {
            get { return _backgroundIllustKey; }
            set { _backgroundIllustKey = value; }
        }

        public AudioClip BackgroundMusic
        {
            get { return _backgroundMusic; }
            set { _backgroundMusic = value; }
        }

        //for Vinove + Ingame Datas
        public List<VinoveScript> Scripts
        {
            get { return _scripts; }
        }


        ////////////////////////////////////////////////////////////////////////
        /// Func
        /// 

        /// <summary>
        /// <para>Eng. Create Scene class. Manage one conversation </para>
        /// <para>Kor. 씬 클래스를 생성합니다. 여러 개의 대사 집합을 가집니다. </para>
        /// </summary>
        /// <param name="_name"> <para> Eng. Name diaply on graph </para>
        ///                           <para> Kor. 그래프 상 표시될 이름입니다.</para> </param>
        /// <param name="_position"> <para> Eng. Rect for Pos and Scale on graph </para>
        ///                           <para> Kor. 그래프상의 위치, 크기 용 Rect 입니다</para> </param>
        public VinoveScene(string _name, Rect _position)
        {
            _sceneName = _name;
            _rectSize = _position;
            _UID = UIDGenerator.Instance.Generate("SCE");

            _scripts = new List<VinoveScript>();

            SyncLinkerPosition();
        }

        /// <summary>
        /// <para>Eng. Create copied Scene class. Manage one conversation </para>
        /// <para>Kor. 씬 클래스를 복사 생성합니다. 여러 개의 대사 집합을 가집니다. </para>
        /// </summary>
        /// <param name="_originScene"> <para> Eng. Copied origin Scene graph </para>
        ///                           <para> Kor. 복사 주체 씬 그래프입니다.</para> </param>
        /// <param name="_position"> <para> Eng. Rect for Pos and Scale on graph </para>
        ///                           <para> Kor. 그래프상의 위치, 크기 용 Rect 입니다</para> </param>
        public VinoveScene(VinoveScene _originScene, Rect _position)
        {
            _sceneName = _originScene.Name;
            _rectSize = _position;
            _UID = UIDGenerator.Instance.Generate("SCE");

            _translation = _originScene.Translation;
            _zoomFactor = _originScene.ZoomFactor;

            BackgroundIllustKey = _originScene.BackgroundIllustKey;
            BackgroundMusic = _originScene.BackgroundMusic;

            _scripts = new List<VinoveScript>();

            SyncLinkerPosition();
        }

        /// <summary>
        /// <para>Eng. Calculate graph drawing position with translation and zoomfactor </para>
        /// <para>Kor. Translation과 zoomFactor를 기반으로 그래프가 그려질 위치를 계산합니다 </para>
        /// </summary>
        /// <param name="translation"> <para> Eng. Offset current viewer </para>
        ///                           <para> Kor. 현재 뷰어의 오프셋입니다. </para> </param>
        /// <param name="zoomFactor"> <para> Eng. Zoom factor current viewer </para>
        ///                           <para> Kor. 현재 뷰어의 확대 배율입니다. </para> </param>
        public Rect CalculateGraphPosition(Vector2 translation, float zoomFactor)
        {
            Rect _retVal = new Rect(_rectSize);

            _retVal.x = (_retVal.x * zoomFactor) + translation.x;
            _retVal.y = (_retVal.y * zoomFactor) + translation.y;

            _retVal.width *= zoomFactor;
            _retVal.height *= zoomFactor;

            return _retVal;
        }

        /// <summary>
        /// <para>Eng. Change scene graph position </para>
        /// <para>Kor. 씬 그래프의 위치를 변경합니다. </para>
        /// </summary>
        /// <param name="_delta"> <para> Eng. move delta. </para>
        ///                           <para> Kor. 이동량입니다. </para> </param>
        /// <param name="_zoomFactor"> <para> Eng. ZoomFactor of viewer. </para>
        ///                           <para> Kor. 뷰어의 확대 배율입니다. </para> </param>
        public void MoveSceneGraph(Vector2 _delta, float _parentZoomFactor)
        {
            _rectSize.x += _delta.x / _parentZoomFactor;
            _rectSize.y += _delta.y / _parentZoomFactor;
            // Move SceneGraph

            SyncLinkerPosition();
            // Move Linker
        }

        

        /// <summary>
        /// <para>Eng. Find and return script with uid </para>
        /// <para>Kor. UID를 사용하며 스크립트를 찾아 반환합니다. </para>
        /// </summary>
        /// <param name="_uid"> <para> Eng. UID of searching Script</para>
        ///                           <para> Kor. 찾을 스크립트의 UID.</para> </param>
        public VinoveScript FindScriptWithUID(string _uid)
        {
            foreach (VinoveScript _script in _scripts)
            {
                if (_script.UID == _uid)
                {
                    return _script;
                }
            }

            return null;
        }

        /////////////////////////////
        /// Detector

        /// <summary>
        /// <para>Eng. Detecting 'Is mouse cursor are over the graph?' </para>
        /// <para>Kor. 마우스가 현재 그래프 위에 위치 하였는지 판단합니다. </para>
        /// </summary>
        /// <param name="mousePosition"> <para> Eng. Currently mouse position</para>
        ///                           <para> Kor. 현재 마우스 위치입니다. </para> </param>
        public VinoveScript IsContainWholeGraph(Vector2 _mousePosition)
        {
            foreach (VinoveScript _script in _scripts)
            {
                if (_script.CalculatePosition(_translation, _zoomFactor).Contains(_mousePosition))
                {
                    return _script;
                }
            }
            return null;
        }


        /////////////////////////////
        /// Link

        /// <summary>
        /// <para>Eng. Calculate graph linker drawing position with translation and zoomfactor </para>
        /// <para>Kor. Translation과 zoomFactor를 기반으로 그래프 링커가 그려질 위치를 계산합니다 </para>
        /// </summary>
        /// <param name="_whichLinker"> <para> Eng. Link pos </para>
        ///                           <para> Kor. 링크의 위치입니다. </para> </param>
        /// <param name="translation"> <para> Eng. Offset current viewer </para>
        ///                           <para> Kor. 현재 뷰어의 오프셋입니다. </para> </param>
        /// <param name="zoomFactor"> <para> Eng. Zoom factor current viewer </para>
        ///                           <para> Kor. 현재 뷰어의 확대 배율입니다. </para> </param>
        public Rect LinkerPosition(string _whichLinker, Vector2 translation, float zoomFactor)
        {
            Rect _retVal;

            switch (_whichLinker)
            {
                case "Prev":
                    _retVal = PrevLinkRect;
                    break;

                case "Next":
                    _retVal = NextLinkRect;
                    break;

                default:
                    _retVal = default;
                    break;
            }

            _retVal.x = (_retVal.x * zoomFactor) + translation.x;
            _retVal.y = (_retVal.y * zoomFactor) + translation.y;

            _retVal.width *= zoomFactor;
            _retVal.height *= zoomFactor;

            return _retVal;
        }

        /// <summary>
        /// <para>Eng. Calculate graph linker drawing position with translation and zoomfactor </para>
        /// <para>Kor. Translation과 zoomFactor를 기반으로 그래프의 링커가 그려질 위치를 계산합니다 </para>
        /// </summary>
        public void SyncLinkerPosition()
        {
            const float linkRadius = 10f;
            Rect _retVal = new Rect(_rectSize.x, _rectSize.y, linkRadius, linkRadius);

            //-------------------------------

            _prevGraphRect = _retVal;
            _nextGraphRect = _retVal;

            //-------------------------------

            _prevGraphRect.y = _rectSize.y + (_rectSize.height / 2) - (_retVal.height / 2);

            //-------------------------------
            _nextGraphRect.x = _nextGraphRect.x + _rectSize.width - _nextGraphRect.width;

            _nextGraphRect.y = _rectSize.y + (_rectSize.height / 2) - (_retVal.height / 2);
        }
    }
}