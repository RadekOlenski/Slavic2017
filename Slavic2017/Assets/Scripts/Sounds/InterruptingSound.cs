using System;
using Events;
using Managers;
using UnityEngine;

namespace Sounds
{
    public class InterruptingSound : MonoBehaviour {
        private Sound sound;

        private void Start()
        {
            EventManager.Instance.AddListener<PlayInterruptingSoundEvent>(PlaySound);
        }

        private void PlaySound(PlayInterruptingSoundEvent e)
        {
            sound = AudioManager.Main.NewSound(e.SoundToPlay, interrupts: true);
            sound.playing = !sound.playing;
        }

        private void OnDestroy()
        {
            try
            {
                EventManager.Instance.RemoveListener<PlayInterruptingSoundEvent>(PlaySound);
            }
            catch (Exception)
            {
                Debug.Log("Tried to destroy null object");
            }
        }
    }
}
