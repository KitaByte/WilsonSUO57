using System.Collections.Generic;
using System.Linq;

namespace Server.Services.UOBlackBox
{
    public class UndoHandler
    {
        private readonly Stack<string> undoStack;

        public UndoHandler()
        {
            undoStack = new Stack<string>();
        }

        public void Store(string action)
        {
            undoStack.Push(action);
        }

        public void Undo(string player)
        {
            if (undoStack.Count > 0)
            {
                UnDoActions(player);
            }
        }

        public void UnDoAll(string player)
        {
            while (undoStack.Count > 0)
            {
                UnDoActions(player);
            }
        }

        private void UnDoActions(string player)
        {
            string action = undoStack.Pop();

            if (action != "")
            {
                var command = action.ToString().Split(':');

                switch (command.First())
                {
                    case "Add": // Add:{b.ItemID}:{b.X}:{b.Y}:{b.Z}:{b.BMap}:{b.Hue}
                        {
                            AddAction(command, player);

                            break;
                        }
                    case "Delete": // Delete:{b.X}:{b.Y}:{b.Z}:{b.BMap}
                        {
                            DeleteAction(command, player);

                            break;
                        }
                    case "Alter": // Alter:b.ItemID}:{b.X}:{b.Y}:{b.Z}:{b.BMap}:{e.Property.Name}:{e.OldValue}
                        {
                            AlterAction(command, player);

                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        private void AddAction(string[] command, string player)
        {
            var hue = 0;

            if (int.TryParse(command.Last(), out int val))
            {
                hue = val;
            }

            var map = Map.Parse(command[5]);

            var art = new BoxStatic(player, map, int.Parse(command[1]))
            {
                Staff = player,

                IsUndo = true,

                Hue = hue
            };

            var loc = GetLocation(command);

            art.MoveToWorld(loc, map);
        }

        private void DeleteAction(string[] command, string player)
        {
            var loc = GetLocation(command, 1);

            var map = Map.Parse(command[4]);

            if (World.Items.Values.FirstOrDefault(i => i.Map.Name == map.Name && i.Location.Equals(loc)) is BoxStatic art && art.Staff == player)
            {
                art.IsUndo = true;

                art.Delete();
            }
        }

        private void AlterAction(string[] command, string player)
        {
            var id = int.Parse(command[1]);

            var loc = GetLocation(command);

            var map = Map.Parse(command[5]);

            if (World.Items.Values.FirstOrDefault(i => i.Map.Name == map.Name && i.Location.Equals(loc) && i.ItemID == id) is BoxStatic art && art.Staff == player)
            {
                art.IsUndo = true;

                if (command[6] == "ItemID")
                    art.ItemID = int.Parse(command[7]);

                if (command[6] == "Hue")
                    art.Hue = int.Parse(command[7]);

                if (command[6] == "X")
                    art.X = int.Parse(command[7]);

                if (command[6] == "Y")
                    art.Y = int.Parse(command[7]);

                if (command[6] == "Z")
                    art.Z = int.Parse(command[7]);
            }
        }

        private Point3D GetLocation(string[] command, int offset = 0)
        {
            int.TryParse(command[2 - offset], out int locX);
            int.TryParse(command[3 - offset], out int locY);
            int.TryParse(command[4 - offset], out int locZ);

            return new Point3D(locX, locY, locZ);
        }
    }

    public class UndoManager
    {
        private static readonly Dictionary<string, UndoHandler> Handlers  = new Dictionary<string, UndoHandler>();

        public static void SetupHandler(string player)
        {
            var handler = new UndoHandler();

            if (!Handlers.ContainsKey(player))
            {
                Handlers.Add(player, handler);
            }
        }

        public static void RemoveHandler(string player)
        {
            if (Handlers.ContainsKey(player))
            {
                Handlers.Remove(player);
            }
        }

        public static void AddCommand(string player, string command)
        {
            if (Handlers.TryGetValue(player, out var handler))
            {
                handler.Store(command);
            }
        }

        public static void Undo(string player)
        {
            if (Handlers.TryGetValue(player, out var handler))
            {
                handler.Undo(player);
            }
        }

        public static void UndoAll(string player)
        {
            if (Handlers.TryGetValue(player, out var handler))
            {
                handler.UnDoAll(player);
            }
        }
    }
}
