using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Requiem
{
    [AutoloadEquip(EquipType.Head)]
    public class RequiemCrownNeutral : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Requiem Crown (Neutral)");
            Tooltip.SetDefault("A crown made from the finest materials space has offered so far.");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Red;
            item.defense = 10;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("RequiemChestplate") && legs.type == mod.ItemType("RequiemGreaves");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Viral Beetles float around you, defending you from anything that comes your way.";
            player.AddBuff(mod.BuffType("ViralBeetleBuff"), 2);
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standType == 2)
            {
                item.type = mod.ItemType("RequiemCrownLong");
                item.SetDefaults(mod.ItemType("RequiemCrownLong"));
            }
            if (mPlayer.standType == 1)
            {
                item.type = mod.ItemType("RequiemCrownShort");
                item.SetDefaults(mod.ItemType("RequiemCrownShort"));
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 17);
            recipe.AddIngredient(ItemID.FragmentStardust, 6);
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 6);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}