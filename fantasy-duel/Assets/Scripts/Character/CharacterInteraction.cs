﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterInteraction : MonoBehaviour, ISelectable, IProtectable
{
    private PlayerManager playerManager;
    public bool IsSelected { get; set; }
    public bool IsProteged { get; set; }
    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            playerManager = transform.root.GetComponent<PlayerManager>();
            IsProteged = true;
        }
    }

    private void OnMouseDown()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Deselect();
            Select();
        }
    }

    public void Select()
    {
        if (!playerManager.PlayerTurn.IsMyTurn && !playerManager.PhotonView.IsMine)
        {
            GetComponent<Character>().StopParticles(CharacterParticles.Target);
            playerManager.PlayerParticlesControl.PlayOpponentCharacterParticles();
        }

        playerManager.PlaySoundEffect(Clip.ObjectHit);
        IsSelected = true;
    }

    public void Deselect()
    {
        foreach (GameObject obj in playerManager.PlayerBoardArea.Objects)
        {
            obj.GetComponent<ISelectable>().IsSelected = false;

            if (obj.CompareTag("Card"))
            {
                playerManager.PlayerParticlesControl.StopCardParticles(obj, CardParticles.SelectMatch);

                if (!playerManager.PlayerTurn.IsMyTurn && !playerManager.PhotonView.IsMine)
                {
                    playerManager.PlayerParticlesControl.StopOpponentCardParticles(obj, CardParticles.SelectMatch);
                }
            }
            else
                playerManager.PlayerParticlesControl.StopOpponentCharacterParticles();
        }
    }
}
