using CraftLogs.BLL.Enums;

namespace CraftLogs.BLL.Helpers
{
    public static class ItemSubTypeConverter
    {
        public static int ItemSubTypeToDps(ItemSubTypeEnum itemSubType)
        {
            switch (itemSubType)
            {
                case ItemSubTypeEnum.None:
                    return 0;
                case ItemSubTypeEnum.Dagger:
                    return 27;
                case ItemSubTypeEnum.Sword:
                    return 33;
                case ItemSubTypeEnum.Axe:
                    return 43;
                case ItemSubTypeEnum.Spear:
                    return 100;
                case ItemSubTypeEnum.Hammer:
                    return 150;
                case ItemSubTypeEnum.Shield:
                    return 0;
                default:
                    return 0;
            }
        }

        public static int ItemSubTypeToSpeed(ItemSubTypeEnum itemSubType)
        {
            switch (itemSubType)
            {
                case ItemSubTypeEnum.None:
                    return 0;
                case ItemSubTypeEnum.Dagger:
                    return 6;
                case ItemSubTypeEnum.Sword:
                    return 5;
                case ItemSubTypeEnum.Axe:
                    return 4;
                case ItemSubTypeEnum.Spear:
                    return 3;
                case ItemSubTypeEnum.Hammer:
                    return 2;
                case ItemSubTypeEnum.Shield:
                    return 0;
                default:
                    return 0;
            }
        }

    }
}
