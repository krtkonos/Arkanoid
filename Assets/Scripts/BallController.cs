using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BallController : MonoBehaviour
{
	private float ballSpeed = 8.0f;
	private Vector2 velocity;
	private bool isLaunched = false;
	private float ballRadius;
	private readonly int minLaunchDeg = 20;
	private readonly int maxLaunchDeg = 160;
	BallListManager ballListMan;
	PlayerController paddle;


	void Start()
    {
		SpriteRenderer ballrend = GetComponent<SpriteRenderer>();
		ballRadius = ballrend.bounds.extents.x;
		ballListMan = BallListManager.Instance;
		ballListMan.AddToList?.Invoke(this);
		paddle = PlayerController.Instance;
	}

    void Update()
    {
		if (!isLaunched && Input.GetKeyDown(KeyCode.Space))
		{
			LaunchBall();
		}
		if (IsOutOfScreenBounds())
		{
			DestroyBall();
		}
	}
	void FixedUpdate()
	{
		if (isLaunched)
		{
			Vector3 newPosition = transform.position + (Vector3)velocity * Time.fixedDeltaTime;
			Vector2 collisionNormal = CheckCollision(newPosition);
			if (collisionNormal != Vector2.zero)
			{
				//velocity = Vector2.Reflect(velocity, collisionNormal);
				Vector2 reflection = velocity - 2 * Vector2.Dot(velocity, collisionNormal) * collisionNormal;
				velocity = reflection;

				newPosition += (Vector3)collisionNormal * 0.02f;
			}
			transform.position = newPosition;
		}
	}
	/// <summary>
	/// Lunch Ball in a random degree between minLaunchDeg and maxLaunchDeg
	/// </summary>
	public void LaunchBall()
	{
		float randomAngle = Random.Range(minLaunchDeg, maxLaunchDeg) * Mathf.Deg2Rad;
		velocity = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * ballSpeed;
		isLaunched = true;
		transform.parent = BallListManager.Instance.transform;
	}
	/// <summary>
	/// CheckCollision, If ball colide with Wall - just bounce, if player, check "gamemode" and bounce, if brick give DMG to the brick 
	/// In classic "gamemode" ball bounce depending on position where hit the paddle which allows to influence the ball direction
	/// </summary>
	/// <param name="newPosition"></param>
	/// <returns></returns>
	private Vector2 CheckCollision(Vector3 newPosition)
	{
		int layerMask = LayerMask.GetMask("Player") | LayerMask.GetMask("Wall") | LayerMask.GetMask("Brick");
		RaycastHit2D hit = Physics2D.CircleCast(newPosition, ballRadius, Vector2.zero, 0, layerMask);
		if (hit.collider != null)
		{
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Brick"))
			{
				BrickController brick = hit.collider.gameObject.GetComponent<BrickController>();
				brick.GetDmg();
			}
			else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player") && UIMenuCOntroller.Instance.classics)
			{				
				float relativeHitPosition = (transform.position.x - paddle.transform.position.x) / (paddle.GetWidth() / 2);
				float bounceAngle = Mathf.Lerp(minLaunchDeg, maxLaunchDeg, 1 - (relativeHitPosition + 1) / 2) * Mathf.Deg2Rad;
				velocity = new Vector2(Mathf.Cos(bounceAngle), Mathf.Abs(Mathf.Sin(bounceAngle))) * ballSpeed;
				return Vector2.zero;
			}
			return hit.normal;
		}
		return Vector2.zero;
	}
	/// <summary>
	/// If ball is out of scrren, Destroy it, secures the game will run if ball bugs through wall
	/// </summary>
	/// <returns></returns>
	private bool IsOutOfScreenBounds()
	{
		Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

		if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
		{
			return true;
		}

		return false;
	}
	/// <summary>
	/// Invoke an action to remove ball from the list and than destroy ball
	/// </summary>
	private void DestroyBall() 
	{
		ballListMan.removeFromList?.Invoke(this);
		Destroy(gameObject);
	}
}
