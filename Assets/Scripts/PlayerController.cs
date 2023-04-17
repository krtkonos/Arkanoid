using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject PlayBall;
    private Vector3 SpawnBallPos;

	private float ballSpawnHeight;
	private float ballRadius;
	private float paddleHeight;
	private float paddleWidth;
	private float paddleSpeed = 10.0f;

	public static bool canSpawnNewBall = false;

	public static PlayerController Instance { get; private set; }

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

	void Start()
    {
		GetReferences();		
        SpawnBall();
	}
	void Update()
    {
		float horizontalInput = 0f;

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			horizontalInput = -1f;
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			horizontalInput = 1f;
		}

		Vector3 raycastStartPosition = transform.position + new Vector3(horizontalInput * paddleWidth, 0, 0);
		float raycastDistance = paddleSpeed * Time.deltaTime;
		RaycastHit2D hit = Physics2D.Raycast(raycastStartPosition, new Vector2(horizontalInput, 0), raycastDistance, LayerMask.GetMask("Wall"));
		Debug.DrawLine(raycastStartPosition, raycastStartPosition + new Vector3(horizontalInput, 0, 0) * raycastDistance, Color.red, 0.1f);

		if (hit.collider == null)
		{
			Vector3 newPosition = transform.position + new Vector3(horizontalInput * paddleSpeed * Time.deltaTime, 0, 0);
			transform.position = newPosition;
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			SpawnBall();
		}
	}
	/// <summary>
	/// Gets references for spawning a ball, gets paddle width and height, sets ball spawnpoint depending on paddle and ball size
	/// </summary>
    private void GetReferences()
    {
		PlayBall = Resources.Load<GameObject>("PlayBall");
		SpriteRenderer ballrend = PlayBall.GetComponent<SpriteRenderer>();
		ballRadius = ballrend.bounds.extents.x;

		SpriteRenderer paddleRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
		paddleHeight = paddleRenderer.bounds.extents.y;
		paddleWidth = paddleRenderer.bounds.extents.x;
		ballSpawnHeight = ballRadius + (paddleHeight);
	}
    public void SpawnBall(bool spawnAndLaunch = false)
    {
		SpawnBallPos = new Vector3(transform.position.x, transform.position.y + ballSpawnHeight, transform.position.z);
		GameObject ball = Instantiate(PlayBall, SpawnBallPos, Quaternion.identity, transform);
		if (spawnAndLaunch)
		{
			LaunchBall(ball);
		}
    }	
	public void LaunchBall(GameObject ball)
	{
		ball.GetComponent<BallController>().LaunchBall();
	}
	/// <summary>
	/// Get paddle width for classics "gma mode"
	/// </summary>
	/// <returns></returns>
	public float GetWidth()
	{
		SpriteRenderer paddleRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
		return paddleRenderer.bounds.extents.x;
	}
}
