using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;

namespace JoJoStands.UI
{
    public class GoldExperienceAbilityWheel : AbilityWheel
    {
        public static bool Visible;
        public static GoldExperienceAbilityWheel goldExperienceAbilityWheel;

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
            "Tree_Resprite",
            "Butterfly",
            "LimbRecreation"
        };


        public override string[] abilityDescriptions => new string[AmountOfAbilities]
        {
            "Creates a damage reflecting frog. Enemies who come in contact with the frog have the damage reflected back to them.",
            "Creates a damage reflecting tree. Enemies that come in contact with the tree are damaged and projectiles are reflected.",
            "Creates a butterfly. Enemies who kill this butterfly have their drop chances doubled.",
            "Recreates lost limbs. Hold right-click while standing still to heal health.",
        };

        public override void ExtraInitialize()
        {
            goldExperienceAbilityWheel = this;
        }

        public static void OpenAbilityWheel(MyPlayer modPlayer, int amountOfAbilities)
        {
            Visible = true;
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
            wheel.wheelCenterPosition = new Vector2(Main.screenWidth * wheel.abilityWheel.HAlign, Main.screenHeight * wheel.abilityWheel.VAlign);
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