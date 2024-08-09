using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectInstantiateManager : Singleton<EffectInstantiateManager>
{
    [SerializeField] GameObject _particle;

    public void ParticleInstantiate()
    {
        Instantiate(_particle);
    }
}
