using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public List<BoatController> boats = new List<BoatController>();

    void Awake()
    {
        Instance = this;
    }

    public void ExecuteTurn()
    {
        foreach (BoatController boat in boats)
        {
            boat.ExecuteNextCommand();
        }
    }
}