using System;

using UnityEngine;

namespace Kdevaulo.SoundNinja
{
    [AddComponentMenu(nameof(SoundNinja) + "/" + nameof(DisposableService))]
    public sealed class DisposableService : MonoBehaviour
    {
        private IDisposable[] _disposables;

        public void Initialize(params IDisposable[] disposables)
        {
            _disposables = disposables;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable?.Dispose();
            }
        }
    }
}