using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;
    public float waveDelay = 20.0f;
    public Transform spawnPoint;
    public Text waveTimerText;
    public Text waveNumberText;
    public AudioClip bossGrowl;

    float waveTimer;
    float waveSize = 0.5f;
    int waveNumber = 0;
    bool once = true;
    AudioSource sound;

    void Start()
    {
        waveTimer = 10.0f;
        sound = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        if (SceneHandler.gameOver)
        {
            enabled = false;
            return;
        }

        waveTimer -= Time.deltaTime;

        if (once)
        {
            waveTimerText.text = Mathf.Round(waveTimer).ToString();
        }

        if (waveTimer <= 0)
        {
            once = false;
            waveTimerText.enabled = false;

            StartCoroutine(SpawnWave());
            waveTimer = waveDelay;

            Stats.instance.waves = waveNumber;
            waveNumberText.text = "Wave: " + waveNumber;
        }
    }

    IEnumerator SpawnWave()
    {
        waveNumber++;
        waveSize = waveSize + 0.5f;

        // Spawn boss monster every 5th wave
        if (waveNumber % 5 == 0)
        {
            GameObject bossEnemy = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);

            bossEnemy.transform.localScale += new Vector3(2f, 2f, 0f);

            EnemyController enemyController = bossEnemy.GetComponent<EnemyController>();

            enemyController.maxHealth = Mathf.Log(Mathf.Pow(1.7f, waveNumber)) * enemyController.maxHealth;
            enemyController.currentHealth = Mathf.Log(Mathf.Pow(1.7f, waveNumber)) * enemyController.currentHealth;
            enemyController.speed *= 0.7f;
            enemyController.originalSpeed *= 0.7f;
            enemyController.healthBar.maxValue = enemyController.maxHealth;
            enemyController.healthBar.value = enemyController.currentHealth;

            sound.PlayOneShot(bossGrowl);

            yield return new WaitForSeconds(1f);
        }

        // Spawn normal enemies
        for (int i = 0; i < Mathf.RoundToInt(waveSize); i++)
        {
            GameObject enemyObject = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
            EnemyController enemyController = enemyObject.GetComponent<EnemyController>();

            enemyController.maxHealth = Mathf.Pow(1.03f, waveNumber) * enemyController.maxHealth;
            enemyController.currentHealth = Mathf.Pow(1.03f, waveNumber) * enemyController.currentHealth;
            enemyController.healthBar.maxValue = enemyController.maxHealth;
            enemyController.healthBar.value = enemyController.currentHealth;

            yield return new WaitForSeconds((waveDelay - 5f) / Mathf.RoundToInt(waveSize));
        }
    }
}
