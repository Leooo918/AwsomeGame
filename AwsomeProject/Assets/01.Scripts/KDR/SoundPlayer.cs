using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _lifeTime;
    private float _startTime;
    private bool _isLoop;

    public AudioClip currentAudioClip { get; private set; }

    public void Init(AudioClip audioClip, float volume, float lifetime, bool isDonDestroy, bool is3D)
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = volume;
        float _3dValue = is3D ? 1.0f : 0.0f;
        _audioSource.spatialBlend = _3dValue;
        currentAudioClip = audioClip;
        if (lifetime == -1)
        {
            _audioSource.loop = true;
            _isLoop = true;
        }
        else
        {
            _lifeTime = lifetime;
            _startTime = Time.time;
        }

        _audioSource.clip = audioClip;
        _audioSource.Play();

        if (isDonDestroy)
            DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (_isLoop) return;

        if (_startTime + _lifeTime < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
