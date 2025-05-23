using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public static PlayerManager Instance => instance;

    Player player;
    public Player Player => player;

    Interaction interaction;
    public Interaction Interaction => interaction;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        player = GetComponent<Player>();
        interaction = GetComponent<Interaction>();
    }
}
