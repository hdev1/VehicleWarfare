using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;
using System.Drawing;

namespace VehicleWarfare
{

    public static class KillController
    {
        private static List<int> PedsKilled = new List<int>();

        private static List<int> VehiclesDestroyed = new List<int>();
        public static void Update()
        {
            Ped[] peds = World.GetNearbyPeds(Game.Player.Character, 1000.0f);
            
            VehicleController.GetNearbyVehiclesByFilter(Game.Player.Character.Position, 
                Filters.IsVehicleDestroyedByPlayer
            ).ForEach(
                veh => {
                    if (!VehiclesDestroyed.Contains(veh.GetHashCode()))
                    {
                        int killReward = 0;

                        if (Filters.IsPoliceVehicle(veh))
                        {
                            killReward = 100;
                        }
                        else
                        {
                            killReward = 5;
                        }

                        Game.Player.Money += killReward;
                        VehiclesDestroyed.Add(veh.GetHashCode());
                    }
                }
            );

            // TODO: PedTracker.cs
            for (int i = 0; i < peds.Length; i++)
            {
                if (peds[i].Exists() && peds[i].HasBeenDamagedBy(Game.Player.Character) && peds[i].IsDead)
                {
                    if (!PedsKilled.Contains(peds[i].GetHashCode()))
                    {
                        PedsKilled.Add(peds[i].GetHashCode());

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

        }
    }
}
