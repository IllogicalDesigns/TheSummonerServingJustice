using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour
{
    NormalMove normalMove;

    [SerializeField] CinemachineVirtualCamera virtualCamera;

    [SerializeField] CinemachineVirtualCamera virtualCameraLeft;
    [SerializeField] CinemachineVirtualCamera virtualCameraRight;
    [SerializeField] CinemachineVirtualCamera virtualCameraSlide;

    [Space]
    [SerializeField] Transform groupHolder;
    [Space]
    [SerializeField] Vector3 jumpPunch, doubleJumpPunch;
    [SerializeField] float jumpPunchduration;
    [Space]
    [SerializeField] Vector3 landPunch;
    [SerializeField] float landPunchduration;

    Tween tween;
    bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        normalMove = GetComponent<NormalMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if(normalMove == null) {
            normalMove = GetComponent<NormalMove>();
            return;
        }

        virtualCamera.gameObject.SetActive(false);
        virtualCameraLeft.gameObject.SetActive(false);
        virtualCameraRight.gameObject.SetActive(false);
        virtualCameraSlide.gameObject.SetActive(false);

        if (normalMove.currentState == NormalMove.MovementState.WallRunning) {
            if(normalMove.wallRunLeft) {
                virtualCameraLeft.gameObject.SetActive(true);
            } else {
                virtualCameraRight.gameObject.SetActive(true);
            }
        } else if(normalMove.currentState == NormalMove.MovementState.Sliding) {
            virtualCameraSlide.gameObject.SetActive(true);
        }
        else {
            if(Input.GetKey(KeyCode.Q)) {
                virtualCameraRight.gameObject.SetActive(true);
            }
            else if(Input.GetKey(KeyCode.E)) {
                virtualCameraLeft.gameObject.SetActive(true);
            }
            else
            {
                virtualCamera.gameObject.SetActive(true);
            }
        }
    }

    public void OnJump(int count) {
        isJumping = true;



        //if (count == 1) {
        //    if (tween != null) tween.Kill(true);
        //    tween = groupHolder.DOLocalMoveY(jumpPunch.y, jumpPunchduration).SetLoops(1);
        //}
        //else {
        //    if (tween != null) tween.Kill(true);
        //    tween = groupHolder.DOLocalMoveY(doubleJumpPunch.y, jumpPunchduration).SetLoops(1);
        //}
    }

    public void OnLand() {
        if (isJumping) {
            if (tween != null) tween.Kill(true);
            isJumping = false;
            tween = groupHolder.DOPunchPosition(landPunch, landPunchduration);
        }
    }
}
