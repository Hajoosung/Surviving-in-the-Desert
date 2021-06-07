using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;
    //걷는 속도 변수 정의.

    [SerializeField]
    private float runSpeed;
    // 뛰는 속도 변수 정의

    private float applySpeed;
    // applyspeed 변수 정의

    [SerializeField]
    private float jumpForce;
    // 점프량 변수 정의

    [SerializeField]
    private float crouchSpeed;
    // 앉는 속도 변수 정의

    [SerializeField]
    private float lookSensitivity;
    //카메라의 민감도 변수 정의.


    private bool isRun = false;
    //달리는 상태 false로 둠.
    private bool isCrouch = false;
    //앉은 상태 false로 둠.
    private bool isGround = true;
    //땅에 닿인 상태 true로 둠.

    [SerializeField]
    private float crouchPosY;
    //앉은 위치 변수 설정
    private float originPosY;
    //일어선 위치 변수 설정
    private float applyCrouchPosY;



    private CapsuleCollider capsuleCollider;
    // capsuleCollider 변수 정의




    [SerializeField]
    private float cameraRotationLimit;
    //카메라의 한계 각도 변수 정의.
    private float currentCameraRotationX = 0;
    //카메라의 처음 한계 각도 변수 정의

    [SerializeField]
    private Camera theCamera;
    //카메라 사용을 위한 변수 정의

    private Rigidbody myRigid; 
    // rigidbody 사용을 위한 변수 정의 


    

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        //Player에 적용된 CapsuleCollider를 불러온다.
        myRigid = GetComponent<Rigidbody>();
        //Player에 적용된 rigidbody를 불러온다.
        applySpeed = walkSpeed;
        //applyspeed 를 걷는 속도로 설정;

        originPosY = theCamera.transform.localPosition.y;
        //서있는 위치는 카메라의 y좌표 위치
        applyCrouchPosY = originPosY;
        //기본위치는 서있는 위치
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        //IsGround() 함수 호출
        TryJump();
        //TryJump() 함수 호출
        TryRun();
        //TryRun() 함수 호출
        TryCrouch();
        //TryCrouch() 함수 호출
        Move();
        //Move 함수 호출
        CameraRotation();
        //CameraRotation 함수 호출

        CharacterRotation();
        //CharacterRotation 함수 호출

        

    }


    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        //LeftControl 키를 누른다면
        {
            Crouch();
            // Crouch 함수 호출
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;
        //isCrouch는 true 이다.

        if (isCrouch)
        //isCrouch는 true 이면
        {
            applySpeed = crouchSpeed;
            //속도는 crouchSpeed 변수로 바뀐다
            applyCrouchPosY = crouchPosY;
            // applyCrouchPosY 값은 crouchPosY로 적용
        }
        else
        {
            applySpeed = walkSpeed;
            //속도는 walkSpeed 변수로 바뀐다
            applyCrouchPosY = originPosY;
            // applyCrouchPosY 값은 originPosY로 적용

            
        }

        StartCoroutine(CrouchCoroutine());
        //CrouchCoroutine 함수를 호출


    }

    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        // _posY는 카메라의 y좌표 위치로 변수 정의
        int count = 0;
        //count 변수 정의

        while (_posY != applyCrouchPosY)
        //_posY값이 applyCrouchPosY 값과 다르면
        {
            count++;
            // count는 계속 증가
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.1f);
            // _posY값을 applyCrouchPosY 값까지 자연스럽게 변경
            theCamera.transform.localPosition = new Vector3(0, _posY, -1.8f);
            // 카메라의 y좌표를 _posY 값(applyCrouchPosY 값)으로 변경

            if (count > 15)
                //만약 count 값이 15를 넘어가면
                break;
            // 이 반복문을 빠져나옴
            yield return null;
            //프레임 대기(_posY값을 applyCrouchPosY 값까지 자연스럽게 변경이 끝날때 까지 반복하게 하는 문장)
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, -1.8f);
        // 카메라 Y좌표는 applyCrouchPosY 값을 적용

    }
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        //Player 캡슐 콜라이더의 반 만큼(+0.1f) 레이저를 쏘은 위치
    }
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
            //만약 스페이스바가 한번 눌리고, 땅에 있을 경우
        {
            Jump();
            //Jump함수를 호출한다
        }
    }

    private void Jump()
    {
        if (isCrouch)
            Crouch();

        myRigid.velocity = transform.up * jumpForce;
        // 점프량과 위치를 곱한 만큼 점프한다.
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        //만약 LeftShift를 누르면
        {
            Running();
            //달린다
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        //만약 LeftShift키를 떼면
        {
           RunningCancel();
            //달리지 않음
        }
    }

    private void Running()
    {
    
        isRun = true;
        // 달리는 상태를 true로 둠
        applySpeed = runSpeed;
        //applySpeed 를 뛰는 속도로 설정
    }
 private void RunningCancel()
    {
        isRun = false;
        // 달리는 상태를 false로 둠

        applySpeed = walkSpeed;
        //applySpeed 를 걷는 속도로 설정

    }
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        // 좌우 방향키를 입력하면 오른쪽 방향키 = 1, 왼쪽 방향키 = -1이 되도록 _moveDirX 변수 정의
        float _moveDirZ = Input.GetAxisRaw("Vertical");
        // 위아래 방향키를 입력하면 위 방향키 = 1, 아래 방향키 = -1이 되도록 _moveDirZ 변수 정의
        
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        // 좌우로 움직이도록 _moveHorizontal의 백터값 정의
        Vector3 _moveVertical = transform.forward * _moveDirZ;
        // 위아래로 움직이도록 _moveHorizontal의 백터값 정의

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
        // 좌우로 움직이도록 _moveHorizontal의 백터값 정의

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }


    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        // 마우스의 x축 값이 _yRotation이 되도록 변수 정의
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        //_yRotation 값에 카메라 민감도를 곱한 값을 _characterRotationY로 정의
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
        // _characterRotationY 값을 Quaternion으로 변환시킨 값과 나의 회전 위치 값을 곱한다.

    }

    private void CameraRotation()
    {
        // 상하 카메라 회전
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        // 마우스의 y축 값이 _xRotation이 되도록 변수 정의

        float _cameraRotationX = _xRotation * lookSensitivity;
        //_xRotation 값에 카메라 민감도를 곱한 값을 _cameraRotationX로 정의
        currentCameraRotationX -= _cameraRotationX;
        //현재 카메라의 X축 회전값은 _cameraRotationX의 값만큼 작아짐 
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        //현재 카메라의 X축 회전값은 카메라 로테이션  절대값을 넘지 않게 함.
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        //실제 카메라의 위치정보는 currentCameraRotationX의 값을 가져옴.
    }

}
