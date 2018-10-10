namespace CraftLogs.Values
{
    public static class Texts
    {
        public const string MainPage = "Főoldal";
        public const string ProfilePage = "Profil";
        public const string SettingsPage = "Beállítások";
        public const string QuestPage = "Állomás";
        public const string LogsPage = "Napló";

        /// <summary>
        /// Version number on MainPage with {0} args.
        /// </summary>
        public const string Version = "Version: {0}";

        #region General

        public const string Ok = "OK";
        public const string Yes = "Igen";
        public const string No = "Nem";

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
        /// SettingsPage list text.
        /// </summary>
        public const string Craft1QuestCount = "Craft 1 állomások száma ";
        /// <summary>
        /// SettingsPage list text.
        /// </summary>
        public const string Craft2QuestCount = "Craft 2 állomások száma ";
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

        #region Itemtypes
        public const string Head="sisakja";
        public const string Chest = "páncélja";
        public const string Boots = "csizmája";
        public const string Trinket = "kiegészítője"; //ide majd randomgenerátor
        #endregion

        #region ItemSubTypes
        public const string Dagger = "tőre";
        public const string Sword = "kardja";
        public const string Axe = "baltája";
        public const string Spear = "lándzsája";
        public const string Shield = "pajzsa";
        public const string Hammer = "pörölye";
        public const string Food = "étele"; //ide majd randomgenerátor
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
