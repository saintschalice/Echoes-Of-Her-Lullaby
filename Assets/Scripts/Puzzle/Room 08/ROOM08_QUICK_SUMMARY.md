# Room 08 - Quick Summary

## 🎯 FLOW (5 Steps)

1. **Collect Evidence** → `hasCollectedAllEvidence = true`
2. **Cabinet** → Get Hammer → `hasFoundHammer = true`
3. **Bathtub** → Interact → `hasInteractedWithBathtub = true`
4. **Mirror** → QTE (15 taps, 25 seconds) → `hasBrokenMirror = true`
5. **Passage** → Climb Through → Go to Room 09

---

## 🎮 MIRROR QTE

- **15 taps** (changed from 50)
- **25 seconds** time limit
- **Full screen tap button** with color fill
- **Fill color**: Red-ish `(0.8, 0.2, 0.2, 0.5)`
- **Progress**: "0/15" → "15/15"
- **Mirror sprites**: 4 phases (clean → cracked → shattered)

---

## 🎨 UI SETUP

```
QTE Panel
├─ TapArea (Image) - Full screen, semi-transparent red
│   └─ FillImage (Image) - Filled type, 0 → 1
├─ TimerText - "25.0s"
├─ ProgressText - "0/15"
└─ MirrorImage - Shows cracking progress
```

**Key Settings**:
- TapArea: Color `(0.8, 0.2, 0.2, 0.5)`
- FillImage: Type = Filled, Method = Horizontal/Radial
- Button component added at runtime (no need to add manually)

---

## 📋 PREREQUISITES

**Cabinet**:
- Needs: Evidence collected

**Bathtub**:
- Needs: Hammer obtained

**Mirror**:
- Needs: Evidence + Hammer + Bathtub

---

## ✅ FILES UPDATED

- ✅ `Room08_FlowController.cs` - Updated flags and flow
- ✅ `Room08_Interactable.cs` - Updated prerequisites
- ✅ `Room08_MirrorQTE.cs` - 15 taps, fill image support
- ✅ `ROOM08_UPDATED_FLOW.md` - Complete English guide
- ✅ `ROOM08_FLOW_TAGALOG.md` - Complete Tagalog guide

---

**Ready to test!** 🎮✨

