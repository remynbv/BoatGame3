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

    public void AddCommand(BoatCommand command)
    {
        commandQueue.Add(command);
        if (commandQueue.Count > maxCommands)
        {
            commandQueue.RemoveAt(maxCommands-1);
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
    }
    private void Awake()
    {
        
        actions = new BoatActions();

        /*
        actions = new BoatActions();

        //move
        actions.Movement.Move.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            if (input.y > 0) Forward();
            if (input.y < 0) Backward();
        };

        //rotte
        actions.Movement.Rotation.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            if (input.x > 0) RotateRight();
            if (input.x < 0) RotateLeft();
        };
        */
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

    public void SnapToGrid() //Snaps to grid
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

    public void ExecuteNextCommand()
    {
        if (commandQueue.Count == 0)
            return;

        BoatCommand currentCommand = commandQueue[0];
        commandQueue.RemoveAt(0);

        if (currentCommand.commandType == BoatCommandType.Forward)
        {
            speed += 1;
            if (speed > maxSpeed) 
            {
                speed = maxSpeed;
            }
        } else if (currentCommand.commandType == BoatCommandType.Backward)
        {
            speed -= 1;
            if (speed < minSpeed) 
            {
                speed = minSpeed;
            }
        }

        if (speed > 0)
        {
            for (int i = 0; i < speed; i++)
            {
                Forward();
            }
        } else if (speed < 0)
        {
            int backwardSpeed = -speed;
            for (int i = 0; i < backwardSpeed; i++)
            {
                Backward();
            }
        }

        if (currentCommand.commandType == BoatCommandType.RotateLeft)
        {
            RotateLeft();
        } else if (currentCommand.commandType == BoatCommandType.RotateRight)
        {
            RotateRight();
        }
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

}
