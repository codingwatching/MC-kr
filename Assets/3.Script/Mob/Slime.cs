using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster, IDamageable 
    {
    public GameObject slimePrefab; // 원래 크기의 슬라임 프리팹
    public float deathAnimationDuration = 2f; // 죽는 애니메이션의 지속 시간
    private int currentHealth = 50; // 슬라임의 현재 체력, 필요에 따라 초기화

    protected override void Start() 
    {
        base.Start();
           currentHealth = (int)Health; // 부모 클래스의 Health 초기값을 currentHealth에 저장
    }

    private void OnCollisionEnter(Collision collision) {
        base.OnCollisionEnter(collision); // Monster 클래스의 OnCollisionEnter 메서드 호출

        // 충돌한 물체가 플레이어 또는 동물인지 확인
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Animals")) {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null) {
                damageable.TakeDamage(10); // 닿은 물체에게 10의 데미지를 입힘
            }
        }
    }

    public void TakeDamage(float damage) 
    {
        currentHealth -= (int)damage; // 슬라임의 현재 체력을 감소시킵니다.
        if (currentHealth <= 0) {
            StartCoroutine(OnDie());
        }
    }

    protected override IEnumerator OnDie() {
        // 데스 이펙트 생성
        Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        // 죽는 애니메이션 길이만큼 대기
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 슬라임 분열 코루틴 호출
        yield return StartCoroutine(DieAndSplit());
    }

    private IEnumerator DieAndSplit() {
        // 두 개의 새로운 슬라임 생성
        for (int i = 0; i < 2; i++) {
            GameObject newSlime = Instantiate(slimePrefab, transform.position + new Vector3(i == 0 ? -0.5f : 0.5f, 0, 0), Quaternion.identity);
            newSlime.transform.localScale = transform.localScale / 2; // 분열된 슬라임의 크기를 반으로 줄임
            newSlime.GetComponent<Slime>().currentHealth = currentHealth / 2; // 분열된 슬라임의 체력도 나누어짐
        }

        // 일정 시간 대기 후 현재 슬라임 파괴
        yield return new WaitForSeconds(deathAnimationDuration);
        Destroy(gameObject);
    }
}
