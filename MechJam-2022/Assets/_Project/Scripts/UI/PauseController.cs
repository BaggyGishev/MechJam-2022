using System;
using Gisha.Effects.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gisha.MechJam.UI
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private GameObject popup;

        public static bool IsPaused { private set; get; }


        private void Start()
        {
            Time.timeScale = 1f;
            IsPaused = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!IsPaused)
                    Pause();
                else
                    Unpause();

                AudioManager.Instance.PlaySFX("pause");
            }
        }

        public void OnClick_Resume()
        {
            Unpause();
        }

        public void OnClick_Restart()
        {
            SceneManager.LoadScene(0);
        }

        public void OnClick_Quit()
        {
            Application.Quit();
        }

        private void Pause()
        {
            Time.timeScale = 0f;
            popup.SetActive(true);

            IsPaused = true;
        }

        private void Unpause()
        {
            Time.timeScale = 1f;
            popup.SetActive(false);

            IsPaused = false;
        }
    }
}