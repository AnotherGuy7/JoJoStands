using JoJoStands.Items.Hamon;
using JoJoStands.UI;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;
using static JoJoStands.JoJoStands;
using static JoJoStands.MyPlayer;

namespace JoJoStands
{
    public class CustomizableOptions : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Death Sound Options")]
        [Tooltip("Allows you to choose which deathsound to use")]
        public DeathSoundType deathSound;

        [DefaultValue(true)]
        [Tooltip("Determines whether or not you want to hear custom sounds or vanilla sounds.")]
        public bool Sounds;

        [DefaultValue(true)]
        [Label("Automatic Usages")]
        [Tooltip("Determines whether or not you want abilities to automatically activate, such as Killer Queen's bomb and The World's (Auto Mode) Knives")]
        public bool AutomaticActivations;

        [DefaultValue(true)]
        [Label("Respawn With Stand Out")]
        [Tooltip("Determiens whether or not you want to always respawn with your Stand out.")]
        public bool RespawnWithStandOut;

        [DefaultValue(true)]
        [Label("Ability Wheel Ability Tooltips")]
        [Tooltip("Whether or not you want ability descriptions to pop up whenever you hover over an ability in the Ability Wheel.")]
        public bool AbilityWheelDescriptions;

        [DefaultValue(true)]
        [Label("Hamon Effects")]
        [Tooltip("Determines whether or not you want to see Hamon effects.")]
        public bool HamonEffects;

        [DefaultValue(false)]
        [Label("Range Indicators")]
        [Tooltip("Determines whether or not you want to see the range indicators for stands that need them.")]
        public bool RangeIndicators;

        [DefaultValue(true)]
        [Label("Timestop Effects")]
        [Tooltip("Determines whether or not you want to see the timestop effects.")]
        public bool TimestopEffects;

        [DefaultValue(true)]
        [Label("Timeskip Effects")]
        [Tooltip("Determines whether or not you want to see the timeskip effects.")]
        public bool TimeskipEffects;

        [DefaultValue(true)]
        [Label("Bite the Dust Effects")]
        [Tooltip("Determines whether or not you want to see the bite the dust effects.")]
        public bool BitetheDustEffects;

        [DefaultValue(true)]
        [Label("Color Change Effects")]
        [Tooltip("Determiens whether or not you want to see the world color changes.")]
        public bool ColorChangeEffects;

        [DefaultValue(false)]
        [Label("Hidden References")]
        [Tooltip("Whether or not you want to see hidden references. (Some of these references can cause you to die!)")]
        public bool SecretReferences;

        [DefaultValue(40)]
        [Label("Range Indicator Visibility")]
        [Tooltip("Allows you to choose how transparent the Range Indicator is.")]
        public int RangeIndicatorVisibility;

        [DefaultValue(39)]
        [Label("Stand Slot X Position")]
        public int StandSlotPositionX;

        [DefaultValue(42)]
        [Label("Stand Slot Y Position")]
        public int StandSlotPositionY;

        [DefaultValue(1)]
        [Label("Hamon Bar Size")]
        public int HamonBarSize;

        [DefaultValue(90)]
        [Label("Hamon Bar X Position")]
        public int HamonBarPositionX;

        [DefaultValue(80)]
        [Label("Hamon Bar Y Position")]
        public int HamonBarPositionY;

        [DefaultValue(90)]
        [Label("Ability Wheel X Position")]
        public int AbilityWheelXPos;

        [DefaultValue(50)]
        [Label("Ability Wheel Y Position")]
        public int AbilityWheelYPos;

        [DefaultValue(40)]
        [Label("Sound Volume")]
        [Tooltip("Volume of barrage sounds")]
        public int soundVolume;

        [DefaultValue(StandSearchTypeEnum.Bosses)]
        [Label("Stand Auto Mode Targetting Preference")]
        [Tooltip("Select the type of enemy your Stand should prioritize first when in Auto Mode!")]
        public StandSearchTypeEnum StandSearchTypeEnum;

        [DefaultValue(false)]
        [Label("Sounds Mod Ability Voicelines")]
        [Tooltip("Whether or not you want to hear ability voicelines. These voicelines cause a delay in the activation of abilities! (Only works with the Sounds Mod!)")]
        public bool abilityVoicelines;

        public override void OnChanged()        //couldn't use Player player = Main.LocalPlayer cause it wasn't set to an instance of an object
        {
            JoJoStands.RangeIndicatorAlpha = (float)RangeIndicatorVisibility / 100f;
            JoJoStands.DeathSoundID = deathSound;
            JoJoStands.Sounds = Sounds;
            JoJoStands.TimestopEffects = TimestopEffects;
            JoJoStands.RangeIndicators = RangeIndicators;
            JoJoStands.AutomaticActivations = AutomaticActivations;
            JoJoStands.StandSlotPositionX = StandSlotPositionX;
            JoJoStands.StandSlotPositionY = StandSlotPositionY;
            JoJoStands.HamonBarPositionX = HamonBarPositionX;
            JoJoStands.HamonBarPositionY = HamonBarPositionY;
            JoJoStands.SecretReferences = SecretReferences;
            JoJoStands.ModSoundsVolume = soundVolume / 100f;
            HamonPlayer.HamonEffects = HamonEffects;
            HamonBar.changedInConfig = true;
            HamonBar.sizeMode = HamonBarSize;
            JoJoStands.ColorChangeEffects = ColorChangeEffects;
            JoJoStands.StandSearchTypeEnum = StandSearchTypeEnum;
            JoJoStands.TimeskipEffects = TimeskipEffects;
            JoJoStands.BiteTheDustEffects = BitetheDustEffects;
            JoJoStands.RespawnWithStandOut = RespawnWithStandOut;
            JoJoStands.AbilityWheelDescriptions = AbilityWheelDescriptions;
            JoJoStands.SoundsModAbilityVoicelines = abilityVoicelines;
            AbilityWheel.VerticalAlignmentPercentage = AbilityWheelYPos / 100f;
            if (HamonBarSize >= 4)
            {
                Main.NewText("You can only choose numbers between 0-3!");
                HamonBarSize = 3;
            }
            if (HamonBarSize <= -1)
            {
                Main.NewText("You can only choose numbers between 0-3!");
                HamonBarSize = 0;
                HamonBar.visible = false;
            }
            if (HamonBarSize != 0)
                HamonBar.visible = true;
        }
    }
}