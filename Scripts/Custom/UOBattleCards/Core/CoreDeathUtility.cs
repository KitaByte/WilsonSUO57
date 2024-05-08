using System.Linq;

using Server.Mobiles;
using Server.Services.UOBattleCard.Items;
using Server.Services.UOBattleCards.Cards;
using Server.Services.UOBattleCards.Cards.Types;
using Server.Services.UOBattleCards.Mobiles;

namespace Server.Services.UOBattleCards.Core
{
	public static class CoreDeathUtility
	{
		public static void EventSink_CreatureDeath(CreatureDeathEventArgs e)
        {
            if (CoreUtility.GameTimer == null || !CoreUtility.GameTimer.Running)
            {
                CoreUtility.RestartTimer();
            }

            if (e.Creature is BaseCreature bc)
			{
				if (bc.ControlMaster == null)
				{
					if (CreatureUtility.HasCreature(CreatureUtility.CleanName(bc.GetType().Name)))
					{
                        var cardCount = CreatureUtility.CreatureDict.Count();

                        var chance = Utility.Random(cardCount);

						var total = cardCount * Settings.CardDropRate;

						// Staff Override, works on proximity
						if (e.Corpse.GetMobilesInRange(5)?.FirstOrDefault(m => m.AccessLevel > AccessLevel.Counselor) != null)
						{
							chance = 0;
						}

						if (chance < total)
						{
							var creatureName = CreatureUtility.CleanName(bc.Name);

							var cardInfo = new CardInfo
                            {
                                Name = creatureName,
                                Creature = creatureName
							};

                            e.Corpse.AddItem(new CreatureCard(cardInfo));

							// Gem & Special Card Drops
							if (Utility.Random(100) < Settings.GemDropRate || chance == 0)
							{
								e.Corpse.AddItem(new CardGem());
							}
                            else
                            {
                                // Try Spawn Special Mob (Pre Order)
                                if (bc.GetType() == typeof(Dragon) && Utility.Random(100) == 5)
                                {
                                    var sareus = new SmokeDragon();

                                    sareus.MoveToWorld(e.Corpse.Location, e.Corpse.Map);
                                }
                            }
						}
						else
						{
							if (Utility.Random(100) < (Settings.GemDropRate * 2))
							{
								e.Corpse.AddItem(new CardGem());
							}
						}
					}
				}
			}
		}
	}
}
