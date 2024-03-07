using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaMovimiento : MonoBehaviour
{

    //Direccion que marca el input (mando, teclado)
    private Vector2 direccionInput;
    private Vector3 direccionMovimiento, vectorVertical;

    private CharacterController controller;
    [SerializeField] private float velocidad;
    [SerializeField] private float tiempoRotacion;
    [SerializeField] private float factorGravedad;
    [SerializeField] private float alturaSalto;

    [Header("Deteción suelo")]
    [SerializeField] private Transform pies;
    [SerializeField] private float radioDeteccion;
    [SerializeField] private LayerMask queEsSuelo;
    private bool enSuelo;

    private Controles misControles;
    private Animator anim;

    private float velocidadRotacion;   

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        
    }
    private void OnEnable() //Se activa cada vez que se activa el script.
    {
        misControles = new Controles();
        misControles.Gameplay.Enable();
        
        misControles.Gameplay.Mover.performed += Mover;
        misControles.Gameplay.Mover.canceled += CancelarMovimiento; ;
        misControles.Gameplay.Saltar.started += Saltar;
    }

    private void Saltar(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (enSuelo)
        {
            anim.SetBool("grounded", false);
            anim.SetTrigger("jump");
            vectorVertical.y = Mathf.Sqrt(-2 * alturaSalto * factorGravedad);
        }
    }    

    private void Mover(UnityEngine.InputSystem.InputAction.CallbackContext ctx) //ctx de "contexto"
    {
        direccionInput = ctx.ReadValue<Vector2>();
        Debug.Log("Me muevo hacia..." + direccionInput);
        anim.SetFloat("velocidad", direccionInput.magnitude);
    }
    
    private void CancelarMovimiento(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        direccionInput = ctx.ReadValue<Vector2>();
        Debug.Log("Me muevo hacia..." + direccionInput);
        anim.SetFloat("velocidad", direccionInput.magnitude);
    }   
    

    void Update()
    {
        MoverYRotar();
        AplicarGravedad();
    }

    private void MoverYRotar()
    {
        if(direccionInput != null) //Si hay movimiento
        {
            //Calculo el ángulo al cual tiene que rotarse mi personaje en función de los inputs y la cámara.
            float anguloObjetivo = Mathf.Atan2(direccionInput.x, direccionInput.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

            //Suavizo el ángulo a mi rotación
            float anguloSuavizado = Mathf.SmoothDampAngle(transform.eulerAngles.y, anguloObjetivo, ref velocidadRotacion, tiempoRotacion); 

            //Aplico el ángulo a mi rotación
            transform.eulerAngles = new Vector3(0, anguloSuavizado, 0);

            //Mi direccion de movimiento es en función del Z del mundo PERO rotando tanto como "anguloObjetivo".
            direccionMovimiento= Quaternion.Euler(0, anguloObjetivo, 0) * new Vector3(0, 0, 1);

            //Me muevo, considerando la magnitud del vector de input: a más vuelque el joystick, más rápido iré-
            controller.Move(direccionMovimiento * velocidad * direccionInput.magnitude * Time.deltaTime);
        }
        
    }
    
    private void AplicarGravedad()
    {
        vectorVertical.y += factorGravedad * Time.deltaTime;
        controller.Move(vectorVertical * Time.deltaTime); //El delta es doble porque la gravedad se mide por m/s^2
        //Physics.CheckSphere()
        enSuelo = Physics.CheckSphere(pies.position, radioDeteccion, queEsSuelo); //este bool da la informacion de donde está el suelo

        anim.SetBool("grounded", enSuelo); //En todo momento estoy pendiente si pongo grounded a true o false.

        if(enSuelo && controller.velocity.y < 0) //Si aterrizo
        {
            anim.SetBool("falling", false);
            vectorVertical.y = 0; //Reseteo mi gravedad para que no se acumule.         
        }
        else if(controller.velocity.y < 0) //SI estoy cayendo
        {
            anim.SetBool("falling", true);
        }
    }   

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pies.position, radioDeteccion);
    }
}
