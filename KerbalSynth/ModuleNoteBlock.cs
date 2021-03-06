﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KerbalSynth
{
    public class ModuleNoteBlock : PartModule
    {

        // Note Settings

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0110",
            groupName = "noteGroup", groupDisplayName = "#LOC_KSynth_0100"),
            UI_Cycle(controlEnabled = true, affectSymCounterparts = UI_Scene.None,
            stateNames = new string[] { "#LOC_KSynth_0111", "#LOC_KSynth_0112" })]
        public int pitchModeIndex = 1;

        [KSPAxisField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0120", 
            minValue = 0, maxValue = 87,
            groupName = "noteGroup"),
            UI_FloatRange(stepIncrement = 0.1f, maxValue = 87f, minValue = 0f, affectSymCounterparts = UI_Scene.None)]
        public float noteIndex = 39;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0130",
            groupName = "noteGroup"),
            UI_ChooseOption(controlEnabled = true, affectSymCounterparts = UI_Scene.None, 
            options = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8" })]
        public int noteOctaveIndex = 4;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0140",
            groupName = "noteGroup"),
            UI_ChooseOption(controlEnabled = true, affectSymCounterparts = UI_Scene.None,
            options = new string[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" })]
        public int noteNameIndex = 0;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0150",
            groupName = "noteGroup"),
            UI_Cycle(controlEnabled = true, affectSymCounterparts = UI_Scene.None,
            stateNames = new string[] { "#LOC_KSynth_0151", "#LOC_KSynth_0152", "#LOC_KSynth_0153", "#LOC_KSynth_0154", "#LOC_KSynth_0155" })]
        public int waveformIndex = 3;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0160",
            groupName = "noteGroup"),
            UI_ChooseOption(controlEnabled = true, affectSymCounterparts = UI_Scene.None, options = new string[] { "#LOC_KSynth_0161" })]
        public int harmonicIndex = 0;

        // Amplitude settings

        [KSPAxisField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0210",
            groupName = "amplitudeGroup", groupDisplayName = "#LOC_KSynth_0200"),
            UI_FloatRange(stepIncrement = 0.01f, maxValue = 1f, minValue = 0f, affectSymCounterparts = UI_Scene.None)]
        public float amplitude = 0.5f;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0220",
            groupName = "amplitudeGroup"),
            UI_Cycle(controlEnabled = true, affectSymCounterparts = UI_Scene.None,
            stateNames = new string[] { "#LOC_KSynth_0221", "#LOC_KSynth_0222", "#LOC_KSynth_0223" })]
        public int decayIndex = 2;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0230",
            groupName = "amplitudeGroup"),
            UI_FloatRange(stepIncrement = 0.1f, maxValue = 100f, minValue = 1f, affectSymCounterparts = UI_Scene.None)]
        public float range = 10f;

        //Color

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0310",
            groupName = "colorGroup", groupDisplayName = "#LOC_KSynth_0300"),
            UI_Cycle(controlEnabled = true, affectSymCounterparts = UI_Scene.None,
            stateNames = new string[] { "#LOC_KSynth_0311", "#LOC_KSynth_0312", "#LOC_KSynth_0313", "#LOC_KSynth_0314" })]
        public int colorMode = 0;

        [KSPAxisField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0320",
            groupName = "colorGroup"),
            UI_FloatRange(stepIncrement = 0.01f, maxValue = 1f, minValue = 0f, affectSymCounterparts = UI_Scene.None)]
        public float H = 0.3f;
        [KSPAxisField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0330",
            groupName = "colorGroup"),
            UI_FloatRange(stepIncrement = 0.01f, maxValue = 1f, minValue = 0f, affectSymCounterparts = UI_Scene.None)]
        public float S = 1.0f;
        [KSPAxisField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0340",
            groupName = "colorGroup"),
            UI_FloatRange(stepIncrement = 0.01f, maxValue = 1f, minValue = 0f, affectSymCounterparts = UI_Scene.None)]
        public float V = 1.0f;

        //Attack & Release

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0410",
            groupName = "attackReleaseGroup", groupDisplayName = "#LOC_KSynth_0400"), 
            UI_MinMaxRange(stepIncrement = 0.001f, affectSymCounterparts = UI_Scene.None, 
            maxValueX = 1f, maxValueY = 1f, minValueX = 0f, minValueY = 0f)]
        public Vector2 attackAndRelease = new Vector2(0, 0.05f);

        [KSPAxisField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0420",
            groupName = "attackReleaseGroup"),
            UI_FloatRange(stepIncrement = 0.01f, maxValue = 1f, minValue = 0f, affectSymCounterparts = UI_Scene.None)]
        public float attackAmplitude = 1f;

        [KSPAxisField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0430",
            minValue = 0, maxValue = 10,
            groupName = "attackReleaseGroup"),
            UI_FloatRange(stepIncrement = 0.01f, maxValue = 10f, minValue = 0f, affectSymCounterparts = UI_Scene.None)]
        public float length = 0.8f;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0440",
            groupName = "attackReleaseGroup"),
            UI_Toggle(controlEnabled = true, disabledText = "#LOC_KSynth_0442", enabledText = "#LOC_KSynth_0441")]
        public bool allowMouse = true;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#LOC_KSynth_0450",
            groupName = "attackReleaseGroup"),
            UI_Toggle(controlEnabled = true, disabledText = "#LOC_KSynth_0452", enabledText = "#LOC_KSynth_0451")]
        public bool allowCollision = false;

        //misc

        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public bool isPlaying = false;

        private Buzzer.Waveform waveform;
        private bool mouseWasDown = false;
        private float decay;
        private float pan;

        private Coroutine attackReleaseCoroutine = null;
        private Buzzer buzzer;
        private PartModule colorModule;
        private BaseField colorModuleR;
        private BaseField colorModuleG;
        private BaseField colorModuleB;

        [KSPEvent(guiActive = true, guiActiveEditor = false)]
        public void PlayStart()
        {
            if (buzzer == null)
            {
                buzzer = AudioManager.instance.AllocateBuzzer();
                if (buzzer == null)
                {
                    AudioManager.LogWarning("Max buzzers exceeded, can not create more");
                    // not available
                    return;
                }
                AudioManager.Log("playing " + noteIndex);
                waveform = (Buzzer.Waveform)waveformIndex;
                SyncFrequency();
                CheckDecay();
                buzzer.AmplitudeTarget = amplitude * decay;
                buzzer.Pan = pan;
                buzzer.Wave = waveform;
                buzzer.Play();
            }
            isPlaying = true;
            CheckPlayVisibility();
        }

        [KSPEvent(guiActive = true, guiActiveEditor = false)]
        public void PlayStop()
        {
            if (buzzer != null)
            {
                AudioManager.Log("stopping " + noteIndex);
                buzzer.Stop();
                buzzer = null;
            }
            isPlaying = false;
            CheckPlayVisibility();
        }

        public void PlayToggle()
        {
            if (buzzer == null)
            {
                PlayStart();
            }
            else
            {
                PlayStop();
            }
        }

        [KSPEvent(guiActive = true, guiActiveEditor = false)]
        public void TriggerAttack()
        {
            if (attackReleaseCoroutine != null)
            {
                StopCoroutine(attackReleaseCoroutine);
            }
            attackReleaseCoroutine = StartCoroutine(TriggerAttackCoroutine());
        }

        [KSPEvent(guiActive = true, guiActiveEditor = false)]
        public void TriggerRelease()
        {
            if (attackReleaseCoroutine != null)
            {
                StopCoroutine(attackReleaseCoroutine);
            }
            attackReleaseCoroutine = StartCoroutine(TriggerReleaseCoroutine());
        }

        [KSPEvent(guiActive = true, guiActiveEditor = false)]
        public void TriggerAttackRelease()
        {
            if (attackReleaseCoroutine != null)
            {
                StopCoroutine(attackReleaseCoroutine);
            }
            attackReleaseCoroutine = StartCoroutine(TriggerAttackReleaseCoroutine());
        }

        private IEnumerator TriggerAttackReleaseCoroutine()
        {
            float t = 0;
            while (t < length)
            {
                float attackTime = attackAndRelease.x * length;
                float releaseTime = attackAndRelease.y * length;
                if (t <= attackTime)
                {
                    if (t == 0) amplitude = 0;
                    else amplitude = Mathf.Pow(t / attackTime, 2) * attackAmplitude;
                }
                else if (t <= releaseTime)
                {
                    amplitude = attackAmplitude;
                }
                else
                {
                    amplitude = Mathf.Pow((length - t) / (length - releaseTime), 2) * attackAmplitude;
                }
                yield return 0;
                t += TimeWarp.deltaTime;
            }
            amplitude = 0;
        }

        private IEnumerator TriggerAttackCoroutine()
        {
            float t = 0;
            float attackTime = attackAndRelease.x * length;
            while (t < attackTime)
            {
                if (t == 0)
                {
                    amplitude = 0;
                }
                else
                {
                    amplitude = Mathf.Pow(t / attackTime, 2) * attackAmplitude;
                }
                yield return 0;
                t += TimeWarp.deltaTime;
            }
            amplitude = attackAmplitude;
        }

        private IEnumerator TriggerReleaseCoroutine()
        {
            float t = 0;
            float releaseTime = attackAndRelease.y * length;
            t = releaseTime;
            while (t < length)
            {
                amplitude = Mathf.Pow((length - t) / (length - releaseTime), 2) * attackAmplitude;
                yield return 0;
                t += TimeWarp.deltaTime;
            }
            amplitude = 0;
        }

        [KSPAction("PlayToggle")]
        public void ActionPlayToggle(KSPActionParam param)
        {
            PlayToggle();
        }

        [KSPAction("PlayStart")]
        public void ActionPlayStart(KSPActionParam param)
        {
            PlayStart();
        }

        [KSPAction("PlayStop")]
        public void ActionPlayStop(KSPActionParam param)
        {
            PlayStop();
        }

        [KSPAction("TriggerAttackRelease")]
        public void ActionTriggerAttackRelease(KSPActionParam param)
        {
            TriggerAttackRelease();
        }

        [KSPAction("TriggerAttack")]
        public void ActionTriggerAttack(KSPActionParam param)
        {
            TriggerAttack();
        }

        [KSPAction("TriggerRelease")]
        public void ActionTriggerRelease(KSPActionParam param)
        {
            TriggerRelease();
        }

        public void CheckDecay()
        {
            var min_distance = range / 2f;
            var cam = Camera.main.transform;
            var d = part.transform.position - cam.position;
            var distance = Mathf.Max(d.magnitude, min_distance);
            switch (decayIndex)
            {
                case 0://None
                    decay = 1;
                    break;
                case 1://distance
                    decay = min_distance / distance;
                    break;
                case 2://atm
                    decay = min_distance / distance * Mathf.Min(1, (float)part.atmDensity);
                    break;
            }
            pan = Vector3.Dot(cam.right, d.normalized);
        }

        public void Start()
        {
            colorModule = part.Modules["ModuleCustomColoredEmissive"];
            if (colorModule != null)
            {
                colorModuleR = colorModule.Fields["red"];
                colorModuleG = colorModule.Fields["green"];
                colorModuleB = colorModule.Fields["blue"];
            }

            Fields["pitchModeIndex"].OnValueModified += o =>
            {
                CheckNoteVisibility();
            };
            Fields["noteIndex"].OnValueModified += o =>
            {
                if (pitchModeIndex == 0 && !PitchConsistency())
                {
                    Utils.IndexToOctave(Mathf.RoundToInt(noteIndex), out noteOctaveIndex, out noteNameIndex);
                }
                SyncFrequency();
            };
            Fields["amplitude"].OnValueModified += o =>
            {
                if (buzzer != null)
                {
                    buzzer.AmplitudeTarget = amplitude * decay;
                }
            };
            Fields["waveformIndex"].OnValueModified += o =>
            {
                if (buzzer != null)
                {
                    waveform = (Buzzer.Waveform)waveformIndex;
                    buzzer.Wave = waveform;
                    SyncHarmonics();
                }
                CheckHarmonicsVisibility();
            };
            Fields["harmonicIndex"].OnValueModified += o =>
            {
                if (buzzer != null)
                {
                    SyncHarmonics();
                }
            };
            Fields["decayIndex"].OnValueModified += o =>
            {
                CheckDecayVisibility();
            };

            Callback<object> genIndex = o =>
            {
                if (pitchModeIndex == 1 && !PitchConsistency())
                {
                    noteIndex = Mathf.Clamp(Utils.OctaveToIndex(noteOctaveIndex, noteNameIndex), 0, 87);
                }
                SyncFrequency();
            };
            Fields["noteOctaveIndex"].OnValueModified += genIndex;
            Fields["noteNameIndex"].OnValueModified += genIndex;

            if (colorModule != null)
            {
                Fields["colorMode"].OnValueModified += o =>
                {
                    //hide color settings when using auto color mode
                    CheckColorVisibility();
                };
            }

            if (AudioManager.instance.tones != null && AudioManager.instance.tones.Count != 0)
            {
                harmonicIndex = Math.Min(harmonicIndex, AudioManager.instance.tones.Count);
                var names = AudioManager.instance.tones.Select(tone => tone.name).ToArray();
                ((UI_ChooseOption)Fields["harmonicIndex"].uiControlEditor).options = names;
                ((UI_ChooseOption)Fields["harmonicIndex"].uiControlFlight).options = names;
            }
            else
            {
                harmonicIndex = 0;
            }

            if (isPlaying) PlayStart();
            else PlayStop();

            genIndex.Invoke(null);
            CheckDecayVisibility();
            CheckColorVisibility();
            CheckHarmonicsVisibility();
            CheckNoteVisibility();
        }

        public void Update()
        {
            if (isPlaying)
            {
                CheckDecay();
                buzzer.AmplitudeTarget = amplitude * decay;
                buzzer.Pan = pan;
            }

            if (colorModule != null)
            {
                if (isPlaying || HighLogic.LoadedSceneIsEditor)
                {
                    Color color;
                    switch (colorMode)
                    {
                        case 0:
                            color = Color.HSVToRGB(H, S, V);
                            break;
                        case 1://H
                            color = Color.HSVToRGB(noteIndex / 87f, S, V);
                            break;
                        case 2://V
                            color = Color.HSVToRGB(H, S, amplitude);
                            break;
                        case 3://H&V
                        default:
                            color = Color.HSVToRGB(noteIndex / 87f, S, amplitude);
                            break;
                    }
                    SetColor(color);
                }
                else
                {
                    SetColor(Color.black);
                }
            }

            var mouseIsDown = Mouse.Left.GetButton() && Mouse.HoveredPart == this.part;
            if (mouseIsDown && !mouseWasDown)
            { // Enter
                if (allowMouse && HighLogic.LoadedSceneIsFlight)
                {
                    PlayStart();
                    TriggerAttack();
                }
            }
            if (mouseWasDown && !mouseIsDown)
            { // Leave
                if (allowMouse && HighLogic.LoadedSceneIsFlight)
                {
                    TriggerRelease();
                }
            }
            mouseWasDown = mouseIsDown;
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (allowCollision)
            {
                PlayStart();
                TriggerAttackRelease();
            }
        }

        public void OnDestroy()
        {
            PlayStop();
        }

        private void SyncFrequency()
        {
            if (buzzer != null)
            {
                buzzer.Frequency = Utils.NoteToFrequency(noteIndex);
                SyncHarmonics();
            }
        }

        private void SyncHarmonics()
        {
            if (AudioManager.instance.tones != null && AudioManager.instance.tones.Count != 0)
            {
                var currentTone = AudioManager.instance.tones[harmonicIndex];
                var minLength = currentTone.MaxHarmonicCount;
                if (buzzer.HarmonicArray == null || buzzer.HarmonicArray.Length < minLength)
                {
                    buzzer.HarmonicArray = new Tuple<float, float>[minLength];
                }
                currentTone.GetBlended(noteIndex, buzzer.HarmonicArray);
            }
            else
            {
                buzzer.HarmonicArray = null;
            }
        }

        private bool PitchConsistency()
        {
            return Utils.OctaveToIndex(noteOctaveIndex, noteNameIndex) == noteIndex;
        }

        private void CheckNoteVisibility()
        {
            if (pitchModeIndex == 0)
            {
                SetGui("noteIndex", true);
                SetGui("noteOctaveIndex", false);
                SetGui("noteNameIndex", false);
            }
            else
            {
                SetGui("noteIndex", false);
                SetGui("noteOctaveIndex", true);
                SetGui("noteNameIndex", true);
            }
        }

        private void CheckHarmonicsVisibility()
        {
            if (waveformIndex == 4)
            {
                SetGui("harmonicIndex", true);
            }
            else
            {
                SetGui("harmonicIndex", false);
            }
        }

        private void CheckPlayVisibility()
        {
            if (isPlaying)
            {
                Events["PlayStart"].guiActive = false;
                Events["PlayStop"].guiActive = true;
            }
            else
            {
                Events["PlayStart"].guiActive = true;
                Events["PlayStop"].guiActive = false;
            }

        }

        private void CheckDecayVisibility()
        {
            if (decayIndex == 0)
            {
                SetGui("range", false);
            }
            else
            {
                SetGui("range", true);
            }
        }

        private void CheckColorVisibility()
        {
            if (colorModule == null)
            {
                SetGui("colorMode", false);
                SetGui("H", false);
                SetGui("S", false);
                SetGui("V", false);
            }
            else
            {
                SetGui("colorMode", true);
                switch (colorMode)
                {
                    case 0:
                        SetGui("H", true);
                        SetGui("S", true);
                        SetGui("V", true);
                        break;
                    case 1://h
                        SetGui("H", false);
                        SetGui("S", true);
                        SetGui("V", true);
                        break;
                    case 2://v
                        SetGui("H", true);
                        SetGui("S", true);
                        SetGui("V", false);
                        break;
                    case 3://hv
                        SetGui("H", false);
                        SetGui("S", true);
                        SetGui("V", false);
                        break;
                }
            }
            colorModuleR.guiActive = false;
            colorModuleG.guiActive = false;
            colorModuleB.guiActive = false;
            colorModuleR.guiActiveEditor = false;
            colorModuleG.guiActiveEditor = false;
            colorModuleB.guiActiveEditor = false;
        }

        private void SetColor(Color color)
        {
            colorModuleR.SetValue(color.r, colorModuleR.host);
            colorModuleG.SetValue(color.g, colorModuleG.host);
            colorModuleB.SetValue(color.b, colorModuleB.host);
            colorModuleR.uiControlEditor.onFieldChanged.Invoke(colorModuleR, 0);
            colorModuleG.uiControlEditor.onFieldChanged.Invoke(colorModuleG, 0);
            colorModuleB.uiControlEditor.onFieldChanged.Invoke(colorModuleB, 0);
        }

        private void SetEditorGui(string fieldName, bool visible)
        {
            var f = Fields[fieldName];
            if (f != null)
            {
                f.guiActive = visible;
            }
        }

        private void SetFlightGui(string fieldName, bool visible)
        {
            var f = Fields[fieldName];
            if (f != null)
            {
                f.guiActiveEditor = visible;
            }
        }

        private void SetGui(string fieldName, bool visible)
        {
            var f = Fields[fieldName];
            if (f != null)
            {
                f.guiActive = visible;
                f.guiActiveEditor = visible;
            }
        }
    }
}
