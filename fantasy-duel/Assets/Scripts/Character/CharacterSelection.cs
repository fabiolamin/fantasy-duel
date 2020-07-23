using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class CharacterSelection : MonoBehaviour
{
    private int index = 0;
    private CharacterInfo characterInfo;
    private List<GameObject> orderedCharacters;
    [SerializeField] private GameObject[] characters;
    [SerializeField] private Text winsToUnlockCharacter;
    [SerializeField] private GameObject selectButton;
    [SerializeField] private GameObject selected;
    [SerializeField] private Text playerWins;

    private void Awake()
    {
        orderedCharacters = characters.OrderBy(ch => ch.GetComponent<CharacterInfo>().WinsToUnlock).ToList();
        HideCharacters();
        selected.SetActive(false);
        playerWins.text = PlayerPrefs.GetInt("Wins").ToString();
    }

    private void Start()
    {
        ChangeCharacter(0);
    }

    private void HideCharacters()
    {
        orderedCharacters.ForEach(ch => ch.SetActive(false));
    }

    public void ChangeCharacter(int direction)
    {
        index += direction;
        index = Mathf.Clamp(index, 0, orderedCharacters.Count - 1);
        HideCharacters();
        orderedCharacters[index].SetActive(true);
        characterInfo = orderedCharacters[index].GetComponent<CharacterInfo>();
        ShowCharacterInfo();
    }

    private void ShowCharacterInfo()
    {
        winsToUnlockCharacter.text = "Wins to unlock: " + characterInfo.WinsToUnlock.ToString();
        selectButton.SetActive(characterInfo.IsUnlocked);
        selected.SetActive(characterInfo.Id == PlayerPrefs.GetInt("Character"));
    }

    public void SelectCharacter()
    {
        PlayerPrefs.SetInt("Character", characterInfo.Id);
        selected.SetActive(true);
    }
}
