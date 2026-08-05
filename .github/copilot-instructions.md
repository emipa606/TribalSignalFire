# GitHub Copilot Instructions for Tribal Signal Fire (Continued)

## Mod Overview and Purpose
Tribal Signal Fire (Continued) is a mod for RimWorld designed to enhance the communication capabilities of tribal colonies. Historically, tribal colonies in RimWorld lacked the ability to interact with other factions effectively. This mod addresses that limitation by introducing the Signal Fire, a construction which functions similarly to a campfire but offers interaction akin to a comms console, allowing communication with other factions on the ground.

## Key Features and Systems
- **Signal Fire Construction**: The Signal Fire can be built and needs to be refueled like a standard campfire, but it offers unique interaction capabilities.
- **Communication with Factions**: Enables communication with ground factions. Interaction with orbital trade ships is not supported.
- **Solar Flare Compatibility**: The Signal Fire functions during solar flares as it is not dependent on electricity.
- **Visibility and Manipulation Requirement**: Pawns must have a clear line of sight and the ability to manipulate the Signal Fire to use it.
- **Tribal Faction Restriction**: An option allows for limiting communication to only tribal factions.
- **Improved Call Menu**: The call menu now includes only visible factions.

## Coding Patterns and Conventions
- Adhere to [RimWorld modding conventions](https://rimworldwiki.com/wiki/Modding_Tutorials).
- Consistency in method naming (e.g., `DoSettingsWindowContents`, `GetFloatMenuOptions`).
- Use descriptive method and variable names for clarity (e.g., `leaderIsAvailableToTalk`).

## XML Integration
- **XML Def Files**: Located primarily in `1.6/Defs` and `Source/DefInjected` directories.
- **Defs Included**: `JobDef` and `ThingDef` specific to the Signal Fire functionality.
- XML files are structured for mod integration with RimWorld's existing XML system.

## Harmony Patching
- Harmony patching is not explicitly mentioned but typically involves adjusting game behavior through C# patches in RimWorld.
- Recommended to review the Harmony [documentation](https://harmony.pardeike.net/) to understand implementing method patches if necessary.

## Suggestions for Copilot
- **Leverage C# for core mod features**: Utilize Copilot to auto-generate repetitive code blocks or boilerplate code in C# files such as `ModStuff.cs`, `Building_SignalFire.cs`, and others.
- **XML Assistance**: Use Copilot for writing clear and structured XML definitions, ensuring each def is well-formed.
- **Enhancing Usability**: Suggest user-interface improvements in C# files, such as enhanced options in `GetFloatMenuOptions`.
- **Code Refactoring**: Opt for refactoring suggestions by Copilot to improve code efficiency and readability.
- **Debugging and Error Handling**: Seek guidance from Copilot for common debugging practices, particularly in methods like `TryMakePreToilReservations`.

This instruction file serves as a guide for utilizing GitHub Copilot effectively in your modding project, ensuring a smooth development process for the Tribal Signal Fire (Continued) mod. Enjoy enhancing the tribal communication experience in RimWorld!

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).

