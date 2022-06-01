using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles.PlayerStands.KillerQueenBTD;
using JoJoStands.Tiles;
using Terraria;
using Terraria.Audio;
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
            get { return Mod.Name + "/Items/KillerQueenT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Killer Queen (Stray Cat)");
            Tooltip.SetDefault("Left-click to shoot bubbles and right-click to detonate them!\nSpecial: Bite The Dust!\nRight-Click while holding the Item to revert back to Killer Queen Final (You can revert back to BTD)\nNote: Manually detonating bubble bombs doubles damage!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 684;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
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
                        Item.type = ModContent.ItemType<KillerQueenFinal>();
                        Item.SetDefaults(ModContent.ItemType<KillerQueenFinal>());
                        SoundEngine.PlaySound(SoundID.Grab);
                        mPlayer.revertTimer += 30;
                    }
                }
            }
        }

        public override bool ManualStandSpawning(Player player)
        {
            Projectile.NewProjectile(player.GetSource_FromThis(), player.position, player.velocity, ModContent.ProjectileType<KillerQueenBTDStand>(), 0, 0f, Main.myPlayer);
            return true;
        }

        public override void OnCraft(Recipe recipe)
        {
            Main.player[Main.myPlayer].GetModPlayer<MyPlayer>().canRevertFromKQBTD = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<KillerQueenFinal>())
                .AddIngredient(ModContent.ItemType<RequiemArrow>())
                .AddIngredient(ModContent.ItemType<TaintedLifeforce>())
                .AddIngredient(ModContent.ItemType<StrayCat>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}