using System.Linq;
using Server.Items;
using Server.Targeting;

namespace Server.Custom.MovementSystem
{
    internal class ClimbUtility
    {
        internal static void TryClimb(Mobile from, LandTarget rock)
        {
            if (from.Stam > MoveSettings.ClimbStamMod && HasRope(from))
            {
                var skillMod = (MoveSettings.GetClimbingSkill(from).Value / 2) * 0.01;

                int distanceMod = (int)from.GetDistanceToSqrt(rock.Location) - MoveSettings.ClimbRopeMod;

                if (distanceMod > 0)
                {
                    distanceMod /= 3; // 3 tiles / rope adjustment
                }

                if (Utility.RandomDouble() < MoveSettings.ClimbChance + skillMod)
                {
                    if (Utility.RandomDouble() < MoveSettings.ClimbFallChance - (skillMod * 0.01))
                    {
                        from.Damage(Utility.RandomMinMax(distanceMod, MoveSettings.ClimbDamageMax));

                        from.SendMessage(42, "You fell!");

                        RemoveRopeChance(from, distanceMod);

                        from.Animate(AnimationType.Die, 0);

                        from.PlaySound(Utility.RandomList(0x147, 0x148, 0x149, 0x14A));
                    }
                    else
                    {
                        from.MoveToWorld(rock.Location, from.Map);

                        RemoveRope(from, MoveSettings.ClimbRopeMod + distanceMod);

                        from.Stam -= MoveSettings.ClimbStamMod + distanceMod;

                        from.Animate(AnimationType.Attack, 0);

                        from.PlaySound(Utility.RandomList(0x125, 0x126));
                    }
                }
                else
                {
                    from.SendMessage(42, "You failed to climb!");

                    RemoveRopeChance(from, distanceMod);

                    from.Animate(AnimationType.Spell, 0);

                    from.PlaySound(0x242);
                }
            }
            else
            {
                if (from.Stam < MoveSettings.ClimbStamMod)
                {
                    from.SendMessage(42, "Your exhausted!");
                }
                else
                {
                    from.SendMessage(42, "No rope!");
                }

                from.Animate(AnimationType.Fidget, 0);
            }
        }

        private static bool HasRope(Mobile from)
        {
            var ropePile = from.Backpack.FindItemsByType(typeof(Rope)).ToArray();

            if (ropePile == null || ropePile.Length < MoveSettings.ClimbRopeMod)
            {
                int count = 0;

                foreach (var rope in ropePile)
                {
                    count += rope.Amount;
                }

                if (count >= MoveSettings.ClimbRopeMod)
                {
                    return true;
                }

                return false;
            }

            return true;
        }

        private static void RemoveRopeChance(Mobile from, int distance)
        {
            if (Utility.RandomDouble() < 0.5)
            {
                if (MoveSettings.ClimbRopeMod + distance >= 2)
                {
                    RemoveRope(from, (MoveSettings.ClimbRopeMod + distance) / 2);
                }
                else
                {
                    RemoveRope(from, 1);
                }
            }
            else
            {
                from.SendMessage(42, $"No rope was lost!");
            }
        }

        private static void RemoveRope(Mobile from, int cost)
        {
            var ropePile = from.Backpack.FindItemsByType(typeof(Rope)).ToList();

            if (ropePile != null && ropePile.Count > 0)
            {
                for (int i = 0; i < cost; i++)
                {
                    if (ropePile.Count > 0)
                    {
                        var rope = ropePile.Last();

                        if (rope.Amount > 1)
                        {
                            rope.Amount--;
                        }
                        else
                        {
                            rope.Delete();

                            ropePile.Remove(rope);
                        }
                    }
                    else
                    {
                        from.SendMessage(42, $"The rope was just enough!");

                        cost--;
                    }
                }
            }

            if (cost > 0)
            {
                from.SendMessage(42, $"{cost} ropes used!");
            }
            else
            {
                from.SendMessage(53, $"No ropes used!");
            }
        }

        internal static bool HasRock(Point3D location, Map map)
        {
            var tile = new LandTarget(location, map);

            if (tile.Name.ToLower() == "rock" && (tile.Z == location.Z || tile.Z == location.Z - 1))
            {
                return true;
            }

            return false;
        }
    }
}
