using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViNovE.Framework.Data
{
    [Serializable]
    public class Conversation
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        [SerializeField]
        string _speakerName = "";

        [SerializeField]
        string _speakerIllustrationKey = "";
        [SerializeField]
        Enum.AnimationState _speakerIllustrationAnim = (Enum.AnimationState)0;
        [SerializeField]
        Enum.SpeakerPos _speakerIllustrationPos = (Enum.SpeakerPos)0;

        [SerializeField]
        string _dialog = "";

        [SerializeField]
        AudioClip _soundEffect = null;

        ////////////////////////////////////////////////////////////////////////
        /// Properties

        public string Speaker
        {
            get { return _speakerName; }
            set { _speakerName = value; }
        }

        public string SpeakerIllustKey
        {
            get { return _speakerIllustrationKey; }
            set { _speakerIllustrationKey = value; }
        }
        public Enum.AnimationState SpeakerAnim
        {
            get { return _speakerIllustrationAnim; }
            set { _speakerIllustrationAnim = value; }
        }
        public Enum.SpeakerPos SpeakerPos
        {
            get { return _speakerIllustrationPos; }
            set { _speakerIllustrationPos = value; }
        }

        public string Dialog
        {
            get { return _dialog; }
            set { _dialog = value; }
        }

        public AudioClip SoundEffect
        {
            get { return _soundEffect; }
            set { _soundEffect = value; }
        }

        ////////////////////////////////////////////////////////////////////////
        /// Func
        
        public Conversation()
        { }

        public Conversation(Conversation _originConversation)
        {
            _speakerName = _originConversation.Speaker;
            _speakerIllustrationKey = _originConversation.SpeakerIllustKey;

            _speakerIllustrationAnim = _originConversation.SpeakerAnim;
            _speakerIllustrationPos = _originConversation.SpeakerPos;

            _dialog = _originConversation.Dialog;
            _soundEffect = _originConversation.SoundEffect;
        }
    }

    [Serializable]
    public class VinoveScript
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables
        /// 

        //for Vinove Data
        [SerializeField]
        private Rect _rectSize;  // position on ScriptViewer
        [SerializeField]
        private string _UID;        // Unique Identification
        [SerializeField]
        private string _scriptName;       // Script Name

        [SerializeField]
        private string _parentSceneUID;       // Scene Name

        //for Vinove + Ingame Datas
        [SerializeField]
        Conversation _conversation;
         
        //Link
        [SerializeField]
        private Rect _prevGraphRect;
        [SerializeField]
        private string _prevGraphUID;
        [SerializeField]
        private Rect _nextGraphRect;
        [SerializeField]
        private string _nextGraphUID;

        ////////////////////////////////////////////////////////////////////////
        /// Properties
        /// 

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
            get { return _scriptName; }
            set { _scriptName = value; }
        }

        public string ParentSceneUID
        {
            get { return _parentSceneUID; }
            set { _parentSceneUID = value; }
        }

        public Conversation ScriptConversation
        {
            get { return _conversation; }
            set { _conversation = value; }
        }

        // Link
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


        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. Create script class. Manage one conversation </para>
        /// <para>Kor. 스크립트 클래스를 생성합니다. 한 개의 대사를 담당합니다. </para>
        /// </summary>
        /// <param name="_name"> <para> Eng. Name diplay on graph </para>
        ///                           <para> Kor. 그래프 상 표시될 이름입니다.</para> </param>
        /// <param name="_position"> <para> Eng. Rect for Pos and Scale on graph </para>
        ///                           <para> Kor. 그래프상의 위치, 크기 용 Rect 입니다</para> </param>
        public VinoveScript(string _name, Rect _position, string _parentUID)
        {
            _scriptName = _name;
            _rectSize = _position;
            _UID = UIDGenerator.Instance.Generate("SCR");
            _parentSceneUID = _parentUID;

            _conversation = new Conversation();
            SyncLinkerPosition();
        }

        /// <summary>
        /// <para>Eng. Create script class. Manage one conversation </para>
        /// <para>Kor. 스크립트 클래스를 생성합니다. 한 개의 대사를 담당합니다. </para>
        /// </summary>
        /// <param name="_originScript"> <para> Eng. Original script data </para>
        ///                           <para> Kor. 기존 스크립트 정보입니다.</para> </param>
        /// <param name="_parentUID"> <para> Eng. Created Script parent uid </para>
        ///                           <para> Kor. 생성할 스크립트를 지닌 씬 클래스의 UID 입니다. </para> </param>
        /// <param name="_prevLink"> <para> Eng. Prev link of original script data </para>
        ///                           <para> Kor. 기존 데이터가 가지던 이전 링크입니다 </para> </param>
        /// <param name="_nextLink"> <para> Eng. Next link of original script data </para>
        ///                           <para> Kor. 기존 데이터가 가지던 다음 링크입니다. </para> </param>
        public VinoveScript(VinoveScript _originScript, string _parentUID, string _prevLink, string _nextLink)
        {
            _scriptName = _originScript.Name;
            _rectSize = _originScript.Position;
            _UID = _originScript.UID;
            _parentSceneUID = _parentUID;

            _prevGraphUID = _prevLink;
            _nextGraphUID = _nextLink;

            _conversation = new Conversation(_originScript.ScriptConversation);
            SyncLinkerPosition();
        }


        /// <summary>
        /// <para>Eng. Create script class. Manage one conversation </para>
        /// <para>Kor. 스크립트 클래스를 생성합니다. 한 개의 대사를 담당합니다. </para>
        /// </summary>
        /// <param name="_originScript"> <para> Eng. Original script data </para>
        ///                           <para> Kor. 기존 스크립트 정보입니다.</para> </param>
        /// <param name="_parentUID"> <para> Eng. Created Script parent uid </para>
        ///                           <para> Kor. 생성할 스크립트를 지닌 씬 클래스의 UID 입니다. </para> </param>
        public VinoveScript(VinoveScript _originScript, string _parentUID, Rect _position)
        {
            _scriptName = _originScript.Name;
            _rectSize = _position;
            _UID = UIDGenerator.Instance.Generate("SCR");
            _parentSceneUID = _parentUID;

            _conversation = new Conversation(_originScript.ScriptConversation);

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
        public Rect CalculatePosition(Vector2 translation, float zoomFactor)
        {
            Rect _retVal = new Rect(_rectSize);

            _retVal.x = (_retVal.x * zoomFactor) + translation.x;
            _retVal.y = (_retVal.y * zoomFactor) + translation.y;

            _retVal.width *= zoomFactor;
            _retVal.height *= zoomFactor;

            return _retVal;
        }

        /// <summary>
        /// <para>Eng. Change script graph position </para>
        /// <para>Kor. 스크립트 그래프의 위치를 변경합니다. </para>
        /// </summary>
        /// <param name="_delta"> <para> Eng. move delta. </para>
        ///                           <para> Kor. 이동량입니다. </para> </param>
        public void MoveScriptGraph(Vector2 _delta)
        {
            _rectSize.x += _delta.x;
            _rectSize.y += _delta.y;

            SyncLinkerPosition();
        }

        /// <summary>
        /// <para>Eng. Change script graph position </para>
        /// <para>Kor. 스크립트 그래프의 위치를 변경합니다. </para>
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

        /////////////////////////////
        /// Detector

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