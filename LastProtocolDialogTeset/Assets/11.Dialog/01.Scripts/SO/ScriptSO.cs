using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum ScriptType
{
    NORMAL = 0,
    BRANCH = 1,
    OPTION = 2,
    IMAGE = 3
}


public abstract class ScriptSO : ScriptableObject
{
    public string talkingPeopleName;
    
    [TextArea(3,5)]
    public string talkingDetails;
    
    public List<DialogCharactor> charactors;
    public Sprite background;

    [Space(16)]
    public AudioClip soundEffect;
    public UnityEvent onComplete;

    [HideInInspector] public string guid;
    [HideInInspector] public ScriptType scriptType;
    [HideInInspector] public Vector2 position;
}

[System.Serializable]
public struct DialogCharactor
{
    [Tooltip("캐릭터")]
    public GameObject charactor;
    [Tooltip("대사를 읽기전에 시행할 애니메이션")]
    public CharactorAnimation beforeReadAnimation;
    [Tooltip("대사를 읽고 난 후 시행할 애니메이션")]
    public CharactorAnimation afterReadAnimation;

    public DialogCharactor(GameObject charactor, CharactorAnimation beforeReadAnimation, CharactorAnimation afterReadAnimation)
    {
        this.charactor = charactor;
        this.beforeReadAnimation = beforeReadAnimation;
        this.afterReadAnimation = afterReadAnimation;
    }
}
