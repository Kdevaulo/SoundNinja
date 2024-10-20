using System;
using System.Collections.Generic;
using System.Linq;

using Kdevaulo.SoundNinja.Utils;

using UnityEngine;

namespace Kdevaulo.SoundNinja.Models
{
    public sealed class SoundContainerEventsModel
    {
        public event Action SoundsListUpdated = delegate { };
        public event Action AnalysisFinished = delegate { };
        public event Action AudioClipsLoaded = delegate { };
        public event Action DataSaved = delegate { };

        private List<AudioClip> _loadedClips;

        private List<ISoundDataContainer> _dataContainersCollection;

        public void HandleAnalysisFinished(List<ISoundDataContainer> dataContainersCollection)
        {
            _dataContainersCollection = dataContainersCollection;

            AnalysisFinished.Invoke();
        }

        public void HandleAudioClipsLoaded(List<AudioClip> audioClips)
        {
            _loadedClips = audioClips;

            AudioClipsLoaded.Invoke();
        }

        public void HandleSoundsListUpdated()
        {
            SoundsListUpdated.Invoke();
        }

        public void CacheAudioData(AudioClip audioClip)
        {
            var chosenData = _dataContainersCollection.First(x => x.GetAudioClip() == audioClip);

            SpectrumDataHolder.CacheData(chosenData);

            DataSaved.Invoke();
        }

        public List<AudioClip> GetLoadedClips()
        {
            return _loadedClips;
        }
    }
}