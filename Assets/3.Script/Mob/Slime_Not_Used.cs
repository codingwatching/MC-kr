using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Not_Used
{

    //public GameObject slimePrefab; // 원래 크기의 슬라임 프리팹
    //public float deathAnimationDuration = 2f; // 죽는 애니메이션의 지속 시간
    //private int currentHealth; // 슬라임의 현재 체력, 필요에 따라 초기화
    //public RuntimeAnimatorController slimeAnimatorController; // 기존 슬라임 애니메이터 컨트롤러
    //public RuntimeAnimatorController splitSlimeAnimatorController; // 분열된 슬라임 애니메이터 컨트롤러
    //
    //private Vector3 deathPosition; // 슬라임이 죽을 때의 위치
    //private Entity entity;
    //
    //protected override void Start() 
    //{
    //    entity = GetComponent<Entity>();
    //    if (entity != null)
    //    {
    //        entity.OnDeath += HandleDeath; // 죽음 이벤트 구독
    //    }
    //
    //    base.Start();
    //}
    //
    //// private void Update()
    //// {
    ////     if(Input.GetKeyDown(KeyCode.Q))
    ////     {
    ////         DieAndSplit();
    ////     }
    //// }
    //
    //private void HandleDeath() //슬라임의 죽음 위치를 기준으로 새로운 슬라임을 두 마리 생성합니다.
    //{
    //
    //    // 슬라임의 크기가 0.5 이하일 경우 더 이상 분열하지 않음
    //    if (transform.localScale.x <= 0.5f)
    //    {
    //        StartCoroutine(OnDie());
    //        return;
    //    }
    //
    //
    //    deathPosition = transform.position; // 죽은 위치 저장
    //
    //        Vector3 spawnPosition1 = deathPosition + new Vector3(-0.5f, 0, 0);
    //
    //    GameObject newSlime1 = Instantiate(slimePrefab, spawnPosition1, Quaternion.identity);
    //
    //        // 생성된 슬라임의 속성을 초기화
    //        InitializeNewSlime(newSlime1);
    //
    //    // OnDie 코루틴 호출
    //    StartCoroutine(OnDie());
    //}
    //
    //private void InitializeNewSlime(GameObject newSlime) //슬라임 애니메이션 클립에서 scale 조정하고 있어서 안 작아지는거였음 새로 애니메이션컨트롤러 만들어서 
    //                                                     // 복제된 슬라임의 컨트롤러를 scale 작아진 컨트롤러(slime_s)로 바꿈
    //{
    //    // 슬라임의 크기를 반으로 줄임
    //    newSlime.transform.localScale = transform.localScale * 0.5f;
    //
    //    // 슬라임의 Animator Controller를 slime에서 slime 1로 변경
    //    Animator animator = newSlime.GetComponent<Animator>();
    //    if (animator != null)
    //    {
    //        animator.runtimeAnimatorController = splitSlimeAnimatorController;
    //    }
    //
    //    // 슬라임의 Health 설정
    //    Slime slimeComponent = newSlime.GetComponent<Slime>();
    //    if (slimeComponent != null)
    //    {
    //        slimeComponent.currentHealth = currentHealth / 2;
    //        slimeComponent.Health = Health / 2;
    //    }
    //
    //    // 슬라임의 material을 프리팹의 material과 동일하게 설정(계속 blinkred 할때 색으로 분열되니까)
    //    Renderer[] renderers = newSlime.GetComponentsInChildren<Renderer>();
    //    Renderer prefabRenderer = slimePrefab.GetComponent<Renderer>();
    //    if (prefabRenderer != null)
    //    {
    //        foreach (var renderer in renderers)
    //        {
    //            renderer.material = prefabRenderer.material;
    //        }
    //    }
    //}
    //
    //public override void TakeDamage(int damage)
    //{
    //    base.TakeDamage(damage);
    //    StartCoroutine(BlinkRed()); // 데미지를 입었을 때 BlinkRed 호출
    //}
    //
    //protected override void Die()
    //{
    //    HandleDeath(); // 기본 Die 로직 대신 폭발 로직을 호출
    //}
}
