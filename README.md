# 🏃 Rushline

## 📌 Project Overview

Rushline is a 3D endless runner game focused on mobile gameplay, where the player survives as long as possible by switching lanes, jumping, and sliding through dynamically generated obstacle patterns.

The project was developed using a modular architecture with dependency injection via Zenject, scalable gameplay systems, procedural level generation, rewarded ads integration, Firebase authentication, leaderboard flow, and clean code principles.

## 🎮 Implemented Features

### 🏗 Core Systems
- ✅ **Game Core** - Handles initialization, scene flow, dependency injection, and game lifecycle.
- ✅ **Player Controller** - Supports lane switching, jumping, sliding, collision handling, and death states.
- ✅ **Input System** - Mobile swipe controls with separate keyboard/editor input support.
- ✅ **State Machine** - Manages player states such as idle, run, jump, slide, and death.
- ✅ **Obstacle System** - Dynamically spawns and manages obstacle patterns during gameplay.
- ✅ **World Generation** - Procedural road/platform segment spawning with cleanup logic.
- ✅ **Score System** - Tracks traveled distance and player results.
- ✅ **Continue System** - Allows revive after death using rewarded ads.
- ✅ **Authentication System** - Firebase login / register / logout flow.
- ✅ **Leaderboard System** - Handles score submission and top players display.
- ✅ **Audio System** - Sound/music settings with persistent save data.
- ✅ **UI Window System** - Menus, popups, HUD, settings, defeat screens.

---

## 🛠 Detailed Breakdown

### 🏃 Endless Runner Gameplay
- Character moves forward automatically.
- Player avoids obstacles using:
  - ⬅ Lane Switching
  - ⬆ Jump
  - ⬇ Slide
- Speed gradually increases during gameplay.

### 🎮 Input System
- Swipe gestures for mobile devices.
- Keyboard controls for editor/testing mode.
- Separated input strategies for scalability.

### ⚙ State Machine
- Character logic divided into states:
  - Idle
  - Run
  - Jump
  - Slide
  - Death

This improves maintainability and code clarity.

### 🚧 Obstacle Generation
- Obstacles spawn dynamically during runtime.
- Randomized lane patterns.
- Difficulty increases over time.

### 🛣 Procedural World Generation
- Endless road/platform spawning.
- Old segments are removed automatically.
- Optimized for long gameplay sessions.

### 📈 Score and Progression
- Distance-based score system.
- Session result tracking.
- Personal best flow support.

### ❤️ Continue / Revive System
- After defeat, player can continue by watching rewarded ads.
- Restart and main menu options included.

### 🔐 Authentication System
- Firebase startup initialization.
- User registration.
- Login / logout flow.
- Validation systems.

### 🏆 Leaderboard Flow
- Submit score after run.
- Display top players.
- Show player best result.

### 🔊 Audio Settings
- Music on/off
- Sound on/off
- Saved using PlayerPrefs

---

## 🎨 UI / UX Design

- Main menu
- Login screen
- Leaderboard window
- Gameplay HUD
- Pause popup
- Settings popup
- Defeat popup
- Responsive mobile UI

---

## ⚡ Optimization

- Object pooling principles for obstacles/world parts.
- Config-driven balancing using ScriptableObjects.
- Modular services and scalable architecture.
- Clean separation of gameplay systems.

---

## 🔧 Technologies Used

- **Unity 6000.3.10f1**
- **C#**
- **Zenject**
- **Firebase Authentication**
- **Google Mobile Ads (Rewarded Ads)**
- **PlayerPrefs**
- **ScriptableObjects**
- **State Machine Architecture**
- **Strategy Pattern**
- **Dependency Injection**

---

## 🏆 Conclusion

Rushline is a scalable and well-structured mobile endless runner project that demonstrates practical Unity development skills including gameplay programming, architecture design, DI, ads integration, authentication systems, procedural generation, UI systems, and clean production-ready code structure. 🚀

---

# 🏃 Rushline (Українська)

## 📌 Огляд проєкту

Rushline — це 3D endless runner гра, орієнтована на мобільні платформи, де гравець виживає якомога довше, перемикаючи смуги руху, стрибаючи та ковзаючи між динамічно згенерованими перешкодами.

Проєкт побудований на модульній архітектурі з використанням Zenject, процедурної генерації рівнів, rewarded ads, Firebase авторизації, таблиці лідерів та чистої структури коду.

## 🎮 Реалізовані можливості

### 🏗 Основні системи
- ✅ Ядро гри
- ✅ Керування персонажем
- ✅ Swipe Input система
- ✅ State Machine
- ✅ Генерація перешкод
- ✅ Procedural World Generation
- ✅ Score система
- ✅ Continue після смерті через рекламу
- ✅ Firebase авторизація
- ✅ Leaderboard система
- ✅ Аудіо налаштування
- ✅ UI Window система

## 🔧 Використані технології

- **Unity 6000.3.10f1**
- **C#**
- **Zenject**
- **Firebase**
- **Google Mobile Ads**
- **PlayerPrefs**
- **ScriptableObjects**

## 🏆 Висновок

Rushline демонструє практичні навички Unity розробки: gameplay programming, архітектуру, dependency injection, мобільну оптимізацію, UI системи, procedural generation та production-ready структуру проєкту. 🚀

---

# 🏃 Rushline (Русский)

## 📌 Обзор проекта

Rushline — это 3D endless runner игра для мобильных платформ, где игрок должен как можно дольше выживать, переключая полосы движения, прыгая и скользя между динамически создаваемыми препятствиями.

Проект построен на модульной архитектуре с использованием Zenject, процедурной генерации уровней, rewarded ads, Firebase авторизации, лидерборда и чистой структуры кода.

## 🎮 Реализованные функции

### 🏗 Основные системы
- ✅ Ядро игры
- ✅ Контроллер персонажа
- ✅ Swipe Input система
- ✅ State Machine
- ✅ Генерация препятствий
- ✅ Procedural World Generation
- ✅ Score система
- ✅ Continue после смерти через рекламу
- ✅ Firebase авторизация
- ✅ Leaderboard система
- ✅ Аудио настройки
- ✅ UI Window система

## 🔧 Используемые технологии

- **Unity 6000.3.10f1**
- **C#**
- **Zenject**
- **Firebase**
- **Google Mobile Ads**
- **PlayerPrefs**
- **ScriptableObjects**

## 🏆 Заключение

Rushline демонстрирует практические навыки Unity разработки: gameplay programming, архитектуру, dependency injection, мобильную оптимизацию, UI системы, procedural generation и production-ready структуру проекта. 🚀
