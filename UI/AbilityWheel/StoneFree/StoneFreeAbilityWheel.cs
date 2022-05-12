using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;

namespace JoJoStands.UI
{
    public class StoneFreeAbilityWheel : AbilityWheel
    {
        public static bool visible;
        private static StoneFreeAbilityWheel stoneFreeAbilityWheel;

        private const int AmountOfAbilities = 5;
        public override int amountOfAbilities => AmountOfAbilities;
        public override string buttonTexturePath => "JoJoStands/UI/AbilityWheel/StoneFree/";
        public override string centerTexturePath => "JoJoStands/Items/StoneFreeT1";
        public override string[] abilityNames => new string[AmountOfAbilities]
        {
            "Extended Barrage",
            "    String Traps",
            "            Bind",
            "   Tied Together",
            "           Weave"
        };

        public override string[] abilityTextureNames => new string[AmountOfAbilities]
{
            "ExtendedBarrage",
            "StringTraps",
            "Bind",
            "TiedTogether",
            "Weave"
        };


        public override string[] abilityDescriptions => new string[AmountOfAbilities]
        {
            "Allows Stone Free to turn its arms into strings, letting its punched travel farther but hit slightly weaker.",
            "Allows Stone Free to tie strings onto two tiles to create a string trap. Enemies are hurt by the string and enemy projectiles are stopped by the string.",
            "Allows Stone Free to tie an enemy up. Certain enemies can move despite being tied up.",
            "Allows Stone Free to tie and drag enemies. The closer the enemy is to Stone Free, the greater the strangle damage they receive. Pull efforts are increased with proximity.",
            "Allows Stone Free to create a condensed string shield."
        };

        public override void ExtraInitialize()
        {
            stoneFreeAbilityWheel = this;
        }

        public static void OpenAbilityWheel(MyPlayer modPlayer, int amountOfAbilities)
        {
            visible = true;
            mPlayer = modPlayer;
            mPlayer.chosenAbility = 0;

            StoneFreeAbilityWheel wheel = stoneFreeAbilityWheel;
            wheel.abilitiesShown = amountOfAbilities;
            for (int i = 0; i < AmountOfAbilities; i++)
            {
                wheel.abilityButtons[i].SetButtonPosiiton(wheel.wheelCenter.buttonPosition + (wheel.IndexToRadianPosition(i, AmountOfAbilities, wheel.wheelRotation) * wheel.wheelSpace));
                if (i > wheel.abilitiesShown - 1)
                    wheel.abilityButtons[i].invisible = true;
            }
            wheel.wheelAlignPosition = new Vector2(Main.screenWidth * wheel.abilityWheel.HAlign, Main.screenHeight * wheel.abilityWheel.VAlign);
            wheel.abilityNameText.SetText(wheel.abilityNames[0]);
            wheel.abilityNameText.Left.Pixels = wheel.wheelAlignPosition.X + wheel.wheelCenter.buttonPosition.X;
            wheel.abilityNameText.Top.Pixels = wheel.wheelAlignPosition.Y + wheel.wheelCenter.buttonPosition.Y + 60f;
            wheel.abilityNameText.Left.Pixels += -FontAssets.MouseText.Value.MeasureString(wheel.abilityNames[0]).X * 2f * wheel.textScale;
            wheel.abilityNameText.Left.Pixels += 10f;
        }

        public static void CloseAbilityWheel()
        {
            visible = false;
        }
    }
}