using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager instance;
    private Vector3 lastPlayerPostion;

    private string lastScenePlayed;
    private bool dataLoaded;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //public void SetLastPlayerPosition(Vector3 position) => lastPlayerPostion = position;

    public void ContinuePlay()
    {
        ChangeScene(lastScenePlayed, RespawnType.NoneSpecific);
    }

    public void RestartScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        ChangeScene(sceneName, RespawnType.NoneSpecific);
    }

    private IEnumerator ChangeSceneCo(string sceneName, RespawnType respawnType)
    {
        UI_FadeScreen fadeScreen = FindFadeScreenUI();

        fadeScreen.DoFadeOut();
        yield return fadeScreen.fadeEffectCo;

        SceneManager.LoadScene(sceneName);

        dataLoaded = false;
        yield return null;

        while (dataLoaded == false)
        {
            yield return null;
        }

        fadeScreen = FindFadeScreenUI();
        fadeScreen.DoFadeIn();

        yield return null;

        Player player = Player.instance;

        if (player == null)
            yield break;

        Vector3 position = GetNewPlayerPosition(respawnType);

        if (position != Vector3.zero)
            player.SwapPlayer(position);
    }

    private UI_FadeScreen FindFadeScreenUI()
    {
        if (UI.instance != null)
            return UI.instance.fadeScreenUI;
        else
            return FindFirstObjectByType<UI_FadeScreen>();
    }

    public void ChangeScene(string sceneName, RespawnType respawnType)
    {
        Time.timeScale = 1;
        SaveManager.instance.SaveGame();
        StartCoroutine(ChangeSceneCo(sceneName, respawnType));
    }

    private Vector3 GetNewPlayerPosition(RespawnType type)
    {
        if (type == RespawnType.Portal)
        {
            bool isInTargetScene = SceneManager.GetActiveScene().name == "Level_Rainbow 's place";

            if (isInTargetScene)
            {
                GameObject respawn = GameObject.Find("PortalRespawn");
                if (respawn != null)
                {
                    return respawn.transform.position;
                }
                return new Vector3(-57.05f, -1.51f, 0);
            }
            else
            {
                if (Object_Portal.instance == null)
                {
                    return Vector3.zero;
                }

                Object_Portal portal = Object_Portal.instance;
                Vector3 position = portal.GetPosition();
                portal.SetTrigger(false);
                portal.DisableIfNeeded();
                return position;
            }
        }


        if (type == RespawnType.NoneSpecific)
        {
            var data = SaveManager.instance.GetGameData();
            var checkpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);
            var unlcokedCheckpoints = checkpoints
                .Where(cp => data.unlockedCheckpoints.TryGetValue(cp.GetCheckpointId(), out bool unlocked) && unlocked)
                .Select(cp => cp.GetPosition())
                .ToList();

            var enterTransferpoints = FindObjectsByType<Object_Transferpoint>(FindObjectsSortMode.None)
                .Where(wp => wp.GetTransferpointType() == RespawnType.Enter)
                .Select(wp => wp.GetPositionAndSetTiggerFalse())
                .ToList();

            var selectedPostions = unlcokedCheckpoints.Concat(enterTransferpoints).ToList();

            if (selectedPostions.Count == 0)
                return Vector3.zero;

            return selectedPostions.
                OrderBy(position => Vector3.Distance(position, lastPlayerPostion))
                .First();
        }

        return GetTransferpointPosition(type);
    }

    private Vector3 GetTransferpointPosition(RespawnType type)
    {
        var transferpoints = FindObjectsByType<Object_Transferpoint>(FindObjectsSortMode.None);

        foreach (var point in transferpoints)
        {
            if (point.GetTransferpointType() == type)
                return point.GetPositionAndSetTiggerFalse();
        }

        return Vector3.zero;
    }

    public void LoadData(GameData data)
    {
        lastScenePlayed = data.lastScenePlayed;
        lastPlayerPostion = data.lastPlayerPosition;

        if (string.IsNullOrEmpty(lastScenePlayed))
            lastScenePlayed = "game";

        dataLoaded = true;
    }

    public void SaveData(ref GameData data)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "MainMenu")
            return;

        if (Player.instance != null && !Player.instance.health.isDead)
        {
            data.lastPlayerPosition = Player.instance.transform.position;
        }

        data.lastScenePlayed = currentScene;
        dataLoaded = false;
    }
}
