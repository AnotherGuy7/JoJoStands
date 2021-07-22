using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class HermitPurpleT2 : StandItemClass
    {
        public override int standSpeed => 35;
        public override int standType => 1;
        public override int standTier => 2;

        public override string Texture
        {
            get { return mod.Name + "/Items/HermitPurpleT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hermit Purple (Tier 2)");
            Tooltip.SetDefault("Left-click to use Hermit Purple as a whip and right-click to grab enemies and slowly crush them!\nPassive: Enemies are hurt when they hurt you and get inflicted Sunburn!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 42;
            item.width = 32;
            item.height = 32;
            item.maxStack = 1;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standType = 1;
            mPlayer.hermitPurpleTier = standTier;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("HermitPurpleT1"));
            recipe.AddIngredient(ItemID.HellstoneBar, 7);
            recipe.AddIngredient(ItemID.Amethyst, 3);
            recipe.AddIngredient(mod.ItemType("WillToProtect"), 2);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 2);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
