using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SoundDatabase", menuName = "ScriptableObjects/SoundDatabase", order = 1)]
public class SoundDB : ScriptableObject
{
    [System.Serializable]
    public struct SFX
    {
        public AudioClip Clip;
        public float StartTime;
        [Range(0f,100f)]
        public float Volume;
    }

    [System.Serializable]
    public struct OST
    {
        public AudioClip Clip;
        [Range(0f,100f)]
        public float Volume;
    }

    [Header("Sound Track")]
    public OST FirstLevelOST;

    [Header("SFX")]
    public List<SFX> SpinTopCollideSFX;
    public SFX SpinTopExplosionSFX;
}
