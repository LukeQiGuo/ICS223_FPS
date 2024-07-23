using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGamePopup : BasePopup
{
    public void OnStartGameButton()
    {
        Close();
        Messenger.Broadcast(GameEvent.START_GAME);
    }
}
