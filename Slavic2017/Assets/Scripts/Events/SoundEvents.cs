using System.Collections.Generic;
using Enums;

namespace Events
{
    public class PlayMusicLoopEvent : GameEvent
    {
        public string MusicToPlay { get; set; }

        public PlayMusicLoopEvent(string musicToPlay)
        {
            MusicToPlay = musicToPlay;

            args = new List<object> { MusicToPlay };
        }
    }

    public class PlaySimpleSoundEvent : GameEvent
    {
        public string SoundToPlay { get; set; }

        public PlaySimpleSoundEvent(string soundToPlay)
        {
            SoundToPlay = soundToPlay;

            args = new List<object> { SoundToPlay };
        }
    }

    public class PlaySimpleSoundWithPitchEvent : GameEvent
    {
        public string SoundToPlay { get; set; }
        public float Pitch { get; set; }

        public PlaySimpleSoundWithPitchEvent(string soundToPlay, float pitch = 1f)
        {
            SoundToPlay = soundToPlay;
            Pitch = pitch;
            args = new List<object> { SoundToPlay, Pitch };
        }
    }

    public class PlayInterruptingSoundEvent : GameEvent
    {
        public string SoundToPlay { get; set; }

        public PlayInterruptingSoundEvent(string soundToPlay)
        {
            SoundToPlay = soundToPlay;

            args = new List<object> { SoundToPlay };
        }
    }

    public class PlayCallbackSoundEvent : GameEvent
    {
        public string SoundToPlay { get; set; }

        public PlayCallbackSoundEvent(string soundToPlay)
        {
            SoundToPlay = soundToPlay;

            args = new List<object> { SoundToPlay };
        }
    }
}
