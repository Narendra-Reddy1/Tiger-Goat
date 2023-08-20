using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SovereignStudios
{
    [CreateAssetMenu(fileName = "newAudioAsset", menuName = "ScriptableObjects/AudioAsset", order = 1)]
    public class AudioAsset : BaseScriptlableObject
    {
        [SerializeField]
        private AudioAssetsDictionary m_audioDictionary;

        public AudioClip GetAudioClipByID(AudioID audioID) => m_audioDictionary[audioID];
    }
    public enum AudioID
    {
        GameplayBGM,
        ButtonClickSFX,
        AnimalMovedSFX,
        AnimalClickSFX,
    }

}