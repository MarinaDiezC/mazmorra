using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactuable
{
    private SistemaPatrulla sistemaPatrulla;
    private SistemaInteraccion interactuadorActual;
    [SerializeField] private float tiempoEnfocar;
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private DialogoSO dialogo;


    private Animator anim;

    //override: Sobreescribimos el método interactuar de la clase padre.
    private void Awake()
    {
        sistemaPatrulla= GetComponent<SistemaPatrulla>();
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        gM.OnFinInteraccion += FinalizarConversacion;
    }
    private void FinalizarConversacion()
    {
        interactuadorActual = null;
        sistemaPatrulla.enabled = true;
        anim.SetBool("talking", false);
        ActivarIcono();
    }

    public override void Interactuar(SistemaInteraccion interactuador)
    {
        if(interactuadorActual == null)
        {
            DesactivarIcono();
            interactuadorActual = interactuador;
            sistemaPatrulla.enabled = false;
            StartCoroutine(EnfocarInteractuador());
        }
        else
        {
            gM.IniciarDialogo(dialogo);
        }        
    }

    private IEnumerator EnfocarInteractuador()
    {
        float timer = 0f;
        
        //Rotar desde un origen a un destino: Interpolar.
        Quaternion rotacionInicial = transform.rotation;

        //Obtengo la dirección hacia mi interactuador
        Vector3 direccionInteractuador = (interactuadorActual.transform.position - transform.position).normalized;
        direccionInteractuador.y = 0;

        //Teniendo la dirección, calculo la rotación hacia mi interactuador
        Quaternion rotacionFinal = Quaternion.LookRotation(direccionInteractuador);
        
        while (timer < tiempoEnfocar) //mientras quede tiempo...
        {
            //Sigo interpolando...
            transform.rotation = Quaternion.Slerp(rotacionInicial, rotacionFinal, timer / tiempoEnfocar);

            //Sigo contando...
            timer += Time.deltaTime;

            yield return null;
        }
        anim.SetBool("talking", true);
        gM.IniciarDialogo(dialogo);
    }
}
