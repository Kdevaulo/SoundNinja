namespace Kdevaulo.SoundNinja.SoundMenuBehaviour.SoundProcessingSystem
{
    public sealed class SpectrumData
    {
        public float[] FrequencyData;
        public float[] SmoothData;
        public float[] NormalizedFrequencyData;
        public float[] NormalizedSmoothData;

        public float Amplitude;
        public float SmoothAmplitude;
        public float CurrentSoundTime;
    }
}