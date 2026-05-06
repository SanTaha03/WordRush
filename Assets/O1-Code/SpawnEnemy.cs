using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [Header("Enemy Prefab")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Path for Enemies")]
    [SerializeField] private Transform[] enemyPathWaypoints;
    
    [Header("Spawn Parameters")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxEnemiesAlive = 3;
    [SerializeField] private bool spawnOnStart = true;
    
    [Header("Liste de mots")]
    [SerializeField, TextArea(5,20)] private string availableWordsText;

    private string[] availableWords;

    private readonly List<GameObject> activeEnemies  = new List<GameObject>();
    private int enemyCounter = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        LoadWordsFromText();
        Debug.Log($"SpawnEnemy Start sur {gameObject.name} | enemyPrefab = {(enemyPrefab != null ? enemyPrefab.name : "NULL")}");
        
        if (spawnOnStart)
        {
            InvokeRepeating(nameof(TrySpawnEnemy), 0f, spawnInterval);
        }
    }

    private void LoadWordsFromText()
    {
        availableWords = availableWordsText
            .Split(new[] { '\n', '\r', ',', ';', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < availableWords.Length; i++)
        {
            availableWords[i] = availableWords[i].ToLower().Trim();
        }
    }

    private void TrySpawnEnemy()
    {
        CleanupDestroyedEnemies();

        if(enemyPrefab == null)
        {
            Debug.LogWarning("SpawnEnemy : aucun enemyPrefab assigné.");
            return;
        }

        if(spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("SpawnEnemy : aucun point de spawn assigné.");
            return;
        }
        
        if(enemyPathWaypoints == null || enemyPathWaypoints.Length == 0)
        {
            Debug.LogWarning("SpawnEnemy : aucun chemin assigné pour les ennemis.");
            return;
        }

        if(availableWords == null || availableWords.Length == 0)
        {
            Debug.LogWarning("SpawnEnemy : aucune liste de mots assignée.");
            return;
        }

        if (activeEnemies.Count >= maxEnemiesAlive)
        {
            return;
        }

        SpawnOneEnemy();
    }

    private void SpawnOneEnemy()
    {
        Transform selectedspawnPoints = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        
        GameObject enemyInstance = Instantiate(
            enemyPrefab,
            selectedspawnPoints.position,
            selectedspawnPoints.rotation
        );

        enemyCounter++;

        Enemy enemy = enemyInstance.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.enemyId = "enemy_" + enemyCounter;
            string randomWord = GetRandomAvailableWord();
            enemy.SetWord(randomWord);
        }
        else
        {
            Debug.LogWarning($"SpawnEnemy : le prefab {enemyPrefab.name} n'a pas de composant Enemy.");
        }

        EnemyPathFollow pathFollow = enemyInstance.GetComponent<EnemyPathFollow>();
        if (pathFollow != null)
        {
            pathFollow.SetWaypoints(enemyPathWaypoints);
        }
        else
        {
            Debug.LogWarning($"SpawnEnemy : le prefab {enemyPrefab.name} n'a pas de composant EnemyPathFollow.");
        }

        activeEnemies.Add(enemyInstance);

    }

    private void CleanupDestroyedEnemies()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
    }

    public List<GameObject> GetActiveEnemies()
    {
        CleanupDestroyedEnemies();
        return activeEnemies;
    }
    public void StartSpawning()
    {
        CancelInvoke(nameof(TrySpawnEnemy));
        InvokeRepeating(nameof(TrySpawnEnemy), 0f, spawnInterval);
    }

    public void StopSpawning()
    {
        CancelInvoke(nameof(TrySpawnEnemy));
    }

    public int GetAliveEnemiesCount()
    {
        CleanupDestroyedEnemies();
        return activeEnemies.Count;
    }
    
    public string GetRandomAvailableWord(string excludedWord = "")
    {
        if (availableWords == null || availableWords.Length == 0)
        {
            Debug.LogWarning("SpawnEnemy : aucun mot disponible.");
            return "";
        }

        CleanupDestroyedEnemies();

        List<string> usedWords = new List<string>();

        foreach (GameObject enemyObject in activeEnemies)
        {
            if (enemyObject == null)
            {
                continue;
            }

            Enemy enemy = enemyObject.GetComponent<Enemy>();

            if (enemy == null)
            {
                continue;
            }

            string enemyWord = enemy.GetWord();

            if (!string.IsNullOrEmpty(enemyWord))
            {
                usedWords.Add(enemyWord.ToLower().Trim());
            }
        }

        string cleanedExcludedWord = excludedWord.ToLower().Trim();

        List<string> validWords = new List<string>();

        foreach (string word in availableWords)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                continue;
            }

            string cleanedWord = word.ToLower().Trim();

            if (cleanedWord == cleanedExcludedWord)
            {
                continue;
            }

            if (usedWords.Contains(cleanedWord))
            {
                continue;
            }

            if (validWords.Contains(cleanedWord))
            {
                continue;
            }

            validWords.Add(cleanedWord);
        }

        if (validWords.Count == 0)
        {
            Debug.LogWarning("SpawnEnemy : plus aucun mot disponible sans doublon. Utilisation fallback.");

            foreach (string word in availableWords)
            {
                if (!string.IsNullOrWhiteSpace(word))
                {
                    return word.ToLower().Trim();
                }
            }

            return "";
        }

        return validWords[Random.Range(0, validWords.Count)];
    }
}
