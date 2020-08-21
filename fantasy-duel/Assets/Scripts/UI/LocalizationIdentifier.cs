using UnityEngine;

public class LocalizationIdentifier : MonoBehaviour
{
    [SerializeField] private LocalizationKeyNames keyName;

    public LocalizationKeyNames KeyName { get { return keyName; } private set { keyName = value; } }
}
