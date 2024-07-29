using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity_Data;

public class Entity_Humanoid : Entity
{
    [SerializeField] private Part L_Hand;
    [SerializeField] private Part R_Hand;

    [SerializeField] private Inventory inventory = null;
    [SerializeField] private ItemComponent L_held_data, R_held_data, helmet_data, chestplate_data, leggings_data, boots_data;

    private void Awake()
    {
        TryGetComponent(out inventory);
        Awake_Default();
        L_Hand = gameObject.transform.Find("SimplePlayer.arma/center/Body/Chest/Arm:Left:Upper/Arm:Left:Lower/Arm:Left:Hand").gameObject.GetComponent<Part>();
        R_Hand = gameObject.transform.Find("SimplePlayer.arma/center/Body/Chest/Arm:Right:Upper/Arm:Right:Lower/Arm:Right:Hand").gameObject.GetComponent<Part>();
        Update_Status_Humanoid();
    }

    public void Update_Status_Humanoid()
    {
        if(inventory != null)
        {
            if (inventory.Equipment_Slot[0] != null)
            {
                helmet_data = inventory.Equipment_Slot[0];
                weight_current = weight_current + helmet_data.weight;
                defense_current = defense_current + helmet_data.armorDefense;
            }
            else helmet_data = null;
            if (inventory.Equipment_Slot[1] != null)
            {
                chestplate_data = inventory.Equipment_Slot[1];
                weight_current = weight_current + chestplate_data.weight;
                defense_current = defense_current + chestplate_data.armorDefense;
            }
            else chestplate_data = null;
            if (inventory.Equipment_Slot[2] != null)
            {
                leggings_data = inventory.Equipment_Slot[2];
                weight_current = weight_current + leggings_data.weight;
                defense_current = defense_current + leggings_data.armorDefense;
            }
            else leggings_data = null;
            if (inventory.Equipment_Slot[3] != null)
            {
                boots_data = inventory.Equipment_Slot[3];
                weight_current = weight_current + boots_data.weight;
                defense_current = defense_current + boots_data.armorDefense;
            }
            else boots_data = null;

            if (inventory.Equipment_Slot[4] != null)
            {
                L_held_data = inventory.Equipment_Slot[4];
                L_Hand.Set_Value_Melee(attack_damage_base + L_held_data.meleeDamage, attack_damage_max_rate, attack_damage_min_rate, L_held_data.meleeSpeed, attack_speed_rate, L_held_data.toolTier);
                L_Hand.gameObject.transform.Find("Bare_Hand").gameObject.SetActive(false);
            }
            else
            {
                L_held_data = null;
                L_Hand.Set_Value_Melee(attack_damage_base, attack_damage_max_rate, attack_damage_min_rate, 1f, attack_speed_rate, 1);
                L_Hand.gameObject.transform.Find("Bare_Hand").gameObject.SetActive(true);
            }

            if (inventory.Equipment_Slot[5] != null)
            {
                R_held_data = inventory.Equipment_Slot[5];
                R_Hand.Set_Value_Melee(attack_damage_base + R_held_data.meleeDamage, attack_damage_max_rate, attack_damage_min_rate, R_held_data.meleeSpeed, attack_speed_rate, R_held_data.toolTier);
                R_Hand.gameObject.transform.Find("Bare_Hand").gameObject.SetActive(false);
            }
            else
            {
                R_held_data = null;
                R_Hand.Set_Value_Melee(attack_damage_base, attack_damage_max_rate, attack_damage_min_rate, 1f, attack_speed_rate, 1);
                R_Hand.gameObject.transform.Find("Bare_Hand").gameObject.SetActive(true);
            }



            if (L_held_data != null && R_held_data != null)
            {
                if (L_held_data.equipmentType == "SHIELD")
                {
                    animator.SetInteger("Moveset_Number", 1);
                    animator.SetFloat("LR_Attack_Speed", R_Hand.attack_speed);
                }
                else if (R_held_data.equipmentType == "SHIELD")
                {
                    animator.SetInteger("Moveset_Number", -1);
                    animator.SetFloat("LR_Attack_Speed", L_Hand.attack_speed);
                }
                else if (L_held_data.equipmentType.Contains("ONE_HANDED") && R_held_data.equipmentType.Contains("ONE_HANDED"))
                {
                    animator.SetInteger("Moveset_Number", 2);
                    animator.SetFloat("LR_Attack_Speed", (L_Hand.attack_speed + R_Hand.attack_speed) / 2f);
                }
            }
            else if (L_held_data != null && R_held_data == null)
            {
                if (L_held_data.equipmentType.Contains("ONE_HANDED"))
                {
                    animator.SetInteger("Moveset_Number", -1);
                    animator.SetFloat("LR_Attack_Speed", L_Hand.attack_speed);
                }
                else if (L_held_data.equipmentType.Contains("TWO_HANDED"))
                {
                    animator.SetInteger("Moveset_Number", -3);
                    animator.SetFloat("LR_Attack_Speed", L_Hand.attack_speed);
                }
                else if (L_held_data.equipmentType == "BOW")
                {
                    animator.SetInteger("Moveset_Number", -4);
                    animator.SetFloat("Draw_Speed", L_Hand.draw_speed);
                }
            }
            else if (L_held_data == null && R_held_data != null)
            {
                if (R_held_data.equipmentType.Contains("ONE_HANDED"))
                {
                    animator.SetInteger("Moveset_Number", 1);
                    animator.SetFloat("LR_Attack_Speed", R_Hand.attack_speed);
                }
                else if (R_held_data.equipmentType.Contains("TWO_HANDED"))
                {
                    animator.SetInteger("Moveset_Number", 3);
                    animator.SetFloat("LR_Attack_Speed", R_Hand.attack_speed);
                }
                else if (R_held_data.equipmentType == "BOW")
                {
                    animator.SetInteger("Moveset_Number", 4);
                    animator.SetFloat("Draw_Speed", R_Hand.draw_speed);
                }
            }
            else
            {
                animator.SetInteger("Moveset_Number", 10);
                animator.SetFloat("LR_Attack_Speed", (L_Hand.attack_speed + R_Hand.attack_speed) / 2f);
            }

            switch (animator.GetInteger("Moveset_Number"))
            {
                case 10:
                case -10:
                    guard_rate = 0.5f;
                    break;
                case 1:
                    guard_rate = L_held_data.guardRate;
                    break;
                case -1:
                    guard_rate = R_held_data.guardRate;
                    break;
                case 2:
                case -2:
                    guard_rate = (L_held_data.guardRate + R_held_data.guardRate) / 2f;
                    break;
                case 3:
                    guard_rate = R_held_data.guardRate;
                    break;
                case -3:
                    guard_rate = L_held_data.guardRate;
                    break;
                case 4:
                case -4:
                    guard_rate = 0f;
                    break;
            }
        }
        else
        {
            animator.SetInteger("Moveset_Number", 10);
            L_Hand.Set_Value_Melee(attack_damage_base, attack_damage_max_rate, attack_damage_min_rate, 1f, attack_speed_rate, 1);
            R_Hand.Set_Value_Melee(attack_damage_base, attack_damage_max_rate, attack_damage_min_rate, 1f, attack_speed_rate, 1);
            animator.SetFloat("LR_Attack_Speed", (L_Hand.attack_speed + R_Hand.attack_speed) / 2f);
            guard_rate = 0.5f;
        }
        L_Hand.Set_Collider();
        R_Hand.Set_Collider();

        movement_speed = Mathf.Max(0.1f, movement_speed - weight_rate);

        animator.SetFloat("L_Attack_Speed", L_Hand.attack_speed);
        animator.SetFloat("R_Attack_Speed", R_Hand.attack_speed);
        animator.SetFloat("Movement_Speed", movement_speed);
    }

    public void On_L_Hand_Collider()
    {
        if (animator.GetInteger("Moveset_Number") > 0)
        {
            if (L_Hand.Is_Collider_On_Off()) L_Hand.Collider_On_Off(false);
            else L_Hand.Collider_On_Off(true);
        }
        else if (animator.GetInteger("Moveset_Number") < 0)
        {
            if (R_Hand.Is_Collider_On_Off()) R_Hand.Collider_On_Off(false);
            else R_Hand.Collider_On_Off(true);
        }
    }
    public void On_R_Hand_Collider()
    {
        if (animator.GetInteger("Moveset_Number") > 0)
        {
            if (R_Hand.Is_Collider_On_Off()) R_Hand.Collider_On_Off(false);
            else R_Hand.Collider_On_Off(true);
        }
        else if (animator.GetInteger("Moveset_Number") < 0)
        {
            if (L_Hand.Is_Collider_On_Off()) L_Hand.Collider_On_Off(false);
            else L_Hand.Collider_On_Off(true);
        }
    }
    public void Reset_Hand_Collider()
    {
        L_Hand.Collider_On_Off(false);
        R_Hand.Collider_On_Off(false);
    }
}