# PersistentSceneHider - Tagalog Guide

## Problema
Nakikita lahat sa PersistentScene (Lisa, joystick, UI) bago mag-start ang cutscene.

## Solusyon
**PersistentSceneHider** - I-hide lahat ng visible objects during cutscene!

---

## Paano I-setup (2 Minutes Lang!)

### Step 1: Gumawa ng PersistentSceneHider
1. Buksan ang **PersistentScene**
2. Right-click sa Hierarchy → Create Empty
3. I-rename to **"PersistentSceneHider"**
4. Add Component → **PersistentSceneHider.cs**

### Step 2: I-assign ang References
Sa PersistentSceneHider Inspector:

#### Lisa
- I-drag ang **Lisa** GameObject → `Lisa` field

#### PersistentUI
- I-drag ang **PersistentUI** GameObject → `Persistent UI` field
  - Ito yung parent ng joystick, inventory, etc.
  - Kung wala, i-drag yung joystick mismo

#### Settings
- ✅ I-check ang `Hide On New Game` = **TRUE**
- ✅ I-check ang `Debug Mode` = **TRUE**

**TAPOS NA!** Ganun lang kasimple! 😊

---

## Paano Gumagana

### New Game
```
New Game clicked
    ↓
PersistentScene nag-load
    ↓
PersistentSceneHider nag-detect: "Ah, new game ito!"
    ↓
I-hide si Lisa + joystick + UI
    ↓
Cutscene nag-play (walang nakikita except cutscene)
    ↓
Cutscene tapos
    ↓
I-show si Lisa + joystick + UI
    ↓
Game start!
```

### Load Game
```
Load Game clicked
    ↓
PersistentScene nag-load
    ↓
PersistentSceneHider nag-detect: "Ah, load game ito!"
    ↓
I-show agad si Lisa + joystick + UI
    ↓
Walang cutscene
    ↓
Game continue!
```

---

## Ano ang Na-hide

### Naka-hide sa New Game:
✅ **Lisa** - Player character  
✅ **Joystick** - Controls  
✅ **Inventory** - Item panel  
✅ **Notifications** - Item notifications  
✅ **Lahat ng UI** - Kung naka-group sa PersistentUI  

### Hindi Na-hide:
❌ **AudioManager** - Tumutugtog pa rin  
❌ **SaveSystem** - Gumagana pa rin  
❌ **Camera** - Nag-render pa rin  

---

## Testing

### Test New Game
1. I-click ang "New Game"
2. **Dapat**: Black screen lang (walang Lisa, walang joystick)
3. Cutscene nag-play
4. Pagkatapos: Lumabas si Lisa + joystick
5. Check Console:
   ```
   [PersistentHider] NEW GAME - All persistent objects hidden
   [FoyerIntro] All persistent objects shown after cutscene
   ```

### Test Load Game
1. Mag-save muna
2. Bumalik sa main menu
3. I-click ang "Load Game"
4. **Dapat**: Nakikita agad si Lisa + joystick
5. Walang cutscene
6. Check Console:
   ```
   [PersistentHider] LOAD GAME - All persistent objects visible
   ```

---

## Troubleshooting

### Nakikita pa rin si Lisa/joystick sa new game
**Fix**:
1. Check PersistentSceneHider Inspector:
   - `Hide On New Game` = TRUE
   - `Lisa` reference = assigned
   - `Persistent UI` reference = assigned
2. Check Console kung may "[PersistentHider] NEW GAME" message

### Hindi lumalabas si Lisa/joystick after cutscene
**Fix**:
1. Check kung tumatawag ang cutscene ng `FoyerIntroController.FinishIntro()`
2. Check Console kung may "[FoyerIntro] All persistent objects shown" message

---

## Quick Checklist

- [ ] Create PersistentSceneHider sa PersistentScene
- [ ] Add PersistentSceneHider.cs
- [ ] I-assign si Lisa
- [ ] I-assign ang PersistentUI (joystick parent)
- [ ] Check "Hide On New Game" = TRUE
- [ ] Test new game (walang nakikita during cutscene)
- [ ] Test load game (nakikita agad lahat)

---

## Bakit Ito ang Best Solution?

✅ **Simple** - Assign lang sa Inspector  
✅ **Automatic** - Nag-detect kung new game o load game  
✅ **Complete** - Lahat ng UI na-hide  
✅ **Reliable** - Gumagana every time  
✅ **Easy to Debug** - May clear logs  

---

## Summary

**Dati**: Nakikita si Lisa + joystick bago cutscene  
**Ngayon**: Walang nakikita during cutscene, lumabas after  

**Setup Time**: 2 minutes  
**Difficulty**: Easy  

**Status**: ✅ TAPOS NA - Test mo na!

**Yan na! Super simple lang!** 🎉
