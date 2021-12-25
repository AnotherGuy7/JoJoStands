using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace JoJoStands.UI
{
    internal class AbilityWheel : UIState
    {
        public UIPanel abilityWheel;
        public static bool visible;
        public static MyPlayer mPlayer;

        public override void OnInitialize()
        {
            abilityWheel = new UIPanel();
            abilityWheel.HAlign = 1f;
            abilityWheel.VAlign = 0.5f;
            abilityWheel.Width.Set(180f, 0f);
            abilityWheel.Height.Set(180f, 0f);



            Append(abilityWheel);
        }

        /*public override void Update(GameTime gameTime)
        {
            mPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();


            base.Update(gameTime);
        }*/

        public Vector2 IndexToRadianPosition(int index, int total)
        {
            float angle = (360f / (float)total) * index;
            angle = MathHelper.ToRadians(angle);

            return angle.ToRotationVector2();
        }

        public Vector2 IndexToRadianPosition(int index, int total, float angleAdd)
        {
            float angle = (360f / (float)total) * index;
            angle += angleAdd;
            angle = MathHelper.ToRadians(angle);

            return angle.ToRotationVector2();
        }
    }
}