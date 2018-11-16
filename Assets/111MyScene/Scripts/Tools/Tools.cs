using UnityEngine;
//二进制方式存档需要的引用
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//数据加密与解密需要的引用
using System.Security.Cryptography;
using System.Text;
using System;
using LitJson;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using XLua;
using Manager;

namespace GameTools
{
    [Hotfix]
    public static class Tools
    {
        //public static string BASE_PATH = Application.persistentDataPath + "/SettingDirectory";
        public static string BASE_PATH = Application.persistentDataPath;
        //保存存档
        public static void SaveGameFileByBin<T>(T ob, string fileName)
        {
            //创建二进制格式化程序
            BinaryFormatter bf = new BinaryFormatter();
            //创建文件流
            //
            if (!Directory.Exists(BASE_PATH))
            {
                Directory.CreateDirectory(BASE_PATH);
            }
            FileStream fileStream = File.Create(BASE_PATH + "/" + fileName + ".dbc");
            //调用格式化程序的序列化方法，序列化对应的对象
            bf.Serialize(fileStream, ob);

            //关闭流
            fileStream.Close();

        }

        public static void SaveGameFileByLitJson<T>(T ob, string fileName)
        {
            if (!Directory.Exists(BASE_PATH))
            {
                Directory.CreateDirectory(BASE_PATH);
            }
            string str = JsonMapper.ToJson(ob);

            Debug.Log("json字符串：" + str + "    对象：" + ob);
            FileStream fs = File.Create(BASE_PATH + "/" + fileName + ".json");
            fs.Write(Encoding.UTF8.GetBytes(str), 0, Encoding.UTF8.GetBytes(str).Length);
            fs.Close();
        }
        public static void SaveGameFileByJsonUtility<T>(T ob, string fileName)
        {
            if (!Directory.Exists(BASE_PATH))
            {
                Directory.CreateDirectory(BASE_PATH);
            }
            string str = JsonUtility.ToJson(ob);

            Debug.Log("json字符串：" + str + "    对象：" + ob);
            FileStream fs = File.Create(BASE_PATH + "/" + fileName + ".jsonUnity");
            fs.Write(Encoding.UTF8.GetBytes(str), 0, Encoding.UTF8.GetBytes(str).Length);
            fs.Close();

        }
        public static void SaveGameFileByXml<T>(T ob, string fileName)
        {
            lock ("HasLock")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                FileStream file = File.Create(Tools.BASE_PATH + "/" + fileName + ".xml");
                serializer.Serialize(file, ob);
                file.Close();

            }

        }
        //加载存档
        public static T LoadGameFileByBin<T>(string fileName)
        {
            if (!File.Exists(BASE_PATH + "/" + fileName + ".dbc"))
            {
                Debug.Log("the file is not exited");
                return default(T);
            }
            Debug.Log("二进制存档加载中..");
            //创建一个二进制格式化程序
            BinaryFormatter bf = new BinaryFormatter();
            //打开一个文件流
            FileStream fs = File.Open(BASE_PATH + "/" + fileName + ".dbc", FileMode.Open);
            //调用格式化程序的反序列化方法，将文件流转化为 对应的类型
            T GameFile = (T)bf.Deserialize(fs);
            fs.Close();
            Debug.Log("二进制存档加载完成");
            return GameFile;

        }

        public static T LoadGameFileByJson<T>(string fileName)
        {
            if (!File.Exists(BASE_PATH + "/" + fileName + ".json"))
            {
                Debug.Log("the file is not exited");
                return default(T);
            }
            Debug.Log("litjson存档加载中..");
            T gamefile = JsonMapper.ToObject<T>(File.ReadAllText(BASE_PATH + "/" + fileName + ".json"));
            Debug.Log("litjson文档加载完成");
            return gamefile;
        }
        public static T LoadGameFileByJsonUtility<T>(string fileName)
        {
            if (!File.Exists(BASE_PATH + "/" + fileName + ".jsonUnity"))
            {
                Debug.Log("the file is not exited");
                return default(T);
            }
            Debug.Log("JsonUtility存档加载中..");

            T gamefile = JsonUtility.FromJson<T>(File.ReadAllText(BASE_PATH + "/" + fileName + ".jsonUnity"));
            Debug.Log("JsonUtility文档加载完成");
            return gamefile;
        }
        public static T LoadGameFileByXml<T>(string fileName)
        {
            lock ("HasLock")
            {
                if (!File.Exists(BASE_PATH + "/" + fileName + ".xml"))
                {
                    Debug.Log("the file is not exited");
                    return default(T);
                }
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                FileStream fs = File.OpenRead(Tools.BASE_PATH + "/" + fileName + ".xml");

                T t = (T)serializer.Deserialize(fs);
                fs.Close();
                return t;

            }

        }
        //加密
        public static string DataJiaMi(string jiamiContent, string keyValue)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(keyValue); //将string转为byte流 encoding
                                                                    //加密格式
            RijndaelManaged jiamitool = new RijndaelManaged();
            jiamitool.Key = keyArray;
            jiamitool.Mode = CipherMode.ECB;
            jiamitool.Padding = PaddingMode.PKCS7;

            //生成加密锁
            ICryptoTransform jiamiTransform = jiamitool.CreateEncryptor();
            byte[] jiamiContenArray = UTF8Encoding.UTF8.GetBytes(jiamiContent); //将string转为byte流 encoding

            //得到加密结果
            byte[] jiamiResArray = jiamiTransform.TransformFinalBlock(jiamiContenArray, 0, jiamiContenArray.Length);

            return Convert.ToBase64String(jiamiResArray, 0, jiamiResArray.Length);//将byte流转换为string
                                                                                  //return UTF8Encoding.UTF8.GetString(jiamiResArray);
        }
        //解密
        public static string DatajieMi(string jieMiContent, string keyValue)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(keyValue);//将string转为byte流
                                                                   //加密格式
            RijndaelManaged jieMiTool = new RijndaelManaged();
            jieMiTool.Key = keyArray;
            jieMiTool.Mode = CipherMode.ECB;
            jieMiTool.Padding = PaddingMode.PKCS7;

            //生成加密锁
            ICryptoTransform jiemiTransform = jieMiTool.CreateDecryptor();
            byte[] jiamiContenArray = Convert.FromBase64String(jieMiContent);//将string转为byte流  convert
                                                                             //byte[] jiamiContenArray = UTF8Encoding.UTF8.GetBytes(jieMiContent);
                                                                             //得到解密结果
            byte[] jieMiResArray = jiemiTransform.TransformFinalBlock(jiamiContenArray, 0, jiamiContenArray.Length);
            return UTF8Encoding.UTF8.GetString(jieMiResArray);//将byte流转换为string

        }
        //设置手机的横屏显示
        public static void SetAndroidToLandscape()
        {

            //如果是手机则控制其屏幕转换 及分辨率大小
            //手机设置横屏，禁止竖屏
            Screen.fullScreen = true;

            Screen.orientation = ScreenOrientation.LandscapeLeft;
            //Screen.autorotateToLandscapeLeft = true;
            //Screen.autorotateToLandscapeRight = false;
            //Screen.autorotateToPortrait = false;
            //Screen.autorotateToPortraitUpsideDown = false;
            //手机分辨率设置 全屏设置 刷新频率设置
            Screen.SetResolution(1024, 600, true, 60);
            //游戏场景比例（手机宽/高）设置
            Camera.main.aspect = 1024 / 600;
            //管理组件初始化
        }

        //根据鼠标在屏幕的坐标，返回角色面向鼠标的角度值（赋值给角色的rotation.z）
        public static float GetDushu(Vector2 Targerpos)
        {
            //以下求的角度是以 x正向轴为起点，逆时针旋转的角度a    player的rotation值b=a-45度

            //通过正弦余弦求弧度，再转角度
            float sinx = Targerpos.x / Mathf.Sqrt(Targerpos.x * Targerpos.x + Targerpos.y * Targerpos.y);
            // float cosx = Targerpos.y / Mathf.Sqrt(Targerpos.x * Targerpos.x + Targerpos.y * Targerpos.y);
            float sinDushu = Mathf.Asin(sinx) * Mathf.Rad2Deg;
            //float cosDushu = Mathf.Acos(cosx) * Mathf.Rad2Deg;
            //Debug.Log("sinDushu:" + sinDushu);
            //  Debug.Log("cosDushu:" + Mathf.Acos(cosx));

            if (Targerpos.y > 0)
            {
                return -sinDushu;
            }
            else
            {
                return sinDushu + 180;
            }
        }
        public static float GetDushu(Vector3 Targerpos)
        {
            //以下求的角度是以 x正向轴为起点，逆时针旋转的角度a    player的rotation值b=a-45度

            //通过正弦余弦求弧度，再转角度
            float sinx = Targerpos.x / Mathf.Sqrt(Targerpos.x * Targerpos.x + Targerpos.y * Targerpos.y);
            // float cosx = Targerpos.y / Mathf.Sqrt(Targerpos.x * Targerpos.x + Targerpos.y * Targerpos.y);
            float sinDushu = Mathf.Asin(sinx) * Mathf.Rad2Deg;
            //float cosDushu = Mathf.Acos(cosx) * Mathf.Rad2Deg;
            //Debug.Log("sinDushu:" + sinDushu);
            //  Debug.Log("cosDushu:" + Mathf.Acos(cosx));

            if (Targerpos.y > 0)
            {
                return -sinDushu;
            }
            else
            {
                return sinDushu + 180;
            }
        }
        //播放音效


        public  static  void MyDestroy(GameObject go)
        {
            if (go.transform.parent != null && (go.transform.parent.name == "fish"
                       || go.transform.parent.name == "bullet"
                       || go.transform.parent.name == "web"
                       || go.transform.parent.name == "bigGold"
                       || go.transform.parent.name == "littleGold"))
            {
                MainPoolManager.Instance.DisEnablePoolGo(go.gameObject);
            }
            else
            {
                GameObject.Destroy(go.gameObject);
            }
        }

    }
}
