#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ViNovE.Framework;
using ViNovE.Framework.Data;
using ViNovE.Framework.Error;

namespace ViNovE.Framework.GraphViewer
{
    public partial class GraphViewer
    {
        void DrawSceneMinimap()
        {
            GUI.Box(MinimapRect, "", ViewerStyleSheet.GraphMinimap);

            foreach (VinoveScene scene in currentVinove.Scene)
            {   // repeat each vinove scene
                Rect _sceneGraphSize = new Rect(scene.CalculateGraphPosition(Translation, ZoomFactor));

                if (ErrorDetector.Instance.IsCanvasContainedRect(MinimapDetectRect, _sceneGraphSize))
                {   // if, canvas contains graph
                    //Debug.Log(ConvertGraphPositionToMinimap(_sceneGraphSize));
                    GUI.color = Color.red;
                    GUI.DrawTexture(ConvertGraphPositionToMinimap(_sceneGraphSize), Texture2D.whiteTexture);
                    GUI.color = Color.white;
                }
            }

            foreach (VinoveBranch branch in currentVinove.Branchs)
            {   // repeat each vinove scene
                Rect _sceneGraphSize = new Rect(branch.CalculateBranchPosition(Translation, ZoomFactor));

                if (ErrorDetector.Instance.IsCanvasContainedRect(MinimapDetectRect, _sceneGraphSize))
                {   // if, canvas contains graph
                    //Debug.Log(ConvertGraphPositionToMinimap(_sceneGraphSize));
                    GUI.color = Color.cyan;
                    GUI.DrawTexture(ConvertGraphPositionToMinimap(_sceneGraphSize), Texture2D.whiteTexture);
                    GUI.color = Color.white;
                }
            }

            foreach (VinoveMerge merge in currentVinove.Merges)
            {   // repeat each vinove scene
                Rect _sceneGraphSize = new Rect(merge.CalculateMergePosition(Translation, ZoomFactor));

                if (ErrorDetector.Instance.IsCanvasContainedRect(MinimapDetectRect, _sceneGraphSize))
                {   // if, canvas contains graph
                    //Debug.Log(ConvertGraphPositionToMinimap(_sceneGraphSize));
                    GUI.color = Color.magenta;
                    GUI.DrawTexture(ConvertGraphPositionToMinimap(_sceneGraphSize), Texture2D.whiteTexture);
                    GUI.color = Color.white;
                }
            }

            GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            Vector2 _minimapViewareaSize = new Vector2((MinimapRect.width / 3), (MinimapRect.height / 3));
            GUI.DrawTexture(new Rect(MinimapRect.x + _minimapViewareaSize.x, MinimapRect.y + _minimapViewareaSize.y,
                _minimapViewareaSize.x, _minimapViewareaSize.y), Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        void DrawScriptMinimap()
        {
            GUI.Box(MinimapRect, "", ViewerStyleSheet.GraphMinimap);

            foreach (VinoveScript script in currentScene.Scripts)
            {   // repeat each vinove scene
                Rect _sceneGraphSize = new Rect(script.CalculatePosition(currentScene.Translation, currentScene.ZoomFactor));

                if (ErrorDetector.Instance.IsCanvasContainedRect(MinimapDetectRect, _sceneGraphSize))
                {   // if, canvas contains graph
                    //Debug.Log(ConvertGraphPositionToMinimap(_sceneGraphSize));
                    GUI.color = Color.yellow;
                    GUI.DrawTexture(ConvertGraphPositionToMinimap(_sceneGraphSize), Texture2D.whiteTexture);
                    GUI.color = Color.white;
                }
            }

            GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            Vector2 _minimapViewareaSize = new Vector2((MinimapRect.width / 3), (MinimapRect.height / 3));
            GUI.DrawTexture(new Rect(MinimapRect.x + _minimapViewareaSize.x, MinimapRect.y + _minimapViewareaSize.y,
                _minimapViewareaSize.x, _minimapViewareaSize.y), Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        Rect ConvertGraphPositionToMinimap(Rect _graphPosition)
        {
            Rect _retVal = new Rect(_graphPosition.x /12 + MinimapRect.x + (MinimapRect.width / 3),
                _graphPosition.y/12 + MinimapRect.y + (MinimapRect.height / 3), _graphPosition.width /12, _graphPosition.height/12);
            return _retVal;
        }
    }
}
#endif