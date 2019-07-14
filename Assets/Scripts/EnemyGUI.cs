// GUI Element auf Gegner. Wird aktiv geschaltet, sofern ein Gegner ausgewählt ist.

using UnityEngine;
using System.Collections;

public class EnemyGUI : MonoBehaviour
{

    void Update()
    {
        //Rotate Target GUI
        gameObject.transform.Rotate(new Vector3(0, 0, -2));
        // Set TargetGUI Scale
        gameObject.transform.localScale = new Vector3(.023f, .023f, 1);
    }

    void HitByRay()
    {
        Debug.Log("Enemy hit by Laser Ray");
    }
}