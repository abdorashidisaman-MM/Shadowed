# SHADOWED — CORE DESIGN v1

## 1. Core Fantasy

Shadowed is a fast, violent, emotionally heavy 2.5D action-stealth game set in a fictional city strongly inspired by Iran.

The protagonist begins as a normal human during a violent protest crackdown. He and several friends run into an alley while being pursued. They try to hide, but the officers find them.

Only the protagonist successfully hides inside a deep shadow.

He hears his friends being shot and realizes they are being killed while he survives.

The combination of:

- rage
- anger
- guilt
- fear
- panic
- shame
- emotional numbness

becomes overwhelming.

He feels that he survived because he was a coward.

Instead of simply hiding inside the shadow, he sinks into it.

His physical body disappears.

He becomes a living shadow.

The player gains control at this moment.

The transformation does not make him fearless.

Throughout the game he remains psychologically unstable.

He can be brutally powerful while still feeling panic, guilt and fear.

The emotional fantasy is not:

> “I became a superhero.”

It is:

> “I survived, I do not know what I have become, and I cannot forget why I survived.”

---

# 2. The Two Layers of the World

Shadowed takes place in one physical location but operates through two connected layers.

## Physical World

The physical world contains:

- people
- enemies
- guns
- buildings
- cars
- lamps
- electrical lights
- flagpoles
- trees
- railings
- doors
- furniture
- physical objects
- architecture
- projectiles
- real consequences

Enemies live in this world.

The protagonist does not.

## Shadow World

The shadow world exists across physical surfaces.

It contains:

- the player
- enemy shadows
- environmental shadows
- shadows of weapons
- traversal shadows
- hiding shadows
- shadows of dead friends
- later enemy shadow-projectiles

The player interacts primarily with this layer.

Actions performed in the shadow world can create consequences in the physical world.

---

# 3. Fundamental Rule

Every supernatural interaction happens through shadow.

Every important consequence remains visible in the physical world.

Examples:

```text
attack enemy shadow
→ physical enemy is injured

move object shadow
→ physical object moves

ally creates shadow bridge
→ player gains traversal route

move/block light
→ available shadow geometry changes

physical enemy fires specialized projectile
→ dangerous shadow projectile enters player space
```

This relationship between both worlds is one of Shadowed's main identities.

---

# 4. Laws of the Shadow World

## Law 1 — The Player Has No Physical Body

After the transformation, the protagonist is entirely a shadow entity.

There is no invisible human standing somewhere behind the shadow.

The shadow itself is the character.

Physical humans cannot simply collide with or punch the protagonist.

---

## Law 2 — The Player Requires a Surface

The protagonist can only exist on a physical surface capable of receiving a shadow.

Examples include:

- walls
- floors
- ceilings
- building façades
- vehicles
- doors
- large objects
- curtains
- roofs
- connected architectural surfaces

The playable space therefore consists of surfaces rather than ordinary open 3D space.

---

## Law 3 — Shadows Define Traversable Geometry

The player can move only through regions where usable shadow exists.

If a surface has no usable shadow path, the protagonist cannot simply move across it.

The physical environment therefore determines the shape of the shadow world.

---

## Law 4 — Surface Corners Are Traversable

The protagonist can move around connected 3D corners as long as the surface continues to support shadow.

Examples:

```text
wall → building corner → adjacent wall

floor → vertical surface

side of object → neighboring face
```

The player should visually appear to wrap around geometry rather than teleport between flat planes.

---

## Law 5 — Transferring Between Separate Surfaces Requires a Shadow Connection

If two surfaces are physically separated, the player requires some continuous shadow route between them.

The connection can be created by:

- environmental objects
- moving physical objects
- changing light direction
- thrown objects
- an ally shadow
- another temporary shadow phenomenon

No valid shadow connection means no traversal.

---

# 5. Light

Light is not inherently the enemy.

Light defines where shadows exist and therefore controls playable geometry.

## Normal Light

Direct illumination gradually causes the protagonist to fade.

Exposure is dangerous but not necessarily immediate death.

The player may have time to:

- escape
- block the light
- create another shadow
- reach another surface

Light primarily functions as a spatial obstacle and environmental system.

---

## Manipulating Light

The player can manipulate the environment to change lighting.

Examples:

- throw an object in front of a light
- move something into the beam
- break or redirect a light source
- change the angle of illumination
- create a new cast shadow
- remove an existing shadow

This allows the player to create traversal paths, defensive spaces and combat opportunities.

---

# 6. Death and the Heart

The protagonist's shadow form may be destroyed, but his heart persists.

His heart represents the remaining core of his humanity and existence.

## Checkpoints

Checkpoints are not arbitrary glowing markers.

They are emotionally meaningful locations.

Examples:

- somewhere he has a strong memory
- somewhere he saw a friend
- somewhere someone died
- somewhere important from his past
- somewhere associated with fear, safety, love or guilt

At these locations, the protagonist leaves behind a fragment of his heart.

If his current shadow body is destroyed, he reforms from the most recent heart fragment.

This turns respawning into part of the story.

The journey may also visually or narratively show that the protagonist becomes increasingly fragmented as pieces of his heart are left behind.

---

# 7. Shadow Types

Not every shadow behaves the same way.

Different shadows provide different gameplay functions.

## Environmental Surface Shadows

Used primarily for movement.

Examples:

- building shadows
- tree shadows
- railing shadows
- car shadows
- architectural shadows

---

## Hiding Shadows

Large or dense shadow areas can conceal the protagonist.

The player blends into the darkness and becomes difficult or impossible to distinguish as an unnatural living shadow.

---

## Weapon Shadows

Certain projected shapes can be taken and used as weapons.

Examples:

- sharp branches
- railings
- broken poles
- flagpole bases
- blades
- hooks
- sharp pieces of debris

The protagonist manipulates the shadow representation rather than wielding the physical object normally.

The physical object may move or react as its shadow is manipulated.

This movement can expose the player to enemies.

---

## Light-Blocking Shadows

Some objects can be manipulated specifically to interrupt a light source.

The resulting shadow can:

- create safe space
- open traversal
- redirect dangerous projected shadows
- hide the player
- isolate another shadow

---

## Traversal Shadows

Certain shadows exist mainly to connect otherwise unreachable surfaces.

They may be:

- permanent environmental paths
- moving shadows
- temporary paths
- ally-created connections

---

## Ally Shadows

The shadows of the protagonist's murdered friends persist after death.

They are not ordinary ghosts.

They exist as shadow entities connected to the protagonist.

They can provide:

- traversal support
- temporary bridges
- extended hands
- jumps
- pulls
- environmental manipulation
- combat support
- special attacks

Their abilities should remain connected to shadow logic rather than functioning as arbitrary magic.

---

# 8. Enemy Shadows

Every relevant physical enemy can cast a shadow.

That shadow is a vulnerable extension of the physical person.

The player primarily attacks the enemy's shadow rather than the enemy's physical body.

```text
PLAYER SHADOW
↓
attacks
↓
ENEMY SHADOW
↓
damage transfers
↓
PHYSICAL ENEMY
```

Damage to the shadow produces corresponding consequences in the physical body.

This is the foundation of combat.

---

# 9. Combat

Combat should feel:

- fast
- aggressive
- brutal
- desperate
- unstable
- tactile
- heavy

The protagonist does not fight like a conventional soldier.

Combat is based on manipulating the shadow environment.

## Basic Combat Loop

```text
identify usable shadow
↓
obtain weapon / route
↓
approach enemy shadow
↓
attack shadow
↓
physical body receives consequence
↓
enemies react
↓
escape / reposition / chain next attack
```

---

# 10. Combat Presentation

Attacks should have significant sensory impact.

Possible presentation elements:

- strong camera impulse
- impact freeze
- aggressive shadow deformation
- smear frames
- physical enemy animation reacting to shadow damage
- blood appearing in the physical layer
- sudden audio drop
- loud impact return
- panicked breathing
- screen vibration
- brief visual instability

The violence should contrast with the protagonist's emotional numbness and panic.

---

# 11. Ally-Assisted Special Attacks

Dead friends can participate in special attacks.

These attacks should reflect memory, grief and revenge.

Example:

A murdered female friend lowers and extends her shadow-hair.

The hair forms a noose.

The protagonist uses the shadow noose to execute an enemy through the enemy's shadow.

The physical body reacts to the execution.

The ally is not simply a combat summon.

The moment should feel like the dead are participating in the protagonist's revenge.

---

# 12. Enemy Awareness

Enemies cannot simply use normal videogame vision to detect an ordinary physical player.

There is no physical player body.

Enemies instead observe anomalies produced by interactions between the shadow and physical worlds.

## Unaware

Nothing visibly abnormal has happened.

Enemy follows normal behavior.

---

## Suspicious

Enemies witness something physically impossible or unexplained.

Examples:

- an object moves without visible cause
- a weapon rises by itself
- a light changes unexpectedly
- an object appears to hover
- a shadow moves independently of any physical body
- a sound or disturbance occurs in an impossible location

The enemy does not necessarily understand what is happening.

They investigate.

---

## Alert

Enemies have strong evidence of the living shadow.

Examples:

- directly seeing the protagonist's shadow on an otherwise empty surface
- watching another enemy die when their shadow is attacked
- seeing repeated unnatural shadow movement
- recognizing behavior they were previously warned about

At this point, they actively attempt to counter the protagonist.

---

# 13. Enemy Progression

Enemy understanding evolves throughout the game.

## Early Game

Enemies do not understand what the protagonist is.

Their reactions should include:

- confusion
- fear
- disbelief
- hesitation
- panic

They may initially believe attacks came from another person or unknown threat.

---

## Mid Game

Authorities begin understanding that something is moving through shadows.

They develop tactics such as:

- watching surfaces
- moving lights
- illuminating suspicious walls
- removing environmental shadows
- coordinating multiple lights
- watching for physical objects moving unnaturally

---

## Late Game

Enemies deliberately build technology and tactics designed to fight a shadow entity.

Examples:

- shadow-sensitive equipment
- controlled illumination systems
- moving searchlights
- light traps
- specialized shadow ammunition
- shadow projectiles
- deliberate environmental manipulation

At this stage, the protagonist is no longer an unknown phenomenon.

The enemy has adapted specifically to hunt him.

---

# 14. Shadow Projectiles

Normal physical bullets are not necessarily direct damage sources to the protagonist.

Later enemies gain technology that creates projectiles capable of harming entities in the shadow layer.

These may appear as dangerous projected shadow forms associated with physical weapons.

They can damage the protagonist directly.

This creates combat threats that exist in the same layer as the player.

---

# 15. Enemy Use of Light

Enemies eventually learn to use lighting tactically.

A particularly important tactic:

Enemies move a light source close to a wall or surface.

This reduces or removes competing environmental shadows.

The protagonist's unnatural shadow becomes isolated and easier to identify.

Light therefore becomes an enemy tool without becoming inherently evil.

---

# 16. Physical Consequences of Shadow Manipulation

Manipulating a shadow may cause corresponding movement in the physical object.

This is extremely important for enemy awareness.

Example:

```text
player grabs pole shadow
↓
physical pole begins moving
↓
enemy sees pole moving by itself
↓
enemy becomes suspicious
```

Therefore every shadow interaction has potential tactical consequences.

The player must consider:

> Can anyone in the physical world see what I am doing?

---

# 17. First Playable Slice

## Scene 1 — The Alley

The protest has turned violent.

The protagonist and friends run through a narrow alley.

Officers pursue them.

The group attempts to hide.

They are discovered.

The protagonist alone finds a deep corner shadow.

---

## Scene 2 — The Gunshots

The protagonist remains hidden.

He hears gunfire.

His friends are being killed.

He cannot bring himself to move.

Fear, guilt, anger and panic become overwhelming.

He sinks into the shadow.

His body disappears.

---

## Scene 3 — Transformation

The protagonist awakens as a shadow entity inside the corner.

For the first time, control is given to the player.

The player learns basic surface movement.

The protagonist moves:

```text
wall
→ around corner
→ adjacent surface
```

This teaches that the player exists on geometry rather than in ordinary 3D space.

---

## Scene 4 — The Dead Rise as Shadows

Ahead, the physical bodies of the protagonist's friends remain where they were killed.

Their shadows begin moving independently.

They rise.

Each friend's shadow takes a position around the environment.

The protagonist realizes he is not completely alone.

---

## Scene 5 — First Traversal Assistance

A gap prevents the protagonist from reaching the next playable surface.

One friend's shadow reaches out.

The player presses the action button.

The protagonist grabs the friend's hand.

The friend pulls or swings the player across the gap.

This teaches ally-assisted traversal.

---

## Scene 6 — First Weapon

The protagonist reaches a surface containing:

- enemy shadows
- an environmental shadow resembling a sharp branch, broken railing or flagpole base

The player learns to grab the weapon shadow.

The physical object may subtly move as the shadow is taken.

---

## Scene 7 — First Kill

The protagonist approaches an enemy's projected shadow.

The player drives the sharp shadow weapon into the enemy shadow's head.

On the physical layer, the enemy receives the corresponding fatal injury.

The officer collapses.

The remaining officers freeze.

They do not understand what they witnessed.

---

## Scene 8 — Fear

The surviving enemies inspect the body.

They look around.

There is no attacker.

One notices the wall.

Another notices unnatural shadow movement.

This is the first demonstration of enemy suspicion.

---

## Scene 9 — Ally Special Attack

Another murdered friend appears.

A girl's shadow lowers her hair.

Her hair extends and forms a noose.

The player uses the noose against another enemy shadow.

The enemy is hanged through the shadow interaction.

The physical body reacts correspondingly.

This teaches ally-assisted combat.

---

## Scene 10 — The Survivor Shoots

The final enemy panics.

They begin firing.

The situation escalates from execution tutorial into active combat.

The player must now react rather than simply attack.

---

## Scene 11 — Light Manipulation

The player manipulates a nearby lamp or light source.

Changing the light changes the projected geometry and redirects dangerous shadow trajectories.

The player uses the newly created shadow arrangement to survive the gunfire.

This introduces the fundamental environmental system:

> Light changes the battlefield.

---

## Scene 12 — Escape

The player escapes through a newly created shadow path.

The surviving physical world is left behind.

The tutorial ends with the player understanding:

```text
I am a shadow.

I cannot move everywhere.

The environment creates my world.

My dead friends can help me.

I can hurt people through their shadows.

The physical world can see the consequences of what I do.

And eventually, they will learn how to fight back.
```

---

# 18. Emotional Tone

Shadowed should feel psychologically unstable.

The protagonist cycles between:

- rage
- panic
- numbness
- guilt
- fear
- violent release

Combat should not completely erase the fear.

Being powerful should not mean feeling safe.

Between violent sequences, the protagonist may experience:

- memories
- distorted voices
- breathing
- visual fragments
- hesitation
- silence
- intrusive recollections

The player should occasionally feel both powerful and deeply vulnerable at the same time.

---

# 19. Visual Identity

The physical world should remain grounded and recognizable.

The supernatural becomes more powerful because it exists inside an otherwise believable environment.

## Physical Layer

Visual qualities:

- dense Iranian-inspired urban architecture
- narrow alleys
- apartment blocks
- rooftops
- electrical cables
- metal railings
- shop fronts
- concrete walls
- streetlights
- cars
- signs
- shutters
- staircases
- fluorescent lighting
- old façades
- smoke
- rain or dust depending on scene
- believable human spaces

The physical layer should not feel like generic Gothic fantasy.

---

## Shadow Layer

The shadow world should feel:

- fluid
- distorted
- unnaturally responsive
- deep black
- expressive
- almost alive

The protagonist's silhouette can deform more aggressively during movement and combat.

Ally shadows may feel more dreamlike and emotionally expressive.

---

# 20. Level Design Philosophy

Levels should not primarily be designed as conventional platforms.

They should be designed as networks of possible shadow geometry.

Design questions should include:

```text
What casts a shadow here?

Where does that shadow land?

What happens if the light moves?

Can the player reach the next surface?

What object could create a temporary path?

Can an enemy see the physical consequence of this action?

Could an ally shadow connect these surfaces?

Can this route disappear?

Can the player create a new route?
```

Physical world layout and lighting are therefore inseparable from level design.

---

# 21. Current Unity Systems — Keep

These existing systems remain aligned with the design.

## EnemyHealth

Physical enemy health remains valuable.

---

## EnemyShadow → IDamageable → EnemyHealth

This architecture fits the game's central combat rule extremely well.

The shadow receives the attack.

Damage transfers to the physical enemy.

Keep this.

---

## PlayerStealth / HideShadow Concept

The idea that large shadow areas can conceal the player remains valid.

The implementation may later evolve as the real shadow-surface system becomes more sophisticated.

---

## Weapon Shadow Separation

Weapon shadows and hiding shadows should remain different gameplay categories.

Keep this conceptual separation.

---

## OnDeath Event Architecture

The publisher/subscriber architecture remains useful and should continue.

---

# 22. Current Unity Systems — Reconsider

## Normal Player Vision Detection

The current system:

```text
enemy view angle
+
raycast toward player
→ detected
```

was designed when the protagonist was being treated like a conventional physical stealth character.

This should not remain the foundation of enemy perception.

Parts of the technology may still be reused for enemies looking at physical anomalies or visible shadow surfaces, but the design logic must change.

---

## Conventional Chase Logic

The meaning of "chasing the player" now needs reconsideration.

A physical enemy cannot necessarily run directly toward a shadow as though pursuing a physical human.

Enemy behavior may instead involve:

- approaching the affected surface
- repositioning lights
- investigating physical disturbances
- illuminating nearby surfaces
- controlling exits
- firing at visible shadow locations
- changing the environment

---

# 23. Next Design Problem

Before implementing more AI, define the Shadow Surface System.

This system must answer:

- What counts as a valid playable surface?
- How is the player's position represented on that surface?
- How does the player wrap around corners?
- How are disconnected surfaces linked?
- How does moving light alter playable regions?
- How are shadow paths detected?
- How does the player transition between wall, floor and object faces?
- How do ally-created paths integrate with the same system?

This is now the highest-priority technical design problem for Shadowed.

---

# 24. Core Identity Statement

Shadowed is not a conventional stealth game in which a character happens to look like a shadow.

The player **is the shadow**.

The physical environment creates the player's world.

Light reshapes that world.

The dead help the player move through it.

Enemies can be killed through their shadows.

The physical world gradually learns that something impossible is hunting it.

And the protagonist's growing power never completely removes the fear, guilt and panic that created him.