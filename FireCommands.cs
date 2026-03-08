using UnityEngine;

public enum FireCommandType
{
    Nothing,
    FireBackLeft,
    FireBackRight,
    FireFrontLeft,
    FireFrontRight

}

[System.Serializable]
public class FireCommand
{
    public FireCommandType fireCommandType;
    public FireCommand(FireCommandType type)
    {
        fireCommandType = type;
    }

    
}