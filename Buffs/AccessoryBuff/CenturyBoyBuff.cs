using JoJoStands.Buffs.Debuffs;
using JoJoStands.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.AccessoryBuff
{
    public class CenturyBoyBuff : JoJoBuff
    {
        /*private int limitTimer = 10 * 60 * 60;
        private int breathSave = 0;*/
        private int useTime = 0;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("20th Century Boy");
            // Description.SetDefault("You are being protected!");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            player.shadowDodge = true;
            player.shadowDodgeCount = -100f;
            player.moveSpeed = 0f;
            player.lifeRegen = 0;
            player.manaRegen = 0;
            player.lavaImmune = true;
            player.noFallDmg = true;
            player.controlUseItem = false;
            player.controlQuickHeal = false;
            player.controlQuickMana = false;
            player.controlRight = false;
            player.controlUseTile = false;
            player.maxRunSpeed = 0f;
            player.slotsMinions += 1;
            /*if (JoJoStands.SecretReferences)
            {
                limitTimer--;
                if (player.wet)
                {
                    limitTimer -= 3;
                    if (player.ZoneSnow)
                        limitTimer -= 6;
                    if (breathSave == 0)
                        breathSave = player.breath;
                }
                if (breathSave != 0)
                    player.breath = breathSave;

                if (limitTimer <= 0)
                {
                    if (player.wet || (player.wet && player.ZoneSnow))
                        player.KillMe(PlayerDeathReason.ByCustomReason("The water kept it's constant rythm and " + player.name + " has stopped waiting. And stopped thinking."), player.statLife - 1, 1);
                    else
                        player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " has stopped thinking."), player.statLife - 1, 1);
                    limitTimer = 10 * 60 * 60;
                }
            }*/
            if (player.whoAmI == Main.myPlayer)
            {
                useTime++;
                if (useTime >= 60 * 60 / mPlayer.standTier)
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(useTime / 2 / 60));

                if (Main.mouseRight && mPlayer.standTier == 2 && player.HasItem(ItemID.Dynamite) && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                {
                    int highestDamage = 49;
                    for (int i = 0; i < 50; i++)        //At 50 is where the actual inventory stops, the rest is coins and ammo.
                    {
                        if (player.inventory[i] != null)
                        {
                            if (player.inventory[i].damage > highestDamage)
                                highestDamage = player.inventory[i].damage;
                        }
                    }

                    SoundEngine.PlaySound(SoundID.Item62, player.Center);
                    var explosion = Projectile.NewProjectile(player.GetSource_FromThis(), player.position, Vector2.Zero, ProjectileID.GrenadeIII, highestDamage * 3, 8f, Main.myPlayer);
                    Main.projectile[explosion].timeLeft = 2;
                    Main.projectile[explosion].netUpdate = true;
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(6));
                    player.ConsumeItem(ItemID.Dynamite);
                }
            }
        }

        public override void OnBuffEnd(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (player.whoAmI == Main.myPlayer)
            {
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime((4 / mPlayer.standTier) + (useTime / 60 / 2)));
                useTime = 0;
            }
        }
    }
}