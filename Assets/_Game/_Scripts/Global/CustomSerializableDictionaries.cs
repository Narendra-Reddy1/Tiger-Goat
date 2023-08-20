using System;
using SovereignStudios.Enums;
using UnityEngine;

namespace SovereignStudios
{
    //ScreenManager
    [Serializable]
    public class ScreenSystemObjectDictionary : SerializableDictionary<Window, WindowAddressableObject> { }

    //SpotPoint
    [Serializable]
    public class NeighborsDictionary : SerializableDictionary<DirectionFace, SpotPointBase> { }

    //SpriteManager
    [Serializable]
    public class ResourcesDictionary : SerializableDictionary<ResourceType, UnityEngine.Sprite> { }
    
    [Serializable]
    public class AudioAssetsDictionary : SerializableDictionary<AudioID, AudioClip> { }
}