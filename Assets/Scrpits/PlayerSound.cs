using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
   private Player player;
    private float footstepTimer;
    private float footstepTimeMax=.1f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0)
        {

            if (player.IsWalking()) { 
            footstepTimer = footstepTimeMax;
            float volume = 1f;
            SoundManager.Instance.PlayerFootstepsSound(player.transform.position, volume);
            }
        }
    }


}
