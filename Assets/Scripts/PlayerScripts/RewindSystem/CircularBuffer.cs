using UnityEngine;
using System.Collections.Generic;

namespace RewindSystem
{
    public class CircularBuffer : MonoBehaviour
    {
        private PlayerStateSnapshot[] buffer;
        private int bufferSize;
        private int currentIndex = 0;

        public void Initialize(float maxRewindTime, float recordFrequency)
        {
            bufferSize = Mathf.CeilToInt(maxRewindTime / recordFrequency) + 1;
            buffer = new PlayerStateSnapshot[bufferSize];

            // Initialize buffer with default states
            Vector2 currentPos = transform.position;
            for (int i = 0; i < bufferSize; i++)
            {
                buffer[i] = new PlayerStateSnapshot(currentPos, 100, Time.time);
            }
        }

        public void RecordState(Vector2 position, int health)
        {
            currentIndex = (currentIndex + 1) % bufferSize;
            buffer[currentIndex] = new PlayerStateSnapshot(position, health, Time.time);
        }

        public PlayerStateSnapshot GetSnapshotAtTime(float targetTime)
        {
            int closestIndex = 0;
            float closestTimeDiff = float.MaxValue;

            for (int i = 0; i < bufferSize; i++)
            {
                if (buffer[i] == null) continue;

                float timeDiff = Mathf.Abs(buffer[i].TimeStamp - targetTime);
                if (timeDiff < closestTimeDiff)
                {
                    closestTimeDiff = timeDiff;
                    closestIndex = i;
                }
            }

            return buffer[closestIndex];
        }

        public PlayerStateSnapshot GetRewindSnapshot(float maxRewindTime)
        {
            float targetTime = Time.time - maxRewindTime;
            return GetSnapshotAtTime(targetTime);
        }

        public List<PlayerStateSnapshot> GetTrailSnapshots(int numPoints, float maxRewindTime, Vector2 currentPosition, int currentHealth)
        {
            List<PlayerStateSnapshot> snapshots = new List<PlayerStateSnapshot>();

            // Add current position first
            snapshots.Add(new PlayerStateSnapshot(currentPosition, currentHealth, Time.time));

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
    }
}