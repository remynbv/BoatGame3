using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandButton : MonoBehaviour
{
    public BoatCommand boatCommand;
    public void SendCommand()
    {

       
        if (BoatSelection.SelectedBoat == null)
        {
            print("No selected boat");
            return;
        }
        BoatSelection.SelectedBoat.AddCommand(boatCommand);
        print("Move command sent: " + boatCommand.commandType);
        EventSystem.current.SetSelectedGameObject(null);

    }

}