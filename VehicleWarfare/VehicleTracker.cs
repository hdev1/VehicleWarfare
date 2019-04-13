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
                        float healthChange = previousHealth - newHealth;
                        float bodyHealthChange = previousBodyHealth - newBodyHealth;
                        float petrolTankHealthChange = previousPetrolTankHealth - newPetrolTankHealth;
                        float engineHealthChange = previousEngineHealth - newEngineHealth;

                        Game.Player.Character.CurrentVehicle.Health =
                            (int) (previousHealth - (healthChange / vehicleArmorMultiplier));
                        Game.Player.Character.CurrentVehicle.BodyHealth =
                            (int) (previousBodyHealth - (bodyHealthChange / vehicleArmorMultiplier));
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

                //currentVehicle.ToggleMod(VehicleToggleMod.Turbo, true);
                //Game.Player.Character.CurrentVehicle.FriendlyName = "dAMN";
                
                texttest1.Text = vehicleArmorMultiplier.ToString() + " " + SavedVehicles.Count.ToString();
                //texttest1.Text = Game.Player.Character.CurrentVehicle.BodyHealth.ToString();
                vehicleDamageBar.Percentage = float.Parse(Game.Player.Character.CurrentVehicle.Health.ToString()) /
                                              float.Parse(Game.Player.Character.CurrentVehicle.MaxHealth.ToString());

            }
            //texttest.Text = Game.Player.Character.LastVehicle.IsDriveable.ToString();

            if (SavedVehicles.Count > 0)
            {
                if (SavedVehicles[0].GameVehicle != null)
                {
                    texttest.Text = SavedVehicles[0].GameVehicle.Position.X.ToString();
                }
            }

            // show blips
            foreach (var savedVehicle in SavedVehicles)
            {
                if (Blips.ContainsKey(savedVehicle.DisplayName))
                {
                    foreach (var blip in Blips)
                    {
                        if (blip.Key == savedVehicle.DisplayName)
                        {
                            blip.Value.Position = savedVehicle.LastPosition;
                        }
                    }
                }
            }
            // save vehicle coords
            if (SavedVehicles.Count > 0)
            {
                if (!Game.Player.Character.IsInVehicle())
                {
                    if (Game.Player.Character.LastVehicle.IsPersistent)
                    {
                        foreach (var savedVehicle in SavedVehicles)
                        {
                            if (savedVehicle.DisplayName == Game.Player.Character.LastVehicle.DisplayName)
                            {
                                if (!Game.Player.Character.LastVehicle.IsDriveable)
                                {
                                    savedVehicle.IsSpawned = false;
                                    foreach (var blip in Blips.ToList())
                                    {
                                        if (blip.Key == Game.Player.Character.CurrentVehicle.DisplayName)
                                        {
                                            blip.Value.Remove();
                                            Blips.Remove(blip.Key);
                                            
                                        }
                                    }
                                }
                                else
                                {
                                    //savedVehicle.GameVehicle = Game.Player.Character.LastVehicle;
                                    savedVehicle.LastPosition = Game.Player.Character.LastVehicle.Position;

                                    if (!Blips.ContainsKey(savedVehicle.DisplayName))
                                    {
                                        var blip = World.CreateBlip(savedVehicle.LastPosition);
                                        blip.Sprite = BlipSprite.SportsCar;
                                        Blips.Add(savedVehicle.DisplayName, blip);
                                    }
                                }
                                
                            }
                        }
                    }
                }
                else if (Game.Player.Character.IsInVehicle())
                {
                    foreach (var t in SavedVehicles)
                    {
                        if (t.Model == Game.Player.Character.CurrentVehicle.Model && Game.Player.Character.CurrentVehicle.IsPersistent)
                        {
                            if (!Game.Player.Character.CurrentVehicle.IsDriveable)
                            {
                                t.IsSpawned = false;
                                
                            } else { 
                            if (vehicleArmorMultiplier != t.ArmorLevel)
                                vehicleArmorMultiplier = t.ArmorLevel;
                                //SavedVehicles[i].GameVehicle = Game.Player.Character.CurrentVehicle;
                            }
                            foreach (var blip in Blips.ToList())
                            {
                                if (blip.Key == Game.Player.Character.CurrentVehicle.DisplayName)
                                {
                                    blip.Value.Remove();
                                    Blips.Remove(blip.Key);
                                }
                            }
                        }
                        else
                        {
                            
                            vehicleArmorMultiplier = 1.0f;
                        }
                    }
                }

            }

            /*public static void SaveCar(SavedVehicle vehicle)
        {
            SavedVehicles.Append(vehicle);
        }*/
        }
    }
}
