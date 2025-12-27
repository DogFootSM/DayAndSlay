using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[Serializable]
public class AudioSettings
{
    public float SfxVolume;
    public float BgmVolume;
    public float MasterVolume;
    public bool MasterMute;
    public bool BgmMute;
    public bool SfxMute;
}
