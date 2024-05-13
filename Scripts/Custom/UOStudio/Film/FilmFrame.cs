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

        public int SoundID { get; set; }

        public EffectInfo Effect_Info { get; set; }

        public Point3D Location { get; set; }

        public Direction MoveDirection { get; set; }

        public FilmState State { get; set; }

        public string Line { get; set; }

        public string Action { get; set; }

        public FilmFrame()
        {
            Props = new List<PropInfo>();
        }

        public FilmFrame(Mobile actor, FilmState state, string arg)
        {
            FrameDouble = new BodyDouble();

            Props = new List<PropInfo>();

            SoundID = -1;

            Effect_Info = new EffectInfo(SETypes.None, Point3D.Zero);

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

            writer.Write(SoundID);

            writer.Write((int)Effect_Info.SE_Effect);

            writer.Write(Effect_Info.Location);
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

            writer.WriteLine(SoundID);

            writer.WriteLine((int)Effect_Info.SE_Effect);

            writer.WriteLine($"{Location.X}:{Location.Y}:{Location.Z}");
        }

        internal void Load(GenericReader reader, int version)
        {
            if (version > -1)
            {
                Location = reader.ReadPoint3D();

                MoveDirection = StudioEngine.GetDirection(reader.ReadString());

                string state = reader.ReadString();

                State = (FilmState)Enum.Parse(typeof(FilmState), state);

                Line = reader.ReadString();

                Action = reader.ReadString();

                FrameDouble = new BodyDouble();

                FrameDouble.Load(reader, version);

                Props = new List<PropInfo>();

                int countProp = reader.ReadInt();

                for (int i = 0; i < countProp; i++)
                {
                    var info = new PropInfo();

                    info.LoadPorp(reader, version);

                    Props.Add(info);
                }

                SoundID = reader.ReadInt();
            }

            if (version > 0)
            {
                Effect_Info = new EffectInfo((SETypes)reader.ReadInt(), reader.ReadPoint3D());
            }
        }

        internal void Import(StreamReader reader, int version)
        {
            if (version > -1)
            {
                Location = Point3D.Parse(reader.ReadLine());

                MoveDirection = (Direction)Enum.Parse(typeof(Direction), reader.ReadLine());

                State = (FilmState)Enum.Parse(typeof(FilmState), reader.ReadLine());

                Line = reader.ReadLine();

                Action = reader.ReadLine();

                FrameDouble = new BodyDouble();

                FrameDouble.Import(reader, version);

                var countProps = int.Parse(reader.ReadLine());

                for (int k = 0; k < countProps; k++)
                {
                    PropInfo prop = new PropInfo();

                    prop.Import(reader, version);

                    Props.Add(prop);
                }

                SoundID = int.Parse(reader.ReadLine());
            }

            if (version > 0)
            {
                SETypes type = (SETypes)int.Parse(reader.ReadLine());

                Point3D location = new Point3D(int.Parse(reader.ReadLine()), int.Parse(reader.ReadLine()), int.Parse(reader.ReadLine()));

                Effect_Info = new EffectInfo(type, location);
            }
        }
    }
}
