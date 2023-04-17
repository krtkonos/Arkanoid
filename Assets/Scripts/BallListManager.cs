using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallListManager : MonoBehaviour
{
	public static BallListManager Instance { get; private set; }
	public List<BallController> ballsList = new List<BallController>();

	public Action<BallController> removeFromList;
	public Action<BallController> AddToList;

	public UIMenuCOntroller UIMenup;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	private void OnEnable()
	{
		removeFromList += BallRemoveFromList;
		AddToList += AddBallToList;
	}
	private void OnDisable()
	{
		removeFromList -= BallRemoveFromList;
		AddToList -= AddBallToList;
	}
	private void AddBallToList(BallController ball)
	{
		ballsList.Add(ball);
	}
	private void BallRemoveFromList(BallController ball)
	{
		ballsList.Remove(ball);
		if ((ballsList == null || ballsList.Count == 0) && PlayerController.Instance != null)
		{
			PlayerController.Instance.SpawnBall();
			UIMenup.RemoveLive();
		}
	}
}
