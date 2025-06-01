using UnityEngine;
using System.Collections.Generic;

namespace RewindSystem
{
    public class TimeGhost : MonoBehaviour
    {
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

        private SpriteRenderer playerSpriteRenderer;
        private Animator playerAnimator;

        public void Initialize(SpriteRenderer spriteRenderer, Animator animator)
        {
            playerSpriteRenderer = spriteRenderer;
            playerAnimator = animator;
            CreateGhostObject();
        }

        private void CreateGhostObject()
        {
            ghostObject = new GameObject("TimeGhost");
            ghostRenderer = ghostObject.AddComponent<SpriteRenderer>();

            // Copy sprite properties from player
            ghostRenderer.sprite = playerSpriteRenderer.sprite;
            ghostRenderer.sortingLayerName = playerSpriteRenderer.sortingLayerName;
            ghostRenderer.sortingOrder = playerSpriteRenderer.sortingOrder - 1;
            ghostRenderer.color = ghostColor;

            // Set initial scale
            ghostObject.transform.localScale = transform.localScale * ghostScale;

            // Add glow effect
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

        public void UpdateGhost(List<PlayerStateSnapshot> snapshots)
        {
            if (ghostObject == null || snapshots.Count == 0) return;

            // Position ghost at the last recorded position
            PlayerStateSnapshot lastSnapshot = snapshots[snapshots.Count - 1];
            ghostObject.transform.position = lastSnapshot.Position;

            // Match player's sprite direction
            ghostObject.transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x) * (playerSpriteRenderer.flipX ? -1 : 1) * ghostScale,
                transform.localScale.y * ghostScale,
                transform.localScale.z * ghostScale
            );

            // Pulsing glow effect
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

        public void SetGhostActive(bool active)
        {
            if (ghostObject != null)
            {
                ghostObject.SetActive(active);
            }
        }

        private void OnDestroy()
        {
            if (ghostObject != null)
            {
                Destroy(ghostObject);
            }

        }
    }
}