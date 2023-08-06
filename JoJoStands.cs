using JoJoStands.Items;
using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Items.Seasonal;
using JoJoStands.Networking;
using JoJoStands.Projectiles;
using JoJoStands.Projectiles.PlayerStands.Aerosmith;
using JoJoStands.Projectiles.PlayerStands.Cream;
using JoJoStands.Projectiles.PlayerStands.GoldExperienceRequiem;
using JoJoStands.Projectiles.PlayerStands.StarPlatinum;
using JoJoStands.Projectiles.PlayerStands.TestStand;
using JoJoStands.Projectiles.PlayerStands.TheWorld;
using JoJoStands.Projectiles.PlayerStands.Tusk;
using JoJoStands.UI;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static JoJoStands.MyPlayer;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands
{
    public class JoJoStands : Mod
    {
        public static JoJoStands Instance;
        public static bool SoundsLoaded = false;
        public static bool FanStandsLoaded = false;
        public static Mod JoJoStandsSounds;
        public static Mod JoJoFanStands;

        public static ModKeybind SpecialHotKey;
        public static ModKeybind SecondSpecialHotKey;
        public static ModKeybind StandOutHotKey;
        public static ModKeybind StandAutoModeHotKey;
        public static ModKeybind PoseHotKey;

        public static List<int> timestopImmune;
        public static List<int> timestopOverrideStands;
        public static List<int> standTier1List;
        public static List<int> standProjectileList;
        public static List<int> christmasStands;
        public static List<char> testStandPassword = new List<char>();      //  :) Have *fuuuuuuun!*

        public static float RangeIndicatorAlpha;
        public static bool Sounds = true;
        public static bool TimestopEffects = false;
        public static bool RangeIndicators = false;
        public static bool AutomaticActivations = false;
        public static bool StandAimAssist = false;
        public static bool SecretReferences = false;
        public static int StandSlotPositionX;
        public static int StandSlotPositionY;
        public static float HamonBarPositionX;
        public static float HamonBarPositionY;
        public static float ModSoundsVolume;
        public static bool ColorChangeEffects = false;
        public static bool TimeskipEffects = false;
        public static bool BiteTheDustEffects = false;
        public static bool RespawnWithStandOut = true;
        public static bool StandPvPMode = false;
        public static bool AbilityWheelDescriptions = true;
        public static bool SoundsModAbilityVoicelines = true;
        public static DeathSoundType DeathSoundID;
        public static ColorChangeStyleEnum ColorChangeStyle = ColorChangeStyleEnum.None;
        public static StandSearchTypeEnum StandSearchTypeEnum = StandSearchTypeEnum.Bosses;
        public static bool TestStandUnlocked = false;

        public enum ColorChangeStyleEnum
        {
            None,
            NormalToLightGreen,
            NormalToBlue,
            NormalToPurple,
            NormalToRed,
            NormalToDarkBlue
        }

        public enum DeathSoundType
        {
            None,
            Roundabout,
            Caesar,
            KonoMeAmareriMaroreriMerareMaro,
            LastTrainHome,
            KingCrimsonNoNorioKu,
        }

        public override void Load()
        {
            Instance = GetInstance<JoJoStands>();
            SoundsLoaded = ModLoader.TryGetMod("JoJoStandsSounds", out JoJoStandsSounds);
            FanStandsLoaded = ModLoader.TryGetMod("JoJoFanStands", out JoJoFanStands);

            StandItemClass.standTierNumerals = (Texture2D)Request<Texture2D>("JoJoStands/Extras/StandTierNumerals", AssetRequestMode.ImmediateLoad);
            HamonBar.hamonBarTexture = (Texture2D)Request<Texture2D>("JoJoStands/UI/HamonBar", AssetRequestMode.ImmediateLoad);
            ToBeContinued.TBCArrowTexture = (Texture2D)Request<Texture2D>("JoJoStands/UI/TBCArrow", AssetRequestMode.ImmediateLoad);
            BulletCounter.bulletCounterTexture = (Texture2D)Request<Texture2D>("JoJoStands/UI/BulletCounter", AssetRequestMode.ImmediateLoad);
            AerosmithRadar.aerosmithRadarBackgroundTexture = (Texture2D)Request<Texture2D>("JoJoStands/UI/AerosmithRadar_Background", AssetRequestMode.ImmediateLoad);
            AerosmithRadar.aerosmithRadarCrosshairTexture = (Texture2D)Request<Texture2D>("JoJoStands/UI/AerosmithRadar_Crosshair", AssetRequestMode.ImmediateLoad);
            AerosmithRadar.aerosmithRadarBorderTexture = (Texture2D)Request<Texture2D>("JoJoStands/UI/AerosmithRadar_Border", AssetRequestMode.ImmediateLoad);
            AerosmithRadar.aerosmithRadarBlipTexture = (Texture2D)Request<Texture2D>("JoJoStands/Extras/AerosmithRadar_Blip", AssetRequestMode.ImmediateLoad);
            GoldenSpinMeter.goldenRectangleTexture = (Texture2D)Request<Texture2D>("JoJoStands/UI/GoldenSpinMeter", AssetRequestMode.ImmediateLoad);
            GoldenSpinMeter.goldenRectangleSpinLineTexture = (Texture2D)Request<Texture2D>("JoJoStands/UI/GoldenSpinMeterLine", AssetRequestMode.ImmediateLoad);
            SexPistolsUI.sexPistolsUITexture = (Texture2D)Request<Texture2D>("JoJoStands/UI/SexPistolsUI", AssetRequestMode.ImmediateLoad);
            VoidBar.VoidBarTexture = (Texture2D)Request<Texture2D>("JoJoStands/UI/VoidBar", AssetRequestMode.ImmediateLoad);
            VoidBar.VoidBarBarTexture = (Texture2D)Request<Texture2D>("JoJoStands/UI/VoidBarBar", AssetRequestMode.ImmediateLoad);
            PolaroidTokenLayer.MenacingTextureSpritesheet = Request<Texture2D>("JoJoStands/Extras/MenacingIcons", AssetRequestMode.ImmediateLoad).Value;


            AerosmithStandFinal.AerosmithWhirrSoundEffect = (SoundEffect)Request<SoundEffect>("JoJoStands/Sounds/GameSounds/AerosmithWhirr", AssetRequestMode.ImmediateLoad);

            standProjectileList = new List<int>
            {
                ProjectileType<BombBubble>(),
                ProjectileType<ControllableNail>(),
                ProjectileType<CrossfireHurricaneAnkh>(),
                ProjectileType<DollyDaggerBeam>(),
                ProjectileType<Emerald>(),
                ProjectileType<FireAnkh>(),
                ProjectileType<Fists>(),
                ProjectileType<GoldExperienceBeam>(),
                ProjectileType<HermitPurpleGrab>(),
                ProjectileType<HermitPurpleWhip>(),
                ProjectileType<HighVelocityBubble>(),
                ProjectileType<KnifeProjectile>(),
                ProjectileType<MeltYourHeart>(),
                ProjectileType<MeltYourHeartDrip>(),
                ProjectileType<Nail>(),
                ProjectileType<NailSlasher>(),
                ProjectileType<PlunderBubble>(),
                ProjectileType<StandBullet>(),
                ProjectileType<StarFinger>(),
                ProjectileType<StickyFingersFistExtended>(),
                ProjectileType<TinyBubble>(),
                ProjectileType<TrackerBubble>(),
                ProjectileType<Projectiles.PlayerStands.Cream.Void>(),
                ProjectileType<DashVoid>(),
            };

            standTier1List = new List<int>
            {
                ItemType<AerosmithT1>(),
                ItemType<GoldExperienceT1>(),
                ItemType<HierophantGreenT1>(),
                ItemType<KillerQueenT1>(),
                ItemType<KingCrimsonT1>(),
                ItemType<MagiciansRedT1>(),
                ItemType<SexPistolsT1>(),
                ItemType<StarPlatinumT1>(),
                ItemType<StickyFingersT1>(),
                ItemType<TheWorldT1>(),
                ItemType<TuskAct1>(),
                ItemType<LockT1>(),
                ItemType<GratefulDeadT1>(),
                ItemType<TheHandT1>(),
                ItemType<WhitesnakeT1>(),
                ItemType<DollyDaggerT1>(),
                ItemType<CenturyBoyT1>(),
                ItemType<SilverChariotT1>(),
                ItemType<HermitPurpleT1>(),
                ItemType<BadCompanyT1>(),
                ItemType<CreamT1>(),
                ItemType<StoneFreeT1>(),
                ItemType<CrazyDiamondT1>(),
                ItemType<TowerOfGrayT1>(),
                ItemType<SoftAndWetT1>(),
                ItemType<EchoesAct0>()
            };

            timestopImmune = new List<int>()
            {
                ProjectileType<TheWorldStandT2>(),     //only the timestop capable stands as people shouldn't switch anyway
                ProjectileType<TheWorldStandT3>(),
                ProjectileType<TheWorldStandFinal>(),
                ProjectileType<StarPlatinumStandFinal>(),
                ProjectileType<RoadRoller>(),
                ProjectileType<Fists>(),
                ProjectileType<HamonPunches>(),
                ProjectileType<GoldExperienceRequiemStand>(),
                ProjectileType<TuskAct4Stand>(),
                ProjectileType<TestStandStand>(),
                ProjectileType<StarOnTheTreeStand>(),
                ProjectileType<HighVelocityBubble>()
            };

            timestopOverrideStands = new List<int>
            {
                ItemType<TheWorldT2>(),
                ItemType<TheWorldT3>(),
                ItemType<TheWorldFinal>(),
                ItemType<StarPlatinumFinal>(),
                ItemType<GoldExperienceRequiem>(),
                ItemType<TuskAct4>(),
                ItemType<TestStand>(),
                ItemType<StarOnTheTree>()
            };

            christmasStands = new List<int>()
            {
                ItemType<StarOnTheTree>(),
                ItemType<KingClaus>()
            };

            testStandPassword.Add(Convert.ToChar(84));

            //Where we register the hotkeys
            SpecialHotKey = KeybindLoader.RegisterKeybind(Instance, "Special Ability", Keys.F);
            SecondSpecialHotKey = KeybindLoader.RegisterKeybind(Instance, "Second Special Ability", Keys.B);
            StandOutHotKey = KeybindLoader.RegisterKeybind(Instance, "Stand Out", Keys.G);
            PoseHotKey = KeybindLoader.RegisterKeybind(Instance, "Pose", Keys.V);
            StandAutoModeHotKey = KeybindLoader.RegisterKeybind(Instance, "Stand Auto Mode", Keys.C);

            if (!Main.dedServ)      //Manages resource loading cause the server isn't able to load resources
                JoJoStandsShaders.LoadShaders();
        }

        public override void Close()
        {
            if (timestopImmune != null)
                timestopImmune.Clear();
            if (timestopOverrideStands != null)
                timestopOverrideStands.Clear();
            if (standTier1List != null)
                standTier1List.Clear();
            if (standProjectileList != null)
                standProjectileList.Clear();
            if (christmasStands != null)
                christmasStands.Clear();
            if (testStandPassword != null)
                testStandPassword.Clear();

            base.Close();
        }

        public override void Unload()
        {
            SpecialHotKey = null;
            SecondSpecialHotKey = null;
            PoseHotKey = null;
            StandAutoModeHotKey = null;
            StandOutHotKey = null;
            standTier1List = null;
            timestopImmune = null;
            timestopOverrideStands = null;
            christmasStands = null;
            testStandPassword = null;
            SoundsLoaded = false;
            FanStandsLoaded = false;
            JoJoStandsSounds = null;
            ToBeContinued.TBCArrowTexture = null;
            StandItemClass.standTierNumerals = null;
            HamonBar.hamonBarTexture = null;
            BulletCounter.bulletCounterTexture = null;
            AerosmithRadar.aerosmithRadarBorderTexture = null;
            GoldenSpinMeter.goldenRectangleTexture = null;
            GoldenSpinMeter.goldenRectangleSpinLineTexture = null;
            SexPistolsUI.sexPistolsUITexture = null;
            VoidBar.VoidBarTexture = null;
            VoidBar.VoidBarBarTexture = null;
            AerosmithStandFinal.AerosmithWhirrSoundEffect = null;
            Instance = null;
        }

        public override void AddRecipeGroups()/* tModPorter Note: Removed. Use ModSystem.AddRecipeGroups */
        {
            RecipeGroup willsGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Wills", new int[]
            {
                ItemType<WillToFight>(),
                ItemType<WillToChange>(),
                ItemType<WillToControl>(),
                ItemType<WillToDestroy>(),
                ItemType<WillToEscape>(),
                ItemType<WillToProtect>()
            });
            RecipeGroup.RegisterGroup("JoJoStandsWills", willsGroup);

            RecipeGroup AdamantiteTierGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Adamantite-Tier Bar", new int[]
            {
                ItemID.AdamantiteBar,
                ItemID.TitaniumBar,
            });
            RecipeGroup.RegisterGroup("JoJoStandsAdamantite-TierBar", AdamantiteTierGroup);

            RecipeGroup MythrilTierGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Mythril-Tier Bar", new int[]
            {
                ItemID.MythrilBar,
                ItemID.OrichalcumBar,
            });
            RecipeGroup.RegisterGroup("JoJoStandsMythril-TierBar", MythrilTierGroup);

            RecipeGroup CobaltTierGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Cobalt-Tier Bar", new int[]
            {
                ItemID.CobaltBar,
                ItemID.PalladiumBar,
            });
            RecipeGroup.RegisterGroup("JoJoStandsCobalt-TierBar", CobaltTierGroup);

            RecipeGroup GoldTierGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gold-Tier Bar", new int[]
            {
                ItemID.GoldBar,
                ItemID.PlatinumBar,
            });
            RecipeGroup.RegisterGroup("JoJoStandsGold-TierBar", GoldTierGroup);

            RecipeGroup SilverTierGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Silver-Tier Bar", new int[]
            {
                ItemID.SilverBar,
                ItemID.TungstenBar,
            });
            RecipeGroup.RegisterGroup("JoJoStandsSilver-TierBar", SilverTierGroup);

            RecipeGroup IronTierGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Iron-Tier Bar", new int[]
            {
                ItemID.IronBar,
                ItemID.LeadBar,
            });
            RecipeGroup.RegisterGroup("JoJoStandsIron-TierBar", IronTierGroup);

            RecipeGroup EvilBarGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Evil Bar", new int[]
            {
                ItemID.DemoniteBar,
                ItemID.CrimtaneBar,
            });
            RecipeGroup.RegisterGroup("JoJoStandsEvilBar", EvilBarGroup);

            RecipeGroup RottenVertebraeGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Rotten Chunk or Vertebrae", new int[]
            {
                ItemID.RottenChunk,
                ItemID.Vertebrae,
            });
            RecipeGroup.RegisterGroup("JoJoStandsRottenVertebrae", RottenVertebraeGroup);

            RecipeGroup ShadowTissueGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Shadow Scale or Tissue Sample", new int[]
            {
                ItemID.ShadowScale,
                ItemID.TissueSample,
            });
            RecipeGroup.RegisterGroup("JoJoStandsShadowTissue", ShadowTissueGroup);

            RecipeGroup CursedIchorGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Cursed Flame or Ichor", new int[]
            {
                ItemID.CursedFlame,
                ItemID.Ichor,
            });
            RecipeGroup.RegisterGroup("JoJoStandsCursedIchor", CursedIchorGroup);

            RecipeGroup EvilBulletGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Cursed Bullet or Ichor Bullet", new int[]
            {
                ItemID.CursedBullet,
                ItemID.IchorBullet,
            });
            RecipeGroup.RegisterGroup("JoJoStandsEvilBullet", EvilBulletGroup);

            RecipeGroup SilverBulletGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Silver Bullet or Tungsten Bullet", new int[]
            {
                ItemID.SilverBullet,
                ItemID.TungstenBullet,
            });
            RecipeGroup.RegisterGroup("JoJoStandsSilverBullet", SilverBulletGroup);

            RecipeGroup CrownGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Crown", new int[]
            {
                ItemID.GoldCrown,
                ItemID.PlatinumCrown,
            });
            RecipeGroup.RegisterGroup("JoJoStandsCrown", CrownGroup);

            RecipeGroup WatchGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gold Watch or Platinum Watch", new int[]
            {
                ItemID.GoldWatch,
                ItemID.PlatinumWatch,
            });
            RecipeGroup.RegisterGroup("JoJoStandsWatch", WatchGroup);
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            ModNetHandler.HandlePacket(reader, whoAmI);
        }
    }
}