using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs
{
    public class JackTheRipper : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 40;
            npc.height = 48;
            npc.defense = 24;
            npc.lifeMax = 400;
            npc.damage = 80;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 1f;
            npc.chaseable = true;
            npc.noGravity = false;
            npc.daybreak = true;
            npc.aiStyle = 3;
            aiType = 73;
        }
        private int frame = 0;
        private int dushtime = 0;
        private int run = 200;
        private int knifescooldown = 200;
        private int knifescooldown2 = 0;
        private int dushcooldown = 0;
        private int random = 0;
        private int localjack = 0;
        public bool knifes = false;
        public bool knifes2 = false;
        public bool knifes3 = false;
        public bool dush = false;
        public bool dush2 = false;
        public bool hide = false;
        public bool hide2 = false;
        public override void AI()
        {
            if (random == 0)
            {
                random = Main.rand.Next(1, 999999);
            }
            if (localjack == 0)
            {
                localjack = random;
            }
            knifescooldown -= 1;
            dushcooldown -= 1;
            Player target = Main.player[npc.target];
            if (target.dead || npc.target == -1)
            {
                npc.TargetClosest();
            }
            if (npc.life > npc.lifeMax)
            {
                npc.life = npc.lifeMax;
            }
            if (hide)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC otherNPCs = Main.npc[n];
                    if (otherNPCs.townNPC)
                    {
                        if (otherNPCs.GetGlobalNPC<JoJoGlobalNPC>().jackcatch)
                        {
                            if (localjack == otherNPCs.GetGlobalNPC<JoJoGlobalNPC>().jackvictim)
                            {
                                if (otherNPCs.active)
                                {
                                    npc.life = npc.lifeMax;
                                    npc.Center = otherNPCs.Center;
                                    npc.aiStyle = 0;
                                    npc.damage = 0;
                                    npc.immortal = true;
                                    npc.hide = true;
                                    if (otherNPCs.Distance(target.Center) <= 200f && localjack == otherNPCs.GetGlobalNPC<JoJoGlobalNPC>().jackvictim)
                                    {
                                        otherNPCs.AddBuff(mod.BuffType("Dead"), 2);
                                    }
                                }
                                if (!otherNPCs.active)
                                {
                                    if (localjack == otherNPCs.GetGlobalNPC<JoJoGlobalNPC>().jackvictim)
                                    {
                                        hide = false;
                                        hide2 = true;
                                        otherNPCs.GetGlobalNPC<JoJoGlobalNPC>().jackvictim = 0;
                                        localjack = 0;
                                        otherNPCs.GetGlobalNPC<JoJoGlobalNPC>().jackcatch = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!hide)
            {
                npc.immortal = false;
                npc.hide = false;
                npc.AddBuff(mod.BuffType("Vampire"), 2);
                if (!npc.HasBuff(mod.BuffType("Sunburn")))
                {
                    npc.defense = 24;
                    npc.damage = 80;
                }
                if (npc.HasBuff(mod.BuffType("Sunburn")) && !hide)
                {
                    npc.defense = 0;
                    npc.damage = 40;
                }
                {
                    if (npc.life <= npc.lifeMax / 2 && !npc.HasBuff(mod.BuffType("Sunburn")))
                    {
                        Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Blood, npc.velocity.X * -0.5f, npc.velocity.Y * -0.5f);
                        run -= 1;
                        if (npc.position.X - 200 >= target.position.X && run <= 0 && !knifes && !dush)
                        {
                            npc.direction = 1;
                            npc.velocity.X = 2;
                        }
                        if (npc.position.X + 200 < target.position.X && run <= 0 && !knifes && !dush)
                        {
                            npc.direction = -1;
                            npc.velocity.X = -2;
                        }
                        if (npc.position.X >= target.position.X && run > 0 && !knifes && !dush)
                        {
                            npc.direction = 1;
                            npc.velocity.X = 2;
                        }
                        if (npc.position.X < target.position.X && run > 0 && !knifes && !dush)
                        {
                            npc.direction = -1;
                            npc.velocity.X = -2;
                        }
                        if (!npc.noTileCollide && npc.Distance(target.Center) <= 200f && dushcooldown <= 0 && !knifes && run <= 0 && !target.dead)
                        {
                            dush = true;
                        }
                        if (dush && !dush2)
                        {
                            npc.knockBackResist = 0f;
                            dushtime += 1;
                            npc.aiStyle = 3;
                            aiType = 199;
                            if (dushtime == 200)
                            {
                                dushtime = 0;
                                dushcooldown = 800;
                                dush = false;
                            }
                            if (npc.position.X >= target.position.X)
                            {
                                npc.direction = -1;
                                npc.velocity.X = -4;
                            }
                            if (npc.position.X < target.position.X)
                            {
                                npc.direction = +1;
                                npc.velocity.X = +4;
                            }
                        }
                        if (dush2)
                        {
                            Player player = target;
                            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
                            if (player.mount.Type != 0)
                            {
                                player.mount.Dismount(player);
                            }
                            player.AddBuff(mod.BuffType("MissingOrgans2"), 2);
                            player.npcTypeNoAggro[mod.NPCType("JackTheRipper")] = true;
                            npc.knockBackResist = 0f;
                            npc.damage = 0;
                            player.position.X = npc.position.X;
                            player.position.Y = npc.position.Y;
                            npc.aiStyle = 0;
                            if (target.dead)
                            {
                                dushtime = 0;
                                dushcooldown = 800;
                                dush = false;
                                dush2 = false;
                            }
                        }
                    }
                    if (!npc.noTileCollide && npc.Distance(target.Center) <= 400f && knifescooldown <= 0 && !dush && !target.dead)
                    {
                        knifes = true;
                    }
                    if (knifes)
                    {
                        npc.aiStyle = 0;
                        npc.velocity.X = 0;
                        frame = 3;
                        knifescooldown2 += 1;
                        Vector2 shootVel = target.Center - npc.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 0f);
                        }
                        if (knifescooldown2 == 30)
                        {
                            knifes3 = true;
                        }
                        if (knifescooldown2 == 40)
                        {
                            knifes2 = true;
                        }
                        if (knifescooldown2 == 50)
                        {
                            knifes3 = true;
                        }
                        if (knifescooldown2 == 60)
                        {
                            knifes2 = true;
                        }
                        if (knifescooldown2 == 90)
                        {
                            knifescooldown2 = 0;
                            knifescooldown = 600;
                            knifes = false;
                        }
                        if (knifes2)
                        {
                            shootVel.Normalize();
                            shootVel *= 40f;
                            knifescooldown2 += 1;
                            float rotationk = MathHelper.ToRadians(3);
                            float numberKnives = 3;
                            for (int i = 0; i < numberKnives; i++)
                            {
                                Vector2 perturbedSpeed = new Vector2(shootVel.X, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotationk, rotationk, i / (numberKnives - 1))) * .2f;
                                int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("JackKnife"), (int)(npc.damage / 4), 2f);
                                Main.projectile[proj].netUpdate = true;
                                npc.netUpdate = true;
                            }
                            knifes2 = false;
                        }
                        if (knifes3)
                        {
                            shootVel.Normalize();
                            shootVel *= 40f;
                            float rotationk = MathHelper.ToRadians(3);
                            float numberKnives = 2;
                            for (int i = 0; i < numberKnives; i++)
                            {
                                Vector2 perturbedSpeed = new Vector2(shootVel.X, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotationk, rotationk, i / (numberKnives - 1))) * .2f;
                                int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("JackKnife"), (int)(npc.damage / 4), 2f);
                                Main.projectile[proj].netUpdate = true;
                                npc.netUpdate = true;
                            }
                            knifes3 = false;
                        }
                        if (npc.position.X >= target.position.X)
                        {
                            npc.direction = 1;
                        }
                        if (npc.position.X < target.position.X)
                        {
                            npc.direction = -1;
                        }
                    }
                    if (npc.velocity.X > 0)
                    {
                        npc.spriteDirection = 1;
                    }
                    if (npc.velocity.X < 0)
                    {
                        npc.spriteDirection = -1;
                    }
                    if (!knifes && !dush && !hide)
                    {
                        npc.knockBackResist = 1f;
                        npc.aiStyle = 3;
                        aiType = 73;
                    }
                }
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (npc.life < npc.lifeMax && !hide)
            {
                int vampirism = damage / 2;
                npc.life += vampirism;
            }
            if (dush && !target.HasBuff(mod.BuffType("MissingOrgans2")) && !target.immune)
            {
                dush2 = true;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[npc.target]; 
            if (target.townNPC && npc.life > npc.lifeMax / 2 && !hide && !hide2 && !target.GetGlobalNPC<JoJoGlobalNPC>().jackcatch && npc.Distance(player.Center) >= 400f)
            {
                target.GetGlobalNPC<JoJoGlobalNPC>().jackvictim = random;
                target.GetGlobalNPC<JoJoGlobalNPC>().jackcatch = true;
                hide = true;
            }
            if (npc.life < npc.lifeMax && !hide)
            {
                int vampirism = damage / 2;
                npc.life += vampirism;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            frameHeight = 48;
            if (npc.velocity != Vector2.Zero)
            {
                npc.frameCounter++;
                if (npc.frameCounter >= 10)
                {
                    frame += 1;
                    npc.frameCounter = 0;
                }
                if (frame >= 3)
                {
                    frame = 0;
                }
            }
            npc.frame.Y = frame * frameHeight;
        }
    }
}