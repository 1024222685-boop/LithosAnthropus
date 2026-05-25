using UnityEngine;

public class Object_BlackSmith : Object_NPC, IInteractable
{
    [Header("Quest & Dialogue")]
    [SerializeField] private DialogueLineSO firstDialogueLine;
    [SerializeField] private QuestDataSO[] quests;

    private Animator anim;
    private Inventory_Player inventory;
    private Inventory_Storage storage;

    protected override void Awake()
    {
        base.Awake();
        storage = GetComponent<Inventory_Storage>();
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("isBlacksmith", true);
    }

    public override void Interact()
    {
        base.Interact();
        ui.craftUI.SetCraftUI(storage);
        ui.storageUI.SetupStorage(storage);
        ui.OpenDialogueUI(firstDialogueLine, new DialogueNpcData(rewardNpc, quests));

        //ui.OpenStorageUI(true);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = player.GetComponent<Inventory_Player>();
        storage.SetInventory(inventory);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        ui.HideAllTooltips();
        ui.OpenStorageUI(false);
    }
}
