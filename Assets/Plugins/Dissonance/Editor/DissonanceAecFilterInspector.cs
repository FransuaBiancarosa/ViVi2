using System.Globalization;
using Dissonance.Audio.Capture;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Dissonance.Editor
{
    public class DissonanceAecFilterInspector
        : IAudioEffectPluginGUI
    {
        private bool _initialized;
        private Texture2D _logo;

        private void Initialize()
        {
            _logo = Resources.Load<Texture2D>("dissonance_logo");

            _initialized = true;
        }

        public override bool OnGUI([NotNull] IAudioEffectPlugin plugin)
        {
            if (!_initialized)
                Initialize();

            GUILayout.Label(_logo);
            EditorGUILayout.HelpBox("This filter captures data to drive acoustic echo cancellation. All audio which passes through this filter will be played through your " +
                                    "speakers, the filter will watch you microphone for this audio coming back as an echo and remove it", MessageType.Info);

            if (Application.isPlaying)
            {
                var state = WebRtcPreprocessingPipeline.GetAecFilterState();
                switch (state)
                {
                    case WebRtcPreprocessingPipeline.WebRtcPreprocessor.FilterState.FilterNoInstance:
                        EditorGUILayout.HelpBox("AEC filter is running, but it is not associated with a microphone preprocessor - Microphone not running?", MessageType.Info);
                        break;

                    case WebRtcPreprocessingPipeline.WebRtcPreprocessor.FilterState.FilterNoSamplesSubmitted:
                        EditorGUILayout.HelpBox("AEC filter is running, but no samples were submitted in the last frame - Could indicate audio thread starvation", MessageType.Warning);
                        break;

                    case WebRtcPreprocessingPipeline.WebRtcPreprocessor.FilterState.FilterNotRunning:
                        EditorGUILayout.HelpBox("AEC filter is not running - Audio device not initialized?", MessageType.Warning);
                        break;

                    case WebRtcPreprocessingPipeline.WebRtcPreprocessor.FilterState.FilterOk:
                        EditorGUILayout.HelpBox("AEC filter is running.", MessageType.Info);
                        break;

                    default:
                        EditorGUILayout.HelpBox("Unknown Filter State!", MessageType.Error);
                        break;
                }

                // `GetFloatBuffer` (a built in Unity method) causes a null reference exception when called. This bug seems to be limited to Unity 2019.3 on MacOS.
                // See tracking issue: https://github.com/Placeholder-Software/Dissonance/issues/177
#if (UNITY_EDITOR_OSX && UNITY_2019_3)
                EditorGUILayout.HelpBox("Cannot show detailed statistics in Unity 2019.3 due to an editor bug. Please update to Unity 2019.4 or newer!", MessageType.Error);
#else
                float[] data;
                if (plugin.GetFloatBuffer("AecMetrics", out data, 10))
                {
                    EditorGUILayout.LabelField(
                        "Delay Median (samples)",
                        FormatNumber(data[0])
                    );

                    EditorGUILayout.LabelField(
                        "Delay Deviation",
                        FormatNumber(data[1])
                    );

                    EditorGUILayout.LabelField(
                        "Fraction Poor Delays",
                        FormatPercentage(data[2])
                    );

                    EditorGUILayout.LabelField(
                        "Echo Return Loss",
                        FormatNumber(data[3])
                    );

                    EditorGUILayout.LabelField(
                        "Echo Return Loss Enhancement",
                        FormatNumber(data[6])
                    );

                    EditorGUILayout.LabelField(
                        "Residual Echo Likelihood",
                        FormatPercentage(data[9])
                    );
                }
#endif
            }

            return false;
        }

        [NotNull] private string FormatNumber(float value)
        {
            if (value < 0)
                return "Initialising...";
            else
                return value.ToString(CultureInfo.InvariantCulture);
        }

        [NotNull] private string FormatPercentage(float value)
        {
            if (value < 0)
                return "Initialising...";
            else
                return (value * 100).ToString("0.0", CultureInfo.InvariantCulture) + "%";
        }

        [NotNull] public override string Name
        {
            get { return "Dissonance Echo Cancellation"; }
        }

        [NotNull] public override string Description
        {
            get { return "Captures audio for Dissonance Acoustic Echo Cancellation"; }
        }

        [NotNull] public override string Vendor
        {
            get { return "Placeholder Software"; }
        }
    }
}
