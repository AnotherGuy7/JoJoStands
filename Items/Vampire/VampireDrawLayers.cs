using JoJoStands.Buffs.ItemBuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vampire
{
    public class VampireDrawLayers : PlayerDrawLayer
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
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Extras/ProtectiveFilmLayer");
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
