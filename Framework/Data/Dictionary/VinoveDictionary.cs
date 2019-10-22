using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ViNovE.Framework.Data.Dictionary
{
    [Serializable]
    public class DictionaryData
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables

        [SerializeField]
        private string _key;
        [SerializeField]
        private string _path;
        [SerializeField]
        private Texture2D _texture;

        ////////////////////////////////////////////////////////////////////////
        /// Properties
        /// 

        public string Key
        {
            get { return _key; }
        }
        public string Path
        {
            get { return _path; }
        }
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        ////////////////////////////////////////////////////////////////////////
        /// Func
        /// 

        public DictionaryData(string _addKey, string _addPath, Texture2D addTexture = null)
        {   //Constructor
            _key = _addKey;
            _path = _addPath;
            _texture = addTexture;
        }
    }


    [Serializable]
    public class VinoveDictionary
    {
        ////////////////////////////////////////////////////////////////////////
        /// Variables
        ///
        [SerializeField]
        private int _count;

        [SerializeField]
        List<DictionaryData> _dictionary;

        ////////////////////////////////////////////////////////////////////////
        /// Properties
        /// 

        public int Count
        {
            get { return _count; }
        }
        public List<DictionaryData> Dictionary
        {
            get { return _dictionary; }
        }

        ////////////////////////////////////////////////////////////////////////
        /// Func
        /// 

        public VinoveDictionary()
        {   //Constructor
            _count = 0;
            _dictionary = new List<DictionaryData>();
        }

        public void Add(string _addKey, string _addPath, Texture2D _addTexture = null)
        {
            if(!ContainsKey(_addKey))
            {   // Added key is not contain dictionary
                _dictionary.Add(new DictionaryData(_addKey, _addPath, _addTexture));
                _count++;
            }
        }

        public bool Remove(string _removeKey)
        {
            bool _retVal = false;

            DictionaryData _tempData = default;

            foreach (DictionaryData _data in _dictionary)
            {
                if (_data.Key == _removeKey)
                {
                    _tempData = _data;
                    _retVal = true;
                }
            }

            if(_retVal == true)
            {
                _dictionary.Remove(_tempData);
                _count--;
            }

            return _retVal;
        }

        public void Clear()
        {
            _dictionary.Clear();
            _count = 0;
        }

        public bool FindWithKey(string _key, out DictionaryData _foundData)
        {
            bool _retVal = false;
            _foundData = null;

            foreach (DictionaryData _data in _dictionary)
            {
                if (_data.Key == _key)
                {
                    _foundData = _data;
                    _retVal = true;
                }
            }

            return _retVal;
        }

        public bool GetTextureData(string _key, out Texture2D _texture)
        {
            bool _retVal = false;
            _texture = default;

            foreach (DictionaryData _data in _dictionary)
            {
                if (_data.Key == _key)
                {
                    if(_data.Texture == null)
                    {
#if UNITY_EDITOR
                        _data.Texture = AssetDatabase.LoadAssetAtPath<Texture2D>(_data.Path);
#endif
                    }
                    _texture = _data.Texture;
                    _retVal = true;
                }
            }

            return _retVal;
        }

        public bool GetPathData(string _key, out string _path)
        {
            bool _retVal = false;
            _path = default;

            foreach (DictionaryData _data in _dictionary)
            {
                if (_data.Key == _key)
                {
                    _path = _data.Path;
                }
            }

            return _retVal;
        }

        public bool ContainsKey(string _key)
        {
            bool _retVal = false;

            foreach(DictionaryData _data in _dictionary)
            {
                if(_data.Key == _key)
                {
                    _retVal = true;
                }
            }

            return _retVal;
        }

        public string[] GetKeys()
        {
            List<string> _keys = new List<string>();

            foreach (DictionaryData _data in _dictionary)
            {
                _keys.Add(_data.Key);
            }
            string[] _strKeys = new string[_count];

            _keys.CopyTo(_strKeys);

            return _strKeys;
        }
    }
}