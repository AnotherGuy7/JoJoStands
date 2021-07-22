using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class GratefulDeadT3 : StandItemClass
    {
        public override int standSpeed => 11;
        public override int standType => 1;
        public override string standProjectileName => "GratefulDead";
        public override int standTier => 3;

        public override string Texture
        {
            get { return mod.Name + "/Items/GratefulDeadT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grateful Dead (Tier 3)");
            Tooltip.SetDefault("Punch enemies to make them age and right-click to grab them!\nSpecial: Spread Gas\nMore effective on hot biomes.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 67;
            item.width = 32;
            item.height = 32;
            item.maxStack = 1;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            player.GetModPlayer<MyPlayer>().gratefulDeadTier = standTier;
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("GratefulDeadT2"));
            recipe.AddIngredient(ItemID.Ichor, 10);
            recipe.AddIngredient(ItemID.Bone, 15);
            recipe.AddIngredient(mod.ItemType("WillToControl"), 2);
            recipe.AddIngredient(mod.ItemType("WillToChange"), 2);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("GratefulDeadT2"));
            recipe.AddIngredient(ItemID.CursedFlame, 10);
            recipe.AddIngredient(ItemID.Bone, 15);
            recipe.AddIngredient(mod.ItemType("WillToControl"), 2);
            recipe.AddIngredient(mod.ItemType("WillToChange"), 2);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
