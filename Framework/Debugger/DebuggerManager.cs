using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

using ViNovE.Framework.Error;
using ViNovE.Framework.InGame;
using ViNovE.Framework.InGame.SaveLoad;

namespace ViNovE.Framework.Debugger
{
    public class DebuggerManager : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////////
        /// Singleton

        private static DebuggerManager instance;
        public static DebuggerManager GetInstance()
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(DebuggerManager)) as DebuggerManager;

                if (!instance)
                {
                    ErrorMessages.Instance.MissingSingletoneObject("DebuggerManager");
                }
            }

            return instance;
        }


        ////////////////////////////////////////////////////////////////////////
        /// Variables

        public Text _applicationDataFolder;
        public Text _documentsFolder;
        public Text _assetDir;

        ////////////////////////////////////////////////////////////////////////
        /// Func

        public void Start()
        {
            Init();
        }

        public void Init()
        {
            _applicationDataFolder.text = Application.dataPath + "/StreamingAssets/gamedat.asset";
            _documentsFolder.text = SaveLoadSlotManager.GetInstance()._saveFolderDir;
            //_assetDir.text = InGameManager.GetInstance()._currentVinove.FileDir;
        }

        public void Log(string _log)
        {
            _assetDir.text += _log + "\n";
        }
    }
}