using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardBuilding : MonoBehaviour
{
    private string[] cardsDataFileNames;
    [SerializeField] private CardCollection cardCollection;
    private void Awake()
    {
        cardsDataFileNames = new string[] { "Bases", "Creatures", "Magics" };
        SerializeCard();
    }

    private void SerializeCard()
    {
        foreach (string fileName in cardsDataFileNames)
        {
            string path = Application.streamingAssetsPath + "/Data/" + fileName + ".json";

            using (StreamReader streamReader = new StreamReader(path))
            {
                string jsonFile = streamReader.ReadToEnd();
                CardArray array = JsonUtility.FromJson<CardArray>(jsonFile);

                for (int index = 0; index < array.Card.Length; index++)
                {
                    Card card = array.Card[index];
                    cardCollection.Add(card);
                }
            }
        }
    }
}
