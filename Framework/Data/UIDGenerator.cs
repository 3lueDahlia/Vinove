using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ViNovE.Framework.Error;

public class UIDGenerator
{
    private static UIDGenerator _instance = null;
    public static UIDGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIDGenerator();

                if (_instance == null)
                {
                    ErrorMessages.Instance.MissingSingletoneObject("UIDGenerator");
                }
            }


            return _instance;
        }
    }

    // Start is called before the first frame update
    /// <summary>
    /// <para>Eng. Usage Identification Ganerator </para>
    /// <para>Kor. 고유 식별자 생성기 </para>
    /// </summary>
    /// <returns></returns>
    public string Generate(string _type)
    {
        string newGUID = _type + "-" + Guid.NewGuid().ToString();
        return newGUID;
    }
}
