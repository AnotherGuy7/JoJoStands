using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class JoJoGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity
        {
            get { return true; }
        }
        public override bool CloneNewInstances
        {
            get { return true; }
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref float add, ref float mult, ref float flat)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.sexPistolsTier != 0 && (item.shoot == 10 || item.useAmmo == AmmoID.Bullet))
            {
                if (mPlayer.sexPistolsTier == 1)
                {
                    mult *= 1.05f;
                }
                if (mPlayer.sexPistolsTier == 2)
                {
                    mult *= 1.1f;
                }
                if (mPlayer.sexPistolsTier == 3)
                {
                    mult *= 1.15f;
                }
                if (mPlayer.sexPistolsTier == 4)
                {
                    mult *= 1.2f;
                }
            }
        }

        public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.sexPistolsTier != 0 && (weapon.shoot == 10 || weapon.useAmmo == AmmoID.Bullet) && mPlayer.sexPistolsLeft > 0)
            {
                mPlayer.sexPistolsLeft -= 1;
                type = mod.ProjectileType("SPBullet");
            }
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.StandOut)
            {
                return false;
            }
            return base.AltFunctionUse(item, player);
        }

        public override bool CanUseItem(Item item, Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.StandOut && !mPlayer.StandAutoMode && mPlayer.sexPistolsTier == 0)
            {
                if (item.potion || (item.mountType != -1))      //default value for mountType is -1
                {
                    return true;
                }
                if (!player.controlMount && item.mountType != -1)        //if the player isn't pressing controlMount AND the item's mount is actually something, 
                {
                    return false;
                }
                else if ((!player.controlQuickMana || !player.controlQuickHeal) && item.potion)
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public float normalGravity = 0f;
        public float normalFallSpeed = 0f;
        public bool gravitySaved = false;

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.TheWorldEffect && gravity != 3f)
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
            if (gravitySaved && !mPlayer.TheWorldEffect)
            {
                gravity = normalGravity;
                maxFallSpeed = normalFallSpeed;
                gravitySaved = false;
            }
            if (!mPlayer.TheWorldEffect && normalGravity != 0f && !gravitySaved)
            {
                normalFallSpeed = 0f;
                normalGravity = 0f;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe revolverRecipe = new ModRecipe(mod);
            revolverRecipe.AddIngredient(mod.ItemType("RustyRevolver"));
            revolverRecipe.AddIngredient(ItemID.IronBar, 16);
            revolverRecipe.SetResult(ItemID.Revolver);
            revolverRecipe.AddRecipe();
        }
    }
}
