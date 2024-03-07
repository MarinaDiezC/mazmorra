using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaInteraccion : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] private float radio;
    [SerializeField] private LayerMask queEsInteractuable;
    [SerializeField] private Transform puntoInteraccion;
    [SerializeField] private GameManagerSO gM;

    //Tracking del interactuable actual que tenemos delante.
    private Interactuable interactuableActual;

    private bool interactuando;

    private Controles misControles;

    private void OnEnable()
    {
        misControles= new Controles();
        misControles.Enable();
        misControles.Gameplay.Interactuar.started += Interactuar;
        gM.OnFinInteraccion += FinalizarInteraccion;
    }
    private void FinalizarInteraccion()
    {
        interactuableActual = null;
        interactuando = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!interactuando)
        {
            DetectarInteractuables();
        }        
    }
    private void DetectarInteractuables()
    {
        Collider[] collsDetectados = Physics.OverlapSphere(puntoInteraccion.position, radio, queEsInteractuable);
        if (collsDetectados.Length > 0) //Si hemos detectado algo...
        {
            //POO --> Encapsulamiento, abstracción, herencia, polimorfismo.
            //Siempre sabemos cual es el interactuable actual que tenemos delante.
            interactuableActual = collsDetectados[0].GetComponent<Interactuable>();
            interactuableActual.ActivarIcono();
        }
        else if(interactuableActual != null) //Si tengo un interactuable...
        {
            interactuableActual.DesactivarIcono(); //Desactivaré su icono

            interactuableActual = null; //Y me quedo interactuable
        }
    }

    private void Interactuar(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(interactuableActual != null)
        {
            interactuableActual.Interactuar(this);
            interactuando = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoInteraccion.position, radio);
    }
}