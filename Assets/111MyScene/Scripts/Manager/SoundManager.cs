using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class SoundManager : BaseManager
    {
        #region 声音名字
        public const string FIRE = "FX_发炮_01";
        public const string CHANGE_GUN = "FX_换炮_01";
        public const string GET_SCORE1 = "FX_获分01";
        public const string GET_SCORE2 = "FX_获分02";
        public const string GET_SCORE3 = "FX_获分03";
        public const string GET_GIFT = "FX_获奖声音";
        public const string FISH_BIANFU1 = "FX_角色_蝙蝠鱼_01";
        public const string FISH_BIANFU2 = "FX_角色_蝙蝠鱼_02";
        public const string FISH_JINSHA1 = "FX_角色_大金鲨_01";
        public const string FISH_JINSHA2 = "FX_角色_大金鲨_02";
        public const string FISH_YINSHA1 = "FX_角色_大银鲨_01";
        public const string FISH_YINSHA2 = "FX_角色_大银鲨_02";
        public const string FISH_DENGLONG1 = "FX_角色_灯笼鱼_01";
        public const string FISH_DENGLONG2 = "FX_角色_灯笼鱼_02";
        public const string FISH_HUSHA1 = "FX_角色_虎鲨_01";
        public const string FISH_HUSHA2 = "FX_角色_虎鲨_01";
        public const string FISH_JINQIANG1 = "FX_角色_金枪鱼_01";
        public const string FISH_JINQIANG2 = "FX_角色_金枪鱼_02";
        public const string FISH_LANSHA1 = "FX_角色_蓝鲨_01";
        public const string FISH_LANSHA2 = "FX_角色_蓝鲨_02";
        public const string FISH_WUGUI1 = "FX_角色_乌龟_01";
        public const string FISH_WUGUI2 = "FX_角色_乌龟_02";
        public const string GOLD_BIG = "FX_金币";
        public const string GOLD_LITTLE = "FX_银币_01";
        public const string BG1 = "背景乐_01";
        public const string BG2 = "背景乐_02";
        public const string BG3 = "背景乐_03";
        public const string BG4 = "背景乐_04";
        public const string CLICK = "后台按键音_01";
        #endregion
        private static SoundManager Instance_;
        public static SoundManager Instance
        {
            get
            {
                return Instance_;
            }
        }

        private bool mutestate = false;
        private AudioSource normalAudioSource;
        private AudioSource bGaudioSource;
        Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();
        //初始化
        public override void MngInitial()
        {
            Instance_ = this;
            normalAudioSource = gameObject.AddComponent<AudioSource>();
            bGaudioSource = gameObject.AddComponent<AudioSource>();
            AudioClip[] audioArray = Resources.LoadAll<AudioClip>("Sound");
            //音效加入字典
            foreach (AudioClip ac in audioArray)
            {
                if (audioDict.ContainsKey(ac.name)) continue;

                audioDict.Add(ac.name, ac);
            }
            //设置背景乐
            ChangeBgMusic("背景乐_01");
        }
        //设置静音
        public void SetMute(bool mute)
        {
            mutestate = mute;
            normalAudioSource.mute = mute;
            bGaudioSource.mute = mute;
        }
        //播放短暂audio
        public void PlayAudio(string audioclipName, float volume = 1)
        {
            if (mutestate) return;
            //if (normalAudioSource.isPlaying == true) return;
            if (audioDict.ContainsKey(audioclipName) == false) return;
            normalAudioSource.PlayOneShot(audioDict[audioclipName], volume);
        }
        //改变背景乐
        public void ChangeBgMusic(string bgAudioName)
        {
            if (mutestate) return;
            if (audioDict.ContainsKey(bgAudioName) == false) return;
            bGaudioSource.clip = audioDict[bgAudioName];
            if (bGaudioSource.isPlaying == false)
            {
                bGaudioSource.Play();
            }
        }
    }

}