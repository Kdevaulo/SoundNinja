using System.Collections.Generic;

using UnityEngine;

namespace Kdevaulo.SoundNinja.SoundMenuBehaviour.SoundProcessingSystem
{
    public sealed class SoundSpectrumData : ISoundDataContainer
    {
        private readonly AudioClip _audioClip;

        private readonly List<SpectrumData> _spectrumDataCollection;

        public SoundSpectrumData(AudioClip audioClip, List<SpectrumData> spectrumDataCollection)
        {
            _audioClip = audioClip;
            _spectrumDataCollection = spectrumDataCollection;
        }

        AudioClip ISoundDataContainer.GetAudioClip()
        {
            return _audioClip;
        }

        List<SpectrumData> ISoundDataContainer.GetSpectrumDataCollection()
        {
            return _spectrumDataCollection;
        }
    }
}