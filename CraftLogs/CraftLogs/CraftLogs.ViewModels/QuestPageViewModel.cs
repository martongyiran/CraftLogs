﻿using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.Services;
using CraftLogs.Values;
using Prism.Navigation;

namespace CraftLogs.ViewModels
{
    public class QuestPageViewModel : ViewModelBase
    {
        public QuestPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IDialogService dialogService) : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.QuestPage;
        }
    }
}
