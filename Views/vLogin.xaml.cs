namespace jmedrandas2tarea.Views;

public partial class vLogin : ContentPage
{
    //VECTORES USUARIOS Y CONTRASEÑAS
    string[] usuarios = { "Carlos" , "Ana" , "Jose" };
    string[] contrasenas = {"carlos123" , "ana123" , "jose123" };
    public vLogin()
	{
		InitializeComponent();
	}

    private void btnIniciar_Clicked(object sender, EventArgs e)
    {

        //Usuario y contraseña 
        //Abrir ventana principal
        string usuario = txtUsuario.Text;
        string contrasena = txtPassword.Text;
        // VALIDAR CAMPOS VACÍOS
        if (string.IsNullOrWhiteSpace(usuario))
        {
            DisplayAlert("Error", "Por favor, ingrese el usuario.", "Aceptar");
            txtUsuario.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(contrasena))
        {
            DisplayAlert("Error", "Por favor, ingrese la contraseña.", "Aceptar");
            txtPassword.Focus();
            return;
        }
        //Buscar usuario y contrasena coincide 
        bool autenticado = false;
        string nombreUsuario = "";

        for (int i = 0; i < usuarios.Length; i++)
        {
            // Convertimos el usuario ingresado a minúsculas para comparar
            // El nombre original lo guardamos con su formato original
            if (usuarios[i].ToLower() == usuario.ToLower() &&
                contrasenas[i] == contrasena)
            {
                autenticado = true;
                nombreUsuario = usuarios[i]; // Guardamos el nombre original (Carlos, Ana, Jose)
                break;
            }
        }

        if (autenticado)
        {
            // Mostrar mensaje de bienvenida
            DisplayAlert("Bienvenido", $"Hola {nombreUsuario}, has iniciado sesión correctamente.", "Aceptar");
            Navigation.PushAsync(new vPrincipal());
        }
        else
        {
            DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
            txtPassword.Text = string.Empty;
            txtUsuario.Focus();
        }
    }
}