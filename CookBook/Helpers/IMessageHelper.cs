namespace CookBook.Helpers;

public interface IMessageHelper
{
    public bool ShowMessage(string message, string title);
    public void ShowError(string message, string title);

}