using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;
using NativeUI;

namespace VehicleWarfare.Menus
{
    class WeaponPurchase
    {
        //private UIMenu prevMenu;
        private ShopWeapon shopWeapon;

        public WeaponPurchase()
        {
            //shopWeapon = _shopWeapon;
            //prevMenu = _prevMenu;
        }

        public WeaponPurchase(ShopWeapon _shopWeapon)
        {
            shopWeapon = _shopWeapon;
        }

        public static void ApplyToMenu(ShopWeapon shopWeapon, UIMenu weapons)
        {
            var player = Game.Player;
            weapons.Clear();
            var purchaseWeapon = new UIMenuItem("Purchase " + shopWeapon.Name + " - $" + shopWeapon.Price);
            weapons.AddItem(purchaseWeapon);

            var purchaseAmmo = new UIMenuItem("Purchase " + shopWeapon.AmmoSize + " bullets - $" + shopWeapon.AmmoPrice);
            weapons.AddItem(purchaseAmmo);

            weapons.OnItemSelect += (sender, item, index) =>
            {
                if (item == purchaseWeapon)
                {
                    if (player.Character.Weapons.HasWeapon(shopWeapon.Hash))
                    {
                        UI.Notify("You already have this weapon.");
                    }
                    else
                    {
                        if (shopWeapon.Price > Game.Player.Money)
                        {
                            UI.Notify("Not enough money!");
                        }
                        else
                        {
                            player.Character.Weapons.Give(shopWeapon.Hash, shopWeapon.AmmoSize, false, false);
                            if (player.Character.Weapons[shopWeapon.Hash].Ammo > shopWeapon.AmmoSize)
                            {
                                player.Character.Weapons[shopWeapon.Hash].Ammo = shopWeapon.AmmoSize;
                                //player.Character.Weapons[shopWeapon.Hash].SetComponent(WeaponComponent.assaultrifle);
                            }
                            Game.Player.Money -= shopWeapon.Price;
                        }

                    }
                }
                else if (item == purchaseAmmo)
                {
                    if (!player.Character.Weapons.HasWeapon(shopWeapon.Hash))
                    {
                        UI.Notify("You need to purchase this weapon first.");
                    }
                    else
                    {
                        if (shopWeapon.AmmoPrice > Game.Player.Money)
                        {
                            UI.Notify("Not enough money!");
                        }
                        else
                        {
                            player.Character.Weapons[shopWeapon.Hash].Ammo += shopWeapon.AmmoSize;
                            Game.Player.Money -= shopWeapon.AmmoPrice;
                        }
                    }
                }
            };

            weapons.OnMenuClose += sender =>
            {
                MenuManager.WeaponShop.Visible = true;
            };
        }

        public UIMenu GetMenu()
        {
            if (shopWeapon == null)
            {
                UIMenu weaponPurchaseMenu = new UIMenu("Weapon Shop", "Buy a weapon.");
                return weaponPurchaseMenu;
            }
            else
            {
                UIMenu weaponPurchaseMenu = new UIMenu("Weapon Shop", "Buy a weapon.");
                ApplyToMenu(shopWeapon, weaponPurchaseMenu);
                return weaponPurchaseMenu;
            }
        }
    }
}
