using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo1 : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public Quaternion angulo;
    public float grado;
    public bool atacando;

    public GameObject target;
    public LogicaBarraVida vidaJugador;
    public float dañoAlJugador = 10f;
    public int vidaMaxEnemigo = 50;
    public int vidaActualEnemigo;

    void OnTriggerEnter(Collider coll)
    {
        if(coll.CompareTag("arma") && atacando)
        {
            print("Esqueleto recibió daño");
            RecibirDaño(10f);
        }
    }

    public void RecibirDaño(float cantidad)
    {
        vidaActualEnemigo -= (int)cantidad;
        if (vidaActualEnemigo <= 0)
        {
            Morir();
        }
    }

    public void Morir()
    {
        print("Esqueleto murió");
        gameObject.SetActive(false);
    }

    public void DañarAlJugador()
    {
        if (vidaJugador != null)
        {
            vidaJugador.RecibirDaño(dañoAlJugador);
            print("Jugador recibió daño del esqueleto");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.Find("Player");
        vidaActualEnemigo = vidaMaxEnemigo;
        
        if (target != null)
        {
            vidaJugador = target.GetComponent<LogicaBarraVida>();
        }
    }

    public void Comportamiento_Enemigo()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 5)
        {
            ani.SetBool("run", false);
            cronometro += 1 * Time.deltaTime;
            
            if (cronometro >= 4)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }
            
            switch (rutina)
            {
                case 0:
                    ani.SetBool("walk", false);
                    break;

                case 1:
                    grado = Random.Range(0, 360);
                    angulo = Quaternion.Euler(0, grado, 0);
                    rutina++;
                    break;

                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                    transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                    ani.SetBool("walk", true);
                    break;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, target.transform.position) > 1 && !atacando)
            {
                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                ani.SetBool("walk", false);

                ani.SetBool("run", true);
                transform.Translate(Vector3.forward * 2 * Time.deltaTime);

                ani.SetBool("attack", false);
            }
            else
            {
                ani.SetBool("walk", false);
                ani.SetBool("run", false);

                ani.SetBool("attack", true);
                atacando = true;
            }
        }
    }

    public void Final_Ani()
    {
        ani.SetBool("attack", false);
        atacando = false;
    }

    void Update()
    {
        Comportamiento_Enemigo();
    }
}

