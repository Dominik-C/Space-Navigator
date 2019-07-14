// Zeigt das Fadenkreuz an

using UnityEngine;
using UnityEngine.UI;

public class CrosshairGUI : MonoBehaviour
{
    private Image fadenkreuz;

    private void Awake()
    {
        fadenkreuz = GetComponent<Image>();
    }

    private void Update()
    {
        if (fadenkreuz != null && ShipMovment.PlayerShip != null)
        {
            fadenkreuz.enabled = ShipMovment.PlayerShip.UsingMouseInput;

            if (fadenkreuz.enabled)
            {
                fadenkreuz.transform.position = Input.mousePosition;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
