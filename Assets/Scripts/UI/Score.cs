using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Core;

namespace Core.UI
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private int score;
        [SerializeField] private List<Enemy> enemies;
        [SerializeField] private Earth earth;
        private bool canRun = true;

        private void Start()
        {
            score = 0;
            ScoreToText();
            enemies = new List<Enemy>();
            earth.OnGameOver += BlockScoring;
        }

        private void Update()
        {
            if (canRun)
            {
                Enemy[] enemiesFound = FindObjectsOfType<Enemy>();
                foreach (Enemy enemy in enemiesFound)
                {
                    if (!enemies.Contains(enemy))
                    {
                        enemies.Add(enemy);
                        enemy.OnDeath += AddScore;
                    }
                }

                List<Enemy> enemiesToRemove = new List<Enemy>();
                foreach (Enemy enemy in enemies)
                {
                    if (enemy == null)
                    {
                        enemiesToRemove.Add(enemy);
                    }
                }
                foreach (Enemy enemy in enemiesToRemove)
                {
                    enemy.OnDeath -= AddScore;
                    enemies.Remove(enemy);
                }
            }
        }
               

        public void AddScore(int adder)
        {
            score += adder;
            ScoreToText();
        }

        private void ScoreToText()
        {
            string text = "";
            string scoreString = score.ToString().PadLeft(7, '0');
            char[] letters = scoreString.ToCharArray();
            foreach (char letter in letters)
            {
                text += GetNumberFont(letter);
            }
            scoreText.text = text;
        }

        private string GetNumberFont(char num)
        {
            return $"<sprite=\"numbers\" index={num}>";
        }

        private void BlockScoring()
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.OnDeath -= AddScore;
            }

            canRun = false;
        }

        private void OnDestroy()
        {
            earth.OnGameOver -= BlockScoring;

            foreach (Enemy enemy in enemies)
            {
                enemy.OnDeath -= AddScore;
            }
        }
    }
}