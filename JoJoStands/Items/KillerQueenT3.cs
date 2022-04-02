using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class KillerQueenT3 : StandItemClass
    {
        public override int standSpeed => 10;
        public override int standType => 1;
        public override string standProjectileName => "KillerQueen";
        public override int standTier => 3;

        public override string Texture
        {
            get { return mod.Name + "/Items/KillerQueenT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Killer Queen (1st Bomb Tier 3)");
            Tooltip.SetDefault("Left-click to punch and right-click to trigger any block! \nRange: 14 blocks \nSpecial: Sheer Heart Attack!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 59;
            item.width = 32;
            item.height = 32;
            item.maxStack = 1;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("KillerQueenT2"));
            recipe.AddIngredient(ItemID.HallowedBar, 6);
            recipe.AddIngredient(mod.ItemType("Hand"), 1);
            recipe.AddIngredient(ItemID.SoulofMight, 8);
            recipe.AddIngredient(mod.ItemType("WillToDestroy"), 2);
            recipe.AddIngredient(mod.ItemType("WillToEscape"), 2);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}