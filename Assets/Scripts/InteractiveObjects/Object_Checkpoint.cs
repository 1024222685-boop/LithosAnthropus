using UnityEngine;

public class Object_Checkpoint : MonoBehaviour, ISaveable
{
    private Player player;
    private Object_Checkpoint[] allCheckpoints;
    private Animator anim;

    private void Awake()
    {

        anim = GetComponentInChildren<Animator>();
        allCheckpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);
    }

    public void ActivateCheckpoint(bool active)
    {
        anim.SetBool("isActive", active);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var point in allCheckpoints)
            point.ActivateCheckpoint(false);

        SaveManager.instance.GetGameData().saveCheckpoint = (Vector2)transform.position;
        ActivateCheckpoint(true);
    }

    public void LoadData(GameData data)
    {
        bool active = data.saveCheckpoint == transform.position;
        ActivateCheckpoint(active);

        if (active)
            Player.instance.SwapPlayer(transform.position);
    }

    public void SaveData(ref GameData data)
    {

    }
}
