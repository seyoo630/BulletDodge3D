using System.Collections;
using UnityEngine;

public class MMAKicker : CharacterController
{
    protected override void Awake()
    {
        base.Awake();
        // MMAKicker ĳ���Ϳ� ���� �߰� ������ �ʿ��ϸ� ���⿡ �ۼ�
    }

    protected override void Start()
    {
        base.Start();
        // MMAKicker ĳ���Ϳ� ���� �߰� ������ �ʿ��ϸ� ���⿡ �ۼ�
    }

    protected override void Update()
    {
        base.Update();
        // MMAKicker ĳ���Ϳ� ���� �߰� ������Ʈ ������ �ʿ��ϸ� ���⿡ �ۼ�
    }

    public override void ApplyDamage()
    {
        // MMAKicker ĳ������ ������ ���� ������ �ʿ��ϸ� ���⿡ �ۼ�
        base.ApplyDamage(); // �⺻ ���� ����
    }

    protected override void HandleCombat()
    {
        base.HandleCombat();
        // MMAKicker ĳ������ �߰� ���� ������ �ʿ��ϸ� ���⿡ �ۼ�
    }

    protected override void DetectAndAttackEnemies()
    {
        base.DetectAndAttackEnemies();
        // MMAKicker ĳ������ �߰� �� ���� �� ���� ������ �ʿ��ϸ� ���⿡ �ۼ�
    }

    protected override IEnumerator RecoverFromFall()
    {
        yield return base.RecoverFromFall();
        // MMAKicker ĳ������ �߰� ȸ�� ������ �ʿ��ϸ� ���⿡ �ۼ�
    }

    protected override void OnAnimatorMove()
    {
        base.OnAnimatorMove();
        // MMAKicker ĳ������ �߰� �ִϸ����� �̵� ������ �ʿ��ϸ� ���⿡ �ۼ�
    }


    protected override void ApplyGradeBonuses()
    {
        base.ApplyGradeBonuses();
        // MMAKicker ĳ������ �߰� ��� ���ʽ��� ������ �ۼ�
    }


}
