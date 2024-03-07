using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AI;

public class SistemaPatrulla : MonoBehaviour
{
    [SerializeField] private Transform zonaPatrulla;

    int[] miArray = new int[50];
    List<Vector3> puntosPatrulla = new List<Vector3>();
    private NavMeshAgent agent;
    private Animator anim;

    private int indiceObjetivo = 0; //Me sirve para hacer un tracking de cual es el ídice del siguiente punto. 

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        foreach (Transform item in zonaPatrulla)
        {
            puntosPatrulla.Add(item.position);
        }
    }
    private void OnEnable()
    {
        agent.enabled = true;
        StartCoroutine(PatrullarYEsperar());
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }
    // Update is called once per frame
    private IEnumerator PatrullarYEsperar()
    {
        while (true)
        {
            agent.SetDestination(puntosPatrulla[indiceObjetivo]);
            //Expresión LAMBDA es una forma de introducir un código de forma aonima.
            yield return new WaitUntil(() => agent.pathPending == false);

            anim.SetBool("walking", true);

            yield return new WaitUntil(() => agent.remainingDistance <= 0.2f);

            anim.SetBool("walking", false);

            yield return new WaitForSeconds(Random.Range(1f, 5f));
            
            //Si has llegado...
            CalcularNuevoPunto();
        }
    }

    private void CalcularNuevoPunto()
    {
        indiceObjetivo++;
        indiceObjetivo = Random.Range(0, puntosPatrulla.Count);

        if (indiceObjetivo >= puntosPatrulla.Count)
        {
            indiceObjetivo = 0;
        }        
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        agent.enabled= false;
    }
}
