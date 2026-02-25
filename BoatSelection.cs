using UnityEngine;
using UnityEngine.InputSystem;

public class BoatSelection : MonoBehaviour
{
    public static BoatController SelectedBoat;

    public void SelectBoat(BoatController boat)
    {
        for (int i = 0; i < TurnManager.Instance.boats.Count; i++)
        {
            TurnManager.Instance.boats[i].SetSelected(false);
        }
        SelectedBoat = boat;
        boat.SetSelected(true);
        print("Selected boat: " + boat.name);
    }

    Vector3 mousePosition;
    RaycastHit2D raycastHit2D;
    Transform clickObject;

    // Update is called once per frame
    void Update()
    {
        mousePosition = Mouse.current.position.ReadValue();

        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
            clickObject = raycastHit2D ? raycastHit2D.collider.transform : null;

            if (clickObject)
            {
                BoatController boat = clickObject.GetComponent<BoatController>();
                if (boat)
                {
                    SelectBoat(boat);
                }
            }
        }
    }
}
