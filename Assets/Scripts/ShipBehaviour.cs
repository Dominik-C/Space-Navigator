// Zuständig für Physikalisches Schiffverhalten

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
    public Vector3 lineareKraft = new Vector3(100.0f, 100.0f, 100.0f);      // Instanziiert Vec3 für lineare Force
    public Vector3 diagonaleKraft = new Vector3(100.0f, 100.0f, 100.0f);    // Instanziiert Vec3 für diagonale Force
    public float kraftverstaerker = 100.0f;                                 // Verstärkt Kraft
    public Rigidbody Rigidbody { get { return rbody; } }                    // Get Rigid Body
    private Vector3 kraftLinear = Vector3.zero;                             // Vector mit (0,0,0) Values
    private Vector3 kraftDiagonal = Vector3.zero;                           // Vector mit (0,0,0) Values
    private ShipMovment ship;                                               // Get Playership
    private Rigidbody rbody;

    void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        ship = GetComponent<ShipMovment>();
    }

    void FixedUpdate()
    {
        // Kraft & Drehmoment bestimmen
        if (rbody != null)
        {
            rbody.AddRelativeForce(kraftLinear * kraftverstaerker, ForceMode.Force); 
            rbody.AddRelativeTorque(kraftDiagonal * kraftverstaerker, ForceMode.Force);
        }
    }

    // Legt fest, wieviel Kraft auf das Schiff wirkt.
    public void ShipInput(Vector3 linearInput, Vector3 diagonalInput)
    {
        kraftLinear = VecX(linearInput, lineareKraft);
        kraftDiagonal = VecX(diagonalInput, diagonaleKraft);
    }

    // Gibt multiplizierten Vektor zur Kraftbestimmung zurück
    private Vector3 VecX(Vector3 links, Vector3 rechts)
    {
        Vector3 vectorMiltiply;
        vectorMiltiply.x = links.x * rechts.x;
        vectorMiltiply.y = links.y * rechts.y;
        vectorMiltiply.z = links.z * rechts.z;
        return vectorMiltiply;
    }
}