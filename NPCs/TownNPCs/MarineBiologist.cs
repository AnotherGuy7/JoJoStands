using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.TownNPCs
{
    [AutoloadHead]
    public class MarineBiologist : ModNPC
    {
        public static bool userIsAlive = false;
        public static int standDamage = 0;

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
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (firstButton)
            {
                if (player.HeldItem.type == mod.ItemType("Hamon"))
                {
                    Main.npcChatText = "I haven't heard much about it but that old man tells me that it increases the power of your punches and allows you to heal while standing still.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("StarPlatinumT1") || mPlayer.StandSlot.Item.type == mod.ItemType("StarPlatinumT2"))
                {
                    Main.npcChatText = "You trying to mock me by asking about my own stand? What a pain... Well, I guess I'd be the one to know the most about it. I'll tell you what I can. \nGood grief, you've only just awoken Star Platinum? I expected better of a copycat like yourself. In its first or second tier, Star Platinum is nothing but a glorified pair of floating fists, but things get a bit more interesting as you reach its third and fourth tier.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("StarPlatinumT3"))
                {
                    Main.npcChatText = "At its third tier, Star Platinum gains the ability of Star Finger, a medium range attack that you can use to attack from a distance. I've personally rarely used this move, but it's useful.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("StarPlatinumFinal"))
                {
                    Main.npcChatText = "At its fourth tier, Star Platinum awakens its dormant ability, Star Platinum: The World. I learned this on my trip to Egypt in the 80s, but even I don't know why both Star Platinum and The World can stop time, though it sure worked out in my favor either way.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("HierophantGreenT1"))
                {
                    Main.npcChatText = "Your stand... one of my old friends had it. 'Nobody can deflect the emerald splash' he said. It's up to you to find out if that statement's true or not. \nWhat Hierophant lacks in power, it makes up for in quantity. Hierophant Green's main ability is to send out a ton of emerald splashes at a time, but unfortunately they do a very little amount of damage.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("HierophantGreenT2"))
                {
                    Main.npcChatText = "You now have the ability to place down tripwire. Any enemies who walk into these wires will soon be filled with emeralds.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("HierophantGreenT3") || mPlayer.StandSlot.Item.type == mod.ItemType("HierophantGreenFinal"))
                {
                    Main.npcChatText = "Ah, 20m Emerald Splash. The Old Man told me that my old friend used it during our fight against DIO. It sets up multiple tripwires around the player, covering about 30 tiles but at the cost of not being able to normally use your Hierophant Green.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("TheWorldT1"))
                {
                    Main.npcChatText = "...Get that thing away from me... Well, I guess I could tell you a bit about it, considering it's the same type of stand as my Star Platinum, but I won't enjoy it. \nIn its first tier, I would guess that The World would just be able to punch, like Star Platinum, while getting stronger and faster in its second. Talk to me again when it's a bit stronger.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("TheWorldT2"))
                {
                    Main.npcChatText = "Now that your The World got a bit stronger, I would assume that it learned its signature ability Time Stop, although I'm not sure why it doesn't learn it sooner. Use your special key to activate it.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("TheWorldT3"))
                {
                    Main.npcChatText = "Now that The World is at its third tier, The World can to throw deadly knives. I'm pretty sure those monsters outside aren't smart enough to hide Shonen Jump underneath their clothes, so the knives are pretty effective.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("TheWorldFinal"))
                {
                    Main.npcChatText = "Now this is The World I remember. You have fully learned The World's extents, and can now stop time for 9 seconds while throwing flurrys of knives at your enemies. You can even throw Road Rollers at your enemies while time is stopped too! Do me a favor and stay away from me.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("KillerQueenT1") || mPlayer.StandSlot.Item.type == mod.ItemType("KillerQueenT2"))
                {
                    Main.npcChatText = "That stand... I'm sure I destroyed its user... or, at least, that ambulance did. Well, either way, Killer Queen can turn things into bombs. Not physically, but any object, when touched, will make whoever's touching it explode. \nKiller Queen can throw out a barrage of punches, like most stands of its type, but it can also trigger a controlled detonation on a solid... block. This explosion has the power to damage enemies, but not the ground. Also, be careful not to blow yourself up.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("KillerQueenT3") || mPlayer.StandSlot.Item.type == mod.ItemType("KillerQueenFinal"))
                {
                    Main.npcChatText = "You've unlocked Sheer Heart Attack- it has no weaknesses. It's a small tank-like thing that pursues enemies, and even my Star Platinum wasn't able to destroy it. It's quite a unique ability... In it's fourth tier the cooldown for Sheer Heart Attack decreases and you will be able to send him out more. ";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("KillerQueenBTD"))
                {
                    Main.npcChatText = "You've unlocked yet another power... Bites The Dust. It allows you to rewind time back to the beginning of the day, causing enemies to explode during the rewind. Not only that, you can now shoot bubbles that you've touched and have them pierce up to 7 enemies before exploding.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("AchtungBaby"))
                {
                    Main.npcChatText = "Wha... Frankly, I'm not gonna ask how that's even possible. Perhaps the universal rupture caused the stand's power to transfer into the sunglasses. Either way, that Stand makes you invisible.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("GoldExperienceT1"))
                {
                    Main.npcChatText = "Ah, that stand... The stand of, what was his name, Haruno... no, Giorno Giovanna. Never got to ask him about it too much. What I do know is that it has an ability with extreme potential- the ability to create life. \nIn its current state, Gold Experience can only do so much. For now, you can create a frog that will counterattack anyone who strikes you. A strange way to counter an attack, but it works nonetheless.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("GoldExperienceT2"))
                {
                    Main.npcChatText = "Now, Gold Experience has unlocked the ability to create barriers for you. These barriers are actually trees, but they still offer protection from attacks, as if they were walls.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("GoldExperienceT3"))
                {
                    Main.npcChatText = "Ah, that ability... now you can accelerate the senses of your enemy so fast that they have quite the out of body experience, if you catch my drift. It also slows their physical bodies down. Along with that, you can create butterflies that will protect the loot an enemy is carrying, so that they don't get destroyed while fighting you. If you don't know what I mean, it doubles the chances of enemy loot.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("GoldExperienceFinal"))
                {
                    Main.npcChatText = "Gold Experience's arguably best ability, Limb Recreation, has been unlocked. With this, you can regenerate your health by healing your wounds. Only one more step...";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("GoldExperienceRequiem"))
                {
                    Main.npcChatText = "I can't believe it... you've done it. Gold Experience Requiem has been unleashed, and its powers are yours. It's new abilities are the ability to loop the deaths of a chosen enemy so they die multiple times and the ability to cancel the actions of enemies that touch you and return them to where they started.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("TuskAct1"))
                {
                    Main.npcChatText = "The mysterious force of the Spin... it reminds me of Hamon. Anyways, Tusk has Acts similar to the stand Echoes. Each act gains a new ability with the Spin. Every Spin user's goal is to attain the Infinite Spin. Think you can do it, " + player.name + "? \nTusk was somewhat weak when it began, only being able to fire nails like a bullet from a gun. By the way, I don't mean the metal kind of nails. I mean your fingernails and toenails. Don't worry, they grow back.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("TuskAct2"))
                {
                    Main.npcChatText = "Tusk Act 2 has learned the power of the Golden Rectangle, and can now fire nail bullets that can be guided towards targets. You move closer to the Infinite Spin.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("TuskAct3"))
                {
                    Main.npcChatText = "Act 3 allows you to fire a nail what creates a wormhole, which you can then travel through. This allows you to get the jump on enemies. Make sure you only fire if you have no doubts.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("TuskAct4"))
                {
                    Main.npcChatText = "Tusk Act 4 has found the power of the Infinite Spin, and can cause enemies to... infinitely spin. Along with that, Act 4 can now manifest into a stronger form than the other three acts, allowing it to punch enemies. The catch is that you have to gain rotational energy using Slow Dancer though... But congratulations on obtaining such a power. I'm sure it was a long and roundabout path.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("StickyFingersT1") || mPlayer.StandSlot.Item.type == mod.ItemType("StickyFingersT2"))
                {
                    Main.npcChatText = "While zippers may seem like quite the mundane ability to have, Sticky Fingers is a stand that lets you use them to their absolute maximum potential. Figure out how to use the zippers, and your enemies will be saying Arrivederci, very quickly. \nAt first, Sticky Fingers isn't the most powerful of Stands though it does allow you to unzip your enemies as it punches. Keep it going, don't let your resolve waver, and power up further.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("StickyFingersT3") || mPlayer.StandSlot.Item.type == mod.ItemType("StickyFingersFinal"))
                {
                    Main.npcChatText = "Now, you can unzip your own arms to throw out an extended reach punch. Think of it as a flail, because your fists do have mass, remember. Ah, you can also now throw a zipper in the direction of this thing called a 'cursor' is and zip 30 tiles in that direction";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("SexPistolsT1"))
                {
                    Main.npcChatText = "Ah, good grief... this, well, these, are Sex Pistols. They're six, sentient, bullet-shaped stands that allow you to fire off the gun they're attached to and make the bullets almost lock on to enemies. Sex Pistols never gained any new or extreme abilities, but each of your upgrades increases your guns power. More efficiency means more rounds out, and more bodies.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("SexPistolsT2"))
                {
                    Main.npcChatText = "The faster the pistols come back to you after being fired, the more power you gain. Did I tell you that the Pistols need to eat?";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("SexPistolsT3"))
                {
                    Main.npcChatText = "Faster and faster the chamber goes, can you keep up long enough to keep firing?";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("SexPistolsFinal"))
                {
                    Main.npcChatText = "Sex Pistols has reached max power, they come back to you almost instantly now. Just remember to keep some food in your pockets...";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("KingCrimsonT1"))
                {
                    Main.npcChatText = "The power of King Crimson falls to you now. A powerful stand that strikes at the speed of a sledgehammer. Learn its speeds and take control of its immense power. \nRight now, King Crimson is only capable of striking opponents. This isn't at all a bad thing, because of how King Crimson attacks. King Crimson strikes with such ferocity that it has been shown to tear holes opponents in people, and the attacks have such force that you will fly forward when attacks come out.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("KingCrimsonT2"))
                {
                    Main.npcChatText = "King Crimson has acquired its signature ability of skipping time. It's a strange ability in concept, so here's a summary- you can remove the cause from the effect. Jump past time itself, confuse your enemies, and take your place at the apex of creation.";
                }
                if ((mPlayer.StandSlot.Item.type == mod.ItemType("KingCrimsonT3") || mPlayer.StandSlot.Item.type == mod.ItemType("KingCrimsonFinal")))
                {
                    Main.npcChatText = "Congrats, it seems you've unlocked the power of Epitaph. Epitaph is able to look into the future, so you can anticipate your enemies moves and prepare for them in advance.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("CenturyBoyT1"))
                {
                    Main.npcChatText = "This Stand has no weaknesses, defensively. Century Boy is the ultimate physical defense- when you activate it, the stand mounts itself on your body and you are impervious to all harm. This does have one minor weakness... you lose the ability to move. Do with that what you will.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("CenturyBoyT2"))
                {
                    Main.npcChatText = "Looks like you learned that starting a dynamite then going invincible hurts your enemies and not you. What was stopping you from doing this earlier?";
                }
                if (player.HeldItem.type == mod.ItemType("DollyDaggerT1"))
                {
                    Main.npcChatText = "Intel from the SPW suggests that this stand merely reflects 35% of damage taken.";
                }
                if (player.HeldItem.type == mod.ItemType("DollyDaggerT2"))
                {
                    Main.npcChatText = "Now that you've honed your Dolly Dagger, it can relflect about 70% of damage taken. Even I would have trouble fighting you with that amount of reflection. Oh, you can also stab yourself with the dagger to send that damage to the nearest enemy. That stand of yours has lots of potential.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("MagiciansRedT1"))
                {
                    Main.npcChatText = "Ah, Magician's Red. The original owner of this Stand was the man who taught me about Stands in the first place. You can manipulate fire and create Ankhs to burn your enemies to ash.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("MagiciansRedT2"))
                {
                    Main.npcChatText = "Magician's Red can now use Red Bind, a technique to restrain enemies with your fire. Don't ask me how that doesn't burn through them.";
                }
                if ((mPlayer.StandSlot.Item.type == mod.ItemType("MagiciansRedT3") || mPlayer.StandSlot.Item.type == mod.ItemType("MagiciansRedFinal")))
                {
                    Main.npcChatText = "Now, Magician's Red has relearned all of its abilities. With Crossfire Hurricane, you can launch a volley of Ankhs and overwhelm your enemies.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("AerosmithT1"))
                {
                    Main.npcChatText = "Aerosmith may appear to be nothing more than a model plane, but don't be fooled. It has potential to be extremely powerful and agile. Let loose a hellstorm of bullets and turn your foes into a pincushion.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("AerosmithT2"))
                {
                    Main.npcChatText = "Now, Aerosmith can drop bombs on your enemies. Light them up brighter than before!";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("AerosmithT3") || mPlayer.StandSlot.Item.type == mod.ItemType("AerosmithFinal"))
                {
                    Main.npcChatText = "With this, Aerosmith can now find enemies using a CO2 detecting radar. If it breathes, make sure it won't anymore.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("TheHandT1"))
                {
                    Main.npcChatText = "Right now, The Hand is weak, only able to punch your opponents. It may be a good punch, but there's not much else right now.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("TheHandT2"))
                {
                    Main.npcChatText = "The Hand can now use its signature ability... its right hand. You can use The Hand's hand to scrape away space itself and teleport short distances. The Hand can now cause enemies to, perhaps, lose a few organs.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("TheHandT3"))
                {
                    Main.npcChatText = "With your increased mastery of The Hand, you can now pull enemies towards you by tapping " + JoJoStands.SpecialHotKey.GetAssignedKeys()[0] + ", and you can perform a very powerful attack with The Hand's Hand by holding" + JoJoStands.SpecialHotKey.GetAssignedKeys()[0] + ", given that they are in range.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("TheHandFinal"))
                {
                    Main.npcChatText = "The Hand has reached its maximum potential, and is more powerful than ever. Of course, you seem to be smarter than its previous user. I'm not saying he was a bad kid, or anything. Wonder where he is now...";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("GratefulDeadT1"))
                {
					Main.npcChatText = "Just seeing that Stand is making me feel older than I already am. That's the ability of Grateful Dead, so be careful using it around people you wanna keep around.\nRight now, Grateful Dead can only punch things. Not exactly unusual. Just keep going.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("GratefulDeadT2"))
                {
					Main.npcChatText = "Now, Grateful Dead can grab a target and forcefully age them. The amount of time aged depends on how long you hold them. Don't do handshakes.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("GratefulDeadT3"))
                {
					Main.npcChatText = "So, this Stand just became even more of a safety hazard to any allies, because now it can spread the aging effect as a mist. It's not as immediate, but now multiple people can be aged forwards at once.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("GratefulDeadT4"))
                {
					Main.npcChatText = "Finally, Grateful Dead is at maximum power. Just… stay a few feet away from me. I already have a couple of gray hairs.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("LockT1") || mPlayer.StandSlot.Item.type == mod.ItemType("LockT2"))
                {
					Main.npcChatText = "Oh. The Lock. I feel bad that you have to use this one… wait a minute. The Lock has the ability to make others' hearts heavy with guilt, literally. Use it, and anything that harms you will start to feel that immense weight. Some things may be immune to that guilt, and it can be overcome.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("LockT3") || mPlayer.StandSlot.Item.type == mod.ItemType("LockT4"))
                {
					Main.npcChatText = "The Lock can now force others to feel guilty without any action against you… by harming yourself. Use this ability with caution.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("WhitesnakeT1"))
                {
					Main.npcChatText = "Okay, you need to step away before I crush you. Get that Stand out of my sight. If you think I'm joking, come and find out.\n*sigh* Well, fine. Whitesnake, at this stage, can only throw punches. That's it.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("WhitesnakeT2"))
                {
					Main.npcChatText = "Now, Whitesnake can secrete a gooey substance that damages enemies and sticks to surfaces. It's not a pleasant experience.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("WhitesnakeT3"))
                {
					Main.npcChatText = "Whitesnake has unlocked the ability to steal discs from opponents. If your enemy isn't a Stand user, then they'll be devoid of thought for a few moments. If they are, then you'll disable their Stand temporarily- do all of this by pressing " + JoJoStands.SpecialHotKey.GetAssignedKeys()[0];
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("WhitesnakeFinal"))
                {
					Main.npcChatText = "Due to your annoying persistence, Whitesnake has reached its maximum potential. Now, keep it away from me, I mean it.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("SilverChariotT1"))
                {
                    Main.npcChatText = "Ah, Silver Chariot. Another Stand that belonged to a friend of mine, a fellow Stardust Crusader. You think my hair is ridiculous, you should have seen his. Anyways, Silver Chariot wields a rapier with lightning speed and deadly precision.\nAt the moment, Silver Chariot can only stab with its rapier. However underwhelming that may sound, the stabs come out extremely quick, almost invisible to the eye.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("SilverChariotT2"))
                {
                    Main.npcChatText = "On top of your stabbing flurry, Silver Chariot can now deflect projectiles back at your enemy with {whatever right click is}, almost like a game of baseball, or tennis.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("SilverChariotT3"))
                {
                    Main.npcChatText = "At this stage, Silver Chariot can now shed its armor to become so fast it can create afterimages. What this does for you is allows Silver Chariot to move significantly faster, but you also take more damage. Use this carefully.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("SilverChariotFinal"))
                {
                    Main.npcChatText = "Silver Chariot has reached the highest peak it can. The power of this stand alone rivals legendary swordsmen of old, and your toughest enemies. I wonder what would happen if this Stand were to change form or something.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("CreamT1"))
                {
                    Main.npcChatText = "That Stand makes me… nervous, and I cannot tell you why. Anyways, uh, Cream can harness the void and erase things, almost like that blockhead friend of Josuke’s.\nJust like the rest, Cream cannot do very much at this stage. It can throw a mean chop though, and it can cause enemies to lose their organs. Literally.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("CreamT2"))
                {
                    Main.npcChatText = "Cream has gained the ability to, well, eat you. Specifically, you can enter the void by pressing {specialabilitykey} and riding in Cream like an interdimensional ball of sorts. To control Cream in this state, use {attack} to guide it. The downsides are that you have a limited time in there, indicated by the Void Gauge, and you are effectively blind while in the void, so know where your target is beforehand. Once that time runs out, Cream pukes you out. I am being serious.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("CreamT3"))
                {
                    Main.npcChatText = "As a way to quickly refill your Void Gauge, Cream can consume you and fly about. On top of that, your void offensive now lasts longer, and once it runs out, Cream automatically enters that defensive recharge state instead of being puked out.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("CreamFinal"))
                {
                    Main.npcChatText = "At this final point, Cream can stay in the void for even longer. Once it runs out, you will still enter the recharge state rather than being puked out. Actually, I wonder if you could use this to fire yourself like a cannon… questions for another day.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("HermitPurpleT1"))
                {
                    Main.npcChatText = "Never in my life did I think I would see another Stand user with Hermit Purple. That Stand is special because the last owner I knew, my gramps, was a Hamon user as well, which makes it unique in that he used both in tandem. I wonder, do you have that ability?\nAt the moment, Hermit Purple is nothing more than a mass of vines. You can certainly hit people with them. It will certainly be more effective than getting mauled.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("HermitPurpleT2"))
                {
                    Main.npcChatText = "Well, it seems my initial question has been answered. Hermit Purple coils around you, and anything that opts to attack you will be met with quite the burn. On top of that, now you can grab and pull enemies into their eventual doom by 'clicking and holding right-click'.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("HermitPurpleT3"))
                {
                    Main.npcChatText = "Your connection to Hermit Purple grows like rampant vines; now you can press " + JoJoStands.SpecialHotKey.GetAssignedKeys()[0] + " to overcharge Hermit Purple with Hamon and power up your passive counter. The next three things that hit you will not only feel the burn, but they will take damage and get knocked away.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("HermitPurpleFinal"))
                {
                    Main.npcChatText = "Hermit Purple is as strong as it will ever be, thanks to you. I know that the old man would never have come close, as long as he lived. I still have no idea how he managed to survive as long as he did.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("BadCompanyT1"))
                {
                    Main.npcChatText = "I don't know yet.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("BadCompanyT2"))
                {
                    Main.npcChatText = "I don't know yet.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("BadCompanyT3"))
                {
                    Main.npcChatText = "I don't know yet.";
                }
                if (mPlayer.StandSlot.Item.type == mod.ItemType("BadCompanyT4"))
                {
                    Main.npcChatText = "I don't know yet.";
                }

                if (mPlayer.StandSlot.Item.IsAir)
                {
                    int helpText = Main.rand.Next(0, 7);
                    switch (helpText)
                    {
                        case 0:
                            Main.npcChatText = "Have a stand item in the 'Stand Slot' then press 'Stand Help' and I'll tell you what it can do and how to get it's tiers OR keep pressing 'Stand Help' and I'll tell you how to get specific items and do certain actions.";
                            break;
                        case 1:
                            Main.npcChatText = "To use Stand Specials, in controls, you need to bind a key to 'JoJoStands: Special' then while the stand is activated you press the key to use the special";
                            break;
                        case 2:
                            Main.npcChatText = "Requiem Arrows can be obtained by getting a viral pearl and merging it with a Stand Arrow. This happens because all the Viral Meteorite involved starts to morph and have a different use alltogether.";
                            break;
                        case 3:
                            Main.npcChatText = "To move during another users timestop, hold a time-stopping capable stand and use it's timestop. If the duration of your timestop exceeds the duration of the other users timestop, you can take over that users timestop as well!";
                            break;
                        case 4:
                            Main.npcChatText = "You can dye your stand by putting a dye in the 'Dye Slot.'";
                            break;
                        case 5:
                            Main.npcChatText = "To use a stand you have to put it in the 'Stand Slot' then press the 'Stand Out' bind, which you have to bind a key to in 'Controls.'";
                            break;
                        case 6:
                            Main.npcChatText = "Most stands will require crafting materials called 'Wills' in order to be made. You can get the Will to Fight in Forests during Daytime, the Will to Protect in Forests at night, the Will to Change in the Jungle, the Will to Control in the Corruption/Crimson, the Will to Destory in the Underworld, and finally, the Will to Escape from the dungeon.";
                            break;
                    }
                    
                    if (!mPlayer.receivedArrowShard)
                    {
                        Main.npcChatText = "You seem... reliable. Here, I want you to take this and use it on yourself when you can, I think you'll like what happens when you do.";
                        player.QuickSpawnItem(mod.ItemType("ArrowShard"));
                        mPlayer.receivedArrowShard = true;
                    }
                }
            }
        }

        public override string GetChat()       //Allows you to give this town NPC a chat message when a player talks to it.
        {
            switch (Main.rand.Next(4))    //this are the messages when you talk to the npc, if you want to add more cases, you first need to change the Main.rand to the number of cases (default is included!)
            {
                case 0:
                    return "Yare yare daze... I just can't have a normal day, can I";
                case 1:
                    return "Now that Kira has been dealt with, I can go back to the SPW.";
                case 2:
                    return "I came here looking for a stand user... Do you know who that might be?";
                case 3:
                    return "Back in Egypt, my team and I had to kill a psychotic time-stopping vampire, You might have a similar issue here, " + Main.LocalPlayer.name;
                default:
                    return "All stand users eventually meet, I guess that's why I'm here";
            }
        }

        public override bool CheckActive()
        {
            userIsAlive = true;
            return true;
        }

        public override bool CheckDead()
        {
            userIsAlive = false;
            return true;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)//  Allows you to determine the damage and knockback of this town NPC attack
        {
            if (!Main.hardMode)
            {
                standDamage = 40;  //npc damage
                knockback = 2f;   //npc knockback
            }
            if (Main.hardMode)
            {
                standDamage = 62;
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
            if (!Projectiles.NPCStands.StarPlatinumNPCStand.SPActive)
            {
                projType = mod.ProjectileType("StarPlatinumNPCStand");
                attackDelay = 1;
            }
        }
    }
}