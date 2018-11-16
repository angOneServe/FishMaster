using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Model;
using Manager;
namespace Manager
{
    public class UIManager : BaseManager
    {

        private const string BGCanvas = "BGCanvas";
        private const string CanvasOrder200 = "CanvasOrder200";
        //bg
        private Image bg;
        //Character
        private Slider experienceBar;
        private Text level;
        private Text levelName;
        //menupanel
        private Button backButton;
        private Button settingButton;
        //Giftpanel
        private Text bigCountDownText;
        private Button GiftButton;
        //moneyPanel
        private Text moneyText;
        //Gunprice
        private Text gunPrice;

        private GameObject mutePanel;
        private RectTransform muteRect;
        private Tweener mutePanelTweener;
        //奖励的计时器
        public float bigCountDownTimer = 240;
        public float smallCountDownTimer = 60;
        //引用
        private DataModel model = DataModel.Instance;
        //初始化
        public override void MngInitial()
        {
            GetAllUi();
            UpdateUiDisplay();
            bigCountDownTimer = DataModel.bigCountDown;
            smallCountDownTimer = DataModel.smallCountDown;


        }
        public override void MngUpdate()
        {
            //如果model层有更新，就进行UI更新
            if (DataModel.Instance.uiDataNeedUpdata == true)
            {
                UpdateUiDisplay();
                //通知model层已更新
                DataModel.Instance.RespondUiUpdate();
            }
            UpdateTimer();
            UpDownGunByScrollWheel();
        }
        //获取所有ui组件
        private void GetAllUi()
        {
            bg = GameObject.Find("AllCanvas/BGCanvas/Panel").GetComponent<Image>();

            experienceBar = GameObject.Find("AllCanvas/CanvasOrder200/UI/CharacterPanel/ExperienceBar").GetComponent<Slider>();
            level = GameObject.Find("AllCanvas/CanvasOrder200/UI/CharacterPanel/Level").GetComponent<Text>();
            levelName = GameObject.Find("AllCanvas/CanvasOrder200/UI/CharacterPanel/LevelName").GetComponent<Text>();

            GameObject.Find("AllCanvas/CanvasOrder200/UI/MenuPanel/BackButton").GetComponent<Button>().onClick.AddListener(BackButtonClick);
            GameObject.Find("AllCanvas/CanvasOrder200/UI/MenuPanel/SettingButton").GetComponent<Button>().onClick.AddListener(SetMuteButtonClick);
            GameObject.Find("AllCanvas/CanvasOrder200/UI/GunPanel/UpButton").GetComponent<Button>().onClick.AddListener(UpButtonClick);
            GameObject.Find("AllCanvas/CanvasOrder200/UI/GunPanel/DownButton").GetComponent<Button>().onClick.AddListener(DownButtonClick);

            bigCountDownText = GameObject.Find("AllCanvas/CanvasOrder200/UI/GiftBox/GiftTimerText").GetComponent<Text>();
            GiftButton = bigCountDownText.gameObject.GetComponent<Button>();
            GiftButton.onClick.AddListener(GiftButtonClick);
            GiftButton.enabled = false;

            moneyText = GameObject.Find("AllCanvas/CanvasOrder200/UI/MoneyPanel/MoneyNum/Text").GetComponent<Text>();

            gunPrice = GameObject.Find("AllCanvas/CanvasOrder300/GunPrice/Text").GetComponent<Text>();

            mutePanel = GameObject.Find("AllCanvas/CanvasOrder200/UI/SettingPanel/MutePanel");
            mutePanel.transform.GetChild(0).GetComponent<Toggle>().onValueChanged.AddListener(OnMuteChange);
            muteRect = mutePanel.GetComponent<RectTransform>();
        }
        //更新ui显示
        private void UpdateUiDisplay()
        {
            //bg.sprite = DataModel.Instance.ExchangeBG();
            experienceBar.value = model.GetExpSliderValue();
            level.text = model.lv.ToString();
            levelName.text = model.GetLvName();
            moneyText.text = model.gold.ToString();
            gunPrice.text = model.GetBullectPrice().ToString();
            //
        }
        //更新计时器
        private void UpdateTimer()
        {
            if (GiftButton.enabled == false)
            {
                bigCountDownTimer -= Time.deltaTime;
                //接近整数是更新UI面板计时器显示
                if (bigCountDownTimer % 1 <= 0.1F)
                {
                    bigCountDownText.text = string.Format("{0:000}S", bigCountDownTimer);
                }
                if (bigCountDownTimer <= 0)
                {
                    bigCountDownText.text = "领取金币";
                    GiftButton.enabled = true;
                }
            }

            //smallCountDownTimer -= Time.deltaTime;


            //if (smallCountDownTimer <= 0)
            //{
            //    giftTimer.text = "领取金币";
            //    GiftButton.enabled = true;
            //}

        }
        //检测鼠标滑轮换gun
        private void UpDownGunByScrollWheel()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                UpButtonClick();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                DownButtonClick();
            }
        }
        //button事件 
        private void GiftButtonClick()
        {
            model.AddGoldExp(model.bigCountDownGold, 0);
            bigCountDownTimer = DataModel.bigCountDown;
            bigCountDownText.text = string.Format("{0:000}S", bigCountDownTimer);
            GiftButton.enabled = false;
        }
        private void BackButtonClick()
        {
            Application.Quit();
        }
        private void SetMuteButtonClick()
        {
            if (muteRect.localPosition.y > 500f)
            {
                mutePanelTweener = muteRect.DOLocalMoveY(260, 1.5f);
            }
            else
            {
                muteRect.DOLocalMoveY(710f, 1.5f);
            }
        }
        private void UpButtonClick()
        {
            MianManger.Instance.GunUpDown(false);
            UpdateUiDisplay();
        }
        private void DownButtonClick()
        {
            MianManger.Instance.GunUpDown(true);
            UpdateUiDisplay();
        }
        public void OnMuteChange(bool isOn)
        {
            MianManger.Instance.OnMuteChange(isOn);
        }

    }

}
