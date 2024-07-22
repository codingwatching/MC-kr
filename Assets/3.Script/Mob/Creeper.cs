using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creeper : Monster, IDamageable
{

    public Collider coll;
    private Entity entity;
    public GameObject explosionEffectPrefab;
    public float explosionScaleFactor = 10;
    public float explosionDuration = 5f;
    public float explosionForce = 100000f; // 폭발력이 얼마나 강력할지 설정
    public bool showExplosionRange = true; // 폭발 범위를 보여줄지 여부를 설정하는 변수

    protected override void Start()
    {
        entity = GetComponent<Entity>();
        if (entity != null)
        {
            entity.OnDeath += HandleDeath; // 죽음 이벤트 구독
        }

        base.Start();

    }

    private void OnCollisionEnter(Collision collision) //충돌시 다른 오브젝트에게 데미지를 줌 
    {
        base.OnCollisionEnter(collision); // Monster 클래스의 OnCollisionEnter 메서드 호출

        // 충돌한 물체가 플레이어 또는 동물인지 확인
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Animals"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(10); // 닿은 물체에게 10의 데미지를 입힘
            }
        }
    }

    private void HandleDeath()
    {
        /*
        크리퍼의 콜라이더가 3배로 천천히 커지면서 
        콜라이더에 닿는 오브젝트들을 모두 파괴시켜야함 
        파티클로 폭발하는듯한 이펙트가 나와야함 (파티클은 큐브모양. material은 creeper 와 같은 material로 
        */
        StartCoroutine(ExplosionSequence());

    }

    private IEnumerator ExplosionSequence()
    {
        Debug.Log("폭발준비중");
        // 콜라이더 크기를 점진적으로 증가
        Vector3 originalScale = coll.transform.localScale;
        Vector3 targetScale = originalScale * explosionScaleFactor;
        float elapsedTime = 0f; // 경과 시간을 초기화합니다.

        while (elapsedTime < explosionDuration)
        {
            coll.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / explosionDuration);
            // 경과 시간에 비례하여 콜라이더의 크기를 원래 크기에서 목표 크기로 점진적으로 증가시킵니다.
            elapsedTime += Time.deltaTime;
            Debug.Log($"콜라이더 크기 증가 중: {coll.transform.localScale}");
            yield return null;
        }

        coll.transform.localScale = targetScale;
        // 콜라이더의 크기를 목표 크기로 설정합니다.
        Debug.Log("폭발!");

        // 콜라이더에 닿는 모든 오브젝트 처리
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, coll.bounds.extents.magnitude);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject) // 자기 자신은 제외
            {
                // Rigidbody가 있는 경우 폭발력을 가함
                Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 explosionDirection = hitCollider.transform.position - transform.position;
                    rb.AddExplosionForce(explosionForce, transform.position, coll.bounds.extents.magnitude);
                    // Rigidbody가 있는 경우, 폭발력을 가합니다. AddExplosionForce는 지정된 힘으로 오브젝트를 밀어냅니다.
                }

                // Player 또는 Animals 태그를 가진 물체의 체력 감소
                if (hitCollider.gameObject.CompareTag("Player") || hitCollider.gameObject.CompareTag("Animals"))
                {
                    IDamageable damageable = hitCollider.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(50); // 체력 50 감소
                        Debug.Log($"체력 감소: {hitCollider.gameObject.name}");
                    }
                }
                // 폭발 범위 내의 모든 오브젝트 파괴
                Destroy(hitCollider.gameObject);
                Debug.Log($"오브젝트 파괴: {hitCollider.gameObject.name}");
            }
        }

        // 폭발 파티클 이펙트 생성

        GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Renderer explosionRenderer = explosionEffect.GetComponent<Renderer>();
        Renderer creeperRenderer = GetComponent<Renderer>();
        if (creeperRenderer != null && explosionRenderer != null)
        {
            explosionRenderer.material = creeperRenderer.material;
            // 크리퍼의 material을 폭발 파티클 이펙트에 적용합니다.
            Debug.Log("파티클 이펙트 생성");
        }

        Destroy(explosionEffect, 2f); // 2초 후 파티클 이펙트 제거

        // OnDie 코루틴 호출
        StartCoroutine(entity.OnDie());
    }

    private void OnDrawGizmos()
    {
        if (coll != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, coll.bounds.extents.magnitude * explosionScaleFactor);
            // Gizmos를 사용하여 크리퍼의 위치를 중심으로 콜라이더의 반경에 해당하는 와이어 프레임 구를 그립니다.
        }
    }

}
