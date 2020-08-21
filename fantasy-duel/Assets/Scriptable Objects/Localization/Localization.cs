using UnityEngine;

[CreateAssetMenu(menuName = "New translation")]
public class Localization : ScriptableObject
{
    [SerializeField] private LocalizationKeyNames keyName;
    [SerializeField] private string englishTranslation;
    [SerializeField] private string portugueseTranslation;

    public LocalizationKeyNames KeyName { get { return keyName; } private set { keyName = value; } }
    public string EnglishTranslation { get { return englishTranslation; } private set { englishTranslation = value; } }
    public string PortugueseTranslation { get { return portugueseTranslation; } private set { portugueseTranslation = value; } }
}
