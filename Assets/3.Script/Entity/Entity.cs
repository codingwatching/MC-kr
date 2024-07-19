using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // 데미지를 입으면 3초간 빨간색으로 깜박거리는 코드 (필요시 삭제수정해주세요)
    // 원본 코드 하단에 있어서 필요시 복붙으로 수정 가능합니다.
    // entity가 공격받을때 빨갛게 변함
    // 죽을때 파티클로 이펙트 넣으려고 하는중
    // Start is called before the first frame update

    private float maxHealth=100;
    private float health;
    private float posture;
    private float defence;
    private float weight;
    private float speed;
    public GameObject deathEffectPrefab;

    protected Animator animator;
    private Renderer[] entityRenderer;
    private Color[] originalColor;

    protected virtual void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        entityRenderer = GetComponentsInChildren<Renderer>();
        originalColor = new Color[entityRenderer.Length];
        for (int i = 0; i < entityRenderer.Length; i++)
        {
            originalColor[i] = entityRenderer[i].material.color;
        }

    }

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            if(health > value)
            {
                StartCoroutine(BlinkRed());
            }

            health = value;

            if(health <= 0)
            {
                OnDie();
            }
        }
    }

    private IEnumerator BlinkRed()
    {
        float elapsedTime = 0;
        bool isRed = false;

        while(elapsedTime<2f)
        {
            for (int i = 0; i < entityRenderer.Length; i++)
            {
                entityRenderer[i].material.color = isRed ? originalColor[i] : Color.red;
            }
                isRed = !isRed;
            elapsedTime += 0.3f;  //깜박이는 속도
            yield return new WaitForSeconds(0.3f);
        }
        for (int i = 0; i < entityRenderer.Length; i++)
        {
            entityRenderer[i].material.color = originalColor[i];
        }
    }

    protected virtual void OnDie()
    {
        Debug.Log($"{name} 이 죽었습니다. ");
        if (animator != null)
        {
            animator.SetBool("Die", true); // die 애니메이션 트리거
        }
        Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject,1f);
    }

    public void TakeDamage(float damage) {
        Health -= damage;
    }
}
/*
 
★초기상태 필요하면 복붙해서 원복하기★

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // Start is called before the first frame update
    private float maxHealth;
    private float health;
    private float posture;
    private float defence;
    private float weight;
    private float speed;

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;

            if(health <= 0)
            {
                //Ondie 메서드 만들기
            }
        }
    }
    //나머지는 커스텀 해서 구현 할 것

}
 
 */