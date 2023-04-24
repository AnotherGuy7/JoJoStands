using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class GreaterPlatinumAmuletOfChange : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 4));
            // DisplayName.SetDefault("Greater Amulet of Change");
            // Tooltip.SetDefault("10% increased Stand crit chance");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standCritChangeBoosts += 10f;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            bool alternateAmuletEquipped = false;
            for (int i = 0; i < player.armor.Length; i++)
            {
                Item Item = player.armor[i];
                if (Item.type == ModContent.ItemType<GreaterGoldAmuletOfChange>())
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
                .AddIngredient(ItemID.HallowedBar, 5)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 5)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .AddIngredient(ModContent.ItemType<PlatinumAmuletOfChange>())
                .Register();
        }
    }
}
