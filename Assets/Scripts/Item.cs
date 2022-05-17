using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
        if (other.tag == "Player")
        {
            // Debug.Log("Item!!");
            GameManager.instance.AddScore(2);
            GameManager.instance.SubLife(-1); // 오히려 더하게 한다...
            gameObject.SetActive(false);
        }

    }
}

