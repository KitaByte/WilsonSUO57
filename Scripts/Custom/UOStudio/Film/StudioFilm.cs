using System;
using System.IO;
using System.Linq;
using Server.Gumps;
using Server.Mobiles;
using System.Collections;
using Server.ContextMenus;
using System.Collections.Generic;

namespace Server.Custom.UOStudio
{
    public class StudioFilm : Item
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public bool PLAY
        {
            get { return IsPlaying; }
            set
            {
                PlayFilm(User);
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string FilmName { get { return Name; } set { Name = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public string LinkedFilm { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsLinkConcurrent { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool PlayOnMovement { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int OnMovementRange { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int FilmSpeed { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int FilmDelay { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool FilmAnim { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool UseFilmProps { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool InDebug { get { return StudioEngine.IsDebug; } set { StudioEngine.IsDebug = value; } }

        public VideoRecorder Recorder { get; private set; }

        public Map StageMap { get; set; }

        public Point2D StageStart { get; set; }

        public Point2D StageEnd { get; set; }

        public List<FilmFrame> Frames { get; set; }

        public ArrayList StageProps { get; set; }

        public bool IsPlaying { get; set; }

        public List<StudioProp> CurrentProps { get; set; }

        public void AddStageProps(List<StudioProp> props)
        {
            CurrentProps.AddRange(props);
        }

        private FilmPlayer _FilmPlayer;

        private Mobile User;

        internal int _PropID = 42;

        internal int _HueID = 0;

        internal int _SoundID = -1;

        public StudioFilm(VideoRecorder recorder, Map map) : this()
        {
            var date = DateTime.Now;

            Name = $"Film #{date.Year.ToString().Substring(2)}{date.DayOfYear}-{date.Hour}{date.Minute}";

            Hue = Utility.RandomMetalHue();

            Recorder = recorder;

            StageMap = map;

            StageStart = Point2D.Zero;

            StageEnd = Point2D.Zero;

            IsLinkConcurrent = true;

            PlayOnMovement = false;

            OnMovementRange = 3;

            IsPlaying = false;

            FilmSpeed = 2;

            FilmDelay = 1;

            FilmAnim = true;

            UseFilmProps = false;

            InDebug = false;
        }

        public StudioFilm() : base(0x0E34)
        {
            Frames = new List<FilmFrame>();

            StageProps = new ArrayList();

            LootType = LootType.Blessed;

            Weight = 1.0;
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.AccessLevel >= AccessLevel.GameMaster)
            {
                list.Add(new FilmEntry());
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.IsStaff() && Frames.Count > 0)
            {
                User = from;

                if (!Visible && InRange(from, 1))
                {
                    Visible = true;

                    Movable = true;

                    from.AddToBackpack(this);

                    from.SendMessage(53, "Film magically appears in your Backpack!");
                }
                else
                {
                    from.SendGump(new PropertiesGump(from, this));
                }
            }
        }

        public override bool HandlesOnMovement => PlayOnMovement;

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (PlayOnMovement && InRange(m.Location, OnMovementRange) && !IsPlaying)
            {
                PlayFilm(m);
            }
        }

        public override bool OnDroppedToWorld(Mobile from, Point3D p)
        {
            if (Parent == null)
            {
                Visible = false;

                Movable = false;

                from.SendMessage(53, "Film is locked down and not visible to players!");

                from.SendMessage(53, "Stand within 1 tile and double click to pick up!");
            }

            return base.OnDroppedToWorld(from, p);
        }

        public override void OnDelete()
        {
            User?.CloseGump(typeof(StudioGump));

            base.OnDelete();
        }

        public void AddFrame(Mobile actor, FilmState state, string arg)
        {
            if (Recorder != null && Recorder.IsRecording && GetStage().Contains(actor.Location))
            {
                Frames.Add(new FilmFrame(actor, state, arg));

                Frames.Last().Sounds.Add(_SoundID);

                _SoundID = -1;

                BaseGump.SendGump(new StudioGump(actor as PlayerMobile, Recorder));

                Recorder.UpdateBlockerColor();

                ClearCurrentProps();
            }
        }

        public void AddPropInfo(StudioProp prop, PropInfo info)
        {
            if (CurrentProps == null)
            {
                CurrentProps = new List<StudioProp>();
            }

            CurrentProps.Add(prop);

            if (Frames.Count > 0)
            {
                Frames.Last().Props.Add(info);
            }
        }

        public void RemovePropInfo(PropInfo info)
        {
            if (Frames.Count > 0)
            {
                if (Frames.Last().Props.Count > 0)
                {
                    var infoToRemove = Frames.Last().Props.First(i => i.IsSame(info));

                    if (infoToRemove != null)
                    {
                        Frames.Last().Props.Remove(infoToRemove);
                    }
                }
            }
        }

        private Rectangle2D stage = new Rectangle2D(Point2D.Zero, Point2D.Zero);

        internal Rectangle2D GetStage()
        {
            if (stage.Start == Point2D.Zero || stage.End == Point2D.Zero)
            {
                stage = new Rectangle2D(StageStart, StageEnd);
            }

            return stage;
        }

        private DateTime LastPlayed = DateTime.MinValue;

        internal void PlayFilm(Mobile from)
        {
            if (_FilmPlayer == null && LastPlayed < DateTime.Now - TimeSpan.FromMinutes(FilmDelay))
            {
                FilmStarter = from;

                if (FilmSpeed < 1)
                {
                    FilmSpeed = 2;
                }

                CurrentProps = new List<StudioProp>();

                _FilmPlayer = new FilmPlayer(this, TimeSpan.FromMilliseconds(FilmSpeed * 250));

                _FilmPlayer.Start();

                UpdateProps(true);

                IsPlaying = true;

                if (from.IsStaff() && StudioEngine.IsDebug)
                {
                    from.SendMessage(63, $"Started Film - {Name}!");
                }

                if (!string.IsNullOrEmpty(LinkedFilm) && IsLinkConcurrent)
                {
                    var film = World.Items.Values.First(f => f.Name == LinkedFilm);

                    if (film != null && film is StudioFilm sf)
                    {
                        sf.PlayFilm(from);
                    }
                }
            }
            else
            {
                if (from.IsStaff() && StudioEngine.IsDebug)
                {
                    from.SendMessage(43, "Currently busying playing film, please wait!");
                }
            }
        }

        private Mobile FilmStarter;

        internal void StopPlaying()
        {
            UpdateProps(false);

            ClearCurrentProps();

            LastPlayed = DateTime.Now;

            _FilmPlayer = null;

            IsPlaying = false;

            if (!string.IsNullOrEmpty(LinkedFilm) && !IsLinkConcurrent)
            {
                var film = World.Items.Values.First(f => f.Name == LinkedFilm);

                if (film != null && film is StudioFilm sf)
                {
                    if (FilmStarter != null)
                    {
                        sf.PlayFilm(FilmStarter);
                    }
                }
            }
        }

        internal void AddProp(Item prop)
        {
            prop.Visible = false;

            StageProps.Add(prop);
        }

        internal void RemoveProp(Item prop)
        {
            if (StageProps.Contains(prop))
            {
                StageProps.Remove(prop);
            }
        }

        internal void AddSound(int sound)
        {
            if (Frames != null && Frames.Count > 0)
            {
                Frames.Last().Sounds.Remove(Frames.Last().Sounds.Last());

                if (sound > -1 && sound < 0x683)
                {
                    Frames.Last().Sounds.Add(sound);
                }
                else
                {
                    Frames.Last().Sounds.Add(-1);
                }
            }
        }

        private void UpdateProps(bool isVisible)
        {
            int count = StageProps.Count;

            if (count > 0)
            {
                // clean
                for (int i = count - 1; i >= 0; i--)
                {
                    var item = StageProps[i];

                    if (item is Item _item && _item.Deleted)
                    {
                        StageProps.RemoveAt(i);
                    }
                }

                count = StageProps.Count;

                if (count > 0)
                {
                    // set visibility
                    for (int i = 0; i < count; i++)
                    {
                        var item = StageProps[i];

                        if (item is Item _item)
                        {
                            _item.Visible = isVisible;

                            if (!_item.Visible && !UseFilmProps)
                            {
                                _item.Visible = false;
                            }
                        }
                    }
                }
            }
        }

        private static readonly List<StudioProp> OldProps = new List<StudioProp>();

        internal void ClearCurrentProps()
        {
            if (CurrentProps != null && CurrentProps.Count > 0)
            {
                OldProps.AddRange(CurrentProps);

                CurrentProps.Clear();

                Timer.DelayCall(TimeSpan.FromMilliseconds(500), () =>
                {
                    if (OldProps.Count > 0)
                    {
                        int count = OldProps.Count;

                        for (int i = 0; i < count; i++)
                        {
                            try
                            {
                                if (!OldProps[i].Deleted)
                                {
                                    OldProps[i].Delete();
                                }
                            }
                            catch
                            {
                                break;
                            }
                        }
                    }

                    OldProps.Clear();
                });
            }
        }

        public StudioFilm(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(LinkedFilm);

            writer.Write(IsLinkConcurrent);

            writer.Write(PlayOnMovement);

            writer.Write(OnMovementRange);

            writer.Write(FilmSpeed);

            writer.Write(FilmDelay);

            writer.Write(FilmAnim);

            writer.Write(UseFilmProps);

            writer.Write(StageMap);

            writer.Write(StageStart);

            writer.Write(StageEnd);

            writer.Write(Frames.Count);

            foreach (var frame in Frames)
            {
                frame.Save(writer);
            }

            writer.WriteItemList(StageProps);
        }

        public void Export(StreamWriter writer)
        {
            writer.WriteLine(LinkedFilm);

            writer.Write(IsLinkConcurrent);

            writer.WriteLine(PlayOnMovement);

            writer.WriteLine(OnMovementRange);

            writer.WriteLine(FilmSpeed);

            writer.WriteLine(FilmDelay);

            writer.WriteLine(FilmAnim);

            writer.WriteLine(UseFilmProps);

            writer.WriteLine(StageMap);

            writer.WriteLine(StageStart);

            writer.WriteLine(StageEnd);

            writer.WriteLine(Frames.Count);

            foreach (var frame in Frames)
            {
                frame.Export(writer);
            }

            writer.WriteLine(StageProps.Count);

            if (StageProps.Count > 0)
            {
                foreach (var prop in StageProps)
                {
                    if (prop is StudioProp sp)
                    {
                        sp.Export(writer);
                    }
                }
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            LinkedFilm = reader.ReadString();

            if (StudioEngine.HasData(out int version) && version > 8)
            {
                IsLinkConcurrent = reader.ReadBool();
            }

            PlayOnMovement = reader.ReadBool();

            OnMovementRange = reader.ReadInt();

            FilmSpeed = reader.ReadInt();

            FilmDelay = reader.ReadInt();

            FilmAnim = reader.ReadBool();

            UseFilmProps = reader.ReadBool();

            StageMap = reader.ReadMap();

            StageStart = reader.ReadPoint2D();

            StageEnd = reader.ReadPoint2D();

            int count = reader.ReadInt();

            Frames = new List<FilmFrame>();

            for (int i = 0; i < count; i++)
            {
                var frame = new FilmFrame();

                frame.Load(reader);

                Frames.Add(frame);
            }

            StageProps = new ArrayList();

            StageProps = reader.ReadItemList();
        }

        public void Import(StreamReader reader)
        {
            LinkedFilm = reader.ReadLine();

            if (StudioEngine.HasData(out int version) && version > 8)
            {
                IsLinkConcurrent = bool.Parse(reader.ReadLine());
            }

            PlayOnMovement = bool.Parse(reader.ReadLine());

            OnMovementRange = int.Parse(reader.ReadLine());

            FilmSpeed = int.Parse(reader.ReadLine());

            FilmDelay = int.Parse(reader.ReadLine());

            FilmAnim = bool.Parse(reader.ReadLine());

            UseFilmProps = bool.Parse(reader.ReadLine());

            StageMap = Map.Parse(reader.ReadLine());

            StageStart = Point2D.Parse(reader.ReadLine());

            StageEnd = Point2D.Parse(reader.ReadLine());

            var frameCount = int.Parse(reader.ReadLine());

            for (int i = 0; i < frameCount; i++)
            {
                var frame = new FilmFrame();

                frame.Import(reader);

                Frames.Add(frame);
            }

            var countStageProps = int.Parse(reader.ReadLine());

            for (int i = 0; i < countStageProps; i++)
            {
                StudioProp prop = new StudioProp();

                prop.Import(reader);

                StageProps.Add(prop);
            }
        }
    }
}
