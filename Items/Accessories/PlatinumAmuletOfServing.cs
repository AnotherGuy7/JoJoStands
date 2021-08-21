using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items.Accessories
{
    public class PlatinumAmuletOfServing : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(10, 4));
            DisplayName.SetDefault("Amulet of Serving");
            Tooltip.SetDefault("30% increased Stand attack damage\nIncreased defense while the Stand is out");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 50, 0, 0);
            item.rare = ItemRarityID.Lime;
            item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.3f;
            if (player.GetModPlayer<MyPlayer>().standOut)
            {
                player.statDefense += 12;
            }
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            bool alternateAmuletEquipped = false;
            for (int i = 0; i < player.armor.Length; i++)
            {
                Item item = player.armor[i];
                if (item.type == mod.ItemType("GoldAmuletOfServing"))
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
            recipe.AddIngredient(mod.ItemType("GreaterPlatinumAmuletOfFight"));
            recipe.AddIngredient(mod.ItemType("GreaterPlatinumAmuletOfProtect"));
            recipe.AddIngredient(mod.ItemType("WillToFight"), 2);
            recipe.AddIngredient(mod.ItemType("WillToProtect"), 2);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
