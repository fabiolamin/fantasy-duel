using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class PlayerInfo : MonoBehaviourPunCallbacks, IDamageable
{
    private PlayerManager playerManager;
    [SerializeField] private int maxLifePoints;
    [SerializeField] private int maxCoins;
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private ParticleSystem damageParticle;
    [SerializeField] private ParticleSystem coinsParticle;

    public int LifePoints { get; private set; }
    public int Coins { get; private set; }
    public int WonRounds { get; private set; } = 0;

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
        Damage(3);
        UpdateCoins(5);
    }

    [PunRPC]
    private void DamageRPC(int amount)
    {
        LifePoints = Mathf.Clamp(LifePoints - amount, 0, maxLifePoints);
        playerManager.PlayCharacterAnimation(CharacterAnimations.Damage);
        damageParticle.Play();

        if (LifePoints <= 0)
        {
            playerManager.PlayCharacterAnimation(CharacterAnimations.Die);
            AudioManager.Instance.Play(Audio.SoundEffects, Clip.Round, false);
            RoundManager.Instance.SetRound();
            RoundManager.Instance.CheckRounds();
        }
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

    public void Damage(int amount)
    {
        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("DamageRPC", RpcTarget.AllBuffered, amount);
            playerManager.PlayerHUD.SetHUD();
        }
    }

    public void AddRound()
    {
        if(playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("AddRoundRPC", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void AddRoundRPC()
    {
        WonRounds++;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(targetPlayer == playerManager.PhotonView.Owner && playerManager.PhotonView.IsMine)
        {
            if(changedProps.ContainsKey("IsReadyToPlayTurn"))
            {
                playerManager.PlayerTurn.StartTurn();
            }
            else if(changedProps.ContainsKey("IsReadyToUpdateObject"))
            {
                playerManager.PlayerBoardArea.SetDamageToObject(changedProps);
            }
            else if (changedProps.ContainsKey("IsMatchOver"))
            {
                playerManager.PlayerHUD.ActiveMatchMessage(false);
                playerManager.PlayerHUD.ShowEndMatchPanel();
                playerManager.IsReadyToDisconnect = true;
            }
        }
    }
}
