using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public static PlayerManager Instance => instance;

    Player player;
    public Player Player => player;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        player = GetComponent<Player>();
    }
}
