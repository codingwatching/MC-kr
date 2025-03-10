using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity_Data;

public class Entity_Non_Humanoid_Control : MonoBehaviour
{
    [SerializeField] private Entity entity;
    [SerializeField] private Rigidbody rigidbody_self;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform head_transform;
    [SerializeField] private GameObject position_anchor;
    [SerializeField] private GameObject rotation_anchor;

    [SerializeField] private Target_Handler target_handler;

    [SerializeField] private bool is_passive = false;

    private Quaternion target_rotation;
    private float key_h, key_v;

    private float speed_h;
    private float speed_v;

    private float jump_height = 1f;

    private float input_key_h = 0f;
    private float input_key_v;

    private bool input_key_sprint = false;
    private bool input_key_jump = false;

    private bool is_L_down = false;
    private bool is_R_down = false;

    private bool is_tracking = false;



    [SerializeField] private float EntityWidth = 0.25f;
    [SerializeField] private float EntityHeight = 2f;
    [SerializeField] private bool isGrounded;
    private Vector3 velocity;
    private World world;



    private void Awake()
    {
        world = FindObjectOfType<World>();
        TryGetComponent(out rigidbody_self);
        TryGetComponent(out entity);
        TryGetComponent(out animator);
        TryGetComponent(out target_handler);
    }

    private void Update()
    {
        if ((target_handler.target != null && target_handler.target.activeSelf != false) && !is_tracking)
        {
            is_tracking = true;
            StartCoroutine(Tracking_Target_Co());
            StartCoroutine(Attack_Target_Co());
        }
        else if (target_handler.target == null || target_handler.target.activeSelf == false)
        {
            target_handler.target = null;
            is_tracking = false;
        }

        isGrounded = Check_Ground();

        Move_Control();
        Attack_Control();
    }

    private void LateUpdate()
    {
        Rotation_Control();

        if (animator.GetFloat("Speed_V") > 0f && front)
        {
            transform.position += transform.forward * -0.1f;
            input_key_jump = true;
        }
        else input_key_jump = false;

        if (animator.GetFloat("Speed_V") < 0f && back)
        {
            transform.position += transform.forward * 0.1f;
            input_key_jump = true;
        }
        else input_key_jump = false;
        
        if (animator.GetFloat("Speed_H") > 0f && right)
        {
            transform.position += transform.right * -0.1f;
            input_key_jump = true;
        }
        else input_key_jump = false;

        if (animator.GetFloat("Speed_H") < 0f && left)
        {
            transform.position += transform.right * 0.1f;
            input_key_jump = true;
        }
        else input_key_jump = false;

        if (down) transform.position += transform.up * 0.1f;
    }

    private IEnumerator Tracking_Target_Co()
    {
        while (target_handler.target != null)
        {
            input_key_v = Mathf.Lerp(input_key_v, 1f, 0.5f);
            input_key_sprint = true;
            yield return new WaitForSeconds(1.0f);
        }
        input_key_v = 0f;
        input_key_sprint = false;
        yield return null;
    }
    private IEnumerator Attack_Target_Co()
    {
        while (target_handler.target != null)
        {
            if (Vector3.Distance(target_handler.target.transform.position, transform.position) < 2f)
            {
                switch (Random.Range(0, 6))
                {
                    case 0:
                    case 1:
                        is_L_down = true;
                        is_R_down = false;
                        break;
                    case 2:
                    case 3:
                        is_L_down = false;
                        is_R_down = true;
                        break;
                    case 4:
                        is_L_down = true;
                        is_R_down = true;
                        break;
                    case 5:
                        is_L_down = false;
                        is_R_down = false;
                        yield return new WaitForSeconds(2.0f);
                        break;
                }
            }
            yield return new WaitForSeconds(0.5f);
            is_L_down = false;
            is_R_down = false;
            yield return new WaitForSeconds(0.5f);
        }
        is_L_down = false;
        is_R_down = false;
        yield return null;
    }
    private void Rotation_Control()
    {
        rotation_anchor.transform.position = position_anchor.transform.position;
        if (target_handler.target != null && target_handler.target.activeSelf != false) rotation_anchor.transform.LookAt(target_handler.target.transform);

        target_rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, rotation_anchor.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        if (!animator.GetBool("Is_Attacking"))
        {
            if (is_passive)
            {
                target_rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, rotation_anchor.transform.rotation.eulerAngles.y - 180f, transform.rotation.eulerAngles.z);
                transform.rotation = Quaternion.Slerp(transform.rotation, target_rotation, 5f * Time.deltaTime);
                head_transform.LookAt(rotation_anchor.transform.position + rotation_anchor.transform.forward * -5f);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, target_rotation, 5f * Time.deltaTime);
                head_transform.LookAt(rotation_anchor.transform.position + rotation_anchor.transform.forward * 5f);
            }
        }
    }
    private void Move_Control()
    {
        if (isGrounded)
        {
            if (!animator.GetBool("Is_Attacking"))
            {
                key_h = input_key_h;
                key_v = input_key_v;

                if (key_h != 0 && (animator.GetInteger("Moveset_Number") == 2 || animator.GetInteger("Moveset_Number") == -2))
                {
                    if (key_h < 0) animator.SetInteger("Moveset_Number", -2);
                    if (key_h > 0) animator.SetInteger("Moveset_Number", 2);
                }

                animator.SetBool("IsGround", true);
                animator.SetBool("IsJump", false);

                velocity = Vector3.zero;

                // 짬푸
                if (input_key_jump)
                {
                    animator.SetBool("IsJump", true);
                    animator.SetBool("IsGround", false);
                    isGrounded = false;
                    velocity = rigidbody_self.velocity + transform.up * jump_height * 9.8f * 0.5f;
                }
            }
            else
            {
                key_h = 0f;
                key_v = 0f;
            }
        }
        else
        {
            animator.SetBool("IsGround", false);
            velocity += transform.up * -9.8f * 0.01f;
        } 

        Move_Animation(key_h, key_v);
        if (velocity != Vector3.zero)
        {
            velocity = new Vector3(Mathf.Lerp(velocity.x, 0f, Time.deltaTime), velocity.y, Mathf.Lerp(velocity.z, 0f, Time.deltaTime));
            if (Mathf.Abs(velocity.x) < 0.01f) velocity.x = 0f;
            if (Mathf.Abs(velocity.z) < 0.01f) velocity.z = 0f;
            transform.position = transform.position + velocity * Time.deltaTime;
        }
    }

    private void Move_Animation(float key_h, float key_v)
    {
        speed_h = input_key_sprint ? key_h * 2f : key_h;
        speed_v = input_key_sprint ? key_v * 2f : key_v;

        animator.SetFloat("Speed_H", Mathf.Lerp(animator.GetFloat("Speed_H"), speed_h, Time.deltaTime * 3f));
        animator.SetFloat("Speed_V", Mathf.Lerp(animator.GetFloat("Speed_V"), speed_v, Time.deltaTime * 3f));

        Vector3 move_direction = transform.forward * speed_v * animator.GetFloat("Movement_Speed");

        velocity.x = move_direction.x;
        velocity.z = move_direction.z;
    }
    private void Attack_Control()
    {
        if (!entity.is_stunned)
        {
            animator.SetBool("LR_Attack", is_L_down && is_R_down);
            animator.SetBool("L_Attack", is_L_down);
            animator.SetBool("R_Attack", is_R_down);
        }
        else
        {
            animator.SetBool("LR_Attack", false);
            animator.SetBool("L_Attack", false);
            animator.SetBool("R_Attack", false);
        }
    }

    public void On_Attack_Trigger_Enter()
    {
        animator.SetBool("Is_Attacking", true);
    }
    public void On_Attack_Trigger_Exit()
    {
        animator.SetBool("Is_Attacking", false);
    }

    private bool Check_Ground()
    {
        if (world.CheckForVoxel(new Vector3(transform.position.x - EntityWidth, transform.position.y + 0.05f, transform.position.z - EntityWidth)) ||
            world.CheckForVoxel(new Vector3(transform.position.x + EntityWidth, transform.position.y + 0.05f, transform.position.z - EntityWidth)) ||
            world.CheckForVoxel(new Vector3(transform.position.x + EntityWidth, transform.position.y + 0.05f, transform.position.z + EntityWidth)) ||
            world.CheckForVoxel(new Vector3(transform.position.x - EntityWidth, transform.position.y + 0.05f, transform.position.z + EntityWidth)) ||
            world.CheckForVoxel(new Vector3(transform.position.x - EntityWidth, transform.position.y - 0.05f, transform.position.z - EntityWidth)) ||
            world.CheckForVoxel(new Vector3(transform.position.x + EntityWidth, transform.position.y - 0.05f, transform.position.z - EntityWidth)) ||
            world.CheckForVoxel(new Vector3(transform.position.x + EntityWidth, transform.position.y - 0.05f, transform.position.z + EntityWidth)) ||
            world.CheckForVoxel(new Vector3(transform.position.x - EntityWidth, transform.position.y - 0.05f, transform.position.z + EntityWidth))
            )
        {
            if (world.CheckWaterForVoxel(new Vector3(transform.position.x - EntityWidth, transform.position.y + 0.05f, transform.position.z - EntityWidth)) ||
                world.CheckWaterForVoxel(new Vector3(transform.position.x + EntityWidth, transform.position.y + 0.05f, transform.position.z - EntityWidth)) ||
                world.CheckWaterForVoxel(new Vector3(transform.position.x + EntityWidth, transform.position.y + 0.05f, transform.position.z + EntityWidth)) ||
                world.CheckWaterForVoxel(new Vector3(transform.position.x - EntityWidth, transform.position.y + 0.05f, transform.position.z + EntityWidth)))
                return false;
            else return true;
        }
        else return false;
    }

    private bool front
    {
        get
        {
            if (world.CheckForVoxel(transform.position + transform.forward * EntityWidth + transform.up * EntityHeight * 0.3f) || world.CheckForVoxel(transform.position + transform.forward * EntityWidth + transform.up * EntityHeight))
            {
                if (world.CheckWaterForVoxel(transform.position + transform.forward * EntityWidth + transform.up * EntityHeight * 0.3f) || world.CheckWaterForVoxel(transform.position + transform.forward * EntityWidth + transform.up * EntityHeight)) return false;
                else return true;
            }
            else return false;
        }
    }
    private bool back
    {
        get
        {
            if (world.CheckForVoxel(transform.position - transform.forward * EntityWidth + transform.up * EntityHeight * 0.3f) || world.CheckForVoxel(transform.position - transform.forward * EntityWidth + transform.up * EntityHeight))
            {
                if (world.CheckWaterForVoxel(transform.position - transform.forward * EntityWidth + transform.up * EntityHeight * 0.3f) || world.CheckWaterForVoxel(transform.position - transform.forward * EntityWidth + transform.up * EntityHeight)) return false;
                else return true;
            }
            else return false;
        }
    }
    private bool left
    {
        get
        {
            if (world.CheckForVoxel(transform.position - transform.right * EntityWidth + transform.up * EntityHeight * 0.3f) || world.CheckForVoxel(transform.position - transform.right * EntityWidth + transform.up * EntityHeight))
            {
                if (world.CheckWaterForVoxel(transform.position - transform.right * EntityWidth + transform.up * EntityHeight * 0.3f) || world.CheckWaterForVoxel(transform.position - transform.right * EntityWidth + transform.up * EntityHeight)) return false;
                else return true;
            }
            else return false;
        }
    }
    private bool right
    {
        get
        {
            if (world.CheckForVoxel(transform.position + transform.right * EntityWidth + transform.up * EntityHeight * 0.3f) || world.CheckForVoxel(transform.position + transform.right * EntityWidth + transform.up * EntityHeight))
            {
                if (world.CheckWaterForVoxel(transform.position + transform.right * EntityWidth + transform.up * EntityHeight * 0.3f) || world.CheckWaterForVoxel(transform.position + transform.right * EntityWidth + transform.up * EntityHeight)) return false;
                else return true;
            }
            else return false;
        }
    }
    private bool down
    {
        get
        {
            if (world.CheckForVoxel(transform.position + transform.up * EntityHeight * 0.1f))
            {
                if (world.CheckWaterForVoxel(transform.position + transform.up * EntityHeight * 0.1f)) return false;
                else return true;
            }
            else return false;
        }
    }
}
