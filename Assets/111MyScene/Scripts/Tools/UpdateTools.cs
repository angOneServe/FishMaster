using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace GameTools
{
    public static class UpdateTools
    {
        //updateTools

        //需要的路径 ab包根路径如"AssetBundles"       lua文件根路径如"LuaFiles"         配置文件固定相对路径如"Cfg/cfg.txt"    url固定路径如"http://local/"

        //根据文件路径的到MD5值
        private static string GetMD5ByFilepath(string filepath)
        {
            if (File.Exists(filepath) == false) return null;    //文件不存在返回空
            MD5 md5 = new MD5CryptoServiceProvider();
            //每个字节转为16进制输出
            StringBuilder md5Str = new StringBuilder();
            byte[] md5Bytes = md5.ComputeHash(File.ReadAllBytes(filepath));
            for (int i = 0; i < md5Bytes.Length; i++)
            {
                md5Str.Append(md5Bytes[i].ToString("x2"));
            }
            return md5Str.ToString();
        }

        //根据路径(文件夹不含“.”)创建目录
        public static void CreatDirectory(string filepath, bool hasFilename = false)
        {
            //检测路径
            Debug.Log(filepath);
            if (string.IsNullOrEmpty(filepath) == true) return;

            string[] pathParts = filepath.Split('\\');   //这里要用\,用\\表示
            int pathPartNum = pathParts.Length;
            //有文件名就去掉一段

            if (hasFilename == true)
            {
                pathPartNum = pathPartNum - 1;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < pathPartNum; i++)
            {
                builder.Append(pathParts[i]).Append("/");
                if (Directory.Exists(builder.ToString())) continue;//有这个目录,忽略创建
                Directory.CreateDirectory(builder.ToString());
            }
        }

        //根据路径生成配置文件（编辑器使用）（前面是ab包，后面是lua文件）
        public static void CreatCfg(string localtoRootUrl, string abRootPath, string luaRootPath, string cfgFullPath)
        {
            //检查路径
            if (Directory.Exists(abRootPath) == false) CreatDirectory(abRootPath);
            if (Directory.Exists(luaRootPath) == false) CreatDirectory(luaRootPath);
            if (Directory.Exists(cfgFullPath) == false) CreatDirectory(cfgFullPath, true);

            //得到所有ab包路径 所有lua文件路径
            string[] abFullPaths = Directory.GetFiles(localtoRootUrl + abRootPath, "*", SearchOption.AllDirectories);
            string[] luaFullPath = Directory.GetFiles(localtoRootUrl + luaRootPath, "*", SearchOption.AllDirectories);
            string temp;
            int preLen = localtoRootUrl.Length;
            //生成配置文件
            StringBuilder cfgBuilder = new StringBuilder();
            if (abFullPaths != null || abFullPaths.Length > 0)
            {
                for (int i = 0; i < abFullPaths.Length; i++)
                {
                    if (string.IsNullOrEmpty(abFullPaths[i]) == true) continue;
                    temp = abFullPaths[i].Substring(preLen);
                    cfgBuilder.Append(temp).Append(",").Append(GetMD5ByFilepath(abFullPaths[i])).Append("\n");
                }
            }

            if (luaFullPath != null || luaFullPath.Length > 0)
            {
                for (int i = 0; i < luaFullPath.Length; i++)
                {
                    if (string.IsNullOrEmpty(luaFullPath[i]) == true) continue;
                    temp = luaFullPath[i].Substring(preLen);
                    cfgBuilder.Append(temp).Append(",").Append(GetMD5ByFilepath(luaFullPath[i])).Append("\n");
                }

                //保存配置文件
                CreatDirectory(localtoRootUrl + cfgFullPath, true);
                FileStream fs = File.Create(localtoRootUrl + cfgFullPath);
                byte[] data = System.Text.Encoding.UTF8.GetBytes(cfgBuilder.ToString());
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();

            }
        }
        //根据cfg得到字典
        public static Dictionary<string, string> GetDictByCfg(string cfg)
        {
            //检测cfg
            if (string.IsNullOrEmpty(cfg) == true) return null;

            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] items = cfg.Split('\n');
            for (int i = 0; i < items.Length; i++)
            {
                if (string.IsNullOrEmpty(items[i]) == true) continue;

                string[] parts = items[i].Split(',');
                if (parts.Length != 2) continue;
                dict.Add(parts[0], parts[1]);
            }
            return dict;
        }

        //比较localCfg和remoteCfg得到更新List
        public static List<string> CampareCfg(Dictionary<string, string> localDict, Dictionary<string, string> remoteDict)
        {

            List<string> pathList = new List<string>();
            //检测传入的字典
            if (remoteDict == null || remoteDict.Keys.Count <= 0) return null;
            if (localDict == null || localDict.Keys.Count <= 0)
            {
                foreach (string key in remoteDict.Keys)
                {
                    pathList.Add(key);
                }
                return pathList;
            }

            //比较字典
            foreach (string filename in remoteDict.Keys)
            {
                if (string.IsNullOrEmpty(filename) == true) continue;

                if (localDict.ContainsKey(filename) == false)
                {
                    pathList.Add(filename);
                }
                else
                {
                    if (localDict[filename].Equals(remoteDict[filename]) == false)
                    {
                        pathList.Add(filename);
                    }
                }
            }
            //返回list
            return pathList;
        }

    }


}