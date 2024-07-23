using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System;


public class Entity : MonoBehaviour, IDamageable {
        // 데미지를 입으면 3초간 빨간색으로 깜박거리는 코드 (필요시 삭제수정해주세요)
        // entity가 공격받을때 빨갛게 변함
        // 죽을때 파티클로 이펙트
        // awake에서 entityeditor 에 있는 정보 적용
        //

        public string type;
        public float damage = 10;
        private float maxHealth = 100;
        public float health;
        private float posture;
        private float defence;
        private float weight;
        private float speed;
        public GameObject deathEffectPrefab;

        protected Animator animator;
        private Renderer[] entityRenderer;
        private Color[] originalColor;

        private Rigidbody rb;
        private Vector3 originalPosition; // 공격 시 위치를 저장할 변수
      
        public event Action OnDeath; // 죽음 이벤트 선언

      //  protected virtual void Awake()
      //  {
      //      LoadEntityData();
      //  }

    protected virtual void Start() {

            health = maxHealth;
            animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        entityRenderer = GetComponentsInChildren<Renderer>();
            originalColor = new Color[entityRenderer.Length];
            for (int i = 0; i < entityRenderer.Length; i++) {
                originalColor[i] = entityRenderer[i].material.color;
            }

        }

        public float Health {
            get {
                return health;
            }
            set {
                if (health > value) {
                    StartCoroutine(BlinkRed());
                }

                health = value;

                if (health <= 0) {
                    Die();

                }
            }
        }

        protected virtual void Die() {
            Debug.Log("죽어버림ㅜㅜ");
            OnDeath?.Invoke(); // 죽음 이벤트 호출
        }

        private IEnumerator BlinkRed() {
            float elapsedTime = 0;
            bool isRed = false;

            while (elapsedTime < 2f) {
                for (int i = 0; i < entityRenderer.Length; i++) {
                    // ObstacleDetector 컴포넌트를 가진 오브젝트는 제외
                    if (entityRenderer[i].GetComponent<ObstacleDetector>() != null) {
                        continue;
                    }

                    entityRenderer[i].material.color = isRed ? originalColor[i] : Color.red;
                }
                isRed = !isRed;
                elapsedTime += 0.3f; // 깜박이는 속도
                yield return new WaitForSeconds(0.3f);
            }
            for (int i = 0; i < entityRenderer.Length; i++) {
                if (entityRenderer[i].GetComponent<ObstacleDetector>() != null) {
                    continue;
                }

                entityRenderer[i].material.color = originalColor[i];
            }
        }

        public virtual IEnumerator OnDie() // virtual 키워드 추가
        {
            animator.SetTrigger("Die");
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(stateInfo.length);
            Debug.Log("애니메이션 대기 완료");


            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);

        }

        public void TakeDamage(float damage) {
            Health -= damage;
        }
    public void Attack(Entity target)
    {
        if (target != null && target is IDamageable)
        {
            StartCoroutine(PerformAttack(target));
        }
    }

    private IEnumerator PerformAttack(Entity target)
    {
        // 현재 위치 저장
        originalPosition = transform.position;

        // Rigidbody의 x축과 z축 이동을 제한
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

        // 공격 애니메이션 트리거 설정
        animator.SetBool("Fight", true);

        // 공격 애니메이션 길이만큼 대기
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        // 공격 대상에게 데미지 입히기
        if (target != null && target is IDamageable)
        {
            ((IDamageable)target).TakeDamage(damage);
        }

        // Rigidbody의 이동 제한 해제
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // x, z 위치를 공격 시작 위치로 고정
        transform.position = new Vector3(originalPosition.x, transform.position.y, originalPosition.z);

        // 공격 애니메이션 종료
        animator.SetBool("Fight", false);
    }

    public void Initialize(Entity jsonEntity) {
            this.type = jsonEntity.type;
            this.name = jsonEntity.name;
            this.health = jsonEntity.health;
            this.maxHealth = jsonEntity.health; // assuming maxHealth should be set to the loaded health
            this.damage = jsonEntity.damage;
        }

    private void LoadEntityData()
    {
        string jsonFilePath = "Entities.json";
        EntityData entityData = JsonHelper.LoadFromJson(jsonFilePath);
        Entity jsonEntity = entityData.entities.Find(e => e.name == this.name && e.type == this.type);

        if (jsonEntity != null)
        {
            Initialize(jsonEntity);
        }
        else
        {
            Debug.LogError($"Entity data not found for {this.name} of type {this.type}");
        }
    }
}

public static class JsonHelper
{
    public static EntityData LoadFromJson(string path)
    {
        string fullPath = Path.Combine(Application.dataPath, path);
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            return JsonConvert.DeserializeObject<EntityData>(json);
        }
        else
        {
            Debug.LogError("JSON file not found");
            return new EntityData();
        }
    }

    public static void SaveToJson(EntityData data, string path)
    {
        string fullPath = Path.Combine(Application.dataPath, path);
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(fullPath, json);
    }

}

    public interface IDamageable //데미지 입는 인터페이스 
    {
        void TakeDamage(float damage);
    }


