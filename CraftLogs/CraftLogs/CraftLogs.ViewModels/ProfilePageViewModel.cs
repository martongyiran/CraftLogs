using System;
using System.Collections.Generic;
using System.Text;
using CraftLogs.Repositories.Local;
using Prism.Navigation;

namespace CraftLogs.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {
        public ProfilePageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository) : base(navigationService, dataRepository)
        {
            Title = "Profile Page";
        }
    }
}
