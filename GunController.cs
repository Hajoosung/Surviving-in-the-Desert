using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;
    // currentGun 정의

    private float currentFireRate;
    //currentFireRate 변수 정의

    private bool isReload = false;
    // isReload false로 둠
    

    [SerializeField]
    private Vector3 originPos;
    //본래 포지션 값

    private AudioSource audioSource;
    //AudioSource 변수 정의

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //audioSource 컴포넌트 가져오기
        originPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        // GunFireRateCalc 함수 호출
        TryFire();
        // TryFire 함수 호출
        TryReload();
        //TryReload 함수 호출
       
    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            // 만약 currentFireRate가 0보다 크면
            currentFireRate -= Time.deltaTime;
        // currentFireRate를 1초에 1씩 감소시킴(Gun 쿨타임 지정)
    }
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        // 만약 왼쪽 마우스 버튼을 누르고 currentFireRate가 0 이하이고 장전이 되있으면
        {
            Fire();
            //Fire 함수 호출
        }
    }

    private void Fire()
    {
        if (!isReload)
            //재장전이 false이면
        {

            if (currentGun.currentBulletCount > 0)
                //만약 총알이 있으면,
                Shoot();
            //Shoot 함수 호출
            else
            {
                StartCoroutine(ReloadCoroutine());
                //ReloadCoroutine 함수 호출
            }

        }
    }

    private void Shoot()
    {
        currentGun.currentBulletCount--;
        // 총알 소모시 currentBulletCount -1
        currentFireRate = currentGun.fireRate;
        // 연사 속도 재계산.
        currentGun.muzzleFlash.Play();
        // Gun 이펙트 실행
        PlaySE(currentGun.fire_Sound);
        // 총 소리 실행

    }

    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
            //만약 r키를 누르고, 재장전이 false이며, 재장전 탄알수보다 현재 탄알수가 작다면

            StartCoroutine(ReloadCoroutine());
        //ReloadCoroutine 함수 호출
    }

    IEnumerator ReloadCoroutine()
    {
        if( currentGun.carryBulletCount > 0)
            //만약 현재 총알 개수가 0개 이상이면,
        {

            isReload = true;
            //재장전을 true로 바꿈

            currentGun.anim.SetTrigger("Reload");
            //Reload 애니메이션 실행

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            //재장전하면 탄알집의 탄알 개수가 소유한 탄알개수에 더해짐
            currentGun.currentBulletCount = 0;
            //탄알집의 총알개수는 0

            yield return new WaitForSeconds(currentGun.reloadTime);
            // 재장전 대기시간 설정




            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
                // 만약 현재 소유한 총알 개수가 재장전 총알개수보다 많거나 같으면
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                //현재 탄알집의 총알개수는 재장전 총알개수와 같게 된다.
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
                //현재 총알개수에서 재장전 총알개수를 뺀다.
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                // 현재 탄알집의 총알개수는 소유한 총알개수에 대입시킨다.
                currentGun.carryBulletCount = 0;
                //소유한 총알개수를 0으로 한다.


            }

            isReload = false;
            //재장전을 false로 바꿈
        }
    }

   

   


    IEnumerator FineSightActivateCoroutine()
    {
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            //현재 총위치가 정조준 총위치와 같지 않으면
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            // 현재 총위치 값을 정조준 총위치 값까지 자연스럽게 변경
            yield return null;
            //프레임 대기
        }
    }

    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        //현재 총위치가 원래 총위치와 같지 않으면
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            //현재 총위치 값을 원래 총위치 값까지 자연스럽게 변경

            yield return null;
            //프레임 대기
        }
    }


    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        //audioSource.clip은 _clip을 가져다 씀
        audioSource.Play();
        // 총 소리 실행
    }

}
