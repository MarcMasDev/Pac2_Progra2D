# Pac 2

Marc Mas Vidal
Vídeo: https://drive.google.com/file/d/1BKbgIKBwUv7yUDessFk351_2zLcGIXJj/view?usp=sharing

**Índex**

[TOCM]

[TOC]

Controls
=============
Fletxes per moure's. Fletxa a dalt per saltar. Z X C per dispars.

Features (PAC 3)
=============
#### Personatge amb tres animacions (Idle, Walk, Jump)
El player compta amb un Animator que gestiona tres estats diferenciats:
- Idle quan està quiet.
- Walk quan es mou horitzontalment.
- Jump quan està en l'aire.
Les transicions són dinàmiques segons l'estat del jugador. Es gestionen via PlayerMovement.

#### Sistema de partícules al joc
S'han introduït diversos sistemes de partícules:
- Explosió de projectils: quan impacten amb obstacles o enemics.
- Mort d'enemics: s'activen partícules en morir.
- Destrucció de blocs: genera pols/partícules visuals.
- Partícules ambientals: com boira o pols en moviment a l'escena principal.

#### Enemics amb intel·ligència artificial
S'ha afegit un nou tipus d'enemic volador (BatEnemyFSM) amb una FSM (Finite State Machine) simple però funcional:
- Freeze: queda aturat quan el jugador està lluny.
- Wander: vola de forma semialetòria, canviant direcció cada X segons.
- ChasePlayer: persegueix el jugador si entra a certa distància.
- AttackPlayer: fa una càrrega ràpida (dash) cap al jugador quan està molt a prop.

Això amplia considerablement la varietat de comportaments respecte als enemics de la Pràctica 2.

#### Físiques i col·lisions per projectils
Els projectils utilitzen física amb Rigidbody2D i es comporten de manera diferent segons el tipus:
- Standard: impacte normal que fa mal.
- Climb: projectil sòlid que permet al jugador saltar-hi a sobre i utilitzar-lo com a plataforma.
- Explosion: projectil que esclata provocant dany en àrea.

Cada tipus de projectil gestiona propietats de física i col·lisions pròpies.

#### Checkpoints
S'han afegit checkpoints. Al agafarlos i perdre, el jugador fa respawn allí. En cas de victòria, s'esborren els checkpoints.
Aquests estàn a la Script "PowerUps" i a la script "Inventory".

#### Ús de Tags i Layers
S'han utilitzat tant Tags com Layers:
- Tags: identificació ràpida d'objectes per danyar o interactuar (Enemy, Player, Ground, etc.).
- Layers: gestió de què pot danyar què, quins projectils col·lisionen amb quins elements, etc., especialment a DestroyOnCol i DamageDealer.

Features (PAC 2)
=============

#### Moviment horitzontal i salt de Mario
Utilitzant el component Rigidbody i els Inputs s'ha fet un jugador que es mou en ambdues direccions. Utilitzant el *sprite renderer*, es pot girar la imatge. A més, hi ha un sistema *custom* per manejar la fricció i evitar patinar, ja que els *physics materials* de Unity tenien masses implicacions sobre la física i la velocitat del moviment. També s'ha fet acceleració i s'ha limitat la velocitat a un màxim.

Moviment per física: `rb.velocity += new Vector2(moveSpeed * Time.deltaTime, 0f);`

Fricció: `vel.x = Mathf.Lerp(vel.x, 0, decelerateSpeed * Time.deltaTime);`

----
Pel salt s'han utilitzat físiques. Per comprovar que el jugador toca el terra al saltar s'ha afegit un transform fill del Player als peus, com a punt d'origen de comprovació si està o no col·lisionant amb un objecte des del qual pot saltar.

També està adaptat perquè salti al matar un enemic utilitzant el sistema de actions de Unity.

Hi ha alguna millora com el fet que es pot aguantar la tecla espai per saltar més (com al Mario original) o el jump buffer.

Salt:  `rb.velocity = new Vector2(rb.velocity.x, jumpForce);`

#### Scroll horitzontal i càmera que segueix Mario
*Scroll smooth* de la càmera. També hi ha un *offset* perquè es vegi centrat però al mateix temps enfocant a les direccions a les que volem que vagi el jugador (dreta i a dalt).

Offset: `Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);`

Moviment Smooth: `transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);`

#### Col·lisions amb l'entorn
El jugador té col·lisió. És una càpsula per evitar que es quedi encallat. Aquest col·lideix amb tot l'entorn, enemics... S'ha fet ús de col·lisions normals, triggers i de *Physics2D* per fer detecció d'objectes en el món.

#### Enemics Goombas sense moviment
- Tots els enemics són mòbils. Perquè es vegi la implementació, he afegit 1 goomba (cuc taronja) immòbil. Només cal desactivar el script *generic movement* i treure l'animació de moure del goomba. 

- Tots els objectes que infligeixen mal tenen el script **DamageDealer**. 
- Aquesta és genèrica i s'utilitza en tots els casos en què es vol fer mal a una altra entitat. 

- Per fer-ho resta vida al script **HealthSystem** que implementa una interfície per assegurar-se de sempre poder rebre mal. Es pot decidir a qui fa mal (mitjançant el sistema de *tags*) i es pot modificar l'àrea de mal. 
- La decisió per evitar errors i per poder tenir un sistema independent genèric que no requereixi d'altres components ha estat usar *physics 2D*, la forma és *box* perquè és el que anava millor a l'hora d'aplicar solucions al projecte. 


        //Aplica damage a tots els objectes propers
        private void DealDamage()
        {
            Collider2D[] colliders = GetEntitiesToDamage();
    
            for (int i = 0; i < colliders.Length; i++)
            {
                if (DamageToTag(colliders[i].transform))
                {
                    IDamageable damageable = colliders[i].GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(damage);
                    }
                }
            }
        }
    
        //Comprova els objectes propers sense dependre de colliders.
        private Collider2D[] GetEntitiesToDamage()
        {
            Vector2 size = damageSize;
            return Physics2D.OverlapBoxAll(damagePoint.position, size, 0f);
        }
    
    
#### Similitud de nivell
És calcat al d'aquesta web: https://supermarioplay.com/


#### Definir una bona estructura de dades per guardar la informació del joc.
Tota l'estructura de dades està pensada per ser escalable, genèrica i adaptable. 

1- Tots els blocs usen la mateixa script i es diferencien per enums. Es pot customitzar per a cadascun els drop rates d'objectes.

2- Tots els objectes que es mouen utilitzen la mateixa script ja que és un comportament compartit.

3- Tots els objectes que fan mal o que tenen vida comparteixen script. El que varia és l'aplicació d'aquesta en cada objecte i la seva assignació de variables.

4- Totes les currencies (coins i temps) es guarden en un mateix script estàtic. Ambdues utilitzen sistemes genèrics compartits diferenciats per enums.

5- Tota l'estructura ha seguit aquestes pautes i patrons. He intentat que sigui el més flexible i adaptable possible dins de les limitacions del projecte i requeriments específics.

#### Claredat i senzillesa en el codi
Tot el codi està comentat. Quasi tot el codi, especialment en scripts més llargs, s'ha dividit en Handlers i mètodes per a que s'entengui bé i sigui llegible. 
En molts casos s'ha preferit la llegibilitat per sobre de l'optimització de línies.

En afegir tantes funcionalitats s'ha fet més extens. Però s'ha intentat presentar una estructura clara i reduïda dins de les possibilitats del projecte.

#### Moviment horitzontal dels Goombas
S'ha fet una script genèrica per tots els moviments similars (powerups i enemics). Utilitzen un sistema de doble detecció (dreta i esquerra) i giren al detectar un obstacle. Per diferenciar obstacles d'altres enemics o del player s'ha afegit una variable *layer mask*.
Es troba al codi **GenericMovement**

El moviment funciona amb físiques (rigidbody).
Per imitar el funcionament del Mario (https://supermarioplay.com/) s'ha implementat la variable: **moveIfPlayerNear**. En cas que aquest comportament no sigui desitjat es pot desactivar al prefab de l'enemic o del power up.

#### Mort de Goombas en saltar a sobre
Utilitzant el mateix damage dealer, però assignant-li la tag enemy, i fent servir com a punt d’origen per detectar entitats col·lindants el transform isGrounded (que és un fill del player situat als seus peus), es pot aplicar dany a qualsevol entitat que tingui aquesta tag i implementi un sistema de salut (health system).

#### Blocs
En el script **Block** s'implementen tres tipus de blocs diferents mitjançant l'enum BlockType, cadascun amb un comportament propi quan el jugador els colpeja des de sota:
###### Special
Els blocs de tipus special estan pensats per donar un bonus (com un power-up o objecte) i després passar a ser used. Quan el jugador els colpeja es crida a HandleSpecialBlock(), que:
- Dóna el bonus una única vegada mitjançant la funció GiveBonus().

- Canvia el tipus de bloc a used, evitant que es torni a activar.

- Executa l'animació de bump (pujada i baixada visual del bloc) mitjançant una coroutine.

- Aquest comportament imita els clàssics blocs "?" que només es poden activar una vegada. Després seran blocks Used.

###### Solid
Els blocs de tipus solid estan pensats per donar un bonus (com un power-up o objecte). Quan el jugador els colpeja es crida a HandleSolidBlock(), que:

- Si el jugador té un power-up actiu (concretament, el mushroom), es poden destruir. Això es comprova amb la funció ShouldDestroy(), i si retorna true, es generen partícules (particles) i el bloc es destrueix amb Destroy().

- Si el jugador no té el power-up, el bloc no es trenca, però pot donar un bonus igualment una sola vegada, amb GiveBonus().

- També executen la coroutine de bump quan el jugador els toca. Això es fa sempre (al contrari que amb els used) per molt que no es doni bonus.

###### Used
Els blocs de tipus used no tenen cap efecte si el jugador els colpeja.

- No s'activa cap animació ni es dóna cap bonus.

- El canvi visual es fa amb un nou sprite (usedBlock), per indicar que ja han estat utilitzats. Aquest canvi es realitza dins la funció ChangeBlockToUsed().

###### End Blocks
Aquests són blocs que ofereixen un bonus una sola vegada però no tenen cap comportament especial (com el bump o el used).

#### Super Mushrooms
El comportament del super mushroom es gestiona des de l’script PowerUps, que ha d’estar en el jugador. Aquest script requereix els components HealthSystem i DamageDealer.

Quan el jugador col·lisiona amb un bolet (tag definida), si no s’ha aplicat encara:

- Fa créixer el jugador (transform.localScale),

- El cura (healthSystem.Heal()),

- Marca la variable mushroomApplied com a certa i reprodueix el so.

Quan rep mal, si tenia el power-up actiu:

- Torna a la mida inicial i reprodueix un so de pèrdua de power-up.

HealthSystem.OnDamaged es fa servir per detectar quan perdre l’efecte.

---
Les monedes són un exemple de com es podrien implementar altres power-ups dins aquest sistema. 
S’han inclòs com a extra per al nivell i fan servir Inventory.AddCoins() en col·lidir amb el jugador.

#### GUI amb comptadors de punts, de monedes i de temps
La interfície mostra tres valors en pantalla: monedes, punts i temps restant.

El script CurrencyUpdater s'encarrega de mostrar els valors de monedes i punts, llegint-los directament des de la classe estàtica Inventory, que permet mantenir aquestes dades entre escenes.

El temps es gestiona des del script TimeManager, que redueix el valor inicial amb el pas del temps i actualitza el text. Quan el temps s'acaba, es llança l'escena de Game Over.

Els punts s'afegeixen automàticament amb PointsManager, que escolta l'event de mort dels enemics i suma la puntuació corresponent.

Tant els coins com els points es poden donar de forma automatitzada al player gràcies als prefabs *AutoCollect...* i a la script **AutoCollect**

#### Animacions i efectes
Animacions: monedes, enemics, player (saltar i córrer --> des de script **PlayerMovement**).

Efectes: enviromental fog, mort dels enemics, destrucció d'un bloc, shiny effect per les monedes i els mushrooms (he fet el que he pogut visualment).

#### Música i efectes
Banda Sonora en loop.  
Efectes de so: salt del jugador, recollida de monedes, activació d'un power-up, pèrdua del power-up, impacte o dany rebut, destrucció d'un bloc, mort del jugador.

Tot es controla des de la script **AudioManager**.

#### Tilemaps
Tot el joc i el nivell es fa mitjançant tilemaps. Composite colliders, tilemap colliders... Tot el set-up del tilemap l'he fet jo a Unity (no era un tilemap d'algun asset pack ja preparat, només tenia la spritesheet en png).


#### Bibliografia

Els següents recursos externs han estat utilitzats per completar el projecte, respectant les llicències i citant els autors corresponents.

**Referències**

GameDev Market. (2022). *8Bit Music - 062022* [Asset musical]. Unity Asset Store. https://assetstore.unity.com/packages/audio/music/8bit-music-062022-225623

Garcia, A. (s. d.). *8-Bit Sfx* [Paquet d’efectes de so]. Unity Asset Store. https://assetstore.unity.com/packages/audio/sound-fx/8-bit-sfx-32831

Mocci, R. (s. d.). *Dogica* [Tipografia]. Dafont. https://www.dafont.com/dogica.font

v3x3d. (s. d.). *Paper-Pixels-8x8* [Asset gràfic]. Itch.io. https://v3x3d.itch.io/paper-pixels