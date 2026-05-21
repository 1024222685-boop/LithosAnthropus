using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_DeathScreen : MonoBehaviour
{
    public void RestartTheGameBTN()
    {
        AudioManager.instance.PlayGlobalSFX("button_click");

        GameManager.instance.ChangeScene("game", RespawnType.NoneSpecific);
    }

    public void CheckpointBTN()
    {
        AudioManager.instance.PlayGlobalSFX("button_click");

        GameManager.instance.RestartScene();
    }

    public void MainMenuBTN()
    {
        AudioManager.instance.PlayGlobalSFX("button_click");

        GameManager.instance.ChangeScene("MainMenu", RespawnType.NoneSpecific);
    }
}
