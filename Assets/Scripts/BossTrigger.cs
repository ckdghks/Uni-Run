using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
        if (other.tag == "Player")
        {
            Debug.Log("Boss Hit!");
            //GameManager.instance.AddScore(1);

            GameManager.instance.Sub_BLife(1);
            if (GameManager.instance.isBossDead == true)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
