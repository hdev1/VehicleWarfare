using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

namespace VehicleWarfare
{
    public class SavedVehicle
    {
        public Vehicle GameVehicle;
        public float ArmorLevel = 1;
        public bool IsSpawned;

        public float PreviousHealth;
        public float PreviousBodyHealth;
        public float PreviousPetrolTankHealth;
        public float PreviousEngineHealth;

        public float NitrousCapacity = 10.0f;
        public float NitrousPower = 10.0f;
        public float NitrousAmount = 10.0f;
        public bool NitrousActivated = false;
        public Stopwatch NitrousTimer = new Stopwatch();
        public Stopwatch NitrousTick = new Stopwatch();
    }
}
