using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Animal {
    private Transform playerTransform;

    protected override void Update() {
        base.Update();
    }

    protected override void OnPlayerDetected() {
        playerTransform = FindObjectOfType<Player>().transform; // 플레이어의 Transform을 가져옴
        StartCoroutine(FleeSequence());
    }

    IEnumerator FleeSequence() {
        // 플레이어 주위를 한 바퀴 도는 동안의 시간
        float circleDuration = 5f;
        float circleSpeed = 3f;
        float circleRadius = 3f;
        float startTime = Time.time;

        while (Time.time < startTime + circleDuration) {
            Vector3 offset = new Vector3(Mathf.Sin(Time.time * circleSpeed) * circleRadius, 0, Mathf.Cos(Time.time * circleSpeed) * circleRadius);
            agent.SetDestination(playerTransform.position + offset);
            yield return null;
        }

        // 일정 시간 동안 플레이어를 따라다님
        ChangeState(State.Follow);
        yield return FollowPlayer(3f);// 여기서 FollowPlayer 코루틴이 호출됩니다.

        // 원래 상태로 복귀
        ChangeState(GetRandomState());
    }

    protected override void ChangeState(State newState) {
        currentState = newState;

        switch (currentState) {
            case State.Wander:
                agent.speed = baseSpeed;
                animator.SetInteger("Walk", 1);
                SetRandomDestination();
                break;
            case State.Jump:
                agent.isStopped = true;
                agent.updatePosition = false;
                agent.updateRotation = false;
                rb.isKinematic = false;
                animator.Play("Jump");
                StartCoroutine(JumpThenIdle());
                break;
            case State.Idle:
                agent.ResetPath();
                animator.Play("Idle");
                StartCoroutine(IdleThenWander());
                break;
            case State.Run:
                agent.speed = baseSpeed * 2;
                animator.SetInteger("Walk", 1);
                SetRandomDestination();
                break;
            case State.Follow:
                agent.speed = baseSpeed;
                break;
        }
    }

    private IEnumerator FollowPlayer(float duration) {
        float startTime = Time.time;
        while (Time.time < startTime + duration) {
            if (playerTransform != null) {
                agent.SetDestination(playerTransform.position);
            }
            yield return null;
        }
    }
}