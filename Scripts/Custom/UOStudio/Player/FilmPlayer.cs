using System;
using System.Linq;
using System.Collections.Generic;

namespace Server.Custom.UOStudio
{
    public class FilmPlayer : Timer
    {
        private readonly StudioFilm _Film;

        private int _FrameCounter;

        StudioActor actor = null;

        public FilmPlayer(StudioFilm film, TimeSpan interval) : base(interval, interval)
        {
            Priority = TimerPriority.TwoFiftyMS;

            _Film = film;

            _FrameCounter = 0;
        }

        protected override void OnTick()
        {
            if (StudioEngine.IsPaused)
            {
                // do nothing
            }
            else
            {
                FilmFrame frame = _Film.Frames[_FrameCounter];

                if (_Film.Frames.Count > 0)
                {
                    PlayStudioSound(frame);

                    if (_FrameCounter == 0)
                    {
                        CreateActor(frame);

                        if (StudioEngine.IsDebug)
                        {
                            SendStaffMessage(0, 43);
                        }
                    }
                    else
                    {
                        if (actor != null && ValidateActor())
                        {
                            UpdateBodyMod(frame);

                            actor.Direction = frame.MoveDirection;

                            switch (frame.State)
                            {
                                case FilmState.Move:
                                    {
                                        Point3D loc = frame.GetNextLocation();

                                        actor.Direction = actor.GetDirectionTo(loc);

                                        actor.Move(actor.Direction);

                                        if (actor.Location != loc)
                                        {
                                            actor.MoveToWorld(loc, actor.Map);
                                        }

                                        break;
                                    }

                                case FilmState.Line:
                                    {
                                        actor.Say(frame.Line);

                                        break;
                                    }

                                case FilmState.Action:
                                    {
                                        StudioEngine.RunAction(actor, frame.Action);

                                        break;
                                    }
                            }
                        }

                        if (StudioEngine.IsDebug)
                        {
                            SendStaffMessage(_FrameCounter, _FrameCounter + 1);
                        }
                    }

                    UpdateProps(frame);

                    _FrameCounter++;
                }

                if (_FrameCounter == _Film.Frames.Count)
                {
                    DelayCall(TimeSpan.FromMilliseconds(250 * _Film.FilmSpeed), () =>
                    {
                        if (ValidateActor())
                        {
                            RemoveActor(actor, _Film);
                        }

                        _Film.StopPlaying();
                    });

                    if (StudioEngine.IsDebug)
                    {
                        SendStaffMessage(-1, 43);
                    }

                    Stop();
                }
            }
        }

        private void SendStaffMessage(int frame, int hue)
        {
            foreach (var mobile in World.Mobiles.Values.Where(m => m.IsStaff()))
            {
                if (_Film.InRange(mobile.Location, 10))
                {
                    if (frame != -1)
                    {
                        mobile.SendMessage(hue, $"Frame {frame}");
                    }
                    else
                    {
                        mobile.SendMessage(hue, "Frame Ended!");
                    }
                }
            }
        }

        private void PlayStudioSound(FilmFrame frame)
        {
            if (frame.Sounds != null && frame.Sounds.Count > _FrameCounter && frame.Sounds[_FrameCounter] != -1)
            {
                Effects.PlaySound(frame.Location, _Film.StageMap, frame.Sounds[_FrameCounter]);
            }
        }

        private void UpdateProps(FilmFrame frame)
        {
            if (frame.Props.Count > 0)
            {
                List<StudioProp> props = new List<StudioProp>();

                foreach (var info in frame.Props)
                {
                    StudioProp prop = new StudioProp(info.ID, info.HUE);

                    prop.MoveToWorld(new Point3D(info.X, info.Y, info.Z), _Film.StageMap);

                    props.Add(prop);
                }

                _Film.ClearCurrentProps();

                _Film.AddStageProps(props);
            }
            else
            {
                _Film.ClearCurrentProps();
            }
        }

        private bool ValidateActor()
        {
            if (!actor.Deleted && actor.Alive)
            {
                return true;
            }

            return false;
        }

        private static void RemoveActor(Mobile oldActor, StudioFilm film)
        {
            oldActor.Hidden = true;

            if (film.FilmAnim)
            {
                StudioEngine.PlayStudioEffects(oldActor.Location, oldActor.Map);
            }

            DelayCall(TimeSpan.FromMilliseconds(250 * film.FilmSpeed), () =>
            {
                oldActor.Delete();
            });
        }

        private void CreateActor(FilmFrame frame)
        {
            if (_Film.FilmAnim)
            {
                StudioEngine.PlayStudioEffects(frame.Location, _Film.StageMap);
            }

            actor = new StudioActor(frame.FrameDouble)
            {
                Hidden = true
            };

            actor.MoveToWorld(frame.Location, _Film.StageMap);

            actor.Direction = frame.MoveDirection;
        }

        private void UpdateBodyMod(FilmFrame frame)
        {
            actor.NameMod = frame.GetName();

            actor.BodyMod = frame.GetBody();

            actor.HueMod = frame.GetHue();

            actor.Hidden = _Film.HideActor;
        }
    }
}
