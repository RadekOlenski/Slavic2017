using System;
using Enums;
using Events;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sounds
{
    public class LoopingMusic : MonoBehaviour
    {
        public Sound sound;

        private void Start()
        {
            EventManager.Instance.AddListener<PlayMusicLoopEvent>(PlayMusic);
            switch (SceneManager.GetActiveScene().name)
            {
                case ScenesEnum.Menu:
                {
                    PlayMusic(MusicEnum.MainTheme);
                    break;
                }
                default:
                {
                    PlayMusic(MusicEnum.LevelTheme);
                    break;
                }
            }
        }

        private void PlayMusic(PlayMusicLoopEvent e)
        {
            sound = AudioManager.Main.NewSound(e.MusicToPlay, loop: true);
            sound.playing = !sound.playing;
        }

        private void PlayMusic(string musicToPlay)
        {
            sound = AudioManager.Main.NewSound(musicToPlay, loop: true);
            sound.playing = !sound.playing;
        }

        private void OnDestroy()
        {
            try
            {
                EventManager.Instance.RemoveListener<PlayMusicLoopEvent>(PlayMusic);
            }
            catch (Exception)
            {
                Debug.Log("Tried to destroy null object");
            }
        }
    }
}