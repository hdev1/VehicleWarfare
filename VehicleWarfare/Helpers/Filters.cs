using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;
using NativeUI;

namespace VehicleWarfare
{
    public static class Filters
    {
        public static Func<Vehicle, bool> IsPoliceVehicle = (veh) =>
        (
            veh.Model == VehicleHash.Police
            || veh.Model == VehicleHash.Police2
            || veh.Model == VehicleHash.Police3
            || veh.Model == VehicleHash.Police4
            || veh.Model == VehicleHash.Policeb
            || veh.Model == VehicleHash.PoliceT
            || veh.Model == VehicleHash.Sheriff
            || veh.Model == VehicleHash.Sheriff2
            || veh.Model == VehicleHash.FBI
            || veh.Model == VehicleHash.FBI2
        );

        public static Func<Vehicle, bool> IsVehicleDestroyedByPlayer = (veh) =>
        (
            veh.Exists() &&
            veh.HasBeenDamagedBy(Game.Player.Character) &&
            !veh.IsDriveable
        );

        public static Func<Vehicle, bool> IsPedKilledByPlayer = (ped) =>
        (
            ped.Exists() &&
            ped.HasBeenDamagedBy(Game.Player.Character) &&
            !ped.IsDriveable
        );

        
    }
}
