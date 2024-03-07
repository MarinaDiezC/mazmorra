using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zonaPatrulla : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        foreach(Transform item in this.transform)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }
    }
}
