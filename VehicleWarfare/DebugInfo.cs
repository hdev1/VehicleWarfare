using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;
using NativeUI;

namespace VehicleWarfare
{
    public static class DebugInfo
    {
        public static TextTimerBar Bar1;
        public static TextTimerBar Bar2;

        public static void Init() {
            Bar1 = new TextTimerBar("CR XYZ", "");
            Bar2 = new TextTimerBar("CP XYZ", "");

            UIManager.TimerBarPool.Add(Bar1);
            UIManager.TimerBarPool.Add(Bar2);
        }
    }
}
