using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffController : MonoBehaviour
{
    Collider staffColl;
    public static bool isStaffInHitt = false;

    public ParticleSystem hitParticlesAnim;
    public Transform staffType;
    Transform tr;
    MeleeWeaponTrail trail;

    public StaffSounds staffSounds;
    bool isCoinsInst;
    float barrelAnimTime;
    float currBarrelAnimTime;
    List<GameObject> coinsList;
    GameObject coin;
    Vector3 barrelPos;

    private void OnEnable()
    {
        tr = GetComponent<Transform>();
        trail = GetComponent<MeleeWeaponTrail>();
        trail.Emit = false;
        coin = Resources.Load("coin5") as GameObject;
        staffColl = GetComponent<Collider>();
        coinsList = new List<GameObject>();
        barrelAnimTime = 1.0f;
        EventsManager.StartListening(EventsIds.STAFF_START_HIT, StartStaffHit);
        EventsManager.StartListening(EventsIds.STAFF_END_HIT, EndStaffHit);
    }

    private void OnDisable()
    {
        EventsManager.StopListening(EventsIds.STAFF_START_HIT, StartStaffHit);
        EventsManager.StopListening(EventsIds.STAFF_END_HIT, EndStaffHit);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isStaffInHitt)
            return;

        Debug.Log("OnTrriggerStaff");
        if (other.tag == "Hitable")
        {
            other.gameObject.SetActive(false);
            PlayHitSound();
            PlayHitAnim();
            return;
        }

        if (other.tag == "Enemy")
        {
            IEnemy enemy = other.GetComponent<IEnemy>();
            PlayHitSound();
            enemy.GetHit();
            return;
        }

        if (other.tag == "Coin_5")
        {
            PlayHitAnim();
            PlayHitSound();
            other.gameObject.SetActive(false);
            return;
        }
    }

    void PlayHitAnim()
    {
        hitParticlesAnim.transform.position = staffType.position;
        hitParticlesAnim.Play(); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("StaffCollision");
    }
    void StartStaffHit()
    {
        isStaffInHitt = true;
        staffColl.enabled = true;
        MoveForward();
    }

    void EndStaffHit()
    {
        isStaffInHitt = false;
        staffColl.enabled = false;
        MoveBack();
    }

    public void MoveForward()
    {
        Vector3 localPos = tr.localPosition;
        localPos.z = -0.0084f;
        tr.localPosition = localPos;
        trail.Emit = true;
    }

    public void MoveBack()
    {
        trail.Emit = false;
        Vector3 localPos = tr.localPosition;
        localPos.z = -0.0001849903f;
        tr.localPosition = localPos;
        //-0.0001849903 tr.localPosition -= tr.forward;
    }

    void PlayHitSound()
    {
        if (!GameSystem.isSoundEnabled)
            return;

      StaffSounds.instance.StaffHit();

    }
}
