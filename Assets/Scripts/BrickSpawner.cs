using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
	private List<GameObject> bricksType = new List<GameObject>();
	public List<GameObject> allBricks = new List<GameObject>();
	public int numRows = 5;
	public int numColumns = 8;
	public const int numColumnsMax = 14;
	public const int numRowsMax = 7;
	private float xOffset = 0.1f;
	private float yOffset = 0.1f;

	public static BrickSpawner Instance { get; private set; }
	private void Awake()
	{
		Instance = this;
	}

	void Start()
	{		
		ReferenceBricks();
		//SpawnBricks();
	}
	/// <summary>
	/// Spawn ranom bricks in a simple X*Y matrix. Checks max columns and rows of the matrix, add brick to a list
	/// </summary>
	public void SpawnBricks()
	{
		SpriteRenderer brickRenderer = bricksType[0].GetComponent<SpriteRenderer>();
		float brickWidth = brickRenderer.bounds.size.x;
		float brickHeight = brickRenderer.bounds.size.y;

		float totalWidth = (brickWidth + xOffset) * numColumns - xOffset;
		if(numColumns > numColumnsMax)
		{
			numColumns = numColumnsMax;
		}
		if (numRows > numRowsMax)
		{
			numRows = numColumnsMax;
		}

		for (int row = 0; row < numRows; row++)
		{
			for (int col = 0; col < numColumns; col++)
			{
				float x = (col * (brickWidth + xOffset) - totalWidth / 2f) + brickWidth/2;
				float y = -row * (brickHeight + yOffset);
				Vector3 brickPosition = new Vector3(x, y, 0) + transform.position;
				int randomIndex = UnityEngine.Random.Range(0, bricksType.Count);
				allBricks.Add(Instantiate(bricksType[randomIndex], brickPosition, Quaternion.identity, transform));
			}
		}
	}
	/// <summary>
	/// Load Bricks and set their hp
	/// </summary>
	private void ReferenceBricks()
	{
		bricksType.Add(Resources.Load<GameObject>("BrickYellow"));
		bricksType.Add(Resources.Load<GameObject>("BrickRed"));
		bricksType[0].GetComponent<BrickController>().MaxHP = 1;
		bricksType[1].GetComponent<BrickController>().MaxHP = 2;
	}
	/// <summary>
	/// Remove brick from the list, if list is empty (last brick was destroyed) ends the game
	/// </summary>
	/// <param name="brick"></param>
	public void RemoveBrickFromList(GameObject brick)
	{
		allBricks.Remove(brick);
		if (allBricks.Count == 0 || allBricks == null)
		{
			UIMenuCOntroller.Instance.EndRound();
		}
	}
	/// <summary>
	/// Destroys All bricks and clear the list to be prepared for a new game
	/// </summary>
	public void ClearBrickList()
	{
		for (int i = 0; i < allBricks.Count; i++)
		{
			Destroy(allBricks[i]);
		}
		allBricks.Clear();
	}
}
