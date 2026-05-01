using UnityEngine;

public class Skill_SickleThrow : Skill_Base
{
    private SkillObject_Sickle currentSickle;
    private float currentThrowPower;

    [Header("Regular Sickle Upgrade")]
    [SerializeField] private GameObject sicklePrefab;
    [Range(0, 10)]
    [SerializeField] private float regularThrowPower = 5;

    [Header("Pierce Sickle Upgrade")]
    [SerializeField] private GameObject pierceSicklePrefab;
    public int amountToPierce = 2;
    [Range(0, 10)]
    [SerializeField] private float pierceThrowPower = 5;

    [Header("Spin Sickle Upgrade")]
    [SerializeField] private GameObject spinSicklePrefab;
    public int maxDistance = 5;
    public float attackPerSecond = 6;
    public float maxSpinDuration = 3;
    [Range(0, 10)]
    [SerializeField] private float spinThrowPower = 5;

    [Header("Bounce Sickle Upgrade")]
    [SerializeField] private GameObject bounceSicklePrefab;
    public int bounceCount = 5;
    public float bounceSpeed = 12;
    [Range(0, 10)]
    [SerializeField] private float bounceThrowPower = 5;

    [Header("Trajectory prediction")]
    [SerializeField] private GameObject predictionDot;
    [SerializeField] private int numberOfDots = 20;
    [SerializeField] private float spaceBetweenDots = .05f;
    private float sickleGravity;
    private Transform[] dots;
    private Vector2 confirmedDirection;
    private float playerFacingDirection => transform.root.localScale.x;

    protected override void Awake()
    {
        base.Awake();
        sickleGravity = sicklePrefab.GetComponent<Rigidbody2D>().gravityScale;
        dots = GenerateDots();
    }

    public override bool CanUseSkill()
    {
        UpdateThrowPower();

        if (currentSickle != null)
        {
            currentSickle.GetSickleBackToPlayer();
            return false;
        }

        return base.CanUseSkill();
    }

    public void ThrowSickle()
    {
        GameObject sicklePrefab = GetSicklePrefab();
        GameObject newSickle = Instantiate(sicklePrefab, dots[1].position, Quaternion.identity);

        currentSickle = newSickle.GetComponent<SkillObject_Sickle>();
        currentSickle.SetupSickle(this, GetThrowPower());

        SetSkillOnCooldown();
    }

    private GameObject GetSicklePrefab()
    {
        if (Unlocked(SkillUpgradeType.SickleThrow))
            return sicklePrefab;

        if (Unlocked(SkillUpgradeType.SickleThrow_Pierce))
            return pierceSicklePrefab;

        if (Unlocked(SkillUpgradeType.SickleThrow_Spin))
            return spinSicklePrefab;

        if (Unlocked(SkillUpgradeType.SickleThrow_Bounce))
            return bounceSicklePrefab;

        Debug.Log("No valid upgrade selected!");
        return null;
    }

    private void UpdateThrowPower()
    {
        switch (upgradeType)
        {
            case SkillUpgradeType.SickleThrow:
                currentThrowPower = regularThrowPower;
                break;
            case SkillUpgradeType.SickleThrow_Pierce:
                currentThrowPower = pierceThrowPower;
                break;
            case SkillUpgradeType.SickleThrow_Spin:
                currentThrowPower = spinThrowPower;
                break;
            case SkillUpgradeType.SickleThrow_Bounce:
                currentThrowPower = bounceThrowPower;
                break;
        }
    }

    private Vector2 GetThrowPower() => confirmedDirection * (currentThrowPower * 10);

    public void PredicTraJectory(Vector2 direction)
    {
        Vector2 correctedDir = direction * playerFacingDirection;

        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].position = GetTraJectoryPoint(correctedDir, i * spaceBetweenDots);
        }
    }

    private Vector2 GetTraJectoryPoint(Vector2 direction, float t)
    {
        float scaledThrowPower = currentThrowPower * 10;
        Vector2 initialVelocity = direction * scaledThrowPower;
        Vector2 gravityEffect = 0.5f * Physics2D.gravity * sickleGravity * (t * t);
        Vector2 predictedPoint = (initialVelocity * t) + gravityEffect;
        Vector2 playerPosition = transform.root.position;

        return playerPosition + predictedPoint;
    }

    public void ConfirmTraJectory(Vector2 direction)
    {
        confirmedDirection = direction * playerFacingDirection;
    }

    public void EnableDots(bool enable)
    {
        foreach (Transform t in dots)
            t.gameObject.SetActive(enable);
    }

    private Transform[] GenerateDots()
    {
        Transform[] newDots = new Transform[numberOfDots];

        for (int i = 0; i < numberOfDots; i++)
        {
            newDots[i] = Instantiate(predictionDot, transform.position, Quaternion.identity, transform).transform;
            newDots[i].gameObject.SetActive(false);
        }

        return newDots;
    }
}