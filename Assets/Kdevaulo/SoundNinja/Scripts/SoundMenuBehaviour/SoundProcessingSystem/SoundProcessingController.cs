using System;
using System.Collections.Generic;
using System.Threading;

using Cysharp.Threading.Tasks;

using DSPLib;

using Kdevaulo.SoundNinja.Models;
using Kdevaulo.SoundNinja.Utils;

using UnityEngine;

namespace Kdevaulo.SoundNinja.SoundMenuBehaviour.SoundProcessingSystem
{
    public sealed class SoundProcessingController : IDisposable
    {
        private readonly SoundContainerEventsModel _soundContainerEventsModel;

        private readonly List<ISoundDataContainer> _dataContainersCollection = new List<ISoundDataContainer>();

        private const int SpectrumSampleSize = 1024;

        private float _highestAmplitude;

        private float[] _highestFrequencyValues = new float[AudioAnalyzerUtils.GroupsCount];

        public SoundProcessingController(SoundContainerEventsModel soundContainerEventsModel)
        {
            _soundContainerEventsModel = soundContainerEventsModel;

            soundContainerEventsModel.AudioClipsLoaded += HandleClipsLoaded;

            // note: needed to fix start groups impulse bug
            for (var i = 0; i < _highestFrequencyValues.Length; i++)
            {
                _highestFrequencyValues[i] = 1;
            }
        }

        void IDisposable.Dispose()
        {
            _soundContainerEventsModel.AudioClipsLoaded -= HandleClipsLoaded;
        }

        private void HandleClipsLoaded()
        {
            var audioClips = _soundContainerEventsModel.GetLoadedClips();
            var target = new List<AudioClip>(audioClips);
            AnalyzeSoundsAsync(target).Forget();
        }

        private async UniTask AnalyzeSoundsAsync(List<AudioClip> audioClips)
        {
            foreach (var clip in audioClips)
            {
                var numChannels = clip.channels;
                var numTotalSamples = clip.samples;
                var frequency = clip.frequency;

                var samples = new float[numTotalSamples * numChannels];
                clip.GetData(samples, 0);

                var thread = new Thread(() => AddSpectrumData(samples, numChannels, numTotalSamples, frequency, clip));

                thread.Start();

                await UniTask.WaitUntil(() => thread.ThreadState == ThreadState.Stopped);
            }

            _soundContainerEventsModel.HandleAnalysisFinished(_dataContainersCollection);
        }

        private void AddSpectrumData(in float[] samples, int numChannels, int numTotalSamples, int frequency,
            AudioClip clip)
        {
            _dataContainersCollection.Add(new SoundSpectrumData(clip,
                GetSpectrumDataCollection(samples, numChannels, numTotalSamples, frequency)));
        }

        private List<SpectrumData> GetSpectrumDataCollection(in float[] samples, int numChannels, int numTotalSamples,
            int sampleRate)
        {
            try
            {
                float[] preProcessedSamples = new float[numTotalSamples];

                FillSamplesWithAverage(samples, numChannels, preProcessedSamples);

                return CalculateSpectrumData(sampleRate, preProcessedSamples);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return null;
            }
        }

        private void FillSamplesWithAverage(float[] samples, int numChannels, float[] preProcessedSamples)
        {
            int numProcessed = 0;
            float combinedChannelAverage = 0f;
            for (int i = 0; i < samples.Length; i++)
            {
                combinedChannelAverage += samples[i];

                if ((i + 1) % numChannels == 0)
                {
                    preProcessedSamples[numProcessed] = combinedChannelAverage / numChannels;
                    numProcessed++;
                    combinedChannelAverage = 0f;
                }
            }
        }

        private List<SpectrumData> CalculateSpectrumData(int sampleRate, float[] preProcessedSamples)
        {
            int iterations = preProcessedSamples.Length / SpectrumSampleSize;

            List<SpectrumData> data = new List<SpectrumData>(iterations);

            FFT fft = new FFT();
            fft.Initialize(SpectrumSampleSize);

            double[] sampleChunk = new double[SpectrumSampleSize];

            for (int i = 0; i < iterations; i++)
            {
                Array.Copy(preProcessedSamples, i * SpectrumSampleSize, sampleChunk, 0, SpectrumSampleSize);

                var scaledFFTSpectrumData = FFTUtils.GetScaledFFTSpectrumData(sampleChunk, fft, SpectrumSampleSize);

                var rawSpectrumValues = Array.ConvertAll(scaledFFTSpectrumData, x => (float) x);

                float currentSongTime = GetTimeFromIndex(i, sampleRate) * SpectrumSampleSize;

                data.Add(CreateSpectrumData(rawSpectrumValues, currentSongTime));
            }

            return data;
        }

        private float GetTimeFromIndex(int index, int sampleRate)
        {
            return 1f / sampleRate * index;
        }

        private SpectrumData CreateSpectrumData(float[] rawSpectrumValues, float currentSongTime)
        {
            var frequencyGroups = AudioAnalyzerUtils.CreateFrequencyForGroups(rawSpectrumValues);

            var normalizedFrequencyGroups =
                AudioAnalyzerUtils.CreateNormalizedValuesGroups(frequencyGroups, ref _highestFrequencyValues);

            return new SpectrumData()
            {
                FrequencyData = frequencyGroups,
                NormalizedFrequencyData = normalizedFrequencyGroups,
                CurrentSoundTime = currentSongTime
            };
        }
    }
}