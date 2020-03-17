using System.IO;
using UnityEngine;

public class CardBuilding : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Vector3 position;
    private GameObject newCard;
    private string cardDataPath;


    private void Awake()
    {
        cardDataPath = Application.dataPath + "/Data/CardData.json";

        using (StreamReader streamReader = new StreamReader(cardDataPath))
        {
            string cardDataJsonFile = streamReader.ReadToEnd();
            CardCollection list = JsonUtility.FromJson<CardCollection>(cardDataJsonFile);

            for (int index = 0; index < list.Cards.Length; index++)
            {
                Cards cards = list.Cards[index];
                newCard = Instantiate(cardPrefab, position, Quaternion.identity);
                newCard.GetComponent<CardHUD>().Set(cards);
            }
        }
    }
}
