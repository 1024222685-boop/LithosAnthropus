using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    private Entity entity;

    [Header("Image Echo VFX")]
    [Range(.01f, .2f)]
    [SerializeField] private float imageEchoInterval = .05f;
    [SerializeField] private GameObject imageEchoPrefab;
    private Coroutine imageEchCo;

    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration = .2f;
    private Material originalMaterial;
    private Coroutine onDamagevfxcoroutine;

    [Header("On Doing Damage VFX")]
    [SerializeField] private Color hitvfxColor = Color.white;
    [SerializeField] private GameObject hitvfx;
    [SerializeField] private GameObject critHitvfx;

    [Header("Element Colors")]
    [SerializeField] private Color mercyvfx = Color.yellow;
    [SerializeField] private Color brutalvfx = Color.red;
    [SerializeField] private Color cowardvfx = Color.gray;
    private Color originalHitVfxColor;
    private Coroutine statusvfxCo;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.sharedMaterial;
        originalHitVfxColor = hitvfxColor; // 保存默认颜色
    }

    public void DoImageEchoEffect(float duration)
    {
        StopImageEchoEffect();
        imageEchCo = StartCoroutine(ImageEchoEffectCo(duration));
    }

    public void StopImageEchoEffect()
    {
        if (imageEchCo != null)
            StopCoroutine(imageEchCo);
    }

    private IEnumerator ImageEchoEffectCo(float duration)
    {
        float timeTracker = 0;

        while (timeTracker < duration)
        {
            CreateImageEcho();

            yield return new WaitForSeconds(imageEchoInterval);
            timeTracker = timeTracker + imageEchoInterval;
        }
    }

    private void CreateImageEcho()
    {
        Vector3 position = entity.anim.transform.position;
        float scale = entity.anim.transform.localScale.x;

        GameObject imageEcho = Instantiate(imageEchoPrefab, position, transform.rotation);

        imageEcho.transform.localScale = new Vector3(scale, scale, scale);
        imageEcho.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
    }

    public void PlayOnStatusVfx(float duration, ElementType element)
    {
        if (statusvfxCo != null)
            StopCoroutine(statusvfxCo);

        if (element == ElementType.Mercy)
            statusvfxCo = StartCoroutine(PlayStatusVfxCo(duration, mercyvfx));

        if (element == ElementType.Brutality)
            statusvfxCo = StartCoroutine(PlayStatusVfxCo(duration, brutalvfx));

        if (element == ElementType.Cowardice)
            statusvfxCo = StartCoroutine(PlayStatusVfxCo(duration, cowardvfx));
    }

    public void StopAllvfx()
    {
        if (statusvfxCo != null)
            StopCoroutine(statusvfxCo);
        if (onDamagevfxcoroutine != null)
            StopCoroutine(onDamagevfxcoroutine);

        sr.color = Color.white;
        sr.material = originalMaterial;
    }

    private IEnumerator PlayStatusVfxCo(float duration, Color effectColor)
    {
        float tickInterval = .25f;
        float timeHasPassed = 0;

        Color lightColor = effectColor * 1.2f;
        Color darkColor = effectColor * .5f;
        bool toggle = false;

        while (timeHasPassed < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);
            timeHasPassed += tickInterval;
        }

        sr.color = Color.white;
        statusvfxCo = null;
    }

    public void CreateOnHitVFX(Transform target, bool isCrit)
    {
        GameObject hitprefab = isCrit ? critHitvfx : hitvfx;

        if (hitprefab != null)
        {
            GameObject vfx = Instantiate(hitprefab, target.position, Quaternion.identity);
            SpriteRenderer vfxSr = vfx.GetComponentInChildren<SpriteRenderer>();

            if (vfxSr != null)
                vfxSr.color = hitvfxColor; // 使用当前设置的颜色
        }
    }

    //自动根据元素类型切换颜色
    public void UpdateOnHitColor(ElementType element)
    {
        if (element == ElementType.Mercy)
            hitvfxColor = mercyvfx;
        else if (element == ElementType.Brutality)
            hitvfxColor = brutalvfx;
        else if (element == ElementType.Cowardice)
            hitvfxColor = cowardvfx;
        else
            hitvfxColor = originalHitVfxColor; // 无元素恢复默认色
    }

    public void PlayOnDamagevfx()
    {
        if (onDamagevfxcoroutine != null)
            StopCoroutine(onDamagevfxcoroutine);

        onDamagevfxcoroutine = StartCoroutine(OnDamagevfxCo());
    }

    private IEnumerator OnDamagevfxCo()
    {
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVfxDuration);
        sr.material = originalMaterial;
        onDamagevfxcoroutine = null;
    }
}