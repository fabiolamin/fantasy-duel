using UnityEngine;
using Photon.Pun;

public class PlayerParticlesControl : MonoBehaviour
{
    private PlayerManager playerManager;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    public void PlayCardParticles(GameObject card, CardParticles cardParticles)
    {
        Card playedCard = card.GetComponent<CardInfo>().Card;

        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("PlayCardParticlesRPC", RpcTarget.AllBuffered, Utility.GetCardIndexFromList
            (
            playedCard, playerManager.PlayerBoardArea.Cards), (int)cardParticles
            );
        }
    }

    public void PlayOpponentCardParticles(GameObject card, CardParticles cardParticles)
    {
        Card playedCard = card.GetComponent<CardInfo>().Card;

        if (!playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("PlayCardParticlesRPC", RpcTarget.AllBuffered, Utility.GetCardIndexFromList
            (
            playedCard, playerManager.PlayerBoardArea.Cards), (int)cardParticles
            );
        }
    }

    [PunRPC]
    private void PlayCardParticlesRPC(int cardIndex, int particlesIndex)
    {
        playerManager.PlayerBoardArea.Cards[cardIndex].GetComponent<CardParticlesManager>().Play((CardParticles)particlesIndex);
    }

    public void StopAllCardsParticles()
    {
        playerManager.PlayerBoardArea.Cards.ForEach(card => {
            StopCardParticles(card, CardParticles.SelectMatch);
            StopCardParticles(card, CardParticles.Available);
        });
    }

    public void StopCardParticles(GameObject card, CardParticles cardParticles)
    {
        Card playedCard = card.GetComponent<CardInfo>().Card;

        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("StopCardParticlesRPC", RpcTarget.AllBuffered, Utility.GetCardIndexFromList
            (
            playedCard, playerManager.PlayerBoardArea.Cards), (int)cardParticles
            );
        }
    }

    public void StopOpponentCardParticles(GameObject card, CardParticles cardParticles)
    {
        Card playedCard = card.GetComponent<CardInfo>().Card;

        if (!playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("StopCardParticlesRPC", RpcTarget.AllBuffered, Utility.GetCardIndexFromList
            (
            playedCard, playerManager.PlayerBoardArea.Cards), (int)cardParticles
            );
        }
    }

    [PunRPC]
    private void StopCardParticlesRPC(int cardIndex, int particlesIndex)
    {
        if (cardIndex >= 0)
            playerManager.PlayerBoardArea.Cards[cardIndex].GetComponent<CardParticlesManager>().Stop((CardParticles)particlesIndex);
    }

    public void PlayOpponentCharacterParticles()
    {
        if(!playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("PlayOpponentCharacterParticlesRPC", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void PlayOpponentCharacterParticlesRPC()
    {
        GetComponentInChildren<Character>().PlayParticles(CharacterParticles.Selected);
    }

    public void StopCharacterParticles()
    {
        if(playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("StopCharacterParticlesRPC", RpcTarget.AllBuffered);
        }
    }

    public void StopOpponentCharacterParticles()
    {
        if (!playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("StopCharacterParticlesRPC", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void StopCharacterParticlesRPC()
    {
        GetComponentInChildren<Character>().StopParticles(CharacterParticles.Selected);
    }
}
