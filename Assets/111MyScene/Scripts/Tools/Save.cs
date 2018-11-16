using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;
using XLua;
namespace GameTools
{
    [Hotfix]
    public class Save
    {
        public int mScore;//分数
        public Vector2 mPlanePos = new Vector2();//飞机位置
        public Vector2 mCameraPos = new Vector2();
        public List<Vector2> brickPosList = new List<Vector2>();//障碍物位置

        public Save()
        {

        }
        public Save(int score, Vector2 planePos, Vector2 cameraPos, List<Vector2> list)
        {
            this.mScore = score;
            mPlanePos = planePos;
            mCameraPos = cameraPos;
            brickPosList = list;

        }

        public string GetSaveStr()
        {
            //StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append(mScore.ToString());
            //stringBuilder.AppendFormat("#",Convert.ToString(mPlanePos.x.ToString("0.00")), Convert.ToString(mPlanePos.y));
            //stringBuilder.AppendFormat("#", Convert.ToString(mCameraPos.x), Convert.ToString(mCameraPos.x));
            //foreach (Vector2 pos in this.brickPosList)
            //{
            //    stringBuilder.AppendFormat("#", pos.x.ToString(), pos.y.ToString());
            //}
            //stringBuilder.Remove(stringBuilder.Length - 1, 1);
            //Debug.Log("存档："+ Convert.ToString(mPlanePos.x) + "               "+stringBuilder.ToString());
            //return stringBuilder.ToString();
            string stringBuilder = "";
            stringBuilder += Convert.ToString(mScore);
            stringBuilder += "#" + Convert.ToString(mPlanePos.x.ToString("0.00")) + "#" + Convert.ToString(mPlanePos.y);
            stringBuilder += "#" + Convert.ToString(mCameraPos.x.ToString("0.00")) + "#" + Convert.ToString(mCameraPos.y);
            foreach (Vector2 pos in this.brickPosList)
            {
                stringBuilder += "#" + pos.x.ToString() + "#" + pos.y.ToString();
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            Debug.Log("存档：" + Convert.ToString(mPlanePos.x) + "               " + stringBuilder.ToString());
            return stringBuilder.ToString();
        }
        public void GetSaveClass(string classStr)
        {

            Debug.Log("加载存档" + classStr);
            string[] strArray = classStr.Split('#');
            this.mScore = int.Parse(strArray[0]);
            this.mPlanePos.x = Convert.ToSingle(strArray[1]);
            this.mPlanePos.y = Convert.ToSingle(strArray[2]);
            this.mCameraPos.x = Convert.ToSingle(strArray[3]);
            this.mCameraPos.y = Convert.ToSingle(strArray[4]);
            for (int i = 0; 2 * i < (strArray.Length - 5); i++)
            {
                Vector2 tempPos = new Vector2(Convert.ToSingle(strArray[i * 2 + 5]), Convert.ToSingle(strArray[i * 2 + 6]));
                Debug.Log(tempPos);
                brickPosList.Add(tempPos);
            }
        }
        public int GetScore()
        {
            return mScore;
        }
        public Vector2 GetPlanePos()
        {
            return mPlanePos;
        }
        public Vector2 GetCameraPos()
        {
            return mCameraPos;
        }
        public List<Vector2> GetbrickPosList()
        {
            return brickPosList;
        }
        public override string ToString()
        {
            return string.Format("save：{0}   {1}   {2}   {3}", this.mScore, this.mPlanePos, this.mCameraPos, this.brickPosList[0]);
        }
        public SaveJson SaveToSavejson()
        {
            SaveJson saveJson = new SaveJson();
            saveJson.brickPosListX = new List<double>();
            saveJson.brickPosListY = new List<double>();
            saveJson.mScore = this.mScore;
            saveJson.mPlanePosX = this.mPlanePos.x;
            saveJson.mPlanePosY = this.mPlanePos.y;
            saveJson.mCameraPosX = this.mCameraPos.x;
            saveJson.mCameraPosY = this.mCameraPos.y;
            for (int i = 0; i < this.brickPosList.Count; i++)
            {
                saveJson.brickPosListX.Add(this.brickPosList[0].x);
                saveJson.brickPosListY.Add(this.brickPosList[0].y);
            }
            return saveJson;
        }
    }
    [Serializable]
    public class SaveJson
    {
        public int mScore;//分数
        public double mPlanePosX { set; get; }//飞机位置x
        public double mPlanePosY { set; get; }//飞机位置y
        public double mCameraPosX { set; get; }
        public double mCameraPosY { set; get; }
        public List<double> brickPosListX { set; get; }//障碍物位置
        public List<double> brickPosListY { set; get; }//障碍物位置

        public Save SavejsonToSave()
        {
            Save save = new Save();
            save.mScore = this.mScore;
            save.mPlanePos = new Vector2((float)this.mPlanePosX, (float)this.mPlanePosY);
            save.mCameraPos = new Vector2((float)this.mCameraPosX, (float)this.mCameraPosY);
            for (int i = 0; i < this.brickPosListX.Count; i++)
            {
                save.brickPosList.Add(new Vector2((float)this.brickPosListX[i], (float)this.brickPosListY[i]));
            }
            return save;
        }
    }
    ///经测试，直接用 BinaryFormatter序列化加[Serializable]的类的对象，简单的int string类型可以，但Vector2 Vector3不可以
    //[Serializable]
    //public class SaveClass
    //{
    //    public int mScore { set; get; }//分数
    //    public string mName{ set; get; }//飞机位置
    //}

}


