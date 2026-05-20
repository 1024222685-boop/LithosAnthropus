using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio Database")]
public class AudioDatabaseSO : ScriptableObject
{
    public List<AudioCilpData> player;
    public List<AudioCilpData> uiAudio;

    [Header("Music Lists")]
    public List<AudioCilpData> mainMenuMusic;
    public List<AudioCilpData> levelMusic;

    private Dictionary<string, AudioCilpData> clipCollection;

    private void OnEnable()
    {
        clipCollection = new Dictionary<string, AudioCilpData>();

        AddToCollection(player);
        AddToCollection(uiAudio);
        AddToCollection(mainMenuMusic);
        AddToCollection(levelMusic);
    }

    public AudioCilpData Get(string groupName)
    {
        return clipCollection.TryGetValue(groupName,out var data) ? data : null;
    }

    private void AddToCollection(List<AudioCilpData> listToAdd)
    {
        foreach (var data in listToAdd)
        {
            if (data != null && clipCollection.ContainsKey(data.audioName) == false)
            {
                clipCollection.Add(data.audioName, data);
            }
        }
    }
}

[System.Serializable]
public class AudioCilpData
{
    public string audioName;
    public List<AudioClip> clips = new List<AudioClip>();
    [Range(0f,1f)] public float maxVolume = 1f;

    public AudioClip GetRandomClip()
    {
        if(clips == null || clips.Count == 0)
            return null;

        return clips[Random.Range(0, clips.Count)];
    }
}