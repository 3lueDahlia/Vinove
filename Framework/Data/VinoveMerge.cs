using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ViNovE.Framework.Error;

namespace ViNovE.Framework.Data
{
    [Serializable]
    public class VinoveMerge
    {

        ////////////////////////////////////////////////////////////////////////
        /// Variables
        
        /////////////////////////////
        /// For Vinove Data

        //for Vinove Data
        [SerializeField]
        private Rect _rectSize;  // position on SceneViewer
        [SerializeField]
        private string _UID;        // Unique Identification
        [SerializeField]
        private string _mergerName;       // Scene Name



        /////////////////////////////
        /// Link

        [SerializeField]
        private Rect[] _mergeGraphRect;
        [SerializeField]
        private string[] _mergeGraphUID;

        //for Vinove + Ingame Datas
        [SerializeField]
        private Rect _nextGraphRect;
        [SerializeField]
        private string _nextGraphUID;

        [SerializeField]
        int _mergeCount = 1;

        int _selectedMerge;

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
            get { return _mergerName; }
            set { _mergerName = value; }
        }

        // ------------------------ Link ------------------------
        public string Next
        {
            get { return _nextGraphUID; }
            set { _nextGraphUID = value; }
        }
        public Rect NextGraphRect
        {
            get { return _nextGraphRect; }
            set { _nextGraphRect = value; }
        }

        public Rect[] MergeRects
        {
            get { return _mergeGraphRect; }
        }
        public string[] MergeUIDs
        {
            get { return _mergeGraphUID; }
        }
        public int MergeCount
        {
            get { return _mergeCount; }
        }

        ////////////////////////////////////////////////////////////////////////
        /// Func
        /// 

        /// <summary>
        /// <para>Eng. Create Merge class. Scene's next to same one scene. </para>
        /// <para>Kor. 병합기 클래스를 생성합니다. 여러 개의 씬의 다음 대상을 하나의 씬으로 만듭니다. </para>
        /// </summary>
        /// <param name="_name"> <para> Eng. Name diaply on graph </para>
        ///                           <para> Kor. 그래프 상 표시될 이름입니다.</para> </param>
        /// <param name="_position"> <para> Eng. Rect for Pos and Scale on graph </para>
        ///                           <para> Kor. 그래프상의 위치, 크기 용 Rect 입니다</para> </param>
        public VinoveMerge(string _name, Rect _position)
        {
            _mergerName = _name;
            _rectSize = _position;
            _UID = UIDGenerator.Instance.Generate("MRG");

            _nextGraphRect = new Rect();

            _mergeCount = 2;
            _mergeGraphRect = new Rect[_mergeCount];
            _mergeGraphUID = new string[_mergeCount];

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
        public Rect CalculateMergePosition(Vector2 translation, float zoomFactor)
        {
            Rect _retVal = new Rect(_rectSize);

            _retVal.x = (_retVal.x * zoomFactor) + translation.x;
            _retVal.y = (_retVal.y * zoomFactor) + translation.y;

            _retVal.width *= zoomFactor;
            _retVal.height *= zoomFactor;

            return _retVal;
        }

        /// <summary>
        /// <para>Eng. Change Merge graph position </para>
        /// <para>Kor. 분기 그래프의 위치를 변경합니다. </para>
        /// </summary>
        /// <param name="_delta"> <para> Eng. move delta. </para>
        ///                           <para> Kor. 이동량입니다. </para> </param>
        public void MoveMergeGraph(Vector2 _delta, float _parentZoomFactor)
        {
            _rectSize.x += _delta.x / _parentZoomFactor;
            _rectSize.y += _delta.y / _parentZoomFactor;
            // Move SceneGraph

            SyncLinkerPosition();
            // Move Linker
        }

        /// <summary>
        /// <para>Eng. Change Merge count on VinoveMerge </para>
        /// <para>Kor. 그래프 분기점의 분기 개수를 변경합니다 </para>
        /// </summary>
        /// <param name="_changedCount"> <para> Eng. Wanna change count to this num </para>
        ///                           <para> Kor. 이 값으로 변경하겠습니다. </para> </param>
        /// <param name="_min"> <para> Eng. Minimum value of change target var </para>
        ///                           <para> Kor. 변경 대상 값의 최소값. </para> </param>
        /// <param name="_max"> <para> Eng. Maximum value of change target var </para>
        ///                           <para> Kor. 변경 대상의 최대값. </para> </param>
        public void ChangeMergeCount(Vinove _currentVinove, int _changedCount, int _min, int _max)
        {
            int _mergeCountTmp = _mergeCount;
            _mergeCount = _changedCount;

            if (_changedCount < _min || _changedCount > _max)
            {
                ErrorMessages.Instance.WrongInputRange(_min, _max);
            }
            else if (_mergeCountTmp == _changedCount)
            {   // if, same. no need to change
                return;
            }

            string[] _uidTmp = new string[_mergeCountTmp];
            Rect[] _rectTmp = new Rect[_mergeCountTmp];

            for (int i = 0; i < _mergeCountTmp; i++)
            {
                _uidTmp[i] = _mergeGraphUID[i];
                _rectTmp[i] = _mergeGraphRect[i];
            }

            _mergeGraphUID = new string[_changedCount];
            _mergeGraphRect = new Rect[_changedCount];

            if (_mergeCountTmp < _changedCount)
            {
                for (int i = 0; i < _mergeCountTmp; i++)
                {
                    _mergeGraphUID[i] = _uidTmp[i];
                    _mergeGraphRect[i] = _rectTmp[i];
                }
            }
            else
            {
                int _repeatCount;
                for (_repeatCount = 0; _repeatCount < _changedCount; _repeatCount++)
                {
                    _mergeGraphUID[_repeatCount] = _uidTmp[_repeatCount];
                    _mergeGraphRect[_repeatCount] = _rectTmp[_repeatCount];
                }

                for (; _repeatCount < _mergeCountTmp; _repeatCount++)
                {
                    if (_uidTmp[_repeatCount].StartsWith("MRG"))
                    {
                        VinoveMerge _nextMerge = _currentVinove.FindMergeWithUID(_uidTmp[_repeatCount]);
                        _nextMerge.Next = null;
                    }
                    else if (_uidTmp[_repeatCount].StartsWith("BRA"))
                    {
                        VinoveBranch _nextBranch = _currentVinove.FindBranchWithUID(_uidTmp[_repeatCount]);
                        _nextBranch.BranchsUIDs[_nextBranch.FindUIDLinkIndex(UID)] = null;
                    }
                    else if (_uidTmp[_repeatCount].StartsWith("SCE"))
                    {
                        VinoveScene _nextScene = _currentVinove.FindSceneWithUID(_uidTmp[_repeatCount]);
                        _nextScene.Next = null;
                    }
                }
            }

            SyncLinkerPosition();
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
                    _retVal = _mergeGraphRect[_linkIndex];
                    break;

                case "Next":
                    _retVal = _nextGraphRect;
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
        /// <para>Eng. Link Merge linker to other scene </para>
        /// <para>Kor. 그래프 병합기의 병합 링커를 다른 씬 혹은 분기와 이어줍니다. </para>
        /// </summary>
        /// <param name="_addedUID"> <para> Eng. UID of link target </para>
        ///                           <para> Kor. 링크 대상의 UID. </para> </param>
        /// <param name="_mergeIndex"> <para> Eng. Index of Merge </para>
        ///                           <para> Kor. 변경 대상 분기의 인자. </para> </param>
        public void AddMergeNext(string _addedUID, int _mergeIndex)
        {
            _mergeGraphUID[_mergeIndex] = _addedUID;
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

            _nextGraphRect = _rectTmp;

            //-------------------------------
            float _linkGapY = _rectSize.height / (_mergeCount + 1);
            float _linkPosY = _linkGapY;

            for (int i = 0; i < _mergeCount; i++)
            {
                _mergeGraphRect[i] = _rectTmp;

                _mergeGraphRect[i].y = _rectSize.y + _linkPosY;

                _linkPosY += _linkGapY;
            }

            //-------------------------------

            _nextGraphRect.x = _nextGraphRect.x + _rectSize.width - _nextGraphRect.width;
            _nextGraphRect.y = _rectSize.y + (_rectSize.height / 2) - (_rectTmp.height / 2);

        }

        /// <summary>
        /// <para>Eng. Delete parameter UID </para>
        /// <para>Kor. 인자로 받은 UID를 찾아 제거합니다. </para>
        /// </summary>
        public void DeletePrevUID(string _uid)
        {
            for (int i = 0; i < _mergeCount; i++)
            {
                if (_mergeGraphUID[i] == _uid)
                {
                    _mergeGraphUID[i] = null;
                }
            }
        }

        /// <summary>
        /// <para>Eng. searching linker pos with uid and return. </para>
        /// <para>Kor. 인자로 받은 UID를 몇 번째 링커에 등록되어 있는지 검색 후 반환합니다. </para>
        /// </summary>
        public int FindUIDLinkIndex(string _uid)
        {
            for (int i = 0; i < _mergeCount; i++)
            {
                if (_mergeGraphUID[i] == _uid)
                {
                    return i;
                }
            }

            return default;
        }
    }
}