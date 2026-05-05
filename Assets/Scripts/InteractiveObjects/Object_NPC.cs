using UnityEngine;

public class Object_NPC : MonoBehaviour
{
    protected Transform player;
    protected UI ui;

    [SerializeField] private Transform npc;
    [SerializeField] private GameObject interactToolTip;
    private bool facingRight = true;

    [Header("Floaty Tooltip")]
    [SerializeField] private float floatSpeed = 8f;
    [SerializeField] private float floatRange = .1f;
    [SerializeField] private Vector3 tooltipOffset = new Vector3(0, 2f, 0);

    [Header("NPC Settings")]
    [SerializeField] private bool enableFlip = true;


    protected virtual void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        interactToolTip.SetActive(false);
    }

    protected virtual void Update()
    {
        HandleNpcFlip();
        HandleToolTipFloat();
    }

    private void HandleToolTipFloat()
    {
        if (interactToolTip.activeSelf)
        {
            Vector3 currentBasePos = transform.position + tooltipOffset;
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
            interactToolTip.transform.position = currentBasePos + new Vector3(0, yOffset);
        }
    }

    private void HandleNpcFlip()
    {
        if (!enableFlip)
            return;

        if (player == null || npc == null)
            return;

        if (npc.position.x > player.position.x && facingRight)
        {
            Vector3 scale = npc.localScale;
            scale.x *= -1;
            npc.localScale = scale;
            facingRight = false;
        }
        else if (npc.position.x < player.position.x && facingRight == false)
        {
            Vector3 scale = npc.localScale;
            scale.x *= -1;
            npc.localScale = scale;
            facingRight = true;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        player = collision.transform;
        interactToolTip.SetActive(true);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        interactToolTip.SetActive(false);
    }
}