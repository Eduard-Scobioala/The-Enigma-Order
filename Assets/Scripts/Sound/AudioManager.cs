using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour, IDataPersistance
{
    [Header("Volume")]
    [Range(0, 1)]
    public float musicVolume;
    [Range(0, 1)]
    public float SFXVolume;

    private Bus musicBus;
    private Bus sfxBus;

    private List<EventInstance> eventInstances;
    private EventInstance musicEventInstance;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }

        Instance = this;

        eventInstances = new List<EventInstance>();

        // Get busses for the volume control
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        try
        {
            InitializeMusic(FMODEvents.Instance.backgroundMusic);
        }
        catch{}
    }

    private void Update()
    {
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(SFXVolume);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    private void CleanUp()
    {
        // Stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }

    public void LoadData(GameData data)
    {
        // Set up the volume
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 1.0f);
        SFXVolume =  PlayerPrefs.GetFloat("SFXVolume", 1.0f);
    }

    public void SaveData(GameData data)
    {
        return;
    }
}
