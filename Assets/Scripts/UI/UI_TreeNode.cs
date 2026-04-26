using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skillTree;
    private UI_TreeConnectHandler connectHandler;

    [Header("Unlock details")]
    public UI_TreeNode[] neededNodes;
    public UI_TreeNode[] conflictNodes;
    public bool isUnlocked;
    public bool isLocked;

    [Header("Skill details")]
    public Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private int skillCost;
    [SerializeField] private string lockedColorHex = "#9F9797";

    // 常驻基准色
    private Color baseNormalColor;
    private readonly Color hoverHighlightColor = new Color(0.9f, 0.9f, 0.9f, 1f);


    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
        connectHandler = GetComponent<UI_TreeConnectHandler>();

        // 初始化：未解锁=灰色常驻底色
        baseNormalColor = GetColorByHex(lockedColorHex);
        UpdateIconBaseColor(baseNormalColor);
    }

    private void Start()
    {
        if (skillData.unlockedByDefault)
            Unlock();
    }

    // 永久设置常驻底色
    private void UpdateIconBaseColor(Color newBaseColor)
    {
        baseNormalColor = newBaseColor;
        if (skillIcon != null)
            skillIcon.color = baseNormalColor;
    }

    public void Refund()
    {
        isUnlocked = false;
        isLocked = false;

        UpdateIconBaseColor(GetColorByHex(lockedColorHex));

        skillTree.AddSkillPoints(skillData.cost);
        connectHandler.UnlockedConnectionImage(false);
    }

    private void Unlock()
    {
        isUnlocked = true;
        // 解锁后常驻白色底色
        UpdateIconBaseColor(Color.white);
        LockConflictNodes();

        skillTree.RemoveSkillPoints(skillData.cost);
        connectHandler.UnlockedConnectionImage(true);

        skillTree.skillManager.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData.upgradeData);
    }

    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked)
            return false;

        if (!skillTree.EnoughSkillPoints(skillData.cost))
            return false;

        foreach (var node in neededNodes)
        {
            if (!node.isUnlocked)
                return false;
        }

        foreach (var node in conflictNodes)
        {
            if (node.isUnlocked)
                return false;
        }

        return true;
    }

    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
            node.LockChildNodes();
        }
    }

    public void LockChildNodes()
    {
        isLocked = true;

        foreach (var node in connectHandler.GetChildNodes())
            node.LockChildNodes();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            Unlock();
        else if (isLocked)
            ui.skillToolTip.LockedSkillEffect();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ui == null || ui.skillToolTip == null) return;
        ui.skillToolTip.ShowToolTip(true, rect, this);

        // 只有 未锁定、未解锁 的可交互节点，才允许鼠标高亮
        if (isUnlocked || isLocked)
            return;

        // 鼠标移入：临时高亮
        if (skillIcon != null)
            skillIcon.color = hoverHighlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ui == null || ui.skillToolTip == null) return;
        ui.skillToolTip.ShowToolTip(false, rect);

        //不管什么状态，鼠标移出 强制恢复常驻基准色
        if (skillIcon != null)
            skillIcon.color = baseNormalColor;
    }

    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color color);
        return color;
    }

    private void OnDisable()
    {
        // 失活时强制刷新正确底色
        if (isLocked || !isUnlocked)
            UpdateIconBaseColor(GetColorByHex(lockedColorHex));
        if (isUnlocked)
            UpdateIconBaseColor(Color.white);
    }

    private void OnValidate()
    {
        if (skillData == null)
            return;

        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        skillCost = skillData.cost;
        gameObject.name = "UI_TreeNode - " + skillData.displayName;
    }
}