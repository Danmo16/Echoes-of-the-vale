# Resumen del Sistema de Daño - Echoes of the Vale

## ✅ Implementación Completada

Se ha implementado un **sistema bidireccional de daño** que permite:
- El **jugador golpea al esqueleto** → Esqueleto recibe daño
- El **esqueleto golpea al jugador** → Jugador recibe daño

---

## 📝 Cambios Realizados

### 1. **LogicaBarraVida.cs** 
```csharp
public void RecibirDaño(float cantidad)
{
    vidaActual -= cantidad;
}
```
- Nuevo método para recibir daño
- Integrado con el sistema de barra de vida visual

### 2. **Enemigo1.cs**
Añadidos:
- `vidaJugador` - Referencia a la vida del jugador
- `dañoAlJugador` - Variable configurable (default: 10)
- `vidaMaxEnemigo` - Vida máxima (default: 50)
- `vidaActualEnemigo` - Vida actual del esqueleto
- Método `OnTriggerEnter()` - Detecta cuando es golpeado por el arma
- Método `RecibirDaño()` - Reduce la vida del esqueleto
- Método `DañarAlJugador()` - Aplica daño al jugador
- Método `Morir()` - Desactiva el esqueleto cuando muere
- Inicialización en `Start()` - Obtiene referencia del jugador

### 3. **LogicaPlayer.cs**
Añadidos:
- `dañoAlEnemigo` - Variable configurable (default: 10)
- `enemigoEnRango` - Detecta qué enemigo está en rango
- Método `OnTriggerEnter()` - Detecta cuando un enemigo entra en rango
- Método `OnTriggerExit()` - Detecta cuando un enemigo sale del rango
- Método `GolpearEnemigo()` - Causa daño al enemigo si está en rango

### 4. **AnimationEventHandler.cs** (Nuevo)
- Script auxiliar para facilitar eventos de animación
- Métodos `OnAttackHit()` y `OnEnemyAttackHit()` para llamar desde animaciones

---

## 🎮 Cómo Usar

### Flujo de Daño del Jugador al Esqueleto:
1. Jugador presiona **Return** (ataque)
2. Se reproduce la animación de golpeo
3. **En el frame del golpe**, se llama a `LogicaPlayer.GolpearEnemigo()`
4. Si hay un esqueleto en el trigger, recibe 10 de daño

### Flujo de Daño del Esqueleto al Jugador:
1. Esqueleto se acerca al jugador
2. Esqueleto entra en rango y comienza animación de ataque
3. **En el frame del golpe**, se llama a `Enemigo1.DañarAlJugador()`
4. Jugador recibe 10 de daño

---

## ⚙️ Configuración en Unity

### Tags Necesarios:
- `"arma"` - Para el arma del jugador (espada)
- `"enemigo"` - Para el esqueleto

### Colliders:
- **Jugador**: Box/Capsule Collider con "Is Trigger" ✓
- **Esqueleto**: Box/Capsule Collider con "Is Trigger" ✓
- **Arma (Espada)**: Box/Capsule Collider con "Is Trigger" ✓

### Valores Configurables:

**En LogicaPlayer.cs (Inspector)**:
- `Daño Al Enemigo`: 10 (cuánto daño hace el jugador)

**En Enemigo1.cs (Inspector)**:
- `Daño Al Jugador`: 10 (cuánto daño hace el esqueleto)
- `Vida Max Enemigo`: 50 (vida total del esqueleto)

**En LogicaBarraVida.cs (Inspector)**:
- `Vida Max`: 100 (vida total del jugador)

---

## 📌 Eventos de Animación (IMPORTANTE)

### Para que funcione el daño, DEBES añadir eventos en las animaciones:

**Animación de Ataque del Jugador:**
1. Abre el Animator y selecciona la animación "golpeo"
2. En el timeline, posiciónate en el frame donde la espada golpea
3. Haz clic en "Add Event"
4. Selecciona `AnimationEventHandler.OnAttackHit()` o `LogicaPlayer.GolpearEnemigo()`

**Animación de Ataque del Esqueleto:**
1. Abre el Animator y selecciona la animación de ataque del esqueleto
2. En el timeline, posiciónate en el frame donde el esqueleto golpea
3. Haz clic en "Add Event"
4. Selecciona `AnimationEventHandler.OnEnemyAttackHit()` o `Enemigo1.DañarAlJugador()`

---

## 🔍 Debugging

Abre la **Consola** (Window > General > Console) para ver:
- `"Esqueleto recibió daño"` - cuando golpeas al esqueleto
- `"Jugador recibió daño del esqueleto"` - cuando el esqueleto te golpea
- `"Esqueleto murió"` - cuando la vida llega a 0

---

## 📋 Checklist de Implementación

- [x] Scripts actualizados con métodos de daño
- [x] LogicaBarraVida puede recibir daño
- [x] Enemigo1 detecta golpes y recibe daño
- [x] LogicaPlayer detecta enemigos en rango
- [x] AnimationEventHandler disponible para eventos
- [ ] ⚠️ **FALTA**: Configurar tags en los GameObjects
- [ ] ⚠️ **FALTA**: Configurar colliders como Trigger
- [ ] ⚠️ **FALTA**: Añadir eventos de animación en el Animator

---

## 🐛 Posibles Problemas

**"El daño no se aplica"**
- Verifica que los colliders tengan "Is Trigger" activado
- Verifica que los tags estén correctamente asignados
- Verifica que haya un evento de animación llamando al método

**"El enemigo no detecta al jugador"**
- Asegúrate de que el jugador sea encontrado en el Start() (`GameObject.Find("Player")`)
- Verifica que el nombre del jugador sea exactamente "Player"

**"El jugador no recibe daño"**
- Verifica que `vidaJugador` esté correctamente asignado en el Enemigo1
- Verifica que la barra de vida esté asignada en LogicaBarraVida
- Verifica que haya un evento de animación en el ataque del esqueleto
