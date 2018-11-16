using GameAttribute;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEffect
{
    public class AddWeb_EF : MonoBehaviour
    {
        public int gunIndex;
        public int lvInWeb = 0;       //同样大小网的等级（用于计算网的颜色，威力等）
        public int damage;
        public int webTotaLv = 5;
        public float webWaitTime = 1f;
        private float colorValue = 1;
        public  void SetColor()
        {
            colorValue = (1 - lvInWeb * 1f / webTotaLv);
        }
      
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == null || collision.CompareTag("Fish") == false) return;
            print("addweb");
            //子弹销毁时实例化网
            //GameObject webGo = Instantiate(webPre, transform.position, transform.rotation);
            GameObject webGo = MainPoolManager.Instance.GetGameObject(PoolType.WEB,gunIndex);
            //改变颜色 设置位置
            webGo.GetComponent<SpriteRenderer>().color = new Color(1, colorValue, colorValue, 1);
            webGo.transform.position = transform.position;
            webGo.transform.rotation = transform.rotation;
            //添加网的伤害属性
            webGo.GetComponent<WebArribute>().damage = damage;
            //将网放入子弹的父物体内
            //webGo.transform.SetParent(transform.parent);//改为放入对象池
            //为网添加定时销毁效果
            AutoDestroy_EF DesEF = webGo.GetComponent<AutoDestroy_EF>();
            if (DesEF == null)
            {
                DesEF = webGo.AddComponent<AutoDestroy_EF>();
            }
            DesEF.time = webWaitTime;
        }

    }

}