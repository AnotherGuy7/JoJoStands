using JoJoStands.Items.Hamon;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;

namespace JoJoStands
{
    public class CustomizableOptions : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Death Sound Options")]
        [Tooltip("Allows you to choose which deathsound to use. Use /deathsoundlist to see the available sounds!")]
        public int deathsound;

        [DefaultValue(1)]
        [Label("Hamon Bar's Size")]
        public int HamonBarSize;

        [DefaultValue(true)]
        [Tooltip("Determines whether or not you want to hear custom sounds or vanilla sounds.")]
        public bool Sounds;

        [DefaultValue(true)]
        [Label("Hamon Effects")]
        [Tooltip("Determines whether or not you want to see Hamon effects.")]
        public bool HamonEffects;

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

        [DefaultValue(false)]
        [Label("Range Indicators")]
        [Tooltip("Determines whether or not you want to see the range indicators for stands that need them.")]
        public bool RangeIndicators;

        [DefaultValue(true)]
        [Label("Automatic Usages")]
        [Tooltip("Determines whether or not you want abilities to automatically activate, such as Killer Queen's bomb and The World's (Auto Mode) Knives")]
        public bool AutomaticActivations;

        [DefaultValue(100)]
        [Label("Range Indicator Visibility")]
        [Tooltip("Allows you to choose how transparent the Range Indicator is.")]
        public int RangeIndicatorVisibility;

        [DefaultValue(MyPlayer.StandSearchType.Bosses)]
        [Label("Stand Auto Mode Targetting Preference")]
        [Tooltip("Select the type of enemy your Stand should prioritize first when in Auto Mode!")]
        public MyPlayer.StandSearchType StandSearchType;

        [DefaultValue(39)]
        [Label("Stand Slot X Position")]
        public int StandSlotPositionX;

        [DefaultValue(42)]
        [Label("Stand Slot Y Position")]
        public int StandSlotPositionY;

        [DefaultValue(90)]
        [Label("Hamon Bar X Position")]
        public int HamonBarPositionX;

        [DefaultValue(80)]
        [Label("Hamon Bar Y Position")]
        public int HamonBarPositionY;

        [DefaultValue(false)]
        [Label("Hidden References")]
        [Tooltip("Whether or not you want to see hidden references. (Some of these references can cause you to die!)")]
        public bool SecretReferences;

        [DefaultValue(0.4f)]
        [Label("Sound Volume")]
        [Tooltip("Volume of barrage sounds")]
        public float soundVolume;

        [DefaultValue(true)]
        [Label("Color Change Effects")]
        [Tooltip("Determiens whether or not you want to see the world color changes.")]
        public bool ColorChangeEffects;

        public override void OnChanged()        //couldn't use Player player = Main.LocalPlayer cause it wasn't set to an instance of an object
        {
            MyPlayer.RangeIndicatorAlpha = RangeIndicatorVisibility;
            MyPlayer.DeathSoundID = deathsound;
            MyPlayer.Sounds = Sounds;
            MyPlayer.TimestopEffects = TimestopEffects;
            MyPlayer.RangeIndicators = RangeIndicators;
            MyPlayer.AutomaticActivations = AutomaticActivations;
            MyPlayer.StandSlotPositionX = StandSlotPositionX;
            MyPlayer.StandSlotPositionY = StandSlotPositionY;
            MyPlayer.HamonBarPositionX = HamonBarPositionX;
            MyPlayer.HamonBarPositionY = HamonBarPositionY;
            MyPlayer.SecretReferences = SecretReferences;
            MyPlayer.ModSoundsVolume = soundVolume;
            HamonPlayer.HamonEffects = HamonEffects;
            UI.HamonBarState.changedInConfig = true;
            UI.HamonBarState.sizeMode = HamonBarSize;
            MyPlayer.ColorChangeEffects = ColorChangeEffects;
            MyPlayer.standSearchType = StandSearchType;
            MyPlayer.TimeskipEffects = TimeskipEffects;
            MyPlayer.BiteTheDustEffects = BitetheDustEffects;
            if (JoJoStands.JoJoStandsSounds == null)
            {
                if (deathsound >= 6)
                {
                    Main.NewText("There are no deathsounds over 5!");
                    deathsound = 0;
                }
                if (deathsound <= -1)
                {
                    Main.NewText("There are no deathsounds under 0!");
                    deathsound = 0;
                }
            }
            if (HamonBarSize >= 4)
            {
                Main.NewText("You can only choose numbers between 0-3!");
                HamonBarSize = 3;
            }
            if (HamonBarSize <= -1)
            {
                Main.NewText("You can only choose numbers between 0-3!");
                HamonBarSize = 0;
                UI.HamonBarState.Visible = false;
            }
            if (HamonBarSize != 0)
            {
                UI.HamonBarState.Visible = true;
            }
        }
    }
}