using JoJoStands.Buffs.Debuffs;
using JoJoStands.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.AccessoryBuff
{
    public class CenturyBoyBuff : JoJoBuff
    {
        private int limitTimer = 10 * 60 * 60;
        private int breathSave = 0;

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
            player.GetDamage(DamageClass.Summon) *= 0.7f;
            if (JoJoStands.SecretReferences)
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
            }
            if (Main.mouseRight && mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<CenturyBoyT2>() && player.HasItem(ItemID.Dynamite) && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
            {
                SoundEngine.PlaySound(SoundID.Item62, player.Center);
                var explosion = Projectile.NewProjectile(player.GetSource_FromThis(), player.position, Vector2.Zero, ProjectileID.GrenadeIII, 49, 8f, Main.myPlayer);
                Main.projectile[explosion].timeLeft = 2;
                Main.projectile[explosion].netUpdate = true;
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(6));
                player.ConsumeItem(ItemID.Dynamite);
            }
        }
    }
}