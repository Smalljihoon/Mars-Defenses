using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    private static UIManager instance;  // UI �Ŵ��� �̱���
    public static UIManager Instance
    {
        get { return instance; }
    }

    [Header("UI_Player")]   // �����ϴ� �÷��̾� ���� ����
    [SerializeField] Image Player_FillHpbar = null;
    [SerializeField] Image Base_FillHpbar = null;
    [SerializeField] TMP_Text P_hp_value = null;
    [SerializeField] TMP_Text B_hp_value = null;
    [SerializeField] TMP_Text Money = null;
    [SerializeField] TMP_Text RemainCurBullet = null;

    [Header("UI_Monster")]  // ���ؽ� ��� UI ü�»���
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

    // esc ����� ���
    public void AudioOnActive()
    {
        OptionBanner.SetActive(false);
        AudioBanner.SetActive(true);
    }

    // ����� ��� ����
    public void AudioExitButton()
    {
        AudioBanner.SetActive(false);
        OptionBanner.SetActive(true);
    }

    // esc ������ �� ���
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

    // �������� ��ư
    public void EndButton()
    {
        OptionBanner.SetActive(false);
        EndBanner.SetActive(true);
    }

    // �������� yes
    public void EndYesButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // �������� no
    public void EndNoButton()
    {
        EndBanner.SetActive(false);
        OptionBanner.SetActive(true);
    }

    // ������ ��ư
    public void ExitButton()
    {
        OptionBanner.SetActive(false);
        isActive = false;
        Player.Instance.isAttack = true;
        //isOption= false;
    }

    // ��ȭ ���� Ȱ��ȭ
    public void ShopOnActive()
    {
        EnforceWindow.SetActive(true);
        isActive = true;
        Player.Instance.isAttack = false;
    }

    // ��ȭ ���� ����
    public void ShopExitOnClicks()
    {
        EnforceWindow.SetActive(false);
        isActive = false;
        Player.Instance.isAttack = true;
    }

    // ���� ��� ��ȭ ǥ�� UI
    public void Property_UI()
    {
        string curproperty = GameManager.Instance.Property.ToString();
        Money.text = curproperty;
    }

    // ���� �ϴ� źâ UI
    public void Bullet_UI()
    {
        string curBullet = Player.Instance.cur_magazine.ToString();
        RemainCurBullet.text = curBullet;
    }

    // ���� �ϴ� ����â UI
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

    // ����â�� ǥ���� �� ����ȯ
    public void Stat_Info()
    {
        R_ATK.text = Player.Instance.RifleATK.ToString();
        L_ATK.text = Player.Instance.LauncherATK.ToString();
        M_Speed.text = Player.Instance.MoveSpeed.ToString();
        Magazine.text = Player.Instance.max_magazine.ToString();
        Max_hp.text = Player.Instance.maxHP.ToString();
        Hp_heal.text = Player.Instance.healAmount.ToString();
    }

    // ���� ü�� UI
    public void Base_HpbarUpdate()
    {
        Base_FillHpbar.fillAmount = Base.Instance.curBaseHp / Base.Instance.MaxbaseHP;
        B_hp_value.text = Base.Instance.curBaseHp.ToString();
    }

    // �÷��̾� ü�� UI
    public void Player_HpbarUpdate()
    {
        Player_FillHpbar.fillAmount = Player.Instance.curHP / Player.Instance.maxHP;
        P_hp_value.text = Player.Instance.curHP.ToString();
    }

    // ����, ų ����ȯ
    public void GameCount()
    {
        CurRound.text = GameManager.Instance.Round.ToString();
        CurKill.text = GameManager.Instance.Kill.ToString();
    }
}
