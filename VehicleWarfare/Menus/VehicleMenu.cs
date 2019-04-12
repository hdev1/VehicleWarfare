using System;
using System.Collections.Generic;
using System.Linq;
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

            vehicleMenu.OnListChange += (sender, item, index) =>
                {
                    if (item == armor) VehicleTracker.vehicleArmorMultiplier = float.Parse(armorLevels.ToArray()[item.Index]);
                    else if (item == saveVehicle)
                    {
                        VehicleTracker.SavedVehicles.Append(Game.Player.Character.CurrentVehicle);
                        
                    }
                };

            return vehicleMenu;
        }
    }
}
