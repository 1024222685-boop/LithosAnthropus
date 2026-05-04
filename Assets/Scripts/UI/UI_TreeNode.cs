using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class UI_TreeNode : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
    ISelectHandler, IDeselectHandler, ISubmitHandler
{
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skillTree;
    private UI_TreeConnectHandler connectHandler;
    private Button button;

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

    [Header("Controller choose")]
    [SerializeField] private GameObject SelectionFrame;
    [SerializeField] private float flashSpeed = 3f;
    [SerializeField] private Color flashColor = Color.yellow;
    private Coroutine flashCoroutine;
    private Image frameImage;

    private Color baseNormalColor;
    private readonly Color hoverHighlightColor = new Color(0.9f, 0.9f, 0.9f, 1f);


    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
        connectHandler = GetComponent<UI_TreeConnectHandler>();

        button = GetComponent<Button>();
        if (button == null)
        {
            button = gameObject.AddComponent<Button>();
        }

        button.transition = Selectable.Transition.None;
        button.targetGraphic = skillIcon;
        button.colors = new ColorBlock()
        {
            normalColor = Color.white,
            highlightedColor = Color.white,
            pressedColor = Color.white,
            disabledColor = Color.white,
            colorMultiplier = 1f,
            fadeDuration = 0f
        };

        // 修复：统一初始化外框Image，仅从子物体获取，不重复覆盖
        if (SelectionFrame != null)
        {
            frameImage = SelectionFrame.GetComponentInChildren<Image>();
            if (frameImage != null)
            {
                frameImage.color = flashColor;
            }
            SelectionFrame.SetActive(false);
        }

        baseNormalColor = GetColorByHex(lockedColorHex);
        UpdateIconBaseColor(baseNormalColor);
    }

    private void Start()
    {
        if (skillData.unlockedByDefault)
            Unlock();

        UpdateButtonInteractable();
    }

    private void UpdateButtonInteractable()
    {
        if (button != null)
        {
            button.interactable = true;
        }
    }

    private void UpdateIconBaseColor(Color newBaseColor)
    {
        baseNormalColor = newBaseColor;
        if (skillIcon != null)
            skillIcon.color = baseNormalColor;
    }

    public void Refund()
    {
        if (isUnlocked == false || skillData.unlockedByDefault)
            return;

        isUnlocked = false;
        isLocked = false;

        UpdateIconBaseColor(GetColorByHex(lockedColorHex));

        skillTree.AddSkillPoints(skillData.cost);
        connectHandler.UnlockedConnectionImage(false);

        UpdateButtonInteractable();
    }

    private void Unlock()
    {
        isUnlocked = true;

        UpdateIconBaseColor(Color.white);
        LockConflictNodes();

        skillTree.RemoveSkillPoints(skillData.cost);
        connectHandler.UnlockedConnectionImage(true);

        skillTree.skillManager.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData.upgradeData);

        UpdateButtonInteractable();
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
            node.UpdateButtonInteractable();
        }
    }

    public void LockChildNodes()
    {
        isLocked = true;
        UpdateButtonInteractable();

        foreach (var node in connectHandler.GetChildNodes())
            node.LockChildNodes();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TryUnlockOrShowLocked();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        TryUnlockOrShowLocked();
    }

    private void TryUnlockOrShowLocked()
    {
        if (CanBeUnlocked())
            Unlock();
        else if (isLocked)
            ui.skillToolTip.LockedSkillEffect();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltipAndHighlight();
        ShowSelectionFrame();
    }

    public void OnSelect(BaseEventData eventData)
    {
        ShowTooltipAndHighlight();
        ShowSelectionFrame();

        if (isLocked)
        {
            ui.skillToolTip.LockedSkillEffect();
        }
    }

    private void ShowTooltipAndHighlight()
    {
        if (ui == null || ui.skillToolTip == null) return;
        ui.skillToolTip.ShowToolTip(true, rect, this);

        if (isUnlocked || isLocked)
            return;

        if (skillIcon != null)
            skillIcon.color = hoverHighlightColor;
    }

    // 修复：移除拦截判断，确保协程正常启动
    private void ShowSelectionFrame()
    {
        if (SelectionFrame == null || frameImage == null) return;

        SelectionFrame.SetActive(true);

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashFrame());
    }

    // 修复：优化协程稳定性，增加退出条件与空值保护
    private IEnumerator FlashFrame()
    {
        if (frameImage == null) yield break;

        while (SelectionFrame.activeSelf)
        {
            if (frameImage == null) yield break;

            float alpha = Mathf.PingPong(Time.unscaledTime * flashSpeed, 1f) * 0.7f + 0.3f;
            frameImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltipAndResetColor();
        HideSelectionFrame();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        HideTooltipAndResetColor();
        HideSelectionFrame();
    }

    private void HideTooltipAndResetColor()
    {
        if (ui == null || ui.skillToolTip == null) return;
        ui.skillToolTip.ShowToolTip(false, rect);

        if (skillIcon != null)
            skillIcon.color = baseNormalColor;
    }

    private void HideSelectionFrame()
    {
        if (SelectionFrame == null) return;

        SelectionFrame.SetActive(false);

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }
    }

    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color color);
        return color;
    }

    private void OnDisable()
    {
        HideSelectionFrame();

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