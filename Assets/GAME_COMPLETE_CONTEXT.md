# 🎮 ECHOES OF HER LULLABY - COMPLETE GAME CONTEXT

## 📋 GAME OVERVIEW

**Title**: Echoes of Her Lullaby
**Genre**: 2D Psychological Horror / Puzzle Adventure
**Platform**: Mobile (Android/iOS)
**Engine**: Unity
**Perspective**: 2D Side-scrolling
**Art Style**: Dark, atmospheric, 1970s aesthetic

---

## 🎯 GAME CONCEPT

### **Core Story**:
Lisa, an adult woman, returns to her childhood home to confront repressed memories of trauma and abuse. She is haunted by Emily, a ghostly manifestation who appears to be both protector and tormentor. Through exploring 9 rooms, Lisa uncovers the truth: her mother planned to murder her in a murder-suicide, and Emily (a manifestation of Lisa's survival instinct) saved her by stopping the mother, who then took her own life.

### **Main Characters**:
1. **Lisa** - Protagonist, adult survivor of childhood trauma
2. **Emily** - Ghost/manifestation, Lisa's childhood protector who absorbed mother's violence
3. **Mother** - Antagonist (deceased), abusive parent who planned murder-suicide

### **Core Theme**:
Breaking the cycle of abuse, understanding trauma, forgiveness, and healing.

---

## 🏠 GAME STRUCTURE

### **Total Rooms**: 9 (Room 01 to Room 09)
**Note**: Room 09 is the FINAL ROOM. There is NO Room 10.

### **Room Flow**:
```
Main Menu
    ↓
Room 01: Foyer (Entry)
    ↓
Room 02: Living Room (First puzzles)
    ↓
Room 03: Hallway (Transition)
    ↓
Room 04: Kitchen (Emily AI introduction)
    ↓
Room 05: Dining Room (Cabinet puzzle)
    ↓
Room 06: Return to Hallway (Upstairs)
    ↓
Room 07: Lisa's Bedroom (Tea party puzzle)
    ↓
Room 08: Lisa's Bathroom (Emily hunting, QTE)
    ↓
Room 09: Master Bedroom's Bathroom (FINAL ROOM)
    ├─ 4 Mirror Puzzles
    ├─ Emily's Breakdown
    └─ Ending Cutscene (20 dialogues)
    ↓
Fade to Black
    ↓
Main Menu
    ↓
GAME COMPLETE
```

---

## 🎮 GAMEPLAY MECHANICS

### **Core Mechanics**:

1. **Movement**:
   - Virtual joystick (mobile)
   - 2D side-scrolling
   - Walk left/right

2. **Interaction**:
   - **IInteractable Interface** system
   - **PlayerInteractionController** detects nearby objects
   - **Interact Button** (bottom right of screen)
   - Player walks near object → Interact button activates → Tap to interact

3. **Inventory**:
   - Collect items (keys, evidence, puzzle items)
   - InventoryManager system
   - Item notifications when collected

4. **Dialogue**:
   - DialogueSystemV2
   - Text-based dialogue boxes
   - Character name display
   - Auto-advance or tap to continue

5. **Puzzles**:
   - Room-specific puzzle mechanics
   - Time limits (some puzzles)
   - Drag-and-drop systems
   - QTE (Quick Time Events)

6. **Emily AI**:
   - Pathfinding enemy
   - Chases player in certain rooms
   - Can be hidden from
   - Game over if caught

---

## 🔧 TECHNICAL ARCHITECTURE

### **Key Systems**:

1. **Scene Management**:
   - Persistent Scene (DontDestroyOnLoad)
   - Main Camera in Persistent Scene
   - Individual room scenes
   - SceneTransfer for transitions

2. **Player System**:
   - JoystickPlayerController (movement)
   - PlayerInteractionController (interaction detection)
   - PlayerContext (player state info)
   - VirtualJoystick (mobile input)

3. **Interaction System**:
   ```csharp
   public interface IInteractable
   {
       void OnInteract(PlayerContext context);
       void OnFocus(PlayerContext context);
       void OnBlur(PlayerContext context);
   }
   ```
   - All interactable objects implement IInteractable
   - PlayerInteractionController detects nearby IInteractables
   - Interact button triggers OnInteract()

4. **Dialogue System**:
   - DialogueSystemV2 (singleton)
   - Static dialogue constants in room-specific classes
   - Example: `Room09_Dialogues.ENTRY_1`

5. **Save System**:
   - SaveSystem (singleton)
   - Saves progress, puzzle completion, inventory
   - Multiple save slots

6. **Audio System**:
   - AudioManager (singleton)
   - Ambient music per room
   - SFX for interactions
   - Voice-over support

7. **UI System**:
   - Canvas (Screen Space - Overlay)
   - Panels for puzzles
   - Inventory UI
   - Pause menu
   - Dialogue box

---

## 📁 PROJECT STRUCTURE

### **Key Folders**:

```
Assets/
├── Scenes/
│   ├── MainMenu.unity
│   ├── PersistentScene.unity
│   ├── Room01_Foyer.unity
│   ├── Room02_LivingRoom.unity
│   ├── ... (Room03-Room08)
│   └── Room09_MasterBathroomFinal.unity
│
├── Scripts/
│   ├── GameManagement/
│   │   ├── IInteractable.cs
│   │   ├── SaveSystem.cs
│   │   ├── SceneTransfer.cs
│   │   └── ScreenFader.cs
│   │
│   ├── Player/
│   │   ├── JoystickPlayerController.cs
│   │   ├── PlayerInteractionController.cs
│   │   └── VirtualJoystick.cs
│   │
│   ├── Dialogues/
│   │   └── DialogueSystemV2.cs
│   │
│   ├── AI/
│   │   └── EmilyAI.cs
│   │
│   ├── Puzzle/
│   │   ├── Room 01/ (scripts for room 01)
│   │   ├── Room 02/ (scripts for room 02)
│   │   ├── ... (Room 03-08)
│   │   └── Room 09/
│   │       ├── Room09_FlowController.cs
│   │       ├── Room09_Dialogues.cs
│   │       ├── Room09_Interactable.cs
│   │       ├── Mirror1_MedicineCabinet.cs
│   │       ├── Mirror2_BathtubDrain.cs
│   │       ├── Mirror3_VanityTerror.cs
│   │       ├── Mirror4_EvidenceSequence.cs
│   │       └── DraggableItem.cs
│   │
│   └── UI/
│       └── (UI-related scripts)
│
├── Art/
│   ├── Sprites/
│   ├── Animations/
│   └── UI/
│
├── Audio/
│   ├── Music/
│   ├── SFX/
│   └── Ambient/
│
└── Resources/
    └── Data/
        └── MainItemDatabase.asset
```

---

## 🎨 ART STYLE & AESTHETICS

### **Visual Style**:
- 2D sprites
- Dark, muted color palette
- 1970s aesthetic (furniture, decor, clothing)
- Sepia tones for flashbacks
- Atmospheric lighting (dim, shadows)

### **UI Style**:
- Minimalist
- Semi-transparent panels
- Vintage fonts
- Dark backgrounds
- Clear, readable text

### **Character Design**:
- Lisa: Adult woman, simple clothing
- Emily: Ghostly, translucent, child-like
- Mother: Stern, 1970s dress, cold expression

---

## 🎵 AUDIO DESIGN

### **Music**:
- Ambient, atmospheric tracks
- Tense music for chase sequences
- Peaceful music for ending
- Lullaby motif throughout

### **Sound Effects**:
- Footsteps
- Door creaks
- Object interactions
- Emily's whispers/screams
- Puzzle success/failure sounds

### **Voice-Over** (Optional):
- Dialogue can be text-only or voiced
- Lisa's internal monologue
- Emily's whispers

---

## 🎯 ROOM 09 DETAILED BREAKDOWN

### **Room Name**: Master Bedroom's Bathroom (FINAL ROOM)

### **Purpose**: 
- Climactic puzzle room
- Reveals complete truth about mother's murder-suicide plan
- Emily's breakdown and resolution
- Game ending with 20-dialogue cutscene

### **Layout**:
- Bathroom setting
- 4 mirrors on walls
- Bathtub (3 sprite states)
- Medicine cabinet
- Vanity
- Large mirror with frames
- Emily manifestation (full power)

### **Gameplay Flow**:

**Phase 1: Entry (1 minute)**
```
1. Lisa climbs through broken mirror from Room 08
2. Blood everywhere (glass cuts)
3. Door slams shut - TRAPPED
4. Emily manifests at FULL POWER (solid, terrifying)
5. Intro dialogue sequence
6. Player gains control
```

**Phase 2: Puzzle Solving (5-10 minutes)**
```
Player can solve 4 mirror puzzles in ANY ORDER:

Mirror 1: Medicine Cabinet (60 seconds)
- Arrange 6 prescription bottles chronologically
- Years: 1973, 1974, 1975, 1975, 1976, 1976
- Reveals: Mother planning for years

Mirror 2: Bathtub Drain (60 seconds)
- Remove drain cover
- Reassemble 4 torn note pieces
- Complete note: "Tonight I end this child's suffering and mine - forever"
- Reveals: Murder-suicide plan

Mirror 3: Vanity Terror (90 seconds)
- Arrange 8 diary page fragments chronologically
- Shows mother's descent into madness
- Reveals: Timeline from defiance to final plan

Mirror 4: Evidence Sequence (60 seconds)
- Arrange 4 evidence items in order: Rope → Pills → Knife → Towel
- Each placement shows flashback
- Reveals: Premeditated murder plan

Failure Condition: Time runs out → Emily jumpscare → Game Over
Success Condition: Correct arrangement → Mirror complete
```

**Phase 3: All Mirrors Complete (2 minutes)**
```
1. Automatic trigger when all 4 mirrors solved
2. Player control disabled (cutscene)
3. Mother's voice echoes: "Tonight I end this child's defiance forever"
4. Emily's breakdown sequence:
   - Emily becomes translucent
   - Dialogue: "Every time I saved you, I became more like her!"
   - Emily collapses to floor
   - Water rises around her
5. Emily's final words (whisper):
   - "The mirror in there... it will show you everything I tried to hide"
   - "I'm sorry, Lisa. I couldn't protect you from the truth"
6. Player control re-enabled
```

**Phase 4: Ending Cutscene (3-4 minutes)**
```
20 Dialogue Sequence:

Phase 1: Final Realization (1-3)
- Lisa realizes complete truth
- Mother planned everything
- Murder-suicide plan

Phase 2: Understanding Emily (4-6)
- Emily saved Lisa that night
- Emily absorbed mother's rage
- Became what she fought against

Phase 3: Mother's Plan Revealed (7-9)
- Rope to restrain
- Pills to sedate
- Knife to murder
- Mother saw defiance as disease

Phase 4: Emily's Sacrifice (10-12)
- Emily stopped mother
- Mother took her own life
- Emily became monster to save Lisa

Phase 5: Forgiveness (13-15)
- Lisa forgives Emily
- Emily was child too
- Tried to break the cycle

Phase 6: Emily Fades Away (16-18)
- Emily fades into light
- Smiling, peaceful
- Bathroom becomes quiet

Phase 7: Final Words (19-20)
- Lullaby was cry for help
- Both are free now
- Emily can rest

Ending:
- Fade to black (2 seconds)
- Save game completion
- Return to Main Menu
- GAME COMPLETE
```

### **Scripts**:

1. **Room09_FlowController.cs**:
   - Main controller
   - Manages puzzle progress
   - Handles intro sequence
   - Handles ending cutscene
   - Tracks mirror completion

2. **Room09_Dialogues.cs**:
   - All dialogue constants
   - Entry dialogues
   - Puzzle dialogues
   - Success/failure dialogues
   - 20 ending dialogues

3. **Room09_Interactable.cs**:
   - Implements IInteractable
   - Attached to each mirror
   - Triggers puzzle when interacted
   - Works with PlayerInteractionController

4. **Mirror1_MedicineCabinet.cs**:
   - 6 bottle puzzle
   - Chronological arrangement
   - 60 second timer
   - Drag-and-drop system

5. **Mirror2_BathtubDrain.cs**:
   - 4 note piece puzzle
   - Reassemble note
   - 60 second timer
   - Drain cover button

6. **Mirror3_VanityTerror.cs**:
   - 8 diary page puzzle
   - Chronological arrangement
   - 90 second timer
   - Larger time for complexity

7. **Mirror4_EvidenceSequence.cs**:
   - 4 evidence item puzzle
   - Correct sequence
   - 60 second timer
   - Flashback images on correct placement

8. **DraggableItem.cs**:
   - Drag-and-drop system
   - Works for all puzzles
   - Detects slots
   - Notifies puzzle scripts

### **Assets Needed**:

**Sprites**:
- Emily (3 states): Full power, translucent, fading
- Bathtub (3 states): Empty, dirty water, blood
- Medicine cabinet (2 states): Closed, open
- 6 prescription bottles
- 4 torn note pieces
- 8 diary pages
- 4 evidence items: Rope, pills, knife, bloody towel
- 4 mirrors (different styles)
- 4 flashback images (sepia, 1970s style)

**UI**:
- 4 puzzle panels (full screen or large)
- Emily jumpscare panel
- Timer displays
- Slot indicators
- Success/failure feedback

**Audio**:
- Tense music (puzzle phase)
- Peaceful music (ending)
- Emily scream (jumpscare)
- Emily whisper (ending)
- Puzzle sounds (bottle clink, paper rustle, etc.)
- Success/failure sounds
- Fade out sound

---

## 🎮 INTERACTION SYSTEM DETAILS

### **How Interaction Works**:

```csharp
// 1. Object implements IInteractable
public class Room09_Interactable : MonoBehaviour, IInteractable
{
    public void OnInteract(PlayerContext context)
    {
        // Trigger puzzle
    }
    
    public void OnFocus(PlayerContext context)
    {
        // Player is near (optional highlight)
    }
    
    public void OnBlur(PlayerContext context)
    {
        // Player left (optional unhighlight)
    }
}

// 2. Object has Collider2D (Is Trigger: ✓)
// 3. PlayerInteractionController detects it
// 4. Interact button becomes active
// 5. Player taps button → OnInteract() is called
```

### **Setup Requirements**:

**For Interactable Object**:
1. Collider2D component (Is Trigger: ✓ CHECKED)
2. Script implementing IInteractable
3. Generous collider size for easy detection

**In Scene**:
1. Player with PlayerInteractionController
2. Interact button in Canvas
3. EventSystem
4. Canvas with Graphic Raycaster

---

## 💾 SAVE SYSTEM

### **What Gets Saved**:
- Current room/scene
- Puzzle completion states
- Inventory items
- Story progress flags
- Player position (per room)

### **Save Format**:
- JSON files
- Multiple save slots (3 slots)
- Auto-save on puzzle completion
- Manual save at checkpoints

---

## 🎯 DESIGN PRINCIPLES

### **Gameplay**:
1. **Fair but challenging** - Puzzles have clear solutions
2. **No pixel hunting** - Interactable objects are obvious
3. **Forgiving** - Can retry puzzles, multiple save slots
4. **Accessible** - Mobile-friendly controls, clear UI

### **Story**:
1. **Show, don't tell** - Environmental storytelling
2. **Gradual revelation** - Truth unfolds room by room
3. **Emotional impact** - Focus on character relationships
4. **Ambiguity** - Some interpretation left to player

### **Horror**:
1. **Psychological** - Not jump-scare heavy
2. **Atmospheric** - Tension through environment
3. **Meaningful** - Horror serves the story
4. **Respectful** - Handles trauma sensitively

---

## 🐛 COMMON ISSUES & SOLUTIONS

### **Issue 1: Can't interact with objects**
**Solution**: 
- Check Collider2D has Is Trigger ✓
- Check object implements IInteractable
- Check PlayerInteractionController on player
- Check Interact button exists

### **Issue 2: Emily AI not working**
**Solution**:
- Check NavMesh is baked
- Check EmilyAI script assigned
- Check player has "Player" tag
- Check hiding spots have "HidingSpot" tag

### **Issue 3: Dialogue not showing**
**Solution**:
- Check DialogueSystemV2 exists in scene
- Check dialogue constants are correct
- Check Canvas and dialogue panel exist
- Check no errors in Console

### **Issue 4: Scene transition not working**
**Solution**:
- Check scene is in Build Settings
- Check SceneTransfer script
- Check ScreenFader exists
- Check scene names match exactly

### **Issue 5: Puzzles not detecting completion**
**Solution**:
- Check all references assigned in Inspector
- Check DraggableItem scripts on items
- Check slot detection (colliders, tags)
- Check puzzle script logic

---

## 📝 CODING CONVENTIONS

### **Naming**:
- Classes: PascalCase (e.g., `Room09_FlowController`)
- Methods: PascalCase (e.g., `StartPuzzle()`)
- Variables: camelCase (e.g., `isPuzzleActive`)
- Constants: UPPER_SNAKE_CASE (e.g., `ENTRY_DIALOGUE_1`)
- Private fields: camelCase with underscore (e.g., `_currentState`)

### **Structure**:
```csharp
// 1. Using statements
using UnityEngine;
using System.Collections;

// 2. Class documentation
/// <summary>
/// Description of what this class does
/// </summary>

// 3. Class declaration
public class ClassName : MonoBehaviour
{
    // 4. Serialized fields (Inspector)
    [Header("Section Name")]
    public Type variableName;
    
    // 5. Private fields
    private Type _privateName;
    
    // 6. Unity methods
    private void Awake() { }
    private void Start() { }
    private void Update() { }
    
    // 7. Public methods
    public void PublicMethod() { }
    
    // 8. Private methods
    private void PrivateMethod() { }
    
    // 9. Coroutines
    private IEnumerator CoroutineName() { }
}
```

### **Comments**:
- Use `///` for XML documentation
- Use `//` for inline comments
- Comment WHY, not WHAT
- Keep comments updated

---

## 🎯 PERFORMANCE CONSIDERATIONS

### **Mobile Optimization**:
1. **Sprites**: Use sprite atlases, compress textures
2. **Audio**: Compress audio files, use streaming for music
3. **Physics**: Minimize colliders, use triggers when possible
4. **UI**: Pool UI elements, minimize overdraw
5. **Code**: Avoid Update() when possible, use events

### **Memory Management**:
1. Unload unused assets
2. Use object pooling for frequent spawns
3. Destroy objects when leaving rooms
4. Clear references to prevent leaks

---

## 🚀 BUILD SETTINGS

### **Android**:
- Minimum API Level: 24 (Android 7.0)
- Target API Level: 33 (Android 13)
- Scripting Backend: IL2CPP
- Target Architectures: ARM64

### **iOS**:
- Minimum iOS Version: 12.0
- Target SDK: Latest
- Architecture: ARM64

### **Common Settings**:
- Orientation: Landscape
- Resolution: 1920x1080 (scales to device)
- Quality: Medium-High
- VSync: On

---

## 📚 EXTERNAL RESOURCES

### **Unity Packages Used**:
- TextMeshPro (UI text)
- Input System (new input system)
- 2D Sprite (sprite rendering)
- Universal RP (optional, for lighting)

### **Assets**:
- Sprites: Custom or asset store
- Audio: Custom or royalty-free
- Fonts: Free fonts (Google Fonts, etc.)

---

## ✅ COMPLETION CHECKLIST

### **For Room 09**:
- [ ] All 4 mirror puzzles implemented
- [ ] All scripts created and assigned
- [ ] All UI panels created
- [ ] All sprites assigned
- [ ] All audio assigned
- [ ] Ending cutscene complete
- [ ] Tested all puzzle paths
- [ ] Tested success/failure states
- [ ] Tested ending sequence
- [ ] Tested return to main menu

### **For Full Game**:
- [ ] All 9 rooms complete
- [ ] All puzzles working
- [ ] All dialogues implemented
- [ ] Save system working
- [ ] Emily AI working
- [ ] All scenes in Build Settings
- [ ] Tested complete playthrough
- [ ] Performance optimized
- [ ] Bugs fixed
- [ ] Ready for release

---

## 🎉 SUMMARY

**Game**: Echoes of Her Lullaby
**Type**: 2D Psychological Horror Puzzle Adventure (Mobile)
**Rooms**: 9 (Room 01 to Room 09)
**Final Room**: Room 09 (Master Bedroom's Bathroom)
**Ending**: 20-dialogue cutscene → Fade to black → Main Menu

**Core Systems**:
- IInteractable interface for interactions
- PlayerInteractionController for detection
- DialogueSystemV2 for dialogues
- SaveSystem for progress
- Room-specific FlowControllers

**Room 09 Specifics**:
- 4 mirror puzzles (60-90 seconds each)
- Time pressure (Emily attacks if timeout)
- Complete ending cutscene
- Game completion

**Technical**:
- Unity 2D
- Mobile (Android/iOS)
- Virtual joystick controls
- Interact button for interactions
- Drag-and-drop puzzles

---

## 💬 FOR AI ASSISTANTS

When helping with this project:

1. **Understand the system**: IInteractable interface, PlayerInteractionController
2. **Match the pattern**: Look at Room08_Interactable as reference
3. **Mobile-first**: Always consider touch controls
4. **Consistent naming**: Follow existing conventions
5. **Test thoroughly**: Interaction, puzzles, dialogues, transitions
6. **Document changes**: Update guides when making changes
7. **Respect the story**: Maintain tone and themes

**Key Files to Reference**:
- `IInteractable.cs` - Interaction interface
- `PlayerInteractionController.cs` - Interaction detection
- `Room08_Interactable.cs` - Example implementation
- `Room09_FlowController.cs` - Room 09 main controller
- `DialogueSystemV2.cs` - Dialogue system

**Common Tasks**:
- Creating new interactable objects
- Adding puzzles
- Writing dialogues
- Fixing interaction issues
- Optimizing performance
- Testing gameplay

---

**COMPLETE GAME CONTEXT PROVIDED!** 🎮✨

This document contains everything needed to understand and work on "Echoes of Her Lullaby"!

**USE THIS TO PROMPT OTHER AI ASSISTANTS!** 💪📝
