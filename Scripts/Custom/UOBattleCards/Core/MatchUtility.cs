using System;
using System.Collections.Generic;

using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Services.UOBattleCards.Gumps;
using Server.Services.UOBattleCards.Items;

namespace Server.Services.UOBattleCards.Core
{
    public class MatchMakeInfo
    {
        // Challenger Info
        internal Mobile Player { get; set; }

        internal BattleDeck Deck { get; set; }

        internal bool FoilAllowed { get; set; }

        internal ushort MaxRarity { get; set; }

        internal ushort MaxLevel { get; set; }

        internal int ValueCap { get; set; }

        internal ushort RoundTime { get; set; }

        internal ushort Wager { get; set; }

        // Opponent Info

        internal List<(PlayerMobile Opponent, BattleDeck Deck)> ValidPlayers = new List<(PlayerMobile, BattleDeck)>();

        internal bool Accepted { get; set; } = false;

		internal void AddPlayer(PlayerMobile pm, BattleDeck deck)
		{
			if (Wager > 0)
			{
				var gold = pm.Backpack.FindItemByType(typeof(Gold));

				if (gold != null || gold.Amount >= Wager)
				{
					if (ValidPlayers.Find(p => p.Opponent == Player).Opponent == null)
						ValidPlayers.Add((pm, deck));
				}
			}
			else
			{
				if (ValidPlayers.Find(p => p.Opponent == Player).Opponent == null)
					ValidPlayers.Add((pm, deck));
			}
		}
	}

	public static class MatchUtility
    {
        private static List<MatchInfo> Matches = new List<MatchInfo>();

		public static List<MatchMakeInfo> MatchMaking = new List<MatchMakeInfo>();

		public static bool InMatch(Mobile player, bool sendMsg = false)
        {
            if (Matches.Count > 0 && Matches.Find(i => i.PlayerOne == player || i.PlayerTwo == player) != null)
            {
                if (sendMsg)
                    player.SendMessage(32, "Access denied while in match!");

                return true;
            }

            if (MatchMaking.Find(m => m.Player == player) != null)
            {
                if (sendMsg)
                    player.SendMessage(32, "Access denied while in match making!");

                return true;
            }

            return false;
		}

		public static MatchInfo GetMatchInfo(PlayerMobile pm)
		{
			if (InMatch(pm))
			{
				return Matches.Find(i => i.PlayerOne == pm || i.PlayerTwo == pm);
			}
			else
			{
				return null;
			}
		}

		public static void AddMatchPlayers(MatchInfo info)
        {
            if (info.PlayerOne != null && info.PlayerTwo != null)
            {
                info.PlayerOneStats = PlayerUtility.PlayerStats.Find(p => p.Name == info.PlayerOne.Name);

                if (info.PlayerOneStats == null)
                {
                    info.PlayerOneStats = new PlayerInfo(info.PlayerOne.Name);
                }

                if (info.IsAI)
                {
                    info.PlayerTwoStats = new PlayerInfo("AI");
                }
                else
                {
                    info.PlayerTwoStats = PlayerUtility.PlayerStats.Find(p => p.Name == info.PlayerTwo.Name);

                    if (info.PlayerTwoStats == null)
                    {
                        info.PlayerTwoStats = new PlayerInfo(info.PlayerTwo.Name);
                    }
                }

                Matches.Add(info);
            }
            else
            {
                info.PlayerOne?.SendMessage(32, "Match failed to start!");
            }
        }

		public static void RemoveMatch(MatchInfo match)
        {
            var matchHandle = Matches.Find(m => m.PlayerOne == match.PlayerOne);

            if (matchHandle != null)
            {
                Matches.Remove(matchHandle);
            }
        }

		public static List<MatchInfo> CurrentMatches()
        {
            if (Matches == null)
            {
                Matches = new List<MatchInfo>();
            }

            return Matches;
        }

		public static void FindMatch(int location)
        {
            if (MatchMaking == null || MatchMaking.Count <= location)
                return;

            var match = MatchMaking[location];

            MatchMaking.RemoveAt(location);

            if (match != null && match.ValidPlayers.Count == 0)
            {
                var mobs = World.Mobiles.Values;

                var players = new List<PlayerMobile>();

                if (mobs == null || mobs.Count == 0)
                {
                    return;
                }

                foreach (var mob in mobs)
                {
                    if (mob is PlayerMobile pm)
                    {
                        players.Add(pm);
                    }
                }

                if (players.Count > 0)
                {
                    for (var i = 0; i < players.Count; i++)
                    {
						if (players[i] != match.Player && players[i] is PlayerMobile pm)
						{
							var decks = pm.Backpack.FindItemsByType(typeof(BattleDeck));

							if (decks != null && decks.Length > 0)
							{
								for (var j = 0; j < decks.Length; j++)
								{
									if (decks[j] is BattleDeck deck)
									{
										if (deck.ValidateDeck(match))
										{
											match.AddPlayer(pm, deck);
										}
									}
								}
							}
						}
                    }
                }

                if (match.ValidPlayers.Count > 0)
                {
                    foreach (var player in match.ValidPlayers)
                    {
                        if (!player.Opponent.HasGump(typeof(MatchAcceptGump)))
                            BaseGump.SendGump(new MatchAcceptGump(player.Opponent, match));
                    }
                }
                else
                {
                    LoadAIPlayer(match);
                }

                match.Player.SendMessage(52, $"Challenge sent : Found {match.ValidPlayers.Count + 1} opponents!");
            }
        }

		public static void LoadAIPlayer(MatchMakeInfo match)
        {
            var game = new MatchInfo()
            {
                PlayerOne = match.Player,
                PlayerOneDeck = match.Deck,
                PlayerTwo = match.Player,
                PlayerTwoDeck = match.Deck,
                RoundTimeSet = 0,
                MatchTick = 0,
                IsAI = true,
                Wager = 0
            };

            GameUtility.StartGame(game);

            match.Player.SendMessage(52, $"Loading AI opponent!");
        }
	}
}
