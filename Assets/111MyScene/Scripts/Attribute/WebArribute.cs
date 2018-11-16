using GameTools;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameAttribute
{
    public class WebArribute : MonoBehaviour
    {
        public int damage;  //网的伤害值

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null && collision.CompareTag("Fish"))
            {
                collision.GetComponent<FishAttribute>().BeHurt(damage);
            }
        }

        
    }

}
