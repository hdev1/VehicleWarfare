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
                    SaveableVehicle SaveableVehicle = new SaveableVehicle
                    {
                        ArmorLevel = VehicleController.vehicleArmorMultiplier,
                        IsSpawned = true,
                    };
                    //UI.Notify(Game.Player.Character.CurrentVehicle.GetHashCode().ToString() + "-" + SaveableVehicle.HashCode.ToString());
                    //UI.Notify();

                    //var blip = World.CreateBlip(Game.Player.Character.Position);
                    //blip.Sprite = BlipSprite.SportsCar;

                    var veh = World.GetNearbyVehicles(Game.Player.Character.Position, 2000);
                    foreach (var v in veh)
                    {
                        if (v.GetHashCode() == Game.Player.Character.CurrentVehicle.GetHashCode())
                        {
                            SaveableVehicle.GameVehicle = v;
                            SaveableVehicle.PreviousHealth = v.Health;
                            SaveableVehicle.PreviousBodyHealth = v.BodyHealth;
                            SaveableVehicle.PreviousEngineHealth = v.EngineHealth;
                            SaveableVehicle.PreviousPetrolTankHealth = v.PetrolTankHealth;
                        }
                    }

                    //VehicleTracker.Blips.Add(SaveableVehicle.DisplayName, blip);
                        
                    VehicleController.SaveableVehicles.Add(SaveableVehicle);
                } else if (item == spawnVehicle)
                {
                    if (VehicleController.SaveableVehicles.Count > 0)
                    {
                        if (!VehicleController.SaveableVehicles[0].IsSpawned)
                        {
                            VehicleController.SaveableVehicles[0].IsSpawned = true;
                            var vehicle = World.CreateVehicle(VehicleController.SaveableVehicles[0].GameVehicle.Model, Game.Player.Character.Position);
                            vehicle.PlaceOnGround();
                            VehicleController.SaveableVehicles[0].GameVehicle = vehicle;

                            VehicleController.SaveableVehicles[0].PreviousHealth = vehicle.Health;
                            VehicleController.SaveableVehicles[0].PreviousBodyHealth = vehicle.BodyHealth;
                            VehicleController.SaveableVehicles[0].PreviousEngineHealth = vehicle.EngineHealth;
                            VehicleController.SaveableVehicles[0].PreviousPetrolTankHealth = vehicle.PetrolTankHealth;
                            VehicleController.SaveableVehicles[0].GameVehicle.IsPersistent = true;
                        }
                    }
                }
            };
            vehicleMenu.OnListChange += (sender, item, index) =>
                {
                    if (item == armor) VehicleController.vehicleArmorMultiplier = float.Parse(armorLevels.ToArray()[item.Index]);
                };

            return vehicleMenu;
        }
    }
}
