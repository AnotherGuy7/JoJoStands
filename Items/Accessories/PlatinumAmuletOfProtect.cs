using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items.Accessories
{
    public class PlatinumAmuletOfProtect : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(10, 4));
            DisplayName.SetDefault("Amulet of Protect");
            Tooltip.SetDefault("Increased defense while Stand is out");
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 0, 25, 0);
            item.rare = ItemRarityID.Blue;
            item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            if (player.GetModPlayer<MyPlayer>().StandOut)
            {
                player.statDefense += 6;
            }
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            bool alternateAmuletEquipped = false;
            for (int i = 0; i < player.armor.Length; i++)
            {
                Item item = player.armor[i];
                if (item.type == mod.ItemType("GoldAmuletOfProtect"))
                {
                    alternateAmuletEquipped = true;
                    break;
                }
            }
            return !alternateAmuletEquipped;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Chain, 1);
            recipe.AddIngredient(ItemID.PlatinumBar, 3);
            recipe.AddIngredient(mod.ItemType("WillToProtect"), 3);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}