using UnityEngine;

    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject menuPanel;

        public void OpenMenu()
        {
            menuPanel.SetActive(true);
            Time.timeScale = 0f; // dừng tất cả physics, animation và Update có Time.deltaTime
        }

        public void CloseMenu()
        {
            menuPanel.SetActive(false);
            Time.timeScale = 1f; // trở lại tốc độ bình thường
        }
    }

