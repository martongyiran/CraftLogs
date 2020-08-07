/*
Copyright 2019 Gyirán Márton Áron

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. 
*/

using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.Values;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {

        private Settings _settings;
        private string _housePrefix;
        private string _castPrefix;
        private string _selectedImage = null;

        private string _name;
        private string _house;
        private string _img1;
        private string _img2;
        private string _img3;
        private CharacterClassEnum _cast;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string House
        {
            get => _house;
            set
            {
                SetProperty(ref _house, value);
                SetHousePrefix();
                SetImages();
            }
        }

        public CharacterClassEnum Cast
        {
            get => _cast;
            set
            {
                SetProperty(ref _cast, value);
                SetCastPrefix();
                SetImages();
            }
        }

        public string Img1
        {
            get => _img1;
            set => SetProperty(ref _img1, value);
        }

        public string Img2
        {
            get => _img2;
            set => SetProperty(ref _img2, value);
        }

        public string Img3
        {
            get => _img3;
            set => SetProperty(ref _img3, value);
        }

        public List<string> Houses { get; set; } = new List<string> { Texts.House1, Texts.House2, Texts.House3, Texts.House4, Texts.House5, Texts.House6 };

        public List<CharacterClassEnum> Classes { get; set; } = new List<CharacterClassEnum> { CharacterClassEnum.Mage, CharacterClassEnum.Rogue, CharacterClassEnum.Warrior };

        public string SelectedImage
        {
            get => _selectedImage;
            set => SetProperty(ref _selectedImage, value);
        }

        public DelayCommand SaveCommand => new DelayCommand(async () => await ExecuteSaveCommandAsync());
        public DelayCommand CancelCommand => new DelayCommand(async () => await ExecuteCancelCommandAsync());

        public RegisterPageViewModel(
            INavigationService navigationService,
            ILocalDataRepository dataRepository,
            IPageDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.Register_Title;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _settings = DataRepository.GetSettings();
            House = Houses[0];
        }

        private async Task ExecuteSaveCommandAsync()
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrEmpty(_selectedImage))
            {
                bool sure = await DialogService.DisplayAlertAsync(Texts.Save, Texts.Register_SaveDialog, Texts.Save, Texts.Cancel);
                if (sure)
                {
                    _settings.AppMode = AppModeEnum.Team;
                    DataRepository.SaveToFile(_settings);
                    DataRepository.CreateTeamProfile(Name, NameToEnum(House), Cast, _selectedImage);
                    DataRepository.CreateLogs();

                    await NavigateToWithoutHistory(NavigationLinks.ProfilePage);
                }
            }
            else
            {
                await DialogService.DisplayAlertAsync(Texts.Error, Texts.Register_MissingError, Texts.Ok);
            }
        }

        private async Task ExecuteCancelCommandAsync()
        {
            var settings = DataRepository.GetSettings();
            settings.AppMode = AppModeEnum.None;
            DataRepository.SaveToFile(settings);

            await NavigateToWithoutHistory(NavigationLinks.SelectModePage);
        }

        private void SetHousePrefix()
        {
            switch (NameToEnum(House))
            {
                case HouseEnum.House1:
                    _housePrefix = "h1_";
                    break;
                case HouseEnum.House2:
                    _housePrefix = "h2_";
                    break;
                case HouseEnum.House3:
                    _housePrefix = "h3_";
                    break;
                case HouseEnum.House4:
                    _housePrefix = "h4_";
                    break;
                case HouseEnum.House5:
                    _housePrefix = "h5_";
                    break;
                case HouseEnum.House6:
                    _housePrefix = "h6_";
                    break;
            }
        }

        private HouseEnum NameToEnum(string name)
        {
            if (name.Equals(Texts.House1))
                return HouseEnum.House1;
            else if(name.Equals(Texts.House2))
                return HouseEnum.House2;
            else if (name.Equals(Texts.House3))
                return HouseEnum.House3;
            else if (name.Equals(Texts.House4))
                return HouseEnum.House4;
            else if (name.Equals(Texts.House5))
                return HouseEnum.House5;
            else if (name.Equals(Texts.House6))
                return HouseEnum.House6;
            else
                return HouseEnum.House1;
        }

        private void SetCastPrefix()
        {
            switch (Cast)
            {
                case CharacterClassEnum.Mage:
                    _castPrefix = "mage";
                    break;
                case CharacterClassEnum.Warrior:
                    _castPrefix = "warrior";
                    break;
                case CharacterClassEnum.Rogue:
                    _castPrefix = "rogue";
                    break;
            }
        }

        private string GetImgUrl(string num)
        {
            return "@drawable/" + _housePrefix + _castPrefix + num + ".png";
        }

        private void SetImages()
        {
            Img1 = GetImgUrl("1");
            Img2 = GetImgUrl("2");
            Img3 = GetImgUrl("3");
        }
    }
}
