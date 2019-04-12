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
        public static Vehicle[] SavedVehicles;
        public static int MaxSavedVehicles = 3;

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
        }

        public static void Update()
        {
            if (Game.Player.Character.IsInVehicle())
            {
                MenuManager.TimerBarPool.Draw();
                var currentVehicle = Game.Player.Character.CurrentVehicle;

                currentVehicle.EngineTorqueMultiplier = 5.0f;

                if (previousBodyHealth == -1.0f)
                {
                    previousBodyHealth = currentVehicle.Health;
                }
                else
                {
                    var newBodyHealth = currentVehicle.BodyHealth;
                    var newHealth = currentVehicle.Health;
                    var newPetrolTankHealth = currentVehicle.PetrolTankHealth;
                    var newEngineHealth = currentVehicle.EngineHealth;

                    if (vehicleArmorMultiplier != 1)
                    {
                        float healthChange = previousHealth- newHealth;
                        float bodyHealthChange = previousBodyHealth - newBodyHealth;
                        float petrolTankHealthChange = previousPetrolTankHealth- newPetrolTankHealth;
                        float engineHealthChange = previousEngineHealth - newEngineHealth;

                        Game.Player.Character.CurrentVehicle.Health = (int)(previousHealth - (healthChange / vehicleArmorMultiplier));
                        Game.Player.Character.CurrentVehicle.BodyHealth = (int)(previousBodyHealth - (bodyHealthChange / vehicleArmorMultiplier));
                        Game.Player.Character.CurrentVehicle.PetrolTankHealth =
                            (int) (previousPetrolTankHealth - (petrolTankHealthChange / vehicleArmorMultiplier));
                        Game.Player.Character.CurrentVehicle.EngineHealth =
                            (int) (previousEngineHealth - (engineHealthChange / vehicleArmorMultiplier));
                    }

                    previousHealth = Game.Player.Character.CurrentVehicle.Health;
                    previousBodyHealth = Game.Player.Character.CurrentVehicle.BodyHealth;
                    previousEngineHealth = Game.Player.Character.CurrentVehicle.EngineHealth;
                    previousPetrolTankHealth = Game.Player.Character.CurrentVehicle.PetrolTankHealth;
                }
                
                currentVehicle.ToggleMod(VehicleToggleMod.Turbo, true);

                texttest.Text = Game.Player.Character.CurrentVehicle.EngineHealth.ToString();
                texttest1.Text = Game.Player.Character.CurrentVehicle.BodyHealth.ToString();
                vehicleDamageBar.Percentage = float.Parse(Game.Player.Character.CurrentVehicle.Health.ToString()) / float.Parse(Game.Player.Character.CurrentVehicle.MaxHealth.ToString());

            }

            // save vehicle coords
            /*if (Game.Player.Character.IsJumpingOutOfVehicle)
            {
                if (Game.Player.Character.LastVehicle.IsPersistent)
                {
                    for (int i=0;i<SavedVehicles.Length;i++)
                    {
                        if (SavedVehicles[i].Model == Game.Player.Character.LastVehicle.Model)
                        {
                            SavedVehicles[i] = Game.Player.Character.LastVehicle;
                        }
                    }
                }
            }


            if (SavedVehicles.Length > 0)
            {
                foreach (var vehicle in SavedVehicles)
                {
                    
                }
            }*/
        }

        public static void SaveCar(Vehicle vehicle)
        {
            SavedVehicles.Append(vehicle);
        }
    }
}
