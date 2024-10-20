using Kdevaulo.SoundNinja.Models;
using Kdevaulo.SoundNinja.SoundMenuBehaviour;
using Kdevaulo.SoundNinja.SoundMenuBehaviour.AudioPlayerSystem;
using Kdevaulo.SoundNinja.SoundMenuBehaviour.FileLoadSystem;
using Kdevaulo.SoundNinja.SoundMenuBehaviour.SoundProcessingSystem;
using Kdevaulo.SoundNinja.SoundMenuBehaviour.SoundsContainerSystem;

using UnityEngine;

namespace Kdevaulo.SoundNinja
{
    [AddComponentMenu(nameof(SoundNinja) + "/" + nameof(SoundMenuStartup))]
    public sealed class SoundMenuStartup : MonoBehaviour
    {
        [SerializeField] private DisposableService _disposableService;

        [SerializeField] private FileLoadView _fileLoadView;
        [SerializeField] private SoundContainerView _soundContainerView;

        [SerializeField] private AudioSource _audioSource;

        private FileLoadController _fileLoadController;

        private void Awake()
        {
            var sceneManagerService = FindObjectOfType<SceneManagerService>();

            var soundContainerEventsModel = new SoundContainerEventsModel();

            var menuAudioPlayer = new AudioPlayer(_audioSource);

            var sceneSwitcher = new SceneSwitcher(sceneManagerService, soundContainerEventsModel);

            _fileLoadController = new FileLoadController(_fileLoadView, soundContainerEventsModel);

            var soundProcessingController = new SoundProcessingController(soundContainerEventsModel);
            var soundsContainerController =
                new SoundsContainerController(_soundContainerView, soundContainerEventsModel, menuAudioPlayer);

            _disposableService.Initialize(soundsContainerController, soundProcessingController, _fileLoadController,
                _fileLoadView, sceneSwitcher);
        }

        private void Start()
        {
            _fileLoadController.Initialize();
        }

        private void OnDestroy()
        {
            _disposableService.Dispose();
        }
    }
}