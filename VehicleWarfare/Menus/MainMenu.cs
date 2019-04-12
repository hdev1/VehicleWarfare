using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativeUI;
using VehicleWarfare.Menus;

namespace VehicleWarfare
{
    public class MainMenu
    {

        public MainMenu()
        {
            
        }

        public UIMenu GetMenu()
        {
            UIMenu mainMenu = new UIMenu("Main Menu", "Vehicle Warfare v1.0");

            var weaponShopItem = new UIMenuItem("Weapon Shop");
            mainMenu.AddItem(weaponShopItem);

            var vehicleMenuItem = new UIMenuItem("Vehicle Menu");
            mainMenu.AddItem(vehicleMenuItem);

            mainMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == weaponShopItem)
                {
                    mainMenu.Visible = false;
                    MenuManager.WeaponShop.Visible = true;
                } else if (item == vehicleMenuItem)
                {
                    mainMenu.Visible = false;
                    MenuManager.VehicleMenu.Visible = true;
                }
            };

            return mainMenu;
        }
    }
}
