using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
namespace Server.Custom.UOStudio
{
    public class FilmFrame
    {
        public BodyDouble FrameDouble { get; set; }

        public List<PropInfo> Props { get; set; }

        public List<int> Sounds { get; set; }

        public Point3D Location { get; set; }

        public Direction MoveDirection { get; set; }

        public FilmState State { get; set; }

        public string Line { get; set; }

        public string Action { get; set; }

        public FilmFrame()
        {
            Props = new List<PropInfo>();

            Sounds = new List<int>();
        }

        public FilmFrame(Mobile actor, FilmState state, string arg)
        {
            FrameDouble = new BodyDouble();

            Props = new List<PropInfo>();

            Sounds = new List<int>();

            FrameDouble.CopyActor(actor);

            Location = actor.Location;

            MoveDirection = actor.Direction;

            State = state;

            switch (State)
            {
                case FilmState.Move:
                    {
                        Line = arg;

                        Action = arg;

                        break;
                    }

                case FilmState.Line:
                    {
                        Line = arg;

                        Action = string.Empty;

                        break;
                    }

                case FilmState.Action:
                    {
                        Line = string.Empty;

                        Action = arg;

                        break;
                    }

                default:
                    {
                        Line = string.Empty;

                        Action = string.Empty;

                        break;
                    }
            }
        }

        internal string GetName()
        {
            return FrameDouble.ActorName;
        }

        internal int GetBody()
        {
            return FrameDouble.BodyID;
        }

        internal int GetHue()
        {
            return FrameDouble.SkinHue;
        }

        internal Point3D GetNextLocation()
        {
            if (Action.Contains(':'))
            {
                var coords = Action.Split(':');

                int x = int.Parse(coords[0]);

                int y = int.Parse(coords[1]);

                int z = int.Parse(coords[2]);

                return new Point3D(x, y, z);
            }
            else
            {
                return Point3D.Zero;
            }
        }

        internal void Save(GenericWriter writer)
        {
            writer.Write(Location);

            writer.Write(MoveDirection.ToString());

            writer.Write(State.ToString());

            writer.Write(Line);

            writer.Write(Action);

            FrameDouble.Save(writer);

            writer.Write(Props.Count);

            if (Props.Count > 0)
            {
                foreach (var prop in Props)
                {
                    prop.SavePorp(writer);
                }
            }

            writer.Write(Sounds.Count);

            if (Sounds.Count > 0)
            {
                foreach (var sound in Sounds)
                {
                    writer.Write(sound);
                }
            }
        }

        internal void Export(StreamWriter writer)
        {
            writer.WriteLine(Location);

            writer.WriteLine(MoveDirection.ToString());

            writer.WriteLine(State.ToString());

            writer.WriteLine(Line);

            writer.WriteLine(Action);

            FrameDouble.Export(writer);

            writer.WriteLine(Props.Count);

            if (Props.Count > 0)
            {
                foreach (var propInfo in Props)
                {
                    propInfo.Export(writer);
                }
            }

            writer.WriteLine(Sounds.Count);

            if (Sounds.Count > 0)
            {
                foreach (var sound in Sounds)
                {
                    writer.WriteLine(sound);
                }
            }
        }

        internal void Load(GenericReader reader)
        {
            Location = reader.ReadPoint3D();

            MoveDirection = StudioEngine.GetDirection(reader.ReadString());

            string state = reader.ReadString();

            State = (FilmState)Enum.Parse(typeof(FilmState), state);

            Line = reader.ReadString();

            Action = reader.ReadString();

            FrameDouble = new BodyDouble();

            FrameDouble.Load(reader);

            Props = new List<PropInfo>();

            int countProp = reader.ReadInt();

            for (int i = 0; i < countProp; i++)
            {
                var info = new PropInfo(reader);

                Props.Add(info);
            }

            Sounds = new List<int>();

            int countSound = reader.ReadInt();

            for (int i = 0; i < countSound; i++)
            {
                Sounds.Add(reader.ReadInt());
            }
        }

        internal void Import(StreamReader reader)
        {
            Location = Point3D.Parse(reader.ReadLine());

            MoveDirection = (Direction)Enum.Parse(typeof(Direction), reader.ReadLine());

            State = (FilmState)Enum.Parse(typeof(FilmState), reader.ReadLine());

            Line = reader.ReadLine();

            Action = reader.ReadLine();

            FrameDouble = new BodyDouble();

            FrameDouble.Import(reader);

            var countProps = int.Parse(reader.ReadLine());

            for (int k = 0; k < countProps; k++)
            {
                PropInfo prop = new PropInfo();

                prop.Import(reader);

                Props.Add(prop);
            }

            var countSounds = int.Parse(reader.ReadLine());

            for (int l = 0; l < countSounds; l++)
            {
                Sounds.Add(int.Parse(reader.ReadLine()));
            }
        }
    }
}
