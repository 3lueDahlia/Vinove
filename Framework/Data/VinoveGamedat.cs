using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ViNovE.Framework.Data.Dictionary;

namespace ViNovE.Framework.Data
{
    public class VinoveGamedat
    {
        [SerializeField]
        string _fileDir;
        [SerializeField]
        string _fileName;

        /////////////////////////////
        /// For Graph Data
        [SerializeField]
        List<string> _uids;         // UID Intergration Storage in 1 Vinove

        [SerializeField]
        string _starterUID;

        [SerializeField]
        List<VinoveScene> _scenes;   // Scenes list in Vinove
        [SerializeField]
        List<VinoveBranch> _branchs;   // Branchs list in Vinove
        [SerializeField]
        List<VinoveMerge> _merges;   // Merges list in Vinove

        /////////////////////////////
        /// For Game Data

        [SerializeField]
        List<string> _speakerNames;
        [SerializeField]
        VinoveDictionary _speakerIllustrations;
        [SerializeField]
        VinoveDictionary _backgroundIllustrations;

        public VinoveGamedat(Vinove _saveVinove)
        {
            _fileDir = _saveVinove.FileDir;
            _fileName = _saveVinove.FileName;

            _uids = _saveVinove.UIDs;
            _starterUID = _saveVinove.StarterUID;

            _scenes = _saveVinove.Scene;
            _branchs = _saveVinove.Branchs;
            _merges = _saveVinove.Merges;

            _speakerNames = _saveVinove.SpeakerNames;
            _speakerIllustrations = _saveVinove.SpeakerIllustrations;
            _backgroundIllustrations = _saveVinove.BackgroundIllustrations;
        }
    }
}