using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ViNovE.Framework.Error;

namespace ViNovE.Framework.Data
{
    [Serializable]
    public class VinoveBranch
    {
        /////////////////////////////
        /// For Vinove Data
        
        //for Vinove Data
        [SerializeField]
        private Rect _rectSize;  // position on SceneViewer
        [SerializeField]
        private string _UID;        // Unique Identification
        [SerializeField]
        private string _branchName;       // Scene Name

        //for Vinove + Ingame Datas
        [SerializeField]
        private Rect _prevGraphRect;
        [SerializeField]
        private string _prevGraphUID;

        [SerializeField]
        private Rect [] _branchGraphRect;
        [SerializeField]
        private string[] _branchGraphUID;
        [SerializeField]
        private string[] _branchAlternatives;

        [SerializeField]
        int _branchCount = 1;

        int _selectedBranch;

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
            get { return _branchName; }
            set { _branchName = value; }
        }

        // ------------------------ Link ------------------------
        public string Prev
        {
            get { return _prevGraphUID; }
            set { _prevGraphUID = value; }
        }
        public Rect PrevGraphRect
        {
            get { return _prevGraphRect; }
            set { _prevGraphRect = value; }
        }

        public Rect[] BranchsRects
        {
            get { return _branchGraphRect; }
        }
        public string[] BranchsUIDs
        {
            get { return _branchGraphUID; }
        }
        public string[] BranchsAlternatives
        {
            get { return _branchAlternatives; }
        }
        public int BranchCount
        {
            get { return _branchCount; }
        }

        ////////////////////////////////////////////////////////////////////////
        /// Func
        /// 

        /// <summary>
        /// <para>Eng. Create Branch class. Make multiple-choice and distribute each scene </para>
        /// <para>Kor. 분배기 클래스를 생성합니다. 선택지를 만들어, 각 선택지에 맞는 씬을 분배합니다. </para>
        /// </summary>
        /// <param name="_name"> <para> Eng. Name diaply on graph </para>
        ///                           <para> Kor. 그래프 상 표시될 이름입니다.</para> </param>
        /// <param name="_position"> <para> Eng. Rect for Pos and Scale on graph </para>
        ///                           <para> Kor. 그래프상의 위치, 크기 용 Rect 입니다</para> </param>
        public VinoveBranch(string _name, Rect _position)
        {
            _branchName = _name;
            _rectSize = _position;
            _UID = UIDGenerator.Instance.Generate("BRA");

            _prevGraphRect = new Rect();

            _branchCount = 2;
            _branchGraphRect = new Rect[_branchCount];
            _branchGraphUID = new string[_branchCount];
            _branchAlternatives = new string[_branchCount];
            for(int i=0; i<_branchCount; i++)
            {
                BranchsAlternatives[i] = "";
            }

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
        public Rect CalculateBranchPosition(Vector2 translation, float zoomFactor)
        {
            Rect _retVal = new Rect(_rectSize);

            _retVal.x = (_retVal.x * zoomFactor) + translation.x;
            _retVal.y = (_retVal.y * zoomFactor) + translation.y;

            _retVal.width *= zoomFactor;
            _retVal.height *= zoomFactor;

            return _retVal;
        }

        /// <summary>
        /// <para>Eng. Change branch graph position </para>
        /// <para>Kor. 분기 그래프의 위치를 변경합니다. </para>
        /// </summary>
        /// <param name="_delta"> <para> Eng. move delta. </para>
        ///                           <para> Kor. 이동량입니다. </para> </param>
        public void MoveBranchGraph(Vector2 _delta, float _parentZoomFactor)
        {
            _rectSize.x += _delta.x / _parentZoomFactor;
            _rectSize.y += _delta.y / _parentZoomFactor;
            // Move SceneGraph

            SyncLinkerPosition();
            // Move Linker
        }

        /// <summary>
        /// <para>Eng. Change branch count on VinoveBranch </para>
        /// <para>Kor. 그래프 분기점의 분기 개수를 변경합니다 </para>
        /// </summary>
        /// <param name="_changedCount"> <para> Eng. Wanna change count to this num </para>
        ///                           <para> Kor. 이 값으로 변경하겠습니다. </para> </param>
        /// <param name="_min"> <para> Eng. Minimum value of change target var </para>
        ///                           <para> Kor. 변경 대상 값의 최소값. </para> </param>
        /// <param name="_max"> <para> Eng. Maximum value of change target var </para>
        ///                           <para> Kor. 변경 대상의 최대값. </para> </param>
        public void ChangeBranchCount(Vinove _currentVinove, int _changedCount, int _min, int _max)
        {
            int _branchCountTmp = _branchCount;
            _branchCount = _changedCount;

            if (_changedCount < _min || _changedCount > _max)
            {
                ErrorMessages.Instance.WrongInputRange(_min, _max);
            }
            else if(_branchCountTmp == _changedCount)
            {   // if, same. no need to change
                return;
            }

            string[] _AlterTmp = new string[_branchCountTmp];
            string[] _uidTmp = new string[_branchCountTmp];
            Rect[] _rectTmp = new Rect[_branchCountTmp];

            for (int i = 0; i < _branchCountTmp; i++)
            {   //back up link data before change
                _AlterTmp[i] = _branchAlternatives[i];
                _uidTmp[i] = _branchGraphUID[i];
                _rectTmp[i] = _branchGraphRect[i];
            }

            _branchAlternatives = new string[_changedCount];
            _branchGraphUID = new string[_changedCount];
            _branchGraphRect = new Rect[_changedCount];
            //re-delegate branch datas

            for (int i = 0; i < _changedCount; i++)
            {
                BranchsAlternatives[i] = "";    // init Alternatives
            }

            if (_branchCountTmp < _changedCount)
            {
                for(int i=0; i< _branchCountTmp; i++)
                {
                    _branchAlternatives[i] = _AlterTmp[i];
                    _branchGraphUID[i] = _uidTmp[i];
                    _branchGraphRect[i] = _rectTmp[i];
                }
            }
            else
            {
                int _repeatCount = 0;
                for (_repeatCount = 0; _repeatCount < _changedCount; _repeatCount++)
                {
                    _branchAlternatives[_repeatCount] = _AlterTmp[_repeatCount];
                    _branchGraphUID[_repeatCount] = _uidTmp[_repeatCount];
                    _branchGraphRect[_repeatCount] = _rectTmp[_repeatCount];
                }

                for(; _repeatCount < _branchCountTmp; _repeatCount++)
                {
                    if (_uidTmp[_repeatCount].StartsWith("MRG"))
                    {
                        VinoveMerge _nextMerge = _currentVinove.FindMergeWithUID(_uidTmp[_repeatCount]);
                        _nextMerge.MergeUIDs[_nextMerge.FindUIDLinkIndex(UID)] = null;
                    }
                    else if (_uidTmp[_repeatCount].StartsWith("BRA"))
                    {
                        VinoveBranch _nextBranch = _currentVinove.FindBranchWithUID(_uidTmp[_repeatCount]);
                        _nextBranch.Prev = null;
                    }
                    else if (_uidTmp[_repeatCount].StartsWith("SCE"))
                    {
                        VinoveScene _nextScene = _currentVinove.FindSceneWithUID(_uidTmp[_repeatCount]);
                        _nextScene.Prev = null;
                    }
                }
            }

            SyncLinkerPosition();
        }

        public void ChangeAlternative(string _alternative, int _index)
        {
            _branchAlternatives[_index] = _alternative;
        }
        public string GetAlternativeWithIndex(int _index)
        {
            return _branchAlternatives[_index];
        }

        public bool IsEmptyAllNextLink()
        {
            for(int i=0; i<_branchCount; i++)
            {
                if(ErrorDetector.Instance.IsStringHasData(_branchGraphUID[i]))
                {
                    return false;
                }
            }

            return true;
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
        public Rect LinkerPosition(string _whichLinker, Vector2 translation, float zoomFactor, int _linkIndex = 0)
        {
            Rect _retVal;

            switch (_whichLinker)
            {
                case "Prev":
                    _retVal = _prevGraphRect;
                    break;

                case "Next":
                    _retVal = _branchGraphRect[_linkIndex];
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
        /// <para>Eng. Link branch linker to other scene </para>
        /// <para>Kor. 그래프 분기점의 분기 링커를 다른 씬 혹은 분기와 이어줍니다. </para>
        /// </summary>
        /// <param name="_addedUID"> <para> Eng. UID of link target </para>
        ///                           <para> Kor. 링크 대상의 UID. </para> </param>
        /// <param name="_branchIndex"> <para> Eng. Index of branch </para>
        ///                           <para> Kor. 변경 대상 분기의 인자. </para> </param>
        public void AddBranchNext(string _addedUID, int _branchIndex)
        {
            _branchGraphUID[_branchIndex] = _addedUID;
        }

        /// <summary>
        /// <para>Eng. Sync Graph linker pos with graph pos </para>
        /// <para>Kor. 그래프의 링커 위치를 그래프 위치를 토대로 다시 계산합니다. </para>
        /// </summary>
        public void SyncLinkerPosition()
        {
            const float linkRadius = 10f;
            Rect _rectTmp = new Rect(_rectSize.x, _rectSize.y, linkRadius, linkRadius);

            //-------------------------------

            _prevGraphRect = _rectTmp;
            

            //-------------------------------

            _prevGraphRect.y = _rectSize.y + (_rectSize.height / 2) - (_rectTmp.height / 2);

            //-------------------------------
            float _linkGapY = _rectSize.height / (BranchCount + 1);
            float _linkPosY = _linkGapY;

            for (int i=0; i<BranchCount; i++)
            {
                _branchGraphRect[i] = _rectTmp;

                _branchGraphRect[i].x = _branchGraphRect[i].x + _rectSize.width - _branchGraphRect[i].width;
                _branchGraphRect[i].y = _rectSize.y + _linkPosY;

                _linkPosY += _linkGapY;
            }
        }

        /// <summary>
        /// <para>Eng. Delete parameter UID </para>
        /// <para>Kor. 인자로 받은 UID를 찾아 제거합니다. </para>
        /// </summary>
        public void DeleteNextUID(string _uid)
        {
            for(int i=0; i<_branchCount; i++)
            {
                if(_branchGraphUID[i] == _uid)
                {
                    _branchGraphUID[i] = null;
                }
            }
        }

        /// <summary>
        /// <para>Eng. searching linker pos with uid and return. </para>
        /// <para>Kor. 인자로 받은 UID를 몇 번째 링커에 등록되어 있는지 검색 후 반환합니다. </para>
        /// </summary>
        public int FindUIDLinkIndex(string _uid)
        {
            for (int i = 0; i < _branchCount; i++)
            {
                if (_branchGraphUID[i] == _uid)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
