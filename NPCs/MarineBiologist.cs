using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.NPCs
{
    [AutoloadHead]
    public class MarineBiologist : ModNPC
    {
        public static bool userIsAlive = false;
        public static int attackPower = 0;

        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 18; //the npc sprite width
            npc.height = 46;  //the npc sprite height
            npc.aiStyle = 7; //this is the npc ai style, 7 is Pasive Ai
            npc.defense = 34;
            npc.lifeMax = 500;
            npc.HitSound = SoundID.NPCHit1; 
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 1f;
            Main.npcFrameCount[npc.type] = 25;
            NPCID.Sets.ExtraFramesCount[npc.type] = 9;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 50; //this defines the npc danger detect range
            NPCID.Sets.AttackType[npc.type] = 1; //this is the attack type,  0 (throwing), 1 (shooting), or 2 (magic). 3 (melee) 
            NPCID.Sets.HatOffsetY[npc.type] = 4; //this defines the party hat position
            animationType = NPCID.Guide;  //this copy the guide animation
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return true;
        }

        public override string TownNPCName()
        {
            return "Jotaro";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Stand Help";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool openShop)
        {
            Player player = Main.LocalPlayer;
            if (firstButton)
            {
                if (firstButton && (player.HeldItem.type == mod.ItemType("StarPlatinumT1") || player.HeldItem.type == mod.ItemType("StarPlatinumT2")))
                {
                    Main.npcChatText = "You trying to mock me by asking about my own stand? What a pain... Well, I guess I'd be the one to know the most about it. I'll tell you what I can. \nGood grief, you've only just awoken Star Platinum? I expected better of a copycat like yourself. In its first or second tier, Star Platinum is nothing but a glorified pair of floating fists, but things get a bit more interesting as you reach its third and fourth tier.";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("StarPlatinumT3"))
                {
                    Main.npcChatText = "At its third tier, Star Platinum gains the ability of Star Finger, a medium range attack that you can use to attack from a distance. I've personally rarely used this move, but it's useful.";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("StarPlatinumFinal"))
                {
                    Main.npcChatText = "At its fourth tier, Star Platinum awakens its dormant ability, Star Platinum: The World. I learned this on my trip to Egypt in the 80s, but even I don't know why both Star Platinum and The World can stop time, but it sure worked out in my favor either way.";
                }
                if (firstButton && (player.HeldItem.type == mod.ItemType("HierophantGreenT1") || player.HeldItem.type == mod.ItemType("HierophantGreenT2") || player.HeldItem.type == mod.ItemType("HierophantGreenT3")))
                {
                    Main.npcChatText = "Your stand... one of my old friends had it. 'Nobody can deflect the emerald splash' he said. It's up to you to find out if that statement's true or not. \nWhat Hierophant lacks in power, it makes up for in quantity. Hierophant Green's main ability is to send out a ton of emerald splashes at a time, but unfortunately they do a very little amount of damage.";
                }
                if (firstButton && (player.HeldItem.type == mod.ItemType("TheWorldT1") || player.HeldItem.type == mod.ItemType("TheWorldT2")))
                {
                    Main.npcChatText = "...Get that thing away from me... Well, I guess I could tell you a bit about it, considering it's the same type of stand as my Star Platinum, but I won't enjoy it. \nIn its first tier, I would guess that The World would just be able to punch, like Star Platinum, while getting stronger and faster in its second. Talk to me again when it's a bit stronger.";
                }
                if (firstButton && (player.HeldItem.type == mod.ItemType("TheWorldT3") || player.HeldItem.type == mod.ItemType("TheWorldFinal")))
                {
                    Main.npcChatText = "When The World reaches its third tier, I would assume that it learns its signature ability Time Stop, although I'm not sure why it doesn't learn it sooner. Use your special key to activate it. \nAt its fourth tier, The World learns the ability to throw deadly knives. I'm pretty sure those monsters outside aren't smart enough to hide Shonen Jump underneath their clothes, so the knives are pretty effective.";
                }
                if (firstButton && (player.HeldItem.type == mod.ItemType("KillerQueenT1") || player.HeldItem.type == mod.ItemType("KillerQueenT2")))
                {
                    Main.npcChatText = "That stand... I'm sure I destroyed its user... or, at least, that ambulance did. Well, either way, Killer Queen can turn things into bombs. Not physically, but any object, when touched, will make whoever's touching it explode. \nKiller Queen can throw out a barrage of punches, like most stands of its type, but it can also trigger a controlled detonation on a solid... block. This explosion has the power to damage enemies, but not the ground. Also, be careful not to blow yourself up.";
                }
                if (firstButton && (player.HeldItem.type == mod.ItemType("KillerQueenT3") || player.HeldItem.type == mod.ItemType("KillerQueenFinal")))
                {
                    Main.npcChatText = "You've unlocked Sheer Heart Attack- it has no weaknesses. It's a small tank-like thing that pursues enemies, and even my Star Platinum wasn't able to destroy it. It's quite a unique ability... In it's fourth tier the cooldown for Sheer Heart Attack decreases and you will be able to send him out more. ";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("KillerQueenBTD"))
                {
                    Main.npcChatText = "You've unlocked yet another power... Bites The Dust. It allows you to rewind time back to the beginning of the day, but you need to remember to tag someone before using it. Not only that, you can now shoot bubbles that you've touched and have them pierce up to 7 enemies before exploding.";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("AchtungBaby"))
                {
                    Main.npcChatText = "Wha... Frankly, I'm not gonna ask how that's even possible. Perhaps the universal rupture caused the stand's power to transfer into the sunglasses. Either way, that Stand makes you invisible";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("GoldExperienceT1"))
                {
                    Main.npcChatText = "Ah, that stand... The stand of, what was his name, Haruno... no, Giorno Giovanna. Never got to ask him about it too much. What I do know is that it has an ability with extreme potential- the ability to create life. \nIn its current state, Gold Experience can only do so much. For now, you can create a frog that will counterattack anyone who strikes you. A strange way to counter an attack, but it works nonetheless.";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("GoldExperienceT2"))
                {
                    Main.npcChatText = "Now, Gold Experience has unlocked the ability to create barriers for you. These barriers are actually trees, but they still offer protection from attacks, as if they were walls.";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("GoldExperienceT3"))
                {
                    Main.npcChatText = "Ah, that ability... now you can accelerate the senses of your enemy so fast that they have quite the out of body experience, if you catch my drift. It also slows their physical bodies down. Along with that, you can create butterflies that will protect the loot an enemy is carrying, so that they don't get destroyed while fighting you. If you don't know what I mean, it doubles the chances of enemy loot.";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("GoldExperienceFinal"))
                {
                    Main.npcChatText = "Gold Experience's arguably best ability, Limb Recreation, has been unlocked. With this, you can regenerate your health by healing your wounds. Only one more step...";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("GoldExperienceRequiem"))
                {
                    Main.npcChatText = "I can't believe it... you've done it. Gold Experience Requiem has been unleashed, and its powers are yours. It's new abilities are the ability to loop the deaths of a chosen enemy so they die multiple times and the ability to cancel the actions of enemies that touch you and return them to where they started.";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("TuskAct1"))
                {
                    Main.npcChatText = "The mysterious force of the Spin... it reminds me of Hamon. Anyways, Tusk has Acts similar to the stand Echoes. Each act gains a new ability with the Spin. Every Spin user's goal is to attain the Infinite Spin. Think you can do it, " + player.name + "? \nTusk was somewhat weak when it began, only being able to fire nails like a bullet from a gun. By the way, I don't mean the metal kind of nails. I mean your fingernails and toenails. Don't worry, they grow back.";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("TuskAct2"))
                {
                    Main.npcChatText = "Tusk Act 2 has learned the power of the Golden Rectangle, and can now fire nail bullets that can be guided towards targets. You move closer to the Infinite Spin.";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("TuskAct3"))
                {
                    Main.npcChatText = "Act 3 allows you to fire a nail what creates a wormhole, which you can then travel through. This allows you to get the jump on enemies. Make sure you only fire if you have no doubts.";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("TuskAct4"))
                {
                    Main.npcChatText = "Tusk Act 4 has found the power of the Infinite Spin, and can cause enemies to... infinitely spin. Along with that, Act 4 can now manifest into a stronger form than the other three acts, allowing it to punch enemies. Congratulations on obtaining such a power. It was a long and roundabout path.";
                }
                if (firstButton && (player.HeldItem.type == mod.ItemType("StickyFingersT1") || player.HeldItem.type == mod.ItemType("StickyFingersT2")))
                {
                    Main.npcChatText = "While zippers may seem like quite the mundane ability to have, Sticky Fingers is a stand that lets you use them to their absolute maximum potential. Figure out how to use the zippers, and your enemies will be saying Arrivederci, very quickly. \nAt first, Sticky Fingers isn't the most powerful of Stands though it does allow you to unzip your enemies as it punches. Keep it going, don't let your resolve waver, and power up further.";
                }
                if (firstButton && (player.HeldItem.type == mod.ItemType("StickyFingersT3") || player.HeldItem.type == mod.ItemType("StickyFingersFinal")))
                {
                    Main.npcChatText = "Now, you can unzip your own arms to throw out an extended reach punch. Think of it as a flail, because your fists do have mass, remember.";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("SexPistolsT1"))
                {
                    Main.npcChatText = "Ah, good grief... this, well, these, are Sex Pistols. They're six, sentient, bullet-shaped stands that allow you to fire off the gun they're attached to now and make the bullets almost lock on to enemies. Be careful, having a six shooter means you have to reload. \nSex Pistols never gained any new or extreme abilities, but each of your upgrades decreases the amount of time all of you take to reload the gun. More efficiency means more rounds out, and more bodies.";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("SexPistolsT2"))
                {
                    Main.npcChatText = "More reload speed, more power. Did I tell you that the Pistols need to eat? Don't forget that the special is always to reload even faster.";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("SexPistolsT3"))
                {
                    Main.npcChatText = "Faster and faster the chamber goes, can you keep up long enough to keep firing?";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("SexPistolsFinal"))
                {
                    Main.npcChatText = "Sex Pistols has reached max power, and now you can reload at lightning speed. Just remember to keep some food in your pockets...";
                }
                if (firstButton && (player.HeldItem.type == mod.ItemType("KingCrimsonT1") || player.HeldItem.type == mod.ItemType("KingCrimsonT2")))
                {
                    Main.npcChatText = "The power of King Crimson falls to you now. A powerful stand that strikes at the speed of a sledgehammer. Learn its speeds and take control of its immense power. \nRight now, King Crimson is only capable of striking opponents. This isn't at all a bad thing, because of how King Crimson attacks. King Crimson strikes with such ferocity that it has been shown to tear holes opponents in people, and the attacks have such force that you will fly forward when attacks come out.";
                }
                if (firstButton && (player.HeldItem.type == mod.ItemType("KingCrimsonT3") || player.HeldItem.type == mod.ItemType("KingCrimsonFinal")))
                {
                    Main.npcChatText = "King Crimson has acquired its signature ability of skipping time. It's a strange ability in concept, so here's a summary- you can remove the cause from the effect. Jump past time itself, confuse your enemies, and take your place at the apex of creation.";
                }
                if (firstButton && player.HeldItem.type == mod.ItemType("CenturyBoy"))
                {
                    Main.npcChatText = "This Stand has no weaknesses, defensively. Century Boy is the ultimate physical defense- when you activate it, the stand mounts itself on your body and you are impervious to all harm. This does have one minor weakness... you lose the ability to move. Do with that what you will.";
                }
            }
            if (firstButton && player.HeldItem.IsAir)
            {
                int helpText = Main.rand.Next(0, 6);
                if (helpText == 0)
                {
                    Main.npcChatText = "Hold a stand item then press 'Stand Help' and I'll tell you what it can do and how to get it's tiers OR keep pressing 'Stand Help' and I'll tell you how to get specific items and do certain actions.";
                }
                if (helpText == 1)
                {
                    Main.npcChatText = "To get a Stand Arrow, if you created a new character, in your inventory should be a 'Wrapped Picture', use it to spawn Yoshihiro. You can also kill birds to get a 'Wrapped Picture'. Kill Yoshihiro and he drops the arrow";
                }
                if (helpText == 2)
                {
                    Main.npcChatText = "To use Stand Specials, in controls, you need to bind a key to 'JoJoStands: Special' then while holding the item, you press the key to use the special";
                }
                if (helpText == 3)
                {
                    Main.npcChatText = "Requiem arrows are drops from the Moon Lord, though they are pretty rare...";
                }
                if (helpText == 4)
                {
                    Main.npcChatText = "To use stand accessories, in controls, you need to bind a key to 'JoJoStands: Accessories' then while wearing the accessory, you press the key to use it's ability";
                }
                if (helpText == 5)
                {
                    Main.npcChatText = "To control your stand, First, you need to have a controllable stand minion active(any disc minion), Second, you need to have a bind to 'JoJoStands: Stand Control', Third, you need to have a control style chosen in the Mod Configurations in 'JoJoStands: CustomizableOptions'. While the minion is active, you press the bind and you are able to control your stand depending on the style you picked.";
                }
            }
        }

        public override string GetChat()       //Allows you to give this town NPC a chat message when a player talks to it.
        {
            switch (Main.rand.Next(6))    //this are the messages when you talk to the npc, if you want to add more cases, you first need to change the Main.rand to the number of cases (default is included!)
            {
                case 0:
                    return "Yare yare daze... I just can't have a normal day, can I";
                case 1:
                    return "Now that Kira has been dealt with, I can go back to the SPW.";
                case 2:
                    return "I beat up a professor for my degree";
                case 3:
                    return "I came here looking for a stand user... Do you know who that might be?";
                case 4:
                    return "All stand users eventually meet, I guess that's why I'm here";
                case 5:
                    return "Back in Egypt, my team and I had to kill a psychotic time-stopping vampire, You might have a similar issue here, " + Main.LocalPlayer.name;
                default:
                    return "I'm just here...";
            }
        }

        public override bool CheckActive()
        {
            userIsAlive = true;
            return base.CheckActive();
        }

        public override bool CheckDead()
        {
            userIsAlive = false;
            return base.CheckDead();
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)//  Allows you to determine the damage and knockback of this town NPC attack
        {
            if (!Main.hardMode)
            {
                attackPower = 40;  //npc damage
                knockback = 2f;   //npc knockback
            }
            if (Main.hardMode)
            {
                attackPower = 62;
                knockback = 3f;
            }
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)  //Allows you to determine the cooldown between each of this town NPC's attack. The cooldown will be a number greater than or equal to the first parameter, and less then the sum of the two parameters.
        {
            cooldown = 5;
            randExtraCooldown = 10;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)//Allows you to determine the projectile type of this town NPC's attack, and how long it takes for the projectile to actually appear
        {
            if (!Projectiles.NPCStands.StarPlatinumPart4.SPActive)
            {
                projType = mod.ProjectileType("StarPlatinumPart4");
                attackDelay = 1;
            }
        }
    }
}