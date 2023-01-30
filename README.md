
# Game Sheriff vs. Cowboy
# Información tecnica del juego

# ScriptableObject
# - Se crea un ScriptableObject funciona como una base datos interna para crear diferentes tipos de municiones que se implementan para cada tipo de arma, este script usa la interfaz de usuario de unity para crear municiones se puede usar desde el menu de opciones de unity y directamente desde el scriptableObject


# Maquina de estados de la AI
#  Se crea una maquina de estados que permite manipular la AI de una manera más rápida e efciente desde el inspector, por ejemplo el juego solo tiene un enemigo pero se podría tener más de un enemigo en el juego y cada enemigo puede realizar diferentes acciones como estar quieto y solo atacar al jugador si se acerca a una zona de visión establecida, ir directo a atacarlo y perseguirlo o solo perseguirlo esto es solo un ejemplo detodo lo que podría realizar la AI de manera rápida mediante una maquina de estados, en el caso de Sheriff vs. Cowboy el jugador se enfrenta a un unico enemigo el cual tiene el objetivo de buscar y asesinar al jugador

# Scripts información general
# Se uso el patron de diseño Singleton para permitir el manejo rápido y eficiente de clases entre scripts y la manipulación de datos, en este caso el controlador del jugador es un singleton y se usar en otras clases como por ejemplo la clase arma que no usa singleton ya que es una clase unica para cada arma, otro ejemplo es  la clase vida que de igual forma usa singleton, en este caso es usado por las clases hijas que son, la vida del jugador y la vida del enemigo, por tanto la clase vida es padre de la vida de jugador y de la vida del enemigo esto es solo un ejemplo de como se uso el patron de diseño mencionado anteriomente para el darrollo de este miniproyecto versión Alpha

# Otros Scripts Importantes y sus mecanica
# Script PoolSystem  este script nos permite realizar instancias de objetos que tienen un uso especifico durante el desarrollo del juego, por ejemplo toda la interfaz UI esta en un solo canvas , para evitar tenerla en uso durante todo el transcurso del juego se crea un un objeto clonado que se usa en situaciones especificas del juego como por ejemplo cuando se pierde vida se muestra la cantidad de vida que se ha perdido, de igual forma con las balas , entre otras funciones más que tiene el está interfaz, un mejor ejemplo es el uso de efectos como explosiones o lanzamiento de balas , queremos ahorrar recursos, así que para evitar estar creando y destruyendo objetos mejor intanciamos una cantidad especifica de objetos que solo se activan y desactivan cuando se usan

# El script weapon es un script que nos permite asigar funcionalidades a un arma especifica , por ejemplo la AI y el Jugador usan el mismo script pero cambia su funcionamiento la AI dispara constantemente al jugador mientras este dentro de un rango determinado, por el contrario el arma del jugador es automatica así que al dejar presionado el boton izquierdo del mause dispara constantemente, este script es muy util ya que nos da una visión basica de como usar un arma, en este caso para disparar balas se usa las fuerza del rigidbody de la bala, la bala tiene asociado un script importante llamdo Projectile este script es el que manipula la fuerza y el daño de la bala además del efecto que realiza la misma

# Además de la arma que usa el jugador y el enemigo hay un objeto curioso en el juego que puede servir de arma letal para el enemigo o para el jugador es al azar ya que el jugador o el enemigo podrían usarlo por pura suerte, se trata de un prefab llamado plutonium este prefab es como un arma global que está activo durante el juego 
al dispararle un bala y el plutonium detecta que objeto collisiona con él y sí estás dentro del rango de la explosión puedes perder mucha vida igual que la AI, por eso es un arma global que puede servirle al enemigo o al jugador, es cuestión de suerte, una vez explota el plutonium vuelve a aparecer en algún lugar del mapa en los proximos 10 segúndos más o menos

## Esto es solo un resumen de las cosas importantes tecnicas que fueron creadas para este juego.

# Cosas que se puedne hacer dentro del juego: También se puede recuperar vida, una vez se recoge el botiquin aparece en otra hubicación aleatoria dentro del mapa, algunos objetos tiene interacción propia como por ejemplo los molinos, se puede recoger  una llave que permite abrir puertas cerradas, cuando se entra a una casa
al ser un juego top down desaparece el tejado para poder explorar dentro de la misma

#Esto es solo un resumen tecnico de lo que hace este juego y algunas de sus funcionalidades tecnicas.




