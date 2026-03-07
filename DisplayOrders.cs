using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayOrders : MonoBehaviour
{
	public BoatController boat;

	public GameObject forwardArrow;
	public GameObject backwardArrow;
	public GameObject rotateLeftArrow;
	public GameObject rotateRightArrow;
    public GameObject nothingArrow;

	public Transform arrowContainer;

	public float arrowSpacing = 40f;

	private List<GameObject> arrows = new List<GameObject>();

    private Image tabImage;
	private bool destroyed = false;

    private void Start()
    {
        tabImage = GetComponent<Image>();
    }

	public void Update()
	{
		DestroyArrows();
		if (!destroyed)
		{
			UpdateArrows(); 
		}
	}

	private void DestroyArrows()
	{
		foreach (var obj in arrows)
		{
			if (obj != null)
				Destroy(obj);
		}
		arrows.Clear();
	}
    private void UpdateArrows()
    {
		if (boat == null || boat.commandQueue == null)
			return;

		for (int i = 0; i < boat.commandQueue.Count; i++)
		{
			var command = boat.commandQueue[i];
			GameObject arrowPrefab = null;
			switch (command.commandType)
			{
				case BoatCommandType.Forward:
					arrowPrefab = forwardArrow;
					break;
				case BoatCommandType.Backward:
					arrowPrefab = backwardArrow;
					break;
				case BoatCommandType.RotateLeft:
					arrowPrefab = rotateLeftArrow;
					break;
				case BoatCommandType.RotateRight:
					arrowPrefab = rotateRightArrow;
					break;
                case BoatCommandType.Nothing:
                    arrowPrefab = nothingArrow;
                    break;
			}
			if (arrowPrefab != null)
			{
				GameObject arrow = Instantiate(arrowPrefab, arrowContainer);
				arrow.transform.localPosition = new Vector3(0, -(i + 1) * arrowSpacing, 0);
				arrows.Add(arrow);
			}
		}
    }

    public void setDamaged()
    {
        boat.boatImage.color = new Color32(255,75,75,230);
        tabImage.color = new Color32(255,75,75,230);
    }

    public void setDestroyed()
    {
        boat.boatImage.color = new Color32(50,70,100,230);
        tabImage.color = new Color32(50,70,100,230);
		destroyed = true;
    }
}
