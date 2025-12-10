# Sistema de Daño - Instrucciones de Configuración en Unity

He implementado un sistema de daño bidireccional entre el jugador y el esqueleto. Aquí está lo que necesitas configurar en Unity:

## Cambios Realizados en los Scripts

### 1. **LogicaBarraVida.cs**
- ✅ Añadido método `RecibirDaño(float cantidad)` para aplicar daño al jugador

### 2. **Enemigo1.cs**
- ✅ Añadidas variables de daño y vida
- ✅ Corregido `onTriggerEnter` a `OnTriggerEnter` (mayúscula)
- ✅ Añadido método `RecibirDaño(float cantidad)` para dañar al esqueleto
- ✅ Añadido método `DañarAlJugador()` para que el esqueleto haga daño
- ✅ Añadido método `Morir()` para destruir el esqueleto cuando vida = 0
- ✅ Inicialización de variables en Start()

### 3. **LogicaPlayer.cs**
- ✅ Añadidas variables para detectar enemigos en rango
- ✅ Añadidos métodos `OnTriggerEnter` y `OnTriggerExit` para detección de enemigos
- ✅ Añadido método `GolpearEnemigo()` que se llama durante el ataque

## Configuración Necesaria en Unity

### Para el Esqueleto (Enemigo1):

1. **Añadir Collider Trigger**
   - Selecciona el GameObject del esqueleto
   - En el Inspector, añade un **Box Collider** o **Capsule Collider**
   - Marca la opción **"Is Trigger"** ✓

2. **Asignar Tag "enemigo"**
   - En el Inspector, en la opción "Tag", selecciona o crea el tag **"enemigo"**
   - Asigna este tag al GameObject del esqueleto

3. **Configurar Daño del Enemigo** (en el script Enemigo1)
   - `Daño Al Jugador`: 10 (ajusta según quieras)
   - `Vida Max Enemigo`: 50 (ajusta según quieras)

### Para el Jugador:

1. **Añadir Collider Trigger**
   - Selecciona el GameObject del jugador
   - Añade un **Box Collider** o **Capsule Collider** para detectar colisiones
   - Marca **"Is Trigger"** ✓

2. **Asignar Tag "player"** (si aún no lo tiene)
   - Asegúrate de que el jugador tenga el tag "Player" o crea uno

3. **Configurar Daño del Jugador** (en el script LogicaPlayer)
   - `Daño Al Enemigo`: 10 (ajusta según quieras)

4. **Configurar Barra de Vida**
   - En **LogicaBarraVida**, establece `Vida Max`: 100 (o el valor que desees)

## Cómo Funciona el Sistema

### Ataque del Jugador:
1. Jugador presiona **Return** para atacar
2. Se activa la animación de golpeo
3. **Durante la animación**, debes llamar a `GolpearEnemigo()` (idealmente desde un evento de animación)
4. Si hay un enemigo en el trigger, recibe daño

### Ataque del Esqueleto:
1. Cuando `atacando = true` en el Enemigo1
2. Si la animación de ataque toca al jugador (colisión con trigger)
3. El esqueleto hace daño al jugador mediante `DañarAlJugador()`
4. **Importante**: Debes llamar a `DañarAlJugador()` desde un evento de animación en la animación de ataque del esqueleto

## Pasos Finales Recomendados

### 1. Configura los colliders como Triggers
### 2. Crea/Asigna los tags correctos
### 3. Configura los valores de daño en los scripts
### 4. **IMPORTANTE**: Añade eventos de animación

#### Opción A: Usando AnimationEventHandler (Recomendado)
1. Añade el script `AnimationEventHandler.cs` al GameObject del jugador
2. En el **Animator del Jugador**:
   - Abre la animación de ataque ("golpeo")
   - En el momento donde la espada golpea, haz clic en **"Add Event"**
   - Selecciona `AnimationEventHandler.OnAttackHit()`
3. En el **Animator del Esqueleto**:
   - Abre la animación de ataque
   - En el momento donde el esqueleto golpea, haz clic en **"Add Event"**
   - Selecciona `AnimationEventHandler.OnEnemyAttackHit()`

#### Opción B: Llamadas directas (Sin AnimationEventHandler)
1. En la animación de ataque del **Jugador**:
   - Añade un evento en el frame del golpe
   - Llama a `LogicaPlayer.GolpearEnemigo()`

2. En la animación de ataque del **Esqueleto**:
   - Añade un evento en el frame del golpe
   - Llama a `Enemigo1.DañarAlJugador()`

### Cómo añadir un Animation Event en Unity:
1. Abre el **Animator** (Tab que dice "Animator")
2. Selecciona la animación (ej: "Attack", "golpeo")
3. En la timeline de animación, posiciónate en el frame donde debe hacer daño
4. Haz clic en **"Add Event"** (pequeño botón rojo)
5. En el popup, selecciona el método a llamar
6. Guarda la escena

## Variables Configurables

**En Enemigo1.cs:**
- `daño_Al_Jugador`: Cuánto daño hace el esqueleto (default: 10)
- `vidaMaxEnemigo`: Vida máxima del esqueleto (default: 50)

**En LogicaPlayer.cs:**
- `daño_Al_Enemigo`: Cuánto daño hace el jugador (default: 10)

**En LogicaBarraVida.cs:**
- `vidaMax`: Vida máxima del jugador (configurable en Inspector)
- `imagenBarraVida`: Asigna la imagen de la barra de vida

## Debugging

Para ver si funciona, abre la **Consola** (Window > General > Console):
- "Esqueleto recibió daño" - cuando golpeas al esqueleto
- "Jugador recibió daño del esqueleto" - cuando el esqueleto te golpea
- "Esqueleto murió" - cuando la vida llega a 0
