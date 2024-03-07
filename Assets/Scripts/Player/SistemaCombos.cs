using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaCombos : MonoBehaviour
{
    [SerializeField] private int ataquesCombo;
    [SerializeField] private float tiempoCoolDown;

    private Controles misControles;

    //Determinar si estoy en cool down
    private bool coolingDown;

    //Determina si puedo enlazar ataques
    private bool puedeCombo = true;

    //Determina en que estado de ataque estoy
    private int estadoDeAtaque;

    private Animator anim;

    private float timer;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    

    private void OnEnable()
    {
        misControles = new Controles();

        misControles.Enable();

        misControles.Gameplay.Atacar.started += Atacar;
        //diferencia entre started y performance es si se ha iniciado el recorrido
        //diferente recorrido: performance
    } 

    private void Atacar(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(puedeCombo && !coolingDown) //esto evita que el animator se sature en el caso de spameo del botón atacar
        {
            estadoDeAtaque++;
            if (estadoDeAtaque > ataquesCombo)
            {
                estadoDeAtaque = 1;
            }
            anim.SetInteger("estadoAtaque", estadoDeAtaque);
            puedeCombo = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ResetearCombo()
    {
        estadoDeAtaque = 0;
        anim.SetInteger("estadoAtaque", estadoDeAtaque);
        puedeCombo = true;
    }

    private void ActualizarTimer()
    {
        timer += Time.deltaTime;
        if(timer >= tiempoCoolDown)
        {
            coolingDown = false;
            timer = 0;
            ResetearCombo();
        }
    }
    #region Evetos de animación
    private void VentanaDeCombo()
    {
        puedeCombo = true;
    }

    private void FinAnimacionAtaque()
    {
        if (estadoDeAtaque == 3)
        {
            coolingDown= true;
        }
        ResetearCombo();
    }
    #endregion
}
