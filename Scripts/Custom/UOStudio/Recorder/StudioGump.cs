using Server.Gumps;
using Server.Mobiles;

namespace Server.Custom.UOStudio
{
    public class StudioGump : BaseGump
    {
        private VideoRecorder _Recorder;

        public StudioGump(PlayerMobile user, VideoRecorder recorder) : base(user, 50, 50, null)
        {
            _Recorder = recorder;
        }

        public override void AddGumpLayout()
        {
            Closable = true;
            Resizable = false;
            Dragable = true;

            AddBackground(X, Y, 150, 190, 40000);

            AddLabel(X + 50, Y + 12, 2720, "UO Studio");

            if (_Recorder.IsRecording)
            {
                AddButton(X + 15, Y + 41, 2360, 2361, 1, GumpButtonType.Reply, 0);
                AddLabel(X + 35, Y + 37, 2499, "Stop Recording");
            }
            else
            {
                AddButton(X + 15, Y + 41, 2361, 2360, 1, GumpButtonType.Reply, 0);
                AddLabel(X + 35, Y + 37, 2499, "Start Recording");
            }

            AddButton(X + 15, Y + 71, 2361, 2360, 2, GumpButtonType.Reply, 0);
            AddLabel(X + 35, Y + 67, 2499, "Add Prop");

            AddButton(X + 15, Y + 101, 2361, 2360, 3, GumpButtonType.Reply, 0);
            AddLabel(X + 35, Y + 97, 2499, "Add SFX");

            AddButton(X + 15, Y + 131, 2361, 2360, 4, GumpButtonType.Reply, 0);
            AddLabel(X + 35, Y + 127, 2499, "Add Mod");

            AddButton(X + 15, Y + 161, 2361, 2360, 5, GumpButtonType.Reply, 0);
            AddLabel(X + 35, Y + 157, 2499, "Cancel");
        }

        public override void OnResponse(RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case 0:
                    {
                        Close();

                        break;
                    }

                case 1:
                    {
                        _Recorder.IsRecording = !_Recorder.IsRecording;

                        _Recorder.UpdateBlockers();

                        var offset = StudioEngine.GetDirOS(User.Direction);

                        if (_Recorder.IsRecording)
                        {
                            User.SendMessage(63, "Started Recording!");

                            _Recorder.Film.AddFrame(User, FilmState.Move, $"{User.X + offset.X}:{User.Y + offset.Y}:{User.Z}");

                            Refresh(true, false);
                        }
                        else
                        {
                            User.SendMessage(43, "Stopped Recording!");

                            _Recorder.Film.AddFrame(User, FilmState.Move, $"{User.X + offset.X}:{User.Y + offset.Y}:{User.Z}");

                            _Recorder.Reset(User);

                            Close();
                        }

                        break;
                    }

                case 2:
                    {
                        SendGump(new StudioPropGump(User, _Recorder.Film, X + 25, Y + 130, this));

                        Refresh(true, false);

                        break;
                    }

                case 3:
                    {
                        SendGump(new StudioSoundGump(User, _Recorder.Film, X + 25, Y + 130, this));

                        Refresh(true, false);

                        break;
                    }

                case 4:
                    {
                        SendGump(new StudioModGump(User, X + 25, Y + 130, this));

                        Refresh(true, false);

                        break;
                    }

                case 5:
                    {
                        User.SendMessage(43, "Cancelled Recording!");

                        _Recorder.Film.Delete();

                        _Recorder.Reset(User);

                        Close();

                        break;
                    }
            }

            if (_Recorder.Blockers.Count > 0)
            {
                Refresh(true, false);
            }
        }

        public override void Close()
        {
            User.CloseGump(typeof(StudioPropGump));

            User.CloseGump(typeof(StudioSoundGump));

            User.CloseGump(typeof(StudioModGump));

            base.Close();
        }
    }
}
