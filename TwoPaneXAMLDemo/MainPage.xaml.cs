namespace TwoPaneXAMLDemo;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        twoPaneView.ModeChanged += TwoPaneView_ModeChanged;
    }

    private void TwoPaneView_ModeChanged(object sender, EventArgs e)
    {
        // The mode changed. 
        Label2.Text = twoPaneView.Mode.ToString();
    }
}