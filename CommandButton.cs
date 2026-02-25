using UnityEngine;

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
        print("Command sent: " + boatCommand.commandType);
    }
}