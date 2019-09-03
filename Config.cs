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

        [Label("Hamon Bar's Size")]
        public int HamonBarSize;

        [DefaultValue(true)]
        [Tooltip("Determines whether or not you want to hear custom sounds or vanilla sounds.")]
        public bool Sounds;

        [Label("Stand Control Style: Mouse")]
        [Tooltip("Changes the Stand Control Style to Mouse.")]
        public bool StandControlMouse;

        [Label("Stand Control Style: Binds")]
        [Tooltip("Changes the Stand Control Style to Binds.")]
        public bool StandControlBinds;

        [Label("Player Effects")]
        [Tooltip("Determines whether or not you want to see hamon effects, stand effects, etc.")]
        public bool PlayerEffects;

        public override void OnChanged()        //couldn't use Player player = Main.LocalPlayer cause it wasn't set to an instance of an object
        {
            MyPlayer.deathsoundint = deathsound;
            UI.HamonBarState.sizeMode = HamonBarSize;
            MyPlayer.Sounds = Sounds;
            MyPlayer.StandControlBinds = StandControlBinds;
            MyPlayer.StandControlMouse = StandControlMouse;
            MyPlayer.PlayerEffects = PlayerEffects;
            if (deathsound >= 6)
            {
                Main.NewText("There is are no deathsounds over 5!");
                deathsound = 0;
            }
            if (deathsound <= -1)
            {
                Main.NewText("There is are no deathsounds under 0!");
                deathsound = 0;
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
            if (StandControlBinds && StandControlMouse)
            {
                Main.NewText("You can only choose 1 control style!");
                StandControlMouse = false;
                StandControlBinds = true;
            }
            if (!StandControlBinds && !StandControlMouse)
            {
                StandControlMouse = false;
                StandControlBinds = true;
            }
            if (StandControlBinds)
            {
                StandControlMouse = false;
            }
            if (StandControlMouse)
            {
                StandControlBinds = false;
            }
        }
    }
}