using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using ViNovE.Framework.GraphViewer;
#endif
using ViNovE.Framework.Error;
using ViNovE.Framework.Data.Dictionary;
using ViNovE.Framework.Data.UI;

namespace ViNovE.Framework.Data
{
    public class Vinove : ScriptableObject
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables
        /// 

        [SerializeField, ReadOnly]
        string _fileDir;
        [SerializeField, ReadOnly]
        string _fileName;

        /////////////////////////////
        /// For Graph Edit
        [SerializeField, ReadOnly]
        private Vector2 _translation = new Vector2(0, 0);
        [SerializeField, ReadOnly]
        private float _zoomFactor = 1f;

        /////////////////////////////
        /// For Graph Data
        [SerializeField, ReadOnly]
        List<string> _uids;         // UID Intergration Storage in 1 Vinove

        [SerializeField, ReadOnly]
        string _starterUID;

        [SerializeField, ReadOnly]
        List<VinoveScene> _scenes;   // Scenes list in Vinove
        [SerializeField, ReadOnly]
        List<VinoveBranch> _branchs;   // Branchs list in Vinove
        [SerializeField, ReadOnly]
        List<VinoveMerge> _merges;   // Merges list in Vinove

        /////////////////////////////
        /// For Game Data

        [SerializeField, ReadOnly]
        List<string> _speakerNames;
        [SerializeField, ReadOnly]
        VinoveDictionary _speakerIllustrations;
        [SerializeField, ReadOnly]
        VinoveDictionary _backgroundIllustrations;

        ////////////////////////////////////////////////////////////////////////
        /// Properties
        /// 

        public string FileDir
        {
            get { return _fileDir ; }
            set { _fileDir = value; }
        }
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

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
        /// For Graph Data

        public List<string> UIDs
        {
            get { return _uids; }
        }
        public string StarterUID
        {
            get { return _starterUID; }
            set { _starterUID = value; }
        }

        public List<VinoveScene> Scene
        {
            get { return _scenes; }
        }
        public List<VinoveBranch> Branchs
        {
            get { return _branchs; }
        }
        public List<VinoveMerge> Merges
        {
            get { return _merges; }
        }

        /////////////////////////////
        /// For Game Data
        
        public List<string> SpeakerNames
        {
            get { return _speakerNames; }
        }
        public VinoveDictionary SpeakerIllustrations
        {
            get { return _speakerIllustrations; }
        }
        public VinoveDictionary BackgroundIllustrations
        {
            get { return _backgroundIllustrations; }
        }

        ////////////////////////////////////////////////////////////////////////
        /// Func
        ///

        /////////////////////////////
        /// Initialize
        public Vinove()
        {
            _scenes = new List<VinoveScene>();
            _branchs = new List<VinoveBranch>();
            _merges = new List<VinoveMerge>();

            _uids = new List<string>();

            _speakerNames = new List<string>();
            _speakerIllustrations = new VinoveDictionary();
            _backgroundIllustrations = new VinoveDictionary();
        }

        /////////////////////////////
        /// Graph

#if UNITY_EDITOR
        /// <summary>
        /// <para>Eng. Create scene class. Manage one conversation </para>
        /// <para>Kor. 씬 클래스를 생성합니다. 한 개의 대화를 담당합니다. </para>
        /// </summary>
        /// <param name="_name"> <para> Eng. Want add link in this node </para>
        ///                           <para> Kor. 링크를 추가할 노드</para> </param>
        /// <param name="_position"> <para> Eng. Rect for Pos and Scale on graph </para>
        ///                           <para> Kor. 그래프상의 위치, 크기 용 Rect 입니다</para> </param>
        public void CreateScene(string _name, Rect _position)
        {
            Debug.Log("Created Scene Graph in : " + _position);
            VinoveScene CreatedScene = new VinoveScene(_name, _position);

            _uids.Add(CreatedScene.UID);
            _scenes.Add(CreatedScene);

            if (ErrorDetector.Instance.IsStringEmpty(StarterUID))
            {
                StarterUID = CreatedScene.UID;
            }

            GraphViewer.GraphViewer.UndoStackCreate(CreatedScene.UID, CreatedScene);
        }

        /// <summary>
        /// <para>Eng. Create scene with copied scene. </para>
        /// <para>Kor. 씬 그래프를 복사해서 새로운 씬을 생성합니다. </para>
        /// </summary>
        /// <param name="_copiedScene"> <para> Eng. Origin copied graph </para>
        ///                           <para> Kor. 복사할 씬 그래프. </para> </param>
        public void PasteScene(VinoveScene _copiedScene, Rect _position)
        {
            VinoveScene CreatedScene = new VinoveScene(_copiedScene, _position);

            _uids.Add(CreatedScene.UID);
            _scenes.Add(CreatedScene);

            if (ErrorDetector.Instance.IsStringEmpty(StarterUID))
            {
                StarterUID = CreatedScene.UID;
            }

            for(int i=0; i<_copiedScene.Scripts.Count; i++)
            {
                CreateScript(CreatedScene, _copiedScene.Scripts[i]);
            }

            foreach (VinoveScript _script in CreatedScene.Scripts)
            {
                string _instancedUID = UIDGenerator.Instance.Generate("SCR");
                if (_copiedScene.StarterUID == _script.UID)
                {
                    CreatedScene.StarterUID = _instancedUID;
                }

                if (ErrorDetector.Instance.IsStringHasData(_script.Next))
                {   // it has next link
                    VinoveScript _nextScript = CreatedScene.FindScriptWithUID(_script.Next);

                    if (!ErrorDetector.Instance.IsStringHasData(_script.Prev))
                    {   // it has prev link, already had new uid
                        _script.UID = _instancedUID;
                        _uids.Add(_instancedUID);
                    }

                    if(_copiedScene.StarterUID == _nextScript.UID)
                    {
                        _nextScript.UID = UIDGenerator.Instance.Generate("SCR");
                        CreatedScene.StarterUID = _nextScript.UID;
                        _uids.Add(_nextScript.UID);
                    }
                    else
                    {
                        _nextScript.UID = UIDGenerator.Instance.Generate("SCR");
                        _uids.Add(_nextScript.UID);
                    }

                    _script.Next = _nextScript.UID;
                    _nextScript.Prev = _script.UID;
                }
                else
                {   // non-next link
                    if (!ErrorDetector.Instance.IsStringHasData(_script.Prev))
                    {   // it has prev link, already had new uid
                        _script.UID = _instancedUID;
                        _uids.Add(_instancedUID);
                    }
                }
            }
        }

        /// <summary>
        /// <para>Eng. Create Branch class. Manage one branch </para>
        /// <para>Kor. 분기점 클래스를 생성합니다. 한 개의 분기를 담당합니다. </para>
        /// </summary>
        /// <param name="_name"> <para> Eng. Want add link in this node </para>
        ///                           <para> Kor. 링크를 추가할 노드</para> </param>
        /// <param name="_position"> <para> Eng. Rect for Pos and Scale on graph </para>
        ///                           <para> Kor. 그래프상의 위치, 크기 용 Rect 입니다</para> </param>
        public void CreateBranch(string _name, Rect _position)
        {
            
            Debug.Log("Created Scene Branch in : " + _position);
            VinoveBranch CreatedBranch = new VinoveBranch(_name, _position);
            _uids.Add(CreatedBranch.UID);
            _branchs.Add(CreatedBranch);

            GraphViewer.GraphViewer.UndoStackCreate(CreatedBranch.UID, CreatedBranch);
        }

        /// <summary>
        /// <para>Eng. Create Merge class. Manage merge of scene or branch </para>
        /// <para>Kor. 병합기 클래스를 생성합니다. 씬, 분기의 병합을 담당합니다. </para>
        /// </summary>
        /// <param name="_name"> <para> Eng. Want add link in this node </para>
        ///                           <para> Kor. 링크를 추가할 노드</para> </param>
        /// <param name="_position"> <para> Eng. Rect for Pos and Scale on graph </para>
        ///                           <para> Kor. 그래프상의 위치, 크기 용 Rect 입니다</para> </param>
        public void CreateMerge(string _name, Rect _position)
        {
            Debug.Log("Created Scene Merge in : " + _position);
            VinoveMerge CreatedMerge = new VinoveMerge(_name, _position);
            _uids.Add(CreatedMerge.UID);
            _merges.Add(CreatedMerge);

            GraphViewer.GraphViewer.UndoStackCreate(CreatedMerge.UID, CreatedMerge);
        }

        /// <summary>
        /// <para>Eng. Create script class. Manage one dialogue </para>
        /// <para>Kor. 스크립트 클래스를 생성합니다. 한 개의 대사를 담당합니다. </para>
        /// </summary>
        /// <param name="_scene"> <para> Eng. Scene for add script</para>
        ///                           <para> Kor. 스크립트를 추가 대상인 씬입니다.</para> </param>
        /// <param name="_name"> <para> Eng. Name display on graph </para>
        ///                           <para> Kor. 그래프 상 표시될 이름입니다.</para> </param>
        /// <param name="_position"> <para> Eng. Rect for Pos and Scale on graph </para>
        ///                           <para> Kor. 그래프상의 위치, 크기 용 Rect 입니다</para> </param>
        public void CreateScript(VinoveScene _scene, string _name, Rect _position)
        {
            Debug.Log("Created Script Graph in : " + _position);
            VinoveScript CreatedScript = new VinoveScript(_name, _position, _scene.UID);

            _uids.Add(CreatedScript.UID);
            _scene.Scripts.Add(CreatedScript);

            if (ErrorDetector.Instance.IsStringEmpty(_scene.StarterUID))
            {
                _scene.StarterUID = CreatedScript.UID;
            }

            GraphViewer.GraphViewer.UndoStackCreate(CreatedScript.UID, CreatedScript);
        }

        /// <summary>
        /// <para>Eng. Create script class with Copy/Paste(Only for Scene Copy). Manage one dialogue </para>
        /// <para>Kor. 복사/붙여넣기로 스크립트 클래스를 생성합니다(씬 복사 전용). 한 개의 대사를 담당합니다. </para>
        /// </summary>
        /// <param name="_scene"> <para> Eng. Scene for add script</para>
        ///                           <para> Kor. 스크립트를 추가 대상인 씬입니다.</para> </param>
        /// <param name="_originScript"> <para> Eng. Origin script data </para>
        ///                           <para> Kor. 기존 스크립트 정보입니다.</para> </param>
        public void CreateScript(VinoveScene _scene, VinoveScript _originScript)
        {
            //Debug.Log("Created Script Graph in : " + _originScript.Position);
            VinoveScript CreatedScript = new VinoveScript(_originScript, _scene.UID, _originScript.Prev, _originScript.Next);

            //_uids.Add(CreatedScript.UID);
            _scene.Scripts.Add(CreatedScript);
        }

        /// <summary>
        /// <para>Eng. Create script class with Copy/Paste. Manage one dialogue </para>
        /// <para>Kor. 복사/붙여넣기로 스크립트 클래스를 생성합니다. 한 개의 대사를 담당합니다. </para>
        /// </summary>
        /// <param name="_scene"> <para> Eng. Scene for add script</para>
        ///                           <para> Kor. 스크립트를 추가 대상인 씬입니다.</para> </param>
        /// <param name="_originScript"> <para> Eng. Origin script data </para>
        ///                           <para> Kor. 기존 스크립트 정보입니다.</para> </param>
        /// <param name="_position"> <para> Eng. Rect for Pos and Scale on graph </para>
        ///                           <para> Kor. 그래프상의 위치, 크기 용 Rect 입니다</para> </param>
        public void PasteScript(VinoveScene _scene, VinoveScript _originScript, Rect _position)
        {
            Debug.Log("Pasted Script Graph in : " + _position);
            VinoveScript CreatedScript = new VinoveScript(_originScript, _scene.UID, _position);

            _uids.Add(CreatedScript.UID);
            _scene.Scripts.Add(CreatedScript);

            if (ErrorDetector.Instance.IsStringEmpty(_scene.StarterUID))
            {
                _scene.StarterUID = CreatedScript.UID;
            }
        }


        public void RestoreGraph(VinoveScene _targetScene)
        {
            if(ErrorDetector.Instance.IsStringHasData(_targetScene.Prev))
            {
                if (_targetScene.Prev.StartsWith("SCE"))
                {
                    VinoveScene _prevScene = FindSceneWithUID(_targetScene.Prev);
                    _prevScene.Next = _targetScene.UID;
                }
                else if (_targetScene.Prev.StartsWith("BRA"))
                {
                    VinoveBranch _prevBranch = FindBranchWithUID(_targetScene.Prev);
                }
                if (_targetScene.Prev.StartsWith("MRG"))
                {
                    VinoveMerge _prevMerge = FindMergeWithUID(_targetScene.Prev);
                    _prevMerge.Next = _targetScene.UID;
                }
            }

            if (ErrorDetector.Instance.IsStringHasData(_targetScene.Next))
            {
                if (_targetScene.Next.StartsWith("SCE"))
                {
                    VinoveScene _nextScene = FindSceneWithUID(_targetScene.Next);
                    _nextScene.Prev = _targetScene.UID;
                }
                else if (_targetScene.Next.StartsWith("BRA"))
                {
                    VinoveBranch _nextBranch = FindBranchWithUID(_targetScene.Next);
                    _nextBranch.Prev = _targetScene.UID;
                }
                if (_targetScene.Next.StartsWith("MRG"))
                {
                    VinoveMerge _nextMerge = FindMergeWithUID(_targetScene.Next);
                }
            }

            _scenes.Add(_targetScene);
            _uids.Add(_targetScene.UID);

            if (ErrorDetector.Instance.IsStringEmpty(StarterUID))
            {
                StarterUID = _targetScene.UID;
            }
        }

        public void RestoreGraph(VinoveBranch _targetBranch)
        {
            if (ErrorDetector.Instance.IsStringHasData(_targetBranch.Prev))
            {
                if (_targetBranch.Prev.StartsWith("SCE"))
                {
                    VinoveScene _prevScene = FindSceneWithUID(_targetBranch.Prev);
                    _prevScene.Next = _targetBranch.UID;
                }
                else if (_targetBranch.Prev.StartsWith("BRA"))
                {
                    VinoveBranch _prevBranch = FindBranchWithUID(_targetBranch.Prev);
                }
                if (_targetBranch.Prev.StartsWith("MRG"))
                {
                    VinoveMerge _prevMerge = FindMergeWithUID(_targetBranch.Prev);
                    _prevMerge.Next = _targetBranch.UID;
                }
            }

            foreach(string _branchNextUID in _targetBranch.BranchsUIDs)
            {
                if (ErrorDetector.Instance.IsStringHasData(_branchNextUID))
                {
                    if (_branchNextUID.StartsWith("SCE"))
                    {
                        VinoveScene _nextScene = FindSceneWithUID(_branchNextUID);
                        _nextScene.Prev = _targetBranch.UID;
                    }
                    else if (_branchNextUID.StartsWith("BRA"))
                    {
                        VinoveBranch _nextBranch = FindBranchWithUID(_branchNextUID);
                        _nextBranch.Prev = _targetBranch.UID;
                    }
                    if (_branchNextUID.StartsWith("MRG"))
                    {
                        VinoveMerge _nextMerge = FindMergeWithUID(_branchNextUID);
                    }
                }
            }

            _branchs.Add(_targetBranch);
            _uids.Add(_targetBranch.UID);
        }

        public void RestoreGraph(VinoveMerge _targetMerge)
        {
            foreach (string _mergePrevUID in _targetMerge.MergeUIDs)
            {
                if (ErrorDetector.Instance.IsStringHasData(_mergePrevUID))
                {
                    if (_mergePrevUID.StartsWith("SCE"))
                    {
                        VinoveScene _prevScene = FindSceneWithUID(_mergePrevUID);
                        _prevScene.Next = _targetMerge.UID;
                    }
                    else if (_mergePrevUID.StartsWith("BRA"))
                    {
                        VinoveBranch _prevBranch = FindBranchWithUID(_mergePrevUID);
                    }
                    if (_mergePrevUID.StartsWith("MRG"))
                    {
                        VinoveMerge _prevMerge = FindMergeWithUID(_mergePrevUID);
                        _prevMerge.Next = _targetMerge.UID;
                    }
                }
            }

            if (ErrorDetector.Instance.IsStringHasData(_targetMerge.Next))
            {
                if (_targetMerge.Next.StartsWith("SCE"))
                {
                    VinoveScene _nextScene = FindSceneWithUID(_targetMerge.Next);
                    _nextScene.Prev = _targetMerge.UID;
                }
                else if (_targetMerge.Next.StartsWith("BRA"))
                {
                    VinoveBranch _nextBranch = FindBranchWithUID(_targetMerge.Next);
                    _nextBranch.Prev = _targetMerge.UID;
                }
                if (_targetMerge.Next.StartsWith("MRG"))
                {
                    VinoveMerge _nextMerge = FindMergeWithUID(_targetMerge.Next);
                }
            }

            _merges.Add(_targetMerge);
            _uids.Add(_targetMerge.UID);
        }

        public void RestoreGraph(VinoveScript _targetScript)
        {
            if (ErrorDetector.Instance.IsStringHasData(_targetScript.Prev))
            {
                if (_targetScript.Prev.StartsWith("SCE"))
                {
                    VinoveScene _prevScene = FindSceneWithUID(_targetScript.Prev);
                    _prevScene.Next = _targetScript.UID;
                }
                else if (_targetScript.Prev.StartsWith("BRA"))
                {
                    VinoveBranch _prevBranch = FindBranchWithUID(_targetScript.Prev);
                }
                if (_targetScript.Prev.StartsWith("MRG"))
                {
                    VinoveMerge _prevMerge = FindMergeWithUID(_targetScript.Prev);
                    _prevMerge.Next = _targetScript.UID;
                }
            }

            if (ErrorDetector.Instance.IsStringHasData(_targetScript.Next))
            {
                if (_targetScript.Next.StartsWith("SCE"))
                {
                    VinoveScene _nextScene = FindSceneWithUID(_targetScript.Next);
                    _nextScene.Prev = _targetScript.UID;
                }
                else if (_targetScript.Next.StartsWith("BRA"))
                {
                    VinoveBranch _nextBranch = FindBranchWithUID(_targetScript.Next);
                    _nextBranch.Prev = _targetScript.UID;
                }
                if (_targetScript.Next.StartsWith("MRG"))
                {
                    VinoveMerge _nextMerge = FindMergeWithUID(_targetScript.Next);
                }
            }

            VinoveScene _parentScene = FindSceneWithUID(_targetScript.ParentSceneUID);
            _parentScene.Scripts.Add(_targetScript);
            _uids.Add(_targetScript.UID);

            if (ErrorDetector.Instance.IsStringEmpty(_parentScene.StarterUID))
            {
                _parentScene.StarterUID = _targetScript.UID;
            }
        }
#endif

        /////////////////////////////
        /// UID

        /// <summary>
        /// <para>Eng. Find and return Scene with uid </para>
        /// <para>Kor. UID를 사용하며 씬을 찾아 반환합니다. </para>
        /// </summary>
        /// <param name="_uid"> <para> Eng. UID of searching scene</para>
        ///                           <para> Kor. 찾을 씬의 UID.</para> </param>
        public VinoveScene FindSceneWithUID(string _uid)
        {
            foreach(VinoveScene _scene in _scenes)
            {
                if(_scene.UID == _uid)
                {
                    return _scene;
                }
            }

            return null;
        }

        /// <summary>
        /// <para>Eng. Find and return Branch with uid </para>
        /// <para>Kor. UID를 사용하며 분기점을 찾아 반환합니다. </para>
        /// </summary>
        /// <param name="_uid"> <para> Eng. UID of searching Branch</para>
        ///                           <para> Kor. 찾을 분기의 UID.</para> </param>
        public VinoveBranch FindBranchWithUID(string _uid)
        {
            foreach (VinoveBranch _branch in _branchs)
            {
                if (_branch.UID  == _uid)
                {
                    return _branch;
                }
            }

            return null;
        }

        /// <summary>
        /// <para>Eng. Find and return Merge with uid </para>
        /// <para>Kor. UID를 사용하며 병합기를 찾아 반환합니다. </para>
        /// </summary>
        /// <param name="_uid"> <para> Eng. UID of searching Merge</para>
        ///                           <para> Kor. 찾을 병합기의 UID.</para> </param>
        public VinoveMerge FindMergeWithUID(string _uid)
        {
            foreach (VinoveMerge _merge in _merges)
            {
                if (_merge.UID == _uid)
                {
                    return _merge;
                }
            }

            return null;
        }

        /////////////////////////////
        /// Link

        /////////////////////////////
        /// Detector

        /// <summary>
        /// <para>Eng. Compare 'is that UID has duplicated?' </para>
        /// <para>Kor. 해당 UID가 겹치는지에 대한 여부를 판단합니다.. </para>
        /// </summary>
        /// <param name="_uid"> <para> Eng. UID for compare</para>
        ///                           <para> Kor. 비교할 UID 입니다.</para> </param>
        private bool CheckUIDDuplicated(string _uid)
        {
            string result = _uids.Find(item => item == _uid);
            return result != null ? true : false;
        }

        /// <summary>
        /// <para>Eng. Detect mouse pos on graph </para>
        /// <para>Kor. 마우스가 씬 그래프 위에 위치한지 판단합니다. </para>
        /// </summary>
        /// <param name="mousePosition"> <para> Eng. Currently mouse pos</para>
        ///                           <para> Kor. 현재 마우스 위치입니다</para> </param>
        public VinoveScene IsContainWholeGraph(Vector2 _mousePosition)
        {
            foreach(VinoveScene _scene in _scenes)
            {
                if (_scene.CalculateGraphPosition(_translation, _zoomFactor).Contains(_mousePosition))
                {
                    return _scene;
                }
            }
            return null;
        }

        /// <summary>
        /// <para>Eng. Detect mouse pos on graph </para>
        /// <para>Kor. 마우스가 분기점 위에 위치한지 판단합니다. </para>
        /// </summary>
        /// <param name="mousePosition"> <para> Eng. Currently mouse pos</para>
        ///                           <para> Kor. 현재 마우스 위치입니다</para> </param>
        public VinoveBranch IsContainWholeBranch(Vector2 _mousePosition)
        {
            foreach (VinoveBranch _branch in _branchs)
            {
                if (_branch.CalculateBranchPosition(_translation, _zoomFactor).Contains(_mousePosition))
                {
                    return _branch;
                }
            }
            return null;
        }

        /// <summary>
        /// <para>Eng. Detect mouse pos on merge </para>
        /// <para>Kor. 마우스가 병합기 위에 위치한지 판단합니다. </para>
        /// </summary>
        /// <param name="mousePosition"> <para> Eng. Currently mouse pos</para>
        ///                           <para> Kor. 현재 마우스 위치입니다</para> </param>
        public VinoveMerge IsContainWholeMerge(Vector2 _mousePosition)
        {
            foreach (VinoveMerge _merge in Merges)
            {
                if (_merge.CalculateMergePosition(_translation, _zoomFactor).Contains(_mousePosition))
                {
                    return _merge;
                }
            }
            return null;
        }
    }
}
