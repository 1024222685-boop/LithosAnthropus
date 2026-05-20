using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    private void Start()
    {
        transform.root.GetComponentInChildren<UI_FadeScreen>().DoFadeIn();
        AudioManager.instance.StartBGM("playlist_mainMenu");
    }

    public void PlayBTN()
    {
        AudioManager.instance.PlayGlobalSFX("button_click");

        GameData data = SaveManager.instance.GetGameData();

        if (!string.IsNullOrEmpty(data.lastScenePlayed))
        {
            GameManager.instance.ContinuePlay();
        }
        else
        {
            GameManager.instance.ChangeScene("game", RespawnType.Enter);
        }
    }

    public void QuitGameBTN()
    {
        Application.Quit(0);
    }
}
