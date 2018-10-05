using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinSceneScript : MonoBehaviour {

    public GameObject highScoreTxtObj;
    public GameObject newHighScoreTxtObj;

    public int highScore;
    public int oldHighScore;

    public Color bgColor;
    float timeLeft;
    Color targetColor;

    // Use this for initialization
    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        oldHighScore = PlayerPrefs.GetInt("OldHighScore");
        if (highScore > oldHighScore)
        {
            newHighScoreTxtObj.SetActive(true);
        }
        else
        {
            newHighScoreTxtObj.SetActive(false);
        }
        highScoreTxtObj.GetComponent<Text>().text = "High Score: " + highScore + " Waves";
    }

    void Update()
    {
        if (timeLeft <= Time.deltaTime)
        {
            // transition complete
            // assign the target color
            Camera.main.backgroundColor = targetColor;
            // start a new transition
            targetColor = new Color(Random.value, Random.value, Random.value);
            timeLeft = 4.0f;
        }
        else
        {
            // transition in progress
            // calculate interpolated color
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, targetColor, Time.deltaTime / timeLeft);

            // update the timer
            timeLeft -= Time.deltaTime;
        }
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
