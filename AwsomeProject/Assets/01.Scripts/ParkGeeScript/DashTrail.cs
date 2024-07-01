using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTrail : MonoBehaviour
{
    private ParticleSystem _particle;

    private void Awake()
    {
        _particle = GetComponentInChildren<ParticleSystem>();
        Debug.Log(_particle);
    }

    private void Start()
    {
        _particle.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_particle != null)
        {
            ParticleSystem.MainModule main = _particle.main;
            if (main.startRotation.mode == ParticleSystemCurveMode.Constant)
            {
                main.startRotation = -transform.eulerAngles.y * Mathf.Deg2Rad;
            }
        }
    }
}
