using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ViNovE.Framework.InGame.Utility
{
    ////////////////////////////////////////////////////////////////////////
    /// Func

    public class BranchTextChanger : MonoBehaviour
    {
        public Text _childText;

        public void ChangeText(string _changeText)
        {
            _childText.text = _changeText;
        }
    }
}
