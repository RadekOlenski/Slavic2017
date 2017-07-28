using System;
using Events;
using Managers;
using UnityEngine;

namespace Sounds
{
    public class CallbackSound : MonoBehaviour {

        private Sound sound;

        private void Start()
        {
            EventManager.Instance.AddListener<PlayCallbackSoundEvent>(PlaySound);
        }

        private void PlaySound(PlayCallbackSoundEvent e)
        {
            sound = AudioManager.Main.NewSound(e.SoundToPlay, interrupts: true, callback: delegate(Sound s) {  });
            sound.playing = !sound.playing;
        }

        private void OnDestroy()
        {
            try
            {
                EventManager.Instance.RemoveListener<PlayCallbackSoundEvent>(PlaySound);
            }
            catch (Exception)
            {
                Debug.Log("Tried to destroy null object");
            }
        }
    }
}
