using UnityEngine;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public FireCommand fireCommand;
    public void sendFireCommand()
    {
         if (BoatSelection.SelectedBoat == null)
        {
            print("No selected boat");
            return;
        }
        BoatSelection.SelectedBoat.AddFireCommand(fireCommand);
        print("Fire command sent: " + fireCommand.fireCommandType);
        EventSystem.current.SetSelectedGameObject(null);

    }
}
