using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{
    public int ActualHP;
    public int MaxHP;
	private void Start()
	{
		ActualHP = MaxHP;
	}
	/// <summary>
	/// Brick gets dmg, when HP is zero, adds score, removes brick from a list, possibly spawn a bonus and finaly destroys the brick
	/// </summary>
	/// <param name="dmg"></param>
	public void GetDmg(int dmg = 1)
	{
		ActualHP -= dmg;
		if (ActualHP <= 0 )
		{
			UIMenuCOntroller.Instance.AddScore(MaxHP);
			BrickSpawner.Instance.RemoveBrickFromList(gameObject);
			SpawnChance();
			Destroy(gameObject);
		}
	}
	/// <summary>
	/// Possibility to spawn a bonus which can help player if player catches it
	/// </summary>
	private void SpawnChance()
	{
		float chanceToSpawn = Random.Range(0, 100);
		if (chanceToSpawn <= 2)
		{
			Instantiate(Resources.Load<GameObject>("LifeBall"), transform.position, Quaternion.identity);
		}
		else if (chanceToSpawn < 10 && chanceToSpawn > 2)
		{
			Instantiate(Resources.Load<GameObject>("PointBall"), transform.position, Quaternion.identity);
		}
	}
}
