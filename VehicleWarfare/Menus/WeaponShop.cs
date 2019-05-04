using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;
using NativeUI;

namespace VehicleWarfare.Menus
{
    class WeaponShop
    {
        public WeaponShop()
        {
            //mainMenu = _mainMenu;
            //menuPool = _menuPool;
        }

        public UIMenu GetMenu()
        {
            var weapons = new UIMenu("Weapon Shop", "Customize your weapons");

            ShopWeapon[] items = new ShopWeapon[] {
                new ShopWeapon("Combat PDW", WeaponHash.CombatPDW, 1000, 100, 30),
                new ShopWeapon("Pump Shotgun MK II", WeaponHash.PumpShotgunMk2, 1000, 100, 8),
                new ShopWeapon("Assault Rifle", WeaponHash.AssaultRifle, 1000, 100, 30), 
            };

            foreach (var item in items)
            {
                weapons.AddItem(item.GetUIMenuItem());
            }

            weapons.OnItemSelect += (sender, selectedItem, index) =>
            {
                foreach (var item in items)
                {
                    if (item.GetUIMenuItem().Text == selectedItem.Text)
                    {
                        WeaponPurchase.ApplyToMenu(item, UIManager.WeaponPurchase);

                        UIManager.WeaponPurchase.Visible = true;
                        weapons.Visible = false;
                    }
                }
            };

            weapons.OnMenuClose += sender =>
            {
                UIManager.MainMenu.Visible = true;
            };

            return weapons;
        }
    }
}
