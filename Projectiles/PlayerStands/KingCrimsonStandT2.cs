using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands
{
    public class KingCrimsonStandT2 : StandClass
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/PlayerStands/KingCrimsonStandT1"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 11;
        }

        public override int punchDamage => 74;
        public override float punchKnockback => 3f;
        public override int punchTime => 24;      //KC's punch timings are based on it's frame, so punchTime has to be 3 frames longer than the duration of the frame KC punches in
        public override int halfStandHeight => 32;
        public override float fistWhoAmI => 6f;
        public override int drawOffsetRight => -10;
        public override int drawOffsetLeft => 0;

        public int updateTimer = 0;

        public override void AI()
        {
            SelectFrame();
            updateTimer++;
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }
            if (JoJoStands.SpecialHotKey.JustPressed && !player.HasBuff(mod.BuffType("AbilityCooldown")) && !player.HasBuff(mod.BuffType("SkippingTime")) && projectile.owner == Main.myPlayer)
            {
                player.AddBuff(mod.BuffType("PreTimeSkip"), 10);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/TimeSkip"));
            }

            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames)
                {
                    StayBehind();
                }
            }
            if (modPlayer.StandAutoMode)
            {
                BasicPunchAI();
            }
        }

        public virtual void SelectFrame()
        {
            Player player = Main.player[projectile.owner];
            projectile.frameCounter++;
            if (attackFrames)
            {
                normalFrames = false;
                if (projectile.frameCounter >= 21 - player.GetModPlayer<MyPlayer>().standSpeedBoosts)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame <= 3)
                {
                    projectile.frame = 4;
                }
                if (projectile.frame >= 10)
                {
                    projectile.frame = 4;
                }
            }
            if (normalFrames)
            {
                if (projectile.frameCounter >= 30)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                projectile.frame = 10;
            }
        }
    }
}