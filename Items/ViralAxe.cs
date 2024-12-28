using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class ViralAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("An axe that seems to bind to the user.\nSpawns living infected wood shrapnel upon chopping a tree.");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 23;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.axe = 75 / 5;          //Whatever the value is it should be (that number / 5), cause weird vanilla stuff
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 1, 25, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        private int shootCooldown = 0;

        public override void HoldItem(Player player)
        {
            if (shootCooldown > 0)
            {
                shootCooldown--;
            }
            if (Main.mouseLeft && shootCooldown <= 0)
            {
                float mouseDistance = player.Distance(Main.MouseWorld);
                if (Main.tile[(int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16].TileType == TileID.Trees && mouseDistance <= 5f * 16f)
                {
                    shootCooldown += Item.useTime;
                    for (int p = 0; p < Main.rand.Next(3, 7); p++)
                    {
                        int projIndex = Projectile.NewProjectile(player.GetSource_FromThis(), player.position, new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f)), ModContent.ProjectileType<ViralWoodSharpnel>(), 23, 4f, player.whoAmI);
                        Main.projectile[projIndex].netUpdate = true;
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
