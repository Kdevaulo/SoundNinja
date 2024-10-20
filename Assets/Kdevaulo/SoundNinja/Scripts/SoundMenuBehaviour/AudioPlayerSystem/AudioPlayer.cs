using UnityEngine;

namespace Kdevaulo.SoundNinja.SoundMenuBehaviour.AudioPlayerSystem
{
    public sealed class AudioPlayer
    {
        private readonly AudioSource _audioSource;

        public AudioPlayer(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public void PlayClip(AudioClip clip)
        {
            // todo: now clip plays ones, need to think about cycles or playing "standart" music after chosen finished
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }
}