using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooter : MonoBehaviour
{
    private Camera cam;
    private AudioSource shootingAudioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;

    private int currentAmmo = 6;
    private int maxAmmo = 6;
    private int totalClips = 3;

    private bool isGameActive = true;
    [SerializeField] private Animator animator;
    private bool isShooting = false;
    private bool isReloading = false;
    private float originalVolume; // 新增

    void Start()
    {
        cam = GetComponent<Camera>();
        shootingAudioSource = GetComponent<AudioSource>(); // 获取当前游戏对象上的AudioSource组件

        if (shootingAudioSource != null)
        {
            originalVolume = shootingAudioSource.volume; // 保存原始音量
        }
    }

    void Awake()
    {
        Messenger.AddListener(GameEvent.GAME_ACTIVE, OnGameActive);
        Messenger.AddListener(GameEvent.GAME_INACTIVE, OnGameInactive);
        Messenger.AddListener(GameEvent.MUTE_ALL_SOUNDS, OnMuteAllSounds);
        Messenger.AddListener(GameEvent.UNMUTE_ALL_SOUNDS, OnUnmuteAllSounds); // 新增
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.GAME_ACTIVE, OnGameActive);
        Messenger.RemoveListener(GameEvent.GAME_INACTIVE, OnGameInactive);
        Messenger.RemoveListener(GameEvent.MUTE_ALL_SOUNDS, OnMuteAllSounds);
        Messenger.RemoveListener(GameEvent.UNMUTE_ALL_SOUNDS, OnUnmuteAllSounds); // 新增
    }

    void LateUpdate()
    {
        if (!isGameActive || isShooting || isReloading)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                Debug.Log("No ammo. Press R to reload.");
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void Shoot()
    {
        isShooting = true;
        PlayShootSound();
        animator.SetBool("shot", true);

        Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
        Ray ray = cam.ScreenPointToRay(point);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
            if (target != null)
            {
                target.ReactToHit();
            }
            else
            {
                StartCoroutine(CreateTempSphereIndicator(hit.point));
            }

            currentAmmo--;
            Messenger<int>.Broadcast(GameEvent.AMMO_CHANGED, currentAmmo);
            Debug.Log("Remaining Ammo: " + currentAmmo);
        }
        StartCoroutine(ResetShotAnimation());
    }

    private IEnumerator ResetShotAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("shot", false);
        isShooting = false;
    }

    private void Reload()
    {
        if (totalClips > 0 && currentAmmo < maxAmmo)
        {
            isReloading = true;
            PlayReloadSound();
            animator.SetBool("reload", true);

            totalClips--;
            currentAmmo = maxAmmo;
            Messenger<int>.Broadcast(GameEvent.AMMO_CHANGED, currentAmmo);
            Messenger<int>.Broadcast(GameEvent.CLIPS_CHANGED, totalClips);
            Debug.Log("Reloaded. Clips left: " + totalClips);

            StartCoroutine(ResetReloadAnimation());
        }
        else
        {
            Debug.Log("No more clips left.");
        }
    }

    private IEnumerator ResetReloadAnimation()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("reload", false);
        isReloading = false;
    }

    private IEnumerator CreateTempSphereIndicator(Vector3 hitPosition)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        sphere.transform.position = hitPosition;
        yield return new WaitForSeconds(1);
        Destroy(sphere);
    }

    private void PlayShootSound()
    {
        if (shootClip != null && shootingAudioSource != null)
        {
            shootingAudioSource.PlayOneShot(shootClip);
        }
        else
        {
            Debug.LogWarning("Shoot clip or shooting audio source is missing.");
        }
    }

    private void PlayReloadSound()
    {
        if (reloadClip != null && shootingAudioSource != null)
        {
            shootingAudioSource.PlayOneShot(reloadClip);
        }
        else
        {
            Debug.LogWarning("Reload clip or shooting audio source is missing.");
        }
    }

    private void OnGameActive()
    {
        isGameActive = true;
    }

    private void OnGameInactive()
    {
        isGameActive = false;
    }

    private void OnMuteAllSounds()
    {
        if (shootingAudioSource != null)
        {
            shootingAudioSource.volume = 0; // 将音量设置为0
        }
    }

    private void OnUnmuteAllSounds()
    {
        if (shootingAudioSource != null)
        {
            shootingAudioSource.volume = originalVolume; // 恢复原始音量
        }
    }
}
