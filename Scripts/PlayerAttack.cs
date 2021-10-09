using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private void Attack()
    {
        PlayerMover.instance.PlayerAnimator.SetTrigger(GameData.ANIMATOR_ATTACK_TRIGGER);

        Player.instance.WeaponPlaceTransform.transform.GetChild(0).gameObject.GetComponent<Weapon>().IsReady = true;
    }
}
