using GameTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using XLua;


namespace Manager
{
    public class UpdateManager : MonoBehaviour
    {
        //path.txt存放一些路径
        //固定放在Resources下，发布后就不变了
        public void Awake()
        {
            Initial();
        }
        public void Initial()
        {

            //检测LOCAL_URL是否为空，是就根据当前安装的位置生成Application.streamingAssetsPath
            if (string.IsNullOrEmpty(LOCAL_URL) == true)
            {
                LOCAL_URL = Application.streamingAssetsPath + @"\";
            }
        }

        //GameRoot游戏开始持有一个UpdateManager
        //UpdateManager内部处理 检测更新-->更新-->加载资源
        //需要的路径 ab包根路径如"AssetBundles"       lua文件根路径如"LuaFiles"         配置文件固定相对路径如"Cfg/cfg.txt"    url固定路径如"http://local/"
        //路径
        public static string URL = @"http://localhost/";
        public static string LOCAL_URL;
        public static string CFG_FULL_PATH = @"Cfg\cfg.txt";
        public static string AB_PATH = "AssetBundles";
        public static string LUA_PATH = "LuaFiles";



        //远程配置获取情况
        private const string CFG_ERROR = "CFG_ERROR";
        //远程配置表
        private string remoteCfg = "";
        //加载资源的存储字典 
        Dictionary<string, GameObject> GoDict = new Dictionary<string, GameObject>();

        //处理下载更新的协程
        public IEnumerator UpdateAndDownload()
        {
            //检测cache
            while (!Caching.ready)
            {
                yield return null;
            }

            //发起更新请求 下载remotecfg
            yield return DownloadCfg();
            if (remoteCfg == CFG_ERROR)
            {
                //Debug.Log("未获取到配置文件，无更新");
                //加载资源
                yield return LoadAssetFromLoacl();
                yield break;
            }
            if (string.IsNullOrEmpty(remoteCfg) == true)
            {
                Debug.Log("配置文件为空，无更新");
                //加载资源
                yield return LoadAssetFromLoacl();
                yield break;
            }

            //比较localcfg与remotecfg 得到updatelist
            string localCfg;
            if (File.Exists(UpdateManager.LOCAL_URL + CFG_FULL_PATH) == false)
            {
                localCfg = "";
            }
            else
            {
                localCfg = File.ReadAllText(UpdateManager.LOCAL_URL + CFG_FULL_PATH);
            }
            UpdateTools.CreatDirectory(UpdateManager.LOCAL_URL + CFG_FULL_PATH, true);
            List<string> pathList = UpdateTools.CampareCfg(UpdateTools.GetDictByCfg(localCfg), UpdateTools.GetDictByCfg(remoteCfg));
            //根据updatelist下载资源 或则无更新和更新失败，将控制权交给加载资源，然后break
            yield return DownloadByPathList(pathList);
            //下载结束  更新配置文件cfg

            //将控制权交给 加载资源
            FileStream fs = new FileStream(UpdateManager.LOCAL_URL + CFG_FULL_PATH, FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(remoteCfg);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
            print("下载完成 pathlist" + pathList.Count + "   remotecfg:" + remoteCfg + "   localCfg" + localCfg);

            //加载资源
            yield return LoadAssetFromLoacl();
        }

        //发起更新请求 下载远程配置表
        private IEnumerator DownloadCfg()
        {
            UnityWebRequest downCfgRequest = new UnityWebRequest(URL + CFG_FULL_PATH);
            downCfgRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return downCfgRequest.Send();
            if (string.IsNullOrEmpty(downCfgRequest.error) == false)
            {
                Debug.LogWarning("下载remote cfg出错:" + downCfgRequest.error);
                remoteCfg = CFG_ERROR;
                yield break;
            }
            remoteCfg = downCfgRequest.downloadHandler.text;
        }

        //下载并保存资源 通过路径list 
        IEnumerator DownloadByPathList(List<string> list)
        {

            if (list == null || list.Count <= 0) yield break;


            for (int i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrEmpty(list[i]) == true) continue;
                UpdateTools.CreatDirectory(LOCAL_URL + list[i], true);
                yield return StartCoroutine(DownloadAndSaveByFile(list[i]));
            }
        }
        //下载并保存资源 通过路径
        IEnumerator DownloadAndSaveByFile(string path)
        {
            print(path + "下载中。。。");
            UnityWebRequest downAssetRequest = new UnityWebRequest(URL + path);
            downAssetRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return downAssetRequest.Send();
            if (string.IsNullOrEmpty(downAssetRequest.error) == false)
            {
                Debug.LogError("下载资源出错:" + downAssetRequest.error + "\n  path:" + URL + path);
                yield break;
            }
            FileStream fs = new FileStream(LOCAL_URL + path, FileMode.Create);
            byte[] data = downAssetRequest.downloadHandler.data;
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
            print(path + "下载完成");
        }

        //加载资源
        private IEnumerator LoadAssetFromLoacl()
        {
            try
            {
                if (File.Exists(LOCAL_URL + AB_PATH + @"\" + AB_PATH) == false) yield break;
                AssetBundle mainAb = AssetBundle.LoadFromFile(LOCAL_URL + AB_PATH + @"\" + AB_PATH);
                AssetBundleManifest manifest = mainAb.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                foreach (string abStr in manifest.GetAllAssetBundles())
                {
                    AssetBundle abtemp = AssetBundle.LoadFromFile(LOCAL_URL + AB_PATH + @"\" + abStr);
                    GameObject go = abtemp.LoadAsset<GameObject>(abtemp.GetAllAssetNames()[0]);
                    GoDict.Add(go.name, go);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }

        }

        //通过物体名字（在预制体面板的名字） 获取资源
        public GameObject GetGameObject(string name)
        {
            if (GoDict.ContainsKey(name) == false) return null;

            return GoDict[name];
        }
    }


}