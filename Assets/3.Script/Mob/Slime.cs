using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster, IDamageable 
    {

    public GameObject slimePrefab; // 원래 크기의 슬라임 프리팹
    public float deathAnimationDuration = 2f; // 죽는 애니메이션의 지속 시간
    private int currentHealth = 20; // 슬라임의 현재 체력, 필요에 따라 초기화
    public RuntimeAnimatorController slimeAnimatorController; // 기존 슬라임 애니메이터 컨트롤러
    public RuntimeAnimatorController splitSlimeAnimatorController; // 분열된 슬라임 애니메이터 컨트롤러


    private Vector3 deathPosition; // 슬라임이 죽을 때의 위치

    private Entity entity;

    protected override void Start() 
    {
        entity = GetComponent<Entity>();
        if (entity != null)
        {
            entity.OnDeath += HandleDeath; // 죽음 이벤트 구독
        }

        base.Start();
           currentHealth = (int)Health; // 부모 클래스의 Health 초기값을 currentHealth에 저장
       // DieAndSplit();
    }


   // private void Update()
   // {
   //     if(Input.GetKeyDown(KeyCode.Q))
   //     {
   //         DieAndSplit();
   //     }
   // }


    private void OnCollisionEnter(Collision collision) //충돌시 다른 오브젝트에게 데미지를 줌 
    { 
        base.OnCollisionEnter(collision); // Monster 클래스의 OnCollisionEnter 메서드 호출

        // 충돌한 물체가 플레이어 또는 동물인지 확인
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Animals")) {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null) {
                damageable.TakeDamage(10); // 닿은 물체에게 10의 데미지를 입힘
            }
        }
    }

    private void HandleDeath() 
    {
        deathPosition = transform.position; // 죽은 위치 저장
        Debug.Log("슬라임 분열 시작");

            Vector3 spawnPosition1 = deathPosition + new Vector3(-0.5f, 0, 0);
        Vector3 spawnPosition2 = deathPosition + new Vector3(0.5f, 0, 0);

        GameObject newSlime1 = Instantiate(slimePrefab, spawnPosition1, Quaternion.identity);
        GameObject newSlime2 = Instantiate(slimePrefab, spawnPosition2, Quaternion.identity);

            // 생성된 슬라임의 속성을 초기화
            InitializeNewSlime(newSlime1);
            InitializeNewSlime(newSlime2);
    }

    private void InitializeNewSlime(GameObject newSlime)
    {
        Debug.Log("슬라임 초기화!");

        // 슬라임의 크기를 반으로 줄임
        newSlime.transform.localScale = transform.localScale * 0.5f;

        // 슬라임의 Animator Controller를 slime에서 slime 1로 변경
        Animator animator = newSlime.GetComponent<Animator>();
        if (animator != null)
        {
            animator.runtimeAnimatorController = splitSlimeAnimatorController;
        }

        // 슬라임의 Health 설정
        Slime slimeComponent = newSlime.GetComponent<Slime>();
        if (slimeComponent != null)
        {
            slimeComponent.currentHealth = currentHealth / 2;
            slimeComponent.Health = Health / 2;
        }

        // 슬라임의 material을 프리팹의 material과 동일하게 설정
        Renderer[] renderers = newSlime.GetComponentsInChildren<Renderer>();
        Renderer prefabRenderer = slimePrefab.GetComponent<Renderer>();
        if (prefabRenderer != null)
        {
            foreach (var renderer in renderers)
            {
                renderer.material = prefabRenderer.material;
            }
        }
    }

}
