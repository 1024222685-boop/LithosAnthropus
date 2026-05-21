using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Options : MonoBehaviour
{
    private Player player;
    [SerializeField] private Toggle healthBarToggle;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float mixerMultiplier = 25;

    [Header("BGM Volume Settings")]
    [SerializeField] private Slider bgmSlideer;
    [SerializeField] private string bgmParametr;

    [Header("SFX Volume Settings")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private string sfxParametr;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        healthBarToggle.onValueChanged.AddListener(OnHealthToggleChanged);

        healthBarToggle.isOn = PlayerPrefs.GetInt("HealthBar", 1) == 1;

        OnHealthToggleChanged(healthBarToggle.isOn);
    }

    public void BGMSliderValue(float value)
    {
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(bgmParametr, newValue);
    }

    public void SFXSliderValue(float value)
    {
        float newValue = MathF.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(sfxParametr, newValue);
    }

    private void OnHealthToggleChanged(bool isOn)
    {
        player.health.EnableHealthBar(isOn);
    }

    public void GoMainMenuBTN() => GameManager.instance.ChangeScene("MainMenu", RespawnType.NoneSpecific);

    private void OnEnable()
    {
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParametr, 0.6f);
        bgmSlideer.value = PlayerPrefs.GetFloat(bgmParametr, 0.6f);

        healthBarToggle.isOn = PlayerPrefs.GetInt("HealthBar", 1) == 1;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(sfxParametr, sfxSlider.value);
        PlayerPrefs.SetFloat(bgmParametr, bgmSlideer.value);

        PlayerPrefs.SetInt("HealthBar", healthBarToggle.isOn ? 1 : 0);

        PlayerPrefs.Save();
    }

    public void LoadUpVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParametr, 0.6f);
        bgmSlideer.value = PlayerPrefs.GetFloat(bgmParametr, 0.6f);
    }
}
