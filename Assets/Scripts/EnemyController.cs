using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    Transform nextNode;
    public float speed = 5.0f;
    public float precision = 0.1f;
    public float maxHealth = 100f;
    public Slider healthBar;
    public int money = 20;
    public int score = 10;
    public GameObject deathEffect;
    public AudioClip deathSound;

    AudioSource sound;
    public float currentHealth;
    int nodenum = 0;
    public float originalSpeed;

    void Awake()
    {
        nextNode = Nodes.nodes[nodenum];
        currentHealth = maxHealth;
        healthBar.value = maxHealth;
        healthBar.maxValue = maxHealth;
        originalSpeed = speed;
    }

    void Update()
    {
        // Move enemy towards the direction of next node
        Vector2 direction = nextNode.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        // Node reached
        if (Vector2.Distance(transform.position, nextNode.position) <= precision)
        {
            nodenum++;

            if (nodenum >= Nodes.nodes.Length)
            {
                if (gameObject != null)
                {
                    Destroy(gameObject);
                    SceneHandler.instance.DecreaseLives();
                }
                return;
            }
            nextNode = Nodes.nodes[nodenum];
        }
        else if (currentHealth <= 0)
        {
            GameObject audioHolder = GameObject.Find("AudioHolder");
            sound = audioHolder.GetComponent<AudioSource>();
            sound.PlayOneShot(deathSound);

            Destroy(gameObject);
            GameObject effect = Instantiate(deathEffect, transform.position, transform.rotation);
            Destroy(effect, 1.0f);
            Builder.instance.SetMoney(money);
            SceneHandler.instance.SetScore(score);
        }
    }

    public void DamageEnemy(float damage)
    {
        currentHealth -= damage;

        healthBar.value = currentHealth;
    }

    public void SlowEnemy(float slow, float duration)
    {
        speed *= slow;
        speed = Mathf.Clamp(speed, originalSpeed / 2, originalSpeed);
        Invoke("ResetSpeed", duration);
    }

    private void ResetSpeed()
    {
        speed = originalSpeed;
    }
}
