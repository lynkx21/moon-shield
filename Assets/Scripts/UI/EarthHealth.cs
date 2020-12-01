using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Core.UI
{
    public class EarthHealth : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private int health;
        [SerializeField] private Earth earth;

        private void Start()
        {
            health = earth.hitPoints;
            earth.OnTakeDamage += UpdateHealth;
            HealthToText();
        }

        public void UpdateHealth(int value)
        {
            health = value;
            HealthToText();
        }

        private void HealthToText()
        {
            string text = "";
            string healthString = health.ToString().PadLeft(2, '0');
            char[] letters = healthString.ToCharArray();
            foreach (char letter in letters)
            {
                text += GetNumberFont(letter);
            }
            healthText.text = text;
        }

        private string GetNumberFont(char num)
        {
            return $"<sprite=\"numbers\" index={num}>";
        }

        private void OnDestroy()
        {
            earth.OnTakeDamage -= UpdateHealth;
        }
    }
}