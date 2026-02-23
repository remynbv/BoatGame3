using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
public class BoatControllers : MonoBehaviour
{
    public Tilemap tilemap;
    public Vector3Int currentCell;  // current pos
    private int facing = 0;         // Facing Dir    
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
    }
    private void Awake()
    {
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

    private void Forward()
    {
        Move(getDirs()[facing]);
    }

    private void Backward()
    {
        int backDir = (facing + 3) % 6;
        Move(getDirs()[backDir]);
    }

    private void RotateLeft()
    {
        facing = (facing + 5) % 6;
        RotatePic();
    }

    private void RotateRight()
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

}
