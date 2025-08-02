using UnityEngine;
public class PauseManager : MonoBehaviour
{
    private bool IsPaused = false;
    [SerializeField] private GameObject PauseUI;
    private void Start()
    {
        if(PauseUI == null) { throw new System.Exception($"{nameof(PauseUI)} was null in {nameof(PauseManager)}, please assign it."); }
    }
    private void TogglePause()
    {
        if (IsPaused)
        {
            GameManager.UnpauseGame();
            PauseUI.SetActive(false);
            IsPaused = false;
            return;
        }
        GameManager.PauseGame();
        PauseUI.SetActive(true);
        IsPaused = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}