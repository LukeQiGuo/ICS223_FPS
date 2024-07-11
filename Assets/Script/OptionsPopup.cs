using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPopup : BasePopup
{

    [SerializeField] private SettingsPopup settingsPopup;


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
        Messenger.Broadcast(GameEvent.POPUP_CLOSED);
        Messenger.Broadcast(GameEvent.GAME_ACTIVE);

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
