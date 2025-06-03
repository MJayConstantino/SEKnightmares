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
                buffer[i] = new PlayerStateSnapshot(currentPos, 10, Time.time);
            }
        }

        public void RecordState(Vector2 position, int health)
        {
            // (ENQUEUE) - Adds new state to buffer, overwrites oldest if full
            currentIndex = (currentIndex + 1) % bufferSize;
            buffer[currentIndex] = new PlayerStateSnapshot(position, health, Time.time);
            Debug.Log($"Recorded snapshot at index {currentIndex}: Pos={position}, Health={health}, Time={Time.time}");
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
            // (PEEK BACK) - Gets oldest relevant snapshot without removing it
            float targetTime = Time.time - maxRewindTime;
            var snapshot = GetSnapshotAtTime(targetTime);
            Debug.Log($"Rewinding to: Pos={snapshot.Position}, Health={snapshot.Health}, Time={snapshot.TimeStamp}");
            return snapshot;
        }

        public List<PlayerStateSnapshot> GetTrailSnapshots(int numPoints, float maxRewindTime, Vector2 currentPosition, int currentHealth)
        {
            List<PlayerStateSnapshot> snapshots = new List<PlayerStateSnapshot>();

            // (PEEK FRONT) - Gets most recent state
            snapshots.Add(new PlayerStateSnapshot(currentPosition, currentHealth, Time.time));

            // Gets states between front and back for trail visualization
            float timeStep = maxRewindTime / (numPoints - 1);
            for (int i = 1; i < numPoints; i++)
            {
                float targetTime = Time.time - (i * timeStep);
                // (PEEK AT INDEX) - Gets snapshot at specific time
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