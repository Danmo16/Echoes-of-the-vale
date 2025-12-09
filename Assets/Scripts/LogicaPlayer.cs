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

    public bool estoyAtacando;
    public bool avanzoSolo;
    public float impulsoGolpe = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puedoSaltar = false;
        anim = GetComponent<Animator>();

        velocidadInicial = velocidadMovimiento;
        velocidadAgachado = velocidadMovimiento * 0.5f;

        estoyAtacando = false;
    }

    void FixedUpdate()
    {
        // Rotaci�n y movimiento
        if (!estoyAtacando)
        {
            transform.Rotate(0, x * velocidadRotacion * Time.deltaTime, 0);
            transform.Translate(0, 0, y * velocidadMovimiento * Time.deltaTime);
        }

        if (avanzoSolo)
        {
            rb.linearVelocity = transform.forward * impulsoGolpe;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Leer las teclas (W, A, S, D)
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Return) && puedoSaltar && !estoyAtacando)
        {
            anim.SetTrigger("golpeo");
            estoyAtacando = true;
        }

        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);

        // Salto
        if (puedoSaltar)
        {
            if (!estoyAtacando)
            {
                if(Input.GetKeyDown(KeyCode.Space))
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

    public void DejoDeGolpear()
    {
        estoyAtacando = false;
    }

    public void AvanzoSolo()
    {
        avanzoSolo = true;
    }

    public void DejoDeAvanzar()
    {
        avanzoSolo = false;
    }
}
