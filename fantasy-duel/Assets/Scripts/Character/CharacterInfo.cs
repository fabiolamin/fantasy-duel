using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private int winsToUnlock;

    public int Id { get { return id; } private set { id = value; } }
    public int WinsToUnlock { get { return winsToUnlock; } private set { winsToUnlock = value; } }
    public bool IsUnlocked { get; private set; }

    private void Awake()
    {
        IsUnlocked = false;

        if (PlayerPrefs.GetInt("Wins") >= WinsToUnlock)
            IsUnlocked = true;
    }
}
