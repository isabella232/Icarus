using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GM : MonoBehaviour {

    public int lives = 3;
    public int bricks = 8;

    public float resetDelay = 1f;

    public Text livesText;

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

        var slowZonePosition = transform.position;
        slowZonePosition.x -= .5f;
        slowZonePosition.y += .25f;
        Instantiate(slowZone, slowZonePosition, Quaternion.identity);

        cloneBall = Instantiate(ball, transform.position, Quaternion.identity) as GameObject;
    }

    void CheckGameOver() {
        if (bricks < 1)
        {
            youWon.SetActive(true);
            Time.timeScale = .25f;
            Invoke("Reset", resetDelay);
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
    }

    public void DestroyBrick()
    {
        bricks--;
        CheckGameOver();
    }
}
