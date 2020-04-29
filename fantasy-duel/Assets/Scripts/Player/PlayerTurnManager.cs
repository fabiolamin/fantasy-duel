using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerTurnManager : MonoBehaviourPunCallbacks
{
    private PhotonView photonView;
    private PlayerDeck playerDeck;
    private PlayerHUD playerHUD;
    private float durationAux;
    private bool IsReadyToCountdown = false;
    private ExitGames.Client.Photon.Hashtable property = new ExitGames.Client.Photon.Hashtable();

    [SerializeField] private float duration = 30f;
    [SerializeField] private float timeToShowDuration = 15f;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        playerDeck = GetComponent<PlayerDeck>();
        playerHUD = GetComponent<PlayerHUD>();

        durationAux = duration;

        if (!PhotonNetwork.IsMasterClient)
        {
            EndTurn();
        }
    }

    private void Update()
    {
        if (IsReadyToCountdown)
        {
            StartCountdown();
            VerifyCountdown();
        }
    }

    private void StartCountdown()
    {
        duration -= Time.deltaTime;
    }

    private void VerifyCountdown()
    {
        if (duration <= timeToShowDuration)
        {
            playerHUD.SetTurnDurationText((int)duration);

            if (duration <= 0)
            {
                EndTurn();
            }
        }
    }

    public void EndTurn()
    {
        if (photonView.IsMine)
        {
            playerHUD.UpdateTurnDurationText(false);
            duration = durationAux;
            IsReadyToCountdown = false;
            playerHUD.UpdateButtons(false);
            playerDeck.UpdateDeckLock(true);
            UpdateTurnProperty();
            PhotonNetwork.PlayerListOthers[0].SetCustomProperties(property);
        }
    }

    private void UpdateTurnProperty()
    {
        if (property.ContainsKey("IsReadyToPlayTurn"))
        {
            property.Remove("IsReadyToPlayTurn");
        }
        property.Add("IsReadyToPlayTurn", true);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer == photonView.Owner && photonView.IsMine)
        {
            playerHUD.UpdateTurnMessage(true);
            playerHUD.UpdateButtons(true);
            playerDeck.UpdateDeckLock(false);
            IsReadyToCountdown = true;
        }
    }
}
