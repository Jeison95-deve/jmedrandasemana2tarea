namespace jmedrandas2tarea.Views;

public partial class vPrincipal : ContentPage
{
	public vPrincipal()
	{
		InitializeComponent();
        dpFecha.Date = DateTime.Now;
    }
    // Validar que solo se ingresen números (enteros o decimales) del 0-10
    private void OnValidarNota(object sender, TextChangedEventArgs e)
    {
        Entry entry = (Entry)sender;
        string nuevoTexto = e.NewTextValue;
        string textoAnterior = e.OldTextValue ?? "";

        if (string.IsNullOrEmpty(nuevoTexto))
        {
            return;
        }

        // Reemplazar cualquier coma por punto
        nuevoTexto = nuevoTexto.Replace(',', '.');

        // Permitir: números, un solo punto decimal
        // Esta expresión permite: 1, 1.2, 0.5, 10, etc.
        if (!System.Text.RegularExpressions.Regex.IsMatch(nuevoTexto, @"^\d*\.?\d{0,2}$"))
        {
            entry.Text = textoAnterior;
            return;
        }

        // Contar puntos decimales (solo puede haber uno)
        int cantidadPuntos = nuevoTexto.Count(c => c == '.');
        if (cantidadPuntos > 1)
        {
            entry.Text = textoAnterior;
            return;
        }

        // Si termina con punto, permitir temporalmente
        if (nuevoTexto.EndsWith("."))
        {
            entry.Text = nuevoTexto;
            return;
        }

        // Validar rango 0-10
        if (double.TryParse(nuevoTexto, System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out double valor))
        {
            if (valor > 10)
            {
                // Mostrar mensaje solo cuando sea mayor a 10
                entry.Text = textoAnterior;
                DisplayAlert("Error", "La nota no puede ser mayor a 10", "OK");
            }
            else if (valor < 0 && nuevoTexto != "0")
            {
                entry.Text = textoAnterior;
                DisplayAlert("Error", "La nota no puede ser menor a 0", "OK");
            }
            else
            {
                // Actualizar texto formateado
                if (nuevoTexto != entry.Text)
                {
                    entry.Text = nuevoTexto;
                }
            }
        }
        else if (nuevoTexto != "0")
        {
            // Si no es un número válido, revertir
            entry.Text = textoAnterior;
        }
    }


    private void btnCalcular_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Validar que haya un estudiante seleccionado
            if (pEstudiante.SelectedIndex == -1)
            {
                DisplayAlert("Error", "Debe seleccionar un estudiante", "OK");
                return;
            }

            // Validar campos vacíos
            if (string.IsNullOrEmpty(txtSeguimiento1.Text) ||
                string.IsNullOrEmpty(txtExamen1.Text) ||
                string.IsNullOrEmpty(txtSeguimiento2.Text) ||
                string.IsNullOrEmpty(txtExamen2.Text))
            {
                DisplayAlert("Error", "Debe ingresar todas las notas", "OK");
                return;
            }

            // Obtener y convertir valores
            double seguimiento1 = ConvertirNota(txtSeguimiento1.Text, "Seguimiento 1");
            double examen1 = ConvertirNota(txtExamen1.Text, "Examen 1");
            double seguimiento2 = ConvertirNota(txtSeguimiento2.Text, "Seguimiento 2");
            double examen2 = ConvertirNota(txtExamen2.Text, "Examen 2");

            // Calcular Parcial 1 (30% + 20% = 50%)
            double parcial1 = (seguimiento1 * 0.3) + (examen1 * 0.2);
            // Calcular Parcial 2 (30% + 20% = 50%)
            double parcial2 = (seguimiento2 * 0.3) + (examen2 * 0.2);
            // Nota Final (Parcial1 + Parcial2)
            double notaFinal = parcial1 + parcial2;

            // Mostrar resultados
            txtParcial1.Text = parcial1.ToString("F2");
            txtParcial2.Text = parcial2.ToString("F2");
            txtNotaFinal.Text = notaFinal.ToString("F2");

            // Calcular y mostrar Estado
            string estado = ObtenerEstado(notaFinal);
            txtEstado.Text = estado;

            // Cambiar color según estado
            if (notaFinal >= 7)
            {
                txtNotaFinal.TextColor = Colors.Green;
                txtEstado.TextColor = Colors.Green;
            }
            else if (notaFinal >= 5)
            {
                txtNotaFinal.TextColor = Colors.Orange;
                txtEstado.TextColor = Colors.Orange;
            }
            else
            {
                txtNotaFinal.TextColor = Colors.Red;
                txtEstado.TextColor = Colors.Red;
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    // Función auxiliar para convertir notas
    private double ConvertirNota(string texto, string nombreCampo)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            throw new Exception($"El campo {nombreCampo} está vacío");
        }

        // Reemplazar coma por punto
        string textoProcesado = texto.Trim().Replace(',', '.');

        // Intentar convertir
        if (!double.TryParse(textoProcesado,
            System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture,
            out double nota))
        {
            throw new Exception($"'{texto}' no es un número válido en {nombreCampo}");
        }

        // Validar rango
        if (nota < 0 || nota > 10)
        {
            throw new Exception($"La nota en {nombreCampo} debe estar entre 0 y 10 (ingresado: {nota})");
        }

        return nota;
    }

    // Determinar estado según nota final
    private string ObtenerEstado(double notaFinal)
    {
        if (notaFinal >= 7)
        {
            return "✅ APROBADO";
        }
        else if (notaFinal >= 5 && notaFinal <= 6.9)
        {
            return "⚠️ COMPLEMENTARIO";
        }
        else
        {
            return "❌ REPROBADO";
        }
    }


    

    private void btnImprimir_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Validar que se haya calculado
            if (string.IsNullOrEmpty(txtNotaFinal.Text))
            {
                DisplayAlert("Error", "Debe calcular las notas primero", "OK");
                return;
            }

            // Obtener datos
            string nombre = pEstudiante.SelectedIndex != -1 ? pEstudiante.SelectedItem.ToString() : "No seleccionado";
            string fecha = string.Format("{0:dd/MM/yyyy}", dpFecha.Date);
            string parcial1 = txtParcial1.Text;
            string parcial2 = txtParcial2.Text;
            string notaFinal = txtNotaFinal.Text;
            string estado = ObtenerEstado(Convert.ToDouble(notaFinal));

            // Construir mensaje con saltos de línea
            string mensaje = $"=== REPORTE DE CALIFICACIONES ===\n\n" +
                             $"🏫 UNIVERSIDAD ISRAEL\n\n" +
                             $"📝 NOMBRE: {nombre}\n" +
                             $"📅 FECHA: {fecha}\n\n" +
                             $"📊 PARCIAL 1: {parcial1}\n" +
                             $"📊 PARCIAL 2: {parcial2}\n\n" +
                             $"✨ NOTA FINAL: {notaFinal}\n\n" +
                             $"🎯 ESTADO: {estado}\n\n" +
                             $"--- UISRAEL ---";

            // Mostrar alerta
            DisplayAlert("REPORTE DE CALIFICACIONES", mensaje, "ACEPTAR");
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Error al generar reporte: {ex.Message}", "OK");
        }


    }

    private void btnLimpiar_Clicked(object sender, EventArgs e)
    {
        // Limpiar entradas
        txtSeguimiento1.Text = "";
        txtExamen1.Text = "";
        txtSeguimiento2.Text = "";
        txtExamen2.Text = "";
        txtParcial1.Text = "";
        txtParcial2.Text = "";
        txtNotaFinal.Text = "";
        txtEstado.Text = "";

        // Resetear picker
        pEstudiante.SelectedIndex = -1;

        // Resetear fecha a hoy
        dpFecha.Date = DateTime.Now;

        // Resetear colores
        txtNotaFinal.TextColor = Colors.Red;
        txtEstado.TextColor = Colors.Black;

        DisplayAlert("Éxito", "Todos los campos han sido limpiados", "OK");
    }

    private void btnSalir_Clicked(object sender, EventArgs e)
    {
    
        Navigation.PushAsync(new Views.vLogin());

    }
}