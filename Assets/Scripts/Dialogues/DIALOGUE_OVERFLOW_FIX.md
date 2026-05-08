# 🔧 DIALOGUE TEXT OVERFLOW FIX

## 🐛 Problem

The dialogue text is overflowing and appearing behind the character portrait, as shown in the screenshot.

**Example:**
```
"We escaped lives without three from me... He always
Diary Pages... hidden in the cushions"
```

The text is going behind Lisa's character portrait on the right side.

---

## ✅ Solution (Unity Editor Fix)

This is a **UI layout issue** that needs to be fixed in the Unity Editor, not in code.

### **Step 1: Find the Dialogue Text Component**

1. Open Unity Editor
2. Find the DialogueSystemV2 GameObject in the scene
3. Look for the `dialogueText` TextMeshProUGUI component

### **Step 2: Fix Text Overflow Settings**

**Option A: Enable Text Wrapping**
1. Select the `dialogueText` TextMeshProUGUI component
2. In the Inspector, find **Overflow** settings
3. Set **Overflow** to `Truncate` or `Ellipsis`
4. Enable **Word Wrapping**

**Option B: Adjust Text Container Size**
1. Select the `dialogueText` RectTransform
2. Adjust the **Width** to be narrower (leave space for portrait)
3. Make sure the text area doesn't overlap with the character portrait
4. Example: If portrait is on the right, set text width to 70-80% of dialogue box width

**Option C: Add Layout Group**
1. Add a **Horizontal Layout Group** to the dialogue panel
2. Set **Child Force Expand** to control sizing
3. Add **Layout Element** components to text and portrait
4. Set **Preferred Width** for text (e.g., 70%) and portrait (e.g., 30%)

### **Step 3: Adjust Text Alignment**

1. Set **Alignment** to `Top Left` or `Middle Left`
2. Set **Horizontal Overflow** to `Wrap`
3. Set **Vertical Overflow** to `Truncate`

### **Step 4: Add Padding**

1. In the TextMeshProUGUI component, find **Margin** settings
2. Add **Right Margin** of 100-150 pixels (to avoid portrait)
3. Add **Left Margin** of 20-30 pixels (for padding)

---

## 📐 Recommended Layout

```
┌─────────────────────────────────────────────┐
│  Dialogue Box                               │
│  ┌──────────────────────────┐  ┌─────────┐ │
│  │                          │  │         │ │
│  │  Dialogue Text           │  │ Portrait│ │
│  │  (70% width)             │  │ (30%)   │ │
│  │                          │  │         │ │
│  └──────────────────────────┘  └─────────┘ │
│                                             │
│  [Tap to continue]                          │
└─────────────────────────────────────────────┘
```

**Key Points:**
- Text area: 70% width (left side)
- Portrait: 30% width (right side)
- Text should NOT overlap portrait
- Add padding/margins to prevent overlap

---

## 🎨 Alternative: Move Portrait

If adjusting text width doesn't work, consider moving the portrait:

**Option 1: Portrait Above Text**
```
┌─────────────────────────────────────────────┐
│  ┌─────────┐                                │
│  │Portrait │  Speaker Name                  │
│  └─────────┘                                │
│  ┌──────────────────────────────────────┐  │
│  │                                      │  │
│  │  Dialogue Text (full width)         │  │
│  │                                      │  │
│  └──────────────────────────────────────┘  │
└─────────────────────────────────────────────┘
```

**Option 2: Portrait Below Text**
```
┌─────────────────────────────────────────────┐
│  Speaker Name                               │
│  ┌──────────────────────────────────────┐  │
│  │                                      │  │
│  │  Dialogue Text (full width)         │  │
│  │                                      │  │
│  └──────────────────────────────────────┘  │
│  ┌─────────┐                                │
│  │Portrait │  [Tap to continue]             │
│  └─────────┘                                │
└─────────────────────────────────────────────┘
```

---

## 🧪 Test After Fix

1. Start the game
2. Trigger any dialogue
3. Check that text doesn't overflow
4. Check that text doesn't go behind portrait
5. Test with long dialogues (2 sentences)
6. Test with short dialogues (1 sentence)

---

## 💡 Quick Fix (Temporary)

If you need a quick temporary fix in code, you can limit dialogue length:

```csharp
// In DialogueSystemV2.cs, in TypeText() method:
string displayText = fullText;
if (displayText.Length > 150) // Limit to 150 characters
{
    displayText = displayText.Substring(0, 147) + "...";
}
dialogueText.text = displayText;
```

**Note:** This is NOT recommended as a permanent solution. Fix the UI layout instead.

---

## 🌟 Summary

**Problem:** Text overflows behind portrait  
**Cause:** Text container too wide, overlaps portrait  
**Solution:** Adjust text width in Unity Editor (70% width)  
**Alternative:** Move portrait above/below text  

**Fix Location:** Unity Editor → DialogueSystemV2 → dialogueText component  
**Settings to Change:** Width, Overflow, Wrapping, Margins  

---

**FIX THIS IN UNITY EDITOR!** 🎨
