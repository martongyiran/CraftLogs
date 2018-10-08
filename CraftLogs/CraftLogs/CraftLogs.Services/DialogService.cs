using System.Threading.Tasks;
using Xamarin.Forms;

namespace CraftLogs.Services
{
    public class DialogService : IDialogService
    {
        public Task DisplayAlert(string title, string message, string cancel)
        {
            return Application.Current.MainPage?.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await Application.Current.MainPage?.DisplayAlert(title, message, accept, cancel);
        }
    }
}
