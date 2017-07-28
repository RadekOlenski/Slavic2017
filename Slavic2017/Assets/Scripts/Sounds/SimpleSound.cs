using System;
using Events;
using Managers;
using UnityEngine;

namespace Sounds
{
    public class SimpleSound : MonoBehaviour
    {
        private void Start()
        {
            EventManager.Instance.AddListener<PlaySimpleSoundEvent>(PlaySound);
            EventManager.Instance.AddListener<PlaySimpleSoundWithPitchEvent>(PlaySoundWithPitch);
        }

        private void PlaySound(PlaySimpleSoundEvent e)
        {
            AudioManager.Main.PlayNewSound(e.SoundToPlay);
        }

        private void PlaySoundWithPitch(PlaySimpleSoundWithPitchEvent e)
        {
            AudioManager.Main.PlayNewSoundWithCustomPitch(e.SoundToPlay, e.Pitch);
        }

        private void OnDestroy()
        {
            try
            {
                EventManager.Instance.RemoveListener<PlaySimpleSoundEvent>(PlaySound);
            }
            catch (Exception)
            {
                Debug.Log("Tried to destroy null object");
            }

            try
            {
                EventManager.Instance.RemoveListener<PlaySimpleSoundWithPitchEvent>(PlaySoundWithPitch);
            }
            catch (Exception)
            {
                Debug.Log("Tried to destroy null object");
            }
        }
    }
}
