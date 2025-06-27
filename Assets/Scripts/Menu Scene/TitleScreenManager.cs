using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager Instance;

    [Header("Menus")]
    [SerializeField] GameObject titleScreenMainMenu;
    [SerializeField] GameObject titleScreenLoadMenu;

    [Header("Buttons")]
    [SerializeField] Button mainMenuNewGameButton;
    [SerializeField] Button loadMenuReturnButton;
    [SerializeField] Button mainMenuLoadGameButton;
    [SerializeField] Button deleteCharacterPopUpConfirmButton;

    [Header("Pop Ups")]
    [SerializeField] GameObject noCharacterSlotsPopUp;
    [SerializeField] Button noCharacterSlotsOkayButton;
    [SerializeField] GameObject deleteCharacterSlotPopUp;

    [Header("Character Slots")]
    public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame()
    {
        WorldSaveGameManager.instance.AttemptToCreateNewGame();
    }

    public void OpenLoadGameMenu()
    {
        titleScreenMainMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);

        // Select the return button first
        loadMenuReturnButton.Select();
    }

    public void CloseLoadGameMenu()
    {
        titleScreenLoadMenu.SetActive(false);
        titleScreenMainMenu.SetActive(true);

        // Select the return button first
        mainMenuLoadGameButton.Select();
    }

    public void DisplayNoFreeCharacterSlotsPopup()
    {
        noCharacterSlotsPopUp.SetActive(true);
        noCharacterSlotsOkayButton.Select();
    }

    public void CloseNocharacterSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(false);
        mainMenuNewGameButton.Select();
    }

    public void SelectCharacterSlot(CharacterSlot characterSlot)
    {
        currentSelectedSlot = characterSlot;
    }

    public void SelectNoSlot()
    {
        currentSelectedSlot = CharacterSlot.NO_SLOT;
    }

    public void AttemptToDeleteCharacterSlot()
    {
        if (currentSelectedSlot != CharacterSlot.NO_SLOT)
        {
            deleteCharacterSlotPopUp.SetActive(true);
            deleteCharacterPopUpConfirmButton.Select();
        }
    }

    public void DeleteCharacterSlot()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        WorldSaveGameManager.instance.DeleteGame(currentSelectedSlot);

        // We disable and then enable the load menu, to refresh the slots
        titleScreenLoadMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);
        loadMenuReturnButton.Select();
    }

    public void CloseDeleteCharacterSlotPopUp()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        loadMenuReturnButton.Select();
    }
}
