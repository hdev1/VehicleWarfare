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
                        GameVehicle = Game.Player.Character.CurrentVehicle,
                        ArmorLevel = VehicleTracker.vehicleArmorMultiplier,
                        IsSpawned = true
                    };

                    
                    VehicleTracker.SavedVehicles.Add(savedVehicle);
                } else if (item == spawnVehicle)
                {
                    if (VehicleTracker.SavedVehicles.Count > 0)
                    {
                        if (!VehicleTracker.SavedVehicles[0].IsSpawned)
                        {
                            VehicleTracker.SavedVehicles[0].IsSpawned = true;
                            var vehicle = World.CreateVehicle(VehicleTracker.SavedVehicles[0].GameVehicle.Model, Game.Player.Character.Position);
                            vehicle = VehicleTracker.SavedVehicles[0].GameVehicle;
                            vehicle.PlaceOnGround();


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
