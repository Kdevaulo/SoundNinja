using System.Collections.Generic;

using Kdevaulo.SoundNinja.SoundMenuBehaviour.SoundProcessingSystem;

using UnityEngine;

namespace Kdevaulo.SoundNinja
{
    public interface ISoundDataContainer
    {
        AudioClip GetAudioClip();
        List<SpectrumData> GetSpectrumDataCollection();
    }
}