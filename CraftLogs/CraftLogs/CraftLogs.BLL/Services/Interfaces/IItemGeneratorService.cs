using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;

namespace CraftLogs.BLL.Services.Interfaces
{
    public interface IItemGeneratorService
    {
        Item GenerateHead(int tier, ItemRarityEnum rarity);
        Item GenerateChest(int tier, ItemRarityEnum rarity);
        Item GenerateBoots(int tier, ItemRarityEnum rarity);
        Item GenerateHand(int tier, ItemRarityEnum rarity);
        Item GenerateTrinket(int tier, ItemRarityEnum rarity);
        Item GenerateConsumable(int tier, ItemRarityEnum rarity);
        Item GenerateRandom();
        Item GenerateWeapon(ItemSubTypeEnum itemSubType, int tier, ItemRarityEnum rarity);
    }
}
