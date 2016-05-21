using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GM : MonoBehaviour {

    public int lives = 3;
    public int powerUses = 3;

    public int bricks = 8;

    public string powerUsed = "";

    public float resetDelay = 1f;
    public float setGravityScale = 0;

    public Text livesText;
    public Text powersText;

    public GameObject gameOver;
    public GameObject youWon;
    public GameObject bricksPrefab;
    public GameObject slowZone;
    public GameObject ball;
    public GameObject startingZone;
    public GameObject collisionParticles;

    public static GM instance = null;

    private GameObject cloneBall;

	// Use this for initialization
	void Awake () {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            instance = this;
        }
        Setup();
	}

    void Setup() {
        var startingZonePosition = startingZone.GetComponent<Transform>();
        Instantiate(startingZone, startingZonePosition.position, Quaternion.identity);

        var bricksPosition = bricksPrefab.GetComponent<Transform>();
        Instantiate(bricksPrefab, bricksPosition.position, Quaternion.identity);

        var slowZonePosition = slowZone.GetComponent<Transform>();
        Instantiate(slowZone, slowZonePosition.position, Quaternion.identity);

        cloneBall = Instantiate(ball, transform.position, Quaternion.identity) as GameObject;
        ball ballStats = cloneBall.GetComponent<ball>();
        ballStats.setGravityScale = setGravityScale;
        ballStats.powerUsed = powerUsed;
    }

    void CheckGameOver() {
        if (bricks < 1)
        {
            youWon.SetActive(true);
            Time.timeScale = .25f;
            Invoke("NextLevel", resetDelay);
        }
        if (lives < 1)
        {
            gameOver.SetActive(true);
            Time.timeScale = .25f;
            Invoke("Reset", resetDelay);
        }
    }

    void Reset()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("level2");
    }

    public void LoseLife()
    {
        lives--;
        livesText.text = "Lives: " + lives;
        var ballLocation = cloneBall.GetComponent<Transform>();
        Instantiate(collisionParticles, ballLocation.position, Quaternion.identity);
        Destroy(cloneBall);
        Invoke("SetupBall", resetDelay);
        CheckGameOver();
    }

    void SetupBall()
    {
        cloneBall = Instantiate(ball, transform.position, Quaternion.identity) as GameObject;
        ball ballStats = cloneBall.GetComponent<ball>();
        ballStats.setGravityScale = setGravityScale;
    }

    public void DestroyBrick()
    {
        bricks--;
        CheckGameOver();
    }

    public void PowerDecrease()
    {
        powerUses--;
        powersText.text = powerUsed + ": " + powerUses;
    }
}
