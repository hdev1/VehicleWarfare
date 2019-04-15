using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GTA;
using NativeUI;

namespace VehicleWarfare.Menus
{
    class VehicleMenu
    {
        public VehicleMenu()
        {
            
        }

        public UIMenu GetMenu()
        {
            UIMenu vehicleMenu = new UIMenu("Vehicle Menu", "Customize your vehicle.");

            var saveVehicle = new UIMenuItem("Save current vehicle");
            var spawnVehicle = new UIMenuItem("Spawn vehicle 1");

            var armorLevels = new List<dynamic>()
            {
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "100"
            };
            var armor = new UIMenuListItem("Armor level", armorLevels, 0); 
            vehicleMenu.AddItem(armor);
            vehicleMenu.AddItem(saveVehicle);
            vehicleMenu.AddItem(spawnVehicle);
            vehicleMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == saveVehicle)
                {
                    Game.Player.Character.CurrentVehicle.IsPersistent = true;
                    SavedVehicle savedVehicle = new SavedVehicle
                    {
                        ArmorLevel = VehicleTracker.vehicleArmorMultiplier,
                        IsSpawned = true,
                    };
                    //UI.Notify(Game.Player.Character.CurrentVehicle.GetHashCode().ToString() + "-" + savedVehicle.HashCode.ToString());
                    //UI.Notify();

                    //var blip = World.CreateBlip(Game.Player.Character.Position);
                    //blip.Sprite = BlipSprite.SportsCar;

                    var veh = World.GetNearbyVehicles(Game.Player.Character.Position, 2000);
                    foreach (var v in veh)
                    {
                        if (v.GetHashCode() == Game.Player.Character.CurrentVehicle.GetHashCode())
                        {
                            savedVehicle.GameVehicle = v;
                            savedVehicle.PreviousHealth = v.Health;
                            savedVehicle.PreviousBodyHealth = v.BodyHealth;
                            savedVehicle.PreviousEngineHealth = v.EngineHealth;
                            savedVehicle.PreviousPetrolTankHealth = v.PetrolTankHealth;
                        }
                    }

                    //VehicleTracker.Blips.Add(savedVehicle.DisplayName, blip);
                        
                    VehicleTracker.SavedVehicles.Add(savedVehicle);
                } else if (item == spawnVehicle)
                {
                    if (VehicleTracker.SavedVehicles.Count > 0)
                    {
                        if (!VehicleTracker.SavedVehicles[0].IsSpawned)
                        {
                            VehicleTracker.SavedVehicles[0].IsSpawned = true;
                            var vehicle = World.CreateVehicle(VehicleTracker.SavedVehicles[0].GameVehicle.Model, Game.Player.Character.Position);

                            vehicle.PlaceOnGround();

                            VehicleTracker.SavedVehicles[0].GameVehicle = vehicle;

                            VehicleTracker.SavedVehicles[0].PreviousHealth = vehicle.Health;
                            VehicleTracker.SavedVehicles[0].PreviousBodyHealth = vehicle.BodyHealth;
                            VehicleTracker.SavedVehicles[0].PreviousEngineHealth = vehicle.EngineHealth;
                            VehicleTracker.SavedVehicles[0].PreviousPetrolTankHealth = vehicle.PetrolTankHealth;

                            //var blip = World.CreateBlip(vehicle.Position);
                            //blip.Sprite = BlipSprite.SportsCar;
                            //VehicleTracker.Blips.Add(vehicle.DisplayName, blip);

                        }
                    }
                }
            };
            vehicleMenu.OnListChange += (sender, item, index) =>
                {
                    if (item == armor) VehicleTracker.vehicleArmorMultiplier = float.Parse(armorLevels.ToArray()[item.Index]);
                };

            return vehicleMenu;
        }
    }
}
