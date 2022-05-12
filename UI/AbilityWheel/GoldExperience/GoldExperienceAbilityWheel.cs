using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;

namespace JoJoStands.UI
{
    public class GoldExperienceAbilityWheel : AbilityWheel
    {
        public static bool visible;
        private static GoldExperienceAbilityWheel goldExperienceAbilityWheel;

        private const int AmountOfAbilities = 4;
        public override int amountOfAbilities => AmountOfAbilities;
        public override string buttonTexturePath => "JoJoStands/UI/AbilityWheel/GoldExperience/";
        public override string centerTexturePath => "JoJoStands/Items/GoldExperienceFinal";
        public override string[] abilityNames => new string[AmountOfAbilities]
        {
            "           Frog",
            "           Tree",
            "      Butterfly",
            "Limb Recreation"
        };

        public override string[] abilityTextureNames => new string[AmountOfAbilities]
{
            "Frog",
            "Tree",
            "Butterfly",
            "LimbRecreation"
        };


        public override string[] abilityDescriptions => new string[AmountOfAbilities]
        {
            "Allows Gold Experience to create a damage reflecting frog.",
            "Allows Gold Experience to create a damage reflecting tree.",
            "Allows Gold Experience to create a butterfly. This butterfly will double the drop chances of any enemy that kills it.",
            "Allows Gold Experience to recreate lost limbs. Hold right-click to use."
        };

        public override void ExtraInitialize()
        {
            goldExperienceAbilityWheel = this;
        }

        public static void OpenAbilityWheel(MyPlayer modPlayer, int amountOfAbilities)
        {
            visible = true;
            mPlayer = modPlayer;
            mPlayer.chosenAbility = 0;

            GoldExperienceAbilityWheel wheel = goldExperienceAbilityWheel;
            wheel.abilitiesShown = amountOfAbilities;
            for (int i = 0; i < amountOfAbilities; i++)
            {
                wheel.abilityButtons[i].SetButtonPosiiton(wheel.wheelCenter.buttonPosition + (wheel.IndexToRadianPosition(i, wheel.abilitiesShown, wheel.wheelRotation) * wheel.wheelSpace));
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