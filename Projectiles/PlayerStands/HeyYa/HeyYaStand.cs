using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
        public override bool? CanCutTiles()
        {
            return false;
        }

        public new enum AnimationState
        {
            Idle,
            Jump,
            Pose
        }
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;

        private static readonly List<HeyYaSpeechBubble> _bubbles = new List<HeyYaSpeechBubble>();
        private static readonly HashSet<HeyYaStand> _activeStands = new HashSet<HeyYaStand>();

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

        private static readonly int[] PumpedBuffPool =
        {
            BuffID.Battle,   // tier 0
            BuffID.Rage,     // tier 1
            BuffID.Wrath,    // tier 2
            BuffID.Panic,    // tier 3
            BuffID.Inferno,  // tier 4
        };
        private static readonly string[] PumpedBuffNames =
        {
            "Raaaah!!!",
            "Where is your Rage!",
            "Where is your Wrath!",
            "Rush time!",
            "You are flaming!",
        };

        private static readonly int[] ChillBuffPool =
        {
            BuffID.Sunflower,    // tier 0
            BuffID.Calm,         // tier 1
            BuffID.Fishing,      // tier 2
            BuffID.Shine,        // tier 3
            BuffID.Regeneration, // tier 4
        };
        private static readonly string[] ChillBuffNames =
        {
            "Sunflowers...",
            "Could use a breather, eh?",
            "Trust on your luck, fish...",
            "I can see a lot better now!",
            "Rest your muscles!",
        };

        private bool ChillMode => Projectile.ai[0] == 1f;
        private int SelectedBuffIndex
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        private readonly HashSet<int> _bossesSeenLastTick = new HashSet<int>();
        private readonly HashSet<int> _congratulatedBosses = new HashSet<int>();

        private bool _invasionWasActive = false;
        private int _lastInvasionType = 0;

        private bool _sandstormWasActive = false;
        private bool _rainWasActive = false;
        private bool _stormWasActive = false;
        private bool _blizzardWasActive = false;
        private bool _bloodMoonWasActive = false;
        private bool _eclipseWasActive = false;
        private bool _pumpkinMoonWasActive = false;
        private bool _frostMoonWasActive = false;
        private bool _slimeRainWasActive = false;
        private bool _windyWasActive = false;
        private bool _lanternNightWasActive = false;

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

        private const float BehindOffset = 24f;
        private const float FollowLerp = 0.35f;

        private readonly Dictionary<string, int> _adviceCooldowns = new Dictionary<string, int>();
        private const int AdviceCooldown = 600;

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
            if (Projectile.owner == Main.myPlayer)
                _activeStands.Add(this);
        }

        public override void OnKill(int timeLeft)
        {
            _activeStands.Remove(this);
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
            if (currentAnimationState == AnimationState.Idle) PlayAnimation("Idle");
            else if (currentAnimationState == AnimationState.Jump) PlayAnimation("Jump");
            else if (currentAnimationState == AnimationState.Pose) PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                standTexture = (Texture2D)ModContent.Request<Texture2D>(TextureRoot + "_" + animationName);

                int frameCount = animationName == "Idle" ? IdleFrameCount
                               : animationName == "Jump" ? JumpFrameCount
                               : 1;
                Projectile.height = standTexture.Height / frameCount;
            }

            if (animationName == "Idle") AnimateStand(animationName, IdleFrameCount, IdleFrameSpeed, true);
            else if (animationName == "Jump") AnimateStand(animationName, JumpFrameCount, JumpFrameSpeed, true);
            else if (animationName == "Pose") AnimateStand(animationName, 1, 12, true);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (mPlayer.posing) currentAnimationState = AnimationState.Pose;
            else if (player.velocity.Y != 0f) currentAnimationState = AnimationState.Jump;
            else currentAnimationState = AnimationState.Idle;

            SelectAnimation();
            FollowBehindPlayer(player);
            ApplySelectedBuff(player);
            ApplyPassiveStats(mPlayer);

            if (Projectile.owner == Main.myPlayer)
            {
                TickAdviceCooldowns();
                CheckAdviceTriggers(player);
                CheckSandstorm();
                CheckRain();
                CheckThunderstorm();
                CheckBlizzard();
                CheckBloodMoon();
                CheckEclipse();
                CheckPumpkinMoon();
                CheckFrostMoon();
                CheckSlimeRain();
                CheckWindy();
                CheckLanternNight();
                CheckInvasion();
                TickIdleLines();

                if (SpecialKeyPressed())
                    ToggleMode();

                if (SecondSpecialKeyPressed(false))
                    CycleSelectedBuff();
            }
        }

        private void ToggleMode()
        {
            Projectile.ai[0] = ChillMode ? 0f : 1f;
            SelectedBuffIndex = 0;
            string modeMsg = ChillMode ? "Let's take a breather!" : "Time for some action!";
            SpawnBubble(modeMsg, new Color(255, 220, 50), 80);
            SoundEngine.PlaySound(SoundID.Chat, Projectile.Center);
            Projectile.frame = 0;
            Projectile.frameCounter = 0;
            Projectile.netUpdate = true;
        }

        private void CycleSelectedBuff()
        {
            int maxIndex = Math.Min(Tier, (ChillMode ? ChillBuffPool.Length : PumpedBuffPool.Length) - 1);
            SelectedBuffIndex = (SelectedBuffIndex + 1) % (maxIndex + 1);

            string buffName = ChillMode
                ? ChillBuffNames[SelectedBuffIndex]
                : PumpedBuffNames[SelectedBuffIndex];

            SpawnBubble(buffName, new Color(100, 220, 255), 80);
            SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);

            int dustCount = 30;
            for (int k = 0; k < dustCount; k++)
            {
                float rot = MathHelper.ToRadians((360f / dustCount) * k);
                Vector2 dp = Projectile.Center + rot.ToRotationVector2() * 24f;
                int di = Dust.NewDust(dp, 1, 1, DustID.Electric);
                Main.dust[di].noGravity = true;
                Main.dust[di].velocity = rot.ToRotationVector2() * 3f;
            }

            Projectile.netUpdate = true;
        }

        private void ApplySelectedBuff(Player player)
        {
            int[] pool = ChillMode ? ChillBuffPool : PumpedBuffPool;
            int idx = SelectedBuffIndex;
            if (idx < 0 || idx > Tier || idx >= pool.Length) return;
            if (pool[idx] > 0)
                player.AddBuff(pool[idx], 2);
        }

        private void ResetIdleLineTimer()
        {
            _idleLineInterval = _rand.Next(3600 * 1, 3600 * 5);
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

        private void ApplyPassiveStats(MyPlayer mPlayer)
        {
            Player player = Main.player[Projectile.owner];
            player.GetCritChance(DamageClass.Generic) += CritBonus[Tier];
            player.fishingSkill += FishingPower[Tier];
            mPlayer.heyYaDodgeChance = DodgeChance[Tier];
            mPlayer.heyYaDropRateBonus = DropRateBonus[Tier];
            player.noFallDmg = true;
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

        private void CheckBossDefeats()
        {
            var bossesThisTick = new HashSet<int>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (n.active && n.boss)
                    bossesThisTick.Add(n.type);
            }
            foreach (int bossType in _bossesSeenLastTick)
            {
                if (!bossesThisTick.Contains(bossType) && !_congratulatedBosses.Contains(bossType))
                {
                    _congratulatedBosses.Add(bossType);
                    SpawnBubble("That big galoot didn't stand a chance against you!", new Color(255, 215, 0), HeyYaSpeechBubble.DefaultDuration * 2);
                    SoundEngine.PlaySound(SoundID.Chat, Projectile.Center);
                    break;
                }
            }

            _bossesSeenLastTick.Clear();
            foreach (int t in bossesThisTick)
                _bossesSeenLastTick.Add(t);
        }

        private void CheckSandstorm()
        {
            bool sandActive = Terraria.GameContent.Events.Sandstorm.Happening;
            if (sandActive && !_sandstormWasActive)
                TrySayAdvice("sandstorm_start", "Watch out, the desert's windy today!");
            else if (!sandActive && _sandstormWasActive)
                TrySayAdvice("sandstorm_end", "Phew, that sandstorm finally died down.");
            _sandstormWasActive = sandActive;
        }

        private void CheckRain()
        {
            bool rainActive = Main.raining;
            if (rainActive && !_rainWasActive)
            {
                string[] lines =
                {
                    "Looks like rain's coming in. Watch your footing out there!",
                    "A little rain never hurt anyone... probably.",
                    "Stay sharp -- wet ground means slippery slopes!",
                };
                TrySayAdvice("rain_start", lines[Main.rand.Next(lines.Length)]);
            }
            else if (!rainActive && _rainWasActive)
                TrySayAdvice("rain_end", "Finally clearing up. Good weather for a fight!");
            _rainWasActive = rainActive;
        }

        private void CheckThunderstorm()
        {
            bool stormActive = Main.raining && Main.maxRaining >= 0.8f;
            if (stormActive && !_stormWasActive)
            {
                string[] lines =
                {
                    "Stay out of open ground -- that lightning looks mean!",
                    "This storm's no joke. Eyes open and keep moving!",
                    "I've got a bad feeling about all this lightning...",
                };
                TrySayAdvice("storm_start", lines[Main.rand.Next(lines.Length)]);
            }
            else if (!stormActive && _stormWasActive && Main.raining)
                TrySayAdvice("storm_ease", "Storm's easing up a bit. Still raining, but at least no more lightning!");
            _stormWasActive = stormActive;
        }

        private void CheckBlizzard()
        {
            bool blizzardActive = Main.LocalPlayer.ZoneSnow && Main.raining;
            if (blizzardActive && !_blizzardWasActive)
            {
                string[] lines =
                {
                    "Bundle up tight -- this blizzard's no weather for a stroll!",
                    "A blizzard out here? Visibility's gonna be rough.",
                    "Wind chill's something fierce. Don't let it slow you down!",
                };
                TrySayAdvice("blizzard_start", lines[Main.rand.Next(lines.Length)]);
            }
            else if (!blizzardActive && _blizzardWasActive)
                TrySayAdvice("blizzard_end", "The blizzard's passed. Cold, but at least you can see again!");
            _blizzardWasActive = blizzardActive;
        }

        private void CheckBloodMoon()
        {
            bool bloodActive = Main.bloodMoon;
            if (bloodActive && !_bloodMoonWasActive)
            {
                string[] lines =
                {
                    "The moon's gone red. Never a good sign -- brace yourself!",
                    "Blood moon tonight. Lock the doors... if you had any!",
                    "Something about that red sky makes me want to keep moving.",
                    "Hey, whatever comes out tonight -- we'll handle it together!",
                };
                TrySayAdvice("blood_moon_start", lines[Main.rand.Next(lines.Length)]);
            }
            else if (!bloodActive && _bloodMoonWasActive)
                TrySayAdvice("blood_moon_end", "Sun's coming up. Made it through another blood moon -- lucky us!");
            _bloodMoonWasActive = bloodActive;
        }

        private void CheckEclipse()
        {
            bool eclipseActive = Main.eclipse;
            if (eclipseActive && !_eclipseWasActive)
            {
                string[] lines =
                {
                    "The sun's gone dark! Whatever crawls out in this -- it won't be friendly.",
                    "A solar eclipse? I've heard stories about what comes with these...",
                    "Midday and it's dark as night. This is gonna get rough!",
                };
                TrySayAdvice("eclipse_start", lines[Main.rand.Next(lines.Length)]);
            }
            else if (!eclipseActive && _eclipseWasActive)
                TrySayAdvice("eclipse_end", "The sun's back! Survived an eclipse -- now that's lucky.");
            _eclipseWasActive = eclipseActive;
        }

        private void CheckPumpkinMoon()
        {
            bool pumpkinActive = Main.pumpkinMoon;
            if (pumpkinActive && !_pumpkinMoonWasActive)
            {
                string[] lines =
                {
                    "Pumpkins and moonlight -- spooky out there tonight!",
                    "Halloween came early. Those things are no joke, stay sharp!",
                    "I can smell the trouble already. Let's make some luck!",
                };
                TrySayAdvice("pumpkin_moon_start", lines[Main.rand.Next(lines.Length)]);
            }
            _pumpkinMoonWasActive = pumpkinActive;
        }

        private void CheckFrostMoon()
        {
            bool frostActive = Main.snowMoon;
            if (frostActive && !_frostMoonWasActive)
            {
                string[] lines =
                {
                    "Cold and creepy -- the frost moon brings out the worst!",
                    "Even the monsters are dressed for winter tonight. Stay warm!",
                    "A frost moon... I prefer my luck warm, not frozen.",
                };
                TrySayAdvice("frost_moon_start", lines[Main.rand.Next(lines.Length)]);
            }
            _frostMoonWasActive = frostActive;
        }

        private void CheckSlimeRain()
        {
            bool slimeActive = Main.slimeRain;
            if (slimeActive && !_slimeRainWasActive)
            {
                string[] lines =
                {
                    "It's raining slimes?! Only us, I swear.",
                    "Watch your head -- those things bounce!",
                    "Of all the weather we could get... at least it's funny.",
                };
                TrySayAdvice("slime_rain_start", lines[Main.rand.Next(lines.Length)]);
            }
            else if (!slimeActive && _slimeRainWasActive)
                TrySayAdvice("slime_rain_end", "Slime rain's over. Ground's a mess but we're alright!");
            _slimeRainWasActive = slimeActive;
        }

        private void CheckWindy()
        {
            bool windyActive = Math.Abs(Main.windSpeedCurrent) >= 0.5f;
            if (windyActive && !_windyWasActive)
            {
                string[] lines =
                {
                    "Hold onto your hat -- it's really blowing out there!",
                    "This wind could knock you sideways. Watch your jumps!",
                    "Windy day. Projectiles are gonna go a little funny, heads up.",
                };
                TrySayAdvice("windy_start", lines[Main.rand.Next(lines.Length)]);
            }
            else if (!windyActive && _windyWasActive)
                TrySayAdvice("windy_end", "Wind's died down. Much better!");
            _windyWasActive = windyActive;
        }

        private void CheckLanternNight()
        {
            //bool lanternActive = Main.lanternNight;
            //if (lanternActive && !_lanternNightWasActive)
            //{
            //    string[] lines =
            //    {
            //        "Look at all those lanterns... what a peaceful night. Don't waste it!",
            //        "Lantern night -- even luck feels a little brighter tonight.",
            //        "Beautiful out. Almost makes you forget what we've been through, huh?",
            //    };
            //    TrySayAdvice("lantern_night_start", lines[Main.rand.Next(lines.Length)]);
            //}
            //_lanternNightWasActive = lanternActive;
        }

        public static void NotifyOwnerViralMeteoriteLanded()
        {
            foreach (var s in _activeStands) s.OnViralMeteoriteLanded();
        }

        internal void OnViralMeteoriteLanded()
        {
            TrySayAdvice("viral_meteor_land", "How lucky, a treasure from the sky for us!");
        }

        private void CheckInvasion()
        {
            bool invasionActive = Main.invasionType > 0 && Main.invasionProgressMax > 0;

            if (invasionActive && !_invasionWasActive)
            {
                _lastInvasionType = Main.invasionType;
                TrySayAdvice("invasion_start_" + _lastInvasionType,
                    "You've got luck and the home field advantage, let's go!");
            }
            else if (!invasionActive && _invasionWasActive)
            {
                bool won = Main.invasionProgressWave > 0;
                if (won)
                    TrySayAdvice("invasion_win_" + _lastInvasionType, "Great job beating 'em back!");
                else
                    TrySayAdvice("invasion_lose_" + _lastInvasionType, "Looks like we lucked out -- they're giving up!");
            }

            _invasionWasActive = invasionActive;
        }

        private void CheckAdviceTriggers(Player player)
        {
            CheckBossDefeats();

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
                    {
                        var bocAdvice = new (string, string)[]
                        {
                            ("boc_1", "This one seems to be lighter than the others. Hit it with something fast and hard to beat it back!"),
                            ("boc_2", "Those eye things give me a bad feeling… They'll probably do something nasty if they hit you!"),
                            ("boc_3", "It seems like the fakes aren't as solid as the original…"),
                        };
                        var (key, text) = bocAdvice[Main.rand.Next(bocAdvice.Length)];
                        TrySayAdvice(key, text);
                        return true;
                    }
                case NPCID.QueenBee:
                    {
                        var qbAdvice = new (string, string)[]
                        {
                            ("qb_1", "Those charges are only horizontal, right? Then can't we just jump out of the way?"),
                            ("qb_2", "Those bees it's shooting out don't look too durable. If we have something that can pierce them, can't we just get them out of the way and still hit the big one?"),
                        };
                        var (key, text) = qbAdvice[Main.rand.Next(qbAdvice.Length)];
                        TrySayAdvice(key, text);
                        return true;
                    }
                case NPCID.SkeletronHead:
                    TrySayAdvice("sk_1", "It looks like it's just attacking recklessly while it's spinning… Maybe you can hit it harder when it does that?");
                    return true;
                case NPCID.WallofFlesh:
                    {
                        var wofAdvice = new (string, string)[]
                        {
                            ("wof_1", "Go for the eyes!"),
                            ("wof_2", "Keep your distance, but don't go too far, or it'll pull you back!"),
                        };
                        var (key, text) = wofAdvice[Main.rand.Next(wofAdvice.Length)];
                        TrySayAdvice(key, text);
                        return true;
                    }
                case NPCID.TheDestroyer:
                    TrySayAdvice("dst_1", "Getting swarmed is a bad idea in any context! It should be best to take out the probes before you go back to damaging the worm.");
                    return true;
                case NPCID.Retinazer:
                case NPCID.Spazmatism:
                    TrySayAdvice("twn_1", "With a good horse and some flat ground, I think you should be able to just barely outrun these guys.");
                    return true;
                case NPCID.SkeletronPrime:
                    {
                        var skpAdvice = new (string, string)[]
                        {
                            ("skp_1", "That laser looks a bit tough to dodge… Maybe you should smash it up first?"),
                            ("skp_2", "It looks like the bombs only bounce if they hit solid ground. It should help to fight it pretty high up."),
                        };
                        var (key, text) = skpAdvice[Main.rand.Next(skpAdvice.Length)];
                        TrySayAdvice(key, text);
                        return true;
                    }
                case NPCID.Plantera:
                    {
                        var plaAdvice = new (string, string)[]
                        {
                            ("pla_1", "It looks like it's just moving in a straight line after you, and it's pretty slow… Can't we just run circles around it?"),
                            ("pla_2", "Those bouncy spiky thingies don't look like they can cover much ground vertically… Maybe try staying higher up?"),
                        };
                        var (key, text) = plaAdvice[Main.rand.Next(plaAdvice.Length)];
                        TrySayAdvice(key, text);
                        return true;
                    }
                case NPCID.Golem:
                    {
                        var golAdvice = new (string, string)[]
                        {
                            ("gol_1", "Looks like those fists aren't entirely connected to the main body. How about we… dis-arm him?"),
                            ("gol_2", "Looks like the head is detachable… Once it does that, it'll probably just be a race to the finish!"),
                        };
                        var (key, text) = golAdvice[Main.rand.Next(golAdvice.Length)];
                        TrySayAdvice(key, text);
                        return true;
                    }
                case NPCID.DukeFishron:
                    {
                        var dfAdvice = new (string, string)[]
                        {
                            ("df_1", "It doesn't look like he can turn while he's charging! Try to dodge at a right angle from where he's aiming!"),
                            ("df_2", "Those bubbles don't look very durable! A fast, wide-hitting weapon should swat 'em out of the air!"),
                        };
                        var (key, text) = dfAdvice[Main.rand.Next(dfAdvice.Length)];
                        TrySayAdvice(key, text);
                        return true;
                    }
                case NPCID.CultistBoss:
                    {
                        var lcAdvice = new (string, string)[]
                        {
                            ("lc_1", "The fakes have round eyes. The real one looks angry, and he's got a stripe on his hood."),
                            ("lc_2", "I think you can swat down the sparkly blue bolts with a weapon, that should come in handy!"),
                        };
                        var (key, text) = lcAdvice[Main.rand.Next(lcAdvice.Length)];
                        TrySayAdvice(key, text);
                        return true;
                    }
                case NPCID.MoonLordCore:
                    {
                        var mlAdvice = new (string, string)[]
                        {
                            ("ml_1", "That big laser looks like it packs a punch! I wouldn't want to get hit by that if I were you!"),
                            ("ml_2", "There's no way this guy doesn't have something up his sleeve for when we knock those eyes out. Maybe we should try to get them all out at once?"),
                            ("ml_3", "I doubt that you can escape the big guy, because he'll just warp over to you. But it looks like the eyeballs, once they're detached, are another story."),
                        };
                        var (key, text) = mlAdvice[Main.rand.Next(mlAdvice.Length)];
                        TrySayAdvice(key, text);
                        return true;
                    }
                default:
                    return false;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;

        public override void SendExtraAI(System.IO.BinaryWriter writer)
        {
            writer.Write((byte)currentAnimationState);
            writer.Write((byte)SelectedBuffIndex);
        }

        public override void ReceiveExtraAI(System.IO.BinaryReader reader)
        {
            currentAnimationState = (AnimationState)reader.ReadByte();
            SelectedBuffIndex = reader.ReadByte();
        }
    }
}