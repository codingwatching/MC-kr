using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Equipment_Type
{
    HELMET,
    CHESTPLATE,
    LEGGINGS,
    BOOTS,
    


    SHIELD,

    DAGGER,

    SWORD_LIGHT,
    SWORD_HEAVY,
    
    AXE_LIGHT,
    AXE_HEAVY,
    
    BLUNT_LIGHT,
    BLUNT_HEAVY,

    SPEAR,

    BOW,



    PICKAXE,
    SHOVEL,
    HOE
}



public class Item_Data
{
    public string item_ID; //아이템 아이디
    public string item_name; //아이템 이름
    public GameObject item_model_in_inventory; //인벤토리에서 보여질 아이템의 형태
    public GameObject item_model_in_hand; //손에 있을 때 보여질 아이템의 형태
}



public class Item_Stackable_Data : Item_Data
{
    public int stack_max; //겹칠 수 있는 최대 개수
}

public class Item_Placeable_Data : Item_Stackable_Data
{
    public Equipment_Type require_tool_type; //해당 블럭을 파괴하기 위해 필요한 도구의 타입
    public int require_tool_tier; //해당 블럭을 파괴하기 위해 필요한 도구의 티어
    public float durability_max; //해당 블럭의 체력
    public float durability_current;
    public GameObject item_model_in_world; //설치되었을 때 보여질 아이템의 형태 (근데 이건 텍스쳐로 해놔야 되나)
}

public class Item_Function_Data : Item_Placeable_Data
{
    //상호작용이 가능한 블럭의 기능은 이벤트로 만들어두고 추가하는 식으로?
}

public class Item_Eatable_Data : Item_Stackable_Data
{
    public float hunger_amount; //먹었을 때 회복되는 배고픔 수치
    public float thirst_amount; //먹었을 때 회복되는 목마름 수치
    public float fatigue_amount; //먹었을 때 회복되는 피로도 수치

    public float freshness_max; //최대 유통기한
    public float freshness_current; //현재 유통기한
}

public class Item_Material_Data : Item_Stackable_Data
{
    //추가할 게... 있나...?
}



public class Item_Unstackable_Data : Item_Data
{
    //추가할 게... 있나...?
}

public class Item_Equipment_Data : Item_Unstackable_Data
{
    public Equipment_Type equipment_type; //장비의 타입
    public float weight; //장비의 무게
    public float durability_max; //장비의 최대 내구도
    public float durability_current; //장비의 현재 내구도
}

public class Item_Armor_Data : Item_Equipment_Data
{
    public float armor_defense; //방어구의 방어력
    public GameObject item_model_in_equip; //방어구 장착 시의 모델링
}

public class Item_Gear_Data : Item_Equipment_Data
{
    public float attack_damage; //무기 또는 도구의 공격력
    public float attack_speed; //무기 또는 도구의 공격속도
    public float attack_reach; //무기 또는 도구의 공격거리 (만약 모델링에 collider 붙여서 처리할거면 필요없음)
    public float guard_rate; //무기 또는 도구의 방어배율

    public string[] actions; // 무기 또는 도구가 가진 애니메이션

    public int tool_tier; //무기 또는 도구의 티어 (블럭 파괴 외에도 제작 스테이션 등에서 사용할수도?)
}

public class item_Bow_Data : Item_Gear_Data
{
    public GameObject arrow; //소모할 화살
}