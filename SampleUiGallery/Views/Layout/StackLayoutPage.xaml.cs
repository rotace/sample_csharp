namespace SampleUiGallery.Views.Layout;

public partial class StackLayoutPage : ContentPage
{
	public StackLayoutPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		if(stack.Orientation == StackOrientation.Vertical)
		{
            stack.Orientation = StackOrientation.Horizontal;
        }
		else
		{
			stack.Orientation = StackOrientation.Vertical;
		}
    }
}