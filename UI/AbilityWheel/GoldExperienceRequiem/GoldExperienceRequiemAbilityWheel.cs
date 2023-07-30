using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;

namespace JoJoStands.UI
{
    public class GoldExperienceRequiemAbilityWheel : AbilityWheel
    {
        public static bool Visible;
        public static GoldExperienceRequiemAbilityWheel goldExperienceRequiemAbilityWheel;

        private const int AmountOfAbilities = 4;
        public override int amountOfAbilities => AmountOfAbilities;
        public override string buttonTexturePath => "JoJoStands/UI/AbilityWheel/GoldExperienceRequiem/";
        public override string centerTexturePath => "JoJoStands/Items/GoldExperienceRequiem";
        public override string[] abilityNames => new string[AmountOfAbilities]
        {
            "       Scorpion",
            "           Tree",
            "     Death Loop",
            "Limb Recreation",
        };

        public override string[] abilityTextureNames => new string[AmountOfAbilities]
{
            "Scorpion",
            "Tree",
            "DeathLoop",
            "LimbRecreation",
        };


        public override string[] abilityDescriptions => new string[AmountOfAbilities]
        {
            "Shoots a quick beam. Upon landing, a damage-reflecting scorpion is spawned.",
            "Creates a damage reflecting tree. Enemies that come in contact with the tree are damaged and projectiles are reflected.",
            "Allows you to target an enemy to Death Loop with right-click. Killing that enemy loops its death multiple times.",
            "Recreates lost limbs. Hold right-click while standing still to heal health.",
        };

        public override void ExtraInitialize()
        {
            goldExperienceRequiemAbilityWheel = this;
        }

        public static void OpenAbilityWheel(MyPlayer modPlayer, int amountOfAbilities)
        {
            Visible = true;
            mPlayer = modPlayer;
            mPlayer.chosenAbility = 0;

            GoldExperienceRequiemAbilityWheel wheel = goldExperienceRequiemAbilityWheel;
            wheel.abilitiesShown = amountOfAbilities;
            for (int i = 0; i < AmountOfAbilities; i++)
            {
                wheel.abilityButtons[i].SetButtonPosiiton(wheel.wheelCenter.buttonPosition + (wheel.IndexToRadianPosition(i, wheel.abilitiesShown, wheel.wheelRotation) * wheel.wheelSpace));
                if (i >= amountOfAbilities)
                    wheel.abilityButtons[i].invisible = true;
                else
                    wheel.abilityButtons[i].invisible = false;
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