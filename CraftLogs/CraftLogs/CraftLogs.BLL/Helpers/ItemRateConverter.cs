using CraftLogs.BLL.Enums;

namespace CraftLogs.BLL.Helpers
{
    public static class ItemRateConverter
    {
        public static int TierToRate(int tier)
        {
            switch (tier)
            {
                case 1:
                    return 1;
                case 2:
                    return 4;
                case 3:
                    return 7;
                default:
                    return 0;
            }
        }

        public static int RarityToRate(ItemRarityEnum itemRarity)
        {
            switch (itemRarity)
            {
                case ItemRarityEnum.Common:
                    return 0;
                case ItemRarityEnum.Uncommon:
                    return 1;
                case ItemRarityEnum.Rare:
                    return 2;
                case ItemRarityEnum.Epic:
                    return 3;
                default:
                    return 0;
            }
        }
    }
}
