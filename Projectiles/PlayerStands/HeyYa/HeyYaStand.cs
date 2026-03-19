using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.HeyYa
{
    public abstract class HeyYaStand : StandClass
    {
        protected abstract string IdleTexture { get; }
        protected virtual int IdleFrameCount => 4;

        protected abstract int Tier { get; }

        private static readonly float[] CritBonus = { 4f, 6f, 8f, 10f, 12f };
        private static readonly float[] DropRateBonus = { 1f, 1.5f, 2f, 2.5f, 3f };
        private static readonly float[] DodgeChance = { 5f, 10f, 15f, 20f, 25f };
        private static readonly int[] FishingPower = { 5, 10, 15, 20, 25 };

        private static readonly int[] PumpedBuffs =
        {
            BuffID.Panic,       // T1 – Panic!
            BuffID.Battle,      // T2 – Battle (use your mod buff if custom)
            BuffID.Rage,        // T3 – Rage
            BuffID.Wrath,       // T4 – Wrath
            BuffID.Inferno,     // T5 – Inferno
        };

        private static readonly int[] ChillBuffs =
        {
            BuffID.Sunflower,           // T1 – Happy!
            BuffID.Calm,            // T2 – Calm
            BuffID.Fishing,         // T3 – Fishing
            BuffID.Shine,           // T4 – Shine
            BuffID.Regeneration,    // T5 – Regeneration
        };

        private const int FrameSpeed = 12;
        private const float HoverHeight = 3.5f * 16f;
        private const float MaxFlySpeed = 10f;
        private const float PatrolRange = 3f * 16f;
        private const float PatrolSpeed = 0.18f;
        private const float RotLerpSpeed = 0.18f;

        private float _patrolOffset = 0f;
        private float _patrolDir = 1f;

        private readonly Dictionary<string, int> _adviceCooldowns = new Dictionary<string, int>();
        private const int AdviceCooldown = 600;

        private bool ChillMode => Projectile.ai[0] == 1f;

        public override string Texture => IdleTexture;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = IdleFrameCount;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
            Projectile.timeLeft = 2;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= FrameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % IdleFrameCount;
            }

            HoverNearPlayer(player);

            ApplyBuffs(player);

            ApplyPassiveStats(mPlayer);

            if (Projectile.owner == Main.myPlayer)
            {
                TickAdviceCooldowns();
                CheckAdviceTriggers(player);
            }

            if (Projectile.owner == Main.myPlayer && SpecialKeyPressed())
                ToggleMode();
        }

        private void ApplyBuffs(Player player)
        {
            int[] pool = ChillMode ? ChillBuffs : PumpedBuffs;
            for (int i = 0; i <= Tier && i < pool.Length; i++)
            {
                if (pool[i] > 0)
                    player.AddBuff(pool[i], 2);
            }
        }

        private void ApplyPassiveStats(MyPlayer mPlayer)
        {
            Player player = Main.player[Projectile.owner];

            player.GetCritChance(DamageClass.Generic) += CritBonus[Tier];

            player.fishingSkill += FishingPower[Tier];

            mPlayer.heyYaDodgeChance = DodgeChance[Tier];

            mPlayer.heyYaDropRateBonus = DropRateBonus[Tier];

            player.noFallDmg = true;

            //mPlayer.heyYaImmuneToCheapTrick = true;
        }

        private void ToggleMode()
        {
            Projectile.ai[0] = ChillMode ? 0f : 1f;
            string modeMsg = ChillMode
                ? "Let's take a breather!"
                : "Time for some action!";
            if (Projectile.owner == Main.myPlayer)
                Main.NewText(modeMsg, 255, 220, 50);

            SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
            Projectile.frame = 0;
            Projectile.frameCounter = 0;
            Projectile.netUpdate = true;
        }

        private void HoverNearPlayer(Player player)
        {
            Vector2 hoverBase = player.Center + new Vector2(0f, -HoverHeight);
            _patrolOffset += PatrolSpeed * _patrolDir;
            if (_patrolOffset >= PatrolRange)
            {
                _patrolOffset = PatrolRange;
                _patrolDir = -1f;
            }
            else if (_patrolOffset <= -PatrolRange)
            {
                _patrolOffset = -PatrolRange;
                _patrolDir = 1f;
            }

            Vector2 hoverTarget = hoverBase + new Vector2(_patrolOffset, 0f);

            if (Projectile.Distance(player.Center) > 16 * 16f)
            {
                Projectile.tileCollide = false;
                Projectile.velocity = player.Center - Projectile.Center;
                Projectile.velocity.Normalize();
                Projectile.velocity *= MaxFlySpeed + player.moveSpeed;
                Projectile.netUpdate = true;
            }
            else
            {
                Vector2 toTarget = hoverTarget - Projectile.Center;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, toTarget * 0.12f, 0.15f);
                Projectile.tileCollide = false;
                Projectile.netUpdate = true;
            }

            Projectile.rotation = LerpAngle(Projectile.rotation, 0f, RotLerpSpeed * 0.5f);
        }

        private void TickAdviceCooldowns()
        {
            var keys = new List<string>(_adviceCooldowns.Keys);
            foreach (var k in keys)
            {
                _adviceCooldowns[k]--;
                if (_adviceCooldowns[k] <= 0)
                    _adviceCooldowns.Remove(k);
            }
        }

        private void CheckAdviceTriggers(Player player)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (!n.active || !n.boss)
                    continue;

                string bossKey = "boss_" + n.type;

                if (!_adviceCooldowns.ContainsKey("summoned_" + n.type))
                {
                    _adviceCooldowns["summoned_" + n.type] = AdviceCooldown * 10;
                    TrySayAdvice("summoned_any", "There's nothing but good luck in your bag! Let's do this!");
                    SayBossAdvice(n.type);
                }
            }
        }

        private void TrySayAdvice(string key, string message)
        {
            if (_adviceCooldowns.ContainsKey(key))
                return;
            _adviceCooldowns[key] = AdviceCooldown;
            Main.NewText(message, 100, 220, 255);
        }

        private void SayBossAdvice(int npcType)
        {
            switch (npcType)
            {
                case NPCID.KingSlime:
                    TrySayAdvice("ks_1", "Can this big guy only move vertically by jumping? If you get up high enough, it shouldn't be able to reach you easily!");
                    break;
                case NPCID.EyeofCthulhu:
                    TrySayAdvice("eoc_1", "When it lunges at you, it should help to run in a straight line-- this big eye doesn't seem to know how to lead the target!");
                    break;
                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
                    TrySayAdvice("eow_1", "Get this worm out of its element. Fight it on the surface.");
                    break;
                case NPCID.BrainofCthulhu:
                    TrySayAdvice("boc_1", "This one seems to be lighter than the others. Hit it with something fast and hard to beat it back!");
                    TrySayAdvice("boc_2", "Those eye things give me a bad feeling… They'll probably do something nasty if they hit you!");
                    TrySayAdvice("boc_3", "It seems like the fakes aren't as solid as the original…");
                    break;
                case NPCID.QueenBee:
                    TrySayAdvice("qb_1", "Those charges are only horizontal, right? Then can't we just jump out of the way?");
                    TrySayAdvice("qb_2", "Those bees it's shooting out don't look too durable. If we have something that can pierce them, can't we just get them out of the way and still hit the big one?");
                    break;
                case NPCID.SkeletronHead:
                    TrySayAdvice("sk_1", "It looks like it's just attacking recklessly while it's spinning… Maybe you can hit it harder when it does that?");
                    break;
                case NPCID.WallofFlesh:
                    TrySayAdvice("wof_1", "Go for the eyes!");
                    TrySayAdvice("wof_2", "Keep your distance, but don't go too far, or it'll pull you back!");
                    break;
                case NPCID.TheDestroyer:
                    TrySayAdvice("dst_1", "Getting swarmed is a bad idea in any context! It should be best to take out the probes before you go back to damaging the worm.");
                    break;
                case NPCID.Retinazer:
                case NPCID.Spazmatism:
                    TrySayAdvice("twn_1", "With a good horse and some flat ground, I think you should be able to just barely outrun these guys.");
                    break;
                case NPCID.SkeletronPrime:
                    TrySayAdvice("skp_1", "That laser looks a bit tough to dodge… Maybe you should smash it up first?");
                    TrySayAdvice("skp_2", "It looks like the bombs only bounce if they hit solid ground. It should help to fight it pretty high up.");
                    break;
                case NPCID.Plantera:
                    TrySayAdvice("pla_1", "It looks like it's just moving in a straight line after you, and it's pretty slow… Can't we just run circles around it?");
                    TrySayAdvice("pla_2", "Those bouncy spiky thingies don't look like they can cover much ground vertically… Maybe try staying higher up?");
                    break;
                case NPCID.Golem:
                    TrySayAdvice("gol_1", "Looks like those fists aren't entirely connected to the main body. How about we… dis-arm him?");
                    TrySayAdvice("gol_2", "Looks like the head is detachable… Once it does that, it'll probably just be a race to the finish!");
                    break;
                case NPCID.DukeFishron:
                    TrySayAdvice("df_1", "It doesn't look like he can turn while he's charging! Try to dodge at a right angle from where he's aiming!");
                    TrySayAdvice("df_2", "Those bubbles don't look very durable! A fast, wide-hitting weapon should swat 'em out of the air!");
                    break;
                case NPCID.CultistBoss:
                    TrySayAdvice("lc_1", "The fakes have round eyes. The real one looks angry, and he's got a stripe on his hood.");
                    TrySayAdvice("lc_2", "I think you can swat down the sparkly blue bolts with a weapon, that should come in handy!");
                    break;
                case NPCID.MoonLordCore:
                    TrySayAdvice("ml_1", "That big laser looks like it packs a punch! I wouldn't want to get hit by that if I were you!");
                    TrySayAdvice("ml_2", "There's no way this guy doesn't have something up his sleeve for when we knock those eyes out. Maybe we should try to get them all out at once?");
                    TrySayAdvice("ml_3", "I doubt that you can escape the big guy, because he'll just warp over to you. But it looks like the eyeballs, once they're detached, are another story.");
                    break;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> texAsset = ModContent.Request<Texture2D>(IdleTexture);
            Texture2D tex = texAsset.Value;
            int frameHeight = tex.Height / IdleFrameCount;
            Rectangle source = new Rectangle(0, Projectile.frame * frameHeight, tex.Width, frameHeight);
            Vector2 origin = new Vector2(tex.Width / 2f, frameHeight / 2f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Main.EntitySpriteDraw(tex, drawPos, source, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;

        public override void SendExtraAI(System.IO.BinaryWriter writer)
        {
            writer.Write(_patrolOffset);
            writer.Write(_patrolDir);
        }

        public override void ReceiveExtraAI(System.IO.BinaryReader reader)
        {
            _patrolOffset = reader.ReadSingle();
            _patrolDir = reader.ReadSingle();
        }

        private static float LerpAngle(float from, float to, float amount)
        {
            float diff = to - from;
            while (diff > MathHelper.Pi) diff -= MathHelper.TwoPi;
            while (diff < -MathHelper.Pi) diff += MathHelper.TwoPi;
            return from + diff * amount;
        }
    }
}