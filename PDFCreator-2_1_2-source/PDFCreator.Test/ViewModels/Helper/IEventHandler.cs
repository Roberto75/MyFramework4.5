namespace PDFCreator.Test.ViewModels.Helper
{
    public interface IEventHandler<T>
    {
        void OnEventRaised(object sender, T eventargs);
    }
}