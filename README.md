# SHADOWED

**Unity 6 / C# Gameplay Prototype**  
**Solo Developer & Technical Game Designer — Saman Abdolrashidi**

> A narrative action-stealth prototype where shadows are not just visual effects — they are the world the player interacts with.

---

## Gameplay

## Gameplay Video

▶️ [Watch the Shadowed gameplay prototype](https://youtu.be/JvoRQIxdFYw)

Shadowed is a 2.5D gameplay prototype built around a parallel relationship between the physical world and its shadows.

The player controls a man who survives a violent event that kills several of his friends. As guilt, fear, and rage begin to consume him, he gradually loses his physical identity and becomes part of the shadow world.

The shadows of his lost friends remain behind and can occasionally help him.

The central design goal is to make the narrative transformation affect the mechanics themselves rather than separating story and gameplay.

---

## Core Gameplay Idea

Physical objects and characters create shadows that exist as interactive gameplay elements.

The player can:

- Traverse along shadow surfaces
- Detect and select nearby interactive shadows
- Aim toward shadows using directional controller input
- Grab shadow objects
- Pull weapons through the shadow world
- Aim and throw shadow weapons
- Attack an enemy's shadow to damage their physical body
- Use ally shadows for traversal and special interactions
- Move between gameplay and cinematic sequences

Light and shadow are intended to become both environmental constraints and gameplay resources.

---

## Current Playable Prototype

The current build contains a playable gameplay sequence demonstrating:

- Custom shadow-surface traversal
- Controller-based movement and directional aiming
- Shadow detection and interaction
- Weapon grabbing and throwing
- Ally-assisted traversal
- Enemy shadow / physical-body damage connection
- Enemy patrol, chase and attack behavior
- Gameplay-triggered character animations
- Cinematic gameplay sequences
- Enemy death reactions
- 2D skeletal character animation
- Player control transfer between gameplay and scripted sequences

The prototype currently uses greybox environments so development can remain focused on gameplay systems and interaction design.

---

## Systems I Implemented

### Player & Traversal

- Custom surface-based movement system
- Horizontal traversal along authored shadow surfaces
- Jumping and gravity
- Surface attachment and re-snapping
- Controller input through Unity's New Input System

### Shadow Interaction

- Detection of nearby interactive shadows
- Multiple-shadow tracking
- Directional shadow selection
- Visual player-to-shadow connection
- Grab interaction
- Weapon availability and ownership states

### Combat

- Directional aiming
- Shadow weapon pickup
- Animated weapon movement toward the player
- Held weapon representation
- Projectile throwing
- Collision-based damage
- Enemy shadow damage affecting the physical enemy

### Enemy Systems

- Enemy health system
- Patrol / chase / attack states
- Attack ranges
- Death events
- Synchronized shadow and human reactions
- Gameplay-triggered death animations

### Animation & Sequences

- Unity Animator
- Unity Timeline / PlayableDirector
- Unity 2D Animation
- SpriteSkin skeletal animation
- Animation Events
- Event-driven gameplay sequencing
- Transitions between player-controlled gameplay and scripted moments

---

## Technology

- **Engine:** Unity 6
- **Language:** C#
- **Rendering:** Universal Render Pipeline
- **Input:** Unity New Input System
- **Animation:** Unity Animator, Timeline, 2D Animation, SpriteSkin
- **Version Control:** Git / GitHub
- **Target Platform:** Windows PC

---

## Development Approach

Shadowed is being developed as both a personal game project and a technical portfolio project.

My focus has been on building the mechanics myself and understanding why each system works rather than relying on large pre-built gameplay frameworks.

Development follows an iterative process:

**Prototype → Test → Refactor → Integrate → Polish**

The project has also been an opportunity to work across gameplay programming, technical design, animation integration, narrative design, debugging and visual prototyping.

---

## Design Philosophy

The main question behind Shadowed is:

> **What if a character's emotional transformation changed the rules of the world the player interacts with?**

Fear, guilt and rage are not intended to exist only in dialogue or cutscenes.

The goal is for those themes to influence traversal, combat, light, interaction and the player's relationship with the environment.

---

## Current Status

**Playable portfolio prototype / vertical slice**

The current build demonstrates the main gameplay direction and several interconnected systems.

The project is still in development. Environment art, effects, animation, sound and some systems remain prototype-quality and will continue to evolve.

---

## Future Development

Future work may include:

- More advanced light manipulation
- Shadow-based stealth mechanics
- Environmental puzzles
- Additional enemy behaviors
- More traversal combinations
- Expanded ally-shadow abilities
- Cinemachine camera work
- Sound design and original music
- More polished environments
- A larger playable level

---

## Developer

**Saman Abdolrashidi**

Junior Unity Developer / Technical Game Designer

Bologna, Italy

GitHub:  
https://github.com/abdorashidisaman-MM

Project Repository:  
https://github.com/abdorashidisaman-MM/Shadowed
