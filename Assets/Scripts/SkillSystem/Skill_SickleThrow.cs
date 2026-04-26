using UnityEngine;

public class Skill_SickleThrow : Skill_Base
{
    private SkillObject_Sickle currentSickle;

    [Header("Regular Sickle Upgrade")]
    [SerializeField] private GameObject sicklePrefab;
    [Range(0, 10)]
    [SerializeField] private float throwPower = 5;

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
        if (currentSickle != null)
        {
            currentSickle.GetSickleBackToPlayer();
            return false;
        }

        return base.CanUseSkill();
    }

    public void ThrowSickle()
    {
        GameObject newSickle = Instantiate(sicklePrefab, dots[1].position, Quaternion.identity);

        currentSickle = newSickle.GetComponent<SkillObject_Sickle>();
        currentSickle.SetupSickle(this, GetThrowPower());
    }

    private Vector2 GetThrowPower() => confirmedDirection * (throwPower * 10);

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
        float scaledThrowPower = throwPower * 10;
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