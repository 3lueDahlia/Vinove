using System;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

using ViNovE.Framework.InGame;

namespace ViNovE.Framework.Data.IO
{
    public class BinaryIOManager
    {
        private static BinaryIOManager Inst;

        public static BinaryIOManager GetInst()
        {
            if(Inst == null)
            {
                Inst = new BinaryIOManager();
            }

            return Inst;
        }

        public void BinarySerialize<T>(T _tData, string _filePath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_filePath, FileMode.Create);
            formatter.Serialize(stream, _tData);
            stream.Close();
        }

        public T BinaryDeserialize<T>(string _filePath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_filePath, FileMode.Open);
            T _tData = (T)formatter.Deserialize(stream);
            stream.Close();

            return _tData;
        }
    }
}
