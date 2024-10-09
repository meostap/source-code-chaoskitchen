using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{

    [SerializeField] private StoveCounter stoveCounter;
     private AudioSource audioSource;
    private float waringSoundTimer;
    private bool playWarningSound;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArg e)
    {
        float burnShowProgressAmount = .5f;
         playWarningSound = stoveCounter.IsFried() && e.progressNomalized >= burnShowProgressAmount;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnSateChangedEventArgs e)
    {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }

        
    }
    private void Update()
    {
        if (playWarningSound) {
            waringSoundTimer -= Time.deltaTime;
            if (waringSoundTimer < 0f)
            {
                float waringSoundTimeMax = .2f;
                waringSoundTimer = waringSoundTimeMax;

                SoundManager.Instance.playWarningSound(stoveCounter.transform.position);
            } 
        }
    }
}
