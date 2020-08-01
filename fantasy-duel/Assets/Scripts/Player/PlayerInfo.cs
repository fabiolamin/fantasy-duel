using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class PlayerInfo : MonoBehaviourPunCallbacks
{
    private PlayerManager playerManager;
    [SerializeField] private int maxLifePoints;
    [SerializeField] private int maxCoins;
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private ParticleSystem coinsParticle;

    public int LifePoints { get; private set; }
    public int Coins { get; private set; }
    public int WonRounds { get; private set; } = 0;

    public Character Character { get; set; }

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        SetAttributes();

        if (playerManager.PhotonView.IsMine)
        {
            SetCamera();
        }
    }

    public void SetAttributes()
    {
        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("SetAttributesRPC", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void SetAttributesRPC()
    {
        LifePoints = maxLifePoints;
        Coins = maxCoins;
    }

    private void SetCamera()
    {
        Instantiate(cameraPrefab, new Vector3(0.2f, 32, 0.3f), Quaternion.Euler(90, Utility.GetYRotation(), 0), transform);
    }

    public void TransferSomeLifePointsToCoins()
    {
        playerManager.PlaySoundEffect(Clip.Sacrifice);
        ChangeLife(-3);
        UpdateCoins(5);
    }

    [PunRPC]
    private void ChangeLifeRPC(int value)
    {
        LifePoints = Mathf.Clamp(LifePoints + value, 0, maxLifePoints);

        if (value < 0)
        {
            Character.PlayAnimation(CharacterAnimations.Damage);
            Character.PlayParticles(CharacterParticles.Damage);
            CheckLife();
        }
        else
            Character.PlayParticles(CharacterParticles.Healing);
    }

    private void CheckLife()
    {
        if (LifePoints <= 0)
        {
            Character.PlayAnimation(CharacterAnimations.Die);
            AudioManager.Instance.Play(Audio.SoundEffects, Clip.Round, false);
            RoundManager.Instance.SetRound();
            RoundManager.Instance.CheckRounds();
        }
    }

    public void PlayCharacterAnimation(CharacterAnimations characterAnimations)
    {
        if (playerManager.PhotonView.IsMine)
            playerManager.PhotonView.RPC("PlayCharacterAnimationRPC", RpcTarget.AllBuffered, (int)characterAnimations);
    }

    [PunRPC]
    private void PlayCharacterAnimationRPC(int index)
    {
        Character.PlayAnimation((CharacterAnimations)index);
    }

    public void UpdateCoins(int amount)
    {
        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("UpdateCoinsRPC", RpcTarget.AllBuffered, amount);
            playerManager.PlayerHUD.SetHUD();
        }
    }

    [PunRPC]
    private void UpdateCoinsRPC(int amount)
    {
        coinsParticle.Play();
        Coins = Mathf.Clamp(Coins + amount, 0, maxCoins);
    }

    public void AddRound()
    {
        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("AddRoundRPC", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void AddRoundRPC()
    {
        WonRounds++;
    }

    private void CheckPlayerResult()
    {
        if (!RoundManager.Instance.isThereAPlayerLeavingMatch)
        {
            SetPlayerResult();
        }
    }

    private void SetPlayerResult()
    {
        if (WonRounds == RoundManager.Instance.RoundsToFinish)
        {
            int wins = 0;
            wins = PlayerPrefs.GetInt("Wins");
            wins++;
            PlayerPrefs.SetInt("Wins", wins);
        }
        else
        {
            int losses = 0;
            losses = PlayerPrefs.GetInt("Losses");
            losses++;
            PlayerPrefs.SetInt("Losses", losses);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == playerManager.PhotonView.Owner && playerManager.PhotonView.IsMine)
        {
            if (changedProps.ContainsKey("IsReadyToPlayTurn"))
            {
                playerManager.PlayerTurn.StartTurn();
            }
            else if (changedProps.ContainsKey("IsReadyToUpdateObject"))
            {
                playerManager.PlayerBoardArea.UpdateObject(changedProps);
            }
            else if (changedProps.ContainsKey("IsMatchOver"))
            {
                playerManager.PlayerHUD.ActiveNotification(false);
                playerManager.PlayerHUD.ShowEndMatchPanel();
                playerManager.IsReadyToDisconnect = true;
                CheckPlayerResult();
            }
        }
    }

    public void ChangeLife(int value)
    {
        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("ChangeLifeRPC", RpcTarget.AllBuffered, value);
            playerManager.PlayerHUD.SetHUD();
        }
    }
}
