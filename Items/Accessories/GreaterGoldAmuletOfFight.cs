using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class GreaterGoldAmuletOfFight : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 4));
            DisplayName.SetDefault("Greater Amulet of Fight");
            Tooltip.SetDefault("20% increased Stand attack damage");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.2f;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            bool alternateAmuletEquipped = false;
            for (int i = 0; i < player.armor.Length; i++)
            {
                Item Item = player.armor[i];
                if (Item.type == ModContent.ItemType<GreaterPlatinumAmuletOfFight>())
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
                .AddIngredient(ModContent.ItemType<WillToFight>(), 5)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .AddIngredient(ModContent.ItemType<GoldAmuletOfFight>())
                .Register();
        }
    }
}
