using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class ModifiedPurpleRevolver : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/PurpleRevolver"; }
        }

        public int reloadCounter = 0;
        public int reloadStart = 0;
        public int soundCounter = 0;
        public bool canAltUse = true;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Modified Revolver");
			Tooltip.SetDefault("A revolver packing more of a punch than before. Press the special key to reload.");
		}

		public override void SetDefaults()
		{
            item.damage = 61;
            item.width = 30;
            item.height = 30;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = 5;
            item.knockBack = 3f;
            item.value = Item.buyPrice(0, 1, 50, 0);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item41;
            item.autoReuse = false;
            item.ranged = true;
            item.shoot = 10;
            item.useAmmo = AmmoID.Bullet;
            item.maxStack = 1;
            item.shootSpeed = 16f;
        }

        public override void HoldItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (reloadCounter > 0)
            {
                reloadCounter--;
            }
            if (mPlayer.revolverBulletsShot >= 6)       //do you really need this line?
            {
                reloadStart++;
            }
            if (mPlayer.revolverBulletsShot != 6)
            {
                reloadStart = 0;
            }
            if (reloadStart == 1)
            {
                if (MyPlayer.Sounds)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Reload60"));
                }
                reloadCounter = 60;
            }
            if (JoJoStands.SpecialHotKey.JustPressed && reloadCounter <= 1 && player.whoAmI == Main.myPlayer)
            {
                if (MyPlayer.Sounds)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Reload30"));
                }
                reloadCounter = 30;
            }
            if (reloadCounter == 1)
            {
                mPlayer.revolverBulletsShot = 0;
                canAltUse = true;
            }
            if (player.altFunctionUse == 2 && reloadCounter == 0)
            {
                soundCounter++;
                if (soundCounter > 5)
                {
                    Main.PlaySound(SoundID.Item41, player.Center);
                    mPlayer.revolverBulletsShot += 1;
                    soundCounter = 0;
                }
            }
            UI.BulletCounter.Visible = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return canAltUse;
        }

        public override bool CanUseItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (reloadCounter >= 1 || mPlayer.revolverBulletsShot >= 6)
            {
                return false;
            }
            if (mPlayer.revolverBulletsShot < 6)
            {
                mPlayer.revolverBulletsShot += 1;
            }
            if (player.altFunctionUse == 2 && reloadCounter == 0)
            {
                item.useTime = 5;
                item.useAnimation = 30;
            }
            if (player.altFunctionUse == 0)
            {
                item.useTime = 15;
                item.useAnimation = 15;
                canAltUse = false;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("PurpleRevolver"));
            recipe.AddIngredient(ItemID.IllegalGunParts, 6);
            recipe.AddIngredient(ItemID.ShroomiteBar, 7);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}