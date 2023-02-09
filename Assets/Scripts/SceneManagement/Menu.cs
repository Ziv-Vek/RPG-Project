using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

namespace RPG.SceneManagement
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] Canvas pauseMenuCanvas = null;

        private void Start() {
            if (pauseMenuCanvas != null)
            {
                pauseMenuCanvas.enabled = false;
            }
        }

        private void Update() {
            if (SceneManager.GetActiveScene().buildIndex < 1) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (pauseMenuCanvas.enabled == false)
                {
                    ShowPauseMenu();
                }
                else Continue();
            }
        }

        private void ShowPauseMenu()
        {
            Time.timeScale = 0;
            pauseMenuCanvas.enabled = true;
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void PlayGame()
        {
            FindObjectOfType<AudioSource>().Stop();
            SceneManager.LoadScene(1);
        }

        public void ReturnToMainMenu()
        {
            Continue();
            SceneManager.LoadScene(0);
        }

        public void Continue()
        {
            Time.timeScale = 1;
            pauseMenuCanvas.enabled = false;
        }
    }
}

