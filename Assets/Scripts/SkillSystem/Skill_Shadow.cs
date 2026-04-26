using System.Collections;
using UnityEngine;

public class Skill_Shadow : Skill_Base
{
    private SkillObject_Shadow currentShadow;
    private Entity_Health playerHealth;

    [SerializeField] private GameObject shadowPrefab;
    [SerializeField] private float detonateTime = 2;

    [Header("Moving Shaow Upgrade")]
    [SerializeField] private float shadowSpeed = 2;

    [Header("Swap Shadow Upgrade")]
    [SerializeField] private float shadowExitDuration = 10;

    [Header("Multicast Shadow Upgrade")]
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private int currentCharges;
    [SerializeField] private bool isReCharging;

    [Header("Health Recover Shadow Upgrade")]
    [SerializeField] private float saveHealthPercent;

    protected override void Awake()
    {
        base.Awake();
        currentCharges = maxCharges;
        playerHealth = GetComponentInParent<Entity_Health>();
    }

    public override void TryUseSkill()
    {
        if (CanUseSkill() == false)
            return;

        if (Unlocked(SkillUpgradeType.ShadowClone))
            HandleShadowCLoneRegular();

        if (Unlocked(SkillUpgradeType.ShadowClone_CloneSpeedUp))
            HandleShadowCloneMoving();

        if (Unlocked(SkillUpgradeType.ShadowClone_MoreClone))
            HandleShadowMulticlone();

        if (Unlocked(SkillUpgradeType.ShadowClone_Swap))
            HandleShadowSwap();

        if (Unlocked(SkillUpgradeType.ShadowClone_SwapHpRecover))
            HandleShadowHealthRecover();
    }

    private void HandleShadowHealthRecover()
    {
        if (currentShadow == null)
        {
            CreateShadow();
            saveHealthPercent = playerHealth.GetHealthPercent();
        }
        else
        {
            ChangePlayerShadow();
            playerHealth.SetHealthPercent(saveHealthPercent);
            SetSkillOnCooldown();
        }
    }

    private void HandleShadowSwap()
    {
        if (currentShadow == null)
        {
            CreateShadow();
        }
        else
        {
            ChangePlayerShadow();
            SetSkillOnCooldown();
        }
    }

    private void ChangePlayerShadow()
    {
        Vector3 shadowPosition = currentShadow.transform.position;
        Vector3 playerPosition = player.transform.position;

        currentShadow.transform.position = playerPosition;
        currentShadow.Slash();

        player.SwapPlayer(shadowPosition);
    }

    private void HandleShadowMulticlone()
    {
        if (currentCharges <= 0)
            return;

        CreateShadow();
        currentShadow.MoveTowardsClosestTarget(shadowSpeed);
        currentCharges--;

        if (isReCharging == false)
            StartCoroutine(ShadowchargeCo());
    }

    private IEnumerator ShadowchargeCo()
    {
        isReCharging = true;

        while (currentCharges < maxCharges)
        {
            yield return new WaitForSeconds(cooldown);
            currentCharges++;
        }

        isReCharging = false;
    }

    private void HandleShadowCloneMoving()
    {
        CreateShadow();
        currentShadow.MoveTowardsClosestTarget(shadowSpeed);

        SetSkillOnCooldown();
    }

    private void HandleShadowCLoneRegular()
    {
        CreateShadow();
        SetSkillOnCooldown();
    }

    public void CreateShadow()
    {
        float detonateTime = GetDetonateTime();

        GameObject shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
        currentShadow = shadow.GetComponent<SkillObject_Shadow>();
        currentShadow.SetupShadow(this);

        if (Unlocked(SkillUpgradeType.ShadowClone_Swap) || Unlocked(SkillUpgradeType.ShadowClone_SwapHpRecover))
            currentShadow.OnSlash += ForceCooldown;
    }

    public void CreateRawShadow()
    {
        bool canMove = Unlocked(SkillUpgradeType.ShadowClone_CloneSpeedUp) || Unlocked(SkillUpgradeType.ShadowClone_MoreClone);

        GameObject shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
        shadow.GetComponent<SkillObject_Shadow>().SetupShadow(this, detonateTime, canMove, shadowSpeed);
    }

    public float GetDetonateTime()
    {
        if (Unlocked(SkillUpgradeType.ShadowClone_Swap) || Unlocked(SkillUpgradeType.ShadowClone_SwapHpRecover))
            return shadowExitDuration;

        return detonateTime;
    }

    private void ForceCooldown()
    {
        if (OnCooldown() == false)
        {
            SetSkillOnCooldown();
            currentShadow.OnSlash -= ForceCooldown;
        }
    }
}
