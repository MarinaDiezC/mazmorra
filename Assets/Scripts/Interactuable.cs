using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactuable : MonoBehaviour //Funciona como un contrato. Quien la implemente está obligado a hacerlo
{    
    [SerializeField] protected GameObject canvasInteraccion;
    public abstract void Interactuar(SistemaInteraccion interactuador);
    
    public void ActivarIcono()
    {
        canvasInteraccion.SetActive(true);
    }
    
    public void DesactivarIcono()
    {
        canvasInteraccion.SetActive(false);
    }
    //Conjunto de metodos que alguien tiene que cumplir de forma abstracta
}
