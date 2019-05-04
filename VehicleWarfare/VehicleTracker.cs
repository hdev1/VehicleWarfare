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
    public static class VehicleTracker
    {
        public static float vehicleArmorMultiplier = 1.0f;
        private static BarTimerBar vehicleDamageBar;
        public static List<SavedVehicle> SavedVehicles;
        public static List<Vehicle> NearbyVehicles = new List<Vehicle>();
        public static int VehicleTally = 0;
        
        public static readonly float NearbyVehicleRadius = 1000.0f;
        public static void Init()
        {
            SavedVehicles = new List<SavedVehicle>();
        }

        private static void UpdateVehicles()
        {
            foreach (var vehicle in SavedVehicles)
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
            if (SavedVehicles.Count > 0)
            {
                UpdateVehicles();
                if (SavedVehicles[0].GameVehicle != null)
                {
                    //DebugInfo.Bar1.Text = SavedVehicles[0].NitrousActivated.ToString();
                    //DebugInfo.Bar2.Text = SavedVehicles[0].NitrousAmount.ToString();
                }

            }

            // Blip management
            if (SavedVehicles.Count > 0)
            {
                foreach (var savedVehicle in SavedVehicles)
                {
                    
                    if (Game.Player.Character.IsInVehicle())
                    {
                        if (savedVehicle.GameVehicle.GetHashCode() ==
                            Game.Player.Character.CurrentVehicle.GetHashCode())
                        {
                            if (BlipManager.ContainsKey(savedVehicle.GameVehicle.GetHashCode().ToString()))
                                BlipManager.Remove(savedVehicle.GameVehicle.GetHashCode().ToString());
                        }
                    }
                    else
                    {
                        if (!BlipManager.ContainsKey(savedVehicle.GameVehicle.GetHashCode().ToString()) && savedVehicle.GameVehicle.IsDriveable)
                        {
                            BlipManager.Add(savedVehicle.GameVehicle.GetHashCode().ToString(), savedVehicle.GameVehicle.Position, BlipSprite.SportsCar);
                        }
                        
                    }
                    if (!savedVehicle.GameVehicle.IsDriveable)
                    {
                        if (BlipManager.ContainsKey(savedVehicle.GameVehicle.GetHashCode().ToString()))
                            BlipManager.Remove(savedVehicle.GameVehicle.GetHashCode().ToString());
                        savedVehicle.IsSpawned = false;
                    }
                }

            }

        }

        public static void LoadDataIntoVehicle(SavedVehicle data) {
            //TODO            
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
