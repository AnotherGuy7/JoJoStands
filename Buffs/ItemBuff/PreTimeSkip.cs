using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class PreTimeSkip : ModBuff
    {
        public static int userIndex = -1;
        public static Vector2[] playerVelocity = new Vector2[255];

        public override void SetDefaults()
        {
			DisplayName.SetDefault("Skipping Time");
            Description.SetDefault("Time is skipping");
            Main.persistentBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;       //so that it can't be canceled
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            userIndex = player.whoAmI;
            if (player.HasBuff(mod.BuffType("PreTimeSkip")))
            {
                for (int i = 0; i < 255; i++)
                {
                    Player otherPlayer = Main.player[i];
                    if (otherPlayer.active && !otherPlayer.HasBuff(mod.BuffType("PreTimeSkip")) && !otherPlayer.HasBuff(mod.BuffType("SkippingTime")))
                    {
                        playerVelocity[i] = otherPlayer.velocity;
                        otherPlayer.controlUseItem = false;
                        otherPlayer.controlLeft = false;
                        otherPlayer.controlJump = false;
                        otherPlayer.controlRight = false;
                        otherPlayer.controlDown = false;
                        otherPlayer.controlQuickHeal = false;
                        otherPlayer.controlQuickMana = false;
                        otherPlayer.controlRight = false;
                        otherPlayer.controlUseTile = false;
                        otherPlayer.controlUp = false;
                    }
                }
                player.GetModPlayer<MyPlayer>().TimeSkipPreEffect = true;
            }
            else
            {
                if (player.ownedProjectileCounts[mod.ProjectileType("KingCrimsonStandT2")] == 1)
                {
                    player.AddBuff(mod.BuffType("SkippingTime"), 180);
                }
                else if (player.ownedProjectileCounts[mod.ProjectileType("KingCrimsonStandT3")] == 1)
                {
                    player.AddBuff(mod.BuffType("SkippingTime"), 300);
                }
                else if (player.ownedProjectileCounts[mod.ProjectileType("KingCrimsonStandFinal")] == 1)
                {
                    player.AddBuff(mod.BuffType("SkippingTime"), 600);
                }
                player.GetModPlayer<MyPlayer>().TimeSkipPreEffect = false;
            }
        }
    }
}