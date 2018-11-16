using GameTools;
using Manager;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace GameEffect
{
    /// <summary>
    /// 根据tag自动碰撞销毁
    /// </summary>
    public class AutoTriggerDestroy_EF : MonoBehaviour
    {
        public string[] checkTag = { "checkTag" };
        private void OnTriggerEnter2D(Collider2D collision)
        {
            for (int i = 0; i < checkTag.Length; i++)
            {
                if (collision.CompareTag(checkTag[i]))
                {
                    Tools.MyDestroy(this.gameObject);
                }
            }
        }

        
    }
}

