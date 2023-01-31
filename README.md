# Game Sheriff vs. Cowboy
 Información técnica basica del proyecto

# ScriptableObject
- Se crea un ScriptableObject funciona como una base datos interna para crear diferentes tipos de municiones que se implementan para cada tipo de arma, este script usa la interfaz de usuario de Unity para crear municiones, se puede usar desde el menú de opciones de Unity y directamente desde el scriptableObject


# Máquina de estados de la AI
- Se crea una máquina de estados que permite manipular la AI de una manera más rápida y eficiente desde el inspector, por ejemplo el juego solo tiene un enemigo, pero se podría tener más de un enemigo en el juego y cada enemigo puede realizar diferentes acciones como estar quieto y solo atacar al jugador si se acerca a una zona de visión establecida, ir directo a atacarlo y perseguirlo o solo perseguirlo, esto es solo un ejemplo de todo lo que podría realizar la AI de manera rápida mediante una máquina de estados, en el caso de Sheriff vs. Cowboy el jugador se enfrenta a un único enemigo el cual tiene el objetivo de buscar y asesinar al jugador.

# Scripts información general
- Se usó el patrón de diseño Singleton para permitir el manejo rápido y  eficiente de clases entre scripts y  la manipulación de datos, en este caso el controlador del jugador es un singleton y se usa en otras clases como por ejemplo la clase arma que no usa singleton, ya que es una clase única para cada arma, otro ejemplo es  la clase vida que de igual forma usa singleton, en este caso es usado por las clases hijas que son, la vida del jugador y la vida del enemigo, por tanto, la clase vida es padre de la vida de jugador y de la vida del enemigo esto es solo un ejemplo de como se usó el patrón de diseño mencionado anteriormente para el desarrollo de este mini proyecto versión Alpha.

# Otros Scripts Importantes y sus mecánica
- Script PoolSystem  este script nos permite realizar instancias de objetos que tienen un uso específico durante el desarrollo del juego, por ejemplo toda la interfaz UI está en un solo canvas , para evitar tenerla en uso durante todo el transcurso del juego se crea un objeto clonado que se usa en situaciones específicas del juego como por ejemplo cuando se pierde vida se muestra la cantidad de vida que se ha perdido, de igual forma con las balas, entre otras funciones más que tiene está interfaz, un mejor ejemplo es el uso de efectos como explosiones o lanzamiento de balas, queremos ahorrar recursos, así que para evitar estar creando y destruyendo objetos mejor instanciamos una cantidad específica de objetos que solo se activan y desactivan cuando se necesiten.

- El script Weapon es un script que nos permite asignar funcionalidades a un arma específica, por ejemplo la AI y el Jugador usan el mismo script, pero cambia su funcionamiento, la AI dispara constantemente al jugador mientras este dentro de un rango determinado, por el contrario, el arma del jugador es automática así que al dejar presionado el botón izquierdo del mouse dispara constantemente, este script es muy útil, ya que nos da una visión básica de como configurar un arma, en este caso para disparar balas se usa las fuerza del Rigidbody de la bala, la bala tiene asociado un script importante llamado Projectile este script es el que manipula la fuerza y el daño de la bala además del efecto que realiza la misma.

- Además del arma que usa el jugador y el enemigo, hay un objeto curioso en el juego que puede servir de arma letal para el enemigo o para el jugador, es al azar, ya que el jugador o el enemigo podrían usarlo por pura suerte, se trata de un prefab llamado plutonium este prefab es como un arma global que está activo durante el juego al dispararle una bala el script asociado al  prefab detecta que objeto colisiona con él y sí se está dentro del rango de la explosión se puede perder mucha vida, igual le puede suceder a la AI, por eso es un arma global que puede servirle al enemigo o al jugador, es cuestión de suerte, una vez explota el plutonium vuelve a aparecer en algún lugar del mapa en los próximos 10 segundos aproximadamente.


Cosas que se pueden hacer dentro del juego: También se puede recuperar vida, una vez se recoge el botiquín aparece en otra ubicación aleatoria dentro del mapa, algunos objetos tiene interacción propia como por ejemplo los molinos, se puede recoger  una llave que permite abrir puertas cerradas, cuando se entra a una casa al ser un juego top down desaparece el tejado para poder explorar dentro de la misma

# Esto es solo un resumen técnico de lo que hace este proyecto y algunas de sus funciones especificadas.




