using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using NativeUI;

namespace VehicleWarfare
{
    public static class VehicleTracker
    {
        private static float previousBodyHealth = -1.0f;
        private static float previousEngineHealth = -1.0f;
        private static float previousPetrolTankHealth = -1.0f;
        private static float previousHealth = -1.0f;
        public static float vehicleArmorMultiplier = 1.0f;
        private static BarTimerBar vehicleDamageBar;
        private static TextTimerBar texttest;
        private static TextTimerBar texttest1;
        private static BarTimerBar nitrousBar;
        private static Stopwatch nitrousTimer;
        public static bool NitrousActivated = false;
        public static List<SavedVehicle> SavedVehicles;
        public static int MaxSavedVehicles = 3;
        public static Dictionary<string, Blip> Blips = new Dictionary<string, Blip>();

        public static void Init()
        {
            nitrousTimer = new Stopwatch();
            nitrousBar = new BarTimerBar("N2O");
            vehicleDamageBar = new BarTimerBar("Vehicle Health");
            vehicleDamageBar.Percentage = 0.5f;
            texttest = new TextTimerBar("Engine Health", "");
            texttest1 = new TextTimerBar("Body Health", "");
            MenuManager.TimerBarPool.Add(vehicleDamageBar);
            MenuManager.TimerBarPool.Add(texttest);
            MenuManager.TimerBarPool.Add(texttest1);
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
                        (int) (vehicle.PreviousHealth - (healthChange / vehicleArmorMultiplier));
                    vehicle.GameVehicle.BodyHealth =
                        (int) (vehicle.PreviousBodyHealth - (bodyHealthChange / vehicleArmorMultiplier));
                    vehicle.GameVehicle.PetrolTankHealth =
                        (int) (vehicle.PreviousPetrolTankHealth - (petrolTankHealthChange / vehicleArmorMultiplier));
                    vehicle.GameVehicle.EngineHealth =
                        (int) (vehicle.PreviousEngineHealth - (engineHealthChange / vehicleArmorMultiplier));
                }

                vehicle.PreviousHealth = vehicle.GameVehicle.Health;
                vehicle.PreviousBodyHealth = vehicle.GameVehicle.BodyHealth;
                vehicle.PreviousEngineHealth = vehicle.GameVehicle.EngineHealth;
                vehicle.PreviousPetrolTankHealth = vehicle.GameVehicle.PetrolTankHealth;

                if (vehicle.NitrousActivated)
                {
                    if (!vehicle.NitrousTimer.IsRunning) vehicle.NitrousTimer.Start();
                    if (vehicle.NitrousAmount > 0.0f)
                    {
                        
                    }
                }
                else
                {
                    vehicle.NitrousTimer.Stop();
                }
            }


        }

        public static void Update()
        {
            //if (Game.Player.Character.IsInVehicle())
            //{
                MenuManager.TimerBarPool.Draw();
            //}

                //currentVehicle.ToggleMod(VehicleToggleMod.Turbo, true);
                //Game.Player.Character.CurrentVehicle.FriendlyName = "dAMN";
                
                //texttest1.Text = vehicleArmorMultiplier.ToString() + " " + SavedVehicles.Count.ToString();
                //texttest1.Text = Game.Player.Character.CurrentVehicle.BodyHealth.ToString();
                //vehicleDamageBar.Percentage = float.Parse(Game.Player.Character.CurrentVehicle.Health.ToString()) /
                  //                            float.Parse(Game.Player.Character.CurrentVehicle.MaxHealth.ToString());

            //texttest.Text = Game.Player.Character.LastVehicle.IsDriveable.ToString();

            if (SavedVehicles.Count > 0)
            {
                UpdateVehicles();
                if (SavedVehicles[0].GameVehicle != null)
                {
                    texttest.Text = SavedVehicles[0].NitrousAmount.ToString();
                }

            }

            



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
    }
}
