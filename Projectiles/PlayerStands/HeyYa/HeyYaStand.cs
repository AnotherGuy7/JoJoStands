using System;
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
    public class HeyYaSpeechBubble
    {
        public string Text;
        public Vector2 WorldPosition;
        public int TimeLeft;
        public int MaxTime;
        public Color TextColor;
        public Projectile Owner;

        public const int DefaultDuration = 240;

        public HeyYaSpeechBubble(string text, Vector2 worldPos, Color color, Projectile owner, int duration = DefaultDuration)
        {
            Text = text;
            WorldPosition = worldPos;
            MaxTime = duration;
            TimeLeft = duration;
            TextColor = color;
            Owner = owner;
        }
    }

    public abstract class HeyYaStand : StandClass
    {
        public new enum AnimationState
        {
            Idle,
            Jump,
            Pose
        }
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;

        private static readonly List<HeyYaSpeechBubble> _bubbles = new List<HeyYaSpeechBubble>();

        private const float BubbleTextScale = 0.7f;
        private const int BubbleMaxLineWidth = 160;

        private static readonly string[] IdleLines =
        {
            "Hey, ya!",
            "Don't let your guard down out there.",
            "I've got a good feeling about today!",
            "Stay sharp. You never know what's around the corner.",
            "You doing alright? You look a little tired.",
            "Man, this place is something else...",
            "I'm right here if you need me!",
            "Keep moving. Standing still never helped anyone.",
            "Something feels off... stay alert.",
            "You know, I think we make a pretty good team.",
        };

        private int _idleLineTimer = 0;
        private int _idleLineInterval = 0;
        private static readonly Random _rand = new Random();

        private static List<string> WrapText(string text, float scale)
        {
            var font = Terraria.GameContent.FontAssets.MouseText.Value;
            var words = text.Split(' ');
            var lines = new List<string>();
            string current = "";

            foreach (var word in words)
            {
                string test = current.Length == 0 ? word : current + " " + word;
                if (font.MeasureString(test).X * scale > BubbleMaxLineWidth && current.Length > 0)
                {
                    lines.Add(current);
                    current = word;
                }
                else
                {
                    current = test;
                }
            }
            if (current.Length > 0)
                lines.Add(current);

            return lines;
        }

        public static void DrawBubbles(SpriteBatch spriteBatch)
        {
            var font = Terraria.GameContent.FontAssets.MouseText.Value;
            float lineHeight = font.MeasureString("A").Y * BubbleTextScale;

            for (int i = _bubbles.Count - 1; i >= 0; i--)
            {
                var b = _bubbles[i];
                b.TimeLeft--;
                if (b.TimeLeft <= 0) { _bubbles.RemoveAt(i); continue; }

                if (b.Owner != null && b.Owner.active)
                    b.WorldPosition = b.Owner.Center + new Vector2(0f, -40f);
                else
                    b.WorldPosition.Y -= 0.18f;

                float progress = (float)b.TimeLeft / b.MaxTime;
                float alpha = progress < 0.25f ? (progress / 0.25f) : 1f;
                alpha = MathHelper.Clamp(alpha, 0f, 1f);

                Vector2 screenPos = b.WorldPosition - Main.screenPosition;
                List<string> lines = WrapText(b.Text, BubbleTextScale);
                float totalHeight = lines.Count * lineHeight;

                for (int l = 0; l < lines.Count; l++)
                {
                    float lineY = screenPos.Y - totalHeight * 0.5f + l * lineHeight;
                    Utils.DrawBorderString(
                        spriteBatch,
                        lines[l],
                        new Vector2(screenPos.X, lineY),
                        b.TextColor * alpha,
                        BubbleTextScale,
                        0.5f,
                        0f);
                }
            }
        }

        protected abstract string TextureRoot { get; }

        protected virtual int IdleFrameCount => 2;
        protected virtual int IdleFrameSpeed => 12;
        protected virtual int JumpFrameCount => 4;
        protected virtual int JumpFrameSpeed => 12;

        protected abstract int Tier { get; }

        private static readonly float[] CritBonus = { 4f, 6f, 8f, 10f, 12f };
        private static readonly float[] DropRateBonus = { 1f, 1.5f, 2f, 2.5f, 3f };
        private static readonly float[] DodgeChance = { 5f, 10f, 15f, 20f, 25f };
        private static readonly int[] FishingPower = { 5, 10, 15, 20, 25 };

        private static readonly int[] PumpedBuffs =
        {
            BuffID.Panic, BuffID.Battle, BuffID.Rage, BuffID.Wrath, BuffID.Inferno,
        };
        private static readonly int[] ChillBuffs =
        {
            BuffID.Sunflower, BuffID.Calm, BuffID.Fishing, BuffID.Shine, BuffID.Regeneration,
        };

        private const float BehindOffset = 24f;
        private const float MaxFlySpeed = 18f;
        private const float FollowLerp = 0.35f;

        private readonly Dictionary<string, int> _adviceCooldowns = new Dictionary<string, int>();
        private const int AdviceCooldown = 600;

        private bool ChillMode => Projectile.ai[0] == 1f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = Math.Max(IdleFrameCount, JumpFrameCount);
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
            Projectile.timeLeft = 2;
            ResetIdleLineTimer();
        }

        public override void SelectAnimation()
        {
            if (oldAnimationState != currentAnimationState)
            {
                Projectile.frame = 0;
                Projectile.frameCounter = 0;
                oldAnimationState = currentAnimationState;
                Projectile.netUpdate = true;
            }
            if (currentAnimationState == AnimationState.Idle)
                PlayAnimation("Idle");
            else if (currentAnimationState == AnimationState.Jump)
                PlayAnimation("Jump");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>(TextureRoot + "_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, IdleFrameCount, IdleFrameSpeed, true);
            else if (animationName == "Jump")
                AnimateStand(animationName, JumpFrameCount, JumpFrameSpeed, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 12, true);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
            else if (player.velocity.Y != 0f)
                currentAnimationState = AnimationState.Jump;
            else
                currentAnimationState = AnimationState.Idle;

            SelectAnimation();
            FollowBehindPlayer(player);
            ApplyBuffs(player);
            ApplyPassiveStats(mPlayer);

            if (Projectile.owner == Main.myPlayer)
            {
                TickAdviceCooldowns();
                CheckAdviceTriggers(player);
                TickIdleLines();

                if (SpecialKeyPressed())
                    ToggleMode();
            }
        }

        private void ResetIdleLineTimer()
        {
            _idleLineInterval = _rand.Next(3600 * 5, 3600 * 10);
            _idleLineTimer = 0;
        }

        private void TickIdleLines()
        {
            _idleLineTimer++;
            if (_idleLineTimer >= _idleLineInterval)
            {
                string line = IdleLines[_rand.Next(IdleLines.Length)];
                SpawnBubble(line, new Color(255, 255, 255));
                SoundEngine.PlaySound(SoundID.Chat, Projectile.Center);
                ResetIdleLineTimer();
            }
        }

        private void FollowBehindPlayer(Player player)
        {
            float dirSign = player.direction;
            Vector2 target = player.Center + new Vector2(-dirSign * BehindOffset, 0f);
            if (Vector2.Distance(Projectile.Center, target) > 16f * 20f)
            {
                Projectile.Center = target;
                Projectile.velocity = Vector2.Zero;
            }
            else
                Projectile.velocity = (target - Projectile.Center) * FollowLerp;
            Projectile.spriteDirection = player.direction;
            Projectile.rotation = 0f;
            Projectile.tileCollide = false;
            Projectile.netUpdate = true;
        }

        private void ApplyBuffs(Player player)
        {
            int[] pool = ChillMode ? ChillBuffs : PumpedBuffs;
            for (int i = 0; i <= Tier && i < pool.Length; i++)
                if (pool[i] > 0)
                    player.AddBuff(pool[i], 2);
        }

        private void ApplyPassiveStats(MyPlayer mPlayer)
        {
            Player player = Main.player[Projectile.owner];
            player.GetCritChance(DamageClass.Generic) += CritBonus[Tier];
            player.fishingSkill += FishingPower[Tier];
            mPlayer.heyYaDodgeChance = DodgeChance[Tier];
            mPlayer.heyYaDropRateBonus = DropRateBonus[Tier];
            player.noFallDmg = true;
        }

        private void ToggleMode()
        {
            Projectile.ai[0] = ChillMode ? 0f : 1f;
            string modeMsg = ChillMode ? "Let's take a breather!" : "Time for some action!";
            SpawnBubble(modeMsg, new Color(255, 220, 50));
            SoundEngine.PlaySound(SoundID.Chat, Projectile.Center);
            Projectile.frame = 0;
            Projectile.frameCounter = 0;
            Projectile.netUpdate = true;
        }

        private void SpawnBubble(string text, Color color, int duration = HeyYaSpeechBubble.DefaultDuration)
        {
            if (Projectile.owner != Main.myPlayer) return;
            Vector2 pos = Projectile.Center + new Vector2(0f, -40f);
            _bubbles.Add(new HeyYaSpeechBubble(text, pos, color, Projectile, duration));
        }

        private bool TrySayAdvice(string key, string message)
        {
            if (_adviceCooldowns.ContainsKey(key)) return false;
            _adviceCooldowns[key] = AdviceCooldown;
            SpawnBubble(message, new Color(100, 220, 255), HeyYaSpeechBubble.DefaultDuration * 2);
            SoundEngine.PlaySound(SoundID.Chat, Projectile.Center);
            return true;
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
                if (!n.active || !n.boss) continue;

                if (!_adviceCooldowns.ContainsKey("summoned_" + n.type))
                {
                    _adviceCooldowns["summoned_" + n.type] = AdviceCooldown * 10;
                    bool hadSpecific = SayBossAdvice(n.type);
                    if (!hadSpecific)
                        TrySayAdvice("summoned_any", "There's nothing but good luck in your bag! Let's do this!");
                }
            }
        }

        private bool SayBossAdvice(int npcType)
        {
            switch (npcType)
            {
                case NPCID.KingSlime:
                    TrySayAdvice("ks_1", "Can this big guy only move vertically by jumping? If you get up high enough, it shouldn't be able to reach you easily!");
                    return true;
                case NPCID.EyeofCthulhu:
                    TrySayAdvice("eoc_1", "When it lunges at you, it should help to run in a straight line-- this big eye doesn't seem to know how to lead the target!");
                    return true;
                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
                    TrySayAdvice("eow_1", "Get this worm out of its element. Fight it on the surface.");
                    return true;
                case NPCID.BrainofCthulhu:
                    TrySayAdvice("boc_1", "This one seems to be lighter than the others. Hit it with something fast and hard to beat it back!");
                    TrySayAdvice("boc_2", "Those eye things give me a bad feeling… They'll probably do something nasty if they hit you!");
                    TrySayAdvice("boc_3", "It seems like the fakes aren't as solid as the original…");
                    return true;
                case NPCID.QueenBee:
                    TrySayAdvice("qb_1", "Those charges are only horizontal, right? Then can't we just jump out of the way?");
                    TrySayAdvice("qb_2", "Those bees it's shooting out don't look too durable. If we have something that can pierce them, can't we just get them out of the way and still hit the big one?");
                    return true;
                case NPCID.SkeletronHead:
                    TrySayAdvice("sk_1", "It looks like it's just attacking recklessly while it's spinning… Maybe you can hit it harder when it does that?");
                    return true;
                case NPCID.WallofFlesh:
                    TrySayAdvice("wof_1", "Go for the eyes!");
                    TrySayAdvice("wof_2", "Keep your distance, but don't go too far, or it'll pull you back!");
                    return true;
                case NPCID.TheDestroyer:
                    TrySayAdvice("dst_1", "Getting swarmed is a bad idea in any context! It should be best to take out the probes before you go back to damaging the worm.");
                    return true;
                case NPCID.Retinazer:
                case NPCID.Spazmatism:
                    TrySayAdvice("twn_1", "With a good horse and some flat ground, I think you should be able to just barely outrun these guys.");
                    return true;
                case NPCID.SkeletronPrime:
                    TrySayAdvice("skp_1", "That laser looks a bit tough to dodge… Maybe you should smash it up first?");
                    TrySayAdvice("skp_2", "It looks like the bombs only bounce if they hit solid ground. It should help to fight it pretty high up.");
                    return true;
                case NPCID.Plantera:
                    TrySayAdvice("pla_1", "It looks like it's just moving in a straight line after you, and it's pretty slow… Can't we just run circles around it?");
                    TrySayAdvice("pla_2", "Those bouncy spiky thingies don't look like they can cover much ground vertically… Maybe try staying higher up?");
                    return true;
                case NPCID.Golem:
                    TrySayAdvice("gol_1", "Looks like those fists aren't entirely connected to the main body. How about we… dis-arm him?");
                    TrySayAdvice("gol_2", "Looks like the head is detachable… Once it does that, it'll probably just be a race to the finish!");
                    return true;
                case NPCID.DukeFishron:
                    TrySayAdvice("df_1", "It doesn't look like he can turn while he's charging! Try to dodge at a right angle from where he's aiming!");
                    TrySayAdvice("df_2", "Those bubbles don't look very durable! A fast, wide-hitting weapon should swat 'em out of the air!");
                    return true;
                case NPCID.CultistBoss:
                    TrySayAdvice("lc_1", "The fakes have round eyes. The real one looks angry, and he's got a stripe on his hood.");
                    TrySayAdvice("lc_2", "I think you can swat down the sparkly blue bolts with a weapon, that should come in handy!");
                    return true;
                case NPCID.MoonLordCore:
                    TrySayAdvice("ml_1", "That big laser looks like it packs a punch! I wouldn't want to get hit by that if I were you!");
                    TrySayAdvice("ml_2", "There's no way this guy doesn't have something up his sleeve for when we knock those eyes out. Maybe we should try to get them all out at once?");
                    TrySayAdvice("ml_3", "I doubt that you can escape the big guy, because he'll just warp over to you. But it looks like the eyeballs, once they're detached, are another story.");
                    return true;
                default:
                    return false;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;

        public override void SendExtraAI(System.IO.BinaryWriter writer)
        {
            writer.Write((byte)currentAnimationState);
        }

        public override void ReceiveExtraAI(System.IO.BinaryReader reader)
        {
            currentAnimationState = (AnimationState)reader.ReadByte();
        }
    }
}