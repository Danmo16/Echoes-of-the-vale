using UnityEngine;

public class PruebaDaño : MonoBehaviour
{
    public LogicaBarraVida logicaBarraVidaJugador;
    public float daño = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            logicaBarraVidaJugador.vidaActual -= daño;
        }

    }
}
