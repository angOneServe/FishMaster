using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEffect
{
    public class Wave_EF : MonoBehaviour
    {

        private Material waveMaterial;
        private float rateTime = 0.1f;
        public Texture2D[] Waves;
        private int index = 0;
        void Start()
        {
            InvokeRepeating("ChangeWaveTexture", 0, rateTime);
            waveMaterial = GetComponent<MeshRenderer>().materials[0];
        }

        void ChangeWaveTexture()
        {
            if (index < Waves.Length)
            {
                waveMaterial.mainTexture = Waves[index];
                index = (index + 1) % Waves.Length;
            }
        }
    }

}