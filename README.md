# ⏰ Time Rewind Game

A Unity-based 2D action game featuring a unique time rewind mechanic that allows players to revert their position and health to previous states.

## 📋 Table of Contents
- [🚀 Core Features](#-core-features)
- [📦 Requirements](#-requirements)
- [⚙️ Installation & Setup](#️-installation--setup)
- [🎮 How to Play](#-how-to-play)
- [🎯 Game Mechanics](#-game-mechanics)
- [🔧 Technical Implementation](#-technical-implementation)
- [⚙️ Configuration](#️-configuration)
- [🐛 Troubleshooting](#-troubleshooting)

## 🚀 Core Features

### 🎮 Gameplay Features
- **⏪ Time Rewind System**: Revert your position and health up to 5 seconds back in time
- **🏃‍♂️ Smooth 2D Movement**: Physics-based movement with dash mechanics
- **👻 Visual Preview**: See exactly where you'll rewind to with ghost trails
- **💥 Combat System**: Engage enemies with upgradeable weapons
- **📈 Experience & Progression**: Level up weapons through combat experience
- **🔊 Audio Integration**: Immersive sound effects for all actions
- **⏱️ Cooldown Management**: Strategic timing with balanced cooldown systems

### 🛠️ Technical Features
- **🔄 Circular Buffer Algorithm**: Efficient state storage and retrieval system
- **👁️ Real-time Preview**: Live visual feedback of rewind destination
- **🎯 Modular Architecture**: Clean, separated components for easy maintenance
- **⚡ Performance Optimized**: Memory-efficient with fixed-size buffers
- **🎨 Visual Effects**: Trail visualization and ghost effects during preview

## 📦 Requirements

### 🎯 Unity Version
- **Unity 2022.3 LTS** or newer recommended
- Compatible with Unity 2021.3 LTS and above

### 🎮 Input System
- **Unity Input System Package** (com.unity.inputsystem)
- New Input System must be enabled in Project Settings

### 📚 Additional Packages
- **2D Physics**: Built-in Unity 2D physics system
- **Audio**: Unity Audio system for sound effects

## ⚙️ Installation & Setup

### 1. 📁 Clone/Download Project
```bash
git clone [your-repository-url]
cd TimeRewindGame
```

### 2. 🚀 Open in Unity
1. Launch Unity Hub
2. Click **"Open"** and select the project folder
3. Wait for Unity to import all assets

### 3. ⚙️ Configure Input System
1. Go to **Edit > Project Settings > XR Plug-in Management**
2. Under **Input System Package**, select **"Input System Package (New)"**
3. When prompted, restart Unity

### 4. 🎬 Scene Setup
1. Open the main game scene
2. Ensure the following GameObjects are present:
   - 🎮 Player (with required components)
   - 🖼️ UI Canvas (for HUD elements)
   - 🔊 Audio Sources (for sound effects)

### 5. 🏗️ Build Settings
1. Go to **File > Build Settings**
2. Add your game scenes to the build
3. Select target platform
4. Configure player settings as needed

## 🎮 How to Play

### 🎯 Controls
- **🚶 Movement**: `WASD` or `Arrow Keys`
- **💨 Dash**: `Spacebar` (default)
- **⏪ Time Rewind**: `R` key (configurable)

### 🔄 Gameplay Loop
1. **🗺️ Explore**: Move around the game world
2. **⚔️ Combat**: Use weapons to defeat enemies and gain experience
3. **🧠 Strategic Rewind**: Use time rewind to:
   - 🏃‍♂️ Escape dangerous situations
   - ❤️ Recover lost health
   - 📍 Reposition tactically
4. **📈 Progression**: Level up weapons and abilities

### 💡 Pro Tips
- ⏱️ Time rewind has a cooldown - use it wisely!
- 👻 Watch the visual trail to see where you'll rewind to
- ❤️ Rewind not only affects position but also restores health
- 🧭 Plan your movements knowing you can rewind up to 5 seconds

## 🎯 Game Mechanics

### ⏪ Time Rewind System
- **⏱️ Rewind Duration**: Up to 5 seconds of gameplay
- **🔄 Cooldown**: 10 seconds between rewind uses
- **💾 State Recovery**: Both position and health are restored
- **👁️ Visual Feedback**: Trail and ghost effects show rewind preview

### 📈 Player Progression
- **✨ Experience Points**: Gained through combat
- **🔫 Weapon Upgrades**: Automatic weapon improvements on level up
- **📊 Stat Scaling**: Damage, fire rate, and force multipliers

### 🏃‍♂️ Movement System
- **🎮 Base Movement**: Configurable speed with smooth controls
- **💨 Dash Mechanic**: Quick movement with cooldown
- **⚡ Physics-Based**: Uses Unity's Rigidbody2D for realistic movement

## 🔧 Technical Implementation

### 🔄 Circular Buffer Algorithm

The core of the rewind system uses a **Circular Buffer** data structure:

```
Buffer Operations:
├── 📥 ENQUEUE: Add new state (overwrites oldest when full)
├── 👀 PEEK BACK: Get oldest relevant state for rewind
├── 🔝 PEEK FRONT: Get most recent state
└── 📍 PEEK AT INDEX: Get state at specific time
```

#### ⚙️ How It Works:
1. **📝 State Recording**: Every 0.1 seconds, player state is recorded
2. **💾 Buffer Management**: States are stored in a circular array
3. **🕐 Time-Based Retrieval**: Find closest state to target rewind time
4. **💡 Memory Efficiency**: Fixed-size buffer prevents memory bloat

### 🏗️ Architecture Pattern

The system follows a **Component-Based Architecture**:

```
🎮 TimeRewindController (Main Controller)
├── 💾 CircularBuffer (State Storage)
├── 👁️ TimePreview (Visual Preview)
├── 🌟 TimeTrail (Trail Visualization)
└── 👻 TimeGhost (Ghost Effects)
```

### 🧮 Key Algorithms

#### 📝 State Recording
```
Every recordFrequency seconds:
1. Get current position and health
2. Create PlayerStateSnapshot
3. Store in circular buffer at currentIndex
4. Increment currentIndex (with wraparound)
```

#### ⏪ Rewind Calculation
```
On rewind trigger:
1. Calculate target time (current time - maxRewindTime)
2. Search buffer for closest timestamp
3. Apply position and health from found snapshot
4. Trigger cooldown period
```

#### 🌟 Trail Generation
```
For trail visualization:
1. Generate time points between now and max rewind
2. Find corresponding snapshots for each point
3. Render visual trail connecting all points
```

## ⚙️ Configuration

### 🎮 TimeRewindController Settings
```csharp
[Header("⏪ Rewind Settings")]
maxRewindTime = 5f;        // ⏱️ Maximum seconds to rewind
recordFrequency = 0.1f;    // 📝 How often to record state
rewindCooldown = 10f;      // 🔄 Cooldown between rewinds
rewindKey = KeyCode.R;     // ⌨️ Input key for rewind
```

### 🏃‍♂️ Player Movement Settings
```csharp
[Header("🚶 Movement")]
speedPlayer = 5;           // 🎯 Base movement speed
dashSpeed = 15f;           // 💨 Dash speed multiplier
dashDuration = 1f;         // ⏱️ How long dash lasts
dashCooldown = 5f;         // 🔄 Cooldown between dashes
```

### 🔫 Weapon System Settings
```csharp
[Header("⚔️ Weapon Stats")]
baseDamage = 1f;           // 💥 Starting damage
baseFireRate = 20f;        // 🔥 Shots per second
baseFireForce = 20f;       // 💪 Bullet force
```

### 🔍 Debug Features

The system includes extensive debugging:
- **📊 Buffer State Logging**: Shows health values over time
- **👀 Rewind Visualization**: Debug lines showing rewind path
- **⚡ Performance Monitoring**: Track recording frequency and buffer usage

### ⚡ Performance Optimization

#### 💾 Memory Management
- 🔄 Fixed-size circular buffer prevents memory growth
- 📦 State snapshots use minimal data (Vector2 + int + float)
- 🧹 Automatic cleanup of expired bullet objects

#### 🚀 CPU Optimization
- ⏱️ Time-based recording reduces unnecessary state saves
- 🔍 Efficient buffer search using time stamps
- ♻️ Component pooling for visual effects

---

**🎉 Enjoy rewinding time and mastering the temporal battlefield!**
