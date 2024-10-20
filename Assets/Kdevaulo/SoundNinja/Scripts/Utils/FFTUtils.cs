using System.Numerics;

using DSPLib;

namespace Kdevaulo.SoundNinja.Utils
{
    public static class FFTUtils
    {
        public static double[] GetScaledFFTSpectrumData(double[] sampleChunk, FFT fft, uint spectrumSampleSize)
        {
            double[] windowCoefs = DSP.Window.Coefficients(DSP.Window.Type.Hanning, spectrumSampleSize);
            double[] scaledSpectrumChunk = DSP.Math.Multiply(sampleChunk, windowCoefs);
            double scaleFactor = DSP.Window.ScaleFactor.Signal(windowCoefs);

            Complex[] fftSpectrum = fft.Execute(scaledSpectrumChunk);
            double[] scaledFFTSpectrum = DSP.ConvertComplex.ToMagnitude(fftSpectrum);
            scaledFFTSpectrum = DSP.Math.Multiply(scaledFFTSpectrum, scaleFactor);
            return scaledFFTSpectrum;
        }
    }
}