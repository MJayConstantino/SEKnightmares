using UnityEngine;
using UnityEngine.UI;

public class EnemyRadarIndicator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float circleRadius = 100f;
    [SerializeField] private Color enemyColor = Color.red;
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float wobbleAmount = 10f;
    [SerializeField] private float wobbleSpeed = 2f;

    private RectTransform rectTransform;
    private Image arrowImage;
    private Camera mainCamera;
    private Transform target;
    private Canvas canvas;
    
    private Vector2 currentPosition;
    private float currentAngle;
    private float wobbleOffset;
    private Vector2 screenCenter;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        arrowImage = GetComponent<Image>();
        mainCamera = Camera.main;
        screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        wobbleOffset = Random.Range(0f, 2f * Mathf.PI);
        
        if (arrowImage)
        {
            arrowImage.color = enemyColor;
        }
    }

    public void Initialize(Transform targetTransform, Canvas uiCanvas)
    {
        target = targetTransform;
        canvas = uiCanvas;
        
        // Set initial random position on the circle
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        currentPosition = screenCenter + new Vector2(
            Mathf.Cos(randomAngle) * circleRadius,
            Mathf.Sin(randomAngle) * circleRadius
        );
        currentAngle = randomAngle * Mathf.Rad2Deg;
        
        // Set initial position and rotation
        rectTransform.anchoredPosition = currentPosition - screenCenter;
        rectTransform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    private void Update()
    {
        if (!target || !canvas)
        {
            Destroy(gameObject);
            return;
        }

        // Check if target is visible in camera
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(target.position);
        bool isOffScreen = viewportPoint.x < 0 || viewportPoint.x > 1 || 
                          viewportPoint.y < 0 || viewportPoint.y > 1 || 
                          viewportPoint.z < 0;

        // Only show and update indicator when enemy is off screen
        arrowImage.enabled = isOffScreen;
        if (isOffScreen)
        {
            UpdateIndicatorPosition();
        }
    }

    private void UpdateIndicatorPosition()
    {
        // Get direction to enemy in screen space
        Vector2 targetScreenPos = mainCamera.WorldToScreenPoint(target.position);
        Vector2 directionToTarget = (targetScreenPos - screenCenter).normalized;
        
        // Calculate target position on circle
        Vector2 targetPos = screenCenter + (directionToTarget * circleRadius);
        
        // Add wobble effect
        float wobble = Mathf.Sin((Time.time * wobbleSpeed) + wobbleOffset) * wobbleAmount;
        targetPos += new Vector2(
            Mathf.Cos(currentAngle * Mathf.Deg2Rad) * wobble,
            Mathf.Sin(currentAngle * Mathf.Deg2Rad) * wobble
        );
        
        // Smoothly move towards target position
        currentPosition = Vector2.Lerp(currentPosition, targetPos, moveSpeed * Time.deltaTime);
        
        // Calculate target rotation
        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        
        // Smoothly rotate
        currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        
        // Apply position and rotation
        rectTransform.anchoredPosition = currentPosition - screenCenter;
        rectTransform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
}