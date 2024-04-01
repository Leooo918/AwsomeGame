using System.Data.Common;

public class PlayerDashSkill : Skill
{
    public float dashPower;
    public float dashTime;

    public bool canUseSkill;
    public bool isInvincibleWhileDash;
    public bool isAttackWhileDash;

    public override void UseSkill()
    {
        //스킬이 해금되지 않았으면 return false
        if (canUseSkill == false) return;

        Player player = owner as Player;
        player.Dash(dashTime / 10f, dashPower, isInvincibleWhileDash, isAttackWhileDash);
    }

    public void Init(float dashPower, float dashTime, bool canUseSkill, bool isInvincibleWhileDash, bool isAttackWhileDash)
    {
        this.dashPower = dashPower;
        this.dashTime = dashTime;
        this.canUseSkill = canUseSkill;
        this.isInvincibleWhileDash = isInvincibleWhileDash;
        this.isAttackWhileDash = isAttackWhileDash;
    }
}
