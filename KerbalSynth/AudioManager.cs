using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;

namespace KerbalSynth
{
    [KSPAddon(KSPAddon.Startup.FlightAndEditor, false)]
    public class AudioManager : MonoBehaviour
    {

        public static AudioManager instance;

        public List<Tone> tones = new List<Tone>();

        public List<Buzzer> buzzers = new List<Buzzer>();
        public int maxBuzzers = 64;

        private AudioSource source;
        private bool canPlay = false;

        private Regex regexNumber = new Regex(@"((0|([1-9]\d*))(\.\d+)?)|(\.\d+)", RegexOptions.IgnoreCase);

        public void Awake()
        {
            instance = this;
            source = gameObject.AddComponent<AudioSource>();
            var clip = AudioClip.Create("dummy", 44100, 2, 44100, true);
            source.clip = clip;
            source.loop = true;
            LoadTones();
        }

        public void Start()
        {
            Log("AudioManager started");
            source.Play();
        }

        public void Update()
        {
            canPlay = (!FlightDriver.Pause) && TimeWarp.CurrentRate <=4;
        }

        public void OnAudioFilterRead(float[] data, int channels)
        {
            if (canPlay)
            {
                buzzers.ForEach(b =>
                {
                    if (!b.Idle) b.GenerateAudio(data, channels);
                });
            }
        }

        public Buzzer AllocateBuzzer()
        {
            //Log("someone is requiring a buzzer");
            var res = buzzers.FirstOrDefault(b => b.Idle);
            //Log("found " + res);
            if (res == null)
            {
                if (buzzers.Count < maxBuzzers)
                {
                    res = new Buzzer();
                    buzzers.Add(res);
                }
                else
                {
                    return null;
                }
            }
            return res;
        }

        private void LoadTones()
        {
            string folder = Path.Combine(Environment.CurrentDirectory, "GameData/KerbalSynth/Tones");
            if (Directory.Exists(folder))
            {
                var configFiles = Directory.EnumerateFiles(folder).Where(f => Path.GetExtension(f) == ".cfg");
                tones.Clear();
                foreach (var f in configFiles)
                {
                    using (StreamReader reader = new StreamReader(f))
                    {
                        string text = reader.ReadToEnd();
                        var root = CrappyCfgParser.ParseNode(text, "root");
                        var parsedTones = ParseCfgNode(root);
                        parsedTones.ForEach(tone => tones.Add(tone));
                        Log(string.Format("Parsed {0} tones in file {1}", parsedTones.Count, Path.GetFileName(f)));
                    }
                }
                Log(string.Format("Parsed {0} tones.", tones.Count));
            }
            else
            {
                LogWarning("Folder doesnot exist : " + folder);
            }
        }

        public static List<Tone> ParseCfgNode(CrappyCfgParser.CfgNode root)
        {
            var res = new List<Tone>();
            var toneEnumerator = root.EnumerateNode("tone");
            while (toneEnumerator.MoveNext())
            {
                var toneNode = toneEnumerator.Current;
                if (toneNode.ValueCount("name") == 0)
                {
                    LogWarning("TONE Name Not Found!");
                    continue;
                }
                var tone = new Tone(toneNode.values.First(v => v.name == "name").value);
                var harmonicsEnumerator = toneNode.EnumerateNode("harmonics");
                while (harmonicsEnumerator.MoveNext())
                {
                    var harmonicsNode = harmonicsEnumerator.Current;
                    float harmonicsIndex = 0;
                    if (harmonicsNode.ValueCount("note") == 0 || 
                        !float.TryParse(harmonicsNode.values.First(v => v.name == "note").value, out harmonicsIndex))
                    {
                        LogWarning("HARMONICS Note Index Not Found!");
                        continue;
                    }
                    var harmonics = new Harmonics(harmonicsIndex);
                    var harmonicList = new List<Tuple<float, float>>();
                    var harmonicEnumerator = harmonicsNode.EnumerateValue("harmonic");
                    while (harmonicEnumerator.MoveNext()) {
                        var harmonicValue = harmonicEnumerator.Current;
                        Vector2 harmonicV2 = Vector2.zero;
                        Utils.TryParseV2(harmonicValue.value, out harmonicV2);
                        harmonicList.Add(new Tuple<float, float>(harmonicV2.x, harmonicV2.y));
                    }
                    if (harmonicList.Count == 0)
                    {
                        harmonicList.Add(new Tuple<float, float>(1, 0));
                        LogWarning("HARMONICS no definition found, adding default sin wave");
                    }
                    harmonics.harmonicArray = harmonicList.ToArray();
                    tone.SortedInsert(harmonics);
                }
                res.Add(tone);
            }
            return res;
        }

        public static void Log(object msg)
        {
            Debug.Log("[KerbalSynth]" + msg.ToString());
        }

        public static void LogWarning(object msg)
        {
            Debug.LogWarning("[KerbalSynth][Warning]" + msg.ToString());
        }

    }
}
