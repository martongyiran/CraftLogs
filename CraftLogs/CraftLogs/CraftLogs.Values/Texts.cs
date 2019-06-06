/*
Copyright 2018 Gyirán Márton Áron

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

namespace CraftLogs.Values
{
    public static class Texts
    {
        #region Menu texts

        public const string MainPage = "CraftLogs";
        public const string ProfilePage = "Profil";
        public const string InventoryPage = "Hátizsák";
        public const string TradePage = "Csere";
        public const string SettingsPage = "Beállítások";
        public const string QuestPage = "Pontozás";
        public const string LogsPage = "Napló";

        /// <summary>
        /// Version number on MainPage with {0} args.
        /// </summary>
        public const string Version = "Version: {0}";

        #endregion

        #region General

        public const string Ok = "OK";
        public const string Yes = "Igen";
        public const string No = "Nem";
        public const string Save = "Mentés";
        public const string Cancel = "Mégse";
        public const string Error = "Hiba";

        #endregion

        #region SelectModePage texts

        /// <summary>
        /// SelectModePage button text.
        /// </summary>
        public const string TeamMode = "Csapat";

        /// <summary>
        /// SelectModePage button text.
        /// </summary>
        public const string QuestMode = "Állomás";

        /// <summary>
        /// SelectModePage button text.
        /// </summary>
        public const string ShopMode = "Bolt";

        /// <summary>
        /// SelectModePage button text.
        /// </summary>
        public const string ArenaMode = "Aréna";

        /// <summary>
        /// SelectModePage button text.
        /// </summary>
        public const string HqMode = "HQ";

        /// <summary>
        /// SelectModePage dialog text.
        /// </summary>
        public const string ChooseDialog = "Kérlek válaszd ki, milyen módban indítod az alkalmazást!";

        #endregion

        #region SettingsPage texts 

        /// <summary>
        /// SettingsPage toolbar text.
        /// </summary>
        public const string ToDefault = "Alaphelyzet";

        /// <summary>
        /// SettingsPage list text.
        /// </summary>
        public const string CraftDay = "Craft nap: ";

        /// <summary>
        /// SettingsPage list text.
        /// </summary>
        public const string Craft1Start = "Craft 1 kezdete (óra 0-24) ";

        /// <summary>
        /// SettingsPage list text.
        /// </summary>
        public const string Craft2Start = "Craft 2 kezdete (óra 0-24) ";

        /// <summary>
        /// SettingsPage list text.
        /// </summary>
        public const string Craft1MinPont = "Craft 1 Minimum Pont ";

        /// <summary>
        /// SettingsPage list text.
        /// </summary>
        public const string Craft2MinPont = "Craft 2 Minimum Pont ";

        /// <summary>
        /// SettingsPage: save settings text.
        /// </summary>
        public const string SaveSettings = "Mentés";

        /// <summary>
        /// SettingsPage: sum text.
        /// </summary>
        public const string Sum = "Összegzés";

        /// <summary>
        /// SettingsPage: delete profile text.
        /// </summary>
        public const string DeleteProfile = "Profil törlése";

        public const string GetAvgText = "Állomás átlag";

        /// <summary>
        /// SettingsPage: GodMode text.
        /// </summary>
        public const string GodMode = "Isteni üzemmód";

        /// <summary>
        /// SettingsPage: Settings succesfully saved text.
        /// </summary>
        public const string SuccessfulSaving = "A beállítások sikeresen elmentve!";

        /// <summary>
        /// SettingsPage: Settings reset question text.
        /// </summary>
        public const string ResetData = "Biztos, hogy alaphelyzetbe állítod a beállításokat?";

        /// <summary>
        /// SettingsPage: Settings delete question text.
        /// </summary>
        public const string DeleteProfileQuestion = "Biztos, hogy törlöd a profilodat?";

        #endregion

        #region LogsPage texts

        /// <summary>
        /// LogsPage: LoadMore button text.
        /// </summary>
        public const string LoadMore = "Több betöltése";

        /// <summary>
        /// LogsPage: Page is empty text.
        /// </summary>
        public const string EmptyLogs = "Még nem találhatóak bejegyzések!";

        #endregion

        #region RegisterPage texts

        public const string RegisterName = "Állomás neve:";

        public const string RegisterTitle = "Profil létrehozása";

        public const string RegisterNameSave = "Biztos, hogy elmented ezt a nevet? A későbbiekben nem módosíthatod!";

        public const string RegisterMissingName = "Kérlek írj be valamilyen nevet!";

        public const string TeamRegisterName = "Csapat neve:";

        public const string TeamHouse = "Ház választása";

        public const string TeamClass = "Kaszt választása";

        #endregion

        #region Quest

        public const string Result = "Eredmény";

        public const string ResultDialog = "Biztos, hogy {0} pontot adsz a csapatnak?";

        public const string CantScore = "Most nem pontozhatsz!";

        #endregion

        #region Rating

        public const string RatingTitle = "Értékelés";

        #endregion
        //Ezeket törölni kell majd valszeg a jövőben

        #region Itemtypes

        public const string Head="sisakja";
        public const string Chest = "páncélja";
        public const string Boots = "csizmája";
        public const string Trinket = "kiegészítője"; //ide majd randomgenerátor

        #endregion

        #region Stats

        public const string Stamina = "állóképesség";
        public const string Strength = "erő";
        public const string Agility = "fürgeség";
        public const string Intellect = "intelligencia";

        #endregion

        #region ItemSubTypes

        public const string Dagger = "tőre";
        public const string Sword = "kardja";
        public const string Bow = "íja";
        public const string Wand = "pálcája";
        public const string Staff = "botja";
        public const string Hammer = "pörölye";

        #endregion

        #region SetNames

        public const string Set1 = "Aladár";
        public const string Set2 = "Béla";
        public const string Set3 = "Cecil";
        public const string Set4 = "Dominik";
        public const string Set5 = "Erik";

        #endregion
    }
}
