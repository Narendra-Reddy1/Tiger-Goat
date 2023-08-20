using SovereignStudios.EventSystem;
using UnityEngine;

namespace SovereignStudios
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioAsset m_audioAsset;
        private AudioSource m_sfxAudioSource;
        private AudioSource m_bgmAudioSource;
        private void Awake()
        {
            _Init();
        }
        private void OnEnable()
        {
            GlobalEventHandler.AddListener(EventID.REQUEST_TO_PLAY_SFX, Callback_On_SFX_Requested);
            GlobalEventHandler.AddListener(EventID.REQUEST_TO_PLAY_BGM, Callback_On_BGM_Requested);
            GlobalEventHandler.AddListener(EventID.EVENT_ON_AUDIO_TOGGLE_UPDATED, Callback_On_Audio_Toggled);
        }
        private void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.REQUEST_TO_PLAY_SFX, Callback_On_SFX_Requested);
            GlobalEventHandler.RemoveListener(EventID.REQUEST_TO_PLAY_BGM, Callback_On_BGM_Requested);
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_AUDIO_TOGGLE_UPDATED, Callback_On_Audio_Toggled);
        }
        private void _Init()
        {
            m_sfxAudioSource = gameObject.AddComponent<AudioSource>();
            m_bgmAudioSource = gameObject.AddComponent<AudioSource>();
            m_sfxAudioSource.playOnAwake = false;
            m_bgmAudioSource.playOnAwake = false;
            m_bgmAudioSource.volume = 0.75f;
            m_bgmAudioSource.loop = true;
            _AudioToggled();
        }

        private void _PlaySFX(AudioID audioID)
        {
            m_sfxAudioSource.PlayOneShot(m_audioAsset.GetAudioClipByID(audioID));
        }
        private void _PlayBGM(AudioID audioID)
        {
            m_bgmAudioSource.clip = m_audioAsset.GetAudioClipByID(audioID);
            m_bgmAudioSource.Play();
        }
        private void _AudioToggled()
        {
            bool value = PlayerPrefsWrapper.GetPlayerPrefsBool(Enums.PlayerPrefKeys.audio_toggle, true);
            m_bgmAudioSource.mute = !value;
            m_sfxAudioSource.mute = !value;
        }

        private void Callback_On_Audio_Toggled(object args)
        {
            _AudioToggled();
        }
        private void Callback_On_SFX_Requested(object args)
        {
            _PlaySFX((AudioID)args);
        }
        private void Callback_On_BGM_Requested(object args)
        {
            _PlayBGM((AudioID)args);

        }
    }
}