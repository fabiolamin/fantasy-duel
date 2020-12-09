using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CardStorage : MonoBehaviour
{
    private List<Card> tempCollection = new List<Card>();
    private string[] cardsDataFileNames = new string[] { "Bases", "Creatures", "Magics" };
    [SerializeField] private string folderName;
    public List<Card> Collection { get; set; } = new List<Card>();

    public void SaveCardsAsList()
    {
        foreach (string fileName in cardsDataFileNames)
        {
            string path = Application.streamingAssetsPath + "/Data/" + folderName + "/" + fileName + ".txt";

            using (StreamReader streamReader = new StreamReader(path))
            {
                string json = Crypto.Decrypt(streamReader.ReadToEnd());
                CardArray array = JsonUtility.FromJson<CardArray>(json);

                if (array != null)
                {
                    foreach (Card card in array.Card)
                    {
                        Collection.Add(card);
                    }
                }
            }
        }
    }

    public void SaveCardsAsFiles()
    {
        foreach (string fileName in cardsDataFileNames)
        {
            string path = Application.streamingAssetsPath + "/Data/" + folderName + "/" + fileName + ".txt";
            File.WriteAllText(path, String.Empty);
            TextWriter tw = new StreamWriter(path, true);
            Card[] cards = Collection.FindAll(card => card.Type == fileName).OrderBy(c => c.Id).ToArray();
            string encryptedFile = Crypto.Encrypt(Utility.GetJsonFrom(cards));
            tw.WriteLine(encryptedFile);
            tw.Close();
        }
    }

    public Card[] GetCustomDeck()
    {
        AddCardsInTemporaryCollection();
        List<Card> customDeck = new List<Card>();
        int collectionLength = tempCollection.Count;

        while (collectionLength > 0)
        {
            int index = UnityEngine.Random.Range(0, collectionLength);
            Card card = tempCollection[index];
            customDeck.Add(card);
            tempCollection.RemoveAt(index);
            collectionLength--;
        }

        return customDeck.ToArray();
    }

    private void AddCardsInTemporaryCollection()
    {
        foreach (Card card in Collection)
        {
            tempCollection.Add(card);
        }
    }
}
