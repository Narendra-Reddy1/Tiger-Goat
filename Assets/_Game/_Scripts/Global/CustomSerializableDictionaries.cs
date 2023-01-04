using System;
using SovereignStudios.Enums;
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
}