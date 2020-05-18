using Photon.Pun;

public class Utility
{
    public static int GetYRotation()
    {
        int y = 180;
        if (PhotonNetwork.IsMasterClient)
        {
            y = 0;
        }

        return y;
    }
}
