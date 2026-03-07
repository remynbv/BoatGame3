using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
public class BoatController : MonoBehaviour
{
    public Tilemap tilemap;
    public Vector3Int currentCell;  // current pos
    private int facing = 0;         // Facing Dir    

    public int speed = 0;
    public int minSpeed = -2;
    public int maxSpeed = 4;

    public List<BoatCommand> commandQueue = new List<BoatCommand>();
    public int maxCommands = 3;

    public bool Selected = false;
    public bool hasCrashed = false;

    public int hitPoints = 2;
    public DisplayOrders tabOrders;
    public SpriteRenderer boatImage;

    public void AddCommand(BoatCommand command)
    {
        if (TurnManager.Instance.ordersOpen)
        {
            commandQueue.Add(command);
            if (commandQueue.Count > maxCommands)
            {
                commandQueue.RemoveAt(maxCommands-1);
            }
        }
    }

    private BoatActions actions;
    /* 
    0 = Top
    1 = Top right
    2 = Bottom Right
    3 = Bottom
    4 = bottom left
    5 = top left
    */
    private readonly Vector3Int[] evenDirs = new Vector3Int[]
    {
        new Vector3Int(1, 0, 0),    
        new Vector3Int(0, 1, 0),    
        new Vector3Int(-1, 1, 0),   
        new Vector3Int(-1, 0, 0),   
        new Vector3Int(-1, -1, 0),   
        new Vector3Int(0, -1, 0)    
    };
    private readonly Vector3Int[] oddDirs = new Vector3Int[]
    {
        new Vector3Int(1, 0, 0),    
        new Vector3Int(1, 1, 0),   
        new Vector3Int(0, 1, 0),    
        new Vector3Int(-1, 0, 0),    
        new Vector3Int(0, -1, 0),   
        new Vector3Int(1, -1, 0),    
    };


    void Start()
    {
        SnapToGrid();
        TurnManager.Instance.boats.Add(this);
        boatImage = GetComponent<SpriteRenderer>();
    }
    private void Awake()
    {
        actions = new BoatActions();
    }

    public void SetSelected(bool selected)
    {
        Selected = selected;
        // Change color or something to indicate selection later
    }
    
    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }

    public void SnapToGrid() 
    {
        transform.position = tilemap.GetCellCenterWorld(currentCell);
    }

    public int GetFacing()
    {
        return facing;
    }

    public void Forward()
    {
        Move(getDirs()[facing]);
    }

    public void Backward()
    {
        int backDir = (facing + 3) % 6;
        Move(getDirs()[backDir]);
    }

    public void RotateLeft()
    {
        facing = (facing + 5) % 6;
        RotatePic();
    }

    public void RotateRight()
    {
        facing = (facing + 1) % 6;
        RotatePic();
    }

    private Vector3Int[] getDirs()
    {   
        if (currentCell.y % 2 == 0)
        {
            return evenDirs;
        }
        else
        {
            return oddDirs;
        }
    }

    private void Move(Vector3Int offset)
    {
        Vector3Int targetCell = currentCell + offset;
        if (!tilemap.HasTile(targetCell)) return;

        currentCell = targetCell;
        transform.position = tilemap.GetCellCenterWorld(currentCell);

        print((currentCell.x, currentCell.y));
        print("");
        SnapToGrid();
    }

    void RotatePic()
    {
        transform.rotation = Quaternion.Euler(0,0,-facing*60f);
    }

    public Vector3Int[] GetDirs(int y)
    {
        if (y % 2 == 0)
        {
            return evenDirs;
        }
        else
        {
            return oddDirs;
        }
    }

    public void takeDamage()
    {
        hitPoints -= 1;
        if (hitPoints <= 0)
        {
            TurnManager.Instance.deadBoats.Add(this);
            tabOrders.setDestroyed();
        }
        else if (hitPoints == 1)
        {
            tabOrders.setDamaged();
        }
    }

    public void destroyBoat()
    {
        //TurnManager.Instance.boats.Remove(this);
        GameObject dead = Instantiate(TurnManager.Instance.deadBoatPrefab, transform.position, transform.rotation);
        Destroy(this, 0.1f);
        Destroy(dead, 2f);
    }

    public bool CheckCollision()
    {
        foreach (BoatController boat in TurnManager.Instance.boats)
        {
            if (boat != this && boat.currentCell == this.currentCell)
            {
                return true;
            }
        }
        return false;
    }

}
