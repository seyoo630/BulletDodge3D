using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody playerRigidbody;
    Animator playerAnim;

    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float rayDistance = 1.0f; // 레이캐스트 거리

    private void Awake()
    {
        playerRigidbody = this.GetComponent<Rigidbody>();
        playerAnim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveHorizontal, 0, moveVertical).normalized;

        if (moveDirection != Vector3.zero)
        {
            playerAnim.SetBool("run", true);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            playerRigidbody.rotation = Quaternion.Lerp(playerRigidbody.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 앞에 벽이 있는지 Raycast로 확인
            if (!Physics.Raycast(playerRigidbody.position, moveDirection, rayDistance))
            {
                Vector3 movement = moveDirection * moveSpeed * Time.deltaTime;
                playerRigidbody.MovePosition(playerRigidbody.position + movement);
            }
            else
            {
                playerRigidbody.velocity = Vector3.zero;
            }
        }
        else
        {
            playerAnim.SetBool("run", false);
            playerRigidbody.velocity = Vector3.zero;
        }
    }


    private void OnDrawGizmos()
    {
        // Scene 뷰에서 Raycast를 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
    }
}
