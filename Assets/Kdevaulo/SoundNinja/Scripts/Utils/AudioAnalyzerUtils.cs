using UnityEngine;

namespace Kdevaulo.SoundNinja.Utils
{
    public static class AudioAnalyzerUtils
    {
        public const int GroupsCount = 8;

        private const float FrequencyScale = 10;
        private const float DecreaseValue = 0.005f;

        public static float[] CreateFrequencyForGroups(in float[] multiChannelSamples)
        {
            var frequencyValues = new float[GroupsCount];

            var counter = 0;

            for (int i = 0; i < GroupsCount; i++)
            {
                var samplesCount = (int) Mathf.Pow(2, i + 1);

                var average = 0f;

                if (i == GroupsCount - 1)
                {
                    samplesCount += 2;
                    // note: addition 2 bytes to last group is hack to cover 512 bytes at all instead of 510
                }

                for (int k = 0; k < samplesCount; k++)
                {
                    average += multiChannelSamples[counter] * (counter + 1);
                    counter++;
                }

                average /= counter;
                frequencyValues[i] = average * FrequencyScale;
            }

            return frequencyValues;
        }

        public static void CreateSmoothValuesGroups(in float[] frequencyValuesGroups, ref float[] smoothValues,
            ref float[] bufferGroupDecrease, float smoothnessCoefficient)
        {
            ArrayUtils.CheckingForEqualArraysLengths(frequencyValuesGroups.Length, smoothValues.Length,
                bufferGroupDecrease.Length,
                GroupsCount);

            for (int i = 0; i < GroupsCount; i++)
            {
                var currentFrequency = frequencyValuesGroups[i];
                var currentSmooth = smoothValues[i];

                if (currentFrequency > currentSmooth)
                {
                    smoothValues[i] = currentFrequency;
                    bufferGroupDecrease[i] = DecreaseValue;
                }
                else
                {
                    bufferGroupDecrease[i] = (bufferGroupDecrease[i] - currentFrequency) / GroupsCount;
                    smoothValues[i] = smoothnessCoefficient * currentSmooth +
                                      (1 - smoothnessCoefficient) * currentFrequency;
                }
            }
        }

        public static float[] CreateNormalizedValuesGroups(in float[] values, ref float[] highestValues)
        {
            ArrayUtils.CheckingForEqualArraysLengths(values.Length, highestValues.Length, GroupsCount);

            var results = new float[GroupsCount];

            for (int i = 0; i < GroupsCount; i++)
            {
                var currentValue = values[i];
                var currentMax = highestValues[i];

                if (currentValue > currentMax)
                {
                    highestValues[i] = currentMax = currentValue;
                }

                if (currentMax != 0)
                {
                    results[i] = currentValue / currentMax;
                }
            }

            return results;
        }

        public static float CreateAmplitude(in float[] values, ref float highestAmplitude)
        {
            ArrayUtils.CheckingForEqualArraysLengths(values.Length, GroupsCount);

            var currentAmplitude = 0f;

            for (int i = 0; i < GroupsCount; i++)
            {
                currentAmplitude += values[i];
            }

            if (highestAmplitude < currentAmplitude)
            {
                highestAmplitude = currentAmplitude;
            }

            return currentAmplitude / highestAmplitude;
        }
    }
}