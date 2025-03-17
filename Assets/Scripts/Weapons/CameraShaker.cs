using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple camera shaker utility
public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance { get; private set; }
    
    private Transform _cameraTransform;
    private Vector3 _originalPosition;
    private Coroutine _shakeCoroutine;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        _cameraTransform = Camera.main.transform;
        _originalPosition = _cameraTransform.localPosition;
    }
    
    public void Shake(float intensity, float duration)
    {
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
        }
        
        _shakeCoroutine = StartCoroutine(ShakeCoroutine(intensity, duration));
    }
    
    private IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;
            
            _cameraTransform.localPosition = new Vector3(
                _originalPosition.x + x,
                _originalPosition.y + y,
                _originalPosition.z
            );
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        _cameraTransform.localPosition = _originalPosition;
        _shakeCoroutine = null;
    }
}