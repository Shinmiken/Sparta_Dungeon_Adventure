using UnityEngine;

public class WalkAudio : MonoBehaviour
{
    public AudioClip[] WalkClip;
    private AudioSource audioSource;
    public float walkThreshold;
    public float walkRate;
    public float runRate;
    private float walkTime;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(PlayerManager.Instance.Player.Rb.velocity.y) < 0.5f)
        {
            if(PlayerManager.Instance.Player.Rb.velocity.magnitude > walkThreshold)
            {
                if (PlayerManager.Instance.Player.isRun && Time.time - walkTime > runRate)
                {
                    walkTime = Time.time;
                    audioSource.PlayOneShot(WalkClip[Random.Range(0, WalkClip.Length)]);
                }
                else if (Time.time -  walkTime > walkRate)
                {
                    walkTime = Time.time;
                    audioSource.PlayOneShot(WalkClip[Random.Range(0, WalkClip.Length)]);
                }
            }
        }
    }
}
