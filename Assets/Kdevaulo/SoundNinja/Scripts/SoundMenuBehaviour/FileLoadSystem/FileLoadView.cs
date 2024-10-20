using System;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

namespace Kdevaulo.SoundNinja.SoundMenuBehaviour.FileLoadSystem
{
    [AddComponentMenu(nameof(FileLoadSystem) + "/" + nameof(FileLoadView))]
    public sealed class FileLoadView : MonoBehaviour, IDisposable
    {
        public event Action LoadButtonClicked = delegate { };

        [SerializeField] private Button _loadFilesButton;

        [SerializeField] private Transform _loadingInProgressIcon;

        [SerializeField] private GameObject _loadedIcon;

        private readonly Vector3 _loadingRotation = new Vector3(0f, 0f, -360f);

        private Tween _currentTween;

        private void Awake()
        {
            _loadFilesButton.onClick.AddListener(HandleButtonClick);
        }

        void IDisposable.Dispose()
        {
            _loadFilesButton.onClick.RemoveListener(HandleButtonClick);
        }

        public void SetButtonState(bool state)
        {
            _loadFilesButton.enabled = state;
        }

        public void VisualizeLoadFinished()
        {
            TryKillTween();

            SwitchIcon();
        }

        public void VisualizeLoadStarted()
        {
            SwitchIcon(false);

            VisualizeLoadingAsync().Forget();
        }

        private async UniTask VisualizeLoadingAsync()
        {
            TryKillTween();
        
            _currentTween = _loadingInProgressIcon.DORotate(_loadingRotation, 2, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear).SetLoops(-1);
        
            await _currentTween;
        }

        private void HandleButtonClick()
        {
            LoadButtonClicked.Invoke();
        }

        private void SwitchIcon(bool loadFinished = true)
        {
            _loadingInProgressIcon.gameObject.SetActive(!loadFinished);
            _loadedIcon.SetActive(loadFinished);
        }

        private void TryKillTween()
        {
            _currentTween?.Kill();
        }
    }
}