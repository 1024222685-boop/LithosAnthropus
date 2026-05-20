using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        healthBarToggle.onValueChanged.AddListener(OnHealthToggleChanged);

        OnHealthToggleChanged(healthBarToggle.isOn);
    }

    public void BGMSliderValue(float value)
    {
        audioMixer.SetFloat(bgmParametr, value);
    }

    private void OnHealthToggleChanged(bool isOn)
    {
        player.health.EnableHealthBar(isOn);
    }

    public void GoMainMenuBTN() => GameManager.instance.ChangeScene("MainMenu", RespawnType.NoneSpecific);
}
