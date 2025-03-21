using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity_Data;

public class Part : MonoBehaviour
{
    [SerializeField] public float damage_max { get; private set; }
    [SerializeField] public float damage_min { get; private set; }
    [SerializeField] public float attack_speed { get; private set; }

    [SerializeField] public float draw_power { get; private set; }
    [SerializeField] public float draw_speed { get; private set; }
    [SerializeField] public float aim_accuracy { get; private set; }

    public int tool_tier { get; private set; }

    [SerializeField] private Collider collider = null;
    private Dictionary<GameObject, bool> victim_dictionary = new Dictionary<GameObject, bool>();
    [SerializeField] private GameObject self;

    private void Awake()
    {
        self = transform.GetComponentInParent<Entity>().gameObject;
    }

    public void Set_Value_Melee(float melee_damage, float attack_damage_max_rate, float attack_damage_min_rate, float melee_speed, float attack_speed_rate, int tool_tier)
    {
        damage_max = melee_damage * attack_damage_max_rate;
        damage_min = damage_max * attack_damage_min_rate;
        attack_speed = melee_speed * attack_speed_rate;
        this.tool_tier = tool_tier;
    }
    public void Set_Value_Range(float draw_power, float draw_power_rate, float draw_speed, float draw_speed_rate, float aim_accuracy, float aim_accuracy_rate)
    {
        this.draw_power = draw_power * draw_power_rate;
        this.draw_speed = draw_speed * draw_speed_rate;
        this.aim_accuracy = aim_accuracy * aim_accuracy_rate;
    }

    public void Set_Collider()
    {
        collider = GetComponentInChildren<Collider>();
        Collider_On_Off(false);
    }

    public void Collider_On_Off(bool on_off)
    {
        collider.enabled = on_off;
        if (!on_off) victim_dictionary.Clear();
    }
    public bool Is_Collider_On_Off()
    {
        return collider.enabled;
    }

    private void OnTriggerStay(Collider victim)
    {
        if (victim.CompareTag("Entity"))
        {
            if (victim.gameObject != self && (!victim_dictionary.ContainsKey(victim.gameObject) || !victim_dictionary[victim.gameObject]))
            {
                victim_dictionary[victim.gameObject] = true;
                victim.gameObject.GetComponent<Entity>().On_Hit(Random.Range(damage_min, damage_max), collider);
            }
        }
    }

    public void Force_Trigger(Collider victim)
    {
        OnTriggerStay(victim);
    }
}