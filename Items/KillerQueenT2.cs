using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class KillerQueenT2 : StandItemClass
    {
        public override int standSpeed => 11;
        public override int standType => 1;

        public override string Texture
        {
            get { return mod.Name + "/Items/KillerQueenT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Killer Queen (1st Bomb Tier 2)");
            Tooltip.SetDefault("Left-click to punch and right-click to trigger any block! \nRange: 12 blocks\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 35;
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
            recipe.AddIngredient(mod.ItemType("KillerQueenT1"));
            recipe.AddIngredient(ItemID.Dynamite, 12);
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
            recipe.AddIngredient(mod.ItemType("WillToDestroy"));
            recipe.AddIngredient(mod.ItemType("WillToEscape"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}