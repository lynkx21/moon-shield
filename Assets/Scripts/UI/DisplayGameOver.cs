using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class DisplayGameOver : MonoBehaviour
    {
        [SerializeField] GameObject gameOverScreen;
        [SerializeField] Earth earth;

        private void Start()
        {
            earth.OnGameOver += ShowGameOverScreen;
        }

        private void ShowGameOverScreen()
        {
            gameOverScreen.SetActive(true);
        }

        private void OnDestroy()
        {
            earth.OnGameOver -= ShowGameOverScreen;
        }
    }
}