using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudio : MonoBehaviour
{
    [SerializeField] private AudioClip loseLife;
    [SerializeField] private AudioClip gainPoint;

    private AudioSource audioSource;

    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGainPoint()
    {
        audioSource.PlayOneShot(gainPoint);
        
    }

    public void PlayLoseLife()
    {
        audioSource.PlayOneShot(loseLife);
    }
}
