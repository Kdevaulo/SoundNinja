using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using Kdevaulo.SoundNinja.Models;
using Kdevaulo.SoundNinja.Utils;

using SimpleFileBrowser;

using UnityEngine;
using UnityEngine.Networking;

namespace Kdevaulo.SoundNinja.SoundMenuBehaviour.FileLoadSystem
{
    public sealed class FileLoadController : IDisposable
    {
        private readonly FileLoadView _fileLoadView;

        private readonly SoundContainerEventsModel _soundContainerEventsModel;

        private List<AudioClip> _loadedSounds = new List<AudioClip>();

        public FileLoadController(FileLoadView fileLoadView, SoundContainerEventsModel soundContainerEventsModel)
        {
            _fileLoadView = fileLoadView;
            _soundContainerEventsModel = soundContainerEventsModel;

            fileLoadView.LoadButtonClicked += HandleLoadButtonClick;
            soundContainerEventsModel.SoundsListUpdated += HandleSoundsListUpdated;
        }

        void IDisposable.Dispose()
        {
            _fileLoadView.LoadButtonClicked -= HandleLoadButtonClick;
            _soundContainerEventsModel.SoundsListUpdated -= HandleSoundsListUpdated;
        }

        public void Initialize()
        {
            FileBrowser.SetFilters(true,
                new FileBrowser.Filter(FileLoadConstants.SoundsFilterName, FileLoadConstants.SoundsFilterExtensions));

            FileBrowser.SetDefaultFilter(FileLoadConstants.DefaultFilter);
            FileBrowser.SetExcludedExtensions(FileLoadConstants.ExcludedExtensions);
            FileBrowser.AddQuickLink(FileLoadConstants.ShortcutName, FileLoadConstants.ShortcutPath);
        }

        private void HandleLoadButtonClick()
        {
            FileBrowser.ShowLoadDialog(HandlePathsSelected, HandleDialogCancelled, FileBrowser.PickMode.Files, true);
        }

        private void HandlePathsSelected(string[] paths)
        {
            LoadFilesAsync(paths).Forget();
            _fileLoadView.SetButtonState(false);
        }

        private void HandleDialogCancelled()
        {
            _fileLoadView.VisualizeLoadFinished();
        }

        private void HandleSoundsListUpdated()
        {
            _fileLoadView.VisualizeLoadFinished();
            _fileLoadView.SetButtonState(true);
        }

        private async UniTask LoadFilesAsync(string[] paths)
        {
            _fileLoadView.VisualizeLoadStarted();

            _loadedSounds.Clear();

            foreach (var path in paths)
            {
                await TryLoadFileAsync(path);
            }

            _soundContainerEventsModel.HandleAudioClipsLoaded(_loadedSounds);
        }

        private async UniTask TryLoadFileAsync(string path)
        {
            var urlPath = UnityWebRequest.EscapeURL(path);

            AudioClip audioClip;

            using (UnityWebRequest unityWebRequest =
                   UnityWebRequestMultimedia.GetAudioClip("file:///" + urlPath, AudioType.UNKNOWN))
            {
                var request = unityWebRequest.SendWebRequest();

                await request;

                audioClip = DownloadHandlerAudioClip.GetContent(unityWebRequest);
                audioClip.name = RegexUtils.GetSoundName(path);
            }

            _loadedSounds.Add(audioClip);

            // todo: needed to load audio data async, 400ms is a large freeze maybe AudioClip.loadInBackground will help
            audioClip.LoadAudioData();

            await UniTask.WaitUntil(() => audioClip.loadState == AudioDataLoadState.Loaded);
        }
    }
}