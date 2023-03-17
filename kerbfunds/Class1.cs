﻿using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.TextCore;
using KSP.UI.Screens;

// This attribute tells Unity to run this script in all game scenes
[KSPAddon(KSPAddon.Startup.AllGameScenes, false)]

public class AddFunds : MonoBehaviour
{
    private Rect windowRect = new Rect(100, 100, 200, 200); // position and size of the window
    private string inputString = "100"; // default input string
    private bool isAddingFunds = true; // switch between adding funds and researching
    private bool showWindow = false; // toggle window visibility
    private Texture2D buttonTexture; // texture for the toolbar button

    private void Start()
    {
        // Load the button texture from the mod's Textures folder
        buttonTexture = GameDatabase.Instance.GetTexture("KerbalFunds/icon.png", false);
    }

    private void Update()
    {
        // Open the window when the L key is pressed
        if (Input.GetKeyDown(KeyCode.L))
        {
            showWindow = !showWindow;
            if (showWindow)
            {
                // Center the window on the screen and reset the input string
                windowRect = new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200);
                inputString = "100";
            }
        }
        // Open the window when the toolbar button is clicked
        if (ToolbarManager.Instance != null && ToolbarManager.Instance.Toolbar != null)
        {
            if (showWindow)
            {
                // Close the window when the toolbar button is clicked again
                if (Input.GetMouseButtonDown(0))
                {
                    showWindow = false;
                }
            }
            else
            {
                // Show the window when the toolbar button is clicked
                if (Input.GetMouseButtonDown(0))
                {
                    showWindow = true;
                }
            }
        }
    }

    private void OnGUI()
    {
        // Draw the window
        if (showWindow && (HighLogic.LoadedScene == GameScenes.FLIGHT || HighLogic.LoadedScene == GameScenes.SPACECENTER))
        {
            windowRect = GUI.Window(0, windowRect, WindowFunction, "Add Funds or Research");

            // Make sure the input string is not empty
            if (string.IsNullOrEmpty(inputString))
            {
                inputString = "0";
            }
        }
    }
    private void WindowFunction(int windowID)
    {
        // Draw the input field and the submit button
        inputString = GUI.TextField(new Rect(10, 60, 180, 20), inputString);

        if (isAddingFunds) {
            if (GUI.Button(new Rect(60, 100, 80, 30), "Add Funds"))
            {
                int fundsToAdd;
                if (int.TryParse(inputString, out fundsToAdd))
                {
                    // Add the funds to the player's account
                    Funding.Instance.AddFunds(fundsToAdd, TransactionReasons.None);
                    Debug.Log(fundsToAdd+"Funds added");
                }
                
                // Close the window
                inputString = "1000";
            }
        }
        else {
            if (GUI.Button(new Rect(60, 100, 80, 30), "Research"))
            {
                int scienceToAdd;
                if (int.TryParse(inputString, out scienceToAdd))
                {
                    // Add the science to the player's account
                    ResearchAndDevelopment.Instance.AddScience(scienceToAdd, TransactionReasons.None);
                    Debug.Log(scienceToAdd + "Research points added");
                }

                // Close the window
                inputString = "10";
            }
        }

        // Draw the button to switch between adding funds and researching
        if (GUI.Button(new Rect(50, 20, 100, 30), isAddingFunds ? "Adding Funds" : "Adding Research"))
        {
            isAddingFunds = !isAddingFunds;
        }

        // Make the window draggable
        GUI.DragWindow();
    }
}
