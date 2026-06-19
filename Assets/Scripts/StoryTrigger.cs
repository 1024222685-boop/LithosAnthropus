using UnityEngine;
using System.Collections;
using System.Linq;

public class StoryTrigger : MonoBehaviour, ISaveable
{
    [Header("References")]
    [SerializeField] private CinematicBarsController cinematicController;
    [SerializeField] private Player player;
    [SerializeField] private GameObject storyContent;

    [Header("Settings")]
    [SerializeField] private float storyDuration = 4f;
    [SerializeField] private bool triggerOnlyOnce = true;
    [SerializeField] private bool exitOnLeaveTrigger = false;
    [Tooltip("Global unique ID. Must be unique for each trigger. Used for save data persistence.")]
    [SerializeField] private string saveKey = "Story_001";

    private bool hasTriggered;
    private bool saveLoaded;
    private Coroutine activeStoryCoroutine;
    private Collider2D triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!saveLoaded || hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            activeStoryCoroutine = StartCoroutine(StorySequence());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && exitOnLeaveTrigger && activeStoryCoroutine != null)
        {
            StopCoroutine(activeStoryCoroutine);
            EndStory();
        }
    }

    private IEnumerator StorySequence()
    {
        hasTriggered = triggerOnlyOnce;

        player.LockControls();
        UI.instance.EnterCinematicMode();

        cinematicController.EnableCinematicMode();
        yield return new WaitForSeconds(cinematicController.TransitionDuration);

        if (storyContent != null)
            storyContent.SetActive(true);

        float elapsed = 0f;
        while (elapsed < storyDuration)
        {
            elapsed += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
                break;
            yield return null;
        }

        EndStory();
    }

    private void EndStory()
    {
        if (storyContent != null)
            storyContent.SetActive(false);

        UI.instance.ExitCinematicMode();
        player.UnlockControls();
        cinematicController.DisableCinematicMode();

        if (triggerOnlyOnce)
        {
            SaveManager.instance.SaveGame();
        }

        activeStoryCoroutine = null;
    }

    private void TryTriggerIfPlayerInside()
    {
        if (hasTriggered || !saveLoaded || triggerCollider == null)
            return;

        Collider2D[] results = Physics2D.OverlapBoxAll(
            triggerCollider.bounds.center,
            triggerCollider.bounds.size,
            0f
        );

        foreach (var hit in results)
        {
            if (hit.CompareTag("Player"))
            {
                activeStoryCoroutine = StartCoroutine(StorySequence());
                return;
            }
        }
    }

    private void FreezeAllSceneAI(bool freeze)
    {
        MonoBehaviour[] allEnemyAI = FindObjectsOfType<MonoBehaviour>()
            .Where(go => go.GetType().Name.StartsWith("Enemy_"))
            .ToArray();

        foreach (var ai in allEnemyAI)
        {
            ai.enabled = !freeze;
            if (freeze && ai.TryGetComponent(out Rigidbody2D rb))
            {
                rb.velocity = Vector2.zero;
            }
        }

        MonoBehaviour[] allNpcs = FindObjectsOfType<MonoBehaviour>()
            .Where(go => go.GetType().Name.Contains("NPC") || go.GetType().Name.Contains("Merchant") || go.GetType().Name.Contains("BlackSmith"))
            .ToArray();

        foreach (var npc in allNpcs)
        {
            npc.enabled = !freeze;
        }
    }

    public void LoadData(GameData data)
    {
        if (!triggerOnlyOnce)
        {
            saveLoaded = true;
            TryTriggerIfPlayerInside();
            return;
        }

        if (data.triggeredStories.TryGetValue(saveKey, out bool value))
        {
            hasTriggered = value;
        }
        else
        {
            hasTriggered = false;
        }

        saveLoaded = true;
        TryTriggerIfPlayerInside();
    }

    public void SaveData(ref GameData data)
    {
        if (!triggerOnlyOnce) return;

        data.triggeredStories[saveKey] = hasTriggered;
    }
}