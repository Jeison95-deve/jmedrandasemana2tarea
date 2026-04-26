namespace jmedrandas2tarea.Views;

public partial class vLogin : ContentPage
{
	public vLogin()
	{
		InitializeComponent();
	}

    private void btnIniciar_Clicked(object sender, EventArgs e)
    {

        //Usuario y contraseña 
        //Abrir ventana principal
        Navigation.PushAsync(new vPrincipal());

    }
}