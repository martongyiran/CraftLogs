using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;

namespace CraftLogs.ViewModels
{
    public class ItemTestPageViewModel : ViewModelBase
    {
        private readonly IItemGeneratorService ItemGeneratorService;

        private ObservableCollection<Item> itemsList;

        public ObservableCollection<Item> ItemsList
        {
            get { return itemsList; }
            set { SetProperty(ref itemsList, value); }
        }

        public ItemTestPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IItemGeneratorService itemGeneratorService) : base(navigationService, dataRepository, dialogService)
        {
            ItemGeneratorService = itemGeneratorService;
            Title = "Item Generator Test Page";
            ItemsList = new ObservableCollection<Item>();
            var json = JsonConvert.SerializeObject(ItemGeneratorService.GenerateRandom());
            var b = GetHash(json);
            var c = GetHashString(json);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            ItemsList = GenerateTestItems();

        }

        private ObservableCollection<Item> GenerateTestItems()
        {
            ObservableCollection<Item> res = new ObservableCollection<Item>();
            for (int i = 0; i < 20; i++)
            {
                res.Add(ItemGeneratorService.GenerateRandom());
            }

            return res;
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

    }
}