using UnityEngine;

public enum BoatCommandType
{
    Forward,
    Backward,
    RotateLeft,
    RotateRight,
    Nothing
}

[System.Serializable]
public class BoatCommand
{
    public BoatCommandType commandType;
    public BoatCommand(BoatCommandType type)
    {
        commandType = type;
    }

    
}