using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    [AutoloadEquip(EquipType.Waist)]
    public class FamilyPhoto : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Family Photo");
            // Tooltip.SetDefault("Successful Stand Melee Attacks reduce damage taken.");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 5);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().familyPhotoEquipped = true;
        }
    }
}