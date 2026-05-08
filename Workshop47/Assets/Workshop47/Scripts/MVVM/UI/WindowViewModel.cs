using System;
using R3;
using Workshop47.Scripts.Game.Gameplay.Input;

namespace Workshop47.Scripts.MVVM.UI
{
    public abstract class WindowViewModel : IDisposable
    {
        public abstract string Id { get; }
        public Observable<WindowViewModel> CloseRequested => _closeRequested;

        private readonly Subject<WindowViewModel> _closeRequested = new();

        public void RequestClose()
        {
            _closeRequested.OnNext(this);
        }

        public virtual void Dispose() { }
    }
}