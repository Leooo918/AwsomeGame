using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AirBirdEnum
{
    Idle,
    Patrol,
    Chase,
    Shoot,
    Dead
}

public enum AirBirdSkillEnum
{
    Shoot
}

public class AirBird : Enemy<AirBirdEnum>
{
    
}
