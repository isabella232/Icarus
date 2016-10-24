using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GM : MonoBehaviour {

    public int lives = 3;
    public int powerUses = 0;

    public int bricks = 8;
    public int bricksBroken;

    public string powerUsed = "";
    public GameObject levelPower;

    public float resetDelay = 1f;
    public float setGravityScale = 0;
    public float timeScale = 1;

    public Text livesText;
    public Text powersText;
    public Text bricksText;

    public GameObject gameOver;
    public GameObject youWon;
    public GameObject bricksPrefab;
    public GameObject slowZone;
    public GameObject ballReference;
    public GameObject startingZone;
    public GameObject collisionParticles;
    public GameObject loadingImage;

    public static GM instance = null;

    private GameObject cloneBall;

    public bool destroyOn = false;

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
        Time.timeScale = timeScale;
    }

    void Setup() {
        var startingZonePosition = startingZone.GetComponent<Transform>();
        Instantiate(startingZone, startingZonePosition.position, Quaternion.identity);

        var bricksPosition = bricksPrefab.GetComponent<Transform>();
        Instantiate(bricksPrefab, bricksPosition.position, Quaternion.identity);

        var slowZonePosition = slowZone.GetComponent<Transform>();
        Instantiate(slowZone, slowZonePosition.position, Quaternion.identity);

        SetupBall();
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
        loadingImage.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void NextLevel()
    {
        Time.timeScale = 1f;
        loadingImage.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
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
        cloneBall = Instantiate(ballReference, transform.position, Quaternion.identity) as GameObject;
        ball.setGravityScale = setGravityScale;
    }

    public void DestroyBrick()
    {
        bricks--;

        bricksBroken++;
        if (bricksBroken == 5)
        {
            bricksBroken = 0;
            PowerIncrease();
        }
        bricksText.text = "Bricks Left: " + bricks;

        CheckGameOver();
    }

    public void PowerDecrease()
    {
        //Debug.Log("decreasing power");
        powerUses--;
        powersText.text = powerUsed + ": " + powerUses;
    }

    public void PowerIncrease()
    {
        powerUses++;
        powersText.text = powerUsed + ": " + powerUses;
    }
}
