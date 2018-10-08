using System.Threading.Tasks;

namespace CraftLogs.Services
{
    public interface IDialogService
    {
        /// <summary>
        /// Basic Xamarin.Forms DisplayAlert.
        /// </summary>
        /// <param name="title">Title text.</param>
        /// <param name="message">Message text.</param>
        /// <param name="cancel">Cancel text.</param>
        /// <returns></returns>
        Task DisplayAlert(string title, string message, string cancel);
        /// <summary>
        /// Basic Xamarin.Forms DisplayAlert.
        /// </summary>
        /// <param name="title">Title text.</param>
        /// <param name="message">Message text.</param>
        /// <param name="accept">Accept text.</param>
        /// <param name="cancel">Cancel text.</param>
        /// <returns></returns>
        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);
    }
}
