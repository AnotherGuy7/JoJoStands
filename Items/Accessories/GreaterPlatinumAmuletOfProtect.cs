using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class GreaterPlatinumAmuletOfProtect : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 4));
            DisplayName.SetDefault("Greater Amulet of Protect");
            Tooltip.SetDefault("Increased defense while the Stand is out");
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
            if (player.GetModPlayer<MyPlayer>().standOut)
            {
                player.statDefense += 12;
            }
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            bool alternateAmuletEquipped = false;
            for (int i = 0; i < player.armor.Length; i++)
            {
                Item Item = player.armor[i];
                if (Item.type == ModContent.ItemType<GreaterGoldAmuletOfProtect>())
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
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 5)
                .AddIngredient(ModContent.ItemType<PlatinumAmuletOfProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
