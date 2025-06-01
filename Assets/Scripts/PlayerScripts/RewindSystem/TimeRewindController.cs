using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RewindSystem; 

public class TimeRewindController : MonoBehaviour
{
    [Header("Rewind Settings")]
    [SerializeField] private float maxRewindTime = 5f;
    [SerializeField] private float recordFrequency = 0.1f;
    [SerializeField] public float rewindCooldown = 10f;
    [SerializeField] public KeyCode rewindKey = KeyCode.R;
    [SerializeField] private AudioSource rewindSoundEffect;

    [Header("Visual Effects")]
    [SerializeField] private bool useVisualEffect = true;
    [SerializeField] private Color rewindColor = new Color(0, 0.8f, 1f, 0.8f);
    
    [Header("Component References")]
    [SerializeField] private bool showTrail = true;
    
    // Components
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator playerAnimator;
    
    // Separated components
    private CircularBuffer circularBuffer;
    private TimePreview timePreview;
    private TimeTrail timeTrail;
    private TimeGhost timeGhost;
    
    // Rewind control
    private bool isRewinding = false;
    private bool canRewind = true;
    private Color originalColor;
    private float lastRecordTime = 0f;

    [SerializeField] private float _rewindCooldown = 10f;

    public bool CanRewind => canRewind;
    public float RewindCooldownRemaining { get; private set; }

    private void Awake()
    {
        // Get components
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        originalColor = spriteRenderer.color;

        // Initialize separated components
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // Add and initialize CircularBuffer
        circularBuffer = gameObject.AddComponent<CircularBuffer>();
        circularBuffer.Initialize(maxRewindTime, recordFrequency);

        // Add and initialize TimePreview
        timePreview = gameObject.AddComponent<TimePreview>();
        timePreview.Initialize(spriteRenderer, playerAnimator, circularBuffer);

        // Add and initialize TimeTrail if enabled
        if (showTrail)
        {
            timeTrail = gameObject.AddComponent<TimeTrail>();
            timeTrail.Initialize(spriteRenderer, circularBuffer);
        }

        // Add and initialize TimeGhost
        timeGhost = gameObject.AddComponent<TimeGhost>();
        timeGhost.Initialize(spriteRenderer, playerAnimator);
    }

    private void Update()
    {
        // Record state at regular intervals when not rewinding
        if (!isRewinding && Time.time >= lastRecordTime + recordFrequency)
        {
            circularBuffer.RecordState(transform.position, playerHealth.CurrentHealth);
            lastRecordTime = Time.time;
        }
        
        // Update preview and trail when not rewinding
        if (!isRewinding && canRewind)
        {
            timePreview.UpdatePreview(maxRewindTime);
            
            if (showTrail && timeTrail != null)
            {
                Vector2 previewPos = timePreview.GetPreviewPosition();
                var trailSnapshots = circularBuffer.GetTrailSnapshots(
                    10, maxRewindTime, transform.position, playerHealth.CurrentHealth);
                timeTrail.UpdateTrail(maxRewindTime, previewPos, playerHealth.CurrentHealth);
                timeGhost.UpdateGhost(trailSnapshots);
            }
        }
        
        // Check for rewind input
        if (Input.GetKeyDown(rewindKey) && canRewind && !isRewinding)
        {
            StartRewind();
            if (rewindSoundEffect != null)
            {
                rewindSoundEffect.Play();
            }
        }

        // Update cooldown
        if (!canRewind)
        {
            RewindCooldownRemaining -= Time.deltaTime;
            if (RewindCooldownRemaining <= 0)
            {
                RewindCooldownRemaining = 0;
            }
        }

        if (isRewinding)
        {
            RewindTime();
        }
    }
    
    private IEnumerator RewindCooldown()
    {
        RewindCooldownRemaining = _rewindCooldown;
        yield return new WaitForSeconds(_rewindCooldown);
        canRewind = true;
        
        // Reactivate preview and trail
        timePreview.SetPreviewActive(true);
        if (timeTrail != null)
        {
            timeTrail.SetTrailEnabled(true);
        }
        if (timeGhost != null)
        {
            timeGhost.SetGhostActive(true);
        }
    }

    private void StartRewind()
    {
        isRewinding = true;
        canRewind = false;
        
        // Hide preview and trail during rewind
        timePreview.SetPreviewActive(false);
        if (timeTrail != null)
        {
            timeTrail.SetTrailEnabled(false);
        }
        
        // Disable player movement
        playerMovement.enabled = false;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        
        // Visual effect
        if (useVisualEffect)
        {
            spriteRenderer.color = rewindColor;
        }
    }

    private void RewindTime()
    {
        // Get rewind snapshot and apply it
        PlayerStateSnapshot rewindSnapshot = circularBuffer.GetRewindSnapshot(maxRewindTime);
        if (rewindSnapshot != null)
        {
            transform.position = rewindSnapshot.Position;
            playerHealth.TakeDamage(playerHealth.CurrentHealth - rewindSnapshot.Health);
        }
        
        StopRewind();
    }

    private void StopRewind()
    {
        isRewinding = false;

        // Hide ghost during cooldown
        if (timeGhost != null)
        {
            timeGhost.SetGhostActive(false);
        }

        // Re-enable player movement
        playerMovement.enabled = true;
        rb.isKinematic = false;

        // Reset visual effect
        if (useVisualEffect)
            spriteRenderer.color = originalColor;

        // Start cooldown
        StartCoroutine(RewindCooldown());
    }

    private void OnDestroy()
    {
        // Components will clean up themselves in their OnDestroy methods
    }
}