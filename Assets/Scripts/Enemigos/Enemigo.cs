using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    private Transform target;

    [Header("Overlap")]
    [SerializeField] private float radioVision;
    [SerializeField] private LayerMask queEsTarget;

    [Header("Angulo vista")]
    [SerializeField] private float anguloVision;
    [SerializeField] private LayerMask queEsObstaculo;

    [Header("Atacar")]
    [SerializeField] private float tiempoEntreAtaques;

    private float timer;

    #region Mis Componentes
    private NavMeshAgent agent;
    private SistemaPatrulla sistemaPatrulla;
    private Animator anim;
    #endregion

    private enum Estado { Patrullar, Perseguir, Atacar };

    Estado estadoActual;

    public float RadioVision { get => radioVision; }
    public float AnguloVision { get => anguloVision; }



    // Start is called before the first frame update
    void Start()
    {
        estadoActual = Estado.Patrullar;
        agent = GetComponent<NavMeshAgent>();
        sistemaPatrulla = GetComponent<SistemaPatrulla>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (estadoActual == Estado.Perseguir)
        {
            Perseguir();
        }
        else if (estadoActual == Estado.Atacar)
        {
            Atacar();
        }
    }
    private void FixedUpdate()
    {
        if (estadoActual == Estado.Patrullar)
        {
            DetectarTargets();
        }
    }
    private void DetectarTargets()
    {
        Collider[] collsDetectados = Physics.OverlapSphere(transform.position, radioVision, queEsTarget);

        if (collsDetectados.Length > 0) //Has pasado el primer check.
        {
            target = collsDetectados[0].transform;

            Vector3 direccionATarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, direccionATarget) <= anguloVision / 2) //Segundo check.
            {
                //último check
                if (!Physics.Raycast(transform.position, direccionATarget, radioVision, queEsObstaculo))
                {
                    agent.isStopped = true;
                    anim.ResetTrigger("IsAlert");
                    anim.SetTrigger("IsAlert");
                    Invoke(nameof(Alerta), 0.5f); //Llamar a función con retardo.

                }
            }



        }
    }

    private void Alerta()
    {
        estadoActual = Estado.Perseguir;
    }
    private void Perseguir()
    {
        sistemaPatrulla.enabled = false;
        agent.enabled = true;
        agent.speed = 3f;
        agent.stoppingDistance = 1.5f; //Distancia covid.
        agent.SetDestination(target.position);

        if (agent.remainingDistance <= agent.stoppingDistance) //pARA ATACAR
        {
            estadoActual = Estado.Atacar;
        }
        else if (agent.remainingDistance > radioVision) //PARA perder de vista.
        {
            sistemaPatrulla.enabled = true;
            agent.stoppingDistance = 0;
            agent.speed = 1F;
            estadoActual = Estado.Patrullar;
        }
    }
    private void Atacar()
    {
        timer += Time.deltaTime;
        if (timer >= tiempoEntreAtaques)
        {
            agent.isStopped = true;
            anim.SetTrigger("Attack");
            timer = 0f;

            if (Vector3.Distance(transform.position, target.position) > 1.5f)
            {
                timer = tiempoEntreAtaques;
                agent.isStopped = false;
                estadoActual = Estado.Perseguir;
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}