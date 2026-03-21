using Workshop47.Scripts.DI;

namespace Workshop47.Scripts.MVVM.UI
{
    public abstract class UIManager
    {
        protected readonly DIContainer Container;

        protected UIManager(DIContainer container)
        {
            Container = container;
        }
    }
}