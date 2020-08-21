using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class LocalizationLoader : MonoBehaviour
{
    private LocalizationIdentifier[] identifiers;

    [SerializeField] private Localization[] localizations;

    public Localization[] Localizations { get { return localizations; } private set { localizations = value; } }

    private void Awake()
    {
        identifiers = Resources.FindObjectsOfTypeAll<LocalizationIdentifier>();
        Load((Languages)PlayerPrefs.GetInt("Language"));
    }

    public void Load(Languages language)
    {
        switch (language)
        {
            case Languages.English:
                foreach (var identifier in identifiers)
                {
                    identifier.GetComponent<Text>().text = localizations.Single(l => l.KeyName == identifier.KeyName).EnglishTranslation;
                }
                break;
            case Languages.Portuguese:
                foreach (var identifier in identifiers)
                {
                    identifier.GetComponent<Text>().text = localizations.Single(l => l.KeyName == identifier.KeyName).PortugueseTranslation;
                }
                break;
        }
    }

    public void UpdateLocalization()
    {
        string choosenLanguage = GameObject.FindGameObjectWithTag("Language").GetComponent<Text>().text;

        Languages language = (Languages)Enum.Parse(typeof(Languages), choosenLanguage);

        PlayerPrefs.SetInt("Language", (int)language);

        Load((Languages)PlayerPrefs.GetInt("Language"));
    }
}
