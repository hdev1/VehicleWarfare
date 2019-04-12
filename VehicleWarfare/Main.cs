// Native UI Menu Template 3.0 - Abel Software
// You must download and use Scripthook V Dot Net Reference and NativeUI Reference (LINKS AT BOTTOM OF THE TEMPLATE)
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using GTA;
using GTA.Native;
using NativeUI;
using VehicleWarfare;
using VehicleWarfare.Menus;

public class Main : Script
{

    ScriptSettings config;
    Keys OpenMenu;

    public Main()
        {
            KeyUp += OnKeyUp;
            KeyDown += OnKeyDown;
            Tick += OnTick;

            MenuManager.Init();

            config = ScriptSettings.Load("scripts\\settings.ini");
            OpenMenu = config.GetValue<Keys>("Options", "OpenMenu", Keys.F7);

            KeyDown += (o, e) =>
            {
                if (e.KeyCode == OpenMenu && !MenuManager.MenuPool.IsAnyMenuOpen())
                    MenuManager.MainMenu.Visible = !MenuManager.MainMenu.Visible;
            };
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Z)
        {
            Game.Player.Character.CurrentVehicle.IsPersistent = true;
        }
    }


    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        
    }

    private void OnTick(object sender, EventArgs e)
    {
        // Process menus
        MenuManager.Update();
        KillTracker.Update();
        VehicleTracker.Update();

    }

    public void PlayerModelMenu(UIMenu menu)
    {
        /*var playermodelmenu = MenuPool.AddSubMenu(menu, "Player Model Menu");
        for (int i = 0; i < 1; i++) ;

        //We will change our player model to the male LSPD officer
        var malecop = new UIMenuItem("Male LSPD Officer", "");
        playermodelmenu.AddItem(malecop);
        playermodelmenu.OnItemSelect += (sender, item, index) =>
        {
            if (item == malecop)
            {
                Game.Player.ChangeModel("S_M_Y_COP_01");
            }
        };*/
    }

    public void VehicleMenu(UIMenu menu)
    {
        /**/
    }

    public void WeaponMenu(UIMenu menu)
    {
        
    }

    //Now we will add all of our sub menus into our main menu, and set the general information of the entire menu
    
}

