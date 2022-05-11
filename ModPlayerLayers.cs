using System;
using System.Collections.Generic;

namespace JoJoStands
{
    public class ModPlayerLayers
    {
        public static readonly PlayerLayer MenacingPose = new PlayerLayer("JoJoStands", "Menacing Pose", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands>());



            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>());
            if (drawPlayer.active && drawPlayer.velocity == Vector2.Zero && mPlayer.poseMode)
            {
                Texture2D texture = Mod.Assets.Request<Texture2D>("Extras/Menacing>().Value);



                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y);
                if (drawPlayer.direction == -1)
                {
                    drawX += 2;
                }

                mPlayer.poseFrameCounter++;
                if (mPlayer.poseFrameCounter >= 7)
                {
                    mPlayer.menacingFrames += 1;
                    mPlayer.poseFrameCounter = 0;
                    if (mPlayer.menacingFrames >= 6)
                    {
                        mPlayer.menacingFrames = 0;
                    }
                }

                int frameHeight = texture.Height / 6;
                Vector2 drawPos = new Vector2(drawX, drawY);
                Rectangle poseAnimRect = new Rectangle(0, frameHeight * mPlayer.menacingFrames, texture.Width, frameHeight);
                Color drawColor = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
                Vector2 poseOrigin = new Vector2(texture.Width / 2f, frameHeight / 2f);
                DrawData data = new DrawData(texture, drawPos, poseAnimRect, drawColor, drawPlayer.fullRotation, poseOrigin, 1f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer AerosmithRadarCam = new PlayerLayer("JoJoStands", "Aerosmith Radar Cam", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands>());






            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>());
            if (drawPlayer.active && mPlayer.standRemoteMode && (mPlayer.StandSlot.Item.type == ModContent.ItemType<AerosmithT3>()) || mPlayer.StandSlot.Item.type == ModContent.ItemType<AerosmithFinal>())))
            {
    Texture2D texture = Mod.Assets.Request<Texture2D>("Extras/AerosmithRadar>().Value);

    int drawX = (int)(drawInfo.position.X + 4f + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
        int drawY = (int)(drawInfo.position.Y + 7f - Main.screenPosition.Y);
    if (drawPlayer.direction == -1)
    {
        drawX -= 2;
    }
    mPlayer.aerosmithRadarFrameCounter++;
    if (mPlayer.aerosmithRadarFrameCounter >= 30)
        mPlayer.aerosmithRadarFrameCounter = 0;
    int frame = mPlayer.aerosmithRadarFrameCounter / 16;
    int frameHeight = texture.Height / 2;

    Vector2 drawPos = new Vector2(drawX, drawY);
    Rectangle animRect = new Rectangle(0, frameHeight * frame, texture.Width, frameHeight);
    Color drawColor = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
    Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);
    DrawData data = new DrawData(texture, drawPos, animRect, drawColor, 0f, origin, 1f, SpriteEffects.None, 0);
    Main.playerDrawData.Add(data);
}
        });



public static readonly PlayerLayer KCArmLayer = new PlayerLayer("JoJoStands", "KCArm", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
{
    Player drawPlayer = drawInfo.drawPlayer;
    Mod Mod = ModLoader.GetMod("JoJoStands>());

    MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>());
    if (drawPlayer.active && mPlayer.wearingEpitaph)
    {
        Texture2D texture = Mod.Assets.Request<Texture2D>("Extras/KCArm>().Value);

        int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
        int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y);
        SpriteEffects effects = SpriteEffects.None;
        if (drawPlayer.direction == -1)
        {
            drawX += 2;
            effects = SpriteEffects.FlipHorizontally;
        }

        Vector2 drawPos = new Vector2(drawX, drawY);
        Rectangle animRect = new Rectangle(0, 0, texture.Width, texture.Height);
        Color drawColor = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
        Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
        DrawData data = new DrawData(texture, drawPos, animRect, drawColor, 0f, origin, 1f, effects, 0);
        Main.playerDrawData.Add(data);
    }
});

public static readonly PlayerLayer HermitPurpleArmsLayer = new PlayerLayer("JoJoStands", "HermitPurpleArmsLayer", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //gotten from ExampleMod's MyPlayer, but I understand what is happening
{
    Player drawPlayer = drawInfo.drawPlayer;
    Mod Mod = ModLoader.GetMod("JoJoStands>());

    MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>());
    SpriteEffects effects = SpriteEffects.None;
    if (drawPlayer.active && mPlayer.hermitPurpleTier != 0)
    {
        Texture2D texture = Mod.Assets.Request<Texture2D>("Extras/HermitPurple_Arms>().Value);

        int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
        int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y) - 4;
        if (drawPlayer.direction == -1)
        {
            drawX += 2;
            effects = SpriteEffects.FlipHorizontally;
        }
        if (drawPlayer.direction == 1)
        {
            effects = SpriteEffects.None;
        }
        Color color = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
        if (mPlayer.hermitPurpleHamonBurstLeft > 0)        //No increment here because it's already incrememnted in the Body layer
        {
            if (mPlayer.hermitPurpleSpecialFrameCounter >= 5)
            {
                color = Color.Yellow;
                if (mPlayer.hermitPurpleSpecialFrameCounter >= 10)
                {
                    mPlayer.hermitPurpleSpecialFrameCounter = 0;
                }
            }
        }
        DrawData data = new DrawData(texture, new Vector2(drawX, drawY), drawPlayer.bodyFrame, color, drawPlayer.bodyRotation, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
        Main.playerDrawData.Add(data);
    }
});

public static readonly PlayerLayer HermitPurpleBodyLayer = new PlayerLayer("JoJoStands", "HermitPurpleBodyLayer", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //gotten from ExampleMod's MyPlayer, but I understand what is happening
{
    Player drawPlayer = drawInfo.drawPlayer;
    Mod Mod = ModLoader.GetMod("JoJoStands>());

    MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>());
    SpriteEffects effects = SpriteEffects.None;
    if (drawPlayer.active && mPlayer.hermitPurpleTier > 1)
    {
        Texture2D texture = Mod.Assets.Request<Texture2D>("Extras/HermitPurple_Body>().Value);

        int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
        int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y) - 4;
        if (drawPlayer.direction == -1)
        {
            drawX += 2;
            effects = SpriteEffects.FlipHorizontally;
        }
        if (drawPlayer.direction == 1)
        {
            effects = SpriteEffects.None;
        }
        Color color = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
        if (mPlayer.hermitPurpleHamonBurstLeft > 0)
        {
            mPlayer.hermitPurpleSpecialFrameCounter++;
            if (mPlayer.hermitPurpleSpecialFrameCounter >= 5)
            {
                color = Color.Yellow;
                if (mPlayer.hermitPurpleSpecialFrameCounter >= 10)
                {
                    mPlayer.hermitPurpleSpecialFrameCounter = 0;
                }
            }
        }
        Vector2 drawPos = new Vector2(drawX, drawY);
        DrawData data = new DrawData(texture, drawPos, drawPlayer.bodyFrame, color, drawPlayer.bodyRotation, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
        Main.playerDrawData.Add(data);
    }
});

public static readonly PlayerLayer CenturyBoyActivated = new PlayerLayer("JoJoStands", "CenturyBoyActivated", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //gotten from ExampleMod's MyPlayer, but I understand what is happening
{
    Player drawPlayer = drawInfo.drawPlayer;
    Mod Mod = ModLoader.GetMod("JoJoStands>());

    MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>());
    SpriteEffects effects = SpriteEffects.None;
    if (drawPlayer.active && mPlayer.showingCBLayer)
    {
        Texture2D texture = Mod.Assets.Request<Texture2D>("Extras/CB>().Value);

        int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
        int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y);
        if (drawPlayer.direction == -1)
        {
            effects = SpriteEffects.FlipHorizontally;
        }
        if (drawPlayer.direction == 1)
        {
            effects = SpriteEffects.None;
        }
        if (mPlayer.StandDyeSlot.Item.dye != 0)
        {
            ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(mPlayer.StandDyeSlot.Item.type);
            shader.Apply(null);
        }
        Color drawColor = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
        DrawData data = new DrawData(texture, new Vector2(drawX, drawY - 9f), drawPlayer.bodyFrame, drawColor, drawPlayer.bodyRotation, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
        Main.playerDrawData.Add(data);
    }
});

public static readonly PlayerLayer SexPistolsLayer = new PlayerLayer("JoJoStands", "Sex Pistols Layer", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
{
    Player drawPlayer = drawInfo.drawPlayer;
    Mod Mod = ModLoader.GetMod("JoJoStands>());

    MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>());
    if (drawPlayer.active && mPlayer.sexPistolsLeft != 0 && mPlayer.standOut && mPlayer.sexPistolsTier != 0)
    {
        int frame = 6 - mPlayer.sexPistolsLeft;       //frames 0-5
        Texture2D texture = Mod.Assets.Request<Texture2D>("Extras/SexPistolsLayer>().Value);


int drawX = (int)(drawInfo.position.X + 4f + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
        int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y);
        if (drawPlayer.direction == -1)
        {
            drawX = drawX - 2;
        }
        int frameHeight = texture.Height / 6;

        Vector2 drawPos = new Vector2(drawX, drawY);
        Rectangle sexPistolsAnimRect = new Rectangle(0, frameHeight * frame, texture.Width, frameHeight);
        Color drawColor = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
        DrawData data = new DrawData(texture, drawPos, sexPistolsAnimRect, drawColor, drawPlayer.fullRotation, new Vector2(texture.Width / 2f, frameHeight / 2f), 1f, SpriteEffects.None, 0);
        Main.playerDrawData.Add(data);
    }
});

public static readonly PlayerLayer PhantomHoodLongGlowmask = new PlayerLayer("JoJoStands", "Phantom Hood Long Glowmask", PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
{
    Player drawPlayer = drawInfo.drawPlayer;
    Mod Mod = ModLoader.GetMod("JoJoStands>());

    MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>());
    if (drawPlayer.active && mPlayer.phantomHoodLongEquipped && drawPlayer.head == Mod.GetEquipSlot("PhantomHoodLong", EquipType.Head))
    {
        Texture2D texture = Mod.Assets.Request<Texture2D>("Extras/PhantomHoodLong_Head_Glowmask>().Value);

        float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
        int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
        int drawY = (int)(drawInfo.position.Y - Main.screenPosition.Y) - 1;
        SpriteEffects effects = SpriteEffects.None;
        if (drawPlayer.direction == -1)
        {
            effects = SpriteEffects.FlipHorizontally;
        }
        Vector2 offset = new Vector2(0f, 12f);
        Vector2 pos = new Vector2(drawX, drawY) + offset;

        DrawData data = new DrawData(texture, pos, drawPlayer.bodyFrame, Color.White, 0f, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
        Main.playerDrawData.Add(data);
    }
});

public static readonly PlayerLayer PhantomHoodNeutralGlowmask = new PlayerLayer("JoJoStands", "Phantom Hood Neutral Glowmask", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
{
    Player drawPlayer = drawInfo.drawPlayer;
    Mod Mod = ModLoader.GetMod("JoJoStands>());

    MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>());
    if (drawPlayer.active && mPlayer.phantomHoodNeutralEquipped && drawPlayer.head == Mod.GetEquipSlot("PhantomHoodNeutral", EquipType.Head))
    {
        Texture2D texture = Mod.Assets.Request<Texture2D>("Extras/PhantomHoodNeutral_Head_Glowmask>().Value);

        float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
        int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
        int drawY = (int)(drawInfo.position.Y - Main.screenPosition.Y) - 1;
        SpriteEffects effects = SpriteEffects.None;
        if (drawPlayer.direction == -1)
        {
            //drawX -= 2;
            effects = SpriteEffects.FlipHorizontally;
        }
        Vector2 offset = new Vector2(0f, 12f);
        Vector2 pos = new Vector2(drawX, drawY) + offset;

        DrawData data = new DrawData(texture, pos, drawPlayer.bodyFrame, Color.White, 0f, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
        Main.playerDrawData.Add(data);
    }
});

public static readonly PlayerLayer PhantomHoodShortGlowmask = new PlayerLayer("JoJoStands", "Phantom Hood Short Glowmask", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
{
    Player drawPlayer = drawInfo.drawPlayer;
    Mod Mod = ModLoader.GetMod("JoJoStands>());

    MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>());
    if (drawPlayer.active && mPlayer.phantomHoodShortEquipped && drawPlayer.head == Mod.GetEquipSlot("PhantomHoodShort", EquipType.Head))
    {
        Texture2D texture = Mod.Assets.Request<Texture2D>("Extras/PhantomHoodShort_Head_Glowmask>().Value);

        float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
        int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
        int drawY = (int)(drawInfo.position.Y - Main.screenPosition.Y) - 1;
        SpriteEffects effects = SpriteEffects.None;
        if (drawPlayer.direction == -1)
        {
            effects = SpriteEffects.FlipHorizontally;
        }
        Vector2 offset = new Vector2(0f, 12f);
        Vector2 pos = new Vector2(drawX, drawY) + offset;

        DrawData data = new DrawData(texture, pos, drawPlayer.bodyFrame, Color.White * alpha, 0f, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
        Main.playerDrawData.Add(data);
    }
});

public static readonly PlayerLayer PhantomChestplateGlowmask = new PlayerLayer("JoJoStands", "Phantom Chestplate Glowmask", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
{
    Player drawPlayer = drawInfo.drawPlayer;
    Mod Mod = ModLoader.GetMod("JoJoStands>());

    MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>());
    if (drawPlayer.active && mPlayer.phantomChestplateEquipped && drawPlayer.body == Mod.GetEquipSlot("PhantomChestplate", EquipType.Body))
    {
        Texture2D texture = Mod.Assets.Request<Texture2D>("Extras/PhantomChestplate_Body_Glowmask>().Value);

        float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
        float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;
        float drawY = (int)drawInfo.position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
        Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;

        DrawData data = new DrawData(texture, position, drawPlayer.bodyFrame, Color.White * alpha, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
        data.shader = drawInfo.bodyArmorShader;
        Main.playerDrawData.Add(data);
    }
});

public static readonly PlayerLayer PhantomArmsGlowmask = new PlayerLayer("JoJoStands", "Phantom Chestplate Arms Glowmask", PlayerLayer.Arms, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
{
    Player drawPlayer = drawInfo.drawPlayer;
    Mod Mod = ModLoader.GetMod("JoJoStands>());

    MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>());
    if (drawPlayer.active && mPlayer.phantomChestplateEquipped && drawPlayer.body == Mod.GetEquipSlot("PhantomChestplate", EquipType.Body))
    {
        Texture2D texture = Mod.Assets.Request<Texture2D>("Extras/PhantomChestplate_Arms_Glowmask>().Value);

        float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
        float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;
        float drawY = (int)drawInfo.position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
        Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;

        DrawData data = new DrawData(texture, position, drawPlayer.bodyFrame, Color.White * alpha, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
        data.shader = drawInfo.bodyArmorShader;
        Main.playerDrawData.Add(data);
    }
});

public static readonly PlayerLayer PhantomLeggingsGlowmask = new PlayerLayer("JoJoStands", "Phantom Leggings Glowmask", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
{
    Player drawPlayer = drawInfo.drawPlayer;
    Mod Mod = ModLoader.GetMod("JoJoStands>());

    MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>());
    if (drawPlayer.active && mPlayer.phantomLeggingsEquipped && drawPlayer.legs == Mod.GetEquipSlot("PhantomLeggings", EquipType.Legs))
    {
        Texture2D texture = Mod.Assets.Request<Texture2D>("Extras/PhantomLeggings_Legs_Glowmask>().Value);

        float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
        Vector2 offset = new Vector2(0f, 18f);
        float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;      //The reason we do this is cause position as a float moves the glowmask around too much
        float drawY = (int)drawInfo.position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2;
        Vector2 position = new Vector2(drawX, drawY) + drawPlayer.legPosition - Main.screenPosition + offset;

        DrawData data = new DrawData(texture, position, drawPlayer.legFrame, Color.White * alpha, drawPlayer.legRotation, drawInfo.legOrigin, 1f, drawInfo.spriteEffects, 0);
        data.shader = drawInfo.legArmorShader;
        Main.playerDrawData.Add(data);
    }
});

public static readonly PlayerLayer BlackUmbrellaLayer = new PlayerLayer("JoJoStands", "Black Umbrella Layer", PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
{
    Player drawPlayer = drawInfo.drawPlayer;
    Mod Mod = ModLoader.GetMod("JoJoStands>());

    MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>());
    if (drawPlayer.active && mPlayer.blackUmbrellaEquipped)
    {
        Texture2D texture = Mod.Assets.Request<Texture2D>("Extras/UmbrellaHat>().Value);

        float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
        int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
        int drawY = (int)(drawInfo.position.Y - Main.screenPosition.Y) - 1;
        SpriteEffects effects = SpriteEffects.None;
        if (drawPlayer.direction == -1)
        {
            effects = SpriteEffects.FlipHorizontally;
        }
        Vector2 offset = new Vector2(0f, 0f);
        Vector2 pos = new Vector2(drawX, drawY) + offset;

        DrawData data = new DrawData(texture, pos, null, Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, effects, 0);
        Main.playerDrawData.Add(data);
    }
});

public override void ModifyDrawLayers(List<PlayerLayer> layers)
{
    if (Player.dead || (Player.mount.Type != -1))
    {
        KCArmLayer.visible = false;
        MenacingPose.visible = false;
        AerosmithRadarCam.visible = false;
        CenturyBoyActivated.visible = false;
        SexPistolsLayer.visible = false;
        PhantomHoodLongGlowmask.visible = false;
        PhantomHoodNeutralGlowmask.visible = false;
        PhantomHoodShortGlowmask.visible = false;
        PhantomChestplateGlowmask.visible = false;
        PhantomArmsGlowmask.visible = false;
        PhantomLeggingsGlowmask.visible = false;
        BlackUmbrellaLayer.visible = false;
    }
    else
    {
        SexPistolsLayer.visible = true;
        KCArmLayer.visible = true;
        MenacingPose.visible = true;
        AerosmithRadarCam.visible = true;
        CenturyBoyActivated.visible = true;
        PhantomHoodLongGlowmask.visible = phantomHoodLongEquipped;
        PhantomHoodNeutralGlowmask.visible = phantomHoodNeutralEquipped;
        PhantomHoodShortGlowmask.visible = phantomHoodShortEquipped;
        PhantomChestplateGlowmask.visible = phantomChestplateEquipped;
        PhantomArmsGlowmask.visible = phantomChestplateEquipped;
        PhantomLeggingsGlowmask.visible = phantomLeggingsEquipped;
        BlackUmbrellaLayer.visible = blackUmbrellaEquipped;

        if (Player.ownedProjectileCounts[ModContent.ProjectileType<WormholeNail>())] != 0)
                {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].visible = false;
            }
        }
        if (Player.ownedProjectileCounts[ModContent.ProjectileType<Void>())] != 0 || Player.ownedProjectileCounts[ModContent.ProjectileType<ExposingCream>())] != 0)
                {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].visible = false;
            }
        }
    }

    int headLayer = layers.FindIndex(l => l == PlayerLayer.Head);       //Finding the head layer then as long as it exists we insert the armor over it
    if (headLayer > -1)
    {
        layers.Insert(headLayer + 1, PhantomHoodLongGlowmask);
        layers.Insert(headLayer + 1, PhantomHoodNeutralGlowmask);
        layers.Insert(headLayer + 1, PhantomHoodShortGlowmask);
        layers.Insert(headLayer + 1, BlackUmbrellaLayer);
    }
    int bodyLayer = layers.FindIndex(l => l == PlayerLayer.Body);
    if (bodyLayer > -1)
    {
        layers.Insert(bodyLayer + 1, PhantomChestplateGlowmask);
        int armsLayer = layers.FindIndex(l => l == PlayerLayer.Arms);
        if (armsLayer > -1)
        {
            layers.Insert(armsLayer + 1, PhantomArmsGlowmask);
        }
    }
    int legsLayer = layers.FindIndex(l => l == PlayerLayer.Legs);
    if (legsLayer > -1)
    {
        layers.Insert(legsLayer + 1, PhantomLeggingsGlowmask);
    }

    layers.Add(AerosmithRadarCam);
    layers.Add(KCArmLayer);
    layers.Add(MenacingPose);
    layers.Add(CenturyBoyActivated);
    layers.Add(SexPistolsLayer);
    layers.Add(HermitPurpleBodyLayer);
    layers.Add(HermitPurpleArmsLayer);
}
    }
}
