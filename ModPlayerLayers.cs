﻿using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace JoJoStands
{
    public class PoseDrawLayer : PlayerDrawLayer
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

    public class MenacingPoseLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.FrontAccFront);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && drawPlayer.velocity == Vector2.Zero && mPlayer.poseMode)
            {
                Texture2D texture = ModContent.Request<Texture2D>("Extras/Menacing").Value;
                int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.Position.Y + 20f - Main.screenPosition.Y);
                if (drawPlayer.direction == -1)
                    drawX += 2;

                mPlayer.poseFrameCounter++;
                if (mPlayer.poseFrameCounter >= 7)
                {
                    mPlayer.menacingFrames += 1;
                    mPlayer.poseFrameCounter = 0;
                    if (mPlayer.menacingFrames >= 6)
                        mPlayer.menacingFrames = 0;
                }

                int frameHeight = texture.Height / 6;
                Vector2 drawPos = new Vector2(drawX, drawY);
                Rectangle poseAnimRect = new Rectangle(0, frameHeight * mPlayer.menacingFrames, texture.Width, frameHeight);
                Color drawColor = Lighting.GetColor((int)((drawInfo.Position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.Position.Y + drawPlayer.height / 2f) / 16f));
                Vector2 poseOrigin = new Vector2(texture.Width / 2f, frameHeight / 2f);

                DrawData drawData = new DrawData(texture, drawPos, poseAnimRect, drawColor, drawPlayer.fullRotation, poseOrigin, 1f, SpriteEffects.None, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }

    public class AerosmithRadarCameraLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.FaceAcc);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.standRemoteMode && (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<AerosmithT3>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<AerosmithFinal>()))
            {
                Texture2D texture = ModContent.Request<Texture2D>("Extras/AerosmithRadar").Value;
                int drawX = (int)(drawInfo.Position.X + 4f + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.Position.Y + 7f - Main.screenPosition.Y);
                if (drawPlayer.direction == -1)
                    drawX -= 2;

                mPlayer.aerosmithRadarFrameCounter++;
                if (mPlayer.aerosmithRadarFrameCounter >= 30)
                    mPlayer.aerosmithRadarFrameCounter = 0;

                int frame = mPlayer.aerosmithRadarFrameCounter / 16;
                int frameHeight = texture.Height / 2;

                Vector2 drawPos = new Vector2(drawX, drawY);
                Rectangle animRect = new Rectangle(0, frameHeight * frame, texture.Width, frameHeight);
                Color drawColor = Lighting.GetColor((int)((drawInfo.Position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.Position.Y + drawPlayer.height / 2f) / 16f));
                Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);

                DrawData drawData = new DrawData(texture, drawPos, animRect, drawColor, 0f, origin, 1f, SpriteEffects.None, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }

    public class KingCrimsonArmLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Torso);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.wearingEpitaph)
            {
                Texture2D texture = ModContent.Request<Texture2D>("Extras/KCArm").Value;
                int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.Position.Y + 20f - Main.screenPosition.Y);
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                {
                    drawX += 2;
                    effects = SpriteEffects.FlipHorizontally;
                }

                Vector2 drawPos = new Vector2(drawX, drawY);
                Rectangle animRect = new Rectangle(0, 0, texture.Width, texture.Height);
                Color drawColor = Lighting.GetColor((int)((drawInfo.Position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.Position.Y + drawPlayer.height / 2f) / 16f));
                Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

                DrawData drawData = new DrawData(texture, drawPos, animRect, drawColor, 0f, origin, 1f, effects, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }

    public class HermitPurpleArmsLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Torso);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.hermitPurpleTier != 0)
            {
                Texture2D texture = ModContent.Request<Texture2D>("Extras/HermitPurple_Arms").Value;
                int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.Position.Y + 20f - Main.screenPosition.Y) - 4;
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                {
                    drawX += 2;
                    effects = SpriteEffects.FlipHorizontally;
                }
                if (drawPlayer.direction == 1)
                    effects = SpriteEffects.None;

                Color color = Lighting.GetColor((int)((drawInfo.Position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.Position.Y + drawPlayer.height / 2f) / 16f));
                if (mPlayer.hermitPurpleHamonBurstLeft > 0)        //No increment here because it's already incrememnted in the Body layer
                {
                    if (mPlayer.hermitPurpleSpecialFrameCounter >= 5)
                    {
                        color = Color.Yellow;
                        if (mPlayer.hermitPurpleSpecialFrameCounter >= 10)
                            mPlayer.hermitPurpleSpecialFrameCounter = 0;
                    }
                }
                DrawData drawData = new DrawData(texture, new Vector2(drawX, drawY), drawPlayer.bodyFrame, color, drawPlayer.bodyRotation, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }

    public class HermitPurpleBodyLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Torso);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.hermitPurpleTier > 1)
            {
                Texture2D texture = ModContent.Request<Texture2D>("Extras/HermitPurple_Body").Value;
                int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.Position.Y + 20f - Main.screenPosition.Y) - 4;
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                {
                    drawX += 2;
                    effects = SpriteEffects.FlipHorizontally;
                }
                if (drawPlayer.direction == 1)
                    effects = SpriteEffects.None;

                Color color = Lighting.GetColor((int)((drawInfo.Position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.Position.Y + drawPlayer.height / 2f) / 16f));
                if (mPlayer.hermitPurpleHamonBurstLeft > 0)
                {
                    mPlayer.hermitPurpleSpecialFrameCounter++;
                    if (mPlayer.hermitPurpleSpecialFrameCounter >= 5)
                    {
                        color = Color.Yellow;
                        if (mPlayer.hermitPurpleSpecialFrameCounter >= 10)
                            mPlayer.hermitPurpleSpecialFrameCounter = 0;
                    }
                }
                Vector2 drawPos = new Vector2(drawX, drawY);
                DrawData drawData = new DrawData(texture, drawPos, drawPlayer.bodyFrame, color, drawPlayer.bodyRotation, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }

    public class CenturyBoyLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Torso);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.showingCBLayer)
            {
                Texture2D texture = ModContent.Request<Texture2D>("Extras/CB").Value;
                int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.Position.Y + 20f - Main.screenPosition.Y);
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == 1)
                    effects = SpriteEffects.None;
                else
                    effects = SpriteEffects.FlipHorizontally;
                if (mPlayer.StandDyeSlot.SlotItem.dye != 0)
                {
                    ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(mPlayer.StandDyeSlot.SlotItem.type);
                    shader.Apply(null);
                }
                Color drawColor = Lighting.GetColor((int)((drawInfo.Position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.Position.Y + drawPlayer.height / 2f) / 16f));

                DrawData drawData = new DrawData(texture, new Vector2(drawX, drawY - 9f), drawPlayer.bodyFrame, drawColor, drawPlayer.bodyRotation, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }

    public class SexPistolsLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.FrontAccFront);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.sexPistolsLeft != 0 && mPlayer.standOut && mPlayer.sexPistolsTier != 0)
            {
                int frame = 6 - mPlayer.sexPistolsLeft;       //frames 0-5
                Texture2D texture = ModContent.Request<Texture2D>("Extras/SexPistolsLayer").Value;
                int drawX = (int)(drawInfo.Position.X + 4f + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.Position.Y + 20f - Main.screenPosition.Y);
                if (drawPlayer.direction == -1)
                    drawX -= 2;
                int frameHeight = texture.Height / 6;

                Vector2 drawPos = new Vector2(drawX, drawY);
                Rectangle sexPistolsAnimRect = new Rectangle(0, frameHeight * frame, texture.Width, frameHeight);
                Color drawColor = Lighting.GetColor((int)((drawInfo.Position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.Position.Y + drawPlayer.height / 2f) / 16f));
                DrawData drawData = new DrawData(texture, drawPos, sexPistolsAnimRect, drawColor, drawPlayer.fullRotation, new Vector2(texture.Width / 2f, frameHeight / 2f), 1f, SpriteEffects.None, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }

    public class PhantomHoodLongGlowmaskLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Head);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.phantomHoodLongEquipped && drawPlayer.head == Mod.GetEquipSlot("PhantomHoodLong", EquipType.Head))
            {
                Texture2D texture = ModContent.Request<Texture2D>("Extras/PhantomHoodLong_Head_Glowmask").Value;
                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.Position.Y - Main.screenPosition.Y) - 1;
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                    effects = SpriteEffects.FlipHorizontally;

                Vector2 offset = new Vector2(0f, 12f);
                Vector2 pos = new Vector2(drawX, drawY) + offset;

                DrawData drawData = new DrawData(texture, pos, drawPlayer.bodyFrame, Color.White, 0f, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }

    public class PhantomHoodNeutralGlowmaskLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Head);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.phantomHoodNeutralEquipped && drawPlayer.head == Mod.GetEquipSlot("PhantomHoodNeutral", EquipType.Head))
            {
                Texture2D texture = ModContent.Request<Texture2D>("Extras/PhantomHoodNeutral_Head_Glowmask").Value;
                int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.Position.Y - Main.screenPosition.Y) - 1;
                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                    effects = SpriteEffects.FlipHorizontally;

                Vector2 offset = new Vector2(0f, 12f);
                Vector2 pos = new Vector2(drawX, drawY) + offset;

                DrawData drawData = new DrawData(texture, pos, drawPlayer.bodyFrame, Color.White, 0f, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }

    public class PhantomHoodShortGlowmaskLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Head);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.phantomHoodShortEquipped && drawPlayer.head == Mod.GetEquipSlot("PhantomHoodShort", EquipType.Head))
            {
                Texture2D texture = ModContent.Request<Texture2D>("Extras/PhantomHoodShort_Head_Glowmask").Value;
                int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.Position.Y - Main.screenPosition.Y) - 1;
                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                    effects = SpriteEffects.FlipHorizontally;

                Vector2 offset = new Vector2(0f, 12f);
                Vector2 pos = new Vector2(drawX, drawY) + offset;

                DrawData drawData = new DrawData(texture, pos, drawPlayer.bodyFrame, Color.White * alpha, 0f, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }

    public class PhantomChestplateGlowmaskLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Torso);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.phantomChestplateEquipped && drawPlayer.body == Mod.GetEquipSlot("PhantomChestplate", EquipType.Body))
            {
                Texture2D texture = ModContent.Request<Texture2D>("Extras/PhantomChestplate_Body_Glowmask").Value;
                float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
                float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;

                DrawData drawData = new DrawData(texture, position, drawPlayer.bodyFrame, Color.White * alpha, drawPlayer.bodyRotation, /*drawInfo.bodyOrigin*/ drawInfo.rotationOrigin, 1f, drawInfo.playerEffect, 0);
                //data.shader = drawInfo.bodyArmorShader;
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }

    public class PhantomArmsGlowmaskLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Torso);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.phantomChestplateEquipped && drawPlayer.body == Mod.GetEquipSlot("PhantomChestplate", EquipType.Body))
            {
                Texture2D texture = ModContent.Request<Texture2D>("Extras/PhantomChestplate_Arms_Glowmask").Value;
                float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
                float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
                Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;

                DrawData drawData = new DrawData(texture, position, drawPlayer.bodyFrame, Color.White * alpha, drawPlayer.bodyRotation, /*drawInfo.bodyOrigin*/ drawInfo.rotationOrigin, 1f, drawInfo.playerEffect, 0);
                //drawData.shader = drawInfo.bodyArmorShader;
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }

    public class PhantomLeggingsLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Leggings);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.phantomLeggingsEquipped && drawPlayer.legs == Mod.GetEquipSlot("PhantomLeggings", EquipType.Legs))
            {
                Texture2D texture = ModContent.Request<Texture2D>("Extras/PhantomLeggings_Legs_Glowmask").Value;
                Vector2 offset = new Vector2(0f, 18f);
                float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;      //The reason we do this is cause position as a float moves the glowmask around too much
                float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2;
                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                Vector2 position = new Vector2(drawX, drawY) + drawPlayer.legPosition - Main.screenPosition + offset;

                DrawData drawData = new DrawData(texture, position, drawPlayer.legFrame, Color.White * alpha, drawPlayer.legRotation, /*drawInfo.legOrigin*/ drawInfo.legsOffset, 1f, drawInfo.playerEffect, 0);
                //data.shader = drawInfo.legArmorShader;
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }
}