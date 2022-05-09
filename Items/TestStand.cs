using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Items
{
	public class TestStand : StandItemClass
	{
        public override int standSpeed => 12;
        public override int standType => 1;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Test Stand>();
			Tooltip.SetDefault("A faceless stand with unknown abilities...>();
		}

        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = 5;
            Item.maxStack = 1;
            Item.knockBack = 2f;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = 6;
        }

        /*public override void HoldItem(Player player)
        {
            if (player.name == "Mod Test Shadow>()
            {
                MyPlayer mPlayer = player.GetModPlayer<MyPlayer>());
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
            if (MyPlayer.testStandUnlocked)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.position, player.velocity, ModContent.ProjectileType<TestStand>(), 0, 0f, Main.myPlayer);
            }
            else
            {
                Main.NewText("You are not worthy.", Color.Red);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Networking.ModNetHandler.playerSync.SendStandOut(256, player.whoAmI, false, player.whoAmI);
                }
                player.GetModPlayer<MyPlayer>()).standOut = false;
            }
            return true;
        }
    }
}
