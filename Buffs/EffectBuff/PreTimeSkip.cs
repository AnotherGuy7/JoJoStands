using JoJoStands.Projectiles.PlayerStands.KingCrimson;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class PreTimeSkip : ModBuff
    {
        public static int userIndex = -1;
        public static Vector2[] playerVelocity = new Vector2[Main.maxPlayers];

        public override void SetStaticDefaults()
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
            if (player.HasBuff(ModContent.BuffType<PreTimeSkip>()))
            {
                for (int i = 0; i < 255; i++)
                {
                    Player otherPlayer = Main.player[i];
                    if (otherPlayer.active && !otherPlayer.HasBuff(ModContent.BuffType<PreTimeSkip>()) && !otherPlayer.HasBuff(ModContent.BuffType<SkippingTime>()))
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
                player.GetModPlayer<MyPlayer>().timeskipPreEffect = true;
            }
            else
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<KingCrimsonStandT2>()] == 1)
                    player.AddBuff(ModContent.BuffType<SkippingTime>(), 180);
                else if (player.ownedProjectileCounts[ModContent.ProjectileType<KingCrimsonStandT3>()] == 1)
                    player.AddBuff(ModContent.BuffType<SkippingTime>(), 300);
                else if (player.ownedProjectileCounts[ModContent.ProjectileType<KingCrimsonStandFinal>()] == 1)
                    player.AddBuff(ModContent.BuffType<SkippingTime>(), 600);
                player.GetModPlayer<MyPlayer>().timeskipPreEffect = false;
            }
        }
    }
}