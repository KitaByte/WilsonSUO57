using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Server.Custom.RumorMill
{
    internal static class RumorCore
    {
        private static List<IRumor> _Rumors;

        internal static void LoadRumors()
        {
            _Rumors = new List<IRumor>();

            Assembly assembly = Assembly.GetExecutingAssembly();

            var iRumorList = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IRumor)) && t.GetConstructor(Type.EmptyTypes) != null);

            foreach (var type in iRumorList)
            {
                IRumor instance = (IRumor)Activator.CreateInstance(type);

                AddRumor(instance);
            }
        }

        private static void AddRumor(IRumor rumor)
        {
            if (_Rumors != null && !_Rumors.Contains(rumor))
            {
                _Rumors.Add(rumor);

                rumor.InitRumor();
            }
        }

        private static void RemoveRumor(IRumor rumor)
        {
            if (_Rumors != null && _Rumors.Contains(rumor))
            {
                _Rumors.Remove(rumor);

                rumor.EndRumor();
            }
        }

        internal static void SpreadRumor(Mobile from, Mobile to)
        {
            if (_Rumors != null && _Rumors.Count > 0)
            {
                _Rumors[GetRandomRumor()].SpreadRumor(from, to);
            }
        }

        private static int GetRandomRumor()
        {
            return Utility.RandomMinMax(0, (_Rumors.Count - 1));
        }

        internal static void ResetRumor(IRumor rumor)
        {
            if (_Rumors != null && _Rumors.Count > 0)
            {
                if (_Rumors.Contains(rumor))
                {
                    RemoveRumor(rumor);

                    AddRumor(rumor);
                }
            }
        }

        internal static void RunCleanUp()
        {
            if (_Rumors != null && _Rumors.Count > 0)
            {
                for (int i = 0; i < _Rumors.Count; i++)
                {
                    _Rumors[i].EndRumor();
                }
            }
        }
    }
}
