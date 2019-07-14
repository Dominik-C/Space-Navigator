// Verwaltung von Schiffbewegungen

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ShipBehaviour))]
[RequireComponent(typeof(PlayerMovment))]

public class ShipMovment : MonoBehaviour
{    
    public bool isPlayer = false;
    private PlayerMovment movment;
    private ShipBehaviour behaviour;    

    public static ShipMovment PlayerShip
    {
        get
        {
            return playerShip;
        }
    }

    public bool UsingMouseInput
    {
        get
        {
            return movment.useMouseInput;
        }
    }

    public Vector3 Velocity
    {
        get
        {
            return behaviour.Rigidbody.velocity;
        }
    }

    private static ShipMovment playerShip;

    private void Awake()
    {
        movment = GetComponent<PlayerMovment>();
        behaviour = GetComponent<ShipBehaviour>();
    }

    // Es sollte niemals mehr als ein Spieler in der Scene sein!
    private void Update()
    {
        // Gebe die Movements an Behaviour weiter
        behaviour.ShipInput(new Vector3(movment.seitwaerts, 0.0f, 0.0f), new Vector3(movment.mousePitch, movment.mouseYaw, movment.rollen));
        if (isPlayer)
            playerShip = this;
    }
}
