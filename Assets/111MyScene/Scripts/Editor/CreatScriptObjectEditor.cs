using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Manager;
using System.IO;

public class CreatScriptObjectEditor : MonoBehaviour {
    [MenuItem("Assets/CreatPoolManagerAsset")]
    static void CreatPoolManagerAsset()
    {
        string path = Path.Combine( Application.dataPath ,"ScriptObjectFold");
        if (Directory.Exists(path)==false)
        {
            Directory.CreateDirectory(path);
        }
              
        PoolManager poolmanager=ScriptableObject.CreateInstance<PoolManager>();
        //string fullpath = Path.Combine(path, "PoolManager.asset");

        AssetDatabase.CreateAsset(poolmanager, "Assets/ScriptObjectFold/PoolManager.asset");
        AssetDatabase.SaveAssets();
    }
}
