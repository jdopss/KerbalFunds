using UnityEngine;
using KSP.UI.Screens;
using ClickThroughFix;

public class MyToolbarButton : MonoBehaviour, IButton
{
    private const string buttonTexturePath = "KerbalFunds/icon.png";

    private ApplicationLauncherButton button = null;

    public void OnClick()
    {
        showWindow = !showWindow;
    }

    public void OnHover()
    {
        // Show a tooltip or other information when the user hovers over the button
    }

    public void OnUpdate()
    {
        // Update the state of the button here, for example to show or hide it depending on the current game state
        if (HighLogic.LoadedSceneIsFlight)
        {
            SetVisible(true);
        }
        else
        {
            SetVisible(false);
        }
    }

    public void SetVisible(bool visible)
    {
        if (button == null)
        {
            // Create the button if it doesn't exist yet
            button = ApplicationLauncher.Instance.AddModApplication(
                OnClick,
                OnClick,
                null,
                null,
                null,
                null,
                ApplicationLauncher.AppScenes.FLIGHT,
                GameDatabase.Instance.GetTexture(buttonTexturePath, false)
            );
        }

        if (this.visible != visible)
        {
            // Show or hide the button
            button.Visible = visible;
            this.visible = visible;
        }
    }
}