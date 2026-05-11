# 🤖 AI ASSISTANT PROMPT TEMPLATE

## 📋 QUICK CONTEXT FOR AI

Copy-paste this when prompting other AI assistants about your game:

---

## PROMPT TEMPLATE:

```
I'm working on a Unity 2D mobile game called "Echoes of Her Lullaby" - a psychological horror puzzle adventure.

GAME OVERVIEW:
- Platform: Mobile (Android/iOS)
- Genre: 2D Psychological Horror / Puzzle Adventure
- Rooms: 9 total (Room 01 to Room 09)
- Room 09 is the FINAL ROOM (no Room 10)

TECHNICAL ARCHITECTURE:
- Interaction System: IInteractable interface + PlayerInteractionController
- All interactable objects implement IInteractable with OnInteract(), OnFocus(), OnBlur()
- Player walks near object → Interact button activates → Tap button → OnInteract() called
- Mobile controls: Virtual joystick + Interact button (bottom right)

INTERACTION PATTERN (from Room08_Interactable.cs):
```csharp
public class Room08_Interactable : MonoBehaviour, IInteractable
{
    public void OnInteract(PlayerContext context)
    {
        // Interaction logic here
    }
    
    public void OnFocus(PlayerContext context) { }
    public void OnBlur(PlayerContext context) { }
}
```

SETUP REQUIREMENTS:
- Object needs Collider2D (Is Trigger: ✓ CHECKED)
- Object implements IInteractable
- Player has PlayerInteractionController component
- Interact button exists in Canvas

ROOM 09 SPECIFICS:
- Final room with 4 mirror puzzles
- Each puzzle has 60-90 second timer
- Failure = Emily jumpscare → Game Over
- Success = Mirror complete
- All 4 complete → Ending cutscene (20 dialogues) → Main Menu

MY QUESTION:
[Your specific question here]
```

---

## EXAMPLE PROMPTS:

### **For Bug Fixing**:
```
I'm working on "Echoes of Her Lullaby" (Unity 2D mobile game).

CONTEXT:
- Uses IInteractable interface for interactions
- PlayerInteractionController detects nearby IInteractables
- Interact button triggers OnInteract()

PROBLEM:
[Describe your problem]

WHAT I'VE TRIED:
[What you've tried]

QUESTION:
[Your question]
```

### **For New Features**:
```
I'm working on "Echoes of Her Lullaby" (Unity 2D mobile game).

EXISTING SYSTEM:
- Room08_Interactable implements IInteractable
- Uses OnInteract(PlayerContext context) method
- Collider2D with Is Trigger checked

I WANT TO ADD:
[Describe feature]

QUESTION:
How should I implement this following the existing pattern?
```

### **For Code Review**:
```
I'm working on "Echoes of Her Lullaby" (Unity 2D mobile game).

CODING CONVENTIONS:
- Classes: PascalCase
- Methods: PascalCase
- Variables: camelCase
- Implements IInteractable for interactions

MY CODE:
[Paste your code]

QUESTION:
Does this follow the project's patterns? Any improvements?
```

---

## DETAILED CONTEXT (if needed):

If AI needs more context, refer them to:
```
Full game context available in: Assets/GAME_COMPLETE_CONTEXT.md

Key points:
1. Mobile game with virtual joystick + interact button
2. IInteractable interface for all interactions
3. PlayerInteractionController detects nearby objects
4. Room 09 is final room with 4 puzzles + ending
5. Colliders must be triggers for detection
6. Follow Room08_Interactable.cs pattern
```

---

## COMMON QUESTIONS & ANSWERS:

**Q: How do I make an object interactable?**
```
A: 
1. Add Collider2D (Is Trigger: ✓)
2. Implement IInteractable interface
3. Add OnInteract(), OnFocus(), OnBlur() methods
4. Player walks near → Interact button → Tap → OnInteract() called
```

**Q: Why can't I interact with my object?**
```
A: Check:
1. Collider2D has Is Trigger CHECKED
2. Object implements IInteractable
3. PlayerInteractionController on player
4. Interact button exists in scene
5. Player has "Player" tag
```

**Q: How do puzzles work?**
```
A: 
1. Interactable triggers puzzle panel
2. Panel has drag-and-drop items
3. Timer counts down
4. Correct solution → Success
5. Timeout → Emily attack → Game Over
```

**Q: What's the interaction flow?**
```
A:
Player near object
    ↓
PlayerInteractionController detects IInteractable
    ↓
Interact button activates
    ↓
Player taps button
    ↓
OnInteract() called
    ↓
Action happens
```

---

## KEY FILES TO REFERENCE:

```
Assets/Scripts/GameManagement/IInteractable.cs
Assets/Scripts/Player/PlayerInteractionController.cs
Assets/Scripts/Puzzle/Room 08/Room08_Interactable.cs (EXAMPLE)
Assets/Scripts/Puzzle/Room 09/Room09_Interactable.cs
Assets/Scripts/Puzzle/Room 09/Room09_FlowController.cs
Assets/GAME_COMPLETE_CONTEXT.md (FULL CONTEXT)
```

---

## QUICK SPECS:

**Engine**: Unity 2022+
**Platform**: Mobile (Android/iOS)
**Controls**: Virtual joystick + Interact button
**Interaction**: IInteractable interface
**Dialogue**: DialogueSystemV2
**Save**: SaveSystem (JSON)
**Rooms**: 9 total (01-09)
**Final Room**: Room 09 (4 puzzles + ending)

---

## EXAMPLE FULL PROMPT:

```
I'm working on a Unity 2D mobile horror game called "Echoes of Her Lullaby".

TECHNICAL SETUP:
- Mobile game with virtual joystick controls
- Interaction system uses IInteractable interface
- PlayerInteractionController detects nearby IInteractables
- Player taps Interact button to trigger OnInteract()
- All interactable objects need Collider2D (Is Trigger: ✓)

CURRENT ISSUE:
I'm implementing Room 09 (final room) with 4 mirror puzzles. Each mirror should be interactable and open a puzzle panel when the player taps the Interact button.

EXISTING PATTERN (from Room08_Interactable.cs):
```csharp
public class Room08_Interactable : MonoBehaviour, IInteractable
{
    public enum ObjectType { Bathtub, Mirror, Door }
    public ObjectType myType;
    
    public void OnInteract(PlayerContext context)
    {
        switch (myType)
        {
            case ObjectType.Bathtub:
                ExamineBathtub();
                break;
            // etc.
        }
    }
    
    public void OnFocus(PlayerContext context) { }
    public void OnBlur(PlayerContext context) { }
}
```

MY QUESTION:
[Your specific question here]

Please help me implement this following the existing project pattern.
```

---

## 🎯 TIPS FOR PROMPTING AI:

1. **Start with context** - Mention it's Unity 2D mobile game
2. **Explain the system** - IInteractable interface pattern
3. **Show existing code** - Reference Room08_Interactable.cs
4. **Be specific** - What exactly do you need help with?
5. **Mention constraints** - Mobile controls, existing systems
6. **Ask for pattern matching** - "Follow existing pattern"

---

## ✅ CHECKLIST BEFORE PROMPTING:

- [ ] Mentioned it's Unity 2D mobile game
- [ ] Explained IInteractable system
- [ ] Referenced existing code (Room08)
- [ ] Described specific problem/need
- [ ] Asked to follow existing patterns
- [ ] Provided relevant code snippets
- [ ] Mentioned mobile controls if relevant

---

**USE THIS TEMPLATE TO GET BETTER AI RESPONSES!** 🤖✨

The more context you provide, the better the AI can help!

**KAYA MO YAN!** 💪📝
