using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using NativeUI;

namespace VehicleWarfare
{
    public static class VehicleController
    {
        public static float vehicleArmorMultiplier = 1.0f;
        public static List<SaveableVehicle> SaveableVehicles;
        public static List<Vehicle> NearbyVehicles = new List<Vehicle>();
        public static int VehicleTally = 0;
        
        public static readonly float NearbyVehicleRadius = 1000.0f;
        public static void Init()
        {
            SaveableVehicles = new List<SaveableVehicle>();
        }

        private static void UpdateVehicles()
        {
            foreach (var vehicle in SaveableVehicles)
            {
                var newBodyHealth = vehicle.GameVehicle.BodyHealth;
                var newHealth = vehicle.GameVehicle.Health;
                var newPetrolTankHealth = vehicle.GameVehicle.PetrolTankHealth;
                var newEngineHealth = vehicle.GameVehicle.EngineHealth;

                if (vehicle.ArmorLevel != 1.0f)
                {
                    float healthChange = vehicle.PreviousHealth - newHealth;
                    float bodyHealthChange = vehicle.PreviousBodyHealth - newBodyHealth;
                    float petrolTankHealthChange = vehicle.PreviousPetrolTankHealth - newPetrolTankHealth;
                    float engineHealthChange = vehicle.PreviousEngineHealth - newEngineHealth;

                    vehicle.GameVehicle.Health =
                        (int) (vehicle.PreviousHealth - (healthChange / vehicle.ArmorLevel));
                    vehicle.GameVehicle.BodyHealth =
                        (int) (vehicle.PreviousBodyHealth - (bodyHealthChange / vehicle.ArmorLevel));
                    vehicle.GameVehicle.PetrolTankHealth =
                        (int) (vehicle.PreviousPetrolTankHealth - (petrolTankHealthChange / vehicle.ArmorLevel));
                    vehicle.GameVehicle.EngineHealth =
                        (int) (vehicle.PreviousEngineHealth - (engineHealthChange / vehicle.ArmorLevel));
                }

                vehicle.PreviousHealth = vehicle.GameVehicle.Health;
                vehicle.PreviousBodyHealth = vehicle.GameVehicle.BodyHealth;
                vehicle.PreviousEngineHealth = vehicle.GameVehicle.EngineHealth;
                vehicle.PreviousPetrolTankHealth = vehicle.GameVehicle.PetrolTankHealth;

                if (vehicle.NitrousActivated)
                {
                    if (!vehicle.NitrousTimer.IsRunning) vehicle.NitrousTimer.Start();

                        if (vehicle.NitrousTimer.ElapsedMilliseconds >= 100) {
                            vehicle.NitrousAmount -= 0.1f;
                            vehicle.NitrousTimer.Restart();
                            vehicle.GameVehicle.EngineTorqueMultiplier = 10.0f;
                        }
                        //vehicle.GameVehicle.EnginePowerMultiplier = 50.0f;
                }
                else
                {
                    vehicle.NitrousTimer.Stop();
                }
            }
        }

        private static void UpdateNearbyVehicles(){
            NearbyVehicles = World.GetNearbyVehicles(Game.Player.Character, NearbyVehicleRadius).ToList();
        }
        public static void Update()
        {
            UpdateNearbyVehicles();

            // Debugging
            if (SaveableVehicles.Count > 0)
            {
                UpdateVehicles();
                if (SaveableVehicles[0].GameVehicle != null)
                {
                    //DebugInfo.Bar1.Text = SaveableVehicles[0].NitrousActivated.ToString();
                    //DebugInfo.Bar2.Text = SaveableVehicles[0].NitrousAmount.ToString();
                }

            }

            // Blip management
            if (SaveableVehicles.Count > 0)
            {
                foreach (var SaveableVehicle in SaveableVehicles)
                {
                    
                    if (Game.Player.Character.IsInVehicle())
                    {
                        if (SaveableVehicle.GameVehicle.GetHashCode() ==
                            Game.Player.Character.CurrentVehicle.GetHashCode())
                        {
                            if (BlipController.ContainsKey(SaveableVehicle.GameVehicle.GetHashCode().ToString()))
                                BlipController.Remove(SaveableVehicle.GameVehicle.GetHashCode().ToString());
                        }
                    }
                    else
                    {
                        if (!BlipController.ContainsKey(SaveableVehicle.GameVehicle.GetHashCode().ToString()) && SaveableVehicle.GameVehicle.IsDriveable)
                        {
                            BlipController.Add(SaveableVehicle.GameVehicle.GetHashCode().ToString(), SaveableVehicle.GameVehicle.Position, BlipSprite.SportsCar);
                        }
                        
                    }
                    if (!SaveableVehicle.GameVehicle.IsDriveable)
                    {
                        if (BlipController.ContainsKey(SaveableVehicle.GameVehicle.GetHashCode().ToString()))
                            BlipController.Remove(SaveableVehicle.GameVehicle.GetHashCode().ToString());
                        SaveableVehicle.IsSpawned = false;
                    }
                }

            }

        }

        public static List<Vehicle> GetNearbyVehiclesByFilter(Vector3 origin, Func<Vehicle, bool> filter) {
            return GetNearbyVehiclesByFilter(origin, filter, NearbyVehicles);
        }

        public static List<Vehicle> GetNearbyVehiclesByFilter(Vector3 origin, Func<Vehicle, bool> filter, List<Vehicle> sourceList) {
            List<Vehicle> _vehs = new List<Vehicle>();
            foreach (var veh in sourceList) {
                if (filter.Invoke(veh))  {
                    _vehs.Add(veh);
                }
            }
            return _vehs;
        }

        public static void GetNearbyVehiclesByFilterAndInvokeAction(Vector3 origin, Func<Vehicle, bool> filter, Action<Vehicle> action) {
            foreach (var veh in NearbyVehicles) {
                if (filter.Invoke(veh))  {
                    action.Invoke(veh);
                }
            }
        }

        public static List<Vehicle> GetDriveableNearbyVehicles(Vector3 origin, Func<Vehicle, bool> filter, List<Vehicle> sourceList) {
            List<Vehicle> _vehs = new List<Vehicle>();
            foreach (var veh in sourceList) {
                if (veh.IsDriveable) _vehs.Add(veh);
            }
            return _vehs;
        }
        
        

        public static List<Vehicle> GetNearbyVehiclesByRadius(Vector3 origin, float radius) {
            return GetNearbyVehiclesByRadius(origin, radius, NearbyVehicles);
        }

        public static List<Vehicle> GetNearbyVehiclesByRadius(Vector3 origin, float radius, List<Vehicle> sourceList) {
            List<Vehicle> _vehs = new List<Vehicle>();
            foreach (var veh in sourceList) {
                if (World.GetDistance(origin, veh.Position) < radius) {
                    _vehs.Add(veh);
                }
            }
            return _vehs;
        }
    }
    
}
