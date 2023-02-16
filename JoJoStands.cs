using JoJoStands.Items;
using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Items.Seasonal;
using JoJoStands.Networking;
using JoJoStands.Projectiles;
using JoJoStands.Projectiles.PlayerStands.Aerosmith;
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

        public static List<int> timestopImmune = new List<int>();
        public static List<int> timestopOverrideStands = new List<int>();
        public static List<int> standTier1List = new List<int>();
        public static List<int> standProjectileList = new List<int>();
        public static List<int> christmasStands = new List<int>();
        public static List<char> testStandPassword = new List<char>();      //  :) Have *fuuuuuuun!*

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

            AerosmithStandFinal.AerosmithWhirrSoundEffect = (SoundEffect)Request<SoundEffect>("JoJoStands/Sounds/GameSounds/AerosmithWhirr", AssetRequestMode.ImmediateLoad);

            standProjectileList.Add(ProjectileType<BombBubble>());
            standProjectileList.Add(ProjectileType<ControllableNail>());
            standProjectileList.Add(ProjectileType<CrossfireHurricaneAnkh>());
            standProjectileList.Add(ProjectileType<Emerald>());
            standProjectileList.Add(ProjectileType<ExplosiveSpin>());
            standProjectileList.Add(ProjectileType<FireAnkh>());
            standProjectileList.Add(ProjectileType<Fists>());
            standProjectileList.Add(ProjectileType<HermitPurpleGrab>());
            standProjectileList.Add(ProjectileType<HermitPurpleWhip>());
            standProjectileList.Add(ProjectileType<HighVelocityBubble>());
            standProjectileList.Add(ProjectileType<KnifeProjectile>());
            standProjectileList.Add(ProjectileType<MeltYourHeart>());
            standProjectileList.Add(ProjectileType<MeltYourHeartDrip>());
            standProjectileList.Add(ProjectileType<Nail>());
            standProjectileList.Add(ProjectileType<NailSlasher>());
            standProjectileList.Add(ProjectileType<PlunderBubble>());
            standProjectileList.Add(ProjectileType<StandBullet>());
            standProjectileList.Add(ProjectileType<StarFinger>());
            standProjectileList.Add(ProjectileType<TinyBubble>());
            standProjectileList.Add(ProjectileType<Projectiles.PlayerStands.Cream.Void>());
            standProjectileList.Add(ProjectileType<Projectiles.PlayerStands.Cream.DashVoid>());

            standTier1List.Add(ItemType<AerosmithT1>());
            standTier1List.Add(ItemType<GoldExperienceT1>());
            standTier1List.Add(ItemType<HierophantGreenT1>());
            standTier1List.Add(ItemType<KillerQueenT1>());
            standTier1List.Add(ItemType<KingCrimsonT1>());
            standTier1List.Add(ItemType<MagiciansRedT1>());
            standTier1List.Add(ItemType<SexPistolsT1>());
            standTier1List.Add(ItemType<StarPlatinumT1>());
            standTier1List.Add(ItemType<StickyFingersT1>());
            standTier1List.Add(ItemType<TheWorldT1>());
            standTier1List.Add(ItemType<TuskAct1>());
            standTier1List.Add(ItemType<LockT1>());
            standTier1List.Add(ItemType<GratefulDeadT1>());
            standTier1List.Add(ItemType<TheHandT1>());
            standTier1List.Add(ItemType<WhitesnakeT1>());
            standTier1List.Add(ItemType<DollyDaggerT1>());
            standTier1List.Add(ItemType<CenturyBoyT1>());
            standTier1List.Add(ItemType<SilverChariotT1>());
            standTier1List.Add(ItemType<HermitPurpleT1>());
            standTier1List.Add(ItemType<BadCompanyT1>());
            standTier1List.Add(ItemType<CreamT1>());
            standTier1List.Add(ItemType<StoneFreeT1>());
            standTier1List.Add(ItemType<CrazyDiamondT1>());
            standTier1List.Add(ItemType<TowerOfGrayT1>());
            standTier1List.Add(ItemType<SoftAndWetT1>());
            standTier1List.Add(ItemType<EchoesAct0>());

            timestopImmune.Add(ProjectileType<TheWorldStandT2>());     //only the timestop capable stands as people shouldn't switch anyway
            timestopImmune.Add(ProjectileType<TheWorldStandT3>());
            timestopImmune.Add(ProjectileType<TheWorldStandFinal>());
            timestopImmune.Add(ProjectileType<StarPlatinumStandFinal>());
            //MyPlayer.stopImmune.Add(ProjectileType("StickyFingersFistExtended>());
            timestopImmune.Add(ProjectileType<RoadRoller>());
            timestopImmune.Add(ProjectileType<Fists>());
            timestopImmune.Add(ProjectileType<HamonPunches>());
            timestopImmune.Add(ProjectileType<GoldExperienceRequiemStand>());
            timestopImmune.Add(ProjectileType<TuskAct4Stand>());
            timestopImmune.Add(ProjectileType<TestStandStand>());
            timestopImmune.Add(ProjectileType<StarOnTheTreeStand>());

            timestopOverrideStands.Add(ItemType<TheWorldT2>());
            timestopOverrideStands.Add(ItemType<TheWorldT3>());
            timestopOverrideStands.Add(ItemType<TheWorldFinal>());
            timestopOverrideStands.Add(ItemType<StarPlatinumFinal>());
            timestopOverrideStands.Add(ItemType<GoldExperienceRequiem>());
            timestopOverrideStands.Add(ItemType<TuskAct4>());
            timestopOverrideStands.Add(ItemType<TestStand>());
            timestopOverrideStands.Add(ItemType<StarOnTheTree>());


            christmasStands.Add(ItemType<StarOnTheTree>());
            christmasStands.Add(ItemType<KingClaus>());

            testStandPassword.Add(Convert.ToChar(84));


            //Where we register the hotkeys
            SpecialHotKey = KeybindLoader.RegisterKeybind(Instance, "Special Ability", Keys.F);
            SecondSpecialHotKey = KeybindLoader.RegisterKeybind(Instance, "Second Special Ability", Keys.B);
            StandOutHotKey = KeybindLoader.RegisterKeybind(Instance, "Stand Out", Keys.G);
            PoseHotKey = KeybindLoader.RegisterKeybind(Instance, "Pose", Keys.V);
            StandAutoModeHotKey = KeybindLoader.RegisterKeybind(Instance, "Stand Auto Mode", Keys.C);


            if (!Main.dedServ)      //Manages resource loading cause the server isn't able to load resources
            {
                JoJoStandsShaders.LoadShaders();
            }
        }

        public override void Close()
        {
            standTier1List.Clear();
            timestopImmune.Clear();
            timestopOverrideStands.Clear();
            christmasStands.Clear();
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
            Instance = null;
        }

        public override void AddRecipeGroups()
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