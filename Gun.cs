using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName; 
    // 총의 이름 정의
    public float range;
    // 사정 거리 정의
    public float accuracy;
    // 정확도 정의
    public float fireRate;
    // 연사속도. 정의
    public float reloadTime;
    // 재장전 속도. 정의
    public float KnifeTime;

    public int damage;
    // 총의 데미지 정의

    public int reloadBulletCount;
    // 총알 재정전 개수 정의
    public int currentBulletCount;
    // 현재 탄알집에 남아있는 총알의 개수 정의
    public int maxBulletCount;
    // 최대 소유 가능 총알 개수 정의
    public int carryBulletCount;
    // 현재 소유하고 있는 총알 개수 정의

    public float retroActionForce;
    // 반동 세기 정의
    public float retroActionFineSightForce; 
    // 정조준시의 반동 세기 정의

    public Vector3 fineSightOriginPos;
    // 정조준선 위치 정의
    public Animator anim; 
    // 애니메이션 정의
    public ParticleSystem muzzleFlash;
    // 총구 파티클이펙트 정의

    public AudioClip fire_Sound;
    //총 소리 정의
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
