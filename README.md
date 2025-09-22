//Versión de Unity 6000.0.47f1
//Como Jugar
El jugador deberá usar los controles nombrados en la pantalla, estos son sliders, para poder determinar Angulo, Fuerza y Position de Disparo del Cañon.
A su vez cuenta con dropdown para poder Seleccionar la Masa de la bala de Cañon.


//Objetivo
Construir un simulador balístico donde el jugador regula ángulo, fuerza y masa del proyectil para derribar objetivos conectados con Joints. El disparo y las colisiones deben estar gobernados por Rigidbody y el sistema de físicas de Unity. Registrar resultados de impacto para evaluar precisión y potencia.
//Requisitos mínimos
- Controles de disparo en pantalla:
Ángulo y fuerza con Slider o InputField.
Masa del proyectil seleccionable
- Disparo físico:
Proyectil con Rigidbody y Collider.
Lanzamiento por AddForce o velocity según el ángulo configurado.
- Escena de objetivos:
Estructuras armadas con Rigidbodies y Joints (FixedJoint, HingeJoint o SpringJoint).
Estabilidad inicial correcta. Si se cae sola, está mal configurada.
-Registro del resultado:
Guardar datos como tiempo de vuelo, punto de impacto, velocidad relativa, impulso de colisión y piezas derribadas.
Mostrar al final de cada intento: puntuación y un breve “reporte de tiro”.
//Al finalizar cada disparo, guardar en Firebase:
- Ángulo
- Fuerza
- Masa del proyectil
- Resultado del impacto (acierto o no, distancia)
- Cantidad de objetos afectados (opcional)
