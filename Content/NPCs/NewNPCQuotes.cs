using Terraria;
using Terraria.ID;
using Terraria.ModLoader;



namespace MurphysMod.Content.NPCs
{
    public class NewNPCQuotes : GlobalNPC
    {
        public override void GetChat(NPC npc, ref string chat)
        {
            if (npc.type == NPCID.Wizard)
            {
                string[] newQuotes = {"Ew, Rune Wizards.", "You've done WHAT?", "I too have once angered the gods. Turns out, Stevari is quite chill.",
                "My apprentice once said something wise. Only once though.", "Did I ever tell you how my apprentice once casted lightning underwater?", "I sometimes miss my late apprentice... Sometimes.",
                "One of these days I will figure out how to cast a nuke. Maybe then the shadow government will sponser me.", "Please leave, I must ponder my orb.", "May I ponder your orb? No? Okay.",
                "I seriously do not understand how you managed to get yourself cursed. Do you know nothing of the arcane?"};

                if (Main.rand.NextBool(3))
                {
                    chat = newQuotes[Main.rand.Next(0, newQuotes.Length)];
                }

            }

            if(npc.type == NPCID.TownSlimePurple)
            {
                if(Main.rand.NextBool(100))
                    chat = "Ough my head hurts... Uh I mean *glorp glorp*";
            }

              if (npc.type == NPCID.TravellingMerchant)
            {
                string[] newQuotes = {"God, I have the fit ON.", "The merchant cannot handle my style.", "Woah there buddy, I don't have that in my stock right now.",
                "Please tell me you aren't broke right now.", "Oh yeah, I can set up a payment plan for you.", "Do you want to see my watch?", "I am really bad at keeping track of time.",
                "I'll be back in about three to seven business months.", "Imagine being foolish enough to invest in dirt blocks.", "Hey, I've got places to be, please hurry it up."};

                if (Main.rand.NextBool(3))
                {
                    chat = newQuotes[Main.rand.Next(0, newQuotes.Length)];
                }
            }

            if (npc.type == NPCID.Guide)
            {
                string[] newQuotes = {"I dunno dude, just check the wiki.", "All of these people with curses, and I have it the worst.", "Have you considered getting into dollmaking?",
                "I once saw the witch doctor with a little version of me. I wonder what he wants.", "Ting tang, walla walla bing bang. Why did I say that?", "The painter once painted me ugly.",
                "The painter insists on using 'zesty pine sap green' when painting the forest. Green is green my guy.", "I don't want a lava bath, thanks.", "Why do I feel like I am constantly hanging upside down?",
                "I really hope the painter doesn't take one of my ears next."};

                if (Main.rand.NextBool(3))
                {
                    chat = newQuotes[Main.rand.Next(0, newQuotes.Length)];
                }
            }

            if (npc.type == NPCID.Merchant)
            {
                string[] newQuotes = {"I cannot express how dirty my dirt blocks are.", "Hello, I like money.",
                "The tax collecter is quite fond of me. I do not feel the same. I sell piggy banks, I am not one.", "I do not sell illegal wares... Not today, at least.",
                "Kosh, kapleck Mog. Haha, mog.", "The dirt bubble is about to pop, I need to liquidate my stock.", "Stop asking about angel statues.", "I have yet to see someone purchase a copper pickaxe from me.",
                "Stop saying the travelling merchant wears nicer clothes than me.", "I am so old and wealthy."};

                if (Main.rand.NextBool(3))
                {
                    chat = newQuotes[Main.rand.Next(0, newQuotes.Length)];
                }
            }

            if (npc.type == NPCID.WitchDoctor)
            {
                string[] newQuotes = {"Ooh, eee, ooh ah ah...", "I found this cool doll, would you like to see it?", "I do not care for mushrooms. No, they are not fun guys.",
                "Please do not pull on my tail, it WILL fall off.", "I am so glad you took care of that bee. I am allergic.",
                "I didn't study the arcane arts just for you to call me by my first name. I am the witch DOCTOR.", "Please do not drink the water in the jungle. I learned that one the hard way.",
                "I do not give romantic advice.", "Have you tried using a flask?", "Which witch is which?", "Who is Alvin?"};

                if (Main.rand.NextBool(3))
                {
                    chat = newQuotes[Main.rand.Next(0, newQuotes.Length)];
                }
            }

            if (npc.type == NPCID.Golfer)
            {
                string[] newQuotes = {"I need a new pair of socks, I got a Hole in One.", "Yes, of course tigers live in the woods.", "I hate it when the merchant is on the turf. Does he even know drive a golf cart?",
                "I'm not sure why everyone hates the angler. He's just a kid y'know.", "I've been on this planet for some 40-odd years, and yet my sister still insists on talking like... That.",
                "I wonder if my clubs can be reset.", "You better not shoot your golf balls down your hellavator. Have some self-respect.", "I'm not naming names, but someone once shot my golf gear out of a cannon.",
                "Please use the lawn mower for its intended purpose, and not as a weapon.", "'Put-ter' there."};

                if (Main.rand.NextBool(3))
                {
                    chat = newQuotes[Main.rand.Next(0, newQuotes.Length)];
                }
            }

            if (npc.type == NPCID.Demolitionist)
            {
                string[] newQuotes = {"Goblins have an inherent evil to them. Sometimes, it takes time to see it.",
                "That tinkerer fellow charged me to reforge my dynamite, told me that it cannot be reforged, and then refused to give me a refund.",
                "Yup, that one is going into the book.", "No, I do not know who Karl is.", "Losing is fun? Only if it is in fuse chicken, and only when it is you.",
                "Myself and the bartender have an ongoing feud to see who can out drink who.", "If you purify the world instead of blowing it up, you're going into the book.",
                "Bees in grenades? Sign me up!", "Bugs give me the heebie jeebies."};

                if (Main.rand.NextBool(3))
                {
                    chat = newQuotes[Main.rand.Next(0, newQuotes.Length)];
                }
            }
        }
    }
}
