#if UNITY_EDITOR
using ViNovE.Framework;
using ViNovE.Framework.Data;

using UnityEngine;
using UnityEditor;

namespace ViNovE.Framework.GraphViewer
{
    public partial class GraphViewer
    {
        static void OpenScriptInspector(VinoveScript _openedScript)
        {
            ScriptInspector _scriptInspector = new ScriptInspector(currentVinove, _openedScript);

            Rect _activeRect = new Rect(e.mousePosition.x, e.mousePosition.y, 0, 0);

            PopupWindow.Show(_activeRect, _scriptInspector);
        }

        static void OpenRenameInspector(string _objectUID)
        {
            RenameInspector _renameInspector = new RenameInspector(currentVinove, _objectUID);

            Vector2 _inspectorSize = _renameInspector.GetWindowSize();
            Rect _activeRect = new Rect(e.mousePosition.x, e.mousePosition.y, 0, 0);

            PopupWindow.Show(_activeRect, _renameInspector);
        }
        static void OpenRenameInspector(string _parentUID, string _objectUID)
        {
            RenameInspector _renameInspector = new RenameInspector(currentVinove, currentVinove.FindSceneWithUID(_parentUID), _objectUID);

            Vector2 _inspectorSize = _renameInspector.GetWindowSize();
            Rect _activeRect = new Rect(e.mousePosition.x, e.mousePosition.y, 0, 0);

            PopupWindow.Show(_activeRect, _renameInspector);
        }

        static void OpenCountInspector(VinoveBranch _targetBranch)
        {
            BranchCountInspector _branchCountInspector = new BranchCountInspector(currentVinove, _targetBranch);

            Vector2 _inspectorSize = _branchCountInspector.GetWindowSize();
            Rect _activeRect = new Rect(e.mousePosition.x, e.mousePosition.y, 0, 0);

            PopupWindow.Show(_activeRect, _branchCountInspector);
        }
        static void OpenCountInspector(VinoveMerge _targetMerge)
        {
            BranchCountInspector _branchCountInspector = new BranchCountInspector(currentVinove, _targetMerge);

            Vector2 _inspectorSize = _branchCountInspector.GetWindowSize();
            Rect _activeRect = new Rect(e.mousePosition.x, e.mousePosition.y, 0, 0);

            PopupWindow.Show(_activeRect, _branchCountInspector);
        }

        static void OpenBranchInspector(VinoveBranch _targetBranch)
        {
            BranchInspector _branchInspector = new BranchInspector(currentVinove, _targetBranch);

            Vector2 _inspectorSize = _branchInspector.GetWindowSize();
            Rect _activeRect = new Rect(e.mousePosition.x, e.mousePosition.y, 0, 0);

            PopupWindow.Show(_activeRect, _branchInspector);
        }

        static void OpenSceneBackgroundInspector(VinoveScene _targetScene)
        {
            SceneBackgroundInspector _BgInspector = new SceneBackgroundInspector(currentVinove, _targetScene);

            Vector2 _inspectorSize = _BgInspector.GetWindowSize();
            Rect _activeRect = new Rect(e.mousePosition.x, e.mousePosition.y, 0, 0);

            PopupWindow.Show(_activeRect, _BgInspector);
        }

        static void AddSpeakerInspector()
        {
            SpeakerDataInspector _speakerDataInspector = new SpeakerDataInspector(currentVinove);

            Vector2 _inspectorSize = _speakerDataInspector.GetWindowSize();
            Rect _activeRect = new Rect(e.mousePosition.x, e.mousePosition.y, 0, 0);

            PopupWindow.Show(_activeRect, _speakerDataInspector);
        }

        static void AddIllustInspector(string _illustType)
        {
            IllustrationInspector _IlluInspector = new IllustrationInspector(currentVinove, _illustType);

            Vector2 _inspectorSize = _IlluInspector.GetWindowSize();
            Rect _activeRect = new Rect(e.mousePosition.x, e.mousePosition.y, 0, 0);

            PopupWindow.Show(_activeRect, _IlluInspector);
        }

        //static void OpenTextureResizeInspector()
        //{
        //    TextureResizeInspector _resizeInspector = new TextureResizeInspector();

        //    Vector2 _inspectorSize = _resizeInspector.GetWindowSize();
        //    Rect _activeRect = new Rect(e.mousePosition.x, e.mousePosition.y, 0, 0);

        //    PopupWindow.Show(_activeRect, _resizeInspector);
        //}
    }
}
#endif