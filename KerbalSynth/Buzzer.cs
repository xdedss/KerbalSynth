using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KerbalSynth
{
    public class Buzzer
    {
        private const float PAN_MIN = 0.3f;

        public float Frequency
        {
            get { return _freq; }
            set { _freq = value; df = 2 * Mathf.PI * Frequency * dt; }
        }
        public float AmplitudeTarget { get; set; }
        public float Pan
        {
            get { return _pan; }
            set
            {
                _pan = Mathf.Clamp(value, -1, 1);
                _l = (1 + PAN_MIN) / 2f - (1 - PAN_MIN) / 2f * Mathf.Sin(Pan * Mathf.PI / 2f);
                _r = (1 + PAN_MIN) / 2f + (1 - PAN_MIN) / 2f * Mathf.Sin(Pan * Mathf.PI / 2f);
            }
        }
        public Waveform Wave
        {
            get { return _wave; }
            set { _wave = value; UpdateSampler(); }
        }
        public Tuple<float, float>[] HarmonicArray { get; set; }
        public bool Idle { get; private set; }


        private Waveform _wave;
        private float _pan, _l, _r;
        private float _freq;


        public enum Waveform
        {
            Sin,
            Saw,
            Square,
            Triangle,
            Harmonics,
        }

        
        private Func<float, float> sampler;

        private int sampleRate;
        private float dt;
        private float df;
        private float phase = 0;

        private float envelope = 0;
        private float envelopeTarget = 1;
        private float envelopeVelocity = 25;

        private float amplitude = 0;
        private float amplitudeVelocity = 50;

        public Buzzer()
        {
            sampleRate = AudioSettings.outputSampleRate;
            dt = 1f / sampleRate;
            Frequency = 1000;
            AmplitudeTarget = 0.25f;
            Pan = 0;
            Wave = Waveform.Triangle;
            Idle = true;
        }

        public void GenerateAudio(float[] data, int channels)
        {
            int dataLen = data.Length / channels;
            if (channels > 1)
            {
                for (int n = 0; n < dataLen; n++)
                {
                    float f = amplitude * envelope * sampler(phase);
                    data[n * channels + 0] += f * _l;
                    data[n * channels + 1] += f * _r;
                    phase += df * TimeWarp.CurrentRate;
                    if (phase >= 2 * Mathf.PI) phase -= 2 * Mathf.PI;
                    amplitude = Mathf.MoveTowards(amplitude, AmplitudeTarget, dt * amplitudeVelocity);
                    envelope = Mathf.MoveTowards(envelope, envelopeTarget, dt * envelopeVelocity);
                    if (envelope == 0)
                    {
                        Idle = true;
                    }
                }
            }
            else
            {
                for (int n = 0; n < dataLen; n++)
                {
                    float f = amplitude * envelope * sampler(phase);
                    data[n * channels] += f;
                    phase += df * TimeWarp.CurrentRate;
                    if (phase >= 2 * Mathf.PI) phase -= 2 * Mathf.PI;
                    amplitude = Mathf.MoveTowards(amplitude, AmplitudeTarget, dt * amplitudeVelocity);
                    envelope = Mathf.MoveTowards(envelope, envelopeTarget, dt * envelopeVelocity);
                    if (envelope == 0)
                    {
                        Idle = true;
                    }
                }
            }
            //AudioManager.Log("buz" + data.Length + " " + channels + " " + df);
        }

        public void Play()
        {
            Idle = false;
            envelope = 0;
            envelopeTarget = 1;
        }

        public void Stop()
        {
            envelopeTarget = 0;
        }

        private void UpdateSampler()
        {
            switch (_wave)
            {
                case Waveform.Saw:
                    sampler = Samplers.Saw;
                    break;
                case Waveform.Square:
                    sampler = Samplers.Square;
                    break;
                case Waveform.Triangle:
                    sampler = Samplers.Triangle;
                    break;
                case Waveform.Sin:
                    sampler = Samplers.Sin;
                    break;
                case Waveform.Harmonics:
                    if (HarmonicArray != null) sampler = phase => Samplers.Harmonic(phase, HarmonicArray);
                    else sampler = Samplers.Sin;
                    break;
            }
        }
    }
}
