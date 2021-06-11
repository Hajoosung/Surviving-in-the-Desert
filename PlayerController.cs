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

    [SerializeField] private float swimSpeed;
    [SerializeField] private float swimFastSpeed;
    [SerializeField] private float upswimSpeed;




    private float applySpeed;
    // applyspeed 변수 정의

    [SerializeField]
    private float jumpForce;
    // 점프량 변수 정의

    // 상태 변수

    private Vector3 lastPos;
    // 움직임 체크 변수


    
    // 앉는 속도 변수 정의

    [SerializeField]
    private float lookSensitivity;
    //카메라의 민감도 변수 정의.

    private bool isWalk = false;

    private bool isRun = false;
    //달리는 상태 false로 둠.
    private bool isCrouch = false;
    //앉은 상태 false로 둠.
    private bool isGround = true;
    //땅에 닿인 상태 true로 둠.

    
    private float originPosY;
    //일어선 위치 변수 설정
    



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

    private GunController theGunController;
    private Crosshair theCrosshair;
    private StatusController theStatusController;


    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        //Player에 적용된 CapsuleCollider를 불러온다.
        myRigid = GetComponent<Rigidbody>();
        //Player에 적용된 rigidbody를 불러온다.
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>();
        theStatusController = FindObjectOfType<StatusController>();



        applySpeed = walkSpeed;
        //applyspeed 를 걷는 속도로 설정;
        originPosY = theCamera.transform.localPosition.y;
        //서있는 위치는 카메라의 y좌표 위치
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.canPlayMove) 
        {
            WaterCheck();
            IsGround();
            //IsGround() 함수 호출
            TryJump();
            //TryJump() 함수 호출
            if(!GameManager.isWater)
            {
                TryRun();
                //TryRun() 함수 호출    
            }

            Move();
            //Move 함수 호출
            MoveCheck();

            if (!Inventory.inventoryActivated)
            {
                CameraRotation();
                //CameraRotation 함수 호출
                CharacterRotation();
                //CharacterRotation 함수 호출
            }
        }

    } 


    private void WaterCheck()
    {
        if(GameManager.isWater)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift))
            applySpeed = swimFastSpeed;
            else
                applySpeed = swimSpeed;

        }
    }

    
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        //Player 캡슐 콜라이더의 반 만큼(+0.1f) 레이저를 쏘은 위치

        theCrosshair.RunningAnimation(!isGround);
    }
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSP() > 0 && !GameManager.isWater)
        //만약 스페이스바가 한번 눌리고, 땅에 있을 경우
        Jump();
        
        //Jump함수를 호출한다.
        else if (Input.GetKey(KeyCode.Space) && GameManager.isWater)
        
            UpSwim();
       
                

            
        }
    
    private void UpSwim()
    {
        myRigid.velocity = transform.up * upswimSpeed;
    }

    private void Jump()
    {
       

        myRigid.velocity = transform.up * jumpForce;
        // 점프량과 위치를 곱한 만큼 점프한다.

        theStatusController.DecreaseStamina(100);
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && theStatusController.GetCurrentSP() > 0)
        //만약 LeftShift를 누르면
        {
            Running();
            //달린다
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || theStatusController.GetCurrentSP() <= 0)
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
        
        theCrosshair.RunningAnimation(isRun);

        applySpeed = runSpeed;
        //applySpeed 를 뛰는 속도로 설정

        theStatusController.DecreaseStamina(10);

    }
    private void RunningCancel()
    {
        isRun = false;
        // 달리는 상태를 false로 둠
        theCrosshair.RunningAnimation(isRun);
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
   
    
    // 움직임 체크
    private void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
                isWalk = true;
            else
                isWalk = false;

            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
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
