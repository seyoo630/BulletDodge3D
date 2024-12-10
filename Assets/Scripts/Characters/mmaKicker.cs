using System.Collections;
using UnityEngine;

public class MMAKicker : CharacterController
{
    protected override void Awake()
    {
        base.Awake();
        // MMAKicker 캐릭터에 대한 추가 설정이 필요하면 여기에 작성
    }

    protected override void Start()
    {
        base.Start();
        // MMAKicker 캐릭터에 대한 추가 설정이 필요하면 여기에 작성
    }

    protected override void Update()
    {
        base.Update();
        // MMAKicker 캐릭터에 대한 추가 업데이트 로직이 필요하면 여기에 작성
    }

    public override void ApplyDamage()
    {
        // MMAKicker 캐릭터의 데미지 적용 로직이 필요하면 여기에 작성
        base.ApplyDamage(); // 기본 로직 유지
    }

    protected override void HandleCombat()
    {
        base.HandleCombat();
        // MMAKicker 캐릭터의 추가 전투 로직이 필요하면 여기에 작성
    }

    protected override void DetectAndAttackEnemies()
    {
        base.DetectAndAttackEnemies();
        // MMAKicker 캐릭터의 추가 적 감지 및 공격 로직이 필요하면 여기에 작성
    }

    protected override IEnumerator RecoverFromFall()
    {
        yield return base.RecoverFromFall();
        // MMAKicker 캐릭터의 추가 회복 로직이 필요하면 여기에 작성
    }

    protected override void OnAnimatorMove()
    {
        base.OnAnimatorMove();
        // MMAKicker 캐릭터의 추가 애니메이터 이동 로직이 필요하면 여기에 작성
    }


    protected override void ApplyGradeBonuses()
    {
        base.ApplyGradeBonuses();
        // MMAKicker 캐릭터의 추가 등급 보너스가 있으면 작성
    }


}
