using System;
using System.IO;
using System.Media;
using System.Text;

namespace PriceComp.GUI
{
    public static class SoundGenerator
    {
        public static void PlaySound(int frequency = 440, int durationMs = 200, double volume = 0.5, bool isTriangle = false)
        {
            Task.Run(() =>
            {
                try
                {
                    using (var stream = GenerateWav(frequency, durationMs, volume, isTriangle))
                    using (var player = new SoundPlayer(stream))
                    {
                        player.PlaySync();
                    }
                }
                catch { } // Ignore errors
            });
        }

        public static void PlaySuccess()
        {
            // A pleasant high-pitched rising chime (Major Triad: C5, E5, G5)
            Task.Run(() =>
            {
                PlayToneSync(523, 100, 0.4); // C5
                PlayToneSync(659, 100, 0.4); // E5
                PlayToneSync(784, 200, 0.4); // G5
            });
        }

        public static void PlayError()
        {
            // Low buzz
            PlaySound(150, 300, 0.5);
        }

        public static void PlayClick()
        {
             // Short tick
             PlaySound(800, 50, 0.3);
        }

        private static void PlayToneSync(int freq, int ms, double vol)
        {
             using (var stream = GenerateWav(freq, ms, vol))
             using (var player = new SoundPlayer(stream))
             {
                 player.PlaySync();
             }
        }

        private static MemoryStream GenerateWav(int frequency, int durationMs, double volume, bool isTriangle = false)
        {
            int sampleRate = 44100;
            int numSamples = (int)(sampleRate * durationMs / 1000.0);
            short[] data = new short[numSamples];
            double amplitude = volume * short.MaxValue;

            for (int i = 0; i < numSamples; i++)
            {
                double t = (double)i / sampleRate;
                
                // Attack and Decay envelope to avoid clicking
                double envelope = 1.0;
                int attackSamples = 500;
                int decaySamples = 500;
                
                if (i < attackSamples) envelope = (double)i / attackSamples;
                else if (i > numSamples - decaySamples) envelope = (double)(numSamples - i) / decaySamples;

                double sampleValue;
                if (isTriangle)
                {
                     // Triangle wave
                     double period = 1.0 / frequency;
                     double timeInPeriod = t % period;
                     sampleValue = 4 * amplitude / period * Math.Abs(timeInPeriod - period / 2) - amplitude;
                }
                else
                {
                    // Sine wave
                    sampleValue = amplitude * Math.Sin(2 * Math.PI * frequency * t);
                }

                data[i] = (short)(sampleValue * envelope);
            }

            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            // RIFF header
            writer.Write(Encoding.ASCII.GetBytes("RIFF"));
            writer.Write(36 + numSamples * 2);
            writer.Write(Encoding.ASCII.GetBytes("WAVE"));

            // fmt chunk
            writer.Write(Encoding.ASCII.GetBytes("fmt "));
            writer.Write(16); // Subchunk1Size
            writer.Write((short)1); // AudioFormat (PCM)
            writer.Write((short)1); // NumChannels (Mono)
            writer.Write(sampleRate);
            writer.Write(sampleRate * 2); // ByteRate
            writer.Write((short)2); // BlockAlign
            writer.Write((short)16); // BitsPerSample

            // data chunk
            writer.Write(Encoding.ASCII.GetBytes("data"));
            writer.Write(numSamples * 2);

            foreach (var sample in data)
            {
                writer.Write(sample);
            }

            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
