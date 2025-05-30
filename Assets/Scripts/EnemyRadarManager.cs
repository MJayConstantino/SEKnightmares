using System.Collections.Generic;
using UnityEngine;

public class EnemyRadarManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private Canvas uiCanvas;
    
    [Header("Settings")]
    [SerializeField] private float updateInterval = 0.2f;

    private Dictionary<BaseEnemy, EnemyRadarIndicator> activeIndicators = 
        new Dictionary<BaseEnemy, EnemyRadarIndicator>();
    private float nextUpdateTime;

    private void Update()
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRadar();
            nextUpdateTime = Time.time + updateInterval;
        }
    }

    private void UpdateRadar()
    {
        if (!uiCanvas) return;

        BaseEnemy[] enemies = FindObjectsOfType<BaseEnemy>();
        List<BaseEnemy> enemyList = new List<BaseEnemy>(enemies);
        
        // Remove indicators for destroyed enemies
        List<BaseEnemy> destroyedEnemies = new List<BaseEnemy>();
        foreach (var kvp in activeIndicators)
        {
            if (kvp.Key == null || !kvp.Key.gameObject || !enemyList.Contains(kvp.Key))
            {
                if (kvp.Value != null && kvp.Value.gameObject != null)
                {
                    Destroy(kvp.Value.gameObject);
                }
                destroyedEnemies.Add(kvp.Key);
            }
        }

        foreach (var enemy in destroyedEnemies)
        {
            activeIndicators.Remove(enemy);
        }

        // Create indicators for new enemies
        foreach (var enemy in enemies)
        {
            if (enemy != null && !activeIndicators.ContainsKey(enemy))
            {
                CreateIndicator(enemy);
            }
        }
    }

    private void CreateIndicator(BaseEnemy enemy)
    {
        if (!enemy || !indicatorPrefab || !uiCanvas) return;

        GameObject indicator = Instantiate(indicatorPrefab, uiCanvas.transform);
        if (!indicator) return;

        EnemyRadarIndicator radarIndicator = indicator.GetComponent<EnemyRadarIndicator>();
        if (radarIndicator)
        {
            radarIndicator.Initialize(enemy.transform, uiCanvas);
            activeIndicators.Add(enemy, radarIndicator);
        }
        else
        {
            Destroy(indicator);
        }
    }

    private void OnDisable()
    {
        foreach (var indicator in activeIndicators.Values)
        {
            if (indicator != null && indicator.gameObject != null)
            {
                Destroy(indicator.gameObject);
            }
        }
        activeIndicators.Clear();
    }
}