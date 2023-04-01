using Microsoft.AspNetCore.Components;

namespace TwoPanelBlazorDemo.Shared;

public partial class TwoPaneView : IDisposable
{
    [Parameter]
    public RenderFragment Pane1 { get; set; }

    [Parameter]
    public RenderFragment Pane2 { get; set; }

    [Parameter]
    public EventCallback<Mode> ModeChanged { get; set; }

    /// <summary>
    /// CSS class name for Pane1
    /// </summary>
    [Parameter]
    public string Pane1Class { get; set; } = string.Empty;

    /// <summary>
    /// CSS class name for Pane2
    /// </summary>
    [Parameter]
    public string Pane2Class { get; set; } = string.Empty;

    /// <summary>
    /// CSS style for Pane1
    /// </summary>
    [Parameter]
    public string Pane1Style { get; set; } = string.Empty;

    /// <summary>
    /// CSS style for Pane2
    /// </summary>
    [Parameter]
    public string Pane2Style { get; set; } = string.Empty;

    /// <summary>
    /// Reflects the current Mode
    /// </summary>
    public Mode Mode { get; private set; }

    /// <summary>
    /// Reflects the type of MobileDevice. Janky Hack Alert!
    /// </summary>
    public MobileDevice MobileDevice { get; private set; } = MobileDevice.None;

    /// <summary>
    /// We need this to determine if the orientation has changed
    /// </summary>
    private DisplayOrientation LastOrientation { get; set; } = DisplayOrientation.Unknown;

    /// <summary>
    /// Timer used to check the orientation
    /// </summary>
    private Timer timer;

    protected override void OnInitialized()
    {
        // This is a stopgap measure to check for Samsung Flip and Fold devices.
        // Developers can test their other devices such as the Microsoft Surface Duo
        // (which I don't have posession of) and improve this measure
        if (DeviceInfo.Current.Manufacturer == "samsung")
        {
            if (DeviceInfo.Current.Name.ToLower().Contains("flip"))
            {
                MobileDevice = MobileDevice.Flip;
            }
            else if (DeviceInfo.Current.Name.ToLower().Contains("fold"))
            {
                MobileDevice = MobileDevice.Fold;
            }
        }

        // Call the code to test the orientation and change the Mode
        TestOrientation();

        // Kick off the timer every 100 ms. You can adjust if you like.
        timer = new Timer(Timer_Elapsed, null, 100, 100);

    }
    private void Timer_Elapsed(object state)
    {
        // Test the orientation and change the Mode
        TestOrientation();
    }

    private void TestOrientation()
    {
        // Have we just changed to Portrait mode?
        if (DeviceDisplay.Current.MainDisplayInfo.Orientation == DisplayOrientation.Portrait
           && LastOrientation != DisplayOrientation.Portrait)
        {
            // You need to call this code on the UI thread. It's an old story.
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Note that Mode changes depending on the device.
                if (MobileDevice == MobileDevice.Fold)
                {
                    Mode = Mode.Wide;
                }
                else if (MobileDevice == MobileDevice.Flip)
                {
                    Mode = Mode.Tall;
                }
                // Save the last orientation
                LastOrientation = DisplayOrientation.Portrait;
                // Raise the ModeChanged event
                ModeChanged.InvokeAsync(Mode);
                // Redraw
                StateHasChanged();
            });
        }
        // Have we just changed to Landscape mode?
        else if (DeviceDisplay.Current.MainDisplayInfo.Orientation == DisplayOrientation.Landscape
            && LastOrientation != DisplayOrientation.Landscape)
        {
            // You need to call this code on the UI thread. It's an old story.
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Note that Mode changes depending on the device.
                if (MobileDevice == MobileDevice.Fold)
                {
                    Mode = Mode.Tall;
                }
                else if (MobileDevice == MobileDevice.Flip)
                {
                    Mode = Mode.Wide;
                }
                // Save the last orientation
                LastOrientation = DisplayOrientation.Landscape;
                // Raise the ModeChanged event
                ModeChanged.InvokeAsync(Mode);
                // Redraw
                StateHasChanged();
            });
        }
    }

    /// <summary>
    /// Clean up the timer
    /// </summary>
    public void Dispose()
    {
        timer.Dispose();
    }

}