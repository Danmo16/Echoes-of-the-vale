using UnityEngine;

public class LogicaPlayer : MonoBehaviour
{
    public float velocidadMovimiento = 5.0f;
    public float velocidadRotacion = 200.0f;

    private Animator anim;
    public float x, y; 

    public Rigidbody rb;
    public float fuerzaSalto = 8f;
    public bool puedoSaltar;

    public float velocidadInicial;
    public float velocidadAgachado;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puedoSaltar = false;
        anim = GetComponent<Animator>();

        velocidadInicial = velocidadMovimiento;
        velocidadAgachado = velocidadMovimiento * 0.5f;
    }

    void FixedUpdate()
    {
        // Rotación y movimiento
        transform.Rotate(0, x * velocidadRotacion * Time.deltaTime, 0);
        transform.Translate(0, 0, y * velocidadMovimiento * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        // Leer las teclas (W, A, S, D)
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);

        // Salto
        if (puedoSaltar)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetBool("salte", true);
                rb.AddForce(new Vector3(0, fuerzaSalto, 0), ForceMode.Impulse);
            }
            anim.SetBool("tocoSuelo", true);

            if (Input.GetKey(KeyCode.LeftControl))
            {
                anim.SetBool("agachado", true);
                velocidadMovimiento = velocidadAgachado;
            }
            else
            {
                anim.SetBool("agachado", false);
                velocidadMovimiento = velocidadInicial;
            }

            anim.SetBool("tocoSuelo", true);

        }
        else
        {
            EstoyCayendo();
        }

    }

    public void EstoyCayendo()
    {
        anim.SetBool("tocoSuelo", false);
        anim.SetBool("salte", false);
    }
}
