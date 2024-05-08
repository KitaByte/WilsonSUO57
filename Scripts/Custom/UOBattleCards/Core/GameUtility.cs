using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Services.UOBattleCards.Cards;
using Server.Services.UOBattleCards.Cards.Types;
using Server.Services.UOBattleCards.Gumps;

namespace Server.Services.UOBattleCards.Core
{
	public class GameUtility
    {
		public static void StartGame(MatchInfo game)
        {
            game.PlayerOne.SendMessage(52, "Game Starting!");

            MatchUtility.AddMatchPlayers(game);

            if (game.PlayerOne != null && game.PlayerTwo != null)
            {
                game.PlayerOneDeck.ShuffleDeck(false);

                if (game.IsAI)
                {
                    game.PlayerTwoDeck.ShuffleDeck(true);
                }
                else
                {
                    game.PlayerTwoDeck.ShuffleDeck(false);
                }

                SendGameGumps(game);
            }
        }

        private static void SendGameGumps(MatchInfo game)
		{
			game.PlayerOneReady = false;
			game.PlayerTwoReady = false;

			if (game.PlayerOne.HasGump(typeof(GameGump)))
            {
                game.PlayerOne.CloseGump(typeof(GameGump));
            }

            BaseGump.SendGump(new GameGump(game.PlayerOne as PlayerMobile, game, 50, 100));

            if (!game.IsAI)
            {
                if (game.PlayerTwo.HasGump(typeof(GameGump)))
                {
                    game.PlayerTwo.CloseGump(typeof(GameGump));
                }

                BaseGump.SendGump(new GameGump(game.PlayerTwo as PlayerMobile, game, 50, 100));
            }
		}

		private static void SendLeadboard(MatchInfo game)
		{
			if (game.PlayerOne.HasGump(typeof(LeaderBoardGump)))
			{
				game.PlayerOne.CloseGump(typeof(LeaderBoardGump));
			}

			BaseGump.SendGump(new LeaderBoardGump(game.PlayerOne as PlayerMobile, game.PlayerOneDeck));

			if (!game.IsAI)
			{
				if (game.PlayerTwo.HasGump(typeof(LeaderBoardGump)))
				{
					game.PlayerTwo.CloseGump(typeof(LeaderBoardGump));
				}

				BaseGump.SendGump(new LeaderBoardGump(game.PlayerTwo as PlayerMobile, game.PlayerTwoDeck));
			}
		}

		private static void CloseGameGumps(MatchInfo game)
		{
			if (game.PlayerOne.HasGump(typeof(GameGump)))
			{
				game.PlayerOne.CloseGump(typeof(GameGump));
			}

			if (!game.IsAI)
			{
				if (game.PlayerTwo.HasGump(typeof(GameGump)))
				{
					game.PlayerTwo.CloseGump(typeof(GameGump));
				}
			}
		}

		public static void UpdateMatch(MatchInfo game)
		{
			if (game.PlayerOneReady && game.PlayerTwoReady && game.Round != 0)
			{
				UpdateCards(game);

				game.UpdateMatchPosition();

				SendGameGumps(game);
			}
		}

        private static void UpdateCards(MatchInfo game)
        {
            if (game.IsRoundStart)
            {
                var damageOne = 1;

                var damageTwo = 1;

                var PlayerOneCard = game.PlayerOneDeck.CardDeck[game.Round - 1];

                var PlayerTwoCard = game.PlayerTwoDeck.CardDeck[game.Round - 1];

                if (game.IsAI && game.PlayerTwoDeck.AIDeck.Count > 0)
                {
                    PlayerTwoCard = game.PlayerTwoDeck.AIDeck[game.Round - 1];
                }

                (bool Attack, bool Block) isAIAttack = (false, false);

                if (game.IsAI)
                {
                    var chance = Utility.RandomBool();

                    isAIAttack = (chance, !chance);
                }

                if (PlayerOneCard is CreatureCard p1Card)
                {
                    damageTwo += p1Card.Info.GetAttack();
                }
                else
                {
                    damageTwo += GetSupportCardDamage(PlayerOneCard);
                }

                if (PlayerTwoCard is CreatureCard p2Card)
                {
                    damageOne += p2Card.Info.GetAttack();
                }
                else
                {
                    damageOne += GetSupportCardDamage(PlayerTwoCard);
                }

                if (game.PlayerOneChoice.Attack)
                {
                    damageTwo *= 2;
                }

                if (game.PlayerOneChoice.Block)
                {
                    damageOne /= 2;
                }

                if (game.PlayerOneChoice.Pass)
                {
                    // Pass has no mod
                }

                if (game.PlayerTwoChoice.Attack || isAIAttack.Attack)
                {
                    damageOne *= 2;
                }

                if (game.PlayerTwoChoice.Block || isAIAttack.Block)
                {
                    damageTwo /= 2;
                }

                if (game.PlayerTwoChoice.Pass || game.IsAI && isAIAttack.Attack == false && isAIAttack.Block == false)
                {
                    // Pass has no mod
                }

                if (PlayerOneCard is CreatureCard card1)
                {
                    card1.Info.Damage += damageOne;

                    CheckKilled(card1);

                    game.PlayerOne.SendMessage(32, $"Your card took {damageOne} damage");

                    if (!game.IsAI)
                    {
                        game.PlayerTwo.SendMessage(72, $"You strike for {damageOne} damage");
                    }

                    LevelUtility.SendActionXP(card1, damageOne / card1.Info.GetLevel());
                }

                if (PlayerTwoCard is CreatureCard card2)
                {
                    card2.Info.Damage += damageTwo;

                    CheckKilled(card2);

                    game.PlayerOne.SendMessage(72, $"You strike for {damageTwo} damage");

                    if (!game.IsAI)
                    {
                        game.PlayerTwo.SendMessage(32, $"Your card took {damageTwo} damage");
                    }

                    LevelUtility.SendActionXP(card2, damageTwo / card2.Info.GetLevel());
                }

                if (damageOne < damageTwo)
                {
                    game.PlayerOneStats.LastMatchPoints++;

                    game.RoundWinner = PlayerTypes.PlayerOne;
                }
                else if (damageOne > damageTwo)
                {
                    game.PlayerTwoStats.LastMatchPoints++;

                    game.RoundWinner = PlayerTypes.PlayerTwo;
                }
                else
                {
                    game.RoundWinner = PlayerTypes.None;
                }

                game.PlayerOne.SendMessage(42, $"Score : {game.PlayerOneStats.LastMatchPoints} to {game.PlayerTwoStats.LastMatchPoints}");

                if (!game.IsAI)
                    game.PlayerTwo.SendMessage(42, $"Score : {game.PlayerTwoStats.LastMatchPoints} to {game.PlayerOneStats.LastMatchPoints}");
            }
        }

        private static int GetSupportCardDamage(BaseCard card)
        {
            var damage = 0;

            if (card is SkillCard skill)
            {
                damage = skill.TryGetDamage();
            }

            if (card is SpecialCard special)
            {
                damage = special.TryGetDamage();
            }

            if (card is SpellCard spell)
            {
                damage = spell.TryGetDamage();
            }

            if (card is TrapCard trap)
            {
                damage = trap.TryGetDamage();
            }

            return damage;
        }

        private static void CheckKilled(CreatureCard card)
        {
            if (card.Info.Damage >= card.Info.GetDefense())
            {
                card.Info.Damage = card.Info.GetDefense();

                card.Info.Owner?.SendMessage(32, $"{card.Name} has been killed!");
            }
        }

		public static void EndGame(MatchInfo game)
        {
            game.RoundTimeSet = 0;

            if (game.RoundEndedBy == PlayerTypes.PlayerOne)
            {
                game.PlayerTwo.SendMessage(42, $"Match Ended by {game.PlayerOne.Name}, Thanks for playing!");

                game.PlayerOne.SendMessage("You Lost!");
                game.PlayerOneStats.UpdateInfo(0, 0, 1);

				if (!game.IsAI)
				{
					game.PlayerTwo.SendMessage("You Won");
					game.PlayerTwoStats.UpdateInfo(1, 0, 0);

					PayWager(game.PlayerTwo as PlayerMobile, game.Wager);
				}
            }
            else if (game.RoundEndedBy == PlayerTypes.PlayerTwo)
            {
                game.PlayerOne.SendMessage(42, $"Match Ended by {game.PlayerTwo.Name}, Thanks for playing!");

                game.PlayerOne.SendMessage("You Won!");
                game.PlayerOneStats.UpdateInfo(1, 0, 0);

				if (!game.IsAI)
				{
					game.PlayerTwo.SendMessage("You Lost");
					game.PlayerTwoStats.UpdateInfo(0, 0, 1);

					PayWager(game.PlayerOne as PlayerMobile, game.Wager);
				}
            }
            else
            {
				if (game.PlayerOneStats.LastMatchPoints > game.PlayerTwoStats.LastMatchPoints)
				{
					game.PlayerOne.SendMessage("You Won!");
					game.PlayerOneStats.UpdateInfo(1, 0, 0);

					if (!game.IsAI)
					{
						game.PlayerTwo.SendMessage("You Lost");
						game.PlayerTwoStats.UpdateInfo(0, 0, 1);

						PayWager(game.PlayerOne as PlayerMobile, game.Wager);
					}
				}
				else if (game.PlayerOneStats.LastMatchPoints < game.PlayerTwoStats.LastMatchPoints)
				{
					game.PlayerOne.SendMessage("You Lost!");
					game.PlayerOneStats.UpdateInfo(0, 0, 1);

					if (!game.IsAI)
					{
						game.PlayerTwo.SendMessage("You Won");
						game.PlayerTwoStats.UpdateInfo(1, 0, 0);

						PayWager(game.PlayerTwo as PlayerMobile, game.Wager);
					}
				}
				else
				{
					game.PlayerOne.SendMessage("You Tied!");
					game.PlayerOneStats.UpdateInfo(0, 0, 1);

					if (!game.IsAI)
					{
						game.PlayerTwo.SendMessage("You Tied");
						game.PlayerTwoStats.UpdateInfo(1, 0, 0);

						ReturnWager(game);
					}
                }
            }

            if (PlayerUtility.PlayerStats.Find(p => p.Name == game.PlayerOne.Name) == null)
                PlayerUtility.PlayerStats.Add(game.PlayerOneStats);

			game.PlayerOne.SendMessage(52, "Thanks for player UO Battle Cards!");

			if (!game.IsAI)
            {
                if (PlayerUtility.PlayerStats.Find(p => p.Name == game.PlayerTwo.Name) == null)
                    PlayerUtility.PlayerStats.Add(game.PlayerTwoStats);

				game.PlayerTwo.SendMessage(52, "Thanks for player UO Battle Cards!");
			}

			KickDamagedCards(game);

			CloseGameGumps(game);

			SendLeadboard(game);

			MatchUtility.RemoveMatch(game);
        }

		private static void PayWager(PlayerMobile player, int amount)
        {
            if (amount > 0)
            {
                player.AddToBackpack(new Gold() { Amount = amount * 2 });
            }
        }

        private static void ReturnWager(MatchInfo game)
        {
            if (game.Wager > 0)
            {
                game.PlayerOne.AddToBackpack(new Gold() { Amount = game.Wager });

                game.PlayerTwo.AddToBackpack(new Gold() { Amount = game.Wager });
            }
        }

        private static void KickDamagedCards(MatchInfo game)
        {
            for (var i = 0; i < 32; i++)
            {
                if (game.PlayerOneDeck.CardDeck[i] is CreatureCard cc1)
                {
                    if (cc1.Info.Damage > 0)
                    {
                        game.PlayerOne.AddToBackpack(cc1.Info.Card);

                        game.PlayerOne.SendMessage(32, $"{cc1.Name} is Damaged : Kicked!");
                    }
                }

                if (!game.IsAI)
                {
                    if (game.PlayerTwoDeck.CardDeck[i] is CreatureCard cc2)
                    {
                        if (cc2.Info.Damage > 0)
                        {
                            game.PlayerTwo.AddToBackpack(cc2.Info.Card);

                            game.PlayerTwo.SendMessage(32, $"{cc2.Name} is Damaged : Kicked!");
                        }
                    }
                }
            }
        }
    }
}
