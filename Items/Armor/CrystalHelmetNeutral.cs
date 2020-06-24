using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class CrystalHelmetNeutral : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Helmet (Neutral)");
            Tooltip.SetDefault("A helmet made to empower the force of the wills\nStand stat buffs change depending on stand type.");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.rare = ItemRarityID.LightPurple;
            item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standType == 1)
            {
                item.type = mod.ItemType("CrystalHelmetShort");
                item.SetDefaults(mod.ItemType("CrystalHelmetShort"));
            }
            if (mPlayer.standType == 2)
            {
                item.type = mod.ItemType("CrystalHelmetLong");
                item.SetDefaults(mod.ItemType("CrystalHelmetLong"));
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrystalShard, 10);
            recipe.AddIngredient(ItemID.Silk, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}