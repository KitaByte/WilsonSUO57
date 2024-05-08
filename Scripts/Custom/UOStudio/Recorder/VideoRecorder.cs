using System.Linq;
using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Custom.UOStudio
{
    public class VideoRecorder : Item
    {
        public bool IsRecording { get; set; }

        internal List<Item> Blockers { get; private set; }

        internal StudioFilm Film { get; private set; }

        [Constructable]
        public VideoRecorder() : base(0x14F5)
        {
            Name = "Video Recorder";

            Hue = 2734;

            Weight = 1.0;

            LootType = LootType.Blessed;

            IsRecording = false;
        }

        public VideoRecorder(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.IsStaff())
            {
                if (Blockers == null)
                {
                    Blockers = new List<Item>();
                }

                if (Film == null)
                {
                    from.SendMessage(53, "Target top left corner of stage!");

                    Film = new StudioFilm(this, from.Map);

                    from.Target = new StudioTarget(this, Film);
                }
                else
                {
                    BaseGump.SendGump(new StudioGump(from as PlayerMobile, this));
                }
            }
        }

        internal void UpdateBlockers()
        {
            if (Blockers.Count > 0)
            {
                for (int i = 0; i < Blockers.Count; i++)
                {
                    Blockers[i].Visible = IsRecording;
                }
            }
        }

        internal void UpdateBlockerColor()
        {
            if (Blockers.Count > 0)
            {
                for (int i = 0; i < Blockers.Count; i++)
                {
                    switch (Utility.Random(2))
                    {
                        case 0:
                            {
                                Blockers[i].Hue = 1175; // black

                                break;
                            }

                        case 1:
                            {
                                Blockers[i].Hue = 2734; // gold

                                break;
                            }
                    }
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }

        internal void Reset(Mobile from)
        {
            if (Film != null && Film.Frames.Count == 0)
            {
                Film.Delete();
            }

            Film = null;

            Hue = 2734;

            StudioEngine.RemoveActor(from);

            StudioEngine.RemoveBlockers(this);

            StudioEngine.ResetMods(from as PlayerMobile);

            from.SendMessage(43, "Stage Cleared!");
        }
    }

    sealed class StudioTarget : Target
    {
        private readonly VideoRecorder _Recorder;

        private readonly StudioFilm _Film;

        public StudioTarget(VideoRecorder recorder, StudioFilm film) : base(24, true, TargetFlags.None)
        {
            _Recorder = recorder;

            _Film = film;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is IPoint3D p && (p is StaticTarget || p is LandTarget))
            {
                if (_Film.StageStart == Point2D.Zero)
                {
                    _Film.StageStart = new Point2D(p.X, p.Y);

                    from.SendMessage(53, "Target bottom right corner of stage!");

                    from.Target = new StudioTarget(_Recorder, _Film);

                    _Recorder.Hue = 2500;
                }
                else if (_Film.StageEnd == Point2D.Zero)
                {
                    _Film.StageEnd = new Point2D(p.X, p.Y);

                    if (_Film.StageStart > _Film.StageEnd)
                    {
                        (_Film.StageEnd, _Film.StageStart) = (_Film.StageStart, _Film.StageEnd);
                    }

                    var props = World.Items.Values.Where(i => i is StudioProp).ToList();

                    if (props.Count > 0)
                    {
                        foreach (var prop in props)
                        {
                            if (_Film.StageMap == prop.Map && _Film.GetStage().Contains(prop.Location))
                            {
                                _Film.AddProp(prop);
                            }
                        }
                    }

                    StudioEngine.SetUpBarrier(from, _Recorder, _Film.GetStage(), _Film.StageMap);

                    if (from is PlayerMobile pm)
                    {
                        var x = _Film.StageStart.X + _Film.GetStage().Width / 2;

                        var y = _Film.StageStart.Y + _Film.GetStage().Height / 2;

                        pm.MoveToWorld(new Point3D(x, y, p.Z), _Film.StageMap);

                        if (!StudioEngine.Actors.ContainsKey(pm))
                        {
                            StudioEngine.AddActor(pm, _Film);
                        }

                        BaseGump.SendGump(new StudioGump(pm, _Recorder));
                    }

                    _Recorder.Hue = 2752;

                    from.AddToBackpack(_Film);

                    from.SendMessage(53, "Stage Set : Film added to your backpack!");
                }
            }
        }
    }
}
