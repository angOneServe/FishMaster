using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
namespace GameAttribute
{
    public class Gold : MonoBehaviour
    {
        private float moveTime = 0.15f;

        void Update()
        {

            transform.position = Vector3.MoveTowards(transform.position, DataModel.Instance.GoldCollectBox.position, moveTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null && collision.name == "GoldCollectBox")
            {
                MianManger.Instance.mSoundManager.PlayAudio(SoundManager.CLICK, 0.2F);
                GameTools.Tools.MyDestroy(this.gameObject);
            }
        }
    }

}
