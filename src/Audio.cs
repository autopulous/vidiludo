using System;
using System.Threading;
using System.Media;
using System.IO;

namespace Vidiludo
{
    public class Audio
    {
        private static SoundPlayer player = new SoundPlayer();

        private static Int64 priority = 0;
        private static Stream stream = null;

        private static Thread thread = null;

        public void Play(Stream resourceStream, Int64 resourcePriority)
        {
            if (priority >= resourcePriority) return;

            if (null != thread) thread.Abort();

            thread = new Thread(Audio.Processor);

            priority = resourcePriority;
            stream = resourceStream;

            thread.Start();
        }

        private static void Processor() // used to be public
        {
            player.Stream = stream;

            player.PlaySync();

            priority = 0;
            stream = null;

            thread.Abort();
        }
    }
}
