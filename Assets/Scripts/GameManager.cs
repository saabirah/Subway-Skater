using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int COIN_SCORE_AMOUNT = 5;
    public static GameManager Instance { set; get; }

    public bool IsDead { set; get; }
    private bool isGameStarted = false;



    // Ui and Ui fields
    public Animator gameCanvas, MenuAnim, DiamondAnim;
    public TextMeshProUGUI scoreText, coinText, modifierText, hiScoreText; // pour importer automatiquement un namespace ctrl +.
    private PlayerMotor motor;
    private float score, coinScore, modifierScore;
    private int lastScore;
   


    //Death menu
    public Animator deathMenuAnim;
    public TextMeshProUGUI deadScoreText, deadCoinText;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        modifierScore = 1;
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();

        modifierText.text = "x" + modifierScore.ToString("0.0");
        coinText.text = coinScore.ToString("0");
        scoreText.text = score.ToString("0");
        hiScoreText.text = PlayerPrefs.GetInt("Hiscore").ToString();
        // UpdateScores();  

    }

    // Update is called once per frame
    public void Update()
    {
        if(MobileImput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.StartRunning();
            FindObjectOfType<GlacierSpawner>().IsScrolling = true;
            FindObjectOfType<CameraMotor>().IsMoving = true;
            gameCanvas.SetTrigger("Show");
            MenuAnim.SetTrigger("Hide");
        }
       
        if (isGameStarted && !IsDead)
        {
            //Bump the score up
            lastScore = (int) score;
            score += (Time.deltaTime * modifierScore);
           

            if(lastScore == (int)score)
            {
                Debug.Log("heloolkjg");
                scoreText.text = score.ToString("0");
            }
        }
        
    }


    public void GetCoin()
    {
        DiamondAnim.SetTrigger("Collect");
                coinScore++;
        coinText.text = coinScore.ToString();

        score += COIN_SCORE_AMOUNT;
        scoreText.text = score.ToString("0");
    }

    /*
    public void UpdateScores()
    {
        scoreText.text = score.ToString();
        coinText.text = coinScore.ToString();
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }*/

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        //UpdateScores();
        modifierText.text = "x" + modifierScore.ToString("0.0");

    }

    public void OnPlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void OnDeath()
    {
        FindObjectOfType<GlacierSpawner>().IsScrolling = false;
        IsDead = true;
        deadScoreText.text = score.ToString("0");
        deadCoinText.text = coinScore.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        gameCanvas.SetTrigger("Hide");

        // Check if this is a highscore
        if(score > PlayerPrefs.GetInt("Hiscore"))
        {
            float s = score;

            if(s%1== 0)
            {
                s += 1;
                PlayerPrefs.SetInt("Hiscore", (int)s);
            }
           
        }
        
        
    }
}
