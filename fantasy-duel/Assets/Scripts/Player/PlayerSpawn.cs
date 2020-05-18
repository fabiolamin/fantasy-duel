using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSpawn : MonoBehaviour
{
    private void Awake()
    {
        InstantiatePlayer();
    }

    private void InstantiatePlayer()
    {
        //int y = 180;
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    y = 0;
        //}

        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.Euler(0, Utility.GetYRotation(), 0));
    }
}
