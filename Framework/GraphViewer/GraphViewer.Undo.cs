#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ViNovE.Framework.Data;
using ViNovE.Framework.Enum;

namespace ViNovE.Framework.GraphViewer
{
    public class UndoRedoStackData
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        UndoRedoWorkType _targetWorkType;
        GraphType _targetGraphType;

        string _targetGraphUID;

        VinoveScene _targetScene;
        VinoveBranch _targetBranch;
        VinoveMerge _targetMerge;
        VinoveScript _targetScript;

        ////////////////////////////////////////////////////////////////////////
        /// Properties

        public UndoRedoWorkType TargetWorkType
        {
            get { return _targetWorkType; }
        }
        public GraphType TargetGraphType
        {
            get { return _targetGraphType; }
        }

        public string TargetGraphUID
        {
            get { return _targetGraphUID; }
        }

        public VinoveScene TargetScene
        {
            get { return _targetScene; }
        }
        public VinoveBranch TargetBranch
        {
            get { return _targetBranch; }
        }
        public VinoveMerge TargetMerge
        {
            get { return _targetMerge; }
        }
        public VinoveScript TargetScript
        {
            get { return _targetScript; }
        }

        ////////////////////////////////////////////////////////////////////////
        /// Func

        public UndoRedoStackData(UndoRedoWorkType _workType, GraphType _graphType, string _graphUID)
        {
            _targetWorkType = _workType;
            _targetGraphType = _graphType;

            _targetGraphUID = _graphUID;
        }

        public void SetGraphData(VinoveScene _scene)
        {
            _targetScene = _scene;
        }
        public void SetGraphData(VinoveBranch _branch)
        {
            _targetBranch = _branch;
        }
        public void SetGraphData(VinoveMerge _merge)
        {
            _targetMerge = _merge;
        }
        public void SetGraphData(VinoveScript _script)
        {
            _targetScript = _script;
        }
    }

    public partial class GraphViewer
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        public static List<UndoRedoStackData> _undoList;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        /// <summary>
        /// <para>Eng. Initialize Stack Data </para>
        /// <para>Kor. Undo 스택 데이터를 초기화합니다. </para>
        /// </summary>
        public static void InitUndoStack()
        {
            _undoList = new List<UndoRedoStackData>();
        }

        // -------------------- Undo - Create --------------------

        /// <summary>
        /// <para>Eng. Push 'Create' into Undo Stack </para>
        /// <para>Kor. Undo 스택에 '생성' 스택을 넣습니다. </para>
        /// </summary>
        /// <param name="_targetUID">
        /// <para>Eng. Target Graph UID </para>
        /// <para>Kor. 대상 그래프의 UID 입니다. </para></param>
        /// <param name="_scene">
        /// <para>Eng. Target Graph Data </para>
        /// <para>Kor. 대상 그래프 데이터입니다. </para></param>
        public static void UndoStackCreate(string _targetUID, VinoveScene _scene)
        {
            UndoRedoStackData _stackData = new UndoRedoStackData(UndoRedoWorkType.GraphCreate, GraphType.Scene, _targetUID);
            _stackData.SetGraphData(_scene);

            AddUndoData(_stackData);
        }

        /// <summary>
        /// <para>Eng. Push 'Create' into Undo Stack </para>
        /// <para>Kor. Undo 스택에 '생성' 스택을 넣습니다. </para>
        /// </summary>
        /// <param name="_targetUID">
        /// <para>Eng. Target Graph UID </para>
        /// <para>Kor. 대상 그래프의 UID 입니다. </para></param>
        /// <param name="_branch">
        /// <para>Eng. Target Graph Data </para>
        /// <para>Kor. 대상 그래프 데이터입니다. </para></param>
        public static void UndoStackCreate(string _targetUID, VinoveBranch _branch)
        {
            UndoRedoStackData _stackData = new UndoRedoStackData(UndoRedoWorkType.GraphCreate, GraphType.Branch, _targetUID);
            _stackData.SetGraphData(_branch);

            AddUndoData(_stackData);
        }

        /// <summary>
        /// <para>Eng. Push 'Create' into Undo Stack </para>
        /// <para>Kor. Undo 스택에 '생성' 스택을 넣습니다. </para>
        /// </summary>
        /// <param name="_targetUID">
        /// <para>Eng. Target Graph UID </para>
        /// <para>Kor. 대상 그래프의 UID 입니다. </para></param>
        /// <param name="_merge">
        /// <para>Eng. Target Graph Data </para>
        /// <para>Kor. 대상 그래프 데이터입니다. </para></param>
        public static void UndoStackCreate(string _targetUID, VinoveMerge _merge)
        {
            UndoRedoStackData _stackData = new UndoRedoStackData(UndoRedoWorkType.GraphCreate, GraphType.Merge, _targetUID);
            _stackData.SetGraphData(_merge);

            AddUndoData(_stackData);
        }

        /// <summary>
        /// <para>Eng. Push 'Create' into Undo Stack </para>
        /// <para>Kor. Undo 스택에 '생성' 스택을 넣습니다. </para>
        /// </summary>
        /// <param name="_targetUID">
        /// <para>Eng. Target Graph UID </para>
        /// <para>Kor. 대상 그래프의 UID 입니다. </para></param>
        /// <param name="_script">
        /// <para>Eng. Target Graph Data </para>
        /// <para>Kor. 대상 그래프 데이터입니다. </para></param>
        public static void UndoStackCreate(string _targetUID, VinoveScript _script)
        {
            UndoRedoStackData _stackData = new UndoRedoStackData(UndoRedoWorkType.GraphCreate, GraphType.Script, _targetUID);
            _stackData.SetGraphData(_script);

            AddUndoData(_stackData);
        }

        // -------------------- Undo - Delete --------------------

        /// <summary>
        /// <para>Eng. Push 'Delete' into Undo Stack </para>
        /// <para>Kor. Undo 스택에 '삭제' 스택을 넣습니다. </para>
        /// </summary>
        /// <param name="_targetUID">
        /// <para>Eng. Target Graph UID </para>
        /// <para>Kor. 대상 그래프의 UID 입니다. </para></param>
        /// <param name="_scene">
        /// <para>Eng. Target Graph Data </para>
        /// <para>Kor. 대상 그래프 데이터입니다. </para></param>
        public static void UndoStackDelete(string _targetUID, VinoveScene _scene)
        {
            UndoRedoStackData _stackData = new UndoRedoStackData(UndoRedoWorkType.GraphDelete, GraphType.Scene, _targetUID);
            _stackData.SetGraphData(_scene);

            AddUndoData(_stackData);
        }

        /// <summary>
        /// <para>Eng. Push 'Delete' into Undo Stack </para>
        /// <para>Kor. Undo 스택에 '삭제' 스택을 넣습니다. </para>
        /// </summary>
        /// <param name="_targetUID">
        /// <para>Eng. Target Graph UID </para>
        /// <para>Kor. 대상 그래프의 UID 입니다. </para></param>
        /// <param name="_branch">
        /// <para>Eng. Target Graph Data </para>
        /// <para>Kor. 대상 그래프 데이터입니다. </para></param>
        public static void UndoStackDelete(string _targetUID, VinoveBranch _branch)
        {
            UndoRedoStackData _stackData = new UndoRedoStackData(UndoRedoWorkType.GraphDelete, GraphType.Branch, _targetUID);
            _stackData.SetGraphData(_branch);

            AddUndoData(_stackData);
        }

        /// <summary>
        /// <para>Eng. Push 'Delete' into Undo Stack </para>
        /// <para>Kor. Undo 스택에 '삭제' 스택을 넣습니다. </para>
        /// </summary>
        /// <param name="_targetUID">
        /// <para>Eng. Target Graph UID </para>
        /// <para>Kor. 대상 그래프의 UID 입니다. </para></param>
        /// <param name="_merge">
        /// <para>Eng. Target Graph Data </para>
        /// <para>Kor. 대상 그래프 데이터입니다. </para></param>
        public static void UndoStackDelete(string _targetUID, VinoveMerge _merge)
        {
            UndoRedoStackData _stackData = new UndoRedoStackData(UndoRedoWorkType.GraphDelete, GraphType.Merge, _targetUID);
            _stackData.SetGraphData(_merge);

            AddUndoData(_stackData);
        }

        /// <summary>
        /// <para>Eng. Push 'Delete' into Undo Stack </para>
        /// <para>Kor. Undo 스택에 '삭제' 스택을 넣습니다. </para>
        /// </summary>
        /// <param name="_targetUID">
        /// <para>Eng. Target Graph UID </para>
        /// <para>Kor. 대상 그래프의 UID 입니다. </para></param>
        /// <param name="_script">
        /// <para>Eng. Target Graph Data </para>
        /// <para>Kor. 대상 그래프 데이터입니다. </para></param>
        public static void UndoStackDelete(string _targetUID, VinoveScript _script)
        {
            UndoRedoStackData _stackData = new UndoRedoStackData(UndoRedoWorkType.GraphDelete, GraphType.Script, _targetUID);
            _stackData.SetGraphData(_script);

            AddUndoData(_stackData);
        }

        // -------------------- Undo - Move --------------------

        /// <summary>
        /// <para>Eng. Push 'Move' into Undo Stack </para>
        /// <para>Kor. Undo 스택에 '이동' 스택을 넣습니다. </para>
        /// </summary>
        /// <param name="_targetUID">
        /// <para>Eng. Target Graph UID </para>
        /// <para>Kor. 대상 그래프의 UID 입니다. </para></param>
        /// <param name="_scene">
        /// <para>Eng. Target Graph Data </para>
        /// <para>Kor. 대상 그래프 데이터입니다. </para></param>
        public static void UndoStackMove(string _targetUID, VinoveScene _scene)
        {
            UndoRedoStackData _stackData = new UndoRedoStackData(UndoRedoWorkType.GraphMove, GraphType.Scene, _targetUID);
            _stackData.SetGraphData(_scene);

            AddUndoData(_stackData);
        }

        /// <summary>
        /// <para>Eng. Push 'Move' into Undo Stack </para>
        /// <para>Kor. Undo 스택에 '이동' 스택을 넣습니다. </para>
        /// </summary>
        /// <param name="_targetUID">
        /// <para>Eng. Target Graph UID </para>
        /// <para>Kor. 대상 그래프의 UID 입니다. </para></param>
        /// <param name="_branch">
        /// <para>Eng. Target Graph Data </para>
        /// <para>Kor. 대상 그래프 데이터입니다. </para></param>
        public static void UndoStackMove(string _targetUID, VinoveBranch _branch)
        {
            UndoRedoStackData _stackData = new UndoRedoStackData(UndoRedoWorkType.GraphMove, GraphType.Branch, _targetUID);
            _stackData.SetGraphData(_branch);

            AddUndoData(_stackData);
        }

        /// <summary>
        /// <para>Eng. Push 'Move' into Undo Stack </para>
        /// <para>Kor. Undo 스택에 '이동' 스택을 넣습니다. </para>
        /// </summary>
        /// <param name="_targetUID">
        /// <para>Eng. Target Graph UID </para>
        /// <para>Kor. 대상 그래프의 UID 입니다. </para></param>
        /// <param name="_merge">
        /// <para>Eng. Target Graph Data </para>
        /// <para>Kor. 대상 그래프 데이터입니다. </para></param>
        public static void UndoStackMove(string _targetUID, VinoveMerge _merge)
        {
            UndoRedoStackData _stackData = new UndoRedoStackData(UndoRedoWorkType.GraphMove, GraphType.Merge, _targetUID);
            _stackData.SetGraphData(_merge);

            AddUndoData(_stackData);
        }

        /// <summary>
        /// <para>Eng. Push 'Move' into Undo Stack </para>
        /// <para>Kor. Undo 스택에 '이동' 스택을 넣습니다. </para>
        /// </summary>
        /// <param name="_targetUID">
        /// <para>Eng. Target Graph UID </para>
        /// <para>Kor. 대상 그래프의 UID 입니다. </para></param>
        /// <param name="_script">
        /// <para>Eng. Target Graph Data </para>
        /// <para>Kor. 대상 그래프 데이터입니다. </para></param>
        public static void UndoStackMove(string _targetUID, VinoveScript _script)
        {
            UndoRedoStackData _stackData = new UndoRedoStackData(UndoRedoWorkType.GraphMove, GraphType.Script, _targetUID);
            _stackData.SetGraphData(_script);

            AddUndoData(_stackData);
        }

        public static void AddUndoData(UndoRedoStackData _stackData)
        {
            if(_undoList.Count < 20)
            {
                _undoList.Add(_stackData);
            }
            else
            {
                _undoList.RemoveAt(0);
                _undoList.Add(_stackData);
            }
        }

        public static UndoRedoStackData GetUndoData()
        {
            UndoRedoStackData _retVal = _undoList[_undoList.Count-1];
            _undoList.RemoveAt(_undoList.Count - 1);
            return _retVal;
        }

        // -------------------- Undo / Redo Sequence --------------------
        public static void UndoSequence()
        {
            UndoRedoStackData _undoData = GetUndoData();

            switch (_undoData.TargetWorkType)
            {
            case UndoRedoWorkType.GraphCreate:
                { 
                    GraphDeleteSequence(_undoData.TargetGraphUID, false);
                    break;
                }
            case UndoRedoWorkType.GraphDelete:
                {
                    switch (_undoData.TargetGraphType)
                    {
                        case GraphType.Scene:
                            currentVinove.RestoreGraph(_undoData.TargetScene);
                            break;
                        case GraphType.Branch:
                            currentVinove.RestoreGraph(_undoData.TargetBranch);
                            break;
                        case GraphType.Merge:
                            currentVinove.RestoreGraph(_undoData.TargetMerge);
                            break;
                        case GraphType.Script:
                            currentVinove.RestoreGraph(_undoData.TargetScript);
                            break;
                    }
                    break;
                }

            case UndoRedoWorkType.GraphMove:
                {
                    switch (_undoData.TargetGraphType)
                    {
                        case GraphType.Scene:
                            currentVinove.FindSceneWithUID(_undoData.TargetGraphUID).Position = _undoData.TargetScene.Position;
                            break;
                        case GraphType.Branch:
                            currentVinove.FindBranchWithUID(_undoData.TargetGraphUID).Position = _undoData.TargetBranch.Position;
                            break;
                        case GraphType.Merge:
                            currentVinove.FindMergeWithUID(_undoData.TargetGraphUID).Position = _undoData.TargetMerge.Position;
                            break;
                        case GraphType.Script:
                            currentVinove.FindSceneWithUID(_undoData.TargetScript.ParentSceneUID).FindScriptWithUID(_undoData.TargetGraphUID).Position = _undoData.TargetScript.Position;
                            break;
                    }
                    break;
                }
            }
        }
    }
}
#endif