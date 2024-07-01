using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPopup : MonoBehaviour
{
    [SerializeField] private UIController uiController;
    [SerializeField] private SettingsPopup settingsPopup;
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void OnsettingsButton()
    {
        Debug.Log("settings clicks");
        Close();
        settingsPopup.Open();
    }

    public void OnExitGameButton()
    {
        Debug.Log("exit game");
        Application.Quit();
    }

    public void onReturnToGameButton()
    {
        Debug.Log("return to game");
        Close();
        uiController.SetGameActive(true);
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
