using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{   
    private const string OST_Key = "OST_Volume";
    private const string SFX_Key = "SFX_Volume";
    private const string OST_Enabled_Key = "OST_Enabled";
    private const string SFX_Enabled_Key = "SFX_Enabled";

    public float SFXVolume 
    {
        get
        {
            return sfxVolume;
        }
    }
    public float OSTVolume
    {
        get
        {
            return ostVolume;
        }
    }
    public bool SFXEnabled
    {
        get
        {
            return sfxEnabled;
        }
    }
    public bool OSTEnabled
    {
        get
        {
            return ostEnabled;
        }
    }

    private float sfxVolume = 1;
    private float ostVolume = 0.5f;
    private bool sfxEnabled = true;
    private bool ostEnabled = true; 

    private SoundDB.OST activeOST;

    public static SoundManager Instance 
    {
        get
        {
            if(dontuse_instance == null)
            {
                dontuse_instance = (new GameObject("Sound Manager")).AddComponent<SoundManager>();
                dontuse_instance.transform.position = Camera.main.transform.position;
                float result;
                result = PlayerPrefs.GetFloat(OST_Key,0.5f);
                dontuse_instance.ostVolume = result;
                result = PlayerPrefs.GetFloat(SFX_Key,1);
                dontuse_instance.sfxVolume = result;
                int resultBool;
                resultBool = PlayerPrefs.GetInt(OST_Enabled_Key,1);
                dontuse_instance.ostEnabled = resultBool == 1;
                resultBool = PlayerPrefs.GetInt(SFX_Enabled_Key,1);
                dontuse_instance.sfxEnabled = resultBool == 1;
            }

            return dontuse_instance;
        }
    }   
    private static SoundManager dontuse_instance;


    public static void PlayOST(SoundDB.OST clip)
    {
        Instance.activeOST = clip;
    }

    public static void PlayOST(List<SoundDB.OST> clips)
    {
        Instance.activeOST = clips.Random();
    }

    public static void PlaySFX(SoundDB.SFX clip)
    {
        if(clip.Clip == null) return;

        Instance.StartCoroutine(SFXCoroutine(clip.Clip,clip.StartTime,clip.Volume));
    }

    public static void PlaySFX(SoundDB.SFX clip, Vector3 position)
    {
        if(clip.Clip == null) return;

        Instance.StartCoroutine(SFXCoroutine(clip.Clip,clip.StartTime,clip.Volume,position));
    }

    public static void PlaySFX(List<SoundDB.SFX> clips)
    {
        SoundDB.SFX targetClip = clips.Random();

        if(targetClip.Clip == null) return;

        Instance.StartCoroutine(SFXCoroutine(targetClip.Clip,targetClip.StartTime,targetClip.Volume));
    }

    public static void PlaySFX(List<SoundDB.SFX> clips, Vector3 position)
    {
        SoundDB.SFX targetClip = clips.Random();

        if(targetClip.Clip == null) return;

        Instance.StartCoroutine(SFXCoroutine(targetClip.Clip,targetClip.StartTime,targetClip.Volume,position));
    }

    public static void SetSFXVolume(float value)
    {
        value = Mathf.Clamp(value,0,1);

        Instance.sfxVolume = value;
    }

    public static void SetOSTVolume(float value)
    {
        value = Mathf.Clamp(value,0,1);

        Instance.ostVolume = value;
    }

    public static void SetEnabled(bool value, bool isSFX)
    {
        if(isSFX)
        {
            Instance.sfxEnabled = value;
        }
        else
        {
            Instance.ostEnabled = value;
        }
    }

    private void Start()
    {
        StartCoroutine(OSTCoroutine());
    }

    private void OnDestroy()
    {
        if(dontuse_instance)
        {
            PlayerPrefs.SetFloat(OST_Key,dontuse_instance.OSTVolume);
            PlayerPrefs.SetFloat(SFX_Key,dontuse_instance.SFXVolume);
            PlayerPrefs.SetInt(OST_Enabled_Key,dontuse_instance.OSTEnabled ? 1 : 0);
            PlayerPrefs.SetInt(SFX_Enabled_Key,dontuse_instance.SFXEnabled ? 1 : 0);
        }
        dontuse_instance = null;
    }

    private static IEnumerator OSTCoroutine()
    {
        string sourceName = "OST Source - Now Playing: ";
        AudioSource source = (new GameObject(sourceName + "N/A")).AddComponent<AudioSource>();
        source.transform.parent = Instance.transform;
        source.transform.position = Instance.transform.position;
        source.transform.SetAsFirstSibling();
        source.loop = true;
        source.volume = 0;

        while(true)
        {
            if(Instance.activeOST.Clip == null) yield return new WaitWhile(() => Instance.activeOST.Clip == null);
            float accumulatedTime = 0;

            float nowVolume = (Instance.activeOST.Volume/100) * Mathf.Pow(2.7f,4*Instance.OSTVolume - 4) * (Instance.OSTEnabled ? 1 : 0);

            if(source.clip != null)
            {
                float oldVolume = source.volume;
                while(accumulatedTime < 1)
                {
                    accumulatedTime += Time.deltaTime;
                    source.volume = Mathf.Lerp(oldVolume,0,ConstantDatabase.SmoothCurve.Evaluate(accumulatedTime));
                    yield return null;
                }
            }

            source.Stop();
            source.clip = Instance.activeOST.Clip;
            source.gameObject.name = sourceName + (source.clip == null ? "N/A" : source.clip.name);
            source.time = 0;
            source.Play();

            accumulatedTime = 0;
            while(accumulatedTime < 1)
            {
                accumulatedTime += Time.deltaTime;
                source.volume = Mathf.Lerp(0,nowVolume,ConstantDatabase.SmoothCurve.Evaluate(accumulatedTime));
                yield return null;
            }

            bool waiting = true;
            while(waiting)
            {
                yield return null;

                if(Instance.activeOST.Clip != source.clip)
                {
                    waiting = false;
                }
                else
                {
                    float vol = (Instance.activeOST.Volume/100) * Mathf.Pow(2.7f,4*Instance.OSTVolume - 4) * (Instance.OSTEnabled ? 1 : 0);

                    if(source.volume != vol)
                    {
                        source.volume = vol;
                    }
                }

            }
        }
    }

    private static IEnumerator SFXCoroutine(AudioClip clip, float StartTime,float Volume)
    {
        yield return SFXCoroutine(clip,StartTime,Volume,Instance.transform.position);
    }

    private static IEnumerator SFXCoroutine(AudioClip clip, float StartTime,float Volume,Vector3 Position)
    {
        AudioSource source = (new GameObject("SFX: " + clip.name)).AddComponent<AudioSource>();
        source.transform.parent = Instance.transform;
        source.transform.position = Position;
        source.volume = (Volume/100) * Mathf.Pow(2.7f,4*Instance.SFXVolume - 4) * (Instance.SFXEnabled ? 1 : 0);
        source.clip = clip;
        source.time = StartTime >=0 ? StartTime : 0;
        // source.rolloffMode = AudioRolloffMode.Custom;

        if(StartTime < 0)
        {
            source.Stop();
            yield return new WaitForSeconds(-StartTime);
        }
        source.Play();

        yield return new WaitWhile(() => source.isPlaying);
        Destroy(source.gameObject);
    }
}
