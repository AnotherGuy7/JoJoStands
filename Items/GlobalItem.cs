using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Items.Hamon;
using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class JoJoGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        private int generalPurposeTimer = 0;        //Use for anything
        private float normalGravity = 0f;
        private float normalFallSpeed = 0f;
        private bool gravitySaved = false;

        public readonly int[] CrazyDiamondUncraftRarities = new int[4] { ItemRarityID.Green, ItemRarityID.Orange, ItemRarityID.LightPurple, 99 };

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.sexPistolsTier != 0 && (item.shoot == 10 || item.useAmmo == AmmoID.Bullet))
                damage *= 1f + (0.05f * mPlayer.sexPistolsTier);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.sexPistolsTier != 0 && (item.shoot == 10 || item.useAmmo == AmmoID.Bullet) && mPlayer.sexPistolsLeft > 0 && mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                mPlayer.sexPistolsLeft -= 1;
                int projectileIndex = Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, type, damage, knockback, player.whoAmI);
                Main.projectile[projectileIndex].GetGlobalProjectile<JoJoGlobalProjectile>().autoModeSexPistols = true;
                return false;
            }
            return true;
        }

        public override bool AltFunctionUse(Item Item, Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut && mPlayer.standControlStyle != MyPlayer.StandControlStyle.Auto && !mPlayer.standAccessory)
                return false;

            return false;
        }

        public override bool CanUseItem(Item Item, Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut && mPlayer.standControlStyle != MyPlayer.StandControlStyle.Auto && mPlayer.sexPistolsTier == 0 && !mPlayer.standAccessory)
            {
                if (PlayerInput.Triggers.Current.SmartSelect)
                    return true;

                if (Item.potion || Item.mountType != MountID.None || (player.controlHook && !player.miscEquips[4].IsAir))      //default value for mountType is -1
                    return true;

                if (!player.controlMount && Item.mountType != MountID.None)        //if the player isn't pressing controlMount AND the Item's mount is actually something, 
                    return false;

                else if ((!player.controlQuickMana || !player.controlQuickHeal) && Item.potion)
                    return false;

                else
                    return false;
            }

            if (player.HasBuff(ModContent.BuffType<SkippingTime>()))
                return false;

            return true;
        }

        public override void Update(Item Item, ref float gravity, ref float maxFallSpeed)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.timestopActive && gravity != 3f)
            {
                normalGravity = gravity;
                normalFallSpeed = maxFallSpeed;
                gravitySaved = true;
            }
            if (gravitySaved)
            {
                gravity = 3f;
                maxFallSpeed = 0;
            }
            if (gravitySaved && !mPlayer.timestopActive)
            {
                gravity = normalGravity;
                maxFallSpeed = normalFallSpeed;
                gravitySaved = false;
            }
            if (!mPlayer.timestopActive && normalGravity != 0f && !gravitySaved)
            {
                normalFallSpeed = 0f;
                normalGravity = 0f;
            }
        }

        public override void HoldItem(Item Item, Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (generalPurposeTimer > 0)
                generalPurposeTimer--;
            if (Main.mouseRight && mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual && mPlayer.crazyDiamondRestorationMode && mPlayer.crazyDiamondDestroyedTileData.Count == 0 && generalPurposeTimer <= 0)
            {
                for (int i = 0; i < 54; i++)
                {
                    Item ItemSelect = player.inventory[i];
                    if (ItemSelect == Item)
                    {
                        int currentTier = (int)MathHelper.Clamp(mPlayer.standTier - 1, 0, 3);       //Just to prevent any odd crashes
                        mPlayer.ItemBreak(Item, CrazyDiamondUncraftRarities[currentTier]);
                        generalPurposeTimer += 30;
                        break;
                    }
                }
            }
        }

        public override bool ReforgePrice(Item item, ref int reforgePrice, ref bool canApplyDiscount)
        {
            Player player = Main.player[Main.myPlayer];
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.type == NPCID.GoblinTinkerer && npc.HasBuff(ModContent.BuffType<BelieveInMe>()) && npc.active && player.talkNPC == npc.whoAmI)
                    reforgePrice -= (int)(reforgePrice * 0.2f);
            }
            return base.ReforgePrice(item, ref reforgePrice, ref canApplyDiscount);
        }

        public override void AddRecipes()
        {
            Recipe revolverRecipe = Recipe.Create(ItemID.Revolver);
            revolverRecipe.AddIngredient(ModContent.ItemType<RustyRevolver>());
            revolverRecipe.AddRecipeGroup("JoJoStandsIron-TierBar", 16);
            revolverRecipe.AddTile(TileID.Anvils);
            revolverRecipe.Register();
        }
    }
}
