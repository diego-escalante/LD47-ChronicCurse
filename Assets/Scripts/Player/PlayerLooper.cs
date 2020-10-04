using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (SpriteRenderer))]
public class PlayerLooper : MonoBehaviour, ILooper {
    
    public int maxBreakCharges = 1;
    public int loopsToChargeBreak = 3;
    
    private bool isLooping = true;
    private int loopsToChargeBreakLeft;
    private Vector3 position;
    private int breakCharges = 0;

    private SpriteRenderer loopGhost;
    private SpriteRenderer spriteRenderer;
    private Slider breakMeter;
    private LoopController loop;
    private CameraBehavior cam;
    private Animator animator;
    private Slider loopSlider;
    private Slider chargeSlider;
    private Transform shiftButton;
    private MusicController musicController;
    private SoundController soundController;
    private CamShake camShake;
    private PlayerLifeController plc;

    public Sprite run;
    public Sprite stand;

    private bool breakBuffer = false;

    private void OnEnable() {
        this.SubscribeToLoop();
        loopsToChargeBreakLeft = loopsToChargeBreak;
        loopSlider.gameObject.SetActive(true);
        chargeSlider.gameObject.SetActive(true);
        shiftButton.gameObject.SetActive(true);
        loop.playerLooper = this;
    }

    private void OnDisable() {
        this.UnsubscribeFromLoop();
        if (loopSlider != null) {
            loopSlider.gameObject.SetActive(false);
        }
        if (chargeSlider != null) {
            chargeSlider.gameObject.SetActive(false);
        }
        if (shiftButton != null) {
            shiftButton.gameObject.SetActive(false);
        }
        loop.playerLooper = null;
    }

    public void Awake() {
        Transform UITrans = GameObject.FindGameObjectWithTag("UI").transform;
        loopSlider = UITrans.Find("LoopSlider").GetComponent<Slider>();
        chargeSlider = UITrans.Find("BreakMeter").GetComponent<Slider>();
        shiftButton = UITrans.Find("ButtonShift");
        loopSlider.gameObject.SetActive(this.isActiveAndEnabled);
        chargeSlider.gameObject.SetActive(this.isActiveAndEnabled);
        shiftButton.gameObject.SetActive(this.isActiveAndEnabled);

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        loop = GameObject.FindGameObjectWithTag("GameController").GetComponent<LoopController>();
        breakMeter = GameObject.FindGameObjectWithTag("UI").transform.Find("BreakMeter").GetComponent<Slider>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform.parent.GetComponent<CameraBehavior>();
        musicController = loop.GetComponent<MusicController>();
        soundController = loop.GetComponent<SoundController>();
        camShake = cam.transform.Find("Main Camera").GetComponent<CamShake>();
        plc = GetComponent<PlayerLifeController>();
    }

    public void Update() {
        if ((plc.canDie || plc.justRevived) && Input.GetButton("Action") && breakCharges > 0 && !breakBuffer) {
            breakBuffer = true;
        }
        if (breakBuffer && loop.GetLoopPercentage() < 0.95f) {
            breakCharges--;
            breakBuffer = false;
            BreakLoop();
        }
        UpdateBreakMeter();
    }

    public bool IsLooping() {
        return isLooping;
    }

    public void SetState() {
        position = transform.position;
        isLooping = true;
        musicController.MusicMain();
        createLoopGhost();
        soundController.playSetStateSound();
        camShake.TinyShake();
        plc.Revive();
    }

    public void Loop() {
        Vector3 delta = (Vector3)position - transform.position;
        cam.Translate(delta);
        soundController.playSnapBackSound();
        plc.Revive();

        transform.position = position;
        if (breakCharges < maxBreakCharges) {
            loopsToChargeBreakLeft--;
            if (loopsToChargeBreakLeft <= 0) {
                breakCharges++;
            }
        }
    }

    private void BreakLoop() {
        loopsToChargeBreakLeft = loopsToChargeBreak;
        isLooping = false;
        camShake.TinyShake();
        musicController.MusicBoops();
        soundController.playBreakSound();
    }

    private void createLoopGhost() {
        // Create ghost if it doesn't exist.
        if (loopGhost == null) {
            GameObject ghostGO = new GameObject(gameObject.name + " Loop Ghost");
            loopGhost = ghostGO.AddComponent<SpriteRenderer>();
            Color c = loopGhost.color;
            c.a = 0.5f;
            loopGhost.color = c;
        }

        // This totally sucks.
        Sprite s;
        switch (animator.GetCurrentAnimatorStateInfo(0).ToString()) {
            case "PlayerIdleRight":
            s = stand;
            break;
            case "PlayerRunningRight":
            case "PlayerJumpingRight":
            default:
            s = run;
            break;
        }
        loopGhost.sprite = s;
        loopGhost.flipX = spriteRenderer.flipX;
        loopGhost.transform.position = transform.position;
    }

    private void UpdateBreakMeter() {
        breakMeter.value = (loopsToChargeBreak-loopsToChargeBreakLeft)/(float)loopsToChargeBreak + (isLooping ? loop.GetLoopPercentage() / loopsToChargeBreak : 0);
    }
}
