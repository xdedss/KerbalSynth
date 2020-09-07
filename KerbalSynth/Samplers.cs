using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KerbalSynth
{
    public static class Samplers
    {
        public static float Sin(float phase)
        {
            return Mathf.Sin(phase);
        }

        public static float Saw(float phase)
        {
            return (phase / Mathf.PI + 1f) % 2 - 1f;
        }

        public static float Triangle(float phase)
        {
            if (phase < Mathf.PI * 0.5f) return phase / Mathf.PI * 2f;
            else if (phase < Math.PI * 1.5f) return -phase / Mathf.PI * 2f + 2f;
            else return phase / Mathf.PI * 2f - 4f;
        }

        public static float Square(float phase)
        {
            return phase > Mathf.PI ? 1f : -1f;
        }

        public static float Harmonic(float phase, Tuple<float, float>[] harmonics)
        {
            float res = 0;
            for (int i = 0; i < harmonics.Length; i++)
            {
                res += Mathf.Sin(phase * (i + 1) + harmonics[i].Item2) * harmonics[i].Item1;
            }
            return res;
        }
    }
}
