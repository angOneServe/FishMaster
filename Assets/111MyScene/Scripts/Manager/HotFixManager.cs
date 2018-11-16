using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using XLua;
namespace Manager
{
    public class HotFixManager : MonoBehaviour
    {
        LuaEnv luaEnv;
        LuaFunction func;
        // 启动热更新的lua脚本
        public void Initial()
        {
            luaEnv = new LuaEnv();
            luaEnv.AddLoader(LoaderfromLuaFile);
            luaEnv.DoString(@"require 'start'");
        }
        //monobehaviuour函数
        private void OnDisable()
        {
            luaEnv.DoString(@"require 'end'");
        }
        private void OnDestroy()
        {
            if(luaEnv!=null)
            luaEnv.Dispose();
        }

        //自定义函数

        //自定加载器
        private byte[] LoaderfromLuaFile(ref string filename)
        {
            string fullpath = UpdateManager.LOCAL_URL + UpdateManager.LUA_PATH + @"\" + filename + ".lua";
            print(fullpath);
            if (File.Exists(fullpath))
            {
                return File.ReadAllBytes(fullpath);
            }
            return null;
        }

    }


}
