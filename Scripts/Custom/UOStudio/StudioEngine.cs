using System;
using System.IO;
using Server.Items;
using Server.Mobiles;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace Server.Custom.UOStudio
{
    public enum FilmState
    {
        Move,
        Line,
        Action
    }

    public static class StudioEngine
    {
        internal const string Version = "UO_Studio_2024,Version_1.0.0.11";

        internal static bool IsDebug { get; set; }

        internal static bool IsPaused { get; private set; }

        internal static readonly string Studio_DIR = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "UOS_DATA");

        internal static readonly string ImportPath = Path.Combine(Studio_DIR, "IMPORT");

        private static readonly string Studio_FileName = "UOS_Settings.csv";

        private static string cacheData;

        internal static bool HasData(out int version)
        {
            if (!File.Exists(Path.Combine(Studio_DIR, Studio_FileName)))
            {
                try
                {
                    File.WriteAllText(Path.Combine(Studio_DIR, Studio_FileName), "UO_Studio_2024,Version_1.0.0.1"); // Init Ver
                }
                catch
                {
                    version = 0;

                    return false;
                }

                version = 1;

                return true;
            }
            else
            {
                try
                {
                    cacheData = File.ReadAllText(Path.Combine(Studio_DIR, Studio_FileName));
                }
                catch
                {
                    version = 0;

                    return false;
                }

                if (!string.IsNullOrEmpty(cacheData) && int.TryParse(cacheData.ToCharArray().Last().ToString(), out int val))
                {
                    version = val;
                }
                else
                {
                    version = 1;
                }

                return true;
            }
        }

        private static void SaveCurrent()
        {
            try
            {
                File.WriteAllText(Path.Combine(Studio_DIR, Studio_FileName), Version);
            }
            catch
            {
                // do nothing
            }
        }

        internal static Dictionary<Mobile, StudioFilm> Actors { get; private set; }

        internal static void AddActor(Mobile from, StudioFilm film)
        {
            if (!Actors.ContainsKey(from))
            {
                Actors.Add(from, film);

                from.Say("I'll be acting!");
            }
        }

        internal static void RemoveActor(Mobile from)
        {
            if (Actors.ContainsKey(from))
            {
                Actors.Remove(from);
            }
        }

        public static void Initialize()
        {
            ValidateDirectories();

            Actors = new Dictionary<Mobile, StudioFilm>();

            EventSink.Movement += EventSink_Movement;

            EventSink.Speech += EventSink_Speech;

            EventSink.AnimateRequest += EventSink_AnimateRequest;

            EventSink.ItemCreated += EventSink_ItemCreated;

            EventSink.ItemDeleted += EventSink_ItemDeleted;

            EventSink.BeforeWorldSave += EventSink_BeforeWorldSave;

            EventSink.AfterWorldSave += EventSink_AfterWorldSave;
        }

        private static void ValidateDirectories()
        {
            if (!Directory.Exists(Studio_DIR))
            {
                Directory.CreateDirectory(Studio_DIR);
            }

            if (!Directory.Exists(ImportPath))
            {
                Directory.CreateDirectory(ImportPath);
            }
        }

        internal static (int X, int Y) GetDirOS(Direction direction)
        {
            int x = 0;

            int y = 0;

            Movement.Movement.Offset(direction, ref x, ref y);

            return (x, y);
        }

        private static void EventSink_Movement(MovementEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm && Actors.ContainsKey(pm) && !IsPaused)
            {
                SendFrameData(pm, FilmState.Move, $"{e.Mobile.X + GetDirOS(pm.Direction).X}:{e.Mobile.Y + GetDirOS(pm.Direction).Y}:{e.Mobile.Z}");
            }
        }

        private static void EventSink_Speech(SpeechEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm && Actors.ContainsKey(pm) && !IsPaused)
            {
                SendFrameData(pm, FilmState.Line, e.Speech);
            }
        }

        private static void EventSink_AnimateRequest(AnimateRequestEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm && Actors.ContainsKey(pm) && !IsPaused)
            {
                SendFrameData(pm, FilmState.Action, e.Action);
            }
        }

        private static void EventSink_ItemCreated(ItemCreatedEventArgs e)
        {
            if (Actors.Count > 0 && e.Item is StudioProp sp && !IsPaused)
            {
                foreach (var info in Actors.Values)
                {
                    if (info.GetStage().Contains(sp.Location))
                    {
                        if (info.Recorder.IsRecording)
                        {
                            info.AddPropInfo(sp, new PropInfo(sp.ItemID, sp.Hue, sp.X, sp.Y, sp.Z));
                        }
                        else
                        {
                            info.AddProp(sp);
                        }

                        break;
                    }
                }
            }
        }

        private static void EventSink_ItemDeleted(ItemDeletedEventArgs e)
        {
            if (Actors.Count > 0 && e.Item is StudioProp sp && !IsPaused)
            {
                foreach (var info in Actors.Values)
                {
                    if (info.GetStage().Contains(sp.Location))
                    {
                        if (info.Recorder.IsRecording)
                        {
                            info.RemovePropInfo(new PropInfo(sp.ItemID, sp.Hue, sp.X, sp.Y, sp.Z));
                        }
                        else
                        {
                            info.RemoveProp(sp);
                        }

                        break;
                    }
                }
            }
        }

        private static void EventSink_BeforeWorldSave(BeforeWorldSaveEventArgs e)
        {
            IsPaused = true;
        }

        private static void EventSink_AfterWorldSave(AfterWorldSaveEventArgs e)
        {
            IsPaused = false;

            SaveCurrent();
        }

        private static void SendFrameData(Mobile actor, FilmState state, string arg)
        {
            if (Actors.ContainsKey(actor))
            {
                StudioFilm film = Actors[actor];

                if (film.Recorder.IsRecording)
                {
                    film.AddFrame(actor, state, arg);

                    switch (state)
                    {
                        case FilmState.Move:
                            {
                                actor.SendMessage(63, "Move - Recorded!");

                                break;
                            }

                        case FilmState.Line:
                            {
                                actor.SendMessage(73, "Line - Recorded!");

                                break;
                            }

                        case FilmState.Action:
                            {
                                actor.SendMessage(83, "Action - Recorded!");

                                break;
                            }
                    }
                }
            }
        }

        internal static void SetUpBarrier(Mobile from, VideoRecorder recorder, Rectangle2D stage, Map map)
        {
            Point3D point;

            for (var x = stage.Start.X; x < stage.Start.X + stage.Width + 1; x++)
            {
                var z = map.GetAverageZ(x, stage.Start.Y) + 1;

                point = new Point3D(x, stage.Start.Y, z);

                AddBlockers(from, recorder, point, map);
            }

            for (var x = stage.End.X; x > stage.End.X - stage.Width; x--)
            {
                var z = map.GetAverageZ(x, stage.End.Y) + 1;

                point = new Point3D(x, stage.End.Y, z);

                AddBlockers(from, recorder, point, map);
            }

            for (var y = stage.Start.Y; y < stage.Start.Y + stage.Height + 1; y++)
            {
                var z = map.GetAverageZ(stage.Start.X, y) + 1;

                point = new Point3D(stage.Start.X, y, z);

                AddBlockers(from, recorder, point, map);
            }

            for (var y = stage.End.Y; y > stage.End.Y - stage.Height; y--)
            {
                var z = map.GetAverageZ(stage.End.X, y) + 1;

                point = new Point3D(stage.End.X, y, z);

                AddBlockers(from, recorder, point, map);
            }
        }

        private static void AddBlockers(Mobile from, VideoRecorder recorder, Point3D point, Map map)
        {
            var blocker = new Blocker() { Name = $"Stage Set by {from.Name}", Hue = 2752 };

            var losBlocker = new LOSBlocker() { Name = $"Stage Set by {from.Name}", Hue = 2752 };

            recorder.Blockers.Add(blocker);

            recorder.Blockers.Add(losBlocker);

            blocker.MoveToWorld(point, map);

            losBlocker.MoveToWorld(point, map);

            recorder.UpdateBlockers();
        }

        public static void RemoveBlockers(VideoRecorder recorder)
        {
            int count = recorder.Blockers.Count;

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    recorder.Blockers[i].Delete();
                }

                recorder.Blockers.Clear();
            }
        }

        internal static Direction GetDirection(string action)
        {
            switch (action)
            {
                case "Up":          return Direction.Up;
                case "Mask":        return Direction.Mask;      // up
                case "ValueMask":   return Direction.ValueMask; // up
                case "Right":       return Direction.Right;
                case "129":         return Direction.Right;
                case "East":        return Direction.East;
                case "130":         return Direction.East;
                case "Down":        return Direction.Down;
                case "131":         return Direction.Down;
                case "South":       return Direction.South;
                case "132":         return Direction.South;
                case "Left":        return Direction.Left;
                case "133":         return Direction.Left;
                case "West":        return Direction.West;
                case "134":         return Direction.West;
                case "North":       return Direction.North;
                case "Running":     return Direction.North;
                default:            return Direction.Mask;
            }
        }

        internal static void RunAction(Mobile from, string act)
        {
            int action;

            bool useNew = Core.SA;

            switch (act.ToLower())
            {
                case "bow":
                    {
                        action = useNew ? 0 : 32;

                        break;
                    }

                case "salute":
                    {
                        action = useNew ? 1 : 33;

                        break;
                    }

                default: return;
            }

            if (useNew)
            {
                from.Animate(AnimationType.Emote, action);
            }
            else
            {
                from.Animate(action, 5, 1, true, false, 0);
            }
        }

        internal static void PlayStudioEffects(Point3D loc, Map map)
        {
            Effects.PlaySound(loc, map, Utility.RandomList(0x3E, 0x3F));

            Effects.SendLocationEffect(loc, map, 0x9F89, 18, Utility.RandomBrightHue(), 0);
        }

        internal static bool TryHexToInt(string hexValue, out int value)
        {
            if (hexValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                hexValue = hexValue.Substring(2);

                try
                {
                    value = int.Parse(hexValue, NumberStyles.HexNumber);

                    return true;
                }
                catch
                {
                    value = 0;

                    return false;
                }
            }
            else
            {
                return int.TryParse(hexValue, out value);
            }
        }

        internal static void ResetMods(PlayerMobile user)
        {
            user.NameMod = null;

            user.BodyMod = 0x0;

            user.HueMod = -1;
        }
    }
}
