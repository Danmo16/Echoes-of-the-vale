using UnityEngine;

public class LogicaObjeto : MonoBehaviour
{
    public bool destruirConCursor;
    public bool destruirAutomatico;
    public LogicaPlayer logicaPlayer;

    public int tipo;

    // 1. Crece
    // 2. Aumenta velocidad
    // 3. Aumenta salto
    // 4. Lento

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logicaPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<LogicaPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Efecto()
    {
        switch (tipo)
        { 
            case 1:
                logicaPlayer.gameObject.transform.localScale = new Vector3(3, 3, 3);
                break;

            case 2:
                logicaPlayer.velocidadInicial += 5;
                break;

            case 3:
                logicaPlayer.fuerzaSalto += 10;
                break;

            case 4:
                logicaPlayer.velocidadInicial -= 10;
                break;

            default:
                Debug.Log("sin efecto");
                break;
        }
    }
}
