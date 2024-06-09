using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.DealOrNoDeal
{
    public class HowieMandel : BaseCreature
    {
        private const int costToPlay = 5000;

        private const int roundsToPlay = 10;

        private PlayerMobile currentPlayer;

        private int currentOffer = 0;

        private int currentPot = 0;

        private int dealModifier = roundsToPlay;

        [Constructable]
        public HowieMandel() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
        {
            InitStats(31, 41, 51);

            Title = "the Dealer";

            Name = "Howie Mandel";

            Body = 0x190;

            SpeechHue = Utility.RandomBrightHue();

            Hue = Utility.RandomSkinHue();

            FacialHairItemID = 0x2040;

            FacialHairHue = 2500;

            CantWalk = true;

            AddItem(new LongPants(1175));

            AddItem(new FancyShirt(2500));

            AddItem(new Boots(1175));

            Container pack = new Backpack
            {
                Movable = false
            };

            AddItem(pack);
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            return from is PlayerMobile;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm)
            {
                if (pm.InRange(Location, 3) && pm.Alive)
                {
                    if (currentPlayer == null)
                    {
                        if (e.Speech.ToLower() == "play")
                        {
                            SayTo(pm, true, $"Drop {costToPlay} gold on me to play, Deal or No Deal!");

                            pm.SendAsciiMessage(43, "Say 'Deal' to accept banks offer!");
                            pm.SendAsciiMessage(43, "Say 'No Deal' or drop gold in the amount of the bank offer to continue!");
                            pm.SendAsciiMessage(43, "After {roundsToPlay} deals, you might beat the bank and win big!");
                            pm.SendAsciiMessage(43, "Or the bank will win and you'll walk away with less gold!");
                        }
                        else
                        {
                            SayTo(pm, true, $"Want to 'play' a game of Deal or No Deal?");
                        }
                    }
                    else
                    {
                        if (pm == currentPlayer)
                        {
                            switch (e.Speech.ToLower())
                            {
                                case "deal":
                                    {
                                        SayTo(currentPlayer, true, $"You accept the banks offer of {currentOffer} gold!");

                                        GivePrize(currentOffer);

                                        ResetGame();

                                        break;
                                    }

                                case "no deal":
                                    {
                                        SayTo(currentPlayer, true, $"Drop {currentOffer} gold on me to continue!");

                                        break;
                                    }

                                default:
                                    {
                                        SayTo(currentPlayer, true, "Say 'deal' to accept or 'no deal' to continue.");

                                        break;
                                    }
                            }
                        }
                        else
                        {
                            pm.SendAsciiMessage(53, "Game in progress, please ask again, after it ends!");
                        }
                    }
                }
            }

            base.OnSpeech(e);
        }

        public override bool CheckGold(Mobile from, Item dropped)
        {
            if (dropped is Gold gold)
            {
                if (Utility.RandomDouble() < 0.5)
                {
                    PlaySound(Utility.RandomList(0x30C, 0x41B));
                }

                if (currentPlayer == null)
                {
                    if (gold.Amount >= costToPlay)
                    {
                        currentPlayer = from as PlayerMobile;

                        PlaySound(0x682);

                        UpdateOffer(gold);

                        return true;
                    }
                }
                else if (from == currentPlayer)
                {
                    if (dealModifier > 1)
                    {
                        if (gold.Amount >= currentOffer)
                        {
                            UpdateOffer(gold);

                            return true;
                        }
                        else
                        {
                            SayTo(currentPlayer, true, $"I need {currentOffer} gold to continue!");
                        }
                    }
                    else
                    {
                        if (gold.Amount >= currentOffer)
                        {
                            UpdatePotOffer(gold);

                            int prize = Utility.RandomMinMax(1, currentPot) * Utility.RandomMinMax(1, roundsToPlay);

                            if (prize / 2 > costToPlay && Utility.RandomDouble() > 0.05)
                            {
                                prize = Utility.RandomMinMax(costToPlay, prize / 2);
                            }
                            else
                            {
                                prize = costToPlay;
                            }

                            if (prize > currentPot)
                            {
                                SayTo(currentPlayer, true, $"You beat the bank, you recieve {prize} gold as your reward!");

                                if (currentPlayer.Female)
                                {
                                    currentPlayer.PlaySound(0x30F);
                                }
                                else
                                {
                                    currentPlayer.PlaySound(0x41E);
                                }
                            }
                            else
                            {
                                SayTo(currentPlayer, true, $"You lost to the bank, you recieve {prize} gold for trying!");

                                if (currentPlayer.Female)
                                {
                                    currentPlayer.PlaySound(0x31C);
                                }
                                else
                                {
                                    currentPlayer.PlaySound(0x42C);
                                }
                            }

                            GivePrize(prize);

                            ResetGame();

                            return true;
                        }
                        else
                        {
                            SayTo(currentPlayer, true, $"I need {currentOffer} gold to continue!");
                        }
                    }
                }
                else
                {
                    from.SendAsciiMessage(53, "Game in progress, your gold isn't needed!");
                }
            }

            return false;
        }

        private void UpdateOffer(Gold gold)
        {
            UpdatePotOffer(gold);

            SayTo(currentPlayer, true, $"Bank offers you {currentOffer} gold to stop playing!");

            dealModifier--;

            if (dealModifier > 1)
            {
                SayTo(currentPlayer, true, $"You have {dealModifier} offers left!");
            }
            else
            {
                SayTo(currentPlayer, true, "You have 1 offer left to reveal final prize!");
            }
        }

        private void UpdatePotOffer(Gold gold)
        {
            int overPay;

            if (currentOffer == 0)
            {
                overPay = Math.Abs(costToPlay - gold.Amount);

                currentPot += costToPlay;
            }
            else
            {
                overPay = Math.Abs(currentOffer - gold.Amount);

                currentPot += currentOffer;
            }

            if (overPay > 0)
            {
                currentPlayer.AddToBackpack(new Gold(overPay));

                currentPlayer.SendAsciiMessage(53, $"You gave too much gold, {overPay} gold was returned!");
            }

            int randomValue = Utility.RandomMinMax(costToPlay + ((25 * roundsToPlay) * (roundsToPlay - dealModifier)), currentPot);

            if (dealModifier > 2)
            {
                currentOffer = randomValue / dealModifier;
            }
            else
            {
                currentOffer = randomValue / 2;
            }

            PlaySound(Utility.RandomList(0x36, 0x37));

            gold.Delete();
        }

        private void GivePrize(int prize)
        {
            if (prize > 60000)
            {
                currentPlayer.AddToBackpack(new BankCheck(prize));
            }
            else
            {
                currentPlayer.AddToBackpack(new Gold(prize));
            }

            currentPlayer.PlaySound(Utility.RandomList(0x36, 0x37));
        }

        private void ResetGame()
        {
            currentPlayer = null;

            currentOffer = 0;

            currentPot = 0;

            dealModifier = roundsToPlay;
        }

        public override void OnThink()
        {
            if (currentPlayer != null)
            {
                if (!currentPlayer.Alive || currentPlayer.Deleted)
                {
                    ResetGame();
                }
                else
                {
                    if (!InRange(currentPlayer, 10))
                    {
                        currentPlayer.SendAsciiMessage(33, "Return to Deal or No Deal or the Game will end and you'll recieve nothing!");

                        if (!InRange(currentPlayer, 40))
                        {
                            ResetGame();
                        }
                    }
                }
            }

            base.OnThink();
        }

        public HowieMandel(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            _ = reader.ReadInt();
        }
    }
}
