using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //Direccion que marca el input (mando, teclado)
    private Vector2 direccionInput;
    private Vector3 direccionMovimiento;
    [SerializeField] private float velocidad;
    [SerializeField] private float tiempoRotacion;

    private CharacterController controller;

    Controles misControles;

    private float velocidadRotacion;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    void Start()
    {

    }
    private void OnEnable() //Se activa cada vez que se activa el script.
    {
        misControles = new Controles();

        misControles.Gameplay.Enable();

        misControles.Gameplay.Interactuar.started += Interactuar;
        misControles.Gameplay.Mover.started += Mover;
        misControles.Gameplay.Mover.canceled += CancelarMovimiento; ;   
    }

    private void Interactuar(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Inreactuo!");
    }

    private void Mover(UnityEngine.InputSystem.InputAction.CallbackContext ctx) //ctx de "contexto"
    {
        direccionInput = ctx.ReadValue<Vector2>();
        Debug.Log("Me muevo hacia..." + direccionInput);
    }
    
    private void CancelarMovimiento(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        direccionInput = ctx.ReadValue<Vector2>();
        Debug.Log("Me muevo hacia..." + direccionInput);
    }   
    

    void Update()
    {
        MoverYRotar();
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

            //Me muevo.
            direccionMovimiento = new Vector3(direccionInput.x, 0, direccionInput.y);
            controller.Move(direccionMovimiento * velocidad * Time.deltaTime);

            //Me muevo, considerando la magnitud del vector de input: a más vuelque el joystick, más rápido iré-
            controller.Move(direccionMovimiento * velocidad * direccionInput.magnitude * Time.deltaTime);
        }
        
    }
}
