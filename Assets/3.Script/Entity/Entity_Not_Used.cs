using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System;


public class Entity_Not_Used : MonoBehaviour
{
    //// 데미지를 입으면 3초간 빨간색으로 깜박거리는 코드 (필요시 삭제수정해주세요)
    //// entity가 공격받을때 빨갛게 변함
    //// 죽을때 파티클로 이펙트
    //// awake에서 entityeditor 에 있는 정보 적용 =>프리팹 수기로 체력 데미지 조정함
    //// 동물과 몬스터 공격과 데미지 처리 하는 부분 해당 클래스에 구현
    ////TakeDamage 메서드가 실행될 때 체력이 0 이하가 되면 Die 메서드가 호출되며, 이는 OnDeath 이벤트를 트리거합니다.
    //
    //public string type;
    //public string name;
    //public int damage = 10;
    //public int maxHealth = 100;
    //private int health;
   //// private float posture;
   //// private float defence;
   //// private float weight;
   //// private float speed;
    //public GameObject deathEffectPrefab;
    //
    //protected Animator animator;
    //private Renderer[] entityRenderer;
    //private Color[] originalColor;
    //protected Rigidbody rb;
    //protected Collider col;
    //protected Vector3 originalPosition; // 공격 시 위치를 저장할 변수
    //
    //public event Action OnDeath; // 죽음 이벤트 선언
    //
    ////  protected virtual void Awake()
    ////  {
    ////      LoadEntityData();
    ////  }
    //
    //protected virtual void Start()
    //{
    //
    //    health = maxHealth;
    //    animator = GetComponent<Animator>();
    //    col = GetComponent<Collider>(); // 콜라이더 초기화
    //    rb = GetComponent<Rigidbody>();
    //    entityRenderer = GetComponentsInChildren<Renderer>();
    //    //  LoadEntityData();
    //    originalColor = new Color[entityRenderer.Length]; //각 renderer의 원래 색상을 저장해서 깜박일때 본래 색으로 돌아올 수 있게.
    //    for (int i = 0; i < entityRenderer.Length; i++)
    //    {
    //        originalColor[i] = entityRenderer[i].material.color;
    //    }
    //}
    //
    //    public int Health {
    //    get {
    //        return health;
    //    }
    //    set {
    //        if (health > value) {
    //            StartCoroutine(BlinkRed());
    //            Debug.Log($"{name}가 데미지를 입어 현재 체력: {value}");
    //        }
    //        health = value;
    //
    //        if (health <= 0) {
    //            Die();
    //
    //        }
    //    }
    //}
    //
    //    protected virtual void Die() {
    //        Debug.Log($"{name}죽어버림ㅜㅜ");
    //    OnDeath?.Invoke(); // 죽음 이벤트 호출
    //      //  StartCoroutine(OnDie()); // 바로 OnDie 코루틴 호출
    //}
    //
   //// private IEnumerator DelayedDie()
   //// {
   ////     // OnDeath 이벤트가 완료될 때까지 대기
   ////     yield return new WaitForEndOfFrame();
   //// }
    //
    //protected IEnumerator BlinkRed() {
    //        float elapsedTime = 0;
    //        bool isRed = false;
    //
    //        while (elapsedTime < 2f) {
    //            for (int i = 0; i < entityRenderer.Length; i++) {
    //                // ObstacleDetector 컴포넌트를 가진 오브젝트는 제외
    //                if (entityRenderer[i].GetComponent<ObstacleDetector>() != null) {
    //                    continue;
    //                }
    //
    //                entityRenderer[i].material.color = isRed ? originalColor[i] : Color.red;
    //            }
    //            isRed = !isRed;
    //            elapsedTime += 0.3f; // 깜박이는 속도
    //            yield return new WaitForSeconds(0.3f);
    //        }
    //        for (int i = 0; i < entityRenderer.Length; i++) {//깜박이고 나서 원래 색깔로 돌아가는 부분
    //            if (entityRenderer[i].GetComponent<ObstacleDetector>() != null) {
    //                continue;
    //            }
    //
    //            entityRenderer[i].material.color = originalColor[i];
    //        }
    //    }
    //
    //    public virtual IEnumerator OnDie() // virtual 키워드 추가
    //    {
    //        animator.SetTrigger("Die");
    //        yield return new WaitForSeconds(2f);
    //        Debug.Log("애니메이션 대기 완료");
    //  
    //        Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
    //        Destroy(gameObject);
    //  
    //    }
    //
    //    public virtual void TakeDamage(int damage) {
    //    Debug.Log($"엔티티!! {name}이(가) {damage}의 데미지를 받음");
    //    Health -= damage;
    //
    //}
    //
    //public void Attack(Entity target)
    //{
    //    if (target != null && target is IDamageable)
    //    {
    //        StartCoroutine(PerformAttack(target));
    //    }
    //}
    //
    //private IEnumerator PerformAttack(Entity target) //공격 애니메이션 진행하는 부분만 남겨두고 좌표 고정하는것 모두 지움 
    //{
    //    // 공격 애니메이션 트리거 설정
    //    animator.SetBool("Fight", true);
    //
    //    // 공격 애니메이션 길이만큼 대기
    //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    //    yield return new WaitForSeconds(stateInfo.length);
    //
    //    // 공격 대상에게 데미지 입히기
    //    if (target != null && target is IDamageable)
    //    {
    //        ((IDamageable)target).TakeDamage(damage);
    //    }
    //    // 공격 애니메이션 종료
    //    animator.SetBool("Fight", false);
    //}

  //  public void Initialize(Entity jsonEntity)
  //  {
  //      this.health = jsonEntity.health;
  //      this.maxHealth = jsonEntity.health;
  //      this.damage = jsonEntity.damage;
  //  }
}

public interface IDamageable //데미지 입는 인터페이스 
    {
        void TakeDamage(int damage);
    }


