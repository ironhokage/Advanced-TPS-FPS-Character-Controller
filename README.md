# Advanced TPS-FPS Character Controller

A modular and extensible **Third-Person / First-Person Character Controller for Unity**, built with a systems-oriented architecture that separates input, locomotion, camera, physics, state management, events, procedural motion, visuals, and supporting systems.

**Dependencies:**
  - This project is built using the [Kinematic Character Controller](https://assetstore.unity.com/packages/tools/physics/kinematic-character-controller-99131) asset from the Unity Asset Store.
  - It also has the [Juicy Springs from LlamaAcademy](https://github.com/llamacademy/juicy-springs).

The goal of this project is to provide a flexible foundation for building responsive and maintainable character controllers without relying on a single monolithic script.

---

## Overview

The controller is divided into independent systems that work together to create the complete character experience:

* **Camera** — camera and perspective-related systems
* **Locomotion** — character movement and locomotion logic
* **Physics** — physics-related character behavior
* **State Machines** — state-driven character behavior and transitions
* **Input** — player input and input abstraction
* **Events** — event-based communication between systems
* **Spring** — reusable spring-based smoothing and procedural motion
* **Managers** — higher-level system coordination
* **Visuals** — visual and presentation-related systems
* **Utilities** — reusable helper functionality
* **Prefabs** — reusable Unity prefab configurations
* **Scripts** — supporting/core implementation
* **Testing Scripts** — development and testing functionality

This structure keeps individual systems focused and makes the controller easier to extend as the project grows.

---

The controller is intentionally structured around **composition and separation of concerns** rather than putting every behavior into one controller component.

---

## Features

### TPS / FPS Foundation

Designed around the needs of both third-person and first-person gameplay.

The camera, locomotion, physics, state, and visual layers are separated so that perspective-specific behavior can be developed without tightly coupling it to the entire character system.

### Modular Locomotion

Character movement is isolated into its own module, providing a dedicated layer for movement behavior and making it easier to expand the controller with additional movement systems.

### State Machine Architecture

Character behavior is organized around explicit states rather than one large collection of conditional checks.

This provides a clean foundation for adding and managing different gameplay or locomotion states.

### Dedicated Physics Layer

Physics is treated as its own subsystem, separating physical behavior from high-level movement intent.

This helps keep locomotion logic focused while providing a dedicated area for physical interactions and resolution.

### Camera System

Camera behavior is isolated from the core movement system, making it easier to develop different camera configurations for first-person and third-person gameplay.

### Event System

The project includes a dedicated event layer for communication between systems.

This reduces unnecessary direct dependencies and allows multiple systems to respond to the same controller events without requiring the source system to know about every listener.

### Spring-Based Motion

The dedicated `Spring` system provides a reusable foundation for responsive and smoothly interpolated motion.

Spring-based behavior can be useful for camera movement, offsets, rotations, procedural motion, and other responsive systems.

### Managers

Higher-level system coordination is separated into the `Managers` module rather than being embedded directly into individual gameplay components.

### Visual Separation

Visual and presentation-related functionality lives separately from the core controller logic.

This makes it easier to work with different character visuals, perspective setups, and presentation systems.

### Prefab-Based Workflow

Controller configurations can be composed and reused through Unity prefabs, making the system easier to deploy across different scenes and characters.

### Testing Infrastructure

Development and testing functionality has its own `TestingScripts` module, keeping experimentation and validation code separate from the primary controller architecture.

---

## Project Structure

```text
Assets/
│
├── Camera/
├── Events/
├── Input/
├── Locomotion/
├── Managers/
├── Physics/
├── Prefabs/
├── Scripts/
├── Spring/
├── StateMachines/
├── TestingScripts/
├── Utilities/
└── Visuals/
```

### Module Responsibilities

| Module           | Responsibility                          |
| ---------------- | --------------------------------------- |
| `Camera`         | Camera and perspective-related behavior |
| `Events`         | Event-driven communication              |
| `Input`          | Player input and input abstraction      |
| `Locomotion`     | Character movement and locomotion       |
| `Managers`       | High-level system coordination          |
| `Physics`        | Physical character behavior             |
| `Prefabs`        | Reusable Unity prefabs                  |
| `Scripts`        | Core/supporting implementation          |
| `Spring`         | Spring-based smoothing and motion       |
| `StateMachines`  | State-driven character behavior         |
| `TestingScripts` | Testing and development functionality   |
| `Utilities`      | Shared helper systems                   |
| `Visuals`        | Visual and presentation systems         |

---

## Design Philosophy

The controller follows a few core principles:

**Separation of Concerns**
Each major responsibility has its own system instead of being concentrated in one controller.

**Composition Over Monolithic Design**
The character is assembled from multiple cooperating systems.

**Loose Coupling**
Events and system boundaries reduce unnecessary dependencies between components.

**Extensibility**
New movement states, gameplay systems, camera behaviors, and visual systems can be introduced without restructuring the entire controller.

**Reusability**
Utilities, spring systems, prefabs, and modular components can be reused across different character configurations.

**Maintainability**
Keeping systems focused makes the codebase easier to understand, debug, and iterate on.

---

## Why Use This Architecture?

Character controllers tend to become difficult to maintain once movement, camera behavior, physics, input, animation, effects, and gameplay states begin accumulating in the same class.

This project approaches the problem differently.

Instead of:

```text
One CharacterController.cs
├── Input
├── Movement
├── Camera
├── Physics
├── States
├── Effects
├── Visuals
└── Everything Else
```

the project separates those responsibilities:

```text
Input
Camera
Locomotion
Physics
State Machines
Events
Spring
Visuals
Managers
Utilities
```

Each system has a clearer purpose and can evolve independently.

---

## Extending the Controller

The architecture provides natural extension points for additional gameplay systems.

Additional functionality can be built around the existing systems without turning the main controller into a central dependency for everything.

---

## Intended Use

This project is suitable as a foundation for:

* FPS games
* TPS games
* Hybrid FPS/TPS games
* Action games
* Adventure games
* Character-controller prototypes
* Locomotion experiments
* Combat prototypes
* Custom Unity gameplay frameworks
* Technical gameplay programming projects

---

## Development

The project is structured to support iterative development and experimentation.

The dedicated `TestingScripts` module provides a place for testing controller behavior and experimenting with systems while keeping the main architecture organized.

The modular structure also makes it possible to focus development on individual areas such as locomotion, camera behavior, state transitions, or physics without needing to modify unrelated systems.

---

## Repository Status

This project is an **advanced Unity character-controller framework** with a modular architecture intended for continued development and expansion.

The repository structure is deliberately organized around distinct gameplay systems rather than a single all-in-one controller.

---

## Summary

**Advanced TPS-FPS Character Controller** is a modular Unity framework for creating responsive first-person and third-person character experiences.

Its architecture is built around:

* Camera
* Locomotion
* Physics
* State Machines
* Input
* Events
* Spring-based motion
* Managers
* Visuals
* Utilities
* Prefabs
* Testing

The core idea is simple:

> **A character controller should be a collection of focused systems working together, not one giant script.**

This architecture provides a clean foundation for developing, experimenting with, and extending advanced character movement systems in Unity.
