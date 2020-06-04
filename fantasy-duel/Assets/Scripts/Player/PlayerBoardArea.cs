using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerBoardArea : MonoBehaviourPunCallbacks
{
    private PlayerManager playerManager;
    [SerializeField] private GameObject playerLifeObject;
    public List<GameObject> Objects { get; private set; } = new List<GameObject>();
    public List<GameObject> Cards { get; private set; } = new List<GameObject>();

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        Objects.Add(playerLifeObject);
    }

    public void SetCard(Card card, Vector3 position)
    {
        if (playerManager.PhotonView.IsMine)
        {
            float[] cardPosition = new float[] { position.x, position.y, position.z };
            playerManager.PhotonView.RPC("SetCardRPC", RpcTarget.AllBuffered, playerManager.PlayerHand.GetIndex(card), cardPosition);
        }
    }

    [PunRPC]
    private void SetCardRPC(int index, float[] position)
    {
        playerManager.PlayerHand.Cards[index].transform.position = new Vector3(position[0], position[1], position[2]);
        playerManager.PlayerHand.Cards[index].transform.rotation = Quaternion.Euler(90, Utility.GetYRotation(), 0);
    }

    public void Add(GameObject card)
    {
        Card playedCard = card.GetComponent<CardInfo>().Card;

        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("AddRPC", RpcTarget.AllBuffered, playerManager.PlayerHand.GetIndex(playedCard));
        }
    }

    [PunRPC]
    private void AddRPC(int index)
    {
        Objects.Add(playerManager.PlayerHand.Cards[index]);
        Cards.Add(playerManager.PlayerHand.Cards[index]);
    }

    public void DamageObject(string targetTag, int targetId, string targetType, int amount)
    {
        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("DamageObjectRPC", RpcTarget.AllBuffered, targetTag, targetId, targetType, amount);
        }
    }

    [PunRPC]
    private void DamageObjectRPC(string targetTag, int targetId, string targetType, int amount)
    {
        switch (targetTag)
        {
            case "PlayerLife":
                playerManager.PlaySoundEffect(Clip.DiamondHit);
                GetComponent<IDamageable>().Damage(amount);
                break;

            case "Card":
                GameObject card = GetSelectedCard(targetId, targetType);
                card.GetComponent<IDamageable>().Damage(amount);
                break;
        }
    }

    private GameObject GetSelectedCard(int targetId, string targetType)
    {
        foreach (GameObject selectableCard in Cards)
        {
            Card card = selectableCard.GetComponent<CardInfo>().Card;

            if (card.Id == targetId && card.Type == targetType)
            {
                return selectableCard;
            }
        }

        return null;
    }

    public void SetDamageToObject(ExitGames.Client.Photon.Hashtable changedProps)
    {
        string targetTag = changedProps["TargetTag"].ToString();
        int targetCardId = 0;
        string targetCardType = "";

        if (targetTag == "Card")
        {
            targetCardId = (int)changedProps["TargetCardID"];
            targetCardType = changedProps["TargetCardType"].ToString();
        }

        int cardAttack = (int)changedProps["CardAttack"];

        DamageObject(targetTag, targetCardId, targetCardType, cardAttack);
    }
}
