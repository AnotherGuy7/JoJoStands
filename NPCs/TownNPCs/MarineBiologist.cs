using JoJoStands.Items;
using JoJoStands.Items.Hamon;
using JoJoStands.Items.Vanities;
using JoJoStands.Projectiles.NPCStands;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.TownNPCs
{
    [AutoloadHead]
    public class MarineBiologist : ModNPC
    {
        public static bool UserIsAlive = false;
        public static int standDamage = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 50; //this defines the NPC danger detect range
            NPCID.Sets.AttackType[NPC.type] = 1; //this is the attack type,  0 (throwing), 1 (shooting), or 2 (magic). 3 (melee) 
            NPCID.Sets.HatOffsetY[NPC.type] = 4; //this defines the party hat position
            NPC.Happiness.SetBiomeAffection<OceanBiome>(AffectionLevel.Love);
            NPC.Happiness.SetBiomeAffection<ForestBiome>(AffectionLevel.Like);
            NPC.Happiness.SetBiomeAffection<DesertBiome>(AffectionLevel.Hate);
            NPC.Happiness.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Dislike);
            NPC.Happiness.SetNPCAffection<Priest>(AffectionLevel.Hate);
            NPC.Happiness.SetNPCAffection<Gambler>(AffectionLevel.Hate);
            NPC.Happiness.SetNPCAffection<HamonMaster>(AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection(NPCID.Angler, AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Dislike);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18; //the NPC sprite width
            NPC.height = 46;  //the NPC sprite height
            NPC.aiStyle = 7; //this is the NPC ai style, 7 is Pasive Ai
            NPC.defense = 34;
            NPC.lifeMax = 500;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 1f;
            AnimationType = NPCID.Guide;  //this copy the guide animation
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement("Affiliate of the SpeedWagon Foundation and an expert in Stand research. He passed by for an unknown reason, or was he perhaps attracted here by a certain force?")
            });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return true;
        }

        public override List<string> SetNPCNameList()
        {
            List<string> possibleNames = new List<string>();

            possibleNames.Add("Jotaro");

            return possibleNames;
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
                if (mPlayer.awaitingViralMeteoriteTip)
                {
                    mPlayer.awaitingViralMeteoriteTip = false;
                    Main.npcChatText = "Did you feel that too? It felt as if a certain force were pulling us toward the meteor. That is no ordinary meteor, we must find it immediately. If that meteorite happened to be a Viral Meteorite, we must take care of that virus as soon as possible before it falls into the wrong hands again. Make sure to check the entire surface of the world for it. If you can't find it, check your sky islands and chasms as well. Hurry!";
                    return;
                }

                if (player.HeldItem.type == ModContent.ItemType<Hamon>())
                {
                    Main.npcChatText = "I haven't heard much about it, but that old man tells me that Hamon is a versatile use of sun-like energy. From what he's told me, it can increase your physical capabilities and be used defensively or offensively, healing being one of its most useful uses. If that's all true, it's no wonder that man has lived this long.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<StarPlatinumT1>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<StarPlatinumT2>())
                {
                    Main.npcChatText = "You trying to mock me by asking about my own stand? What a pain... Well, I guess I'd be the one to know the most about it. I'll tell you what I can. \nGood grief, you've only just awoken Star Platinum? I expected better of a copycat like yourself. In its first or second tier, Star Platinum is nothing but a glorified pair of floating fists, but things get a bit more interesting as you reach its third and fourth tier.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<StarPlatinumT3>())
                {
                    Main.npcChatText = "At its third tier, Star Platinum gains the ability of Star Finger, a medium range attack that you can use to attack from a distance. I've personally rarely used this move, but it's useful.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<StarPlatinumFinal>())
                {
                    Main.npcChatText = "At its fourth tier, Star Platinum awakens its dormant ability, Star Platinum: The World. I learned this on my trip to Egypt in the 80s, but even I don't know why both Star Platinum and The World can stop time, though it sure worked out in my favor either way.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<HierophantGreenT1>())
                {
                    Main.npcChatText = "Your stand... one of my old friends had it. 'Nobody can deflect the emerald splash' he said. It's up to you to find out if that statement's true or not. \nWhat Hierophant lacks in power, it makes up for in quantity. Hierophant Green's main ability is to send out a ton of emerald splashes at a time, but unfortunately they do a very little amount of damage.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<HierophantGreenT2>())
                {
                    Main.npcChatText = "You now have the ability to shoot wires that wrap and paralyze your enemies. Enemies that are wrapped in your wires will not be able to move for a short time, so use that time wisely. Hierophant Green is also able to be controlled at much farther distances now, so take advantage of that to set up tripwires and shoot from advantageous positions.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<HierophantGreenT3>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<HierophantGreenFinal>())
                {
                    Main.npcChatText = "Ah, 20m Emerald Splash. The Old Man told me that my old friend used it during our fight against DIO. It sets up multiple tripwires around the player, covering about 30 tiles but at the cost of not being able to normally use your Hierophant Green.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TheWorldT1>())
                {
                    Main.npcChatText = "...Get that thing away from me... Well, I guess I could tell you a bit about it, considering it's the same type of stand as my Star Platinum, but I won't enjoy it. \nIn its first tier, I would guess that The World would just be able to punch, like Star Platinum, while getting stronger and faster in its second. Talk to me again when it's a bit stronger.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TheWorldT2>())
                {
                    Main.npcChatText = "Now that your The World got a bit stronger, I would assume that it learned its signature ability Time Stop, although I'm not sure why it doesn't learn it sooner. Use your special key to activate it.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TheWorldT3>())
                {
                    Main.npcChatText = "Now that The World is at its third tier, The World can to throw deadly knives. I'm pretty sure those monsters outside aren't smart enough to hide Shonen Jump underneath their clothes, so the knives are pretty effective.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TheWorldFinal>())
                {
                    Main.npcChatText = "Now this is The World I remember. You have fully learned The World's extents, and can now stop time for 9 seconds while throwing flurrys of knives at your enemies. You can even throw Road Rollers at your enemies while time is stopped too! Do me a favor and stay away from me.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<KillerQueenT1>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<KillerQueenT2>())
                {
                    Main.npcChatText = "That stand... I'm sure I destroyed its user... or, at least, that ambulance did. Well, either way, Killer Queen can turn things into bombs. Not physically, but any object, when touched, will make whoever's touching it explode. \nKiller Queen can throw out a barrage of punches, like most stands of its type, but it can also trigger a controlled detonation on a solid... block. This explosion has the power to damage enemies, but not the ground. Also, be careful not to blow yourself up.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<KillerQueenT3>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<KillerQueenFinal>())
                {
                    Main.npcChatText = "You've unlocked Sheer Heart Attack- it has no weaknesses. It's a small tank-like thing that pursues enemies, and even my Star Platinum wasn't able to destroy it. It's quite a unique ability... In it's fourth tier the cooldown for Sheer Heart Attack decreases and you will be able to send him out more. ";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<KillerQueenBTD>())
                {
                    Main.npcChatText = "You've unlocked yet another power... Bites The Dust. Bites the Dust allows you to rewind time back to the moment you first activated the ability. It's useful for dodging lethal events that you know will happen in the future, in theory anyway. We only know this much because of the way its original user acted before his... final moments.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<AchtungBaby>())
                {
                    Main.npcChatText = "Wha... Frankly, I'm not gonna ask how that's even possible. Perhaps the universal rupture caused the stand's power to transfer into the sunglasses. Either way, that Stand makes you invisible.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<GoldExperienceT1>())
                {
                    Main.npcChatText = "Ah, that stand... The stand of, what was his name, Haruno... no, Giorno Giovanna. Never got to ask him about it too much. What I do know is that it has an ability with extreme potential- the ability to create life. \nIn its current state, Gold Experience can only do so much. For now, you can create a frog that will counterattack anyone who strikes you. A strange way to counter an attack, but it works nonetheless.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<GoldExperienceT2>())
                {
                    Main.npcChatText = "Now, Gold Experience has unlocked the ability to create barriers for you. These barriers are actually trees, but they still offer protection from attacks, as if they were walls.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<GoldExperienceT3>())
                {
                    Main.npcChatText = "Ah, that ability... now you can accelerate the senses of your enemy so fast that they have quite the out of body experience, if you catch my drift. It also slows their physical bodies down. Along with that, you can create butterflies that will protect the loot an enemy is carrying, so that they don't get destroyed while fighting you. If you don't know what I mean, it doubles the chances of enemy loot.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<GoldExperienceFinal>())
                {
                    Main.npcChatText = "Gold Experience's arguably best ability, Limb Recreation, has been unlocked. With this, you can regenerate your health by healing your wounds. Only one more step...";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<GoldExperienceRequiem>())
                {
                    Main.npcChatText = "I can't believe it... you've done it. Gold Experience Requiem has been unleashed, and its powers are yours. It's new abilities are the ability to loop the deaths of a chosen enemy so they die multiple times and the ability to cancel the actions of enemies that touch you and return them to where they started.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TuskAct1>())
                {
                    Main.npcChatText = "The mysterious force of the Spin... it reminds me of Hamon. Anyways, Tusk has Acts similar to the stand Echoes. Each act gains a new ability with the Spin. Every Spin user's goal is to attain the Infinite Spin. Think you can do it, " + player.name + "? \nTusk was somewhat weak when it began, only being able to fire nails like a bullet from a gun. By the way, I don't mean the metal kind of nails. I mean your fingernails and toenails. Don't worry, they grow back.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TuskAct2>())
                {
                    Main.npcChatText = "Tusk Act 2 has learned the power of the Golden Rectangle, and can now fire nail bullets that can be guided towards targets. You move closer to the Infinite Spin.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TuskAct3>())
                {
                    Main.npcChatText = "Act 3 allows you to fire a nail what creates a wormhole, which you can then travel through. This allows you to get the jump on enemies. Make sure you only fire if you have no doubts.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TuskAct4>())
                {
                    Main.npcChatText = "Tusk Act 4 has found the power of the Infinite Spin, and can cause enemies to... infinitely spin. Along with that, Act 4 can now manifest into a stronger form than the other three acts, allowing it to punch enemies. The catch is that you have to gain rotational energy using Slow Dancer though... But congratulations on obtaining such a power. I'm sure it was a long and roundabout path.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<StickyFingersT1>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<StickyFingersT2>())
                {
                    Main.npcChatText = "While zippers may seem like quite the mundane ability to have, Sticky Fingers is a stand that lets you use them to their absolute maximum potential. Figure out how to use the zippers, and your enemies will be saying Arrivederci, very quickly. \nAt first, Sticky Fingers isn't the most powerful of Stands though it does allow you to unzip your enemies as it punches. Keep it going, don't let your resolve waver, and power up further.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<StickyFingersT3>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<StickyFingersFinal>())
                {
                    Main.npcChatText = "Now, you can unzip your own arms to throw out an extended reach punch. Think of it as a flail, because your fists do have mass, remember. Ah, you can also now throw a zipper in the direction of this thing called a 'cursor' is and zip 30 tiles in that direction";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<SexPistolsT1>())
                {
                    Main.npcChatText = "Ah, good grief... this, well, these, are Sex Pistols. They're six, sentient, bullet-shaped stands that will stay around the area you want them to. Whenever a bullet goes near them, they will redirect that bullet to the nearest enemy. Sex Pistols never gained any new or extreme abilities, but each of your upgrades increases your guns power. More efficiency means more rounds out, and more bodies.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<SexPistolsT2>())
                {
                    Main.npcChatText = "The pistols should be able to kick bullets with even greater force now. Did I tell you that the Pistols need to eat?";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<SexPistolsT3>())
                {
                    Main.npcChatText = "Faster and faster the chamber goes, can you keep up long enough to keep firing? Pressing " + JoJoStands.SpecialHotKey.GetAssignedKeys()[0] + " will allow your pistols to go into a frenzy, where they would kick bullets until the end. ";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<SexPistolsFinal>())
                {
                    Main.npcChatText = "Sex Pistols has reached max power, they come back to you almost instantly now. Just remember to keep some food in your pockets...";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<KingCrimsonT1>())
                {
                    Main.npcChatText = "The power of King Crimson falls to you now. A powerful stand that strikes at the speed of a sledgehammer. Learn its speeds and take control of its immense power. \nRight now, King Crimson is only capable of striking opponents. This isn't at all a bad thing, because of how King Crimson attacks. King Crimson strikes with such ferocity that it has been shown to tear holes opponents in people, and the attacks have such force that you will fly forward when attacks come out.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<KingCrimsonT2>())
                {
                    Main.npcChatText = "King Crimson has acquired its signature ability of skipping time. It's a strange ability in concept, so here's a summary- you can remove the cause from the effect. Jump past time itself, confuse your enemies, and take your place at the apex of creation. And now that it can use its signurature ability, it can also perform 'Quick Timeskips' when needed, mainly while it's blocking enemies.";
                }
                else if ((mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<KingCrimsonT3>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<KingCrimsonFinal>()))
                {
                    Main.npcChatText = "Congrats, it seems you've unlocked the power of Epitaph. Epitaph is able to look into the future, so you can anticipate your enemies moves and prepare for them in advance.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<CenturyBoyT1>())
                {
                    Main.npcChatText = "This Stand has no weaknesses, defensively. Century Boy is the ultimate physical defense- when you activate it, the stand mounts itself on your body and you are impervious to all harm. This does have one minor weakness... you lose the ability to move. Do with that what you will.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<CenturyBoyT2>())
                {
                    Main.npcChatText = "Looks like you learned that starting a dynamite then going invincible hurts your enemies and not you. What was stopping you from doing this earlier?";
                }
                else if (player.HeldItem.type == ModContent.ItemType<DollyDaggerT1>())
                {
                    Main.npcChatText = "Intel from the SPW suggests that this stand merely reflects 35% of damage taken.";
                }
                else if (player.HeldItem.type == ModContent.ItemType<DollyDaggerT2>())
                {
                    Main.npcChatText = "Now that you've honed your Dolly Dagger, it can relflect about 70% of damage taken. Even I would have trouble fighting you with that amount of reflection. Oh, you can also stab yourself with the dagger to send that damage to the nearest enemy. That stand of yours has lots of potential.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<MagiciansRedT1>())
                {
                    Main.npcChatText = "Ah, Magician's Red. The original owner of this Stand was the man who taught me about Stands in the first place. You can manipulate fire and create Ankhs to burn your enemies to ash.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<MagiciansRedT2>())
                {
                    Main.npcChatText = "Magician's Red can now use Red Bind, a technique to restrain enemies with your fire. Don't ask me how that doesn't burn through them.";
                }
                else if ((mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<MagiciansRedT3>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<MagiciansRedFinal>()))
                {
                    Main.npcChatText = "Now, Magician's Red has relearned all of its abilities. With Crossfire Hurricane, you can launch a volley of Ankhs and overwhelm your enemies.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<AerosmithT1>())
                {
                    Main.npcChatText = "Aerosmith may appear to be nothing more than a model plane, but don't be fooled. It has potential to be extremely powerful and agile. Let loose a hellstorm of bullets and turn your foes into a pincushion.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<AerosmithT2>())
                {
                    Main.npcChatText = "Now, Aerosmith can drop bombs on your enemies. Light them up brighter than before!";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<AerosmithT3>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<AerosmithFinal>())
                {
                    Main.npcChatText = "With this, Aerosmith can now find enemies using a CO2 detecting radar. If it breathes, make sure it won't anymore.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TheHandT1>())
                {
                    Main.npcChatText = "Right now, The Hand is weak, only able to punch your opponents. It may be a good punch, but there's not much else right now.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TheHandT2>())
                {
                    Main.npcChatText = "The Hand can now use its signature ability... its right hand. You can use The Hand's hand to scrape away space itself and teleport short distances. The Hand can now cause enemies to, perhaps, lose a few organs.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TheHandT3>())
                {
                    Main.npcChatText = "With your increased mastery of The Hand, you can now pull enemies towards you by tapping " + JoJoStands.SpecialHotKey.GetAssignedKeys()[0] + ", and you can perform a very powerful attack with The Hand's Hand by holding" + JoJoStands.SpecialHotKey.GetAssignedKeys()[0] + ", given that they are in range.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TheHandFinal>())
                {
                    Main.npcChatText = "The Hand has reached its maximum potential, and is more powerful than ever. Of course, you seem to be smarter than its previous user. I'm not saying he was a bad kid, or anything. Wonder where he is now...";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<GratefulDeadT1>())
                {
                    Main.npcChatText = "Just seeing that Stand is making me feel older than I already am. That's the ability of Grateful Dead, so be careful using it around people you wanna keep around.\nRight now, Grateful Dead can only punch things. Not exactly unusual. Just keep going.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<GratefulDeadT2>())
                {
                    Main.npcChatText = "Now, Grateful Dead can grab a target and forcefully age them. The amount of time aged depends on how long you hold them. Don't do handshakes.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<GratefulDeadT3>())
                {
                    Main.npcChatText = "So, this Stand just became even more of a safety hazard to any allies, because now it can spread the aging effect as a mist. It's not as immediate, but now multiple people can be aged forwards at once.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<GratefulDeadFinal>())
                {
                    Main.npcChatText = "Finally, Grateful Dead is at maximum power. Just... stay a few feet away from me. I already have a couple of gray hairs.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<LockT1>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<LockT2>())
                {
                    Main.npcChatText = "Oh. The Lock. I feel bad that you have to use this one... wait a minute. The Lock has the ability to make others' hearts heavy with guilt, literally. Use it, and anything that harms you will start to feel that immense weight. Some things may be immune to that guilt, and it can be overcome.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<LockT3>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<LockFinal>())
                {
                    Main.npcChatText = "The Lock can now force others to feel guilty without any action against you... by harming yourself. Use this ability with caution.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<WhitesnakeT1>())
                {
                    Main.npcChatText = "Okay, you need to step away before I crush you. Get that Stand out of my sight. If you think I'm joking, come and find out.\n*sigh* Well, fine. Whitesnake, at this stage, can only throw punches. That's it.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<WhitesnakeT2>())
                {
                    Main.npcChatText = "Now, Whitesnake can secrete a gooey substance that damages enemies and sticks to surfaces. It's not a pleasant experience for those affected. You'll also notice that you can send Whitesnake farther away from you now, and that it has a gun. As you get stronger, that ability will help you out more and more, keep that in mind.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<WhitesnakeT3>())
                {
                    Main.npcChatText = "Whitesnake has unlocked the ability to steal discs from opponents. If your enemy isn't a Stand user, then they'll be devoid of thought for a few moments. If they are, then you'll disable their Stand temporarily- do all of this by pressing and holding " + JoJoStands.SpecialHotKey.GetAssignedKeys()[0];
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<WhitesnakeFinal>())
                {
                    Main.npcChatText = "Due to your annoying persistence, Whitesnake has reached its maximum potential. Now, keep it away from me, I mean it.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<SilverChariotT1>())
                {
                    Main.npcChatText = "Ah, Silver Chariot. Another Stand that belonged to a friend of mine, a fellow Stardust Crusader. You think my hair is ridiculous, you should have seen his. Anyways, Silver Chariot wields a rapier with lightning speed and deadly precision.\nAt the moment, Silver Chariot can only stab with its rapier. However underwhelming that may sound, the stabs come out extremely quick, almost invisible to the eye.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<SilverChariotT2>())
                {
                    Main.npcChatText = "On top of your stabbing flurry, Silver Chariot can now deflect projectiles back at your enemy with it's rapier, almost as if it's playing a game of baseball, or tennis.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<SilverChariotT3>())
                {
                    Main.npcChatText = "At this stage, Silver Chariot can now shed its armor to become so fast it can create afterimages. What this does for you is allows Silver Chariot to move significantly faster, but you also take more damage. Use this carefully.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<SilverChariotFinal>())
                {
                    Main.npcChatText = "Silver Chariot has reached the highest peak it can. The power of this stand alone rivals legendary swordsmen of old, and your toughest enemies. I wonder what would happen if this Stand were to change form or something.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<CreamT1>())
                {
                    Main.npcChatText = "That Stand makes me... nervous, and I cannot tell you why. Anyways, uh, Cream can harness the void and erase things, almost like that blockhead friend of Josuke's.\nJust like the rest, Cream cannot do very much at this stage. It can throw a mean chop though, and it can cause enemies to lose their organs. Literally.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<CreamT2>())
                {
                    Main.npcChatText = "Cream has gained the ability to, well, eat you. Specifically, you can enter the void by pressing " + JoJoStands.SpecialHotKey.GetAssignedKeys()[0] + " and riding in Cream like an interdimensional ball of sorts. To control Cream in this state, use your 'left-click' to guide it. The downsides are that you have a limited time in there, indicated by the Void Gauge, and you are effectively blind while in the void, so know where your target is beforehand. Once that time runs out, Cream pukes you out. I am being serious.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<CreamT3>())
                {
                    Main.npcChatText = "As a way to quickly refill your Void Gauge, Cream can consume you and fly about. On top of that, your void offensive now lasts longer, and once it runs out, Cream automatically enters that defensive recharge state instead of being puked out.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<CreamFinal>())
                {
                    Main.npcChatText = "At this final point, Cream can stay in the void for even longer. Once it runs out, you will still enter the recharge state rather than being puked out. Actually, I wonder if you could use this to fire yourself like a cannon... questions for another day.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<HermitPurpleT1>())
                {
                    Main.npcChatText = "Never in my life did I think I would see another Stand user with Hermit Purple. That Stand is special because the last owner I knew, my gramps, was a Hamon user as well, which makes it unique in that he used both in tandem. I wonder, do you have that ability?\nAt the moment, Hermit Purple is nothing more than a mass of vines. You can certainly hit people with them. It will certainly be more effective than getting mauled.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<HermitPurpleT2>())
                {
                    Main.npcChatText = "Well, it seems my initial question has been answered. Hermit Purple coils around you, and anything that opts to attack you will be met with quite the burn. On top of that, now you can grab and pull enemies into their eventual doom by 'clicking and holding right-click'.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<HermitPurpleT3>())
                {
                    Main.npcChatText = "Your connection to Hermit Purple grows like rampant vines; You can now press " + JoJoStands.SpecialHotKey.GetAssignedKeys()[0] + " to overcharge Hermit Purple with Hamon and power up your passive counter. The next three things that hit you will not only feel the burn, but they will take damage and get knocked away.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<HermitPurpleFinal>())
                {
                    Main.npcChatText = "Hermit Purple is as strong as it will ever be, thanks to you. I know that the old man would never have come close, as long as he lived. I still have no idea how he managed to survive as long as he did.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<BadCompanyT1>())
                {
                    Main.npcChatText = "That Stand... I've never seen it before, though I do remember some descriptions of a Stand like this from Josuke a while back.\nAccording to Josuke, that Stand was able to summon armies of soldiers, tanks, and attack helicopters, though I'd imagine that at this stage, soldiers would be all you could handle. I'll have to ask Josuke for greater details on this Stand.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<BadCompanyT2>())
                {
                    Main.npcChatText = "Your Stand, Bad Company, should now be able to summon more troops, as well as summon tanks. Your troops should also be much stronger now that they've gained combat experience.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<BadCompanyT3>())
                {
                    Main.npcChatText = "According to Josuke, by now, you should be able to summon attack helicopters. Those helicopters would be your greatest asset, but don't depend on them too much. This is just a guess but, if you can summon attack helicopters, what's stopping you from summoning war planes?";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<BadCompanyFinal>())
                {
                    Main.npcChatText = "This Stand is surely something to be afraid of. You now have a small militia under your command, with soldiers, tanks, attack helicopters, and war planes under your command. It amazes me that Josuke was able to survive that battle.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<StoneFreeT1>())
                {
                    Main.npcChatText = "That's... my daughter's stand. Its power allows you to turn it into string. While that may sound very mundane or useless, trust me when I say that it is no joke.\nAt this point, Stone Free has not gained the ability to do much besides punching, but it's quite the punch, if I do say so myself.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<StoneFreeT2>())
                {
                    Main.npcChatText = "Stone Free has started to tap into its unraveling abilities. This one, specifically, allows you to weave a trap between two tiles for any unsuspecting foes. It may not seem like much now, but when used properly it can be an incredibly deadly weapon.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<StoneFreeT3>())
                {
                    Main.npcChatText = "Your strings have strengthened considerably. Now, you can use Stone Free's threads to bind enemies, stopping them in their tracks. Who knows, you may be able to bind them and drag them to you as well.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<StoneFreeFinal>())
                {
                    Main.npcChatText = "This final power-up from Stone Free allows you to weave Stone Free's thread into a strong shield to protect yourself from harm. Now, go forth and spin a legendary tale with this power!";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<SoftAndWetT1>())
                {
                    Main.npcChatText = "Did you let a bubble loose in here? Be careful not to let it pop near me. These things are capable of taking something from whatever they pop next to. I'd prefer to keep my eyesight.\nThey also seem to take effects from things you hold in your hand. I wonder what would happen if these sorts of bubbles popped.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<SoftAndWetT2>())
                {
                    Main.npcChatText = "Hmm. Seems these bubbles are more durable than I thought. You should be able to coat yourself in one, giving you some welcome protection. This Stand sure is full of surprises.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<SoftAndWetT3>())
                {
                    Main.npcChatText = "Soft & Wet can now create bubbles while in the heat of battle. They seem to do a great deal of damage on impact, making you quite the threat.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<SoftAndWetFinal>())
                {
                    Main.npcChatText = "This power! It can't be. It seems the bubbles can now be used to plant bombs in the ground. This kind of power seems unsettlingly familiar. We'll chalk this up to coincidence. For now.";
                }
                else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<SoftAndWetGoBeyond>())
                {
                    Main.npcChatText = "My suspicions are all but confirmed. I knew this explosive power seemed familiar. But it's changed. Evolved. It 'goes beyond' nature.\nHow do I explain this? The short of it is that your bubbles are actually a line spinning at incredible speeds. If this line reaches a vanishing point, there’s no telling what your bubbles can achieve. They could transcend logic, itself. How terrifying…";
                }

                if (mPlayer.StandSlot.SlotItem.IsAir)
                {
                    int helpText = Main.rand.Next(0, 6 + 1);
                    switch (helpText)
                    {
                        case 0:
                            Main.npcChatText = "Have a stand Item in the 'Stand Slot' then press 'Stand Help' and I'll tell you what it can do and how to get it's tiers OR keep pressing 'Stand Help' and I'll tell you how to get specific items and do certain actions.";
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
                        player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<ArrowShard>());
                        mPlayer.receivedArrowShard = true;
                    }
                }
            }
        }

        public override string GetChat()       //Allows you to give this town NPC a chat message when a player talks to it.
        {
            switch (Main.rand.Next(4))    //this are the messages when you talk to the NPC, if you want to add more cases, you first need to change the Main.rand to the number of cases (default is included!)
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
            UserIsAlive = true;
            return true;
        }

        public override bool CheckDead()
        {
            UserIsAlive = false;
            return true;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)//  Allows you to determine the damage and knockback of this town NPC attack
        {
            if (!Main.hardMode)
            {
                standDamage = 40;  //NPC damage
                knockback = 2f;   //NPC knockback
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

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)//Allows you to determine the Projectile type of this town NPC's attack, and how long it takes for the Projectile to actually appear
        {
            if (!Projectiles.NPCStands.StarPlatinumNPCStand.SPActive)
            {
                projType = ModContent.ProjectileType<StarPlatinumNPCStand>();
                attackDelay = 1;
            }
        }
    }
}