using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA.Native;
using NativeUI;

public class ShopWeapon
    {
        public string Name;
        public WeaponHash Hash;
        public int Price;
        public int AmmoPrice;
        public int AmmoSize;

        public UIMenuItem GetUIMenuItem()
        {
            return new UIMenuItem(Name);
        }

        public ShopWeapon(string name, WeaponHash hash, int price, int ammoPrice, int ammoSize)
        {
            Name = name;
            Hash = hash;
            Price = price;
            AmmoPrice = ammoPrice;
            AmmoSize = ammoSize;
        }
    }
