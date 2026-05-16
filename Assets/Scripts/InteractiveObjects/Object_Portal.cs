using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Portal : MonoBehaviour, ISaveable
{
    public static Object_Portal instance;

    [Header("Default Position")]
    [SerializeField] private Vector3 fixedGlobalPortalPosition = new Vector3(138f, 7.09f, 0f);
    [SerializeField] private int fixedGlobalFacingDir = 1;

    [SerializeField] private Animator doorAnimator;

    public bool isActive { get; private set; }
    [SerializeField] private Vector2 defaultPosition;
    [SerializeField] private string placeSceneName = "Level_Rainbow 's place";

    [SerializeField] private Transform respawnPoint;
    [SerializeField] private bool canBeTriggered;

    private string currentSceneName;
    private string returnSceneName;
    private bool returningFromPlace;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        currentSceneName = SceneManager.GetActiveScene().name;
        transform.position = new Vector3(9999, 9999);
    }

    public void ActivatePortal(Vector3 position, int facingDir = 1)
    {
        isActive = true;
        transform.position = fixedGlobalPortalPosition;
        transform.rotation = Quaternion.identity;
        if (fixedGlobalFacingDir == -1)
            transform.Rotate(0, 180, 0);

        doorAnimator.SetTrigger("Open");

        SaveManager.instance.GetGameData().inScenePortals.Clear();
        SaveManager.instance.GetGameData().inScenePortals["GLOBAL_PORTAL"] = fixedGlobalPortalPosition;
    }

    public void DisableIfNeeded()
    {
        if (returningFromPlace == false)
            return;

        isActive = false;
        transform.position = new Vector3(9999, 9999);

        doorAnimator.ResetTrigger("Open");
    }

    private void UseTransfer()
    {
        if (InPlace())
        {
            Debug.Log("Cannot return from this place");
            return;
        }

        string destinationScene = placeSceneName;
        GameManager.instance.ChangeScene(destinationScene, RespawnType.Portal);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeTriggered == false)
            return;

        UseTransfer();
    }

    public void SetTrigger(bool trigger) => canBeTriggered = trigger;
    public Vector3 GetPosition() => respawnPoint != null ? respawnPoint.position : transform.position;

    private bool InPlace() => currentSceneName == placeSceneName;

    public void LoadData(GameData data)
    {
        if (InPlace())
        {
            isActive = false;
            transform.position = new Vector3(9999, 9999);
        }
        else if (data.inScenePortals.ContainsKey("GLOBAL_PORTAL"))
        {
            transform.position = fixedGlobalPortalPosition;
            transform.rotation = Quaternion.identity;
            if (fixedGlobalFacingDir == -1)
                transform.Rotate(0, 180, 0);
            isActive = true;
        }
        else
        {
            isActive = false;
            transform.position = new Vector3(9999, 9999);
        }

        returningFromPlace = data.returningFromPlace;
        returnSceneName = data.portalDestinationSceneName;
    }

    public void SaveData(ref GameData data)
    {
        if (!InPlace() && isActive)
        {
            data.inScenePortals["GLOBAL_PORTAL"] = fixedGlobalPortalPosition;
            data.portalDestinationSceneName = currentSceneName;
        }
        else
        {
            data.inScenePortals.Remove("GLOBAL_PORTAL");
        }

        data.returningFromPlace = InPlace();
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}