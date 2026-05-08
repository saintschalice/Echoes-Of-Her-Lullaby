# Fade In/Fade Out Transitions - Tagalog Guide

**Status**: ✅ IMPLEMENTED  
**Feature**: May fade animation na kada lipat ng room!

---

## Ano ang Ginawa?

Kada lipat ng room, may smooth transition na:
1. **Fade Out** - Screen unti-unting nag-black
2. **Load Scene** - Nag-load ng bagong room
3. **Fade In** - Screen unti-unting bumalik from black

---

## Mga Scripts na Na-update

### 1. ✅ RoomExit.cs
**Saan**: `Assets/Scripts/Puzzle/Room 04/RoomExit.cs`

**Ano ang Binago**:
- ❌ **DATI**: Biglaan lang mag-load ng scene (walang fade)
- ✅ **NGAYON**: May fade out → load scene → fade in

**Bagong Settings**:
```csharp
fadeOutDuration = 0.8f;  // Gaano kabilis mag-fade out
fadeInDuration = 0.8f;   // Gaano kabilis mag-fade in
disablePlayerDuringTransition = true;  // Hindi makagalaw si player habang nag-fade
```

**Flow**:
```
Player pumasok sa trigger
    ↓
Player hindi na makagalaw
    ↓
Screen nag-fade to black (0.8 seconds)
    ↓
Nag-load ng bagong room
    ↓
Screen nag-fade in from black (automatic)
```

---

### 2. ✅ LockedDoor.cs
**Saan**: `Assets/Scripts/GameManagement/LockedDoor.cs`

**Ano ang Binago**:
- ❌ **DATI**: Door opens → biglaan mag-load
- ✅ **NGAYON**: Door opens → fade out → load → fade in

**Bagong Settings**:
```csharp
fadeOutDuration = 0.8f;
fadeInDuration = 0.8f;
useFadeTransition = true;  // I-enable ang fade
```

**Flow**:
```
Player nag-interact sa door
    ↓
Check kung unlocked/may key
    ↓
Door animation (0.5 seconds)
    ↓
Player hindi na makagalaw
    ↓
Screen nag-fade to black (0.8 seconds)
    ↓
Nag-load ng bagong room
    ↓
Screen nag-fade in (automatic)
```

---

### 3. ✅ UnifiedDoorInteraction.cs
**Saan**: `Assets/Scripts/GameManagement/UnifiedDoorInteraction.cs`

**Status**: May fade transition na dati pa!

Walang binago, pero i-verify na `useFadeTransition = true` sa Inspector.

---

## Unity Setup (IMPORTANTE!)

### Kailangan ng ScreenFader GameObject

**Kung wala pa**:
1. Create Empty GameObject: `ScreenFader`
2. Add Component: `ScreenFader` script
3. Create child: UI → Image (name: `FadeImage`)
4. Setup FadeImage:
   - Anchor: Stretch (all corners)
   - Left/Right/Top/Bottom: 0
   - Color: Black (0, 0, 0, 255)
   - Raycast Target: ✅ Checked

**Settings sa ScreenFader**:
- Drag FadeImage to `Fade Image` field
- `Default Fade Duration`: 1
- `Fade Color`: Black
- `Fade In On Start`: ✅ Checked
- `Start Delay`: 0.2

**Canvas Settings**:
- Render Mode: Screen Space - Overlay
- Sort Order: 9999 (para laging nasa top)

---

## Paano I-test

### Test 1: Basic Fade
1. Start game
2. ✅ Screen dapat mag-fade in from black
3. Lumipat ng room
4. ✅ Screen dapat mag-fade out to black
5. ✅ Bagong room mag-load
6. ✅ Screen mag-fade in from black

### Test 2: RoomExit (Trigger)
1. Lakad papunta sa exit trigger
2. ✅ Player dapat tumigil
3. ✅ Screen mag-fade to black (0.8s)
4. ✅ Bagong room mag-load
5. ✅ Screen mag-fade in

### Test 3: LockedDoor
1. Interact sa locked door
2. ✅ "Door is locked" message (walang fade)
3. Kumuha ng key
4. Interact ulit
5. ✅ Door opens (animation)
6. ✅ Screen mag-fade to black
7. ✅ Bagong room mag-load
8. ✅ Screen mag-fade in

---

## Customization

### Baguhin ang Fade Speed
```csharp
// Sa Inspector ng RoomExit o LockedDoor:
fadeOutDuration = 1.5f;  // Mas mabagal (1.5 seconds)
fadeOutDuration = 0.5f;  // Mas mabilis (0.5 seconds)
```

### Baguhin ang Fade Color
```csharp
// Sa Inspector ng ScreenFader:
fadeColor = Color.white;  // Fade to white instead of black
```

### I-disable ang Fade sa Specific Door
```csharp
// Sa Inspector ng LockedDoor:
useFadeTransition = false;  // Walang fade, instant transition
```

---

## Troubleshooting

### Problema: Walang Fade
**Sanhi**: Walang ScreenFader sa scene  
**Fix**: 
1. Check kung may ScreenFader GameObject
2. Check kung may ScreenFader script
3. Check Console kung may errors

### Problema: Hindi Makita ang Fade
**Sanhi**: Canvas sort order mababa  
**Fix**: 
1. Select Canvas
2. Set Sort Order to 9999
3. Ensure FadeImage ay child ng Canvas

### Problema: Player Makagalaw Habang Nag-fade
**Sanhi**: Player controller hindi na-disable  
**Fix**: 
1. Check `disablePlayerDuringTransition = true`
2. Verify na may JoystickPlayerController

### Problema: Walang Fade In
**Sanhi**: `fadeInOnStart` disabled  
**Fix**: 
1. Select ScreenFader
2. Check `Fade In On Start` ✅
3. Adjust `Start Delay` kung kailangan

---

## Summary

✅ **RoomExit.cs** - May fade transition na para sa trigger exits  
✅ **LockedDoor.cs** - May fade transition na para sa doors  
✅ **UnifiedDoorInteraction.cs** - May fade transition na dati pa  
✅ **ScreenFader.cs** - Core system (existing)  

**Lahat ng room transitions ay may smooth fade in/fade out na!** 🎬✨

---

## Important Notes

1. **ScreenFader must exist** - Kailangan may ScreenFader GameObject sa scene
2. **Canvas sort order** - Dapat 9999 para laging nasa top
3. **Fade Image** - Dapat full screen black image
4. **Test all transitions** - I-test lahat ng doors at exits

---

## Mga Files na Na-modify

1. `Assets/Scripts/Puzzle/Room 04/RoomExit.cs` - Added fade
2. `Assets/Scripts/GameManagement/LockedDoor.cs` - Added fade
3. `Assets/Scripts/GameManagement/ScreenFader.cs` - Existing (no changes)

**Ready na para i-test sa Unity!** 🎮
