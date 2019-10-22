using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ViNovE.Framework.Data;

namespace ViNovE.Framework.InGame.Utility
{
    [Serializable]
    public class AssetPathArchive
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        public string _path;
        public string _sceneUID;
        public string _scriptUID;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        public AssetPathArchive()
        {
            _path = default;
        }
        public AssetPathArchive(string _assetPath)
        {
            _path = _assetPath;
        }
        public AssetPathArchive(string _assetPath, string _startSceneUID)
        {
            _path = _assetPath;
            _sceneUID = _startSceneUID;
        }
        public AssetPathArchive(string _assetPath, string _startSceneUID, string _startScriptUID)
        {
            _path = _assetPath;
            _sceneUID = _startSceneUID;
            _scriptUID = _startScriptUID;
        }
    }
}