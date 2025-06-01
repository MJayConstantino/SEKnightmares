using UnityEngine;

namespace RewindSystem
{
    public class TimePreview : MonoBehaviour
    {
        [Header("Preview Settings")]
        [SerializeField] private Color previewColor = new Color(1f, 1f, 1f, 0.4f);

        private GameObject previewObject;
        private SpriteRenderer previewRenderer;
        private Animator previewAnimator;

        private SpriteRenderer playerSpriteRenderer;
        private Animator playerAnimator;
        private CircularBuffer circularBuffer;

        public void Initialize(SpriteRenderer spriteRenderer, Animator animator, CircularBuffer buffer)
        {
            playerSpriteRenderer = spriteRenderer;
            playerAnimator = animator;
            circularBuffer = buffer;
            CreatePreviewObject();
        }

        private void CreatePreviewObject()
        {
            previewObject = new GameObject("RewindPreview");
            previewObject.transform.position = transform.position;

            // Copy sprite renderer from player
            previewRenderer = previewObject.AddComponent<SpriteRenderer>();
            previewRenderer.sprite = playerSpriteRenderer.sprite;
            previewRenderer.sortingLayerName = playerSpriteRenderer.sortingLayerName;
            previewRenderer.sortingOrder = playerSpriteRenderer.sortingOrder - 1;
            previewRenderer.color = previewColor;

            // Copy animator if player has one
            if (playerAnimator != null)
            {
                previewAnimator = previewObject.AddComponent<Animator>();
                previewAnimator.runtimeAnimatorController = playerAnimator.runtimeAnimatorController;
                previewAnimator.avatar = playerAnimator.avatar;

                // Sync initial animation parameters
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

        public void UpdatePreview(float maxRewindTime)
        {
            if (previewObject == null) return;

            // Get the rewind snapshot
            PlayerStateSnapshot rewindSnapshot = circularBuffer.GetRewindSnapshot(maxRewindTime);
            if (rewindSnapshot != null)
            {
                previewObject.transform.position = rewindSnapshot.Position;
            }

            // Update appearance
            UpdatePreviewAppearance();
        }

        private void UpdatePreviewAppearance()
        {
            if (previewRenderer == null) return;

            // Copy visual properties from player
            previewRenderer.sprite = playerSpriteRenderer.sprite;
            previewRenderer.flipX = playerSpriteRenderer.flipX;
            previewRenderer.flipY = playerSpriteRenderer.flipY;
            previewRenderer.color = previewColor;

            // Sync animation states
            if (playerAnimator != null && previewAnimator != null)
            {
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

        public Vector2 GetPreviewPosition()
        {
            return previewObject != null ? previewObject.transform.position : transform.position;
        }

        public void SetPreviewActive(bool active)
        {
            if (previewObject != null)
            {
                previewObject.SetActive(active);
            }
        }

        private void OnDestroy()
        {
            if (previewObject != null)
            {
                Destroy(previewObject);
            }
        }
    }
}