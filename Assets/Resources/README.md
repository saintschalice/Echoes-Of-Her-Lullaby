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
