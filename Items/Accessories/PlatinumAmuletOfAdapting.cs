using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class PlatinumAmuletOfAdapting : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 4));
            // DisplayName.SetDefault("Amulet of Adapting");
            // Tooltip.SetDefault("10% increased Stand dodge chance\n10% increased Stand crit chance");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standDodgeChance += 10f;
            player.GetModPlayer<MyPlayer>().standCritChangeBoosts += 10f;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            bool alternateAmuletEquipped = false;
            for (int i = 0; i < player.armor.Length; i++)
            {
                Item Item = player.armor[i];
                if (Item.type == ModContent.ItemType<GoldAmuletOfAdapting>())
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
                .AddIngredient(ModContent.ItemType<GreaterPlatinumAmuletOfEscape>())
                .AddIngredient(ModContent.ItemType<GreaterPlatinumAmuletOfChange>())
                .AddIngredient(ModContent.ItemType<WillToChange>(), 2)
                .AddIngredient(ModContent.ItemType<WillToEscape>(), 2)
                .AddIngredient(ItemID.Ectoplasm, 5)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
