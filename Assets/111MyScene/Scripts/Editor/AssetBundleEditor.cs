using GameTools;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using XLua;
//#define Android_Platform
public class AssetBundleEditor
{
    public static string  local_url = Application.streamingAssetsPath + @"\";
    [MenuItem("Assets/AssetBundleTools/CreatAssetBundles")]
    static void CreatAssetBundles()
    {
        //string local_url = Application.streamingAssetsPath + @"\";
        if (Directory.Exists(local_url + UpdateManager.AB_PATH) == false)
        {
            Directory.CreateDirectory(local_url + UpdateManager.AB_PATH);      
        }
#if Android_Platform
        BuildPipeline.BuildAssetBundles(SavePath, BuildAssetBundleOptions.None, BuildTarget.Android);
#else
        BuildPipeline.BuildAssetBundles(local_url+ UpdateManager.AB_PATH, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
#endif
    }
    [MenuItem("Assets/AssetBundleTools/CreatCfg")]
    static void CreatCfg()
    {
        //string local_url = Application.streamingAssetsPath + @"\";
        UpdateTools.CreatCfg(local_url, UpdateManager.AB_PATH, UpdateManager.LUA_PATH, UpdateManager.CFG_FULL_PATH);
    }

    

}