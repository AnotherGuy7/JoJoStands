using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StoneFreeT2 : StandItemClass
    {
        public override int standSpeed => 10;
        public override int standType => 1;
        public override string standProjectileName => "StoneFree";
        public override int standTier => 2;

        public override string Texture
        {
            get { return mod.Name + "/Items/StoneFreeT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Free (Tier 2)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click two tiles to create a string trap!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 39;
            item.width = 50;
            item.height = 50;
            item.maxStack = 1;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StandArrow"));
            recipe.AddIngredient(ItemID.HellstoneBar, 16);
            recipe.AddIngredient(ItemID.Silk, 8);
            recipe.AddIngredient(ItemID.Spike, 16);
            recipe.AddIngredient(mod.ItemType("WillToChange"), 2);
            recipe.AddIngredient(mod.ItemType("WillToEscape"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
