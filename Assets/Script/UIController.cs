using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    // private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreValue;
    [SerializeField] private UnityEngine.UI.Image healthBar;
    [SerializeField] private UnityEngine.UI.Image crossHair;
    [SerializeField] private TextMeshProUGUI ammoValue; // 新增
    [SerializeField] private TextMeshProUGUI clipsValue; // 新增
    [SerializeField] private OptionsPopup optionsPopup;
    [SerializeField] private SettingsPopup settingsPopup;
    [SerializeField] private GameOverPopup gameOverPopup;
    [SerializeField] private StartGamePopup startGamePopup; // 新增
    [SerializeField] private TaskCompletedPopup taskCompletedPopup; // 新增



    private int popupsActive = 0;
    private bool isGameActive = true;

    void Awake()
    {

        Messenger<float>.AddListener(GameEvent.HEALTH_CHANGED, OnHealthChanged);
        Messenger.AddListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        Messenger.AddListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
        Messenger.AddListener(GameEvent.GAME_ACTIVE, OnGameActive);
        Messenger.AddListener(GameEvent.GAME_INACTIVE, OnGameInactive);
        Messenger<int>.AddListener(GameEvent.AMMO_CHANGED, OnAmmoChanged); // 新增
        Messenger<int>.AddListener(GameEvent.CLIPS_CHANGED, OnClipsChanged); // 新增
        Messenger.AddListener(GameEvent.START_GAME, OnStartGame); // 新增
        Messenger.AddListener(GameEvent.TASK_COMPLETED, OnTaskCompleted); // 新增
    }

    void OnDestroy()
    {
        Messenger<float>.RemoveListener(GameEvent.HEALTH_CHANGED, OnHealthChanged);
        Messenger.RemoveListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        Messenger.RemoveListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
        Messenger.RemoveListener(GameEvent.GAME_ACTIVE, OnGameActive);
        Messenger.RemoveListener(GameEvent.GAME_INACTIVE, OnGameInactive);
        Messenger<int>.RemoveListener(GameEvent.AMMO_CHANGED, OnAmmoChanged); // 新增
        Messenger<int>.RemoveListener(GameEvent.CLIPS_CHANGED, OnClipsChanged); // 新增
        Messenger.RemoveListener(GameEvent.START_GAME, OnStartGame); // 新增
        Messenger.RemoveListener(GameEvent.TASK_COMPLETED, OnTaskCompleted); // 新增
    }

    void Start()
    {
        // UpdateScore(score);
        healthBar.fillAmount = 1;
        healthBar.color = Color.green;
        SetGameActive(false);
        startGamePopup.Open(); // 显示开始游戏弹窗
        UpdateAmmoCount(10); // 初始化弹药显示
        UpdateClipCount(3); // 初始化弹夹显示
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPopup.IsActive())
            {
                settingsPopup.Close();
                SetGameActive(true);
            }
            else if (optionsPopup.IsActive())
            {
                optionsPopup.Close();
                SetGameActive(true);
            }
            else
            {
                SetGameActive(false);
                optionsPopup.Open();
            }
        }
    }

    private void OnPopupOpened()
    {
        if (popupsActive == 0)
        {
            SetGameActive(false);
            Messenger.Broadcast(GameEvent.MUTE_ALL_SOUNDS);
        }
        popupsActive++;
    }

    private void OnPopupClosed()
    {
        popupsActive--;
        if (popupsActive == 0)
        {
            SetGameActive(true);
            Messenger.Broadcast(GameEvent.UNMUTE_ALL_SOUNDS);
        }
    }

    private void OnGameActive()
    {
        SetGameActive(true);
    }

    private void OnGameInactive()
    {
        SetGameActive(false);
    }

    public void OnHealthChanged(float healthPercentage)
    {
        UpdateHealth(healthPercentage);
    }

    public void UpdateHealth(float healthPercentage)
    {
        healthBar.fillAmount = healthPercentage;
        healthBar.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }

    public void UpdateScore(int newScore)
    {
        scoreValue.text = newScore.ToString();
    }

    private void OnAmmoChanged(int ammo)
    {
        UpdateAmmoCount(ammo);
    }

    private void OnClipsChanged(int clips)
    {
        UpdateClipCount(clips);
    }

    public void UpdateAmmoCount(int ammo)
    {
        ammoValue.text = ammo.ToString();
    }

    public void UpdateClipCount(int clips)
    {
        clipsValue.text = clips.ToString();
    }

    // public void SetGameActive(bool active)
    // {
    //     if (isGameActive == active) return;
    //     isGameActive = active;
    //     if (active)
    //     {
    //         Time.timeScale = 1;
    //         Cursor.lockState = CursorLockMode.Locked;
    //         Cursor.visible = false;
    //         crossHair.gameObject.SetActive(true);
    //         Messenger.Broadcast(GameEvent.GAME_ACTIVE);
    //     }
    //     else
    //     {
    //         Time.timeScale = 0;
    //         Cursor.lockState = CursorLockMode.None;
    //         Cursor.visible = true;
    //         crossHair.gameObject.SetActive(false);
    //         Messenger.Broadcast(GameEvent.GAME_INACTIVE);
    //     }
    // }

    public void SetGameActive(bool active)
    {
        Debug.Log("SetGameActive called with active = " + active);
        if (isGameActive == active)
        {
            Debug.Log("Game state is already " + active);
            return;
        }
        isGameActive = active;
        if (active)
        {
            Debug.Log("Resuming game");
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            crossHair.gameObject.SetActive(true);
            Messenger.Broadcast(GameEvent.GAME_ACTIVE);
        }
        else
        {
            Debug.Log("Pausing game");
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            crossHair.gameObject.SetActive(false);
            Messenger.Broadcast(GameEvent.GAME_INACTIVE);
        }
    }

    public void ShowGameOverPopup()
    {
        if (!gameOverPopup.IsActive())
        {
            gameOverPopup.Open();
            Messenger.Broadcast(GameEvent.GAME_INACTIVE); // 新增
            Messenger.Broadcast(GameEvent.MUTE_ALL_SOUNDS); // 新增
        }
    }
    private void OnStartGame() // 新增
    {
        SetGameActive(true);
        Messenger.Broadcast(GameEvent.UNMUTE_ALL_SOUNDS);
    }
    private void OnTaskCompleted()
    {
        Debug.Log("Task Completed!");
        if (!taskCompletedPopup.IsActive())
        {
            taskCompletedPopup.Open();
            Messenger.Broadcast(GameEvent.GAME_INACTIVE);
            Messenger.Broadcast(GameEvent.MUTE_ALL_SOUNDS);
        }
    }

}
