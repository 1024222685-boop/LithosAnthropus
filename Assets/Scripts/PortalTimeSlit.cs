using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PortalTimeSlit : MonoBehaviour
{
    [Header("SCENE CONFIG")]
    public string starfieldScene = "Across the Time slit";
    public string destinationScene;

    [Header("EFFECT SETTINGS")]
    public float timeInStarfield = 3f;
    public float fadeDuration = 0.3f;

    [Header("WHITE FLASH SETTINGS")]
    public int flashCount = 4;
    public float flashSpeed = 0.08f;

    [Header("CAMERA SHAKE SETTINGS")]
    public float shakeIntensity = 0.3f;
    public float shakeDuration = 0.6f;

    [Header("REFERENCE")]
    public GameObject playerHurtPrefab;
    public GameObject whiteFlashPanel;

    private static PortalTimeSlit activePortal;
    private UI_FadeScreen fadeScreen;
    private GameObject flashPanel;
    private Image flashImage;
    private Vector3 originalCamPos;
    private Camera mainCam;

    private void Awake()
    {
        if (activePortal == null)
        {
            activePortal = this;
            DontDestroyOnLoad(gameObject);
            fadeScreen = FindObjectOfType<UI_FadeScreen>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Transition(other.gameObject));
        }
    }

    private IEnumerator Transition(GameObject originalPlayer)
    {
        if (fadeScreen != null)
        {
            fadeScreen.DoFadeOut(fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
        }

        originalPlayer.SetActive(false);

        AsyncOperation loadStar = SceneManager.LoadSceneAsync(starfieldScene);
        while (!loadStar.isDone) yield return null;
        yield return null;

        Camera cam = Camera.main;
        Vector3 spawnPos = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane + 0.1f));
        spawnPos.z = 0;

        GameObject hurtObj = Instantiate(playerHurtPrefab, spawnPos, Quaternion.identity);

        SpriteRenderer sprite = hurtObj.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.enabled = true;
            sprite.color = Color.white;
            sprite.sortingLayerName = "Player";
            sprite.sortingOrder = 9999;
        }

        fadeScreen = FindObjectOfType<UI_FadeScreen>();
        if (fadeScreen != null)
        {
            fadeScreen.DoFadeIn(fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
        }

        mainCam = Camera.main;
        if (mainCam != null)
        {
            originalCamPos = mainCam.transform.position;
        }

        // 闪烁+抖动同步一起触发
        if (whiteFlashPanel != null && mainCam != null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                flashPanel = Instantiate(whiteFlashPanel, canvas.transform, false);
                flashImage = flashPanel.GetComponent<Image>();
                flashImage.color = new Color(1, 1, 1, 0);

                // 开启同步抖动
                StartCoroutine(ShakeCamera());

                for (int i = 0; i < flashCount; i++)
                {
                    flashImage.color = new Color(1, 1, 1, 1);
                    yield return new WaitForSeconds(flashSpeed);
                    flashImage.color = new Color(1, 1, 1, 0);
                    yield return new WaitForSeconds(flashSpeed);
                }
            }
        }

        yield return new WaitForSeconds(timeInStarfield);

        if (flashPanel != null) Destroy(flashPanel);
        if (mainCam != null)
        {
            mainCam.transform.position = originalCamPos;
        }

        if (fadeScreen != null)
        {
            fadeScreen.DoFadeOut(fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
        }

        Destroy(hurtObj);

        AsyncOperation loadTarget = SceneManager.LoadSceneAsync(destinationScene);
        while (!loadTarget.isDone) yield return null;
        yield return null;

        fadeScreen = FindObjectOfType<UI_FadeScreen>();
        if (fadeScreen != null)
        {
            fadeScreen.DoFadeIn(fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
        }

        Destroy(gameObject);
    }

    IEnumerator ShakeCamera()
    {
        float time = 0;
        while (time < shakeDuration)
        {
            float x = Random.Range(-shakeIntensity, shakeIntensity);
            float y = Random.Range(-shakeIntensity, shakeIntensity);
            mainCam.transform.position = originalCamPos + new Vector3(x, y, 0);
            time += Time.deltaTime;
            yield return null;
        }
        mainCam.transform.position = originalCamPos;
    }
}