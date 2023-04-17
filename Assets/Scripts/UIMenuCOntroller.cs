using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class UIMenuCOntroller : MonoBehaviour
{
	private GameObject lives;
	private GameObject playerPaddle;
	private Transform startGame;
    private Button startGameButton;
    private Toggle classicBounce;
	private Transform livesShow;
    private List<GameObject> livesList = new List<GameObject>();
    private TextMeshProUGUI scoreText;
    public BrickSpawner spawner;

	private float spawnHeight = -4.33f;
	private float MaxLives = 5f;
    private int score = 0;
	public bool classics;
	public static UIMenuCOntroller Instance { get; private set; }
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
        UpdateScore();
	}
    /// <summary>
    /// Reference the UI Elements
    /// </summary>
    private void GetReferences()
    {
		lives = Resources.Load<GameObject>("OneLive");
		playerPaddle = Resources.Load<GameObject>("PlayerContainer");
		livesShow = transform.Find("TheGame/LivesText/LivesHolder");
		startGame = transform.Find("PlayGame");
		startGameButton = startGame.transform.Find("PlayGameButton").GetComponent<Button>();
		classicBounce = startGame.transform.Find("Toggle").GetComponent<Toggle>();
		scoreText = transform.Find("TheGame/Score").GetComponent<TextMeshProUGUI>();
		startGameButton.onClick.AddListener(StartArkanoid);
	}
    /// <summary>
    /// Destroy all lives icon and spawn new three
    /// </summary>
    /// <param name="lives"></param>
    private void SpawnNewLives(int lives = 3)
    {
        for (int i = 0; i < livesList.Count; i++)
        {
            Destroy(livesList[i]);
        }
        livesList.Clear();
        for (int i = 0; i < lives; i++) 
        {
            if(livesList.Count < 5)
            {
                AddLive();
            }
		}
    }
	public void AddLive()
    {
        if (livesList.Count < MaxLives)
        {
		    livesList.Add(Instantiate(lives, livesShow));
        }
	}
    public void RemoveLive() 
    {        
        GameObject live = livesList[livesList.Count - 1];
        livesList.Remove(live);
        Destroy(live);

        if (livesList.Count == 0)
        {
            EndRound();
		}
    }
    private void SpawnPlayerPaddle()
    {
		Instantiate(playerPaddle, new Vector3(0f, spawnHeight, 0f), Quaternion.identity);
	}
    /// <summary>
    /// Clear lists, spawn new lives and bricks, set score to 0, sets the "gamemode"
    /// </summary>
    private void StartArkanoid()
    {
        SpawnPlayerPaddle();
		SpawnNewLives(3);
		startGame.gameObject.SetActive(false);
        BrickSpawner.Instance.ClearBrickList();
        spawner.SpawnBricks();
        score = 0;
        UpdateScore();
        classics = classicBounce.isOn;
	}
    public void AddScore(int scoreplus = 1)
    {
        score += scoreplus;
        UpdateScore();
	}
    private void UpdateScore()
    {
		scoreText.text = "Score: " + score;
	}
    /// <summary>
    /// Destroys player Object and show button for a new game
    /// </summary>
    public void EndRound()
    {
		Destroy(PlayerController.Instance.gameObject);
		startGame.gameObject.SetActive(true);
	}
}
