using System;
using System.IO;

namespace Vidiludo
{
    public class SoundEffect : Audio
    {
        private bool Random;

        private Int64 Volume;

        private int SoundIndex;
        private int SoundCount;

        private Stream[] Sounds;

        private Random Aleatory = new Random((int) DateTime.Now.Ticks);

        public SoundEffect(Stream[] Sounds, Int64 Volume, bool Random)
        {
            this.Sounds = Sounds;
            this.SoundCount = Sounds.Length;
            this.SoundIndex = SoundCount;

            this.Volume = Volume;

            this.Random = Random;
        }

        public void Play()
        {
            Play(Wave, Volume);
        }

        public void Play(Int64 Volume)
        {
            Play(Wave, Volume);
        }

        private Stream Wave
        {
            get
            {
                if (1 == SoundCount)
                {
                    return (Sounds[0]);
                }

                if (Random)
                {
                    return (Sounds[Aleatory.Next(SoundCount)]);
                }

//              then cycling
                {
                    Stream Wave = Sounds[SoundIndex %= SoundCount];

                    ++SoundIndex;

                    return (Wave);
                }
            }
        }
    }
}
