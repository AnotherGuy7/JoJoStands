using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class PlatinumAmuletOfProtect : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 4));
            // DisplayName.SetDefault("Amulet of Protect");
            // Tooltip.SetDefault("Increased defense while Stand is out\nOnly boosts defense while in Manual Mode.");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(silver: 25);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standAccessoryDefense += 6;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            bool alternateAmuletEquipped = false;
            for (int i = 0; i < player.armor.Length; i++)
            {
                Item Item = player.armor[i];
                if (Item.type == ModContent.ItemType<GoldAmuletOfProtect>())
                {
                    alternateAmuletEquipped = true;
                    break;
                }
            }
            return !alternateAmuletEquipped;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Chain, 1)
                .AddIngredient(ItemID.PlatinumBar, 3)
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 3)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}