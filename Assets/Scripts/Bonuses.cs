using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonuses : MonoBehaviour
{
	protected float speed = 3f;
	private float ballRadius;
	public enum BonusType
	{
		None = 0,
		Life,
		NewBall
	};
	public BonusType bonustype;
	private void Start()
	{
		SpriteRenderer ballrend = GetComponent<SpriteRenderer>();
		ballRadius = ballrend.bounds.extents.x;
	}
	void Update()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y - (speed * Time.deltaTime), transform.position.z);
		CheckCollision();
		if (transform.position.y < -5f)
		{
			Destroy(gameObject);
		}
	}
	/// <summary>
	/// Check if player catches the bonus
	/// If bonus is Life bonustype, adds a Life
	/// If bonus is NewBall bonustype, spawns and launches two additional balls
	/// </summary>
	private void CheckCollision()
	{
		RaycastHit2D hit = Physics2D.CircleCast(transform.position, ballRadius, Vector2.zero, 0, LayerMask.GetMask("Player"));
		if (hit.collider != null)
		{
			if (bonustype == BonusType.Life)
			{
				UIMenuCOntroller.Instance.AddLive();
			}
			else if (bonustype == BonusType.NewBall)
			{
				PlayerController.Instance.SpawnBall(true);
				PlayerController.Instance.SpawnBall(true);
			}
			Destroy(gameObject);
		}
	}
}
