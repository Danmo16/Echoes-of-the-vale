using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Necesario para manejar escenas

public class LogicaBarraVida : MonoBehaviour
{
    public int vidaMax;
    public float vidaActual;
    public Image imagenBarraVida;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vidaActual = vidaMax;
    }

    // Update is called once per frame
    void Update()
    {
        RevisarVida();

        if (vidaActual <= 0)
        {
            gameObject.SetActive(false);
            // Función que se desee
        }
    }

    public void RevisarVida()
    {
        imagenBarraVida.fillAmount = vidaActual / vidaMax;
    }

    public void RecibirDaño(float cantidad)
    {
        vidaActual -= cantidad;
    }

    // Nueva función para cuando se acabe el tiempo
    public void TiempoAgotado()
    {
        vidaActual = 0; // Vacía la barra
        RevisarVida(); // Actualiza visualmente la barra
        gameObject.SetActive(false); // Desactiva el objeto
        
        // Opciones para finalizar (elige una):
        
        // OPCIÓN 1: Salir del juego (solo funciona en build, no en el editor)
        //Application.Quit();
        
        // OPCIÓN 2: Recargar la escena actual
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        // OPCIÓN 3: Ir a una escena específica (ej: menú de game over)
        SceneManager.LoadScene("MainMenu");
        
        // OPCIÓN 4: Esperar unos segundos antes de salir/reiniciar
        // Invoke("SalirDelJuego", 2f);
    }

    // Función auxiliar si quieres usar la opción 4
    void SalirDelJuego()
    {
        Application.Quit();
        // O cambiar de escena:
        // SceneManager.LoadScene("MenuPrincipal");
    }
}