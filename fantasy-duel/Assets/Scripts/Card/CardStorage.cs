﻿using System;
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
            string path = Application.streamingAssetsPath + "/Data/" + folderName + "/" + fileName + ".json";

            using (StreamReader streamReader = new StreamReader(path))
            {
                string jsonFile = streamReader.ReadToEnd();
                CardArray array = JsonUtility.FromJson<CardArray>(jsonFile);

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
            string path = Application.streamingAssetsPath + "/Data/" + folderName + "/" + fileName + ".json";
            File.WriteAllText(path, String.Empty);
            TextWriter tw = new StreamWriter(path, true);
            Card[] cards = Collection.FindAll(card => card.Type == fileName).OrderBy(c => c.Id).ToArray();
            tw.WriteLine(Utility.GetJsonFrom(cards));
            tw.Close();
        }
    }

    public Card[] GetCustomDeck()
    {
        AddCardsInTemporaryCollection();
        List<Card> customDeck = new List<Card>();
        int cardsLength = tempCollection.Count;

        while (cardsLength > 0)
        {
            int index = UnityEngine.Random.Range(0, cardsLength);
            Card card = tempCollection[index];
            customDeck.Add(card);
            tempCollection.RemoveAt(index);
            cardsLength--;
        }

        return customDeck.ToArray();
    }

    private List<Card> AddCardsInTemporaryCollection()
    {
        foreach (Card card in Collection)
        {
            tempCollection.Add(card);
        }

        return tempCollection;
    }
}
