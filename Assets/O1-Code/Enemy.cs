using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Infos ennemi")]
    public string enemyId;
    public string word;

    [Header("Vie")]
    public int maxHealth = 50;
    public int currentHealth = 50;

    [Header("Score")]
    [SerializeField] private int scoreValue = 10;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI wordText;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Death")]
    [SerializeField] private float destroyDelayAfterDeath = 1.5f;

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateWordDisplay();
    }

    public void SetWord(string newWord)
    {
        word = newWord.ToLower().Trim();
        UpdateWordDisplay();
    }

    public string GetWord()
    {
        return word;
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= amount;
        Debug.Log($"{enemyId} prend {amount} dégâts. PV restants : {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        Collider enemyCollider = GetComponent<Collider>();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        EnemyPathFollow pathFollow = GetComponent<EnemyPathFollow>();
        if (pathFollow != null)
        {
            pathFollow.enabled = false;
        }

        Destroy(gameObject, destroyDelayAfterDeath);
    }

    private void UpdateWordDisplay()
    {
        if (wordText != null)
        {
            wordText.text = word;
        }
    }
}