using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogicaObjetivosEsferas : MonoBehaviour
{
    public int numDeObjetivos;
    public TextMeshProUGUI textoMision;
    public GameObject botonDeMision;
    
    // Variable para el temporizador
    public float tiempoLimite = 60f;
    private float tiempoRestante;
    private bool misionActiva = true;
    
    // Referencia a la imagen de la barra de vida
    private Image imagenBarraVida;
    
    // HashSet para evitar contar el mismo padre múltiples veces
    private HashSet<GameObject> padresContados = new HashSet<GameObject>();

    void Start()
    {
        // Contar solo los padres únicos de los objetivos
        GameObject[] objetivos = GameObject.FindGameObjectsWithTag("objetivo");
        HashSet<GameObject> padresUnicos = new HashSet<GameObject>();
        
        foreach (GameObject obj in objetivos)
        {
            if (obj.transform.parent != null)
            {
                padresUnicos.Add(obj.transform.parent.gameObject);
            }
            else
            {
                padresUnicos.Add(obj); // Si no tiene padre, contar el objeto mismo
            }
        }
        
        numDeObjetivos = padresUnicos.Count;
        tiempoRestante = tiempoLimite;
        
        // Buscar el objeto con tag "imageHP"
        GameObject barraHP = GameObject.FindGameObjectWithTag("imageHP");
        if (barraHP != null)
        {
            imagenBarraVida = barraHP.GetComponent<Image>();
        }
        
        ActualizarTextoMision();
    }

    void Update()
    {
        if (misionActiva)
        {
            if (tiempoRestante > 0)
            {
                tiempoRestante -= Time.deltaTime;
                ActualizarTextoMision();
                
                if (tiempoRestante <= 0)
                {
                    tiempoRestante = 0;
                    misionActiva = false;
                    textoMision.text = "¡PERDISTE!\nSe acabó el tiempo";
                    textoMision.color = Color.red;
                    
                    // Vaciar la barra de vida
                    if (imagenBarraVida != null)
                    {
                        imagenBarraVida.fillAmount = 0;
                    }
                    
                    // Salir del juego después de 2 segundos
                    Invoke("SalirDelJuego", 2f);
                }
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "objetivo" && misionActiva)
        {
            GameObject padre = col.transform.parent != null ? col.transform.parent.gameObject : col.gameObject;
            
            // Solo contar si este padre no ha sido contado antes
            if (!padresContados.Contains(padre))
            {
                padresContados.Add(padre);
                Destroy(padre);
                numDeObjetivos--;
                ActualizarTextoMision();

                if (numDeObjetivos <= 0)
                {
                    misionActiva = false;
                    textoMision.text = "¡Completaste la misión!\nDirígete al castillo";
                    textoMision.color = Color.green;
                    botonDeMision.SetActive(true);
                }
            }
        }
    }

    void ActualizarTextoMision()
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        
        textoMision.text = "Obtén las esferas hechas de slime" +
                           "\nRestantes: " + numDeObjetivos +
                           "\nTiempo: " + string.Format("{0:00}:{1:00}", minutos, segundos);
        
        if (tiempoRestante <= 10f && tiempoRestante > 0)
        {
            textoMision.color = Color.yellow;
        }
        else if (tiempoRestante > 10f)
        {
            textoMision.color = Color.white;
        }
    }
    
    void SalirDelJuego()
    {
        // OPCIÓN 1: Salir del juego (funciona en build)
        Application.Quit();
        
        // OPCIÓN 2: Si quieres reiniciar la escena, descomenta la siguiente línea:
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        
        // Para que funcione en el editor de Unity también:
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}