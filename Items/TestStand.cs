using JoJoStands.Networking;
using JoJoStands.Projectiles.PlayerStands.TestStand;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TestStand : StandItemClass
    {
        public override int StandSpeed => 12;
        public override int StandType => 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Test Stand");
            Tooltip.SetDefault("A faceless stand with unknown abilities...");
        }

        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.maxStack = 1;
            Item.knockBack = 2f;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        /*public override void HoldItem(Player player)
        {
            if (player.name == "Mod Test Shadow>()
            {
                MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
                if (player.whoAmI == Main.myPlayer)
                {
                    if (player.ownedProjectileCounts[ModContent.ProjectileType<TestStand>()] <= 0 && !mPlayer.StandOut)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.position, player.velocity, ModContent.ProjectileType<TestStand>(), 0, 0f, Main.myPlayer);
                    }
                }
            }
        }*/

        /*public override bool? UseItem(Player player)
        {
            if (player.name != "Mod Test Shadow>()
            {
                Main.NewText("You are not worthy.", Color.Red);
            }
            return false;
        }*/

        public override bool ManualStandSpawning(Player player)
        {
            if (MyPlayer.TestStandUnlocked)
            {
                Projectile.NewProjectile(player.GetSource_FromThis(), player.position, player.velocity, ModContent.ProjectileType<TestStandStand>(), 0, 0f, Main.myPlayer);
            }
            else
            {
                Main.NewText("You are not worthy.", Color.Red);
                player.GetModPlayer<MyPlayer>().standOut = false;
                SyncCall.SyncStandOut(player.whoAmI, false);
            }
            return true;
        }
    }
}
