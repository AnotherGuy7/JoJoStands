using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
 
namespace JoJoStands
{
    public class MyPlayer : ModPlayer
    {
        private const int saveVersion = 0;
        public bool StarPlatinumMinion = false;
        public bool SheerHeartAttackMinion = false;
        public bool Aerosmith = false;
        public static bool hasProjectile;
		public bool JotaroCapAccessoryPrevious;
		public bool JotaroCapAccessory;
		public bool JotaroCapHideVanity;
		public bool JotaroCapForceVanity;
		public bool JotaroCapPower;
        public bool AjaStone;
        public static bool TheWorldEffect;      //the worlds first effect
        public static bool TheWorldAfterEffect;     //the worlds secondary ability, forgot whether I used it or not
        public static bool MinionCurrentlyActive = false;       //to determine if a stand minion is currently active
        public static bool SHAactive = false;      //to determine if SHA is active at the moment
        public static bool BitesTheDust = false;

        public override void ResetEffects()
        {
            StarPlatinumMinion = false;
            SheerHeartAttackMinion = false;
            Aerosmith = false;
        }

        public virtual void OnEnterWorld(MyPlayer player)
        {}

        public override void PostUpdateEquips()
        {}

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (!SHAactive)
            {
                if (JoJoStands.ItemHotKey.JustPressed && player.HeldItem.type == mod.ItemType("KillerQueenT3"))       //KQ Tier 3 Sheer Heart Attack spawning
                {
                    Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("SheerHeartAttack"), 107, 7f, player.whoAmI);
                }
                if (JoJoStands.ItemHotKey.JustPressed && player.HeldItem.type == mod.ItemType("KillerQueenFinal"))    //KQ Final Tier Sheer Heart Attack spawning
                {
                    Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("SheerHeartAttack"), 142, 7f, player.whoAmI);
                }
            }
        }

        public override void SetupStartInventory (IList<Item> items)
        {
			Item item = new Item();
            item.SetDefaults(mod.ItemType("WrappedPicture"));
            item.stack = 1;
            items.Add(item);
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)     //don't even know what I did here, nor if it even works...
        {
            ModPacket packet = mod.GetPacket();
            packet.Write(TheWorldEffect);
            packet.Write((byte)player.whoAmI);
            packet.Send(toWho, fromWho);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)      //same comment as above...
        {
            if (TheWorldEffect && !player.HasBuff(mod.BuffType("TheWorldBUff")))
            {
                player.gravity = 3f;
                player.controlLeft = false;
                player.controlJump = false;
                player.controlRight = false;
                player.controlDown = false;
                player.controlQuickHeal = false;
                player.controlQuickMana = false;
                player.controlRight = false;
                player.controlUseTile = false;
                player.controlUp = false;
                player.maxRunSpeed *= 0;
                player.moveSpeed *= 0;
            }
        }

        public override void PreUpdate()
        {}

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (player.HasItem(mod.ItemType("PokerChip")))
            {
                player.ConsumeItem(mod.ItemType("PokerChip"), true);
                Main.NewText("The chip has given you new life!");
                return false;
            }
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/ToBeContinued4"));
            Main.NewText("To Be Continued");
            return true;
        }
    }
}