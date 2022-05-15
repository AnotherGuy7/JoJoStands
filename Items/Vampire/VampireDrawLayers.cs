using JoJoStands.Buffs.ItemBuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vampire
{
    public class ProtectiveFilmLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.FrontAccFront);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            if (drawPlayer.active && drawPlayer.HasBuff(ModContent.BuffType<ProtectiveFilmBuff>()))
            {
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Extras/ProtectiveFilmLayer");
                int drawX = (int)drawInfo.Position.X;
                int drawY = (int)(drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2f - 1f);
                Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                    effects = SpriteEffects.FlipHorizontally;

                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                Color color = Lighting.GetColor((int)((drawInfo.Position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.Position.Y + drawPlayer.height / 2f) / 16f));
                DrawData drawData = new DrawData(texture, position, drawPlayer.bodyFrame, color * alpha, drawPlayer.fullRotation, drawPlayer.Size / 2f, 1f, effects, 0);

                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }

    public class BlackUmbrellaLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Head);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.blackUmbrellaEquipped)
            {
                Texture2D texture = ModContent.Request<Texture2D>("JoJoStands/Extras/UmbrellaHat").Value;
                int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.Position.Y - Main.screenPosition.Y) - 1;
                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                    effects = SpriteEffects.FlipHorizontally;

                Vector2 offset = new Vector2(0f, 0f);
                Vector2 pos = new Vector2(drawX, drawY) + offset;

                DrawData drawData = new DrawData(texture, pos, null, Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, effects, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }

    public class KnivesLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.FrontAccFront);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            VampirePlayer vampirePlayer = drawPlayer.GetModPlayer<VampirePlayer>();
            if (drawPlayer.active && drawPlayer.HasBuff(ModContent.BuffType<KnifeAmalgamation>()))
            {
                Texture2D texture = ModContent.Request<Texture2D>("JoJoStands/Extras/KnivesLayer").Value;
                int drawX = (int)drawInfo.Position.X;
                int drawY = (int)(drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2f - 1f);
                Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                    effects = SpriteEffects.FlipHorizontally;

                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                Color color = Lighting.GetColor((int)((drawInfo.Position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.Position.Y + drawPlayer.height / 2f) / 16f));
                DrawData drawData = new DrawData(texture, position, drawPlayer.bodyFrame, color * alpha, drawPlayer.fullRotation, drawPlayer.Size / 2f, 1f, effects, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }
}
