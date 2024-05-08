using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom
{
    internal class WilsonDev : BaseCreature
    {
        public override bool IsInvulnerable => true;

        public override bool ShowFameTitle => true;

        [Constructable]
        public WilsonDev() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Wilson";

            Title = "the Code Weaver";

            InitStats(125, 125, 125);

            Body = 0x190;

            Hue = 0x4001;

            HairItemID = 0x203C;

            FacialHairItemID = 0x203E;

            SetDefaultHairHue();

            SpeechHue = Utility.RandomBrightHue();

            AddItem(new ElvenGlasses() { Hue = 2734 });

            AddItem(new BoneGloves() { Hue = 2734 });

            AddItem(new GnarledStaff() { Hue = 2734 });

            AddItem(new BirdsofBritanniaTalisman() { Hue = 2734 });

            AddItem(new GoldNecklace() { Hue = 2734 });

            AddItem(new GoldBracelet() { Hue = 2734 });

            AddItem(new GoldRing() { Hue = 2734 });

            AddItem(new HalfApron(2734));

            AddItem(new BodySash(2734));

            AddItem(new Sandals(2734));

            Container pack = new Backpack
            {
                Movable = false,
                Hue = 2734
            };

            AddItem(pack);

            Blessed = true;

            YellowHealthbar = true;

            Karma = 20000;

            Fame = 20000;

            CantWalk = true;
        }

        public WilsonDev(Serial serial) : base(serial)
        {
        }

        private DateTime lastLooked = DateTime.MinValue;

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (lastLooked < DateTime.Now - TimeSpan.FromSeconds(5))
            {
                lastLooked = DateTime.Now;

                if (m is PlayerMobile pm && InRange(pm.Location, 10))
                {
                    Direction = GetDirectionTo(pm.Location);

                    if (Utility.RandomDouble() < 0.01)
                    {
                        Say($"Hi {pm.Name}, it's great day for a GIFT!");
                    }
                }
            }

            base.OnMovement(m, oldLocation);
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            return true;  
        }

        private DateTime lastSpeech = DateTime.MinValue;

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (lastSpeech < DateTime.Now - TimeSpan.FromSeconds(3))
            {
                lastSpeech = DateTime.Now;
            }
            else
            {
                return;
            }

            if (e.Mobile is PlayerMobile pm && InRange(pm.Location, 3))
            {
                switch (e.Speech.ToLower())
                {
                    case "hi":
                        {
                            Direction = GetDirectionTo(e.Mobile.Location);

                            if (Utility.RandomDouble() < 0.1)
                            {
                                Say($"Hi {pm.Name}, are you here for a GIFT?");
                            }
                            else
                            {
                                Say($"Hi {pm.Name}, how can I HELP you today?");
                            }

                            break;
                        }

                    case "help":
                        {
                            Direction = GetDirectionTo(e.Mobile.Location);

                            Say($"Certainly {pm.Name}, you can contact me here!");

                            pm.SendMessage(pm.HairHue, "You were sent a magical link, opening browser...");

                            Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
                            {
                                pm.LaunchBrowser("https://www.uoopenai.com/wilson");
                            });

                            break;
                        }

                    case "gift":
                        {
                            Direction = GetDirectionTo(e.Mobile.Location);

                            Say($"Certainly {pm.Name}, here is some magic for you!");

                            if (pm.Criminal || pm.Murderer)
                            {
                                GiveNegGift(pm);
                            }
                            else
                            {
                                GivePosGift(pm);
                            }

                            break;
                        }
                }
            }
        }

        private void GiveNegGift(PlayerMobile pm)
        {
            pm.BoltEffect(Utility.RandomBrightHue());

            pm.Hits = 1;

            pm.Mana = 1;

            pm.Stam = 1;

            pm.BAC = 60;

            if (pm.Murderer && Utility.RandomDouble() < 0.5)
            {
                pm.Kill();

                pm.SendMessage(42, "You were killed by the magic!");
            }
            else
            {
                pm.SendMessage(42, "You were damaged by the magic!");
            }
        }

        private void GivePosGift(PlayerMobile pm)
        {
            int orgHairHue = pm.HairHue;

            int orgFacialHairHue = pm.FacialHairHue;

            pm.HairHue = Utility.RandomBrightHue();

            if (!pm.Female)
            {
                pm.FacialHairHue = pm.HairHue;
            }

            PlayGiftEffect(pm.Location, pm.Map);

            Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
            {
                PlayGiftEffect(pm.Location, pm.Map);

                pm.SendMessage(pm.HairHue, "You feel a magical surge run through your body!");

                pm.Hits = pm.HitsMax;

                pm.Mana = pm.ManaMax;

                pm.Stam = pm.StamMax;

                pm.HairHue = orgHairHue;

                if (!pm.Female)
                {
                    pm.FacialHairHue = orgFacialHairHue;
                }
            });
        }

        public override void OnAfterMove(Point3D oldLocation)
        {
            UpdateHairHue(Utility.RandomBrightHue());

            base.OnAfterMove(oldLocation);
        }

        private void UpdateHairHue(int hue)
        {
            if (Utility.RandomDouble() < 0.1)
            {
                HairHue = hue;

                FacialHairHue = HairHue;

                PlayGiftEffect(Location, Map);
            }
            else
            {
                SetDefaultHairHue();
            }
        }

        private void SetDefaultHairHue()
        {
            HairHue = 1153;

            FacialHairHue = HairHue;
        }

        private void PlayGiftEffect(Point3D loc, Map map)
        {
            Effects.PlaySound(loc, map, Utility.RandomList(0x3E, 0x3F));

            Effects.SendLocationEffect(loc, map, 0x9F89, 18, Utility.RandomBrightHue(), 0);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}
