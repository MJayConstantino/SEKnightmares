using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewindController : MonoBehaviour
{
    [Header("Rewind Settings")]
    [SerializeField] private float maxRewindTime = 5f;          // Maximum time to rewind (in seconds)
    [SerializeField] private float recordFrequency = 0.1f;      // How often to record state (in seconds)
    [SerializeField] public float rewindCooldown = 10f;        // Cooldown before rewinding again
    [SerializeField] public KeyCode rewindKey = KeyCode.R;     // Key to press for rewind
    [SerializeField] private AudioSource rewindSoundEffect;     // Optional sound effect

    [Header("Visual Effects")]
    [SerializeField] private bool useVisualEffect = true;
    [SerializeField] private Color rewindColor = new Color(0, 0.8f, 1f, 0.8f);
    
    [Header("Preview Settings")]
    [SerializeField] private Color previewColor = new Color(1f, 1f, 1f, 0.4f); // Opacity of preview
    
    [Header("Trail Settings")]
    [SerializeField] private bool showTrail = true;
    [SerializeField] private Color trailColor = new Color(0, 0.7f, 1f, 0.5f);
    [SerializeField] private float trailWidth = 0.1f;
    [SerializeField] private int trailSegments = 10;            // Number of segments to display in the trail
    [SerializeField] private float trailFadeWidth = 0.05f;      // Width at the end of the trail (for fading effect)
    [SerializeField] private Color trailEndColor = new Color(0, 0.7f, 1f, 0.1f); // Color at the end of the trail

    [Header("Ghost Settings")]
    [SerializeField] private Color ghostColor = new Color(0, 0.7f, 1f, 0.5f);
    [SerializeField] private float ghostScale = 1f;
    [SerializeField] private Color glowColor = new Color(0, 0.7f, 1f, 1f);
    [SerializeField] private float glowIntensity = 2f;
    [SerializeField] private float glowRadius = 1f;
    private GameObject ghostObject;
    private SpriteRenderer ghostRenderer;
    private Animator ghostAnimator;
    private UnityEngine.Rendering.Universal.Light2D ghostGlow;
    
    // Components
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator playerAnimator;
    
    // Preview object
    private GameObject previewObject;
    private SpriteRenderer previewRenderer;
    private Animator previewAnimator;
    
    // Trail objects
    private LineRenderer trailRenderer;
    
    // Circular buffer implementation
    private PlayerStateSnapshot[] stateBuffer;
    private int bufferSize;
    private int currentIndex = 0;
    private float lastRecordTime = 0f;
    
    // Rewind control
    private bool isRewinding = false;
    private bool canRewind = true;
    private Color originalColor;

    [SerializeField] private float _rewindCooldown = 10f;

    public bool CanRewind => canRewind;
    public float RewindCooldownRemaining { get; private set; }

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();

        // Calculate buffer size based on max rewind time and recording frequency
        bufferSize = Mathf.CeilToInt(maxRewindTime / recordFrequency) + 1;
        stateBuffer = new PlayerStateSnapshot[bufferSize];

        // Initialize the buffer with current state
        for (int i = 0; i < bufferSize; i++)
        {
            stateBuffer[i] = new PlayerStateSnapshot(transform.position, playerHealth.CurrentHealth, Time.time);
        }

        originalColor = spriteRenderer.color;

        // Create preview object
        CreatePreviewObject();

        // Create trail if enabled
        if (showTrail)
        {
            CreateTrail();
        }
    }

    private void CreatePreviewObject()
    {
        // Create a new game object for the preview
        previewObject = new GameObject("RewindPreview");
        previewObject.transform.position = transform.position;
        
        // Copy the sprite renderer from the player
        previewRenderer = previewObject.AddComponent<SpriteRenderer>();
        previewRenderer.sprite = spriteRenderer.sprite;
        previewRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
        previewRenderer.sortingOrder = spriteRenderer.sortingOrder - 1; // Behind the player
        
        // Set the preview color and opacity
        previewRenderer.color = previewColor;
        
        // If the player has an Animator, copy it to the preview
        if (playerAnimator != null)
        {
            previewAnimator = previewObject.AddComponent<Animator>();
            previewAnimator.runtimeAnimatorController = playerAnimator.runtimeAnimatorController;
            previewAnimator.avatar = playerAnimator.avatar;
            // Ensure it uses the same animation state
            foreach (AnimatorControllerParameter param in playerAnimator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Bool)
                {
                    previewAnimator.SetBool(param.name, playerAnimator.GetBool(param.name));
                }
                else if (param.type == AnimatorControllerParameterType.Float)
                {
                    previewAnimator.SetFloat(param.name, playerAnimator.GetFloat(param.name));
                }
                else if (param.type == AnimatorControllerParameterType.Int)
                {
                    previewAnimator.SetInteger(param.name, playerAnimator.GetInteger(param.name));
                }
            }
        }
    }

    private void CreateTrail()
    {
        // Create a line renderer for the trail with multiple segments
        trailRenderer = gameObject.AddComponent<LineRenderer>();
        trailRenderer.startWidth = trailWidth;
        trailRenderer.endWidth = trailFadeWidth;
        trailRenderer.positionCount = trailSegments + 1; // +1 for the player's current position
        trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
        trailRenderer.startColor = trailColor;
        trailRenderer.endColor = trailEndColor;
        trailRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
        trailRenderer.sortingOrder = spriteRenderer.sortingOrder - 2; // Behind both the player and preview

        // Set up a gradient for the trail to fade out
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(trailColor, 0.0f), new GradientColorKey(trailEndColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(trailColor.a, 0.0f), new GradientAlphaKey(trailEndColor.a, 1.0f) }
        );
        trailRenderer.colorGradient = gradient;

        // Initialize all points to the current position
        for (int i = 0; i < trailRenderer.positionCount; i++)
        {
            trailRenderer.SetPosition(i, transform.position);
        }
        CreateGhostObject();
    }

    private void Update()
    {
        // Record state at regular intervals when not rewinding
        if (!isRewinding && Time.time >= lastRecordTime + recordFrequency)
        {
            RecordState();
            lastRecordTime = Time.time;
        }
        
        // Always update the preview when not rewinding
        if (!isRewinding && canRewind)
        {
            UpdatePreviewPosition();
            
            // Update trail if enabled
            if (showTrail && trailRenderer != null)
            {
                UpdateTrail();
            }
        }
        
        // Check for rewind key press (R key)
        if (Input.GetKeyDown(rewindKey) && canRewind && !isRewinding)
        {
            StartRewind();
            // Play sound effect only when rewinding
            if (rewindSoundEffect != null)
            {
                rewindSoundEffect.Play();
            }
        }

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
        previewObject.SetActive(true);
        
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
        }
        
        if (ghostObject != null)
        {
            ghostObject.SetActive(true);
        }
    }

    private void UpdatePreviewPosition()
    {
        // Find the state that's closest to 5 seconds ago
        float targetTime = Time.time - maxRewindTime;
        int targetIndex = currentIndex;
        float closestTimeDiff = float.MaxValue;

        for (int i = 0; i < bufferSize; i++)
        {
            if (stateBuffer[i] == null) continue;

            float timeDiff = Mathf.Abs(stateBuffer[i].TimeStamp - targetTime);
            if (timeDiff < closestTimeDiff)
            {
                closestTimeDiff = timeDiff;
                targetIndex = i;
            }
        }

        // Update preview position
        if (stateBuffer[targetIndex] != null)
        {
            previewObject.transform.position = stateBuffer[targetIndex].Position;
        }

        // Copy properties from the player to the preview
        UpdatePreviewAppearance();
    }

    private void UpdatePreviewAppearance()
    {
        // Copy relevant visual properties from the player to the preview
        previewRenderer.sprite = spriteRenderer.sprite;
        previewRenderer.flipX = spriteRenderer.flipX;
        previewRenderer.flipY = spriteRenderer.flipY;
        
        // Keep the preview color/opacity
        previewRenderer.color = previewColor;
        
        // Synchronize animation states if animators exist
        if (playerAnimator != null && previewAnimator != null)
        {
            // Sync animation state
            AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
            previewAnimator.Play(stateInfo.fullPathHash, 0, stateInfo.normalizedTime);
            
            // Sync parameters
            foreach (AnimatorControllerParameter param in playerAnimator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Bool)
                {
                    previewAnimator.SetBool(param.name, playerAnimator.GetBool(param.name));
                }
                else if (param.type == AnimatorControllerParameterType.Float)
                {
                    previewAnimator.SetFloat(param.name, playerAnimator.GetFloat(param.name));
                }
                else if (param.type == AnimatorControllerParameterType.Int)
                {
                    previewAnimator.SetInteger(param.name, playerAnimator.GetInteger(param.name));
                }
            }
        }
    }

    private void CreateGhostObject()
    {
        ghostObject = new GameObject("TimeGhost");
        ghostRenderer = ghostObject.AddComponent<SpriteRenderer>();
        
        // Copy sprite properties from player
        ghostRenderer.sprite = spriteRenderer.sprite;
        ghostRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
        ghostRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
        ghostRenderer.color = ghostColor;
        
        // Set initial scale
        ghostObject.transform.localScale = transform.localScale * ghostScale;

        ghostGlow = ghostObject.AddComponent<UnityEngine.Rendering.Universal.Light2D>();
        ghostGlow.lightType = UnityEngine.Rendering.Universal.Light2D.LightType.Point;
        ghostGlow.color = glowColor;
        ghostGlow.intensity = glowIntensity;
        ghostGlow.pointLightOuterRadius = glowRadius;
        ghostGlow.pointLightInnerRadius = glowRadius * 0.5f;
        ghostGlow.shadowIntensity = 0;
        
        // Create material for ghost with emission
        Material glowMaterial = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default"));
        glowMaterial.EnableKeyword("_EMISSION");
        glowMaterial.SetColor("_EmissionColor", glowColor * glowIntensity);
        ghostRenderer.material = glowMaterial;

        // Add animator if player has one
        if (playerAnimator != null)
        {
            ghostAnimator = ghostObject.AddComponent<Animator>();
            ghostAnimator.runtimeAnimatorController = playerAnimator.runtimeAnimatorController;
            ghostAnimator.avatar = playerAnimator.avatar;
        }
    }

    private void UpdateTrail()
    {
        // Set the first point to the player's current position
        trailRenderer.SetPosition(0, transform.position);

        // Get the ending position (preview position)
        Vector2 endPosition = previewObject.transform.position;

        // Calculate evenly spaced points between the player and preview
        int numPoints = trailSegments + 1; // Including player's position

        // Find appropriate snapshots for the trail
        List<PlayerStateSnapshot> trailSnapshots = GetTrailSnapshots(numPoints);

        // Update trail positions based on the snapshots
        for (int i = 0; i < numPoints; i++)
        {
            if (i < trailSnapshots.Count)
            {
                trailRenderer.SetPosition(i, trailSnapshots[i].Position);
            }
            else
            {
                // If we don't have enough snapshots, interpolate remaining points
                float t = (float)i / (numPoints - 1);
                Vector2 position = Vector2.Lerp(transform.position, endPosition, t);
                trailRenderer.SetPosition(i, position);
            }
        }
        UpdateGhost(trailSnapshots);
    }
    
    private List<PlayerStateSnapshot> GetTrailSnapshots(int numPoints)
    {
        List<PlayerStateSnapshot> snapshots = new List<PlayerStateSnapshot>();
        
        // Add the current position first
        snapshots.Add(new PlayerStateSnapshot(transform.position, playerHealth.CurrentHealth, Time.time));
        
        // Calculate time interval between each point
        float timeStep = maxRewindTime / (numPoints - 1);
        
        // Get snapshots at regular time intervals
        for (int i = 1; i < numPoints; i++)
        {
            float targetTime = Time.time - (i * timeStep);
            PlayerStateSnapshot snapshot = GetSnapshotAtTime(targetTime);
            
            if (snapshot != null)
            {
                snapshots.Add(snapshot);
            }
        }
        
        return snapshots;
    }
    
    private PlayerStateSnapshot GetSnapshotAtTime(float targetTime)
    {
        // Find the snapshot closest to the target time
        int closestIndex = 0;
        float closestTimeDiff = float.MaxValue;
        
        for (int i = 0; i < bufferSize; i++)
        {
            if (stateBuffer[i] == null) continue;
            
            float timeDiff = Mathf.Abs(stateBuffer[i].TimeStamp - targetTime);
            if (timeDiff < closestTimeDiff)
            {
                closestTimeDiff = timeDiff;
                closestIndex = i;
            }
        }
        
        return stateBuffer[closestIndex];
    }

    private void RecordState()
    {
        // Store current state in the circular buffer
        currentIndex = (currentIndex + 1) % bufferSize;
        stateBuffer[currentIndex] = new PlayerStateSnapshot(
            transform.position,
            playerHealth.CurrentHealth,
            Time.time
        );
    }

    private void StartRewind()
    {
        isRewinding = true;
        canRewind = false;
        
        // Hide preview during actual rewind
        previewObject.SetActive(false);
        
        // Hide trail during rewind
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
        
        // Disable player movement during rewind
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
        // Find the state that's closest to 5 seconds ago
        float targetTime = Time.time - maxRewindTime;
        int targetIndex = currentIndex;
        float closestTimeDiff = float.MaxValue;
        
        for (int i = 0; i < bufferSize; i++)
        {
            if (stateBuffer[i] == null) continue;
            
            float timeDiff = Mathf.Abs(stateBuffer[i].TimeStamp - targetTime);
            if (timeDiff < closestTimeDiff)
            {
                closestTimeDiff = timeDiff;
                targetIndex = i;
            }
        }
        
        // Apply the past state
        if (stateBuffer[targetIndex] != null)
        {
            transform.position = stateBuffer[targetIndex].Position;
            playerHealth.TakeDamage(playerHealth.CurrentHealth - stateBuffer[targetIndex].Health);
        }
        
        // End rewind
        StopRewind();
    }
    
    private void UpdateGhost(List<PlayerStateSnapshot> snapshots)
    {
        if (ghostObject == null || snapshots.Count == 0) return;

        // Position ghost at the last recorded position
        PlayerStateSnapshot lastSnapshot = snapshots[snapshots.Count - 1];
        ghostObject.transform.position = lastSnapshot.Position;

        // Match player's sprite direction
        ghostObject.transform.localScale = new Vector3(
            Mathf.Abs(transform.localScale.x) * (spriteRenderer.flipX ? -1 : 1) * ghostScale,
            transform.localScale.y * ghostScale,
            transform.localScale.z * ghostScale
        );

        if (ghostGlow != null)
    {
        float pulse = (Mathf.Sin(Time.time * 2f) + 1f) * 0.5f;
        ghostGlow.intensity = glowIntensity * (0.8f + (pulse * 0.2f));
    }

        // Update ghost animation if it exists
        if (ghostAnimator != null && playerAnimator != null)
        {
            // Copy current animation state
            AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
            ghostAnimator.Play(stateInfo.fullPathHash, 0, stateInfo.normalizedTime);

            // Copy animation parameters
            foreach (AnimatorControllerParameter param in playerAnimator.parameters)
            {
                switch (param.type)
                {
                    case AnimatorControllerParameterType.Bool:
                        ghostAnimator.SetBool(param.name, playerAnimator.GetBool(param.name));
                        break;
                    case AnimatorControllerParameterType.Float:
                        ghostAnimator.SetFloat(param.name, playerAnimator.GetFloat(param.name));
                        break;
                    case AnimatorControllerParameterType.Int:
                        ghostAnimator.SetInteger(param.name, playerAnimator.GetInteger(param.name));
                        break;
                }
            }
        }
    }

    private void StopRewind()
    {
        isRewinding = false;

        if (ghostObject != null)
        {
            ghostObject.SetActive(false);
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
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
        
        if (ghostObject != null)
        {
            Destroy(ghostObject);
        }
    }
}