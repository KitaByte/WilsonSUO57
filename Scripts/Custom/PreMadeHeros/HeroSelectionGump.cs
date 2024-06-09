using Server.Misc;
using System.Linq;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    public class HeroSelectionGump : BaseGump
    {
        private IHero _Hero;

        private readonly HeroDeed _Deed;

        public HeroSelectionGump(PlayerMobile user, HeroDeed deed) : base(user, 50, 50, null)
        {
            _Deed = deed;

            user.SendMessage(53, "Pick a 'Hero' you wish to play!");
        }

        public override void AddGumpLayout()
        {
            Dragable = true;
            Closable = false;
            Resizable = false;

            _Hero = null;

            string name = ServerList.ServerName;

            int nameLength = name.ToCharArray().Count();

            if (nameLength > 9)
            {
                nameLength = (nameLength - 9) * 6;
            }
            else
            {
                nameLength = 0;
            }

            AddBackground(X, Y, 175 + nameLength, 500, 40000);

            AddLabel(X + 25, Y + 12, 53, $"Hero's of {name}");

            // Alchemist
            AddButton(X + 25, Y + 49, 2361, 2360, 1, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 45, 1153, "Alchemist");

            // Archer
            AddButton(X + 25, Y + 69, 2361, 2360, 2, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 65, 1153, "Archer");

            // Bard
            AddButton(X + 25, Y + 89, 2361, 2360, 3, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 85, 1153, "Bard");

            // Bard/Warrior
            AddButton(X + 25, Y + 109, 2361, 2360, 4, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 105, 1153, "Bard/Warrior");

            // Craftsman
            AddButton(X + 25, Y + 129, 2361, 2360, 5, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 125, 1153, "Craftsman");

            // Fence/Poisoner
            AddButton(X + 25, Y + 149, 2361, 2360, 6, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 145, 1153, "Fence/Poisoner");

            // Mage/Archer
            AddButton(X + 25, Y + 169, 2361, 2360, 7, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 165, 1153, "Mage/Archer");

            // Mage/Bard
            AddButton(X + 25, Y + 189, 2361, 2360, 8, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 185, 1153, "Mage/Bard");

            // Mage/Tamer
            AddButton(X + 25, Y + 209, 2361, 2360, 9, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 205, 1153, "Mage/Tamer");

            // Mage/Theif
            AddButton(X + 25, Y + 229, 2361, 2360, 10, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 225, 1153, "Mage/Thief");

            // Necro/Mage
            AddButton(X + 25, Y + 249, 2361, 2360, 11, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 245, 1153, "Necro/Mage");

            // Necromancer
            AddButton(X + 25, Y + 269, 2361, 2360, 12, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 265, 1153, "Necromancer");

            // Necro/Warrior
            AddButton(X + 25, Y + 289, 2361, 2360, 13, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 285, 1153, "Necro/Warrior");

            // Ninja
            AddButton(X + 25, Y + 309, 2361, 2360, 14, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 305, 1153, "Ninja");

            // Ninja/Archer
            AddButton(X + 25, Y + 329, 2361, 2360, 15, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 325, 1153, "Ninja/Archer");

            // Paladin
            AddButton(X + 25, Y + 349, 2361, 2360, 16, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 345, 1153, "Paladin");

            // Samurai
            AddButton(X + 25, Y + 369, 2361, 2360, 17, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 365, 1153, "Samurai");

            // Theif
            AddButton(X + 25, Y + 389, 2361, 2360, 18, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 385, 1153, "Thief");

            // Treasure Hunter
            AddButton(X + 25, Y + 409, 2361, 2360, 19, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 405, 1153, "Treasure Hunter");

            // Warrior
            AddButton(X + 25, Y + 429, 2361, 2360, 20, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 425, 1153, "Warrior");

            // Warrior/Mage
            AddButton(X + 25, Y + 449, 2361, 2360, 21, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 445, 1153, "Warrior/Mage");

            // Last Button
            AddButton(X + 25, Y + 469, 2360, 2361, 0, GumpButtonType.Reply, 0);
            AddLabel(X + 50, Y + 465, 53, "Cancel");
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID == 0)
            {
                Close();
            }
            else
            {
                switch (info.ButtonID)
                {
                    case 1:
                        {
                            _Hero = new Alchemist();

                            break;
                        }

                    case 2:
                        {
                            _Hero = new Archer();

                            break;
                        }

                    case 3:
                        {
                            _Hero = new Bard();

                            break;
                        }

                    case 4:
                        {
                            _Hero = new BardWarrior();

                            break;
                        }

                    case 5:
                        {
                            _Hero = new Craftsman();

                            break;
                        }

                    case 6:
                        {
                            _Hero = new FencerPoisoner();

                            break;
                        }

                    case 7:
                        {
                            _Hero = new MageArcher();

                            break;
                        }

                    case 8:
                        {
                            _Hero = new MageBard();

                            break;
                        }

                    case 9:
                        {
                            _Hero = new MageTamer();

                            break;
                        }

                    case 10:
                        {
                            _Hero = new MageThief();

                            break;
                        }

                    case 11:
                        {
                            _Hero = new NecroMage();

                            break;
                        }

                    case 12:
                        {
                            _Hero = new Necromancer();

                            break;
                        }

                    case 13:
                        {
                            _Hero = new NecroWarrior();

                            break;
                        }

                    case 14:
                        {
                            _Hero = new Ninja();

                            break;
                        }

                    case 15:
                        {
                            _Hero = new NinjaArcher();

                            break;
                        }

                    case 16:
                        {
                            _Hero = new Paladin();

                            break;
                        }

                    case 17:
                        {
                            _Hero = new Samurai();

                            break;
                        }

                    case 18:
                        {
                            _Hero = new Thief();

                            break;
                        }

                    case 19:
                        {
                            _Hero = new TreasureHunter();

                            break;
                        }

                    case 20:
                        {
                            _Hero = new Warrior();

                            break;
                        }

                    case 21:
                        {
                            _Hero = new WarriorMage();

                            break;
                        }
                }

                if (_Hero != null)
                {
                    _Hero.ApplyHero(User);

                    _Deed?.Delete();
                }

                Close();
            }
        }
    }
}
