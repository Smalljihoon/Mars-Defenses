using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    private static UIManager instance;  // UI 매니저 싱글톤
    public static UIManager Instance
    {
        get { return instance; }
    }

    [Header("UI_Player")]   // 우측하단 플레이어 스탯 상태
    [SerializeField] Image Player_FillHpbar = null;
    [SerializeField] Image Base_FillHpbar = null;
    [SerializeField] TMP_Text P_hp_value = null;
    [SerializeField] TMP_Text B_hp_value = null;
    [SerializeField] TMP_Text Money = null;
    [SerializeField] TMP_Text RemainCurBullet = null;

    [Header("UI_Monster")]  // 조준시 상단 UI 체력상태
    public Image Status_hpbar = null;
    public TMP_Text Object_name = null;
    public TMP_Text Hp_num = null;

    [Header("UI_situation")]
    public bool isActive = false;
    public GameObject Status = null;
    public GameObject EnforceWindow = null;
    public GameObject GameOverPanel =null;
    public TMP_Text R_ATK = null;
    public TMP_Text L_ATK = null;
    public TMP_Text M_Speed = null;
    public TMP_Text Magazine = null;
    public TMP_Text Max_hp = null;
    public TMP_Text Hp_heal = null;
    public TMP_Text CurRound = null;
    public TMP_Text CurKill = null;
    public TMP_Text EndTime = null;

    [Header("UI Option")]
    public GameObject OptionBanner = null;
    public GameObject EndBanner = null;
    public GameObject AudioBanner = null;
    //private bool isOption = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Status.SetActive(false);
    }

    // esc 오디오 배너
    public void AudioOnActive()
    {
        OptionBanner.SetActive(false);
        AudioBanner.SetActive(true);
    }

    // 오디오 배너 퇴장
    public void AudioExitButton()
    {
        AudioBanner.SetActive(false);
        OptionBanner.SetActive(true);
    }

    // esc 눌렀을 때 배너
    public void OptionOnActice()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OptionBanner.SetActive(true);
            isActive = true;
            Player.Instance.isAttack = false;
            //isOption= true;
        }
    }

    // 게임종료 버튼
    public void EndButton()
    {
        OptionBanner.SetActive(false);
        EndBanner.SetActive(true);
    }

    // 게임종료 yes
    public void EndYesButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // 게임종료 no
    public void EndNoButton()
    {
        EndBanner.SetActive(false);
        OptionBanner.SetActive(true);
    }

    // 나가기 버튼
    public void ExitButton()
    {
        OptionBanner.SetActive(false);
        isActive = false;
        Player.Instance.isAttack = true;
        //isOption= false;
    }

    // 강화 상점 활성화
    public void ShopOnActive()
    {
        EnforceWindow.SetActive(true);
        isActive = true;
        Player.Instance.isAttack = false;
    }

    // 강화 종료 퇴장
    public void ShopExitOnClicks()
    {
        EnforceWindow.SetActive(false);
        isActive = false;
        Player.Instance.isAttack = true;
    }

    // 우측 상단 재화 표시 UI
    public void Property_UI()
    {
        string curproperty = GameManager.Instance.Property.ToString();
        Money.text = curproperty;
    }

    // 우측 하단 탄창 UI
    public void Bullet_UI()
    {
        string curBullet = Player.Instance.cur_magazine.ToString();
        RemainCurBullet.text = curBullet;
    }

    // 우측 하단 상태창 UI
    public void Status_UI()
    {
        if (!Player.Instance.isNull)
        {
            Status.SetActive(true);
            Object_name.text = Player.Instance.hitname;
            Hp_num.text = Player.Instance.hithp;
            Status_hpbar.fillAmount = Player.Instance.hit_curhp / Player.Instance.hit_maxhp;
        }
        else
        {
            Status.SetActive(false);
        }
    }

    // 상태창에 표시할 값 형변환
    public void Stat_Info()
    {
        R_ATK.text = Player.Instance.RifleATK.ToString();
        L_ATK.text = Player.Instance.LauncherATK.ToString();
        M_Speed.text = Player.Instance.MoveSpeed.ToString();
        Magazine.text = Player.Instance.max_magazine.ToString();
        Max_hp.text = Player.Instance.maxHP.ToString();
        Hp_heal.text = Player.Instance.healAmount.ToString();
    }

    // 기지 체력 UI
    public void Base_HpbarUpdate()
    {
        Base_FillHpbar.fillAmount = Base.Instance.curBaseHp / Base.Instance.MaxbaseHP;
        B_hp_value.text = Base.Instance.curBaseHp.ToString();
    }

    // 플레이어 체력 UI
    public void Player_HpbarUpdate()
    {
        Player_FillHpbar.fillAmount = Player.Instance.curHP / Player.Instance.maxHP;
        P_hp_value.text = Player.Instance.curHP.ToString();
    }

    // 라운드, 킬 형변환
    public void GameCount()
    {
        CurRound.text = GameManager.Instance.Round.ToString();
        CurKill.text = GameManager.Instance.Kill.ToString();
    }
}
