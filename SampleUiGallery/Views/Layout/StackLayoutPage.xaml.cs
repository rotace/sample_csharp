namespace SampleUiGallery.Views.Layout;

public partial class StackLayoutPage : ContentPage
{
	public StackLayoutPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		if(Stack.Orientation == StackOrientation.Vertical)
		{
            Stack.Orientation = StackOrientation.Horizontal;
        }
		else
		{
			Stack.Orientation = StackOrientation.Vertical;
		}
    }
}