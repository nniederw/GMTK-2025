using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private static List<Item> LastBossDefeatingLoadout = null;
    private static List<(Vector2 pos, Item item)> LastDeathItems = null;
    private static uint LastDeathDifficulty = uint.MaxValue;
    private static bool IsPaused = false;
    public static void ExitGame()
    {
        Application.Quit();
    }
    public static void LoadMainMenu()
    {
        UnpauseGame();
        for (int i = 0; i < SceneManager.sceneCount - 1; i++)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
        }
        SceneManager.LoadScene("Main Menu");
    }
    public static void RestartGame()
    {
        UnpauseGame();
        for (int i = 0; i < SceneManager.sceneCount - 1; i++)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
        }
        SceneManager.LoadScene("BaseScene");
    }
    public static void PauseGame()
    {
        Time.timeScale = 0f;
        IsPaused = true;
    }
    public static void UnpauseGame()
    {
        Time.timeScale = 1f;
        IsPaused = false;
    }
    public static bool BossHasBeenDefeatedBefore() => LastBossDefeatingLoadout != null;
    public static void SaveBossDefeatingLoadout(IEnumerable<Item> items) => LastBossDefeatingLoadout = items.ToList();
    public static List<Item> GetBossDefeatingLoadout() => LastBossDefeatingLoadout.ToList();
    public static bool HasLastDeathItems() => LastDeathItems != null;
    public static List<(Vector2 pos, Item item)> GetLastDeathItems() => LastDeathItems.ToList();
    public static uint GetLastDeathDifficulty() => LastDeathDifficulty;
    public static void SaveLastDeathItems(IEnumerable<(Vector2 pos, Item item)> items, uint lastDeathDifficulty)
    {
        LastDeathItems = items.ToList();
        LastDeathDifficulty = lastDeathDifficulty;
    }
}