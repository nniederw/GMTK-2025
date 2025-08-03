using UnityEngine;
public class PlayerWinPopup : MonoBehaviour
{
    [SerializeField] private GameObject YouWinUI;
    private static PlayerWinPopup Instance;
    private void Start()
    {
        if (YouWinUI == null) { throw new System.Exception($"{nameof(YouWinUI)} was null on {nameof(PlayerWinPopup)}, please assign it."); }
        YouWinUI.SetActive(false);
        Instance = this;
    }
    public static void DisplayWinPopup()
    {
        Instance.YouWinUI.SetActive(true);
    }
}
