using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KerbalSynth
{
    public static class Utils
    {
        public static float NoteToFrequency(float noteIndex)
        {
            return 27.5f * Mathf.Pow(2, noteIndex / 12f);
        }

        public static Tuple<float, float> BlendPhasor(Tuple<float, float> p1, Tuple<float, float> p2, float t)
        {
            float real = p1.Item1 * Mathf.Cos(p1.Item2) * (1 - t) + p2.Item1 * Mathf.Cos(p2.Item2) * t;
            float imaginary = p1.Item1 * Mathf.Sin(p1.Item2) * (1 - t) + p2.Item1 * Mathf.Sin(p2.Item2) * t;
            return new Tuple<float, float>(Mathf.Sqrt(real * real + imaginary * imaginary), Mathf.Atan2(imaginary, real));
        }

        public static int OctaveToIndex(int octaveIndex, int nameIndex)
        {
            return octaveIndex * 12 + nameIndex - 9;
        }

        public static void IndexToOctave(int index, out int octaveIndex, out int nameIndex)
        {
            nameIndex = (index + 9) % 12;
            octaveIndex = (index + 9) / 12;
        }

        public static bool TryParseV2(string str, out Vector2 v2)
        {
            v2 = Vector2.zero;
            float x, y;
            var splitted = str.Split(',');
            if (splitted.Length != 2) return false;
            if (float.TryParse(splitted[0], out x) && float.TryParse(splitted[1], out y))
            {
                v2 = new Vector2(x, y);
                return true;
            }
            return false;
        }

        public static int SearchBracketPair(string str, int startIndex, char bracketLeft = '{', char bracketRight = '}')
        {
            startIndex++;
            while (startIndex < str.Length)
            {
                if (str[startIndex] == bracketRight)
                {
                    return startIndex;
                }
                else if (str[startIndex] == bracketLeft)
                {
                    startIndex = SearchBracketPair(str, startIndex);
                }
                startIndex++;
            }
            return startIndex;
        }

        public static bool isSpace(char c)
        {
            return c == ' ' || c == '\t';
        }

        public static bool isLine(char c)
        {
            return c == '\n' || c == '\r';
        }

        public static bool isSomething(char c)
        {
            return !isSpace(c) && !isLine(c);
        }

    }


    public class Tone
    {
        public string name;
        public List<Harmonics> harmonicsList = new List<Harmonics>();

        public Tone(string name)
        {
            this.name = name;
        }

        public int MaxHarmonicCount
        {
            get
            {
                return harmonicsList.Count == 0 ? 1 : harmonicsList.Max(h => h.harmonicArray.Length);
            }
        }

        public Tuple<float, float>[] GetBlended(float index, Tuple<float, float>[] buffer)
        {
            if (harmonicsList.Count == 0)
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = i == 0 ? new Tuple<float, float>(1, 0) : new Tuple<float, float>(0, 0);
                }
            }
            // list should be already sorted (low-high)
            Harmonics leftHarmonics = harmonicsList.First();
            Harmonics rightHarmonics = harmonicsList.Last();
            foreach (var harmonics in harmonicsList)
            {
                if (harmonics.centerIndex <= index)
                {
                    leftHarmonics = harmonics;
                }
                else
                {
                    rightHarmonics = harmonics;
                    break;
                }
            }
            var leftList = leftHarmonics.harmonicArray;
            var rightList = rightHarmonics.harmonicArray;
            //var res = new Tuple<float, float>[Math.Max(leftList.Length, rightList.Length)];
            var t = leftHarmonics.centerIndex == rightHarmonics.centerIndex ? 0 :
                ((index - leftHarmonics.centerIndex) / (rightHarmonics.centerIndex - leftHarmonics.centerIndex));
            for (int i = 0; i < buffer.Length; i++)
            {
                Tuple<float, float> l = i >= leftList.Length ? new Tuple<float, float>(0, 0) : leftList[i];
                Tuple<float, float> r = i >= rightList.Length ? new Tuple<float, float>(0, 0) : rightList[i];
                buffer[i] = Utils.BlendPhasor(l, r, t);
            }
            return buffer;
        }

        public void SortedInsert(Harmonics harmonics)
        {
            int i = 0;
            foreach (var h in harmonicsList)
            {
                if (h.centerIndex > harmonics.centerIndex)
                {
                    break;
                }
                i++;
            }
            harmonicsList.Insert(i, harmonics);
        }
    }

    public class Harmonics
    {
        public static Harmonics Default
        {
            get
            {
                return new Harmonics(39)
                {
                    harmonicArray = new Tuple<float, float>[] { new Tuple<float, float>(1, 0) }
                };
            }
        }
        public Harmonics(float index)
        {
            this.centerIndex = index;
        }
        public float centerIndex;
        public Tuple<float, float>[] harmonicArray;
    }


}
