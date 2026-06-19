using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CinematicBarsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image topBar;
    [SerializeField] private Image bottomBar;

    [Header("Settings")]
    [SerializeField] private float targetAspect = 2.35f;
    [SerializeField] private float transitionDuration = 1f;

    public float TransitionDuration => transitionDuration;

    private float currentBarHeight;
    private float targetBarHeight;

    private void Start()
    {
        CalculateTargetBarHeight();
        SetBarHeight(0f);
    }

    private void CalculateTargetBarHeight()
    {
        float screenHeight = Screen.height;
        float targetHeight = Screen.width / targetAspect;
        targetBarHeight = (screenHeight - targetHeight) / 2f;
    }

    public void EnableCinematicMode()
    {
        StopAllCoroutines();
        StartCoroutine(TransitionBars(targetBarHeight));
    }

    public void DisableCinematicMode()
    {
        StopAllCoroutines();
        StartCoroutine(TransitionBars(0f));
    }

    private IEnumerator TransitionBars(float targetHeight)
    {
        float startHeight = currentBarHeight;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            currentBarHeight = Mathf.Lerp(startHeight, targetHeight, t);
            SetBarHeight(currentBarHeight);
            yield return null;
        }

        currentBarHeight = targetHeight;
        SetBarHeight(currentBarHeight);
    }

    private void SetBarHeight(float height)
    {
        topBar.rectTransform.sizeDelta = new Vector2(0, height);
        bottomBar.rectTransform.sizeDelta = new Vector2(0, height);
    }
}