# Unity Project Folder Structure Creator
# Echoes of Her Lullaby - Automatic folder generation script

# Set the base path for your Unity project
$basePath = "D:\EchoesOfHerLullaby\Echoes Of Her Lullaby"

# Verify the base path exists
if (-not (Test-Path $basePath)) {
    Write-Host "Error: Base path does not exist: $basePath" -ForegroundColor Red
    Write-Host "Please make sure your Unity project exists at this location." -ForegroundColor Yellow
    Read-Host "Press Enter to exit"
    exit
}

# Define the folder structure
$folders = @(
    # Scripts folder structure
    "Assets\Scripts\Player",
    "Assets\Scripts\AI",
    "Assets\Scripts\Puzzle",
    "Assets\Scripts\Audio",
    "Assets\Scripts\UI",
    "Assets\Scripts\GameManagement",
    
    # Scenes folder structure
    "Assets\Scenes",
    
    # Prefabs folder structure
    "Assets\Prefabs\Player",
    "Assets\Prefabs\Interactables",
    "Assets\Prefabs\UI",
    "Assets\Prefabs\Audio",
    
    # Art folder structure
    "Assets\Art\Textures",
    "Assets\Art\Sprites",
    "Assets\Art\Materials",
    "Assets\Art\Animations",
    
    # Audio folder structure
    "Assets\Audio\Music",
    "Assets\Audio\SFX",
    "Assets\Audio\Ambient",
    
    # Resources folder structure
    "Assets\Resources\Data",
    "Assets\Resources\Config"
)

Write-Host "Creating Unity Project Folder Structure..." -ForegroundColor Green
Write-Host "Base Path: $basePath" -ForegroundColor Cyan
Write-Host ""

$createdCount = 0
$skippedCount = 0

foreach ($folder in $folders) {
    $fullPath = Join-Path $basePath $folder
    
    if (-not (Test-Path $fullPath)) {
        try {
            New-Item -ItemType Directory -Path $fullPath -Force | Out-Null
            Write-Host "✓ Created: $folder" -ForegroundColor Green
            $createdCount++
        }
        catch {
            Write-Host "✗ Failed to create: $folder" -ForegroundColor Red
            Write-Host "  Error: $($_.Exception.Message)" -ForegroundColor Yellow
        }
    }
    else {
        Write-Host "○ Already exists: $folder" -ForegroundColor Gray
        $skippedCount++
    }
}

# Create README files for each major section
Write-Host ""
Write-Host "Creating README files..." -ForegroundColor Yellow

$readmeContents = @{
    "Assets\Scripts\README.md" = @"
# Scripts Directory

This directory contains all C# scripts for Echoes of Her Lullaby.

## Directory Structure

- **Player/**: Player movement, interaction, and input handling
- **AI/**: Emily's AI system, pathfinding, and behavior states
- **Puzzle/**: Puzzle mechanics, object interactions, and solutions
- **Audio/**: Audio management, lullaby system, and sound triggers
- **UI/**: User interface, menus, inventory, and HUD
- **GameManagement/**: Game state, story flow, save system, and scene management

## Naming Conventions

- Use PascalCase for class names: `PlayerController`
- Use camelCase for variables: `currentHealth`
- Add descriptive prefixes: `UI_`, `AI_`, `Audio_`
- Use meaningful names that describe functionality
"@

    "Assets\Scenes\README.md" = @"
# Scenes Directory

Contains all Unity scenes for the game.

## Scene List

### Main Scenes
- **MainMenu**: Title screen, options, and game start
- **Room01_Foyer**: Front entrance, tutorial, first key discovery
- **Room02_LivingRoom**: First memory fragment, inventory tutorial
- **Room03_Hallway**: Emily's first encounter, hiding mechanics
- **Room04_Kitchen**: Chase sequence, recipe puzzle
- **Room05_DiningRoom**: Family trauma memories, table setting puzzle
- **Room06_ReturnHallway**: Emily's staircase blockade
- **Room07_LisaBedroom**: Personal belongings, lullaby fragment #3
- **Room08_LisaBathroom**: Escape sequence, mirror breaking
- **Room09_MasterBathroom**: Four-mirror puzzle, Emily's final stand
- **Room10_MasterBedroom**: Mirror revelation, final confrontation

### Utility Scenes
- **LoadingScreen**: Scene transition loading
- **GameOver**: Death/failure state
- **Credits**: End credits sequence

## Scene Naming Convention
- Use descriptive names with room numbers
- Prefix with scene type if needed
- Keep names consistent with story flow
"@

    "Assets\Prefabs\README.md" = @"
# Prefabs Directory

Contains all reusable game objects and prefab variants.

## Directory Structure

- **Player/**: Lisa character, camera rig, input handlers
- **Interactables/**: Doors, keys, memory objects, puzzle items
- **UI/**: Menu panels, HUD elements, dialogue boxes
- **Audio/**: Audio sources, music controllers, ambient sound

## Prefab Naming Conventions
- Use descriptive names: `Key_Golden`, `Door_Bedroom`
- Include material/type: `Mirror_Bathroom`, `Chair_Rocking`
- Use consistent prefixes for categories
- Version variants: `Lisa_Normal`, `Lisa_Possessed`
"@

    "Assets\Art\README.md" = @"
# Art Assets Directory

Contains all visual assets for the pixel-art horror aesthetic.

## Directory Structure

- **Textures/**: Base textures for 3D models and environments
- **Sprites/**: 2D pixel art for UI, characters, and items
- **Materials/**: Unity materials with shader configurations
- **Animations/**: Animation clips and controllers

## Art Guidelines

### Pixel Art Specifications
- **Resolution**: 16x16 to 64x64 pixels for items
- **Color Palette**: Limited, atmospheric horror colors
- **Style**: Consistent pixel art aesthetic
- **Compression**: Use Point filtering for crisp pixels

### Texture Settings
- **Format**: ASTC for Android optimization
- **Max Size**: 1024x1024 for most textures
- **Compression**: High Quality for key visuals
- **Mipmaps**: Disable for pixel art

## Asset Naming
- Include dimensions: `door_64x64`
- Describe content: `lisa_idle_animation`
- Use consistent naming across all assets
"@

    "Assets\Audio\README.md" = @"
# Audio Assets Directory

Sound design for atmospheric horror experience.

## Directory Structure

- **Music/**: Background music, lullaby variations, ambient tracks
- **SFX/**: Sound effects, interaction sounds, jump scares
- **Ambient/**: Room tone, atmospheric sounds, environmental audio

## Audio Specifications

### Technical Requirements
- **Format**: Vorbis (OGG) for compression
- **Quality**: 0.7 for music, 0.5 for SFX
- **Sample Rate**: 44.1kHz standard
- **Load Type**: Compressed in Memory

### Audio Categories

#### Music
- Lullaby theme and 4 fragments
- Room-specific ambient music
- Tension/chase music for Emily encounters

#### SFX
- Footsteps, door creaks, object interactions
- Emily's supernatural sounds
- Puzzle completion sounds

#### Ambient
- Room atmosphere (silence, wind, house settling)
- Emotional ambience for memory sequences
- Spatial audio for immersion

## Implementation Notes
- Use 3D spatial audio for immersion
- Dynamic volume based on story tension
- Seamless looping for ambient tracks
- Quick fade-in/out for sudden scares
"@

    "Assets\Resources\README.md" = @"
# Resources Directory

Runtime-loadable assets and configuration data.

## Directory Structure

- **Data/**: Game data, story content, character stats
- **Config/**: Settings, difficulty parameters, device optimization

## Usage Guidelines

### Data Files
- Story dialogue and narrative text
- Character statistics and progression data
- Puzzle solutions and hint systems
- Memory fragment content

### Config Files
- Performance settings per device tier
- Difficulty scaling parameters
- Audio mix presets
- Control sensitivity settings

## Loading Strategy
Use Resources.Load() sparingly - prefer Addressables for better memory management and loading performance on mobile devices.

## File Formats
- JSON for configuration data
- ScriptableObjects for game data
- CSV for localization (future expansion)
"@
}

foreach ($readmePath in $readmeContents.Keys) {
    $fullReadmePath = Join-Path $basePath $readmePath
    $readmeDir = Split-Path $fullReadmePath -Parent
    
    if (Test-Path $readmeDir) {
        try {
            $readmeContents[$readmePath] | Out-File -FilePath $fullReadmePath -Encoding UTF8
            Write-Host "✓ Created: $readmePath" -ForegroundColor Cyan
        }
        catch {
            Write-Host "✗ Failed to create README: $readmePath" -ForegroundColor Red
        }
    }
}

# Create .gitkeep files for empty directories that Git should track
Write-Host ""
Write-Host "Creating .gitkeep files for empty directories..." -ForegroundColor Yellow

$gitkeepDirs = @(
    "Assets\Scripts\Player",
    "Assets\Scripts\AI", 
    "Assets\Scripts\Puzzle",
    "Assets\Scripts\Audio",
    "Assets\Scripts\UI",
    "Assets\Scripts\GameManagement",
    "Assets\Prefabs\Player",
    "Assets\Prefabs\Interactables", 
    "Assets\Prefabs\UI",
    "Assets\Prefabs\Audio",
    "Assets\Art\Textures",
    "Assets\Art\Sprites",
    "Assets\Art\Materials", 
    "Assets\Art\Animations",
    "Assets\Audio\Music",
    "Assets\Audio\SFX",
    "Assets\Audio\Ambient",
    "Assets\Resources\Data",
    "Assets\Resources\Config"
)

foreach ($dir in $gitkeepDirs) {
    $gitkeepPath = Join-Path $basePath "$dir\.gitkeep"
    try {
        "" | Out-File -FilePath $gitkeepPath -Encoding UTF8
        Write-Host "✓ Created .gitkeep in: $dir" -ForegroundColor DarkGray
    }
    catch {
        Write-Host "✗ Failed to create .gitkeep in: $dir" -ForegroundColor Red
    }
}

# Summary
Write-Host ""
Write-Host "=================== SUMMARY ===================" -ForegroundColor Magenta
Write-Host "Folders created: $createdCount" -ForegroundColor Green
Write-Host "Folders already existed: $skippedCount" -ForegroundColor Gray
Write-Host "README files created: $($readmeContents.Count)" -ForegroundColor Cyan
Write-Host "Project structure ready for Unity development!" -ForegroundColor Green
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "1. Open Unity Hub and add this project" -ForegroundColor White
Write-Host "2. Verify folder structure in Unity Project window" -ForegroundColor White
Write-Host "3. Start implementing core scripts in Scripts/ folders" -ForegroundColor White
Write-Host "4. Import art assets into Art/ subfolders" -ForegroundColor White
Write-Host "5. Set up scenes in Scenes/ directory" -ForegroundColor White
Write-Host ""
Write-Host "Press Enter to exit..." -ForegroundColor Gray
Read-Host