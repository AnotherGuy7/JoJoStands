using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;

namespace JoJoStands.UI
{
    public class StoneFreeAbilityWheel : AbilityWheel
    {
        public static bool Visible;
        public static StoneFreeAbilityWheel stoneFreeAbilityWheel;

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
            "[Changes Special]\nTurns Stone Free's arms into strings, allowing Stone Free's punches to travel farther but pack less of a punch.",
            "Ties strings onto two tiles to create a string trap. Enemies are hurt by the string and enemy projectiles are stopped by the string. The two tiles have to be clicked individually.",
            "[Changes Special]\nTie an enemy up. Certain enemies can move despite being tied up!",
            "Ties and drag enemies. The closer the enemy is to Stone Free, the greater the strangle damage they receive. Move the mouse toward you to pull. Pull efforts are increased with proximity.",
            "[Changes Special]\nCreates a condensed string shield. While in use, you take 20% less damage from projectiles and 7% less damage from enemies. Cannot use other string abilities while this ability is active."
        };

        public override void ExtraInitialize()
        {
            stoneFreeAbilityWheel = this;
        }

        public static void OpenAbilityWheel(MyPlayer modPlayer, int amountOfAbilities)
        {
            Visible = true;
            mPlayer = modPlayer;
            mPlayer.chosenAbility = 0;

            StoneFreeAbilityWheel wheel = stoneFreeAbilityWheel;
            wheel.abilitiesShown = amountOfAbilities;
            for (int i = 0; i < amountOfAbilities; i++)
            {
                wheel.abilityButtons[i].SetButtonPosiiton(wheel.wheelCenter.buttonPosition + (wheel.IndexToRadianPosition(i, wheel.abilitiesShown, wheel.wheelRotation) * wheel.wheelSpace));
                if (i >= wheel.abilitiesShown)
                    wheel.abilityButtons[i].invisible = true;
            }
            wheel.wheelCenterPosition = new Vector2(Main.screenWidth - (wheel.AbilityWheelSize.X / 2f), Main.screenHeight * VerticalAlignmentPercentage);
            wheel.abilityNameText.SetText(wheel.abilityNames[0]);
            wheel.abilityNameText.Left.Pixels = wheel.wheelCenterPosition.X + wheel.wheelCenter.buttonPosition.X;
            wheel.abilityNameText.Top.Pixels = wheel.wheelCenterPosition.Y + wheel.wheelCenter.buttonPosition.Y + 60f;
            wheel.abilityNameText.Left.Pixels += -FontAssets.MouseText.Value.MeasureString(wheel.abilityNames[0]).X * 2f * wheel.textScale;
            wheel.abilityNameText.Left.Pixels += 10f;
            Main.player[Main.myPlayer].GetModPlayer<MyPlayer>().hotbarLocked = true;
            if (!Main.player[Main.myPlayer].GetModPlayer<MyPlayer>().abilityWheelTipDisplayed)
            {
                Main.NewText("*To use the Ability Wheel, use the numbers 1-x OR hover over the ability wheel with your cursor and scroll up or down!*", Color.Gold);
                Main.player[Main.myPlayer].GetModPlayer<MyPlayer>().abilityWheelTipDisplayed = true;
            }
        }

        public static void CloseAbilityWheel()
        {
            Visible = false;
            Main.player[Main.myPlayer].GetModPlayer<MyPlayer>().hotbarLocked = false;
        }
    }
}