using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;

namespace JoJoStands.UI
{
    public class GoldExperienceRequiemAbilityWheel : AbilityWheel
    {
        public static bool visible;
        public static GoldExperienceRequiemAbilityWheel goldExperienceRequiemAbilityWheel;

        private const int AmountOfAbilities = 5;
        public override int amountOfAbilities => AmountOfAbilities;
        public override string buttonTexturePath => "JoJoStands/UI/AbilityWheel/GoldExperienceRequiem/";
        public override string centerTexturePath => "JoJoStands/Items/GoldExperienceRequiem";
        public override string[] abilityNames => new string[AmountOfAbilities]
        {
            "       Scorpion",
            "           Tree",
            "     Death Loop",
            "Limb Recreation",
            "   Back to Zero"
        };

        public override string[] abilityTextureNames => new string[AmountOfAbilities]
{
            "Scorpion",
            "Tree",
            "DeathLoop",
            "LimbRecreation",
            "BtZ"
        };


        public override string[] abilityDescriptions => new string[AmountOfAbilities]
        {
            "Allows Gold Experience Requiem to shoot a beam. The beam spawns a damage-reflecting scorpion.",
            "Allows Gold Experience Requiem to create a damage reflecting tree.",
            "Allows Gold Experience Requiem to have the last thing it kills enter a death loop. Enemies are looped 10x and bosses are looped 3x.",
            "Allows Gold Experience Requiem to recreate lost limbs. Hold right-click to use.",
            "Allows Gold Experience Requiem to nullify all actions done to it within a certain time frame."
        };

        public override void ExtraInitialize()
        {
            goldExperienceRequiemAbilityWheel = this;
        }

        public static void OpenAbilityWheel(MyPlayer modPlayer, int amountOfAbilities)
        {
            visible = true;
            mPlayer = modPlayer;
            mPlayer.chosenAbility = 0;

            GoldExperienceRequiemAbilityWheel wheel = goldExperienceRequiemAbilityWheel;
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