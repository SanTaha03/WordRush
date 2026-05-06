using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WordInputManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI typedWordText;

    [Header("Références")]
    [SerializeField] private SpawnEnemy spawnEnemy;
    [SerializeField] private PlayerShooter playerShooter;

    [Header("Saisie")]
    [SerializeField] private int maxCharacters = 20;

    private string currentInput = "";
    private bool inputEnabled = true;

    private void OnEnable()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput += HandleTextInput;
        }
    }

    private void OnDisable()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput -= HandleTextInput;
        }
    }

    private void Update()
    {
        if (!inputEnabled)
        {
            return;
        }

        HandleSpecialKeys();
        UpdateInputDisplay();
    }

    private void HandleTextInput(char character)
    {
        if (!inputEnabled)
        {
            return;
        }

        if (char.IsLetterOrDigit(character))
        {
            if (currentInput.Length < maxCharacters)
            {
                currentInput += char.ToLower(character);
            }
        }
    }

    private void HandleSpecialKeys()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            if (currentInput.Length > 0)
            {
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
            }
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame)
        {
            ValidateCurrentWord();
        }
    }

    private void ValidateCurrentWord()
    {
        string typedWord = currentInput.ToLower().Trim();

        if (string.IsNullOrEmpty(typedWord))
        {
            return;
        }

        if (spawnEnemy == null)
        {
            Debug.LogWarning("WordInputManager : aucune référence vers SpawnEnemy.");
            return;
        }

        List<GameObject> activeEnemies = spawnEnemy.GetActiveEnemies();

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

            if (enemy.GetWord() == typedWord)
            {
                Debug.Log("Ennemi trouvé : " + enemy.enemyId + " | mot = " + enemy.GetWord());

                bool shotStarted = false;
                if (playerShooter != null)
                {
                    shotStarted = playerShooter.ShootAtEnemy(enemy);
                }
                else
                {
                    Debug.LogWarning("WordInputManager : aucune référence vers PlayerShooter.");
                }

                if (shotStarted)
                {
                    string newWord = spawnEnemy.GetRandomAvailableWord(enemy.GetWord());
                    enemy.SetWord(newWord);
                    ClearInput();
                }

                return;
            }
        }

        Debug.Log("Aucun ennemi trouvé pour le mot : " + typedWord);
    }

    private void UpdateInputDisplay()
    {
        if (typedWordText != null)
        {
            typedWordText.text = currentInput;
        }
    }

    public void ClearInput()
    {
        currentInput = "";
        UpdateInputDisplay();
    }

    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;

        if (!inputEnabled)
        {
            ClearInput();
        }
    }
}