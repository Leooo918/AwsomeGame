using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordUpgrade : MonoBehaviour
{
    private Player player;
    [SerializeField] private ParticleSystem upgradeParticle;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            upgradeParticle.Play();
        }
    }
}
