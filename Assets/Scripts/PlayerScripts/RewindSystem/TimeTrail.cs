using UnityEngine;
using System.Collections.Generic;

namespace RewindSystem
{
    public class TimeTrail : MonoBehaviour
    {
        [Header("Trail Settings")]
        [SerializeField] private Color trailColor = new Color(0, 0.7f, 1f, 0.5f);
        [SerializeField] private float trailWidth = 0.3f;
        [SerializeField] private int trailSegments = 100;
        [SerializeField] private float trailFadeWidth = 0.1f;
        [SerializeField] private Color trailEndColor = new Color(0, 0.7f, 1f, 0.1f);

        private LineRenderer trailRenderer;
        private SpriteRenderer playerSpriteRenderer;
        private CircularBuffer circularBuffer;

        public void Initialize(SpriteRenderer spriteRenderer, CircularBuffer buffer)
        {
            playerSpriteRenderer = spriteRenderer;
            circularBuffer = buffer;
            CreateTrail();
        }

        private void CreateTrail()
        {
            trailRenderer = gameObject.AddComponent<LineRenderer>();
            trailRenderer.startWidth = trailWidth;
            trailRenderer.endWidth = trailFadeWidth;
            trailRenderer.positionCount = trailSegments + 1;
            trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
            trailRenderer.startColor = trailColor;
            trailRenderer.endColor = trailEndColor;
            trailRenderer.sortingLayerName = playerSpriteRenderer.sortingLayerName;
            trailRenderer.sortingOrder = playerSpriteRenderer.sortingOrder - 2;

            // Set up gradient for fading effect
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] {
                new GradientColorKey(trailColor, 0.0f),
                new GradientColorKey(trailEndColor, 1.0f)
                },
                new GradientAlphaKey[] {
                new GradientAlphaKey(trailColor.a, 0.0f),
                new GradientAlphaKey(trailEndColor.a, 1.0f)
                }
            );
            trailRenderer.colorGradient = gradient;

            // Initialize all points to current position
            for (int i = 0; i < trailRenderer.positionCount; i++)
            {
                trailRenderer.SetPosition(i, transform.position);
            }
        }

        public void UpdateTrail(float maxRewindTime, Vector2 previewPosition, int currentHealth)
        {
            if (trailRenderer == null) return;

            // Set first point to player's current position
            trailRenderer.SetPosition(0, transform.position);

            // Get trail snapshots
            int numPoints = trailSegments + 1;
            List<PlayerStateSnapshot> trailSnapshots = circularBuffer.GetTrailSnapshots(
                numPoints, maxRewindTime, transform.position, currentHealth);

            // Update trail positions
            for (int i = 0; i < numPoints; i++)
            {
                if (i < trailSnapshots.Count)
                {
                    trailRenderer.SetPosition(i, trailSnapshots[i].Position);
                }
                else
                {
                    // Interpolate remaining points if we don't have enough snapshots
                    float t = (float)i / (numPoints - 1);
                    Vector2 position = Vector2.Lerp(transform.position, previewPosition, t);
                    trailRenderer.SetPosition(i, position);
                }
            }
        }

        public void SetTrailEnabled(bool enabled)
        {
            if (trailRenderer != null)
            {
                trailRenderer.enabled = enabled;
            }
        }

        public void SetTrailActive(bool active)
        {
            if (trailRenderer != null)
            {
                trailRenderer.gameObject.SetActive(active);
            }
        }
    }
}