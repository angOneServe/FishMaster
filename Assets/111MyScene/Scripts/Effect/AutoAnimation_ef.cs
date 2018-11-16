using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEffect
{
    public class AutoAnimation_ef : MonoBehaviour
    {

        public Sprite[] sprites;
        public float repeatRate = 0.5f;

        private SpriteRenderer spriteRenderer;
        private int index = 0;
        private int length;
        private void Start()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            length = sprites.Length;
            if (length <= 0) return;

            InvokeRepeating("PlayAnimation", 0, repeatRate);
        }

        void PlayAnimation()
        {
            spriteRenderer.sprite = sprites[index];
            index = (index + 1) % length;
        }
    }

}