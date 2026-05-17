using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_DeathScreen : MonoBehaviour
{
    public void RestartTheGameBTN()
    {
        GameManager.instance.ChangeScene("game", RespawnType.NoneSpecific);
    }

    public void CheckpointBTN()
    {
        GameManager.instance.RestartScene();
    }

    public void MainMenuBTN()
    {
        GameManager.instance.ChangeScene("MainMenu", RespawnType.NoneSpecific);
    }
}
