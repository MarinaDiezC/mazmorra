using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Vector2 direccionInput;
    private Vector3 direccionMovimiento;
    [SerializeField] private float velocidad;

    private CharacterController controller;

    Controles misControles;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable() //Se activa cada vez que se activa el script.
    {
        misControles = new Controles();

        misControles.Gameplay.Enable();

        misControles.Gameplay.Interactuar.started += Interactuar;
        misControles.Gameplay.Mover.started += Mover;
        misControles.Gameplay.Mover.canceled += CancelarMovimiento; ;   
    }
    void Start()
    {

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
        direccionMovimiento = new Vector3(direccionInput.x, 0, direccionInput, y);
        controller.Move(direccionMovimiento * velocidad * Time.deltaTime);
    }
}
