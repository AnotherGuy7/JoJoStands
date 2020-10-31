using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class BitesTheDust : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Bite The Dust");
            Description.SetDefault("The day is now restarting and your enemies are disappearing.");
            Main.persistentBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int i = 0; i < Main.maxPlayers; i++)       //first, get rid of all effect owners
                {
                    Player otherPlayer = Main.player[i];
                    if (otherPlayer.active && otherPlayer.team != player.team)
                    {
                        if (otherPlayer.HasBuff(mod.BuffType("TheWorldBuff")))
                        {
                            otherPlayer.ClearBuff(mod.BuffType("TheWorldBuff"));
                        }
                        if (otherPlayer.HasBuff(mod.BuffType("SkippingTime")))
                        {
                            otherPlayer.ClearBuff(mod.BuffType("SkippingTime"));
                        }
                        if (otherPlayer.HasBuff(mod.BuffType("ForesightBuff")))
                        {
                            otherPlayer.ClearBuff(mod.BuffType("ForesightBuff"));
                        }
                    }
                }
            }
            mPlayer.TheWorldEffect = false;     //second, get rid of the effects from everyone
            mPlayer.TimeSkipEffect = false;
            mPlayer.TimeSkipPreEffect = false;
            mPlayer.Foresight = false;
            if (Main.time != 1600)
            {
                player.AddBuff(mod.BuffType(Name), 2);       //to constantly refresh the buff
            }
            if (Main.time < 1600)
            {
                Main.time += 60;        //drag time down to 1600
            }
            if (Main.time > 1600)
            {
                Main.time -= 60;
            }
            if (Main.time == 1600)
            {
                if (!Main.dayTime)
                {
                    Main.dayTime = true;
                }
                player.statLife = player.statLifeMax;
                player.Spawn();
                player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(420));
                player.ClearBuff(mod.BuffType(Name));

            }
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active && !npc.dontTakeDamage && !npc.friendly && npc.lifeMax > 5 && Main.rand.Next(1, 16) == 1)
                {
                    int bomb = Projectile.NewProjectile(npc.Center + new Vector2(Main.rand.NextFloat(0f, 10f), Main.rand.NextFloat(0f, 10f)), Vector2.Zero, ProjectileID.GrenadeIII, 102, 3f, player.whoAmI);
                    Main.projectile[bomb].timeLeft = 2;
                    Main.projectile[bomb].netUpdate = true;
                }
            }
        }
    }
}