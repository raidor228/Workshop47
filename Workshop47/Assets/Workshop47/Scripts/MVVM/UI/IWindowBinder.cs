namespace Workshop47.Scripts.MVVM.UI
{
    public interface IWindowBinder
    {
        void Bind(WindowViewModel viewModel);
        void Close();
    }
}