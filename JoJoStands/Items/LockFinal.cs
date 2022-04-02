using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class LockFinal : StandItemClass
    {
        public override string Texture
        {
            get { return mod.Name + "/Items/LockT1"; }
        }

        public override int standTier => 4;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Lock (Final Tier)");
            Tooltip.SetDefault("Make people that harm you overwhelmed with Guilt! \nSpecial: Damage yourself and make everyone in a 40 tile radius guilty about it.");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standAccessory = true;
            mPlayer.standType = 1;
            mPlayer.poseSoundName = "TheGuiltierYouFeel";
            player.AddBuff(mod.BuffType("LockActiveBuff"), 10);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("LockT3"));
            recipe.AddIngredient(ItemID.ShroomiteBar, 6);
            recipe.AddIngredient(mod.ItemType("WillToEscape"), 2);
			recipe.AddIngredient(mod.ItemType("WillToControl"), 2);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
