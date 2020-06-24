using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class AdamantiteAlloyHelmetNeutral : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Adamantite Alloy Helmet (Neutral)");
            Tooltip.SetDefault("A helmet fused with Viral Meteorite to empower the user.\nStand stat buffs change depending on stand type.");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standType == 1)
            {
                item.type = mod.ItemType("AdamantiteAlloyHelmetShort");
                item.SetDefaults(mod.ItemType("AdamantiteAlloyHelmetShort"));
            }
            if (mPlayer.standType == 2)
            {
                item.type = mod.ItemType("AdamantiteAlloyHelmetLong");
                item.SetDefaults(mod.ItemType("AdamantiteAlloyHelmetLong"));
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AdamantiteBar, 10);
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}