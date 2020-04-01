using System;
using System.IO;
using UnityEngine;

public class CardConversion : MonoBehaviour
{
    private string[] cardsDataFileNames = new string[] { "Bases", "Creatures", "Magics" };
    [SerializeField] private CardCollection cardCollection;
    [SerializeField] private string folderName;

    public void Deserialize()
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
                    for (int index = 0; index < array.Card.Length; index++)
                    {
                        Card card = array.Card[index];
                        cardCollection.Cards.Add(card);
                    }
                }
            }
        }
    }

    public void Serialize()
    {
        foreach (string fileName in cardsDataFileNames)
        {
            string path = Application.streamingAssetsPath + "/Data/" + folderName + "/" + fileName + ".json";

            File.WriteAllText(path, String.Empty);
            TextWriter tw = new StreamWriter(path, true);
            CardArray array = new CardArray();
            array.Card = cardCollection.Cards.FindAll(card => card.Type == fileName).ToArray();
            string jsonFile = JsonUtility.ToJson(array);
            tw.WriteLine(jsonFile);
            tw.Close();
        }
    }
}
