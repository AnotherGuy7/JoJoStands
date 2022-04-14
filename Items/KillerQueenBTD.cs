using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class KillerQueenBTD : StandItemClass
    {
        public static bool taggedAnything = false;

        public override int standSpeed => 30;
        public override int standType => 2;
        public override int standTier => 5;

        public override string Texture
        {
            get { return mod.Name + "/Items/KillerQueenT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Killer Queen (Stray Cat)");
            Tooltip.SetDefault("Left-click to shoot bubbles and right-click to detonate them!\nSpecial: Bite The Dust!\nRight-Click while holding the item to revert back to Killer Queen Final (You can revert back to BTD)\nNote: Manually detonating bubble bombs deal 2x damage!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 684;
            item.width = 32;
            item.height = 32;
            item.maxStack = 1;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.LightPurple;
        }

        public override void HoldItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (player.whoAmI == Main.myPlayer)
            {
                if (mPlayer.canRevertFromKQBTD)
                {
                    if (Main.mouseRight && mPlayer.revertTimer <= 0)
                    {
                        item.type = mod.ItemType("KillerQueenFinal");
                        item.SetDefaults(mod.ItemType("KillerQueenFinal"));
                        Main.PlaySound(SoundID.Grab);
                        mPlayer.revertTimer += 30;
                    }
                }
            }
        }

        public override bool ManualStandSpawning(Player player)
        {
            Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("KillerQueenBTDStand"), 0, 0f, Main.myPlayer);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("KillerQueenFinal"));
            recipe.AddIngredient(mod.ItemType("RequiemArrow"));
            recipe.AddIngredient(mod.ItemType("TaintedLifeforce"));
            recipe.AddIngredient(mod.ItemType("StrayCat"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void OnCraft(Recipe recipe)
        {
            Main.player[item.owner].GetModPlayer<MyPlayer>().canRevertFromKQBTD = true;
        }
    }
}