using System;

using Kdevaulo.SoundNinja.Models;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kdevaulo.SoundNinja.SoundMenuBehaviour
{
    public sealed class SceneSwitcher : IDisposable
    {
        private readonly SceneManagerService _sceneManagerService;
        private readonly SoundContainerEventsModel _soundContainerEventsModel;

        public SceneSwitcher(SceneManagerService sceneManagerService,
            SoundContainerEventsModel soundContainerEventsModel)
        {
            _sceneManagerService = sceneManagerService;
            _soundContainerEventsModel = soundContainerEventsModel;

            soundContainerEventsModel.DataSaved += SwitchScene;
        }

        void IDisposable.Dispose()
        {
            _soundContainerEventsModel.DataSaved -= SwitchScene;
        }

        private void SwitchScene()
        {
            Debug.Log("Test switch");
            //_sceneManagerService.SwitchScene("", LoadSceneMode.Additive);
        }
    }
}