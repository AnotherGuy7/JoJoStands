using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.Alloys
{
    [AutoloadEquip(EquipType.Head)]
    public class TitaniumAlloyMaskNeutral : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Titanium Alloy Mask (Neutral)");
            Tooltip.SetDefault("A mask fused with Viral Meteorite to empower the user.\nStand stat buffs change depending on stand type.");
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
                item.type = mod.ItemType("TitaniumAlloyMaskShort");
                item.SetDefaults(mod.ItemType("TitaniumAlloyMaskShort"));
            }
            if (mPlayer.standType == 2)
            {
                item.type = mod.ItemType("TitaniumAlloyMaskLong");
                item.SetDefaults(mod.ItemType("TitaniumAlloyMaskLong"));
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 11);
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 18);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}