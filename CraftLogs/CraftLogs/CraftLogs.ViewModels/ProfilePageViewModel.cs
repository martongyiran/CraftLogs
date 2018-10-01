using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation;

namespace CraftLogs.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {
        public ProfilePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Profile Page";
        }
    }
}
