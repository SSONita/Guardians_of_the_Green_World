# Guardians of the Green World

A 3D environmental awareness adventure game where players identify polluters, collect trash, and restore the world before it reaches the disaster phase.

---

## 📌 Game Overview
- **Title:** Guardians of the Green World  
- **Genre:** 3D Environmental Awareness / Adventure  
- **Target Audience:** Ages 10+  
- **Goal:** Educate players on environmental responsibility by identifying polluters, collecting trash, and preventing environmental collapse.

---

## 🌍 Story & Setting
The game takes place in a vibrant green world that is slowly deteriorating due to pollution from careless NPCs.  
Your mission is to stop pollution, clean the environment, and restore balance before the world collapses.

---

## 🎮 Core Gameplay

### Player Abilities
- Walk and run in a 3D world
- Pick up trash and store it in a bag
- Interact with NPCs
- Sort trash into the correct bins

### NPC Behavior
- One NPC is randomly selected as the polluter  
- Polluter is marked temporary with an **exclamation mark (!)** everytime they through trash
- NPC apologizes and stops polluting when confronted

### Win Conditions
- Identify the polluter NPC  
- Collect and sort all trash correctly  
- Complete all tasks before **Phase 3** ends  

### Lose Conditions
- Time runs out  
- Trash remains unsorted  
- Polluter NPC not stopped  

---

## 🔄 Gameplay Flow

### Environmental Phases (Each = 1 minute)
- **Phase 1 – Safe:** Bright area, calm music  
- **Phase 2 – Danger:** Cloudy sky, tense music  
- **Phase 3 – Disaster:** Dark environment, alarm sounds  

---

## 🎨 Art Direction
- **Style:** Low-poly 3D  
- **Visual Progression:** Colors become more desaturated as pollution increases  
- **Assets Include:**  
  - Player & NPC models  
  - Low poly trees, forest, houses  
  - Trash items (plastic bottles, cans, paper)  
  - Trash bins  
  - UI indicators  

---

## 🔊 Audio
**Music**
- Calm ambient (Phase 1)  
- Tense atmosphere (Phase 2)  
- Alarm sounds (Phase 3)

**Sound Effects**
- Trash pickup  
- NPC dialogue  
- Phase transition alerts  
- Ambient nature sounds  

---

## 🛠 Technical Requirements
- **Engine:** Unity 3D  
- **Platform:** PC (Windows)  
- **Controls:**  
  - Movement: `WASD`  
  - Interact: `E`  
  - Pick Up Trash: Auto when near  
  - Sorting: Drag & Drop into bins  

### Key Scripts
- **PlayerController:** Handles movement, interactions, trash collection  
- **NPCManager:** Controls NPC behavior, selects polluter, handles trash throwing  
- **TrashSpawner:** Spawns trash every 30 seconds near polluter  
- **PhaseController:** Manages Safe → Danger → Disaster phases  
- **EndingManager:** Displays win/lose results and restart options  

---

## 📦 Scope
### Scenes
- Main Menu  
- Intro Cutscene  
- Gameplay  
- Success Ending  
- Failure Ending  

### Mechanics
- Trash collection & sorting  
- NPC detection  
- Phase-based environmental changes  

### Assets
Free assets + additional custom assets created in Blender.

---

## 📚 Course Information
- **Course:** Game Development  
- **Institute:** Cambodia Academy of Digital Technology  
- **Lecturer:** Dr. VA Hongly  

---

## 👥 Team Members
- SAR Sovannita  
- KEM Veysean  