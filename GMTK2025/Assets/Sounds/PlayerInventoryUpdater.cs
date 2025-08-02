using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(PlayerValues))]
public class PlayerInventoryUpdater : MonoBehaviour
{
    private PlayerValues PlayerValues;
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private Image Slot1UI;
    [SerializeField] private Image Slot2UI;
    [SerializeField] private Image Slot3UI;
    [SerializeField] private Image Slot4UI;
    [SerializeField] private TMP_Text Slot1UIText;
    [SerializeField] private TMP_Text Slot2UIText;
    [SerializeField] private TMP_Text Slot3UIText;
    [SerializeField] private TMP_Text Slot4UIText;
    [SerializeField] private Sprite DefaultItemSprite;
    private void Awake()
    {
        PlayerValues = GetComponent<PlayerValues>();
        PlayerValues.SubscribeToOnPotentialInventoryChange(OnPotentialInventoryChange);
    }
    private void Start()
    {
        if (InventoryUI == null) throw new Exception($"{nameof(InventoryUI)} was null in {nameof(PlayerInventoryUpdater)}, please assing it.");
        if (Slot1UI == null) throw new Exception($"{nameof(Slot1UI)} was null in {nameof(PlayerInventoryUpdater)}, please assing it.");
        if (Slot2UI == null) throw new Exception($"{nameof(Slot2UI)} was null in {nameof(PlayerInventoryUpdater)}, please assing it.");
        if (Slot3UI == null) throw new Exception($"{nameof(Slot3UI)} was null in {nameof(PlayerInventoryUpdater)}, please assing it.");
        if (Slot4UI == null) throw new Exception($"{nameof(Slot4UI)} was null in {nameof(PlayerInventoryUpdater)}, please assing it.");
        if (Slot1UIText == null) throw new Exception($"{nameof(Slot1UIText)} was null in {nameof(PlayerInventoryUpdater)}, please assing it.");
        if (Slot2UIText == null) throw new Exception($"{nameof(Slot2UIText)} was null in {nameof(PlayerInventoryUpdater)}, please assing it.");
        if (Slot3UIText == null) throw new Exception($"{nameof(Slot3UIText)} was null in {nameof(PlayerInventoryUpdater)}, please assing it.");
        if (Slot4UIText == null) throw new Exception($"{nameof(Slot4UIText)} was null in {nameof(PlayerInventoryUpdater)}, please assing it.");
        if (DefaultItemSprite == null) throw new Exception($"{nameof(DefaultItemSprite)} was null in {nameof(PlayerInventoryUpdater)}, please assing it.");
    }
    private void OnPotentialInventoryChange((Item Sword, Item Armor, Item Shield, Item Amulet) inventory)
    {
        Item sword = inventory.Sword;
        Item armor = inventory.Armor;
        Item shield = inventory.Shield;
        Item amulet = inventory.Amulet;
        Slot1UI.sprite = sword != null ? sword.Sprite : DefaultItemSprite;
        Slot2UI.sprite = armor != null ? armor.Sprite : DefaultItemSprite;
        Slot3UI.sprite = shield != null ? shield.Sprite : DefaultItemSprite;
        Slot4UI.sprite = amulet != null ? amulet.Sprite : DefaultItemSprite;
        Slot1UIText.text = sword != null ? sword.GetText() : "";
        Slot2UIText.text = armor != null ? armor.GetText() : "";
        Slot3UIText.text = shield != null ? shield.GetText() : "";
        Slot4UIText.text = amulet != null ? amulet.GetText() : "";
    }
    private void OpenInventory()
    {
        InventoryUI.SetActive(true);
    }
    private void CloseInventory()
    {
        InventoryUI.SetActive(false);
    }
    private void ToggleInventory()
    {
        if (InventoryUI.activeSelf)
        {
            CloseInventory();
            return;
        }
        OpenInventory();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }
}