using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (Enemigo))]
public class FieldOfView : Editor
{	
	void OnSceneGUI() 
	{
		Enemigo fow = (Enemigo) target;
		Handles.color = Color.white;
		Handles.DrawWireArc (fow.transform.position, Vector3.up, Vector3.forward, 360, fow.RadioVision);
		Vector3 viewAngleA = fow.DirFromAngle (-fow.AnguloVision / 2, false);
		Vector3 viewAngleB = fow.DirFromAngle (fow.AnguloVision / 2, false);

		Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleA * fow.RadioVision);
		Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleB * fow.RadioVision);
	}
}
