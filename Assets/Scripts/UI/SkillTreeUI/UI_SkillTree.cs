using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] public int skillPoints;
    [SerializeField] private UI_TreeConnectHandler[] parentNodes;
    [SerializeField] private UI_TreeNode defaultSelectedNode;
    private UI_TreeNode[] allTreeNodes;
    public Player_SkillManager skillManager { get; private set; }   

    private void Start()
    {
        UpdateAllConnections();
    }

    public void UnlockDefaultSkills()
    {
        allTreeNodes = GetComponentsInChildren<UI_TreeNode>(true);
        skillManager = FindAnyObjectByType<Player_SkillManager>();

        foreach(var node in allTreeNodes)
            node.UnlockDefaultSkill();
    }

    private void OnEnable()
    {
        Invoke(nameof(SetDefaultSelectedNode), 0.02f);
    }

    private void SetDefaultSelectedNode()
    {
        if (EventSystem.current == null) return;

        if (defaultSelectedNode != null)
        {
            Button btn = defaultSelectedNode.GetComponent<Button>();
            if (btn != null && btn.interactable)
            {
                EventSystem.current.SetSelectedGameObject(defaultSelectedNode.gameObject);
                return;
            }
        }

        UI_TreeNode[] allNodes = GetComponentsInChildren<UI_TreeNode>();
        foreach (var node in allNodes)
        {
            Button btn = node.GetComponent<Button>();
            if (btn != null && btn.interactable)
            {
                EventSystem.current.SetSelectedGameObject(node.gameObject);
                return;
            }
        }
    }

    [ContextMenu("Reset Skill Tree")]
    public void RefundAllSkills()
    {
        UI_TreeNode[] skillNodes = GetComponentsInChildren<UI_TreeNode>();

        foreach (var node in skillNodes)
            node.Refund();
    }

    public bool EnoughSkillPoints(int cost) => skillPoints >= cost;
    public void RemoveSkillPoints(int cost) => skillPoints = skillPoints - cost;
    public void AddSkillPoints(int points) => skillPoints = skillPoints + points;

    [ContextMenu("Update All Connecitons")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }
}