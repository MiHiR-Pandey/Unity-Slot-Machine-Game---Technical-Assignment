# Unity Slot Machine Game - Technical Assignment

## Overview
A lightweight, responsive 3-reel slot machine game developed in Unity. The project features classic arcade-style vertical symbol cycling, interactive betting options, dynamic balance management, and a fully playable WebGL build optimized for web browsers.

---

## 🎮 Key Features
* **Classic Arcade Reel Mechanics:** Smooth vertical symbol cycling and staggered stop sequences to build suspense.
* **Interactive Betting System:** Dynamic popup menu allowing players to select custom wager amounts (10G, 50G, or 100G) or exit.
* **Win Evaluation Logic:** Automated payline verification ensuring a win is triggered when all active reel symbols match, rewarding a 10x payout multiplier.
* **Mechanical Feedback:** Dynamic lever state animations corresponding to spin actions.
* **Clean UI Feedback:** Auto-fading status text notifications for game updates and balance alerts.

---

## 📁 Project Folder Structure
```text
Assets/
├── Scenes/
│   └── MainGame.unity
├── Scripts/
│   ├── SlotMachineGame.cs      // Manages game state, economy, popup menu, and flow
│   └── ReelHandler.cs          // Controls vertical symbol cycling and reel stopping
├── Sprites/                    // Provided UI frames, buttons, and symbol assets
