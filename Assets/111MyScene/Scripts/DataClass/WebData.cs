using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace GameData
{
    [Serializable]
    public class WebData
    {
        /*
        组件SpriteRender的Sprite 
	    组件CircleCollider2D的Radius
	    脚本WebArribute的Damage属性
	    脚本AutoDestroy_EF的Time
         */
        public Sprite sprite;
        public float Radius;
        public float scale;
        public float Damage;
        public float time;
    }


}