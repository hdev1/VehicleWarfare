using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;

namespace VehicleWarfare
{

    public static class KillTracker
    {
        private static List<int> PedList = new List<int>();
        private static int PedTally = 0;

        private static List<int> VehicleList = new List<int>();
        private static int VehicleTally = 0;

        public static void Update()
        {
            Ped[] peds = World.GetNearbyPeds(Game.Player.Character, 2000);
            Vehicle[] vehicles = World.GetNearbyVehicles(Game.Player.Character, 2000);

            for (int i = 0; i < peds.Length; i++)
            {
                if (peds[i].Exists() && peds[i].HasBeenDamagedBy(Game.Player.Character) && peds[i].IsDead)
                {
                    if (!PedList.Contains(peds[i].GetHashCode()))
                    {
                        PedTally = PedTally + 1;
                        PedList.Add(peds[i].GetHashCode());

                        int killReward = 0;

                        if (peds[i].Model == PedHash.Cop01SMY || peds[i].Model == PedHash.Cop01SFY)
                        {
                            killReward = 10;
                        }
                        else
                        {
                            killReward = 5;
                        }

                        if (peds[i].IsInCombat) killReward += 2;
                        if (peds[i].IsInFlyingVehicle)
                        {
                            killReward += 15;
                        }
                        else if (peds[i].IsInBoat)
                        {
                            killReward += 10;
                        }
                        else if (peds[i].IsInPoliceVehicle)
                        {
                            killReward += 2;
                        }
                        else if (peds[i].IsInVehicle()) killReward += 5;

                        if (peds[i].IsVaulting || peds[i].IsFleeing || peds[i].IsAimingFromCover ||
                            peds[i].IsGoingIntoCover)
                        {
                            killReward += 2;
                        }

                        Game.Player.Money += killReward;
                    }
                }
            }

            for (int i = 0; i < vehicles.Length; i++)
            {
                if (vehicles[i].Exists() && vehicles[i].HasBeenDamagedBy(Game.Player.Character) && !vehicles[i].IsDriveable)
                {
                    if (!VehicleList.Contains(vehicles[i].GetHashCode()))
                    {
                        VehicleTally = VehicleTally + 1;
                        VehicleList.Add(vehicles[i].GetHashCode());

                        int killReward = 0;

                        if (vehicles[i].Model == VehicleHash.Police
                            || vehicles[i].Model == VehicleHash.Police2
                            || vehicles[i].Model == VehicleHash.Police3
                            || vehicles[i].Model == VehicleHash.Police4
                            || vehicles[i].Model == VehicleHash.Policeb
                            || vehicles[i].Model == VehicleHash.PoliceT
                            )
                        {

                            killReward = 10;
                        }
                        else
                        {
                            killReward = 5;
                        }

                        Game.Player.Money += killReward;
                    }
                }
            }


        }
    }
}
