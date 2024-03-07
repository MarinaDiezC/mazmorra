using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogo")]
public class DialogoSO : ScriptableObject
{
    [SerializeField, TextArea (1, 5)] private string[] frases;
    [SerializeField] private float velocidadDialogo;

    //Encapsulados de las variables para que se puedan utilizar en otro script:
    public string[] Frases { get => frases; }
    public float VelocidadDialogo { get => velocidadDialogo; }
}
