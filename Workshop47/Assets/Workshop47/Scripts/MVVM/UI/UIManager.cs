using System;
using Workshop47.Scripts.DI;

namespace Workshop47.Scripts.MVVM.UI
{
    public abstract class UIManager : IDisposable
    {
        protected readonly DIContainer Container;

        protected UIManager(DIContainer container)
        {
            Container = container;
        }

        public virtual void Dispose() { }
    }
}