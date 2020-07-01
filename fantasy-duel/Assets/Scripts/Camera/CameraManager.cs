using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
    private bool HasKeyBeenPressed = false;

    [SerializeField] private Transform camera;
    [SerializeField] private Transform destination;
    [SerializeField] private float speed = 1500f;

    public static CameraManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("Camera Manager is NULL.");
            }

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
            HasKeyBeenPressed = true;

        if (HasKeyBeenPressed)
            Move();
    }

    private void Move()
    {
        if (camera.transform.position != destination.position)
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, destination.position, speed * Time.deltaTime);
    }
}
