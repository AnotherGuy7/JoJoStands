using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class TestStand : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Test Stand");
			Tooltip.SetDefault("A faceless stand with unknown abilities...");
		}

        public override void SetDefaults()
        {
            item.damage = 70;
            item.width = 32;
            item.height = 32;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 2f;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = 6;
        }

        /*public override void HoldItem(Player player)
        {
            if (player.name == "Mod Test Shadow")
            {
                MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
                if (player.whoAmI == Main.myPlayer)
                {
                    if (player.ownedProjectileCounts[mod.ProjectileType("TestStand")] <= 0 && !mPlayer.StandOut)
                    {
                        Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("TestStand"), 0, 0f, Main.myPlayer);
                    }
                }
            }
        }*/

        /*public override bool UseItem(Player player)
        {
            if (player.name != "Mod Test Shadow")
            {
                Main.NewText("You are not worthy.", Color.Red);
            }
            return false;
        }*/
    }
}
